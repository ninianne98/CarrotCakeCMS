using System;
using Carrotware.CMS.Core;

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

	public partial class CommentIndex : AdminBasePage {
		private ContentPageType.PageType pageType = ContentPageType.PageType.BlogEntry;

		protected void Page_Load(object sender, EventArgs e) {
			if (!String.IsNullOrEmpty(Request.QueryString["type"])) {
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