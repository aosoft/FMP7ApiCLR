//
//	FMP7 API for .NET Copyright (c) 2010-2016 TAN-Y
//	FMP7 SDK          Copyright (c) 2010-2014 Guu
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FMP.FMP7
{
	/// <summary>
	/// FMP7 からのメッセージ種別
	/// </summary>
	public enum FMPMessage
	{
		StartFMP = 1,
		EndFMP = 2
	};

	/// <summary>
	/// FMP からのメッセージを表す EventArgs
	/// </summary>
	public class FMPMessageEventArgs : EventArgs
	{
		public FMPMessage Message
		{
			get;
			private set;
		}

		public FMPMessageEventArgs(FMPMessage msg)
		{
			Message = msg;
		}
	}

	/// <summary>
	/// FMP からの通知を管理するクラス。
	/// </summary>
	/// <remarks>
	/// AssignHandle / ReleaseHandle で通知を受けるウィンドウに登録、解除を
	/// してください。
	/// </remarks>
	public class FMPMessageListener
	{
		static private uint _msg = 0;
		private IntPtr _handle = IntPtr.Zero;
		private SubClassProc _proc = null;
		private IntPtr _uIdSubclass = IntPtr.Zero;

		public FMPMessageListener()
		{
			if (_msg == 0)
			{
				uint msg = User32Wrapper.RegisterWindowMessage("FMP7 MESSAGE KEY");
				if (msg == 0)
				{
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
				_msg = msg;
			}
			_proc = this.SubClassProc;
			_uIdSubclass = Marshal.GetFunctionPointerForDelegate(_proc);
		}

		public void AssignHandle(IntPtr handle)
		{
			ReleaseHandle();

			if (handle != IntPtr.Zero)
			{
				if (Comctl32Wrapper.SetWindowSubclass(
					handle, _proc, _uIdSubclass, IntPtr.Zero))
				{
					_handle = handle;
				}
			}
		}

		public void ReleaseHandle()
		{
			if (_handle != IntPtr.Zero)
			{
				Comctl32Wrapper.RemoveWindowSubclass(
					_handle, _proc, _uIdSubclass);
				_handle = IntPtr.Zero;
			}
		}

		private IntPtr SubClassProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr dwRefData)
		{
			if (msg == _msg)
			{
				var ev = FMPMessageEvent;
				if (ev != null)
				{
					ev(this, new FMPMessageEventArgs((FMPMessage)wParam.ToInt32()));
				}
			}

			return Comctl32Wrapper.DefSubclassProc(hWnd, msg, wParam, lParam);
		}

		public event EventHandler<FMPMessageEventArgs> FMPMessageEvent;
	}
}
