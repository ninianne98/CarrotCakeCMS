using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class FileBrowser : AdminBasePage {
		public string sQueryPath = string.Empty;
		public string sQueryMode = "1";
		public string sReturnMode = "0";
		public string sViewMode = string.Empty;
		private string defaultBrowseMode = "file";

		protected FileDataHelper helpFile = new FileDataHelper();

		protected void Page_Load(object sender, EventArgs e) {
			sQueryPath = Request.QueryString["fldrpath"];
			sViewMode = defaultBrowseMode;
			lblWarning.Text = string.Empty;

			try { sQueryMode = Request.QueryString["useTiny"]; } catch { }
			try { sReturnMode = Request.QueryString["returnvalue"]; } catch { }
			try { sViewMode = Request.QueryString["viewmode"]; } catch { }

			if (sViewMode.ToLower() != defaultBrowseMode) {
				lnkThumbView.Visible = false;
				lnkFileView.Visible = true;
				btnRemove.Visible = false;
			} else {
				lnkThumbView.Visible = true;
				lnkFileView.Visible = false;
				btnRemove.Visible = true;
			}

			if (sQueryMode != "1") {
				sQueryMode = "0";
				pnlTiny.Visible = false;
				pnlTiny2.Visible = false;
				pnlFileMgr.Visible = true;
			} else {
				sQueryMode = "1";
				pnlFileMgr.Visible = false;
			}

			if (sReturnMode == "1") {
				btnReturnFile.Visible = true;
			}

			if (!string.IsNullOrEmpty(sQueryPath)) {
				if (sQueryPath.Length == 1) {
					sQueryPath = string.Empty;
				}
			}

			if (string.IsNullOrEmpty(sQueryPath)) {
				sQueryPath = "/";
				lnkUp.Visible = false;
			} else {
				lnkUp.Visible = true;
			}
			sQueryPath = sQueryPath.StartsWith(@"/") ? sQueryPath : @"/" + sQueryPath;
			sQueryPath.Replace("//", "/").Replace("//", "/");

			if (lnkUp.Visible) {
				lnkUp.NavigateUrl = SiteData.CurrentScriptName + "?useTiny=" + sQueryMode + "&returnvalue=" + sReturnMode + "&viewmode=" + sViewMode + "&fldrpath=" + sQueryPath.Substring(0, sQueryPath.Substring(0, sQueryPath.Length - 2).LastIndexOf('/')) + @"/";
			}

			lnkThumbView.NavigateUrl = String.Format("{0}?fldrpath={1}&useTiny={2}&returnvalue={3}&viewmode=thumb", SiteData.CurrentScriptName, sQueryPath, sQueryMode, sReturnMode, sViewMode);
			lnkFileView.NavigateUrl = String.Format("{0}?fldrpath={1}&useTiny={2}&returnvalue={3}&viewmode=file", SiteData.CurrentScriptName, sQueryPath, sQueryMode, sReturnMode, sViewMode);


			if (!IsPostBack) {
				LoadLists();
			}
			lblPath.Text = sQueryPath;


		}

		protected void LoadLists() {
			var fldr = helpFile.GetFolders(sQueryPath);
			var fls = helpFile.GetFiles(sQueryPath);

			GeneralUtilities.BindRepeater(rpFolders, fldr);

			if (sViewMode.ToLower() != defaultBrowseMode) {
				GeneralUtilities.BindRepeater(rpThumbs, fls.Where(x => x.MimeType.StartsWith("image/")).ToList());
				rpThumbs.Visible = true;
				rpFiles.Visible = false;
			} else {
				GeneralUtilities.BindRepeater(rpFiles, fls);
				rpFiles.Visible = true;
				rpThumbs.Visible = false;
			}
		}

		public string CreateFileLink(string sPath) {
			return string.Format("javascript:SetFile('{0}');", sPath);
		}

		public string CreateFileSrc(string sPath, string sFile, string sMime) {

			if (FileImageLink(sMime).ToLower() == "image") {
				return String.Format("{0}{1}", sPath, sFile).ToLower();
			} else {
				return SiteData.AdminFolderPath + "images/document.png";
			}
		}

		public string FileImageLink(string sMime) {

			sMime = sMime.ToLower();
			var mime = sMime.Substring(0, sMime.IndexOf("/"));

			string sImage = "plain";

			switch (mime) {
				case "image":
					sImage = "image";
					break;
				case "audio":
					sImage = "audio";
					break;
				case "video":
					sImage = "video";
					break;
				case "application":
					sImage = "application";
					break;
				default:
					sImage = "plain";
					break;
			}

			switch (sMime) {
				case "application/pdf":
					sImage = "pdf";
					break;
				case "text/html":
				case "text/asp":
					sImage = "html";
					break;
				case "application/excel":
					sImage = "spreadsheet";
					break;
				case "application/rtf":
				case "application/msword":
					sImage = "wordprocessing";
					break;
				case "application/x-compressed":
				case "application/zip":
					sImage = "compress";
					break;
			}

			return sImage;
		}

		protected void btnUpload_Click(object sender, EventArgs e) {
			lblWarning.Text = string.Empty;
			lblWarning.CssClass = string.Empty;
			try {
				if (upFile.HasFile) {
					var sPath = SetSitePath(sQueryPath);

					if ((from b in helpFile.BlockedTypes
						 where upFile.FileName.ToLower().Contains("." + b.ToLower())
						 select b).Count() < 1) {

						upFile.SaveAs(Path.Combine(sPath, upFile.FileName));
						lblWarning.Text = string.Format("file [{0}] uploaded!", upFile.FileName);
						lblWarning.CssClass = "uploadSuccess";
					} else {
						lblWarning.Text = string.Format("[{0}] is a blocked filetype.", upFile.FileName);
						lblWarning.CssClass = "uploadBlocked";
					}

					LoadLists();
				} else {
					lblWarning.Text = "No file detected for upload.";
					lblWarning.CssClass = "uploadNoFile";
				}
			} catch (Exception ex) {
				lblWarning.Text = ex.ToString();
				lblWarning.CssClass = "uploadFail";
			}
		}

		protected void btnRemove_Click(object sender, EventArgs e) {
			var sPath = SetSitePath(sQueryPath);
			foreach (RepeaterItem f in rpFiles.Items) {
				CheckBox chkRemove = (CheckBox)f.FindControl("chkRemove");
				if (chkRemove != null) {
					if (chkRemove.Checked) {
						var fname = chkRemove.Attributes["value"];
						File.Delete(Path.Combine(sPath, fname));
					}
				}
			}
			LoadLists();
		}

		protected string SetSitePath(string sPath) {
			return FileDataHelper.MakeFileFolderPath(sPath);
		}

	}
}
