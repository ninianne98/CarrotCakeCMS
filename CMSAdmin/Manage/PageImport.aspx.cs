using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
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
	public partial class PageImport : AdminBasePage {
		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);

			lblWarning.Text = "";
			lblWarning.Attributes["style"] = "color: #000000;";

			siteHelper.CleanUpSerialData();
		}

		protected void btnUpload_Click(object sender, EventArgs e) {
			string sXML = "";
			if (upFile.HasFile) {
				using (StreamReader sr = new StreamReader(upFile.FileContent)) {
					sXML = sr.ReadToEnd();
				}
			}

			if (!string.IsNullOrEmpty(sXML)) {
				try {
					ContentPageExport cph = ContentPageExport.GetSerializedContentPageExport(sXML);
					ContentPageExport.AssignNewIDs(cph);
					ContentPageExport.SaveSerializedContentPageExport(cph);

					Response.Redirect("./PageAddEdit.aspx?importid=" + cph.NewRootContentID.ToString());
				} catch (Exception ex) {
					lblWarning.Text = ex.ToString();
					lblWarning.Attributes["style"] = "color: #990000;";
				}

			} else {
				lblWarning.Text = "No file appeared in the upload queue.";
				lblWarning.Attributes["style"] = "color: #990000;";
			}
		}




	}
}