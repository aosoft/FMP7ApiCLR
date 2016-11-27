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
	/// <summary>
	/// パート状態
	/// </summary>
	[Flags]
	public enum FMPPartStat : uint
	{
		None = 0,
		Play = 0x0001,	// 1:演奏チャンネル
		KeyOn = 0x0004,	// 1:キーオン継続中 0:キーオフ
		Tai = 0x0008,	// 1:タイ動作中
		SoftEnv = 0x0010,	// 1:SOFT ENV動作中
		HardEnv = 0x0020,	// 1:SSG HARD ENV動作中
		Tone = 0x0040,	// 1:SSG トーン発音 (added 7.06a)
		Noise = 0x0080,	// 1:SSG ノイズ発音 (added 7.06a)
		LFO0 = 0x0100,	// 1:LFO-0 on
		LFO1 = 0x0200,	// 1:LFO-1 on
		LFO2 = 0x0400,	// 1:LFO-2 on
		LFO3 = 0x0800,	// 1:LFO-3 on
		PitchBend = 0x1000,	// 1:ピッチベンド作動中
		ToneBend = 0x2000,	// 1:トーンベンド作動中 (added 7.05c)
	}

	/// <summary>
	/// LFOの種別定義。
	/// C APIでいうところのFMP32_LFO_H系。
	/// </summary>
	/// <remarks>
	/// 7.10a でビットフラグに変更
	/// </remarks>
	[Flags]
	public enum FMPLFOType : ushort
	{
		None = 0,
		Vibrato = 0x0010,		// ビブラート
		Tremolo = 0x0020,		// トレモロ
		Wow = 0x0040,			// ワウ
		Pan = 0x0080,			// オートパン
		Noise = 0x0100,		// ノイズ (added 7.06a)
		BuiltIn = 0x08000		// ハードウェア
	}

	/// <summary>
	/// LFOの波形定義。
	/// C APIでいうところのFMP32_LFO_L系。
	/// </summary>
	public enum FMPLFOWaveform : ushort
	{
		Triangle = 0,	// 三角波
		Saw = 1,	// のこぎり波
		Square = 2,	// 矩形波
		OneShot = 3,	// ワンショット
		Random = 4	// ランダム
	}

	/// <summary>
	/// FMP7 のパートワークを表す構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct FMPPartWork
	{
		public const int MaxChannelCount = FMPGlobalWork.MaxPart;

		public FMPPartStat State;			// 状態フラグ（※bit定義参照）

		public byte dmy00;			// 旧LFO種別定義(modified 7.10a)
		public byte dmy01;			// 旧LFO種別定義(modified 7.10a)
		public byte dmy02;			// 旧LFO種別定義(modified 7.10a)
		public byte dmy03;			// 旧LFO種別定義(modified 7.10a)

		public short Freq;			// 現在の発音論理周波数 0 ～ 6912 (半音64c * 108音階)
								// (freq - 64) 
		public short Detune;		// デチューン値 -6143 ～ 0 ～ +6143

		public ushort Hfreq;			// HARD ENV freq(SSG以外は常に0)
		public ushort EnvNo;			// エンベロープ番号(FMは常に0)

		public ushort Count;			// 発音カウント
		public ushort dmy1;			// 

		public byte Keyon;			// keyon毎にインクリメント(++)する
		public FMPNote Note;			// 音程 o1c=0 ～ o9b=107 rest=255
		public byte Tone;			// 音色番号 0~255
		public byte Volume;			// 音量値 0~127

		public byte Pan;			// パン値 1(R)～128(C)～255(L)
		public byte Noise;			// SSGノイズ周波数(SSG以外は常に0)
		public SByte KeyTrans;		// キートランスポーズ(-95～95)(added 7.05c)
		public byte dmy2;			// 本当はbyte[1](modified 7.05c)
		public byte dmy30;			// 本当はbyte[4]の一要素(added 7.10a)
		public byte dmy31;			// 本当はbyte[4]の一要素(added 7.10a)
		public byte dmy32;			// 本当はbyte[4]の一要素(added 7.10a)
		public byte dmy33;			// 本当はbyte[4]の一要素(added 7.10a)
		public ushort LFO0;			// LFO詳細種別 (※LFO種別定義参照)(added 7.10a)
		public ushort LFO1;			// LFO詳細種別 (※LFO種別定義参照)(added 7.10a)
		public ushort LFO2;			// LFO詳細種別 (※LFO種別定義参照)(added 7.10a)
		public ushort LFO3;			// LFO詳細種別 (※LFO種別定義参照)(added 7.10a)

		private FMPLFOType GetLFOType(ushort lfo)
		{
			return (FMPLFOType)(lfo & 0xfff0);
		}

		private FMPLFOWaveform GetLFOWaveform(ushort lfo)
		{
			return (FMPLFOWaveform)(lfo & 0xf);
		}

		private ushort GetLFO(FMPLFOType lfoType, FMPLFOWaveform lfoWaveform)
		{
			return
				(ushort)(
					((ushort)lfoType & 0xfff0) |
					((ushort)lfoWaveform & 0xf));
		}

		public FMPLFOType LFO0Type
		{
			get { return GetLFOType(LFO0); }
			set
			{
				LFO0 = GetLFO(value, GetLFOWaveform(LFO0));
			}
		}

		public FMPLFOWaveform LFO0Waveform
		{
			get { return GetLFOWaveform(LFO0); }
			set
			{
				LFO0 = GetLFO(GetLFOType(LFO0), value);
			}
		}

		public FMPLFOType LFO1Type
		{
			get { return GetLFOType(LFO1); }
			set
			{
				LFO1 = GetLFO(value, GetLFOWaveform(LFO1));
			}
		}

		public FMPLFOWaveform LFO1Waveform
		{
			get { return GetLFOWaveform(LFO1); }
			set
			{
				LFO1 = GetLFO(GetLFOType(LFO1), value);
			}
		}

		public FMPLFOType LFO2Type
		{
			get { return GetLFOType(LFO2); }
			set
			{
				LFO2 = GetLFO(value, GetLFOWaveform(LFO2));
			}
		}

		public FMPLFOWaveform LFO2Waveform
		{
			get { return GetLFOWaveform(LFO2); }
			set
			{
				LFO2 = GetLFO(GetLFOType(LFO2), value);
			}
		}

		public FMPLFOType LFO3Type
		{
			get { return GetLFOType(LFO3); }
			set
			{
				LFO3 = GetLFO(value, GetLFOWaveform(LFO3));
			}
		}

		public FMPLFOWaveform LFO3Waveform
		{
			get { return GetLFOWaveform(LFO3); }
			set
			{
				LFO3 = GetLFO(GetLFOType(LFO3), value);
			}
		}
	}
}
