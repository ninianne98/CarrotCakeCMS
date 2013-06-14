using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
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
	public partial class ucAdvancedEdit : AdminBaseUserControl {

		public Guid guidContentID = Guid.Empty;
		public bool bLocked = false;
		public ContentPageType.PageType PageType = ContentPageType.PageType.Unknown;
		public UserEditState EditorPrefs = null;

		public string EditedPageFileName = "";

		protected void Page_Load(object sender, EventArgs e) {

			//jquerybasic jb = BasicControlUtils.FindjQuery(this.Page);
			//jquerybasic1.SelectedSkin = jquerybasic.jQueryTheme.NotUsed;   //jb.SelectedSkin;
			//jquerybasic1.JQVersion = jb.JQVersion;
		}

		public string GetWebControlUrl(string resource) {
			string sPath = "";
			try { sPath = jquerybasic.GetWebResourceUrl(resource); } catch { }
			return sPath;
		}

		//protected void Page_Load(object sender, EventArgs e) {
		protected void Page_Init(object sender, EventArgs e) {

			guidContentID = GetGuidIDFromQuery();

			EditorPrefs = UserEditState.cmsUserEditState;
			if (EditorPrefs == null) {
				EditorPrefs = new UserEditState();
				EditorPrefs.Init();
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
				GeneralUtilities.BindRepeater(rpTools, cmsHelper.ToolboxPlugins);
			} else {
				rpTools.Visible = false;
			}

			bLocked = pageHelper.IsPageLocked(pageContents);

			GeneralUtilities.BindList(ddlTemplate, cmsHelper.Templates);
			try { GeneralUtilities.SelectListValue(ddlTemplate, cmsHelper.cmsAdminContent.TemplateFile.ToLower()); } catch { }

			if (!bLocked) {
				foreach (Control c in plcIncludes.Controls) {
					this.Page.Header.Controls.Add(c);
				}

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

				bool bRet = pageHelper.RecordPageLock(pageContents.Root_ContentID, SiteData.CurrentSite.SiteID, SecurityData.CurrentUserGuid);

				cmsDivEditing.Visible = false;

				BasicControlUtils.MakeXUACompatibleFirst(this.Page);

			} else {
				pnlCMSEditZone.Visible = false;
				rpTools.Visible = false;
				btnToolboxSave1.Visible = false;
				btnToolboxSave2.Visible = false;
				btnToolboxSave3.Visible = false;
				btnTemplate.Visible = false;
				btnEditCoreInfo.Visible = false;
				cmsDivEditing.Visible = true;

				if (bLocked && pageContents.Heartbeat_UserId != null) {
					MembershipUser usr = SecurityData.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
					litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.<br />" +
						" Click <b><a href=\"" + pageContents.FileName + "\">here</a></b> to return to the browse view.<br />";
				}
			}
		}

	}
}