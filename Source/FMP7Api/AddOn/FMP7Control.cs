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
	/// ファイルの種類。
	/// </summary>
	public enum FMP7FileType : uint
	{
		Music = 0,
		PWI = 1,
	}

	/// <summary>
	/// exFMP7 (FMP7 addon) 拡張 API 制御
	/// </summary>
	public class FMP7Control
	{
		private enum APICode : uint
		{
			GetFilePath = 1,		// 演奏ファイル情報取得
			GetNumPWI = 2,			// PWI定義数
		}

		/// <summary>
		/// ファイルパスを取得する。
		/// </summary>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public static string GetFilePath(FMP7FileType fileType, int index)
		{
			byte[] ret = new byte[FMPControl.MaxPath * sizeof(char)];

			if (FMPControl.CallExAPI(
				DriverID.FMP7,
				(uint)APICode.GetFilePath, (uint)fileType, (uint)index, ret, 0, ret.Length) != 0)
			{
				return null;
			}

			return FMPControl.GetStringFromUnicodeByteArray(ret);
		}

		/// <summary>
		/// PWI の定義数を取得する。
		/// </summary>
		/// <returns></returns>
		public static int GetNumPWI()
		{
			return FMPControl.CallExAPI(
				DriverID.FMP7, (uint)APICode.GetNumPWI, 0, 0, null, 0, 0);
		}
	}
}
