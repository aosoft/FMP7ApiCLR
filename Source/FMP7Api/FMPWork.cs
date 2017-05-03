//
//	FMP7 API for .NET Copyright (c) 2010-2016 TAN-Y
//	FMP7 SDK          Copyright (c) 2010-2014 Guu
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace FMP.FMP7
{

	/// <summary>
	/// FMP ワーク管理クラス。
	/// Mutexと共有メモリ管理もする。
	/// </summary>
	public class FMPWork : IDisposable
	{
		private static string FMP32KeyMap = "FMP7_PUBLIC_WORK";
		private static string FMP32KeyMutex = "FMP7_PUBLIC_WORK_MUTEX";
		private static readonly int MaxChannelCount = FMPGlobalWork.MaxPart;

		private Mutex m_mutex = null;
		private IntPtr m_map;

		private IntPtr m_mappedMemory;
		private IntPtr m_globalWork;
		private IntPtr m_partWork;
		private IntPtr m_extendWork;
		private FMPWorkSize m_worksize;
		private int m_exworksize;

		public void Dispose()
		{
			Close();
		}

		/// <summary>
		/// 共有ワークを AddOn ドライバ未使用で開きます。
		/// </summary>
		public bool Open()
		{
			return Open(AddOn.DriverType.None);
		}

		public bool Open(int millisecondsTimeout)
		{
			return Open(AddOn.DriverType.None, millisecondsTimeout);
		}

		/// <summary>
		/// 指定の AddOn ドライバのワークを参照できるようにして共有ワークを開きます。
		/// </summary>
		/// <remarks>
		/// アプリで参照する可能性のある AddOn ドライバのフラグをセットしてください。
		/// 指定したドライバの中で一番ワークサイズの大きいものを基準に共有メモリの
		/// マッピングサイズを決定します。
		/// ここで指定したドライバ以外のものは GetExWork で取得することはできません。
		/// </remarks>
		/// <param name="supportExDrvs">サポートドライバ(複数)</param>
		public bool Open(AddOn.DriverType supportExDrvs)
		{
			return Open(supportExDrvs, -1);
		}

		public bool Open(AddOn.DriverType supportExDrvs, int millisecondsTimeout)
		{
			if (m_mappedMemory != IntPtr.Zero)
			{
				//	すでに開かれている
				return true;
			}

			if (millisecondsTimeout >= 0)
			{
				//	一回 mutex の取得を試みる。
				//	長時間ロックされていると以降の API コールで強制 Wait になるため。
				using (var mutex = Mutex.OpenExisting(FMP32KeyMutex, MutexRights.Synchronize))
				{
					if (mutex == null ||
						mutex.WaitOne(millisecondsTimeout) == false)
					{
						return false;
					}
					mutex.ReleaseMutex();
				}
			}

			FMPVersion ver = FMPControl.GetVersion();

			if (ver.Major < 7 ||
				(ver.Major == 7 && ver.Minor < 10) ||
				(ver.Major == 7 && ver.Minor == 10 && ver.MinorChar < 'a'))
			{
				//	FMP7のバージョン 7.10aを満たしていない
				throw new FMPException(FMPError.NotSupportedFMPVersion);
			}

			m_worksize = FMPControl.GetWorkSize();
			m_exworksize = GetAddOnWorkSize(supportExDrvs);

			m_map = Kernel32Wrapper.OpenFileMapping(
				FileMapAccess.FileMapRead, false, FMP32KeyMap);
			if (m_map == IntPtr.Zero)
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}

			try
			{
				m_mutex = Mutex.OpenExisting(FMP32KeyMutex, MutexRights.Synchronize);
				if (m_mutex == null)
				{
					return false;
				}
				if (millisecondsTimeout >= 0)
				{
					if (m_mutex.WaitOne(millisecondsTimeout) == false)
					{
						return false;
					}
				}
				else
				{
					m_mutex.WaitOne();
				}
				try
				{
					int partWorkSizeAll = m_worksize.PartSize * MaxChannelCount;
					m_mappedMemory = Kernel32Wrapper.MapViewOfFile(
						m_map, FileMapAccess.FileMapRead, 0, 0,
						(uint)(m_worksize.GlobalSize + partWorkSizeAll + m_exworksize));

					m_globalWork = m_mappedMemory;
					m_partWork = new IntPtr(m_globalWork.ToInt64() + m_worksize.GlobalSize);
					m_extendWork = new IntPtr(m_partWork.ToInt64() + partWorkSizeAll);
				}
				catch
				{
					m_mutex.ReleaseMutex();
					throw;
				}
			}
			catch
			{
				if (m_mutex != null)
				{
					m_mutex.Close();
					m_mutex = null;
				}

				Kernel32Wrapper.CloseHandle(m_map);
				m_map = IntPtr.Zero;
				throw;
			}

			return true;
		}

		unsafe private int GetAddOnWorkSize(AddOn.DriverType supportExDrvs)
		{

			int ret = 0;

			if ((supportExDrvs & AddOn.DriverType.FMP7) != 0 &&
				ret < sizeof(AddOn.FMP7Work))
			{
				ret = sizeof(AddOn.FMP7Work);
			}

			if ((supportExDrvs & AddOn.DriverType.FMP4) != 0 &&
				ret < sizeof(AddOn.FMP4WorkPtr))
			{
				ret = sizeof(AddOn.FMP4WorkPtr);
			}

			if ((supportExDrvs & AddOn.DriverType.PMD) != 0 &&
				ret < sizeof(AddOn.PMDWorkPtr))
			{
				ret = sizeof(AddOn.PMDWorkPtr);
			}

			if ((supportExDrvs & AddOn.DriverType.MXDRV) != 0 &&
				ret < sizeof(AddOn.MXDRVWorkPtr))
			{
				ret = sizeof(AddOn.MXDRVWorkPtr);
			}

			return ret;
		}

		/// <summary>
		/// 共有メモリを開放し、ワークを閉じます。
		/// </summary>
		public void Close()
		{
			if (m_mappedMemory != IntPtr.Zero)
			{
				Kernel32Wrapper.UnmapViewOfFile(m_mappedMemory);
				m_mappedMemory = IntPtr.Zero;
				m_globalWork = IntPtr.Zero;
				m_partWork = IntPtr.Zero;
				m_extendWork = IntPtr.Zero;

				m_worksize = new FMPWorkSize();
				m_exworksize = 0;
			}

			if (m_map != null)
			{
				Kernel32Wrapper.CloseHandle(m_map);
				m_map = IntPtr.Zero;
			}

			if (m_mutex != null)
			{
				m_mutex.ReleaseMutex();
				m_mutex.Close();
				m_mutex = null;
			}
		}

		/// <summary>
		/// 共有メモリ上のグローバルワークへのポインタを取得します。
		/// コピーではないのでアクセスには注意。
		/// </summary>
		unsafe public FMPGlobalWorkPtr* GlobalWorkPointer
		{
			get { return (FMPGlobalWorkPtr*)m_globalWork.ToPointer(); }
		}

		/// <summary>
		/// 共有メモリ上のパートワークへのポインタを取得します。
		/// コピーではないのでアクセスには注意。
		/// </summary>
		unsafe public FMPPartWork* PartWorkPointer
		{
			get { return (FMPPartWork*)m_partWork.ToPointer(); }
		}

		/// <summary>
		/// 共有メモリ上のAddOn用拡張ワークへのポインタを取得します。
		/// コピーではないのでアクセスには注意。
		/// </summary>
		public IntPtr ExWorkPointer
		{
			get { return m_extendWork; }
		}

		/// <summary>
		/// グローバルワークのコピー(スナップショット)を所得します。
		/// </summary>
		/// <returns></returns>
		unsafe public FMPGlobalWork GetGlobalWork()
		{
			bool open = false;
			if (m_globalWork == IntPtr.Zero)
			{
				Open();
				open = true;
			}

			try
			{
				return (FMPGlobalWork)Marshal.PtrToStructure(
					m_globalWork, typeof(FMPGlobalWork));
			}
			finally
			{
				if (open)
				{
					Close();
				}
			}
		}

		/// <summary>
		/// 指定のパート番号のパートワークのコピー(スナップショット)を取得します。
		/// </summary>
		/// <param name="part"></param>
		/// <returns></returns>
		unsafe public FMPPartWork GetPartWork(int part)
		{
			if (part < 0 || part >= MaxChannelCount)
			{
				throw new ArgumentException();
			}

			bool open = false;
			if (m_globalWork == IntPtr.Zero)
			{
				Open();
				open = true;
			}

			try
			{
				byte* p = (byte*)PartWorkPointer + m_worksize.PartSize * part;
				return *(FMPPartWork*)p;
			}
			finally
			{
				if (open)
				{
					Close();
				}
			}
		}

		/// <summary>
		/// 指定範囲のパートのパートワークをコピーします。
		/// </summary>
		/// <param name="startIndex"コピー>開始パート番号</param>
		/// <param name="destination">コピー先</param>
		/// <param name="destinationIndex">コピー先の開始インデックス</param>
		/// <param name="length">コピーするパート数</param>
		unsafe public void CopyPartWork(
			int startIndex,
			FMPPartWork[] destination,
			int destinationIndex,
			int length)
		{
			if (destination == null)
			{
				throw new ArgumentNullException();
			}

			int maxSourceLength = MaxChannelCount - startIndex;
			int maxDestLength = destination.Length - destinationIndex;
			int maxlength = Math.Min(maxSourceLength, maxDestLength);

			if (startIndex < 0 ||
				startIndex > maxSourceLength ||
				destinationIndex < 0 ||
				destinationIndex > maxDestLength ||
				length < 0 ||
				length > maxlength)
			{
				throw new ArgumentException();
			}

			bool open = false;
			if (m_globalWork == IntPtr.Zero)
			{
				Open();
				open = true;
			}

			try
			{
				var p = PartWorkPointer;
				for (int i = 0; i < length; i++)
				{
					byte* p2 = (byte*)PartWorkPointer + m_worksize.PartSize * (startIndex + i);
					destination[destinationIndex + i] = *(FMPPartWork*)p2;
				}
			}
			finally
			{
				if (open)
				{
					Close();
				}
			}
		}

		/// <summary>
		/// AddOnの拡張ワークを取得します。
		/// 動作中のドライバを判別し、そのドライバに対応したワークの構造体を返します。
		/// Open 時に指定したドライバに対応していないものだった場合は null が返ります。
		/// </summary>
		/// <returns></returns>
		unsafe public object GetExWork()
		{
			bool open = false;
			if (m_extendWork == IntPtr.Zero)
			{
				Open();
				open = true;
			}

			try
			{
				Guid driverId = GlobalWorkPointer->DriverId;
				if (driverId == AddOn.DriverID.FMP7)
				{
					if (m_exworksize >= sizeof(AddOn.FMP7Work))
					{
						return Marshal.PtrToStructure(m_extendWork, typeof(AddOn.FMP7Work));
					}
				}
				else if (driverId == AddOn.DriverID.FMP4)
				{
					if (m_exworksize >= sizeof(AddOn.FMP4WorkPtr))
					{
						return Marshal.PtrToStructure(m_extendWork, typeof(AddOn.FMP4Work));
					}
				}
				else if (driverId == AddOn.DriverID.PMD)
				{
					if (m_exworksize >= sizeof(AddOn.PMDWorkPtr))
					{
						return Marshal.PtrToStructure(m_extendWork, typeof(AddOn.PMDWork));
					}
				}
				else if (driverId == AddOn.DriverID.MXDRV)
				{
					if (m_exworksize >= sizeof(AddOn.MXDRVWorkPtr))
					{
						return Marshal.PtrToStructure(m_extendWork, typeof(AddOn.MXDRVWork));
					}
				}

				return null;
			}
			finally
			{
				if (open)
				{
					Close();
				}
			}
		}
	}
}
