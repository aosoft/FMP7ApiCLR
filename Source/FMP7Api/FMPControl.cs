//
//	FMP7 API for .NET Copyright (c) 2010-2012 TAN-Y <tan-y@big.or.jp>
//	FMP7 SDK          Copyright (c) 2010-2012 Guu
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FMP.FMP7
{
	/// <summary>
	/// 曲データ読み込み時に行う処理
	/// </summary>
	public enum FMPMusicLoadAction
	{
		LoadOnly = 0x0000,			// 読み込みのみ
		LoadAndPlay = 0x0001,			// 読み込み後演奏
	}

	/// <summary>
	/// 曲データのテキストタイプ
	/// </summary>
	public enum FMPText
	{
		Title = 0x0000,			// タイトル
		Creator = 0x0001,			// データ制作者
		Composer = 0x0002,			// 作曲者
		FileName = 0x0010,			// 演奏ファイル名
		Comment = 0x0100,			// コメント
		DetailErrorMessage = 0x1000,			// エラーメッセージ
		ExtList = 0x2000,			// 対応拡張子リスト
	}

	/// <summary>
	/// マスク処理モード
	/// </summary>
	enum FMPMaskMode
	{
		SinglePart = 0x0000,
		AllPart = 0x0001
	}

	/// <summary>
	/// FMP7のエラーコード
	/// </summary>
	public enum FMPError
	{
		NotSupportedFMPVersion = -1,	//	FMPのバージョンを満たしていない

		NonError = 0,		// 正常終了
		FMPNotFound = 1,		// FMP7が見つからない
		Busy = 2,		// データ生成中(added 7.05c)
		OpenProc = 10,		// 呼び出し元プロセスアクセスエラー
		WriteMemory = 11,		// メモリ書き込みエラー
		ReadMemory = 12,		// メモリ読み込みエラー
		WorkSizeNoMatch = 13,		// ワークサイズが合わない
		WorkSizeShort = 14,		// 格納ワークサイズが小さい
		FileLoad = 15,		// ファイル読み込みエラー
		ParamRange = 19,		// パラメータ指定範囲エラー
		PartRange = 20,		// パート番号指定エラー
		SeekRange = 21,		// シーク指定範囲エラー
		CopySizeOver = 22,	// 最大ワークサイズオーバー(added 7.10a)
		DriverNotReady = 23,	//演奏ドライバが未起動(added 7.10a)
	}

	/// <summary>
	/// FMP7エラー例外クラス
	/// </summary>
	public class FMPException : Exception
	{
		private static Dictionary<FMPError, string> m_errMsgTable;

		static FMPException()
		{
			m_errMsgTable = new Dictionary<FMPError,string>();

			m_errMsgTable.Add(
				FMPError.NotSupportedFMPVersion, FMP7ApiResource.ErrMsg_NotSupportedFMPVersion);

			m_errMsgTable.Add(
				FMPError.NonError, FMP7ApiResource.ErrMsg_NonError);
			m_errMsgTable.Add(
				FMPError.FMPNotFound, FMP7ApiResource.ErrMsg_FMPNotFound);
			m_errMsgTable.Add(
				FMPError.Busy, FMP7ApiResource.ErrMsg_Busy);
			m_errMsgTable.Add(
				FMPError.OpenProc, FMP7ApiResource.ErrMsg_OpenProc);
			m_errMsgTable.Add(
				FMPError.WriteMemory, FMP7ApiResource.ErrMsg_WriteMemory);
			m_errMsgTable.Add(
				FMPError.ReadMemory, FMP7ApiResource.ErrMsg_ReadMemory);
			m_errMsgTable.Add(
				FMPError.WorkSizeNoMatch, FMP7ApiResource.ErrMsg_WorkSizeNoMatch);
			m_errMsgTable.Add(
				FMPError.WorkSizeShort, FMP7ApiResource.ErrMsg_WorkSizeShort);
			m_errMsgTable.Add(
				FMPError.FileLoad, FMP7ApiResource.ErrMsg_FileLoad);
			m_errMsgTable.Add(
				FMPError.ParamRange, FMP7ApiResource.ErrMsg_ParamRange);
			m_errMsgTable.Add(
				FMPError.PartRange, FMP7ApiResource.ErrMsg_PartRange);
			m_errMsgTable.Add(
				FMPError.SeekRange, FMP7ApiResource.ErrMsg_SeekRange);
			m_errMsgTable.Add(
				FMPError.CopySizeOver, FMP7ApiResource.ErrMsg_CopySizeOver);
			m_errMsgTable.Add(
				FMPError.DriverNotReady, FMP7ApiResource.ErrMsg_DriverNotReady);
		}

		private static string GetMessage(FMPError e)
		{
			try
			{
				if (e == FMPError.FileLoad)
				{
					return FMPControl.GetTextData(FMPText.DetailErrorMessage);
				}
			}
			catch
			{
			}

			if (m_errMsgTable.ContainsKey(e))
			{
				return m_errMsgTable[e];
			}

			return string.Format("Unknown FMPError ({0})", (int)e);
		}

		public FMPError Error
		{
			get;
			private set;
		}

		public FMPException(FMPError err) :
			base(GetMessage(err))
		{
			Error = err;
		}
	}

	/// <summary>
	/// ワークサイズ
	/// </summary>
	public struct FMPWorkSize
	{
		private int m_globalSize;
		private int m_partSize;

		public int GlobalSize
		{
			get { return m_globalSize; }
		}

		public int PartSize
		{
			get { return m_partSize; }
		}

		public FMPWorkSize(int global, int part)
		{
			m_globalSize = global;
			m_partSize = part;
		}
	}

	/// <summary>
	/// FMPバージョン情報
	/// </summary>
	public struct FMPVersion
	{
		private int m_system;
		private int m_major;
		private int m_minor;

		public int System
		{
			get { return m_system; }
		}

		public int Major
		{
			get { return m_major; }
		}

		public int Minor
		{
			get { return m_minor; }
		}

		public char MinorChar
		{
			get { return (char)m_minor; }
		}

		public FMPVersion(int system, int major, int minor)
		{
			m_system = system;
			m_major = major;
			m_minor = minor;
		}

		public override string ToString()
		{
			return string.Format("{0}.{1:00}{2}", System, Major, MinorChar);
		}
	}



	/// <summary>
	/// FMP制御クラス
	/// </summary>
	public class FMPControl
	{
		public const int MaxPath = 260;

		private static string FMPAppName = "FMP7";
		private const int WM_COPYDATA = 0x4a;
		private static int PID = System.Diagnostics.Process.GetCurrentProcess().Id;

		private enum FMPAPICode
		{
			MusicLoad = 0x0001,			// ファイル読み込み
			MusicPlay = 0x0002,			// 演奏開始
			MusicStop = 0x0003,			// 演奏停止
			MusicPause = 0x0004,			// 演奏一時中断
			MusicFade = 0x0005,			// フェードアウト
			MusicLoad2 = 0x0006,			// ファイル読み込み
			SetMask = 0x0010,			// パートマスク指定
			SetSeek = 0x0011,			// シーク位置指定
			GetVersion = 0x0100,			// ドライババージョン
			GetWorksize = 0x0101,			// 公開ワークサイズ取得
			GetDetail = 0x110,				// 詳細エラーコード取得(added 7.10a)
			GetText = 0x0200,			// 演奏ファイル情報取得
			GetTextLen = 0x0201,			// 演奏ファイル情報文字数
			CallExAPI = 0x1000,			// 拡張APIコール(added 7.10a)
		}

		[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
		private struct FMPAPIInfo
		{
			public int Size;
			public int Pid;
			public IntPtr Data;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
		unsafe private struct ExAPIInfo
		{
			public Guid guid;
			public uint func;
			public uint ex1;
			public uint ex2;
			public uint ex3size;
			public int ret;
			public fixed byte ex3[1];
		}


		/// <summary>
		/// API送信 (APIINFO省略型)
		/// </summary>
		/// <param name="api"></param>
		/// <param name="para"></param>
		private static void Send(FMPAPICode api, int para)
		{
			var cps = new COPYDATASTRUCT();
			cps.dwData = new IntPtr(((int)api & 0xffff) | (para & 0xffff) << 16);
			cps.cbData = 0;
			cps.lpData = IntPtr.Zero;

			Send(cps);
		}

		/// <summary>
		/// API送信 (APIINFO使用)
		/// </summary>
		/// <param name="api"></param>
		/// <param name="para"></param>
		/// <param name="data"></param>
		/// <param name="size"></param>
		unsafe private static void Send(FMPAPICode api, int para, IntPtr data, int size)
		{
			var fai = new FMPAPIInfo();

			fai.Pid = PID;
			fai.Data = data;
			fai.Size = size;

			var cps = new COPYDATASTRUCT();
			cps.dwData = new IntPtr(((int)api & 0xffff) | (para & 0xffff) << 16);
			cps.lpData = new IntPtr(&fai);
			cps.cbData = sizeof(FMPAPIInfo);
			Send(cps);
		}

		private static void Send(COPYDATASTRUCT cps)
		{
			var hwnd = User32Wrapper.FindWindow(FMPAppName, null);
			if (hwnd == IntPtr.Zero)
			{
				throw new FMPException(FMPError.FMPNotFound);
			}

			var r = (FMPError)User32Wrapper.SendMessage(hwnd, WM_COPYDATA, 0, ref cps);
			if (r != FMPError.NonError)
			{
				throw new FMPException(r);
			}
		}

		/// <summary>
		/// FMPが起動しているかどうか確認する。
		/// </summary>
		/// <returns>起動していればtrue</returns>
		public static bool CheckAvailableFMP()
		{
			return User32Wrapper.FindWindow(FMPAppName, null) != IntPtr.Zero;
		}

		/// <summary>
		/// FMPのバージョン情報を取得する。
		/// </summary>
		/// <returns>バージョン情報</returns>
		unsafe public static FMPVersion GetVersion()
		{
			int* pVersion = stackalloc int[3];

			Send(
				FMPAPICode.GetVersion, 0,
				new IntPtr(pVersion),
				sizeof(int) * 3);
			return new FMPVersion(pVersion[0], pVersion[1], pVersion[2]);
		}

		/// <summary>
		/// FMPのワークサイズを取得する。
		/// 基本的にアプリから使用することはありません。
		/// アプリはFMPWorkクラスを使用してください。
		/// </summary>
		/// <seealso cref="FMPWork"/>
		/// <returns>ワークサイズ</returns>
		unsafe public static FMPWorkSize GetWorkSize()
		{
			int* pSize = stackalloc int[2];

			Send(
				FMPAPICode.GetWorksize, 0,
				new IntPtr(pSize),
				sizeof(int) * 2);
			return new FMPWorkSize(pSize[0], pSize[1]);
		}

		/// <summary>
		/// 曲データをロードする。
		/// </summary>
		/// <param name="fileName">曲データファイル名</param>
		/// <param name="loadAction">ロード成功時に行う処理</param>
		unsafe public static void MusicLoad(string fileName, FMPMusicLoadAction loadAction)
		{
			if (fileName == null)
			{
				throw new NullReferenceException();
			}

			var fullpath = System.IO.Path.GetFullPath(fileName);

			fixed (char* pFileName = &fullpath.ToCharArray()[0])
			{
				Send(
					FMPAPICode.MusicLoad, (int)loadAction,
					new IntPtr(pFileName),
					sizeof(char) * (fullpath.Length + 1));
			}
		}

		/// <summary>
		/// 曲データをロードする。ロードのみ。
		/// </summary>
		/// <param name="fileName">曲データファイル名</param>
		public static void MusicLoad(string fileName)
		{
			MusicLoad(fileName, FMPMusicLoadAction.LoadOnly);
		}

		/// <summary>
		/// 曲データをロードして。成功したら再生も開始する。
		/// </summary>
		/// <param name="fileName">曲データファイル名</param>
		public static void MusicLoadAndPlay(string fileName)
		{
			MusicLoad(fileName, FMPMusicLoadAction.LoadAndPlay);
		}

		/// <summary>
		/// 曲データをロードする。(binary渡し版)
		/// </summary>
		/// <param name="fileName">曲データファイル名</param>
		/// <param name="loadAction">ロード成功時に行う処理</param>
		unsafe public static void MusicLoad(
			byte[] data, int startIndex, int length, FMPMusicLoadAction loadAction)
		{
			if (data == null)
			{
				throw new NullReferenceException();
			}

			if (startIndex < 0 || startIndex >= length ||
				length < 0 ||
				data.Length < (startIndex + length))
			{
				throw new ArgumentException();
			}


			fixed (byte* pData = &data[startIndex])
			{
				Send(
					FMPAPICode.MusicLoad, (int)loadAction,
					new IntPtr(pData),
					sizeof(byte) * (length));
			}
		}

		/// <summary>
		/// 曲データをロードする。ロードのみ。(binary版)
		/// </summary>
		public static void MusicLoad(byte[] data, int startIndex, int length)
		{
			MusicLoad(data, startIndex, length, FMPMusicLoadAction.LoadOnly);
		}

		/// <summary>
		/// 曲データをロードして。成功したら再生も開始する。(binary版)
		/// </summary>
		public static void MusicLoadAndPlay(byte[] data, int startIndex, int length)
		{
			MusicLoad(data, startIndex, length, FMPMusicLoadAction.LoadAndPlay);
		}

		/// <summary>
		/// 先頭から演奏を開始する。
		/// </summary>
		public static void MusicPlay()
		{
			Send(FMPAPICode.MusicPlay, 0);
		}

		/// <summary>
		/// 演奏を停止する。
		/// </summary>
		public static void MusicStop()
		{
			Send(FMPAPICode.MusicStop, 0);
		}

		/// <summary>
		/// 演奏を一時停止／再開する。
		/// </summary>
		public static void MusicPause()
		{
			Send(FMPAPICode.MusicPause, 0);
		}

		/// <summary>
		/// 演奏をフェードアウトする。
		/// </summary>
		/// <param name="speed">速度(0-255)</param>
		public static void MusicFadeOut(int speed)
		{
			Send(FMPAPICode.MusicFade, speed);
		}

		/// <summary>
		/// マスクを設定する。(指定パート)
		/// </summary>
		/// <remarks>
		/// マスク状態はワークから取得してください。
		/// </remarks>
		/// <param name="part">パート番号</param>
		/// <param name="mask">マスクするならtrue、解除はfalse</param>
		unsafe public static void SetMask(int part, bool mask)
		{
			uint* pMaskInfo = stackalloc uint[2];
			pMaskInfo[0] = (uint)part;
			pMaskInfo[1] = (uint)(mask ? 1 : 0);

			Send(FMPAPICode.SetMask, (int)FMPMaskMode.SinglePart,
				new IntPtr(pMaskInfo), sizeof(uint) * 2);
		}

		/// <summary>
		/// マスクを設定する。(全パート)
		/// </summary>
		/// <remarks>
		/// マスク状態はワークから取得してください。
		/// </remarks>
		/// <param name="mask">
		/// byte配列で指定するマスク状態。
		/// 配列長は128であること。後半64個は現時点では効果はないが指定としては必要。
		/// 各要素、1でマスク設定、0でマスク解除。
		/// </param>
		unsafe public static void SetMask(byte[] mask)
		{
			if (mask.Length != FMPGlobalWork.MaxPart)
			{
				throw new ArgumentException();
			}

			fixed (byte* pMask = &mask[0])
			{
				Send(FMPAPICode.SetMask, (int)FMPMaskMode.AllPart,
					new IntPtr(pMask), sizeof(byte) * mask.Length);
			}
		}

		/// <summary>
		/// シークする
		/// </summary>
		/// <param name="seek">
		/// シーク位置。クロック単位。
		/// 演奏中の曲のクロック数はワークから取得してください。
		/// </param>
		unsafe public static void SetSeek(uint seek)
		{
			Send(
				FMPAPICode.SetSeek, 0,
				new IntPtr(&seek), sizeof(uint));
		}

		/// <summary>
		/// テキストデータを取得する。
		/// </summary>
		/// <param name="texttype">取得するテキストデータの種別</param>
		/// <returns>文字列</returns>
		unsafe public static string GetTextData(FMPText texttype)
		{
			int len;
			Send(FMPAPICode.GetTextLen, (int)texttype,
				new IntPtr(&len), sizeof(int));
			if (len < 1)
			{
				return null;
			}

			len++;
			char* temp = stackalloc char[len];

			Send(FMPAPICode.GetText, (int)texttype,
				new IntPtr(temp), sizeof(char) * (len));

			return new string(temp);
		}

		public static MusicFileExtInfo GetMusicFileExtInfo()
		{
			return new MusicFileExtInfo(FMPControl.GetTextData(FMPText.ExtList));
		}

		/// <summary>
		/// AddOn 向け拡張 API コール。
		/// </summary>
		/// <remarks>
		/// 通常は使用しません。AddOn 用制御クラスを利用してください。
		/// </remarks>
		/// <param name="driverId">ドライバID</param>
		/// <param name="func">API番号</param>
		/// <param name="ex1">パラメータ1</param>
		/// <param name="ex2">パラメータ2</param>
		/// <param name="ex3">パラメータ3</param>
		/// <param name="startIndex">パラメータ3の開始インデックス</param>
		/// <param name="length">パラメータ3の長さ</param>
		/// <returns></returns>
		unsafe public static int CallExAPI(
			Guid driverId,
			uint func,
			uint ex1,
			uint ex2,
			byte[] ex3,
			int startIndex, int length)
		{

			int ex3Length = 0;
			if (ex3 != null)
			{
				if (startIndex < 0 || (startIndex + length) > ex3.Length)
				{
					throw new ArgumentOutOfRangeException();
				}

				ex3Length = length;
			}
			int dataLength = sizeof(ExAPIInfo) - 1 + ex3Length;
			byte* temp = stackalloc byte[dataLength];
			ExAPIInfo* pInfo = (ExAPIInfo*)temp;

			pInfo->guid = driverId;
			pInfo->func = func;
			pInfo->ex1 = ex1;
			pInfo->ex2 = ex2;
			pInfo->ex3size = (uint)ex3Length;
			if (ex3Length > 0)
			{
				Marshal.Copy(ex3, startIndex, new IntPtr(&pInfo->ex3[0]), ex3Length);
			}

			Send(FMPAPICode.CallExAPI, 0, new IntPtr(temp), dataLength);

			if (ex3Length > 0)
			{
				Marshal.Copy(new IntPtr(&pInfo->ex3[0]), ex3, startIndex, ex3Length);
			}

			return pInfo->ret;
		}


		internal static int GetUnicodeStringLengthPerByte(byte[] src)
		{
			for (int i = 0; i < src.Length; i += sizeof(char))
			{
				int c = (int)src[i] | ((int)src[i + 1] << 8);
				if (c == 0)
				{
					return i;
				}
			}

			return src.Length;
		}

		internal static string GetStringFromUnicodeByteArray(byte[] src)
		{
			int length = GetUnicodeStringLengthPerByte(src);

			if (length < 1)
			{
				return null;
			}
			return Encoding.Unicode.GetString(src, 0, length);
		}

		internal static string GetStringFromSJISByteArray(byte[] src)
		{
			int length = Array.IndexOf<byte>(src, 0);

			if (length < 0)
			{
				return Encoding.GetEncoding(932).GetString(src);
			}
			if (length < 1)
			{
				return null;
			}
			return Encoding.GetEncoding(932).GetString(src, 0, length);
		}
	}
}
