using System;

namespace FMP.FMP7
{
	class Mutex : IDisposable
	{
		private IntPtr m_handle = IntPtr.Zero;


		private Mutex(IntPtr handle)
		{
			m_handle = handle;
		}

		public void Dispose()
		{
			Close();
		}

		public void Close()
		{
			if (m_handle != IntPtr.Zero)
			{
				Kernel32Wrapper.CloseHandle(m_handle);
				m_handle = IntPtr.Zero;
			}
		}

		public void ReleaseMutex()
		{
			if (m_handle == null)
			{
				return;
			}
			Kernel32Wrapper.ReleaseMutex(m_handle);
		}

		public bool WaitOne()
		{
			if (m_handle == null)
			{
				return false;
			}
			return Kernel32Wrapper.WaitForSingleObject(m_handle, Kernel32Wrapper.Infinite) != WaitResult.Timeout;
		}

		public bool WaitOne(int millisecondsTimeout)
		{
			if (m_handle == null)
			{
				return false;
			}
			return Kernel32Wrapper.WaitForSingleObject(m_handle, (uint)millisecondsTimeout) != WaitResult.Timeout;
		}

		public static Mutex OpenExisting(string name, MutexRights rights)
		{
			var handle = Kernel32Wrapper.OpenMutex(rights, false, name);
			if (handle == IntPtr.Zero)
			{
				return null;
			}
			try
			{
				return new Mutex(handle);
			}
			catch
			{
				Kernel32Wrapper.CloseHandle(handle);
				throw;
			}
		}
	}
}
