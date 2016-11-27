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
	public enum PMDPartMode : byte
	{
		None = 0x00,		// 未使用
		FM = 0x01,		// FM
		FMEx = 0x02,		// FM(C-EX)
		SSG = 0x03,		// SSG
		RhythmBD = 0x04,		// リズム(バスドラム)
		RhythmSD = 0x05,		// リズム(スネアドラム)
		RhythmCym = 0x06,		// リズム(シンバル)
		RhythmHat = 0x07,		// リズム(ハイハット)
		RhythmTom = 0x08,		// リズム(タム)
		RhythmRim = 0x09,		// リズム(リムショット)
		ADPCM = 0x0a,		// ADPCM
		PPZ8 = 0x0b,		// PCM(PPZ8)
		PPS = 0x0c,		// PCM(PPS)
	}


	/// <summary>
	/// PMD の拡張ワークを表す構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct PMDWork
	{
		public const int PartCountFM = 6;
		public const int PartCountSSG = 3;
		public const int PartCountADPCM = 1;
		public const int PartCountRhythm = 6;
		public const int PartCountFMEx = 3;
		public const int PartCountPPZ8 = 8;
		public const int PartCountPPS = 2;
		public const int PartCountAll =
							PartCountFM + PartCountSSG + PartCountADPCM +
							PartCountRhythm + PartCountFMEx +
							PartCountPPZ8 + PartCountPPS;

		public byte TimerB;						// TimerB値

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public PMDPartMode[] Mode;				// 音源詳細(※ EX_MODE参照)

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public byte[] Volume;					// 実音量値

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public byte[] Pan;						// パン
	}

	/// <summary>
	/// PMD の拡張ワークを表す構造体(アンセーフ)
	/// </summary>
	/// <remarks>
	/// ポインタが扱える言語(C#、C++)で使用が可能。
	/// unsafe (ネイティブ側)の構造体サイズを知るのにも使用(sizeof(PMDWorkPtr))
	/// </remarks>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	unsafe public struct PMDWorkPtr
	{
		public byte TimerB;											// TimerB値
		public fixed byte Mode[PMDWork.PartCountAll];		// 音源詳細(※ EX_MODE参照)
		public fixed byte Volume[PMDWork.PartCountAll];			// 実音量値
		public fixed byte Pan[PMDWork.PartCountAll];				// パン
	}
}
