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
	public partial class PageTemplateUpdate : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentTemplate);

			SiteData site = siteHelper.GetCurrentSite();
			if (site == null) {
				Response.Redirect("./Default.aspx");
			}

			if (!IsPostBack) {

				ddlTemplate.DataSource = cmsHelper.Templates;
				ddlTemplate.DataBind();

				SetTemplateGrid();
			}
		}

		protected void SetTemplateGrid() {
			gvApply.DataSource = pageHelper.GetAllLatestContentList(SiteID);
			gvApply.DataBind();

		}



		protected void btnSaveMapping_Click(object sender, EventArgs e) {
			List<Guid> lstUpd = new List<Guid>();

			foreach (GridViewRow row in gvApply.Rows) {
				var chkReMap = (CheckBox)row.FindControl("chkReMap");

				if (chkReMap.Checked) {
					var hdnContentID = (HiddenField)row.FindControl("hdnContentID");
					Guid gRoot = new Guid(hdnContentID.Value);

					lstUpd.Add(gRoot);
				}				
			}

			pageHelper.BulkUpdateTemplate(SiteData.CurrentSiteID, lstUpd, ddlTemplate.SelectedValue);

			SetTemplateGrid();
		}

	}
}
