using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FMP.FMP7
{
	/// <summary>
	/// 曲データの拡張子情報
	/// </summary>
	public class MusicFileExtInfo
	{
		/// <summary>
		/// ファイル検索フィルタ情報
		/// </summary>
		public class FilterInfo
		{
			private string _driverName;
			private string _fileFilter;

			internal FilterInfo(string driverName, string fileFilter)
			{
				_driverName = driverName;
				_fileFilter = fileFilter;
			}

			public string DriverName
			{
				get
				{
					return _driverName;
				}
			}

			public string FileFilter
			{
				get
				{
					return _fileFilter;
				}
			}
		}

		#region Private Field

		private FilterInfo[] _filterInfos;
		private string _allFilter;

		#endregion

		#region Constructor

		public MusicFileExtInfo(string extlist)
		{
			if (string.IsNullOrEmpty(extlist))
			{
				return;
			}

			var tmp = extlist.Split(',');
			var tmpbuffer = new List<FilterInfo>(tmp.Length);
			var allexts = new List<string>();

			foreach (var f in tmp)
			{
				try
				{
					var tmp2 = f.Split('|');
					if (tmp2.Length < 2)
					{
						tmpbuffer.Add(new FilterInfo("", f));
					}
					else
					{
						var info = new FilterInfo(tmp2[0].Trim(), tmp2[1].Trim().ToLower());
						var exts = info.FileFilter.Split(';');
						foreach (var ext in exts)
						{
							var ext2 = ext.Trim();
							if (allexts.Contains(ext2) == false)
							{
								allexts.Add(ext2);
							}
						}

						tmpbuffer.Add(info);
					}
				}
				catch
				{
				}
			}

			var allfilter = new StringBuilder();
			foreach (var ext in allexts)
			{
				if (allfilter.Length > 0)
				{
					allfilter.AppendFormat(";{0}", ext);
				}
				else
				{
					allfilter.Append(ext);
				}
			}

			_allFilter = allfilter.ToString();
			_filterInfos = tmpbuffer.ToArray();
		}

		#endregion

		#region Public Method

		/// <summary>
		/// 指定のファイル名の拡張子がサポートしている拡張子かチェックする。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool IsSupportFile(string fileName)
		{
			try
			{
				var ext = Path.GetExtension(fileName);
				if (string.IsNullOrEmpty(ext))
				{
					return false;
				}
				var chkext = string.Format("*{0}", ext.Trim().ToLower());

				return _allFilter.Contains(chkext);
			}
			catch
			{
			}

			return false;
		}

		#endregion

		#region Property

		/// <summary>
		/// 対応拡張子をすべて含んだ検索フィルタ
		/// </summary>
		public string AllFilter
		{
			get
			{
				return _allFilter;
			}
		}

		/// <summary>
		/// フィルタ種別数
		/// </summary>
		public int Count
		{
			get
			{
				if (_filterInfos == null)
				{
					return 0;
				}
				return _filterInfos.Length;
			}
		}

		/// <summary>
		/// 指定のインデックスに対応するフィルタ情報
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public FilterInfo this[int index]
		{
			get
			{
				if (_filterInfos == null)
				{
					throw new ArgumentOutOfRangeException();
				}
				return _filterInfos[index];
			}
		}

		#endregion
	}
}
