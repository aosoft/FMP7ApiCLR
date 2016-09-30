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
	/// <summary>
	/// FMPのステータス
	/// </summary>
	[Flags]
	public enum FMPStat : uint
	{
		None = 0,
		Play = 0x00000001,	// 1:演奏中
		Pause = 0x00000002,	// 1:一時停止中
		Fade = 0x00000010,	// 1:フェード処理中
		Loop = 0x00010000,	// 1:ループした
		Mask = 0x00020000,	// 1:マスクされているパートあり
	}

	/// <summary>
	/// 音源タイプ
	/// </summary>
	public enum FMPSoundUnit : byte
	{
		None = 0,
		FM = 1,
		SSG = 2,
		PCM = 3
	}

	/// <summary>
	/// 音源デバイスタイプ
	/// </summary>
	public enum FMPSoundDeviceUnit : byte
	{
		None = 0x00,	// 未使用
		OPNA = 0x01,	// OPNA音源
		OPM = 0x02,	// OPM音源
		SSG = 0x10,	// SSG音源
		PCM = 0x20,	// PCM音源
		ADPCM = 0x21,	// ADPCM音源
		Rhythm = 0x30,	// リズム音源
	}

	/// <summary>
	/// FMP7 のグローバルワークを表す構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct FMPGlobalWork
	{
		/// <summary>
		/// ワークで確保するチャンネル数。
		/// 演奏でサポートしているものより値は大きい。
		/// </summary>
		public const int MaxPart = 128;
		public const int WorkChannelCount = MaxPart;

		public FMPStat Status;				// 状態(※ステータスビット情報参照)
		public uint PlayTime;				// 演奏継続時間
		public uint AverageCalorie;		// カロリー値（平均）
		public uint InstantCalorie;		// カロリー値（瞬間）
		public uint Count;				// 曲全体の長さ
		public uint CountNow;				// 現在の演奏位置
		public uint Tempo;				// テンポ
		public uint Loop;					// ループ数

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxPart)]
		public FMPSoundUnit[] Mode;				// パートモード (※パートモード種別定義参照)

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxPart)]
		public byte[] PartNo;			// パート番号(1-32)

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxPart)]
		public byte[] Mask;				// パートマスク状態

		public short LevelL;				// 再生レベル（左）
		public short LevelR;				// 再生レベル（右）
		public uint Clock;				// 全音符クロック数
		public uint StartCounter;		// 演奏開始ごとに＋１される(added 7.05c)
		public Guid DriverId;			// 動作音源GUID(added 7.10a)
		public uint DriverVer;			// 動作音源ドライババージョン(added 7.10a)

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxPart)]
		public FMPSoundDeviceUnit[] Device;				// 音源種別（※音源種別定義参照(added 7.10a)
	}

	/// <summary>
	/// FMP7 のグローバルワークを表す構造体(アンセーフ)
	/// </summary>
	/// <remarks>
	/// ポインタが扱える言語(C#、C++)で使用が可能。
	/// </remarks>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	unsafe public struct FMPGlobalWorkPtr
	{
		public FMPStat Status;				// 状態(※ステータスビット情報参照)
		public uint PlayTime;				// 演奏継続時間
		public uint AverageCalorie;		// カロリー値（平均）
		public uint InstantCalorie;		// カロリー値（瞬間）
		public uint Count;				// 曲全体の長さ
		public uint CountNow;				// 現在の演奏位置
		public uint Tempo;				// テンポ
		public uint Loop;					// ループ数
		public fixed byte Mode[FMPGlobalWork.MaxPart];				// パートモード (※パートモード種別定義参照)
		public fixed byte PartNo[FMPGlobalWork.MaxPart];			// パート番号(1-32)
		public fixed byte Mask[FMPGlobalWork.MaxPart];				// パートマスク状態
		public short LevelL;				// 再生レベル（左）
		public short LevelR;				// 再生レベル（右）
		public uint Clock;				// 全音符クロック数
		public uint StartCounter;		// 演奏開始ごとに＋１される(added 7.05c)
		public Guid DriverId;			// 動作音源GUID(added 7.10a)
		public uint DriverVer;			// 動作音源ドライババージョン(added 7.10a)
		public fixed byte DriverDevice[FMPGlobalWork.MaxPart];		// 音源種別（※音源種別定義参照(added 7.10a)
	}

}
