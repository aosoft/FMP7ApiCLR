//
//	FMP7 API for .NET Copyright (c) 2010-2012 TAN-Y
//	FMP7 SDK          Copyright (c) 2010-2012 Guu
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FMP.FMP7
{
	[StructLayout(LayoutKind.Sequential)]
	struct COPYDATASTRUCT
	{
		public IntPtr dwData;
		public int cbData;
		public IntPtr lpData;
	}

	class User32Wrapper
	{
		private const string m_dllName = "user32.dll";

		[DllImport(m_dllName, EntryPoint = "FindWindowW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr FindWindow(string className, string windowName);

		[DllImport(m_dllName)]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		[DllImport(m_dllName, EntryPoint = "SendMessage")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, ref COPYDATASTRUCT lParam);

		[DllImport(m_dllName, EntryPoint = "RegisterWindowMessageW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint RegisterWindowMessage(string lpString);
	}
}
