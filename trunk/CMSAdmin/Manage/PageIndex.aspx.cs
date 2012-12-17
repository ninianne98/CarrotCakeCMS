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

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class PageIndex : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentAdd);

			SiteData site = siteHelper.GetCurrentSite();
			if (site == null) {
				Response.Redirect("./Default.aspx");
			}

			siteHelper.CleanUpSerialData();

			SetGrid(rdoFilterResults2.Checked, ParentPagePicker.SelectedPage);
		}


		protected void SetGrid(bool bAll, Guid? guidParentID) {
			List<ContentPage> lstContent = null;
			if (bAll) {
				lstContent = pageHelper.GetAllLatestContentList(SiteID);
			} else {
				if (guidParentID == null) {
					lstContent = pageHelper.GetTopNavigation(SiteID, false);
				} else {
					lstContent = pageHelper.GetParentWithChildNavigation(SiteID, guidParentID, false);
				}
			}
			gvPages.DataSource = lstContent;
			gvPages.DataBind();
		}


		protected void btnFilter_Click(object sender, EventArgs e) {
			LoadGrid();
		}

		protected void rdoFilterResults_CheckedChanged(object sender, EventArgs e) {
			LoadGrid();
		}

		private void LoadGrid() {
			trFilter.Attributes["style"] = "display:none;";

			if (rdoFilterResults2.Checked) {
				trFilter.Attributes["style"] = "display:none;";
				SetGrid(true, Guid.Empty);
			} else {
				trFilter.Attributes["style"] = "";
				SetGrid(false, ParentPagePicker.SelectedPage);
			}
		}

	}
}
