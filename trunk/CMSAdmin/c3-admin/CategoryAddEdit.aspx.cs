using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
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
	public partial class CategoryAddEdit : AdminBasePage {
		public Guid guidItemID = Guid.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogCategory);
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidItemID = new Guid(Request.QueryString["id"].ToString());
			}

			if (!IsPostBack) {
				ContentCategory item = ContentCategory.Get(guidItemID);
				if (item != null) {
					txtSlug.Text = item.CategorySlug;
					txtLabel.Text = item.CategoryText;
					chkPublic.Checked = item.IsPublic;
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			ContentCategory item = ContentCategory.Get(guidItemID);

			if (item == null || (item != null && item.ContentCategoryID == Guid.Empty)) {
				item = new ContentCategory();
				item.SiteID = SiteData.CurrentSiteID;
				item.ContentCategoryID = Guid.NewGuid();
			}

			item.CategorySlug = txtSlug.Text;
			item.CategoryText = txtLabel.Text;
			item.IsPublic = chkPublic.Checked;

			item.Save();

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + item.ContentCategoryID.ToString());
		}

	}
}