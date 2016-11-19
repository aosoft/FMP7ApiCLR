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
	/// FMPv4 のドライバ情報。動作しているドライバのフラグがセットされます。
	/// </summary>
	[Flags]
	public enum FMP4DriverInfo : int
	{
		FMP = 1,
		PPZ8 = 2,
		PDZFZ8X = 4
	}

	/// <summary>
	/// ファイルの種類。
	/// </summary>
	public enum FMP4FileType : uint
	{
		Music = 0,
		PVI = 1,
		PPZ = 2
	}

	/// <summary>
	/// exFMP4 (FMPv4 addon) 拡張 API 制御
	/// </summary>
	public class FMP4Control
	{
		public const int MaxCommentSize = 16 * 1024;

		private enum FMP4APICode : uint
		{
			GetComment = 1,		// 曲データコメント取得
			GetSync = 2,		// 曲データシンクロカウンタ取得
			GetDriver = 3,		// 動作ドライバ
			GetFilePath = 4		// 演奏ファイル情報取得
		}

		/// <summary>
		/// FMP のコメントデータを取得する。
		/// コメントは全データ(3行以上)の部分を含みます。
		/// 曲データに含まれているデータそのまま(Shift JIS)です。
		/// string として使う場合は適切に変換する必要があります。
		/// </summary>
		/// <returns></returns>
		unsafe public static byte[] GetComment()
		{
			byte[] ret = new byte[MaxCommentSize];

			if (FMPControl.CallExAPI(
				DriverID.FMP4,
				(uint)FMP4APICode.GetComment, 0, 0, ret, 0, MaxCommentSize) != 0)
			{
				return null;
			}

			return ret;
		}

		/// <summary>
		/// 同期カウンタ値を取得する。
		/// </summary>
		/// <returns></returns>
		public static int GetSyncCounter()
		{
			return FMPControl.CallExAPI(
				DriverID.FMP4,
				(uint)FMP4APICode.GetSync, 0, 0, null, 0, 0);
		}

		/// <summary>
		/// 動作ドライバ情報を取得する。
		/// </summary>
		/// <returns></returns>
		public static FMP4DriverInfo GetDriverInfo()
		{
			return (FMP4DriverInfo)FMPControl.CallExAPI(
				DriverID.FMP4,
				(uint)FMP4APICode.GetDriver, 0, 0, null, 0, 0);
		}

		/// <summary>
		/// ファイルパスを取得する。
		/// </summary>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public static string GetFilePath(FMP4FileType fileType)
		{
			byte[] ret = new byte[FMPControl.MaxPath];

			if (FMPControl.CallExAPI(
				DriverID.FMP4,
				(uint)FMP4APICode.GetFilePath, (uint)fileType, 0, ret, 0, FMPControl.MaxPath) != 0)
			{
				return null;
			}

			return FMPControl.GetStringFromSJISByteArray(ret);
		}
	}
}
