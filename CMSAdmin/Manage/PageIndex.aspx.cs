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
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin {
	public partial class PageIndex : AdminBasePage {

		public string sTab = "pageidx-tabs-1";

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);

			if (!IsPostBack) {
				LoadGrid(hdnSort.Value);

				ddlTemplate.DataSource = cmsHelper.Templates;
				ddlTemplate.DataBind();

				SetTemplateGrid();
			}
		}

		protected void SetTemplateGrid() {
			gvApply.DataSource = (from c in pageHelper.GetLatestContentList(SiteID)
								  orderby c.TemplateFile
								  select c).ToList();

			gvApply.DataBind();

			foreach (GridViewRow dgItem in gvApply.Rows) {
				Image imgActive = (Image)dgItem.FindControl("imgActive");
				HiddenField hdnIsActive = (HiddenField)dgItem.FindControl("hdnIsActive");

				if (hdnIsActive.Value.ToLower() != "true") {
					imgActive.ImageUrl = hdnInactive.Value;
					imgActive.AlternateText = "Inactive";
				}
				imgActive.ToolTip = imgActive.AlternateText;
			}

		}


		protected void LoadGrid(string sSortKey) {


			var lstCont = (from c in pageHelper.GetLatestContentList(SiteID)
						   orderby c.NavOrder, c.PageHead
						   select c).ToList();

			LoadGridLive<ContentPage>(gvPages, hdnSort, lstCont, sSortKey);

			foreach (GridViewRow dgItem in gvPages.Rows) {
				Image imgActive = (Image)dgItem.FindControl("imgActive");
				HiddenField hdnIsActive = (HiddenField)dgItem.FindControl("hdnIsActive");

				if (hdnIsActive.Value.ToLower() != "true") {
					imgActive.ImageUrl = hdnInactive.Value;
					imgActive.AlternateText = "Inactive";
				}
				imgActive.ToolTip = imgActive.AlternateText;
			}

			gs.WalkGridForHeadings(gvPages);

		}


		protected void lblSort_Command(object sender, EventArgs e) {
			gs.DefaultSort = hdnSort.Value;
			gs.Sort = hdnSort.Value;
			LinkButton lb = (LinkButton)sender;
			string sSortField = "";
			try { sSortField = lb.CommandName.ToString(); } catch { }
			sSortField = gs.ResetSortToColumn(sSortField);
			LoadGrid(sSortField);
			hdnSort.Value = sSortField;

			sTab = "pageidx-tabs-1";
			SetTab();
		}

		protected void btnSaveMapping_Click(object sender, EventArgs e) {

			foreach (GridViewRow row in gvApply.Rows) {
				var chkReMap = (CheckBox)row.FindControl("chkReMap");

				if (chkReMap.Checked) {
					var hdnContentID = (HiddenField)row.FindControl("hdnContentID");
					Guid gRoot = new Guid(hdnContentID.Value);

					var cont = (from c in db.tblContents
								   where c.IsLatestVersion == true
								   where c.Root_ContentID == gRoot
								   select c).FirstOrDefault();

					cont.TemplateFile = ddlTemplate.SelectedValue;

				}

				db.SubmitChanges();
			}

			SetTemplateGrid();

			sTab = "pageidx-tabs-2";
			SetTab();

			//System.Web.HttpRuntime.UnloadAppDomain();
		}

		protected void SetTab() {

			var Msg2 = " setTimeout(\"SetClientTab(\'" + sTab + "\');\", 1200); ";   //SetTab('"+sTab+"');
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), Msg2, true);
		}

	}
}
