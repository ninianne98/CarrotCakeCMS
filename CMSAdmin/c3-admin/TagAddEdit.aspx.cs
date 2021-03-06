﻿using Carrotware.CMS.Core;
using System;

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

	public partial class TagAddEdit : AdminBasePage {
		public Guid guidItemID = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogTag);

			guidItemID = GetGuidIDFromQuery();

			if (!IsPostBack) {
				ContentTag item = ContentTag.Get(guidItemID);
				if (item != null) {
					txtSlug.Text = item.TagSlug;
					txtLabel.Text = item.TagText;
					chkPublic.Checked = item.IsPublic;
				} else {
					btnDeleteButton.Visible = false;
				}
			}
		}

		protected void btnDelete_Click(object sender, EventArgs e) {
			ContentTag item = ContentTag.Get(guidItemID);
			item.Delete();

			Response.Redirect(SiteFilename.TagIndexURL);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			ContentTag item = ContentTag.Get(guidItemID);

			if (item == null || (item != null && item.ContentTagID == Guid.Empty)) {
				item = new ContentTag();
				item.SiteID = SiteData.CurrentSiteID;
				item.ContentTagID = Guid.NewGuid();
			}

			item.TagSlug = txtSlug.Text;
			item.TagText = txtLabel.Text;
			item.IsPublic = chkPublic.Checked;

			item.Save();

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + item.ContentTagID.ToString());
		}
	}
}