using Carrotware.CMS.Core;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class CommentIndex : AdminBasePage {
		private ContentPageType.PageType pageType = ContentPageType.PageType.BlogEntry;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["type"])) {
				pageType = ContentPageType.GetTypeByName(Request.QueryString["type"].ToString());
			}

			if (pageType == ContentPageType.PageType.ContentEntry) {
				Master.ActivateTab(AdminBaseMasterPage.SectionID.PageComment);
			} else {
				Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogComment);
			}
		}
	}
}