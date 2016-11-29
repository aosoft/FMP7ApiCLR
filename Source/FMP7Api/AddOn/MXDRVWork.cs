//
//	FMP7 API for .NET Copyright (c) 2010-2016 TAN-Y
//	FMP7 SDK          Copyright (c) 2010-2014 Guu
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FMP.FMP7.AddOn
{
	/// <summary>
	/// パートモード種別
	/// </summary>
	public enum MXDRVPartMode : byte
	{
		None = 0x00,		// 未使用
		FM = 0x01,			// FM
		ADPCM = 0x02,		// FMADPCM
	}

	/// <summary>
	/// MXDRV の拡張ワークを表す構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public class MXDRVWork
	{
		public const int PartCountFM = 8;
		public const int PartCountADPCM = 8;
		public const int PartCountAll = PartCountFM + PartCountADPCM;

		public byte TimerB;						// TimerB値

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public MXDRVPartMode[] Mode;				// 音源詳細(※ EX_MODE参照)

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public byte[] Volume;					// 実音量値

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public byte[] Pan;						// パン
	}

	/// <summary>
	/// MXDRV の拡張ワークを表す構造体(アンセーフ)
	/// </summary>
	/// <remarks>
	/// ポインタが扱える言語(C#、C++)で使用が可能。
	/// unsafe (ネイティブ側)の構造体サイズを知るのにも使用(sizeof(MXDRVWorkPtr))
	/// </remarks>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	unsafe public struct MXDRVWorkPtr
	{
		public byte TimerB;											// TimerB値
		public fixed byte Mode[MXDRVWork.PartCountAll];		// 音源詳細(※ EX_MODE参照)
		public fixed byte Volume[MXDRVWork.PartCountAll];			// 実音量値
		public fixed byte Pan[MXDRVWork.PartCountAll];				// パン
	}
}
