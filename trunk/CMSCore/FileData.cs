using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public string MimeType { get; set; }

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


		public FileData GetFolderInfo(string sPath, string sQuery, string myFile) {

			string myFileName;
			string myFileDate;

			var f = new FileData();

			myFileName = Path.GetFileName(myFile).Trim();
			if (myFileName.Length > 0) {
				FileInfo MyFile = new FileInfo(sPath + "/" + myFileName);
				myFileDate = File.GetLastWriteTime(MyFile.FullName).ToString();
				string sP = sQuery + myFileName + "/";

				f.FileName = myFileName;
				f.FolderPath =  MakeFilePathUniform(sP);
				f.FileDate = Convert.ToDateTime(myFileDate);
			}

			return f;
		}


		public List<FileData> GetFolders(string sPath, string sQuery) {

			var dsID = new List<FileData>();

			foreach (string myFile in Directory.GetDirectories(sPath, "*.*")) {
				string myFileName;
				var f = new FileData();

				myFileName = Path.GetFileName(myFile).Trim();
				if (myFileName.Length > 0) {
					f = GetFolderInfo(sPath, sQuery, myFile);
					dsID.Add(f);
				}
			}

			return dsID;
		}


		public FileData GetFileInfo(string sPath, string sQuery, string myFile) {

			string myFileName;
			string myFileDate;
			string myFileSizeF;
			long myFileSize;

			sPath = MakeFilePathUniform(sPath);
			sQuery = MakeFilePathUniform(sQuery);

			sPath = sPath.Substring(0, sPath.LastIndexOf("/") + 1);
			sQuery = sQuery.Substring(0, sQuery.LastIndexOf("/") + 1);

			sPath = MakeFilePathUniform(sPath);
			sQuery = MakeFilePathUniform(sQuery);

			var f = new FileData();

			myFileName = Path.GetFileName(myFile).Trim();
			if (myFileName.Length > 0 && File.Exists(sPath + "/" + myFileName)) {
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

				f.FileName = myFileName;
				f.FolderPath = MakeFilePathUniform(sP);
				f.FileDate = Convert.ToDateTime(myFileDate);
				f.FileSize = Convert.ToInt32(myFileSize);
				f.FileSizeFriendly = myFileSizeF;
				if (!string.IsNullOrEmpty(MyFile.Extension)) {
					f.FileExtension = MyFile.Extension.ToLower();
				} else {
					f.FileExtension = ".";
				}

				f.MimeType = "text/plain";

				try {
					if ((from b in MimeTypes
						 where b.Key.ToLower() == f.FileExtension.ToLower()
						 select b).Count() > 0) {
						f.MimeType = (from b in MimeTypes
									  where b.Key.ToLower() == f.FileExtension.ToLower()
									  select b.Value).FirstOrDefault();
					}
				} catch (Exception ex) { }


			}

			return f;
		}

		public static string MakeFilePathUniform(string sPath) {
			if (!string.IsNullOrEmpty(sPath)) {
				sPath = sPath.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/");
			}
			return sPath;
		}


		public List<FileData> GetFiles(string sPath, string sQuery) {

			var dsID = new List<FileData>();

			foreach (string myFile in Directory.GetFiles(sPath, "*.*")) {
				string myFileName;

				myFileName = Path.GetFileName(myFile).Trim();

				var f = new FileData();
				f.FileName = myFileName;

				if (myFileName.Length > 0) {

					f = GetFileInfo(sPath, sQuery, myFile);

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


		List<string> _spiderdirs = null;

		private void SpiderFolders(string sPath) {

			string[] subdirs;
			try {
				subdirs = Directory.GetDirectories(sPath);
			} catch {
				subdirs = null;
			}

			if (subdirs != null) {
				foreach (string theDir in subdirs) {
					_spiderdirs.Add(theDir);
					SpiderFolders(theDir);
				}
			}
		}


		public List<string> SpiderDeepFolders(string sPath) {
			_spiderdirs = new List<string>();

			SpiderFolders(sPath);

			return _spiderdirs;
		}


		List<FileData> _spiderFD = null;

		private void SpiderFoldersFD(string sPath, string sQuery) {

			string[] subdirs;
			try {
				subdirs = Directory.GetDirectories(sPath);
			} catch {
				subdirs = null;
			}

			if (subdirs != null) {
				foreach (string theDir in subdirs) {
					var f = GetFolderInfo(sPath, sQuery, theDir);
					_spiderFD.Add(f);
					SpiderFoldersFD(sPath + f.FolderPath, f.FolderPath);
				}
			}
		}


		public List<FileData> SpiderDeepFoldersFD(string sPath, string sQuery) {
			_spiderFD = new List<FileData>();

			SpiderFoldersFD(sPath, sQuery);

			return _spiderFD;
		}





		private static Dictionary<string, string> _dict = null;

		public static Dictionary<string, string> MimeTypes {
			get {
				if (_dict == null) {
					_dict = new Dictionary<string, string>();

					_dict.Add(".ai", "application/postscript");
					_dict.Add(".aif", "audio/aiff");
					_dict.Add(".aifc", "audio/aiff");
					_dict.Add(".aiff", "audio/aiff");
					_dict.Add(".aps", "application/mime");
					_dict.Add(".arc", "application/octet-stream");
					_dict.Add(".arj", "application/octet-stream");
					_dict.Add(".asf", "video/x-ms-asf");
					_dict.Add(".asm", "text/x-asm");
					_dict.Add(".asp", "text/asp");
					_dict.Add(".aspx", "text/asp");
					_dict.Add(".asx", "video/x-ms-asf");
					_dict.Add(".au", "audio/basic");
					_dict.Add(".avi", "video/avi");
					_dict.Add(".avs", "video/avs-video");
					_dict.Add(".bin", "application/octet-stream");
					_dict.Add(".bmp", "image/bmp");
					_dict.Add(".bz", "application/x-bzip");
					_dict.Add(".bz2", "application/x-bzip2");
					_dict.Add(".c", "text/plain");
					_dict.Add(".c++", "text/plain");
					_dict.Add(".cc", "text/plain");
					_dict.Add(".cer", "application/x-x509-ca-cert");
					_dict.Add(".class", "application/java");
					_dict.Add(".com", "application/octet-stream");
					_dict.Add(".conf", "text/plain");
					_dict.Add(".cpp", "text/x-c");
					_dict.Add(".crt", "application/x-x509-ca-cert");
					_dict.Add(".csh", "application/x-csh");
					_dict.Add(".css", "text/css");
					_dict.Add(".def", "text/plain");
					_dict.Add(".dir", "application/x-director");
					_dict.Add(".doc", "application/msword");
					_dict.Add(".docx", "application/msword");
					_dict.Add(".dot", "application/msword");
					_dict.Add(".dump", "application/octet-stream");
					_dict.Add(".dvi", "application/x-dvi");
					_dict.Add(".dwf", "model/vnd.dwf");
					_dict.Add(".dwg", "application/acad");
					_dict.Add(".dxf", "application/dxf");
					_dict.Add(".el", "text/x-script.elisp");
					_dict.Add(".eps", "application/postscript");
					_dict.Add(".es", "application/x-esrehber");
					_dict.Add(".etx", "text/x-setext");
					_dict.Add(".evy", "application/envoy");
					_dict.Add(".exe", "application/octet-stream");
					_dict.Add(".f", "text/plain");
					_dict.Add(".fif", "image/fif");
					_dict.Add(".fli", "video/x-fli");
					_dict.Add(".flo", "image/florian");
					_dict.Add(".flx", "text/vnd.fmi.flexstor");
					_dict.Add(".fmf", "video/x-atomic3d-feature");
					_dict.Add(".for", "text/plain");
					_dict.Add(".frl", "application/freeloader");
					_dict.Add(".gif", "image/gif");
					_dict.Add(".gl", "video/gl");
					_dict.Add(".gsd", "audio/x-gsm");
					_dict.Add(".gsm", "audio/x-gsm");
					_dict.Add(".gsp", "application/x-gsp");
					_dict.Add(".gss", "application/x-gss");
					_dict.Add(".gtar", "application/x-gtar");
					_dict.Add(".gz", "application/x-compressed");
					_dict.Add(".gzip", "application/x-compressed");
					_dict.Add(".h", "text/plain");
					_dict.Add(".help", "application/x-helpfile");
					_dict.Add(".hh", "text/plain");
					_dict.Add(".hlp", "application/hlp");
					_dict.Add(".hpg", "application/vnd.hp-hpgl");
					_dict.Add(".hpgl", "application/vnd.hp-hpgl");
					_dict.Add(".hqx", "application/binhex");
					_dict.Add(".hta", "application/hta");
					_dict.Add(".htc", "text/x-component");
					_dict.Add(".htm", "text/html");
					_dict.Add(".html", "text/html");
					_dict.Add(".htmls", "text/html");
					_dict.Add(".htt", "text/webviewhtml");
					_dict.Add(".htx", "text/html");
					_dict.Add(".ico", "image/x-icon");
					_dict.Add(".imap", "application/x-httpd-imap");
					_dict.Add(".inf", "application/inf");
					_dict.Add(".it", "audio/it");
					_dict.Add(".java", "text/plain");
					_dict.Add(".jpe", "image/jpeg");
					_dict.Add(".jpeg", "image/jpeg");
					_dict.Add(".jpg", "image/jpeg");
					_dict.Add(".js", "text/javascript");
					_dict.Add(".log", "text/plain");
					_dict.Add(".m", "text/plain");
					_dict.Add(".m1v", "video/mpeg");
					_dict.Add(".m2a", "audio/mpeg");
					_dict.Add(".m2v", "video/mpeg");
					_dict.Add(".m3u", "audio/x-mpequrl");
					_dict.Add(".man", "application/x-troff-man");
					_dict.Add(".map", "application/x-navimap");
					_dict.Add(".mcd", "application/mcad");
					_dict.Add(".mht", "message/rfc822");
					_dict.Add(".mhtml", "message/rfc822");
					_dict.Add(".mid", "audio/midi");
					_dict.Add(".midi", "audio/midi");
					_dict.Add(".mime", "message/rfc822");
					_dict.Add(".mm", "application/base64");
					_dict.Add(".mod", "audio/mod");
					_dict.Add(".moov", "video/quicktime");
					_dict.Add(".mov", "video/quicktime");
					_dict.Add(".movie", "video/x-sgi-movie");
					_dict.Add(".mp2", "video/mpeg");
					_dict.Add(".mp3", "audio/mpeg3");
					_dict.Add(".mpa", "audio/mpeg");
					_dict.Add(".mpeg", "video/mpeg");
					_dict.Add(".mpg", "video/mpeg");
					_dict.Add(".mpga", "audio/mpeg");
					_dict.Add(".mpp", "application/vnd.ms-project");
					_dict.Add(".mpt", "application/x-project");
					_dict.Add(".mpv", "application/x-project");
					_dict.Add(".mpx", "application/x-project");
					_dict.Add(".mrc", "application/marc");
					_dict.Add(".ms", "application/x-troff-ms");
					_dict.Add(".mv", "video/x-sgi-movie");
					_dict.Add(".my", "audio/make");
					_dict.Add(".o", "application/octet-stream");
					_dict.Add(".p", "text/x-pascal");
					_dict.Add(".pas", "text/pascal");
					_dict.Add(".pbm", "image/x-portable-bitmap");
					_dict.Add(".pct", "image/x-pict");
					_dict.Add(".pcx", "image/x-pcx");
					_dict.Add(".pdf", "application/pdf");
					_dict.Add(".pgm", "image/x-portable-greymap");
					_dict.Add(".pic", "image/pict");
					_dict.Add(".pict", "image/pict");
					_dict.Add(".pl", "text/x-script.perl");
					_dict.Add(".png", "image/png");
					_dict.Add(".pot", "application/mspowerpoint");
					_dict.Add(".potx", "application/mspowerpoint");
					_dict.Add(".pps", "application/mspowerpoint");
					_dict.Add(".ppt", "application/mspowerpoint");
					_dict.Add(".pptx", "application/mspowerpoint");
					_dict.Add(".ppz", "application/mspowerpoint");
					_dict.Add(".pre", "application/x-freelance");
					_dict.Add(".ps", "application/postscript");
					_dict.Add(".psd", "application/octet-stream");
					_dict.Add(".py", "text/x-script.python");
					_dict.Add(".qif", "image/x-quicktime");
					_dict.Add(".qt", "video/quicktime");
					_dict.Add(".ra", "audio/x-pn-realaudio");
					_dict.Add(".ram", "audio/x-pn-realaudio");
					_dict.Add(".ras", "application/x-cmu-raster");
					_dict.Add(".rgb", "image/x-rgb");
					_dict.Add(".rm", "audio/x-pn-realaudio");
					_dict.Add(".rt", "text/richtext");
					_dict.Add(".rtf", "application/rtf");
					_dict.Add(".rtx", "application/rtf");
					_dict.Add(".s", "text/x-asm");
					_dict.Add(".sea", "application/sea");
					_dict.Add(".sgm", "text/sgml");
					_dict.Add(".sgml", "text/sgml");
					_dict.Add(".sh", "application/x-bsh");
					_dict.Add(".shar", "application/x-bsh");
					_dict.Add(".shtml", "text/html");
					_dict.Add(".sit", "application/x-stuffit");
					_dict.Add(".snd", "audio/basic");
					_dict.Add(".svf", "image/x-dwg");
					_dict.Add(".swf", "application/x-shockwave-flash");
					_dict.Add(".t", "application/x-troff");
					_dict.Add(".tar", "application/x-compressed");
					_dict.Add(".tgz", "application/x-compressed");
					_dict.Add(".tif", "image/tiff");
					_dict.Add(".tiff", "image/tiff");
					_dict.Add(".uu", "application/octet-stream");
					_dict.Add(".uue", "text/x-uuencode");
					_dict.Add(".vcs", "text/x-vcalendar");
					_dict.Add(".vda", "application/vda");
					_dict.Add(".vrml", "application/x-vrml");
					_dict.Add(".vsd", "application/x-visio");
					_dict.Add(".vst", "application/x-visio");
					_dict.Add(".vsw", "application/x-visio");
					_dict.Add(".wav", "audio/wav");
					_dict.Add(".wmf", "windows/metafile");
					_dict.Add(".word", "application/msword");
					_dict.Add(".wp", "application/wordperfect");
					_dict.Add(".wri", "application/mswrite");
					_dict.Add(".wsc", "text/scriplet");
					_dict.Add(".xbm", "image/x-xbitmap");
					_dict.Add(".xif", "image/vnd.xiff");
					_dict.Add(".xl", "application/excel");
					_dict.Add(".xla", "application/excel");
					_dict.Add(".xlb", "application/excel");
					_dict.Add(".xlc", "application/excel");
					_dict.Add(".xld", "application/excel");
					_dict.Add(".xlk", "application/excel");
					_dict.Add(".xll", "application/excel");
					_dict.Add(".xlm", "application/excel");
					_dict.Add(".xls", "application/excel");
					_dict.Add(".xlsx", "application/excel");
					_dict.Add(".xlt", "application/excel");
					_dict.Add(".xlv", "application/excel");
					_dict.Add(".xlw", "application/excel");
					_dict.Add(".xm", "audio/xm");
					_dict.Add(".xml", "text/xml");
					_dict.Add(".xpm", "image/xpm");
					_dict.Add(".zip", "application/zip");
					_dict.Add(".zsh", "text/x-script.zsh");
				}

				return _dict;
			}
		}


	}


}
