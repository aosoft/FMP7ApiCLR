//
//	FMP7 API for .NET Copyright (c) 2010-2012 TAN-Y
//	FMP7 SDK          Copyright (c) 2010-2012 Guu
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FMP.FMP7
{
	/// <summary>
	/// 音程
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FMPNote
	{
		private static string[] m_NoteNames =
			{
				"c ", "c#", "d ", "d#", "e ",
				"f ", "f#", "g ", "g#", "a ", "a#", "b "
			};

		private byte m_note;

		public static readonly FMPNote Rest = new FMPNote(255);

		public FMPNote(byte nativeNote)
		{
			m_note = nativeNote;
		}

		/// <summary>
		/// FMP内部で扱う音程データそのもの。
		/// </summary>
		public byte NativeNote
		{
			get
			{
				return m_note;
			}

			set
			{
				m_note = value;
			}
		}

		/// <summary>
		/// 休符かどうか
		/// </summary>
		public bool IsRest
		{
			get
			{
				return m_note == 255;
			}
		}

		/// <summary>
		/// オクターブ値。
		/// </summary>
		/// <remarks>
		/// MML表記の値-1になる。o1 = 0。
		/// </remarks>
		public int Octave
		{
			get
			{
				return m_note / 12;
			}
		}

		/// <summary>
		/// キーコード。
		/// </summary>
		/// <remarks>
		/// c = 0, c+ = 1, ... , a+ = 10, b = 11
		/// </remarks>
		public int Key
		{
			get
			{
				return m_note % 12;
			}
		}

		/// <summary>
		/// 論理周波数値。
		/// FMPPartWorkのFreqと同じ形式の値。
		/// </summary>
		/// <seealso cref="FMPPartWork"/>
		public int Freq
		{
			get
			{
				return ((int)m_note + 1)* 64; 
			}
		}

		/// <summary>
		/// 音程の文字列表現を取得する。
		/// </summary>
		/// <returns>表現した文字列</returns>
		public override string ToString()
		{
			if (IsRest)
			{
				return "r";
			}
			return string.Format("{0}{1}", m_NoteNames[Key], Octave + 1);
		}


		public static bool operator ==(FMPNote srcA, FMPNote srcB)
		{
			return srcA.m_note == srcB.m_note;
		}

		public static bool operator !=(FMPNote srcA, FMPNote srcB)
		{
			return !(srcA == srcB);
		}

		public override int GetHashCode()
		{
			return m_note.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			FMPNote? val = obj as FMPNote?;
			if (val.HasValue)
			{
				return this == val.Value;
			}
			return false;
		}

	}
}
