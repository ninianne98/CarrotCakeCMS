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
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class SiteSkinEdit : AdminBasePage {
		CMSConfigHelper configHelper = new CMSConfigHelper();
		string sTemplateFileQS = String.Empty;
		string sTemplateFile = String.Empty;
		string sFullFilePath = String.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);

			if (!string.IsNullOrEmpty(Request.QueryString["path"])) {
				sTemplateFileQS = Request.QueryString["path"].ToString();
				sTemplateFile = configHelper.DecodeBase64(sTemplateFileQS);
				sFullFilePath = HttpContext.Current.Server.MapPath(sTemplateFile);
			}

			litFileName.Text = sTemplateFile;

			if (!IsPostBack) {
				if (File.Exists(sFullFilePath)) {
					using (StreamReader sr = new StreamReader(sFullFilePath)) {
						txtPageContents.Text = sr.ReadToEnd();
					}
				}
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e) {
			if (File.Exists(sFullFilePath)) {
				Encoding encode = System.Text.Encoding.Default;

				using (var oWriter = new StreamWriter(sFullFilePath, false, encode)) {
					oWriter.Write(txtPageContents.Text);
					oWriter.Close();
				}

				Response.Redirect(SiteData.CurrentScriptName + "?path=" + sTemplateFileQS);
			}
		}

	}
}