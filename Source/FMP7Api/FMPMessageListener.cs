using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
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
	/// WindowsForms の NativeWindow の仕組みを利用しています。
	/// AssignHandle / ReleaseHandle で通知を受けるウィンドウに登録、解除を
	/// してください。
	/// 詳細は NativeWindow クラスのリファレンスを参照してください。
	/// </remarks>
	public class FMPMessageListener : NativeWindow
	{
		static uint _msg = 0;

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
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg == (int)_msg)
			{
				FMPMessageEvent(
					this,
					new FMPMessageEventArgs((FMPMessage)m.WParam.ToInt32()));
			}
		}

		public event EventHandler<FMPMessageEventArgs> FMPMessageEvent;
	}
}
