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
	/// ファイルの種類。
	/// </summary>
	public enum PMDFileType : uint
	{
		Music = 0,
		PPC = 1,
		PPS = 2,
		PPZ1 = 3,
		PPZ2 = 4
	}

	public class PMDControl
	{
		public const int MaxCommentSize = 1024;

		private enum PMDAPICode : uint
		{
			GetMemo = 1,		// 曲データコメント取得
			GetDriver = 2,		// 動作ドライバ
			GetFilePath = 3		// 演奏ファイル情報取得
		}

		unsafe private static byte[] InternalGetMemo(int index)
		{
			byte[] buf = new byte[MaxCommentSize];

			if (FMPControl.CallExAPI(
				DriverID.PMD,
				(uint)PMDAPICode.GetMemo, (uint)index, 0, buf, 0, MaxCommentSize) != 0)
			{
				return null;
			}

			int length = Array.IndexOf<byte>(buf, 0);

			if (length < 0)
			{
				return buf;
			}
			byte[] ret = new byte[length];
			Array.Copy(buf, ret, length);
			return ret;
		}

		/// <summary>
		/// タイトルを取得する。
		/// 曲データに含まれているデータそのまま(Shift JIS)です。
		/// string として使う場合は適切に変換する必要があります。
		/// </summary>
		/// <returns></returns>
		public static byte[] GetTitle()
		{
			return InternalGetMemo(1);
		}

		/// <summary>
		/// 作曲者を取得する。
		/// 曲データに含まれているデータそのまま(Shift JIS)です。
		/// string として使う場合は適切に変換する必要があります。
		/// </summary>
		/// <returns></returns>
		public static byte[] GetComposer()
		{
			return InternalGetMemo(2);
		}

		/// <summary>
		/// 編曲者を取得する。
		/// 曲データに含まれているデータそのまま(Shift JIS)です。
		/// string として使う場合は適切に変換する必要があります。
		/// </summary>
		/// <returns></returns>
		public static byte[] GetArranger()
		{
			return InternalGetMemo(3);
		}

		/// <summary>
		/// メモを取得する。
		/// 曲データに含まれているデータそのまま(Shift JIS)です。
		/// string として使う場合は適切に変換する必要があります。
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public static byte[] GetMemo(int index)
		{
			if (index < 1 || index > 128)
			{
				return null;
			}

			return InternalGetMemo(3 + index);
		}

		/// <summary>
		/// ファイルパスを取得する。
		/// </summary>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public static string GetFilePath(PMDFileType fileType)
		{
			byte[] ret = new byte[FMPControl.MaxPath];

			if (FMPControl.CallExAPI(
				DriverID.PMD,
				(uint)PMDAPICode.GetFilePath, (uint)fileType, 0, ret, 0, FMPControl.MaxPath) != 0)
			{
				return null;
			}

			return FMPControl.GetStringFromSJISByteArray(ret);
		}
	}
}
