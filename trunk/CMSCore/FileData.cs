using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carrotware.CMS.Data;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.Core {

	public class FileData {
		public FileData() { }
		public string FileName { get; set; }
		public string FileExtension { get; set; }
		public DateTime FileDate { get; set; }
		public int FileSize { get; set; }
		public string FileSizeFriendly { get; set; }
		public string FolderPath { get; set; }
	}

	public class FileDataHelper {
		public FileDataHelper() { }

		private string _FileTypes = null;
		public List<string> BlockedTypes {
			get {
				if (_FileTypes == null) {
					try { _FileTypes = System.Configuration.ConfigurationManager.AppSettings["BlockFromFileManager"].ToString(); } catch { }
				}
				if (_FileTypes == null) {
					_FileTypes = "aspx;ascx;asmx;dll;config";
				}
				return _FileTypes.Split(';').ToList();
			}
		}

		public List<FileData> GetFolders(string sPath, string sQuery) {

			var dsID = new List<FileData>();

			foreach (string myFile in Directory.GetDirectories(sPath, "*.*")) {
				string myFileName;
				string myFileDate;
				var f = new FileData();

				myFileName = Path.GetFileName(myFile).Trim();
				if (myFileName.Length > 0) {
					FileInfo MyFile = new FileInfo(sPath + "/" + myFileName);
					myFileDate = File.GetLastWriteTime(MyFile.FullName).ToString();
					string sP = sQuery + myFileName + "/";
					sP.Replace(@"//", @"/").Replace(@"//", @"/");

					f.FileName = myFileName;
					f.FolderPath = sP;
					f.FileDate = Convert.ToDateTime(myFileDate);

					dsID.Add(f);
				}
			}

			return dsID;
		}

		public List<FileData> GetFiles(string sPath, string sQuery) {

			var dsID = new List<FileData>();

			foreach (string myFile in Directory.GetFiles(sPath, "*.*")) {
				string myFileName;
				string myFileDate;
				string myFileSizeF;
				long myFileSize;

				myFileName = Path.GetFileName(myFile).Trim();

				var f = new FileData();
				f.FileName = myFileName;

				if (myFileName.Length > 0) {
					FileInfo MyFile = new FileInfo(sPath + "/" + myFileName);
					myFileDate = File.GetLastWriteTime(MyFile.FullName).ToString();
					myFileSize = MyFile.Length;

					myFileSizeF = myFileSize.ToString() + " B";

					if (myFileSize > 1500) {
						if (myFileSize > (1024 * 1024)) {
							myFileSizeF = (Convert.ToDouble(Convert.ToInt32((myFileSize * 100) / (1024 * 1024))) / 100).ToString() + " MB";
						} else {
							myFileSizeF = (Convert.ToDouble(Convert.ToInt32((myFileSize * 100) / 1024)) / 100).ToString() + " KB";
						}
					}
					string sP = sQuery;
					sP.Replace(@"//", @"/").Replace(@"//", @"/");

					f.FileName = myFileName;
					f.FolderPath = sP;
					f.FileDate = Convert.ToDateTime(myFileDate);
					f.FileSize = Convert.ToInt32(myFileSize);
					f.FileSizeFriendly = myFileSizeF;
					if (!string.IsNullOrEmpty(MyFile.Extension)) {
						f.FileExtension = MyFile.Extension.ToLower();
					}
					try {
						if ((from b in BlockedTypes
							 where b.ToLower().Replace(".", "") == f.FileExtension.Replace(".", "")
							 select b).Count() < 1) {
							dsID.Add(f);
						}
					} catch (Exception ex) { }

				}
			}

			return dsID;
		}


	}


}
