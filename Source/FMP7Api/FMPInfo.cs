using System;

namespace FMP.FMP7
{
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

		/// <summary>
		/// この SDK がサポートしている FMP のバージョンかどうか 
		/// </summary>
		public bool IsSupportedVersion
		{
			get
			{
				return
					Major > 7 ||
					(Major == 7 && Minor > 10) ||
					(Major == 7 && Minor == 10 && MinorChar >= 'a');
			}
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
	/// FMP の情報
	/// </summary>
	public class FMPInfo
	{
		private FMPVersion m_version;
		private FMPWorkSize m_workSize;

		public FMPInfo(FMPVersion version, FMPWorkSize workSize)
		{
			m_version = version;
			m_workSize = workSize;
		}

		public FMPVersion Version
		{
			get
			{
				return m_version;
			}
		}

		public FMPWorkSize WorkSize
		{
			get
			{
				return m_workSize;
			}
		}
	}
}
