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
	[Flags]
	enum FileMapAccess : uint
	{
		FileMapCopy = 0x0001,
		FileMapWrite = 0x0002,
		FileMapRead = 0x0004,
		FileMapAllAccess = 0x001f,
		fileMapExecute = 0x0020,
	}

	class Kernel32Wrapper
	{
		private const string m_dllName = "kernel32.dll";

		[DllImport(m_dllName, EntryPoint = "OpenFileMappingW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenFileMapping(FileMapAccess desiredAccess, bool inheritHandle, string name);

		[DllImport(m_dllName, SetLastError = true)]
		public static extern IntPtr MapViewOfFile(
			IntPtr fileMappingObject,
			FileMapAccess desiredAccess,
			uint fileOffsetHigh,
			uint fileOffsetLow,
			uint numberOfBytesToMap);

		[DllImport(m_dllName, SetLastError = true)]
		public static extern bool UnmapViewOfFile(IntPtr baseAddress);

		[DllImport(m_dllName, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr handle);
	}
}
