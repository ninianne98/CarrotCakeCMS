﻿using System;
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


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class CategoryIndex : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogCategory);
			SiteData site = siteHelper.GetCurrentSite();

			gvPages.DataSource = site.GetCategoryList();
			gvPages.DataBind();
		}

	}
}