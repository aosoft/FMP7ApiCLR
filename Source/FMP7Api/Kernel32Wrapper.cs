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
	[Flags]
	enum FileMapAccess : uint
	{
		FileMapCopy = 0x0001,
		FileMapWrite = 0x0002,
		FileMapRead = 0x0004,
		FileMapAllAccess = 0x001f,
		fileMapExecute = 0x0020,
	}

	[Flags]
	enum MutexRights : uint
	{
		Delete = 0x00010000,
		ReadControl = 0x00020000,
		Synchronize = 0x00100000,
		WriteDAC = 0x00040000,
		WriteOwner = 0x00080000,
		MutexAllAccess = 0x1F0001,
		MutexModifyState = 0x0001,
	}

	enum WaitResult : uint
	{
		Object0 = 0x00000000,
		Abandoned = 0x00000080,
		Timeout = 0x00000102,
	}

	class Kernel32Wrapper
	{
		private const string m_dllName = "kernel32.dll";

		public const uint Infinite = 0xFFFFFFFF;

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

		[DllImport(m_dllName, EntryPoint = "OpenMutexW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenMutex(MutexRights desiredAccess, bool inheritHandle, string name);

		[DllImport(m_dllName, SetLastError = true)]
		public static extern bool ReleaseMutex(IntPtr mutex);

		[DllImport(m_dllName, SetLastError = true)]
		public static extern WaitResult WaitForSingleObject(IntPtr handle, uint milliseconds);
	}
}
