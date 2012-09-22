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

namespace Carrotware.CMS.UI.Admin {
	public partial class PageIndex : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);

			SiteData site = siteHelper.GetCurrentSite();
			if (site == null) {
				Response.Redirect("./Default.aspx");
			}

			siteHelper.CleanUpSerialData();

			LoadGrid();
		}

		protected void LoadGrid() {

			var lstCont = pageHelper.GetLatestContentList(SiteID);

			gvPages.DataSource = lstCont;
			gvPages.DataBind();

		}

	}
}
