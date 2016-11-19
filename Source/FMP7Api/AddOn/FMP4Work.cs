//
//	FMP7 API for .NET Copyright (c) 2010-2012 TAN-Y
//	FMP7 SDK          Copyright (c) 2010-2012 Guu
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
	public enum FMP4PartMode : byte
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
		PDZF = 0x0c,		// PCM(PDZF)
	}

	/// <summary>
	/// LFOモード種別
	/// </summary>
	[Flags]
	public enum FMP4LFOMode : ushort
	{
		None = 0,
		Vibrato0 = 0x0001,	// ビブラート#0(MP)
		Vibrato1 = 0x0002,	// ビブラート#1(MQ)
		Vibrato2 = 0x0004,	// ビブラート#2(MR)
		Tremolo = 0x0008,	// トレモロ
		Wow = 0x0010,	// ワウ
		Echo = 0x0020,	// エコー
		Hardware = 0x0040,	// ハードウェア
	}

	/// <summary>
	/// FMPv4 の拡張ワークを表す構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct FMP4Work
	{
		public const int PartCountFM = 6;
		public const int PartCountSSG = 3;
		public const int PartCountADPCM = 1;
		public const int PartCountRhythm = 6;
		public const int PartCountFMEx = 3;
		public const int PartCountAll =
							PartCountFM + PartCountSSG + PartCountADPCM +
							PartCountRhythm + PartCountFMEx;

		public byte TimerB;						// TimerB値

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public FMP4PartMode[] Mode;				// 音源詳細(※ EX_MODE参照)

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public byte[] ModeIndex;				// 音源インデックス

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public byte[] Volume;					// 実音量値

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public byte[] Pan;						// パン[0-3]/[1-9]

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = PartCountAll)]
		public FMP4LFOMode[] StatusLFO;			// 動作LFO(※ EX_LFO参照)
	}

	/// <summary>
	/// FMPv4 の拡張ワークを表す構造体(アンセーフ)
	/// </summary>
	/// <remarks>
	/// ポインタが扱える言語(C#、C++)で使用が可能。
	/// unsafe (ネイティブ側)の構造体サイズを知るのにも使用(sizeof(FMP4WorkPtr))
	/// </remarks>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	unsafe public struct FMP4WorkPtr
	{
		public byte TimerB;											// TimerB値
		public fixed byte Mode[FMP4Work.PartCountAll];		// 音源詳細(※ EX_MODE参照)
		public fixed byte ModeIndex[FMP4Work.PartCountAll];			// 音源インデックス
		public fixed byte Volume[FMP4Work.PartCountAll];			// 実音量値
		public fixed byte Pan[FMP4Work.PartCountAll];				// パン[0-3]/[1-9]
		public fixed ushort StatusLFO[FMP4Work.PartCountAll];	// 動作LFO(※ EX_LFO参照)
	}
}
