//
//	FMP7 API for .NET Copyright (c) 2010-2012 TAN-Y <tan-y@big.or.jp>
//	FMP7 SDK          Copyright (c) 2010-2012 Guu
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FMP.FMP7.AddOn
{
	/// <summary>
	/// FMP7 の拡張ワークを表す構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct FMP7Work
	{
		public const int PartCountFM = 32;
		public const int PartCountSSG = 32;
		public const int PartCountPCM = 32;
		public const int PartCountAll = 64;
	}
}
