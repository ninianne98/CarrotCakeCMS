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
	public partial class BlogPostTemplateUpdate : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogTemplate);

			SiteData site = siteHelper.GetCurrentSite();
			if (site == null) {
				Response.Redirect("./Default.aspx");
			}

			if (!IsPostBack) {
				txtDate.Text = SiteData.CurrentSite.Now.ToShortDateString();
				ddlTemplate.DataSource = cmsHelper.Templates;
				ddlTemplate.DataBind();

				LoadGrid();
			}
		
		}


		protected void SetGrid(bool bAll, DateTime dateRange, int dateRangeDays) {
			List<ContentPage> lstContent = null;
			if (bAll) {
				lstContent = pageHelper.GetAllLatestBlogList(SiteID);
			} else {
				// use date range
				lstContent = pageHelper.GetPostsByDateRange(SiteID, dateRange, dateRangeDays, false);
			}
			gvPages.DataSource = lstContent;
			gvPages.DataBind();
		}


		protected void btnSaveMapping_Click(object sender, EventArgs e) {
			List<Guid> lstUpd = new List<Guid>();

			foreach (GridViewRow row in gvPages.Rows) {
				var chkReMap = (CheckBox)row.FindControl("chkReMap");

				if (chkReMap.Checked) {
					var hdnContentID = (HiddenField)row.FindControl("hdnContentID");
					Guid gRoot = new Guid(hdnContentID.Value);

					lstUpd.Add(gRoot);
				}
			}

			pageHelper.BulkUpdateTemplate(SiteData.CurrentSiteID, lstUpd, ddlTemplate.SelectedValue);

			LoadGrid();
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
				SetGrid(true, SiteData.CurrentSite.Now, 0);
			} else {
				trFilter.Attributes["style"] = "";
				SetGrid(false, Convert.ToDateTime(txtDate.Text), int.Parse(ddlDateRange.SelectedValue));
			}
		}

	}
}
