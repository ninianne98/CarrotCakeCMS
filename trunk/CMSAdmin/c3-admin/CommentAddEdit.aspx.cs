using System;
using System.Collections.Generic;
using System.Linq;
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


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class CommentAddEdit : AdminBasePage {
		protected void Page_Load(object sender, EventArgs e) {

			ucCommentAddEdit1.FetchItem();

			if (ucCommentAddEdit1.pageType == ContentPageType.PageType.BlogEntry) {
				Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogComment);
			}

			if (ucCommentAddEdit1.pageType == ContentPageType.PageType.ContentEntry) {
				Master.ActivateTab(AdminBaseMasterPage.SectionID.PageComment);
			}
		}



	}
}