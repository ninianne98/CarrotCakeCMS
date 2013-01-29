﻿using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;
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
	public partial class ucAdvancedEdit : BaseUserControl {

		public Guid guidContentID = Guid.Empty;
		public bool bLocked = false;
		public ContentPageType.PageType PageType = ContentPageType.PageType.Unknown;
		public UserEditState EditorPrefs = null;

		public string EditedPageFileName = "";

		//protected void Page_Load(object sender, EventArgs e) {
		protected void Page_Init(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidContentID = new Guid(Request.QueryString["id"].ToString());
			}

			EditorPrefs = UserEditState.cmsUserEditState;
			if (EditorPrefs == null) {
				EditorPrefs = new UserEditState();
				EditorPrefs.EditorMargin = "L";
				EditorPrefs.EditorOpen = "true";
				EditorPrefs.EditorScrollPosition = "0";
				EditorPrefs.EditorSelectedTabIdx = "0";
			}

			string sCurrentPage = SiteData.CurrentScriptName;
			string sScrubbedURL = SiteData.AlternateCurrentScriptName;

			if (sScrubbedURL.ToLower() != sCurrentPage.ToLower()) {
				sCurrentPage = sScrubbedURL;
			}

			ContentPage pageContents = new ContentPage();
			if (guidContentID == Guid.Empty) {
				pageContents = pageHelper.FindByFilename(SiteData.CurrentSiteID, sCurrentPage);
			} else {
				pageContents = pageHelper.FindContentByID(SiteData.CurrentSiteID, guidContentID);
			}

			PageType = pageContents.ContentType;
			EditedPageFileName = pageContents.FileName;

			btnEditCoreInfo.Attributes["onclick"] = "cmsShowEditPageInfo();";

			if (pageContents.ContentType == ContentPageType.PageType.BlogEntry) {
				btnEditCoreInfo.Attributes["onclick"] = "cmsShowEditPostInfo();";
				btnAddChild.Visible = false;
				btnSortChildPages.Visible = false;
			}

			if (cmsHelper.cmsAdminContent != null) {
				EditedPageFileName = cmsHelper.cmsAdminContent.FileName;
			}

			if (cmsHelper.ToolboxPlugins.Count > 0) {
				rpTools.DataSource = cmsHelper.ToolboxPlugins;
				rpTools.DataBind();
			} else {
				rpTools.Visible = false;
			}

			bLocked = pageHelper.IsPageLocked(pageContents);

			ddlTemplate.DataSource = cmsHelper.Templates;
			ddlTemplate.DataBind();

			try { ddlTemplate.SelectedValue = cmsHelper.cmsAdminContent.TemplateFile.ToLower(); } catch { }

			if (!bLocked) {

				//jquerybasic jquerybasic2 = new jquerybasic();
				//jquerybasic2.SelectedSkin = jquerybasic.jQueryTheme.NotUsed;
				//Page.Header.Controls.AddAt(0, jquerybasic2);

				BasicControlUtils.InsertjQuery(this.Page);

				guidContentID = pageContents.Root_ContentID;

				if (cmsHelper.cmsAdminContent == null) {
					pageContents.LoadAttributes();
					cmsHelper.cmsAdminContent = pageContents;
				} else {
					pageContents = cmsHelper.cmsAdminContent;
				}

				cmsDivEditing.Visible = false;

				BasicControlUtils.MakeXUACompatibleFirst(this.Page);

			} else {
				pnlCMSEditZone.Visible = false;
				rpTools.Visible = false;
				btnToolboxSave1.Visible = false;
				btnToolboxSave2.Visible = false;
				btnTemplate.Visible = false;
				btnEditCoreInfo.Visible = false;
				cmsDivEditing.Visible = true;

				if (bLocked && pageContents.Heartbeat_UserId != null) {
					var usr = SecurityData.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
					litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.<br />" +
						" Click <b><a href=\"" + pageContents.FileName + "\">here</a></b> to return to the browse view.<br />";
				}
			}
		}

	}
}