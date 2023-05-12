using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using Carrotware.Web.UI.Controls;
using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ucAdvancedEdit : AdminBaseUserControl {
		public Guid guidContentID = Guid.Empty;
		public bool bLocked = false;
		public ContentPageType.PageType PageType = ContentPageType.PageType.Unknown;
		public UserEditState EditorPrefs = null;

		public string EditedPageFileName = string.Empty;
		public string EditUserName = string.Empty;

		public string AntiCache {
			get {
				return Helper.AntiCache;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
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

			if (sScrubbedURL.ToLowerInvariant() != sCurrentPage.ToLowerInvariant()) {
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
				btnSortChildPages.Visible = false;
			}

			if (cmsHelper.cmsAdminContent != null) {
				EditedPageFileName = cmsHelper.cmsAdminContent.FileName;
			}

			if (cmsHelper.ToolboxPlugins.Any()) {
				GeneralUtilities.BindRepeater(rpTools, cmsHelper.ToolboxPlugins);
			} else {
				rpTools.Visible = false;
			}

			bLocked = pageHelper.IsPageLocked(pageContents.Root_ContentID, SiteData.CurrentSiteID, SecurityData.CurrentUserGuid);

			GeneralUtilities.BindList(ddlTemplate, cmsHelper.Templates);
			try { GeneralUtilities.SelectListValue(ddlTemplate, cmsHelper.cmsAdminContent.TemplateFile.ToLowerInvariant()); } catch { }

			if (!bLocked) {
				foreach (Control c in plcIncludes.Controls) {
					this.Page.Header.Controls.Add(c);
				}

				//jquerybasic jquerybasic2 = new jquerybasic();
				//jquerybasic2.SelectedSkin = jquerybasic.jQueryTheme.NotUsed;
				//Page.Header.Controls.AddAt(0, jquerybasic2);

				//BasicControlUtils.InsertjQuery(this.Page);

				BasicControlUtils.InsertjQueryMain(this.Page);
				BasicControlUtils.InsertjQueryUI(this.Page);

				guidContentID = pageContents.Root_ContentID;

				if (cmsHelper.cmsAdminContent == null) {
					pageContents.LoadAttributes();
					cmsHelper.cmsAdminContent = pageContents;
				} else {
					pageContents = cmsHelper.cmsAdminContent;
				}

				bool bRet = pageHelper.RecordPageLock(pageContents.Root_ContentID, SiteData.CurrentSite.SiteID, SecurityData.CurrentUserGuid);

				cmsDivEditing.Visible = false;

				//BasicControlUtils.MakeXUACompatibleFirst(this.Page);
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
					EditUserName = usr.UserName;
					litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.<br />" +
						" Click <b><a href=\"" + pageContents.FileName + "\">here</a></b> to return to the browse view.<br />";
				}
			}
		}
	}
}