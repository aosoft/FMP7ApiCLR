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
	delegate IntPtr SubClassProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr dwRefData);

	class Comctl32Wrapper
	{
		private const string m_dllName = "Comctl32.dll";

		[DllImport(m_dllName, EntryPoint = "SetWindowSubclass")]
		public static extern bool SetWindowSubclass(IntPtr hWnd, SubClassProc pfnSubclass, IntPtr uIdSubclass, IntPtr dwRefData);

		[DllImport(m_dllName, EntryPoint = "DefSubclassProc")]
		public static extern IntPtr DefSubclassProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport(m_dllName, EntryPoint = "RemoveWindowSubclass")]
		public static extern bool RemoveWindowSubclass(IntPtr hWnd, SubClassProc pfnSubclass, IntPtr uIdSubclass);
	}
}
