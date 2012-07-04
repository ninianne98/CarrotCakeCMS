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
	public partial class PageAddEdit : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		string sPageMode = String.Empty;
		SiteMapOrder orderHelper = new SiteMapOrder();

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);
			lblUpdated.Text = DateTime.Now.ToString();

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidContentID = new Guid(Request.QueryString["id"].ToString());
			}

			if (!string.IsNullOrEmpty(Request.QueryString["mode"])) {
				sPageMode = Request.QueryString["mode"].ToString();
				if (sPageMode.ToLower() == "raw") {
					reBody.CssClass = "rawEditor";
					reLeftBody.CssClass = "rawEditor";
					reRightBody.CssClass = "rawEditor";
					divCenter.Visible = false;
					divRight.Visible = false;
					divLeft.Visible = false;
				}
			}

			if (!IsPostBack) {

				var lstContent = pageHelper.GetLatestContentList(SiteID);

				ddlTemplate.DataSource = cmsHelper.Templates;
				ddlTemplate.DataBind();

				var pageContents = pageHelper.GetLatestContent(SiteID, guidContentID);

				divEditing.Visible = false;

				if (pageContents != null) {

					var bLocked = IsPageLocked(pageContents);

					pnlHB.Visible = !bLocked;
					btnSaveButton.Visible = !bLocked;
					divEditing.Visible = bLocked;

					if (bLocked && pageContents.Heartbeat_UserId != null) {
						var usr = ProfileManager.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
						litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.";
					}

					txtTitle.Text = pageContents.TitleBar;
					txtNav.Text = pageContents.NavMenuText;
					txtHead.Text = pageContents.PageHead;
					txtFileName.Text = pageContents.FileName;

					txtDescription.Text = pageContents.MetaDescription;
					txtKey.Text = pageContents.MetaKeyword;

					txtSort.Text = pageContents.NavOrder.ToString();

					lblUpdated.Text = pageContents.EditDate.ToString();

					reBody.Text = pageContents.PageText;
					reLeftBody.Text = pageContents.LeftPageText;
					reRightBody.Text = pageContents.RightPageText;

					chkActive.Checked = Convert.ToBoolean(pageContents.PageActive);

					if (pageContents.Parent_ContentID != null) {
						txtParent.Text = pageContents.Parent_ContentID.ToString();
					}
					if (pageContents.TemplateFile != null) {
						try { ddlTemplate.SelectedValue = pageContents.TemplateFile.ToLower(); } catch { }
					}

				}
			}


			if (string.IsNullOrEmpty(reBody.Text)) {
				reBody.Text = "<p>&nbsp;</p>";
			}
			if (string.IsNullOrEmpty(reLeftBody.Text)) {
				reLeftBody.Text = "<p>&nbsp;</p>";
			}
			if (string.IsNullOrEmpty(reRightBody.Text)) {
				reRightBody.Text = "<p>&nbsp;</p>";
			}

		}


		protected void btnSave_Click(object sender, EventArgs e) {

			var pageContents = pageHelper.GetLatestContent(SiteID, guidContentID);

			if (pageContents == null) {
				pageContents = new ContentPage();
				pageContents.Root_ContentID = Guid.NewGuid();
				pageContents.ContentID = pageContents.ContentID;
				pageContents.SiteID = SiteID;
			}

			pageContents.IsLatestVersion = true;

			pageContents.TemplateFile = ddlTemplate.SelectedValue;

			pageContents.TitleBar = txtTitle.Text;
			pageContents.NavMenuText = txtNav.Text;
			pageContents.PageHead = txtHead.Text;
			pageContents.FileName = txtFileName.Text;

			pageContents.MetaDescription = txtDescription.Text;
			pageContents.MetaKeyword = txtKey.Text;

			pageContents.EditDate = DateTime.Now;
			pageContents.NavOrder = int.Parse(txtSort.Text);

			pageContents.PageText = reBody.Text;
			pageContents.LeftPageText = reLeftBody.Text;
			pageContents.RightPageText = reRightBody.Text;

			pageContents.PageActive = chkActive.Checked;

			if (txtParent.Text.Length > 5) {
				pageContents.Parent_ContentID = new Guid(txtParent.Text);
			} else {
				pageContents.Parent_ContentID = null;
			}

			pageContents.EditUserId = SiteData.CurrentUserGuid;

			pageContents.SavePageEdit();

			if (sPageMode.Length < 1) {
				Response.Redirect(SiteData.CurrentScriptName + "?id=" + pageContents.Root_ContentID.ToString());
			} else {
				Response.Redirect(SiteData.CurrentScriptName + "?mode=raw&id=" + pageContents.Root_ContentID.ToString());
			}
		}


	}
}
