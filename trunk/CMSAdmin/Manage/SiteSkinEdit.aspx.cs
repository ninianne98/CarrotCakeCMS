using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class SiteSkinEdit : AdminBasePage {
		protected FileDataHelper helpFile = new FileDataHelper();
		public string sTemplateFileQS = String.Empty;
		protected string sTemplateFile = String.Empty;
		protected string sFullFilePath = String.Empty;
		protected string sDirectory = String.Empty;
		protected string sEditFile = String.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentSkinEdit);

			if (!string.IsNullOrEmpty(Request.QueryString["path"])) {
				sTemplateFileQS = Request.QueryString["path"].ToString();
				sTemplateFile = CMSConfigHelper.DecodeBase64(sTemplateFileQS);
				sFullFilePath = HttpContext.Current.Server.MapPath(sTemplateFile);
				sEditFile = sFullFilePath;
			}

			if (!string.IsNullOrEmpty(Request.QueryString["alt"])) {
				string sAltFileQS = Request.QueryString["alt"].ToString();
				string sAltFile = CMSConfigHelper.DecodeBase64(sAltFileQS);
				sEditFile = HttpContext.Current.Server.MapPath(sAltFile);
			}

			litSkinFileName.Text = sTemplateFile;

			litEditFileName.Text = sEditFile.Replace(Server.MapPath("~"), @"\");

			if (File.Exists(sEditFile)) {
				if (!IsPostBack) {
					using (StreamReader sr = new StreamReader(sEditFile)) {
						txtPageContents.Text = sr.ReadToEnd();
					}
				}

				litDateMod.Text = File.GetLastWriteTime(sEditFile).ToString();

				if (sFullFilePath.LastIndexOf(@"\") > 0) {
					sDirectory = sFullFilePath.Substring(0, sFullFilePath.LastIndexOf(@"\"));
				} else {
					sDirectory = sFullFilePath.Substring(0, sFullFilePath.LastIndexOf(@"/"));
				}

				SetSourceFiles(sDirectory);

			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e) {
			if (File.Exists(sEditFile)) {
				Encoding encode = System.Text.Encoding.Default;

				using (var oWriter = new StreamWriter(sEditFile, false, encode)) {
					oWriter.Write(txtPageContents.Text);
					oWriter.Close();
				}

				Response.Redirect(SiteData.CurrentScriptName + "?" + Request.QueryString.ToString());
			}

		}

		public string EncodePath(string sIn) {
			if (!(sIn.StartsWith(@"\") || sIn.StartsWith(@"/"))) {
				sIn = @"/" + sIn;
			}
			return CMSConfigHelper.EncodeBase64(sIn.ToLower());
		}

		protected void SetSourceFiles(string sDir) {

			List<FileData> flsWorking = new List<FileData>();
			List<FileData> fldrWorking = new List<FileData>();

			List<string> lstFileExtensions = new List<string>();
			lstFileExtensions.Add(".css");
			lstFileExtensions.Add(".js");
			lstFileExtensions.Add(".ascx");

			if (Directory.Exists(sDir)) {

				string sDirParent = "";

				if (sDir.LastIndexOf(@"\") > 0) {
					sDirParent = sDir.Substring(0, sDir.LastIndexOf(@"\"));
				} else {
					sDirParent = sDir.Substring(0, sDir.LastIndexOf(@"/"));
				}

				FileData skinFolder = helpFile.GetFolderInfo("/", sDir);

				skinFolder.FolderPath = FileDataHelper.MakeWebFolderPath(sDir);

				fldrWorking = helpFile.SpiderDeepFoldersFD(FileDataHelper.MakeWebFolderPath(sDir));

				fldrWorking.Add(skinFolder);

				try {
					if (Directory.Exists(Server.MapPath("~/includes"))) {
						FileData incFolder = helpFile.GetFolderInfo("/", "/includes");
						fldrWorking.Add(incFolder);
					}
					if (Directory.Exists(Server.MapPath("~/js"))) {
						FileData incFolder = helpFile.GetFolderInfo("/", "/js");
						fldrWorking.Add(incFolder);
					}
					if (Directory.Exists(Server.MapPath("~/css"))) {
						FileData incFolder = helpFile.GetFolderInfo("/", "/css");
						fldrWorking.Add(incFolder);
					}
					//if (Directory.Exists(Server.MapPath("~/files"))) {
					//    FileData incFolder = helpFile.GetFolderInfo("/", Server.MapPath("files"));
					//    fldrWorking.Add(incFolder);
					//}
				} catch (Exception ex) { }

				helpFile.IncludeAllFiletypes();

				foreach (FileData f in fldrWorking) {
					List<FileData> fls = helpFile.GetFiles(f.FolderPath);

					flsWorking = (from m in flsWorking.Union(fls).ToList()
								  join e in lstFileExtensions on m.FileExtension.ToLower() equals e
								  select m).ToList();
				}

				flsWorking = flsWorking.Where(x => x.MimeType.StartsWith("text") && (x.FolderPath.ToLower().StartsWith("/manage/") == false)).ToList();

				rpFiles.DataSource = flsWorking.OrderBy(x => x.FileName).OrderBy(x => x.FolderPath).ToList();
				rpFiles.DataBind();
			}
		}



	}
}