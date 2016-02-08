using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using Carrotware.Web.UI.Controls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class FileBrowser : AdminBasePage {
		public string sQueryPath = String.Empty;
		public string sQueryMode = "1";
		public string sReturnMode = "0";
		public string sViewMode = String.Empty;
		private string defaultBrowseMode = "file";

		protected FileDataHelper helpFile = CMSConfigHelper.GetFileDataHelper();

		protected void Page_Load(object sender, EventArgs e) {
			try { sQueryPath = Request.QueryString["fldrpath"]; } catch { sQueryPath = @"/"; }

			sViewMode = defaultBrowseMode;
			lblWarning.Text = String.Empty;

			try { sQueryMode = Request.QueryString["useTiny"].ToString(); } catch { sQueryMode = "0"; }
			try { sReturnMode = Request.QueryString["returnvalue"].ToString(); } catch { sReturnMode = "0"; }
			try { sViewMode = Request.QueryString["viewmode"].ToString(); } catch { sViewMode = defaultBrowseMode; }

			if (String.Format("{0}", sViewMode).ToLowerInvariant() != defaultBrowseMode) {
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

			if (!String.IsNullOrEmpty(sQueryPath)) {
				if (sQueryPath.Length == 1) {
					sQueryPath = String.Empty;
				}
			}

			if (String.IsNullOrEmpty(sQueryPath)) {
				sQueryPath = "/";
				lnkUp.Visible = false;
			} else {
				lnkUp.Visible = true;
			}
			sQueryPath = sQueryPath.StartsWith(@"/") ? sQueryPath : @"/" + sQueryPath;
			sQueryPath.Replace("//", "/").Replace("//", "/");

			if (lnkUp.Visible) {
				string sUrlUp = sQueryPath.Substring(0, sQueryPath.Substring(0, sQueryPath.Length - 2).LastIndexOf('/')) + @"/";
				lnkUp.NavigateUrl = String.Format("{0}?fldrpath={1}&useTiny={2}&returnvalue={3}&viewmode={4}", SiteData.CurrentScriptName, sUrlUp, sQueryMode, sReturnMode, sViewMode);
			}

			lnkThumbView.NavigateUrl = String.Format("{0}?fldrpath={1}&useTiny={2}&returnvalue={3}&viewmode=thumb", SiteData.CurrentScriptName, sQueryPath, sQueryMode, sReturnMode);
			lnkFileView.NavigateUrl = String.Format("{0}?fldrpath={1}&useTiny={2}&returnvalue={3}&viewmode=file", SiteData.CurrentScriptName, sQueryPath, sQueryMode, sReturnMode);

			if (!lnkThumbView.Visible) {
				lnkRefresh.NavigateUrl = lnkThumbView.NavigateUrl;
			} else {
				lnkRefresh.NavigateUrl = lnkFileView.NavigateUrl;
			}

			if (!IsPostBack) {
				LoadLists();
			}

			FileDirectory.Value = sQueryPath;
			litPath.Text = sQueryPath;
		}

		protected void LoadLists() {
			var fldr = helpFile.GetFolders(sQueryPath);
			var fls = helpFile.GetFiles(sQueryPath);

			GeneralUtilities.BindRepeater(rpFolders, fldr);

			if (sViewMode.ToLowerInvariant() != defaultBrowseMode) {
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
			return String.Format("javascript:SetFile('{0}');", sPath);
		}

		public string CreateFileSrc(string sPath, string sFile, string sMime) {
			if (FileImageLink(sMime).ToLowerInvariant() == "image") {
				return String.Format("{0}{1}", sPath, sFile).ToLowerInvariant();
			} else {
				return SiteData.AdminFolderPath + "images/document.png";
			}
		}

		public string FileImageLink(string sMime) {
			sMime = sMime.ToLowerInvariant();
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
			lblWarning.Text = String.Empty;
			lblWarning.CssClass = String.Empty;
			/*
			try {
				if (upFile.HasFile) {
					var sPath = SetSitePath(sQueryPath);
					string uploadedFileName = upFile.FileName;

					if ((from b in helpFile.BlockedTypes
						 where uploadedFileName.ToLowerInvariant().Contains("." + b.ToLowerInvariant())
						 select b).Count() < 1) {
						if (chkSpaceEscape.Checked) {
							uploadedFileName = uploadedFileName.Replace(" ", "-");
						}

						upFile.SaveAs(Path.Combine(sPath, uploadedFileName));
						lblWarning.Text = String.Format("file [{0}] uploaded!", uploadedFileName);
						lblWarning.CssClass = "uploadSuccess";
					} else {
						lblWarning.Text = String.Format("[{0}] is a blocked filetype.", uploadedFileName);
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
			*/
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