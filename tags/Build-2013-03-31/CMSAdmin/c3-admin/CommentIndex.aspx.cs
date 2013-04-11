using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
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