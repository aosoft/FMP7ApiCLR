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
	/// ドライバ情報
	/// </summary>
	[Flags]
	public enum MXDRVDriverInfo : int
	{
		MXDRV = 0x00000001,
		PCM8 = 0x00000002,
	}

	/// <summary>
	/// ファイルの種類。
	/// </summary>
	public enum MXDRVFileType : int
	{
		Music = 0,
		PDX = 1
	}

	public class MXDRVControl
	{
		public const int MaxCommentSize = 80 * 2;

		private enum APICode : uint
		{
			GetComment = 1,		// 曲データコメント取得
			GetDriver = 2,		// 動作ドライバ
			GetFilePath = 3		// 演奏ファイル情報取得
		}

		/// <summary>
		/// コメントを取得する。
		/// 曲データに含まれているデータそのまま(Shift JIS)です。
		/// string として使う場合は適切に変換する必要があります。
		/// </summary>
		/// <returns></returns>
		unsafe public static byte[] GetComment()
		{
			byte[] buf = new byte[MaxCommentSize];

			if (FMPControl.CallExAPI(
				DriverID.MXDRV,
				(uint)APICode.GetComment, 0, 0, buf, 0, MaxCommentSize) != 0)
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
		/// 動作ドライバ情報を取得する。
		/// </summary>
		/// <returns></returns>
		public static MXDRVDriverInfo GetDriverInfo()
		{
			return (MXDRVDriverInfo)FMPControl.CallExAPI(
				DriverID.MXDRV,
				(uint)APICode.GetDriver, 0, 0, null, 0, 0);
		}

		/// <summary>
		/// ファイルパスを取得する。
		/// </summary>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public static string GetFilePath(MXDRVFileType fileType)
		{
			byte[] ret = new byte[FMPControl.MaxPath];

			if (FMPControl.CallExAPI(
				DriverID.MXDRV,
				(uint)APICode.GetFilePath, (uint)fileType, 0, ret, 0, FMPControl.MaxPath) != 0)
			{
				return null;
			}

			return FMPControl.GetStringFromSJISByteArray(ret);
		}
	}
}
