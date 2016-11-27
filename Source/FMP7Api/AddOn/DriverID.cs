//
//	FMP7 API for .NET Copyright (c) 2010-2016 TAN-Y
//	FMP7 SDK          Copyright (c) 2010-2014 Guu
//

using System;
using System.Collections.Generic;
using System.Text;

namespace FMP.FMP7.AddOn
{
	/// <summary>
	/// AddOn ドライバタイプ
	/// </summary>
	[Flags]
	public enum DriverType
	{
		None = 0,
		FMP7 = 0x0001,
		FMP4 = 0x0002,
		PMD = 0x0004,
		MXDRV = 0x0008
	}

	/// <summary>
	/// AddOn ドライバの GUID。FMPGlobalWork.DriverId にセットされる ID 値です。
	/// </summary>
	public class DriverID
	{
		public static Guid FMP7 = new Guid("{5C7D6EB5-4A5F-40ff-8906-D660B6D55E93}");
		public static Guid FMP4 = new Guid("{6979673D-CD39-4ca5-A3BA-CA164AC27E1A}");
		public static Guid PMD = new Guid("{E4463D69-5266-42c5-B0ED-82F46AC24F2B}");
		public static Guid MXDRV = new Guid("{2DA4ECF6-C4F4-452d-BD0F-0B201974C7F8}");
	}
}
