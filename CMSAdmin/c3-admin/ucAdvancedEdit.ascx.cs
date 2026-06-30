using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using Carrotware.Web.UI.Controls;
using System;
using System.Linq;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ucAdvancedEdit : AdminBaseUserControl {
		public Guid GuidContentID { get; set; } = Guid.Empty;
		public UserEditState EditorPrefs { get; set; }
		public ContentPageType.PageType PageType { get; set; } = ContentPageType.PageType.Unknown;

		public string EditedPageFileName { get; set; } = string.Empty;
		public string EditUserName { get; set; } = string.Empty;
		public bool IsLocked { get; set; }

		public string AntiCache {
			get {
				return Helper.AntiCache;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
		}

		//protected void Page_Load(object sender, EventArgs e) {
		protected void Page_Init(object sender, EventArgs e) {
			this.GuidContentID = GetGuidIDFromQuery();

			this.EditorPrefs = UserEditState.cmsUserEditState;
			if (this.EditorPrefs == null) {
				this.EditorPrefs = new UserEditState();
				this.EditorPrefs.Init();
			}

			litCmsToolbarTitle.Text = string.Format("CarrotCake CMS {0}", SiteData.CurrentDLLMajorMinorVersion);

			string sCurrentPage = SiteData.CurrentScriptName;
			string sScrubbedURL = SiteData.AlternateCurrentScriptName;

			if (sScrubbedURL.ToLowerInvariant() != sCurrentPage.ToLowerInvariant()) {
				sCurrentPage = sScrubbedURL;
			}

			ContentPage pageContents = new ContentPage();
			if (this.GuidContentID == Guid.Empty) {
				pageContents = pageHelper.FindByFilename(SiteData.CurrentSiteID, sCurrentPage);
			} else {
				pageContents = pageHelper.FindContentByID(SiteData.CurrentSiteID, GuidContentID);
			}

			this.PageType = pageContents.ContentType;
			this.EditedPageFileName = pageContents.FileName;

			btnEditCoreInfo.Attributes["onclick"] = "cmsShowEditPageInfo();";

			if (pageContents.ContentType == ContentPageType.PageType.BlogEntry) {
				btnEditCoreInfo.Attributes["onclick"] = "cmsShowEditPostInfo();";
				btnSortChildPages.Visible = false;
			}

			if (cmsHelper.cmsAdminContent != null) {
				this.EditedPageFileName = cmsHelper.cmsAdminContent.FileName;
			}

			if (cmsHelper.ToolboxPlugins.Any()) {
				GeneralUtilities.BindRepeater(rpTools, cmsHelper.ToolboxPlugins);
			} else {
				rpTools.Visible = false;
			}

			this.IsLocked = pageHelper.IsPageLocked(pageContents.Root_ContentID, SiteData.CurrentSiteID, SecurityData.CurrentUserGuid);

			GeneralUtilities.BindList(ddlTemplate, cmsHelper.Templates);
			try { GeneralUtilities.SelectListValue(ddlTemplate, cmsHelper.cmsAdminContent.TemplateFile.ToLowerInvariant()); } catch { }

			this.Page.Header.Controls.Add(new AdminScriptInfo());

			if (!this.IsLocked) {
				foreach (Control c in plcIncludes.Controls) {
					this.Page.Header.Controls.Add(c);
				}

				LoadJQ();

				this.GuidContentID = pageContents.Root_ContentID;

				if (cmsHelper.cmsAdminContent == null) {
					pageContents.LoadAttributes();
					cmsHelper.cmsAdminContent = pageContents;
				} else {
					pageContents = cmsHelper.cmsAdminContent;
				}

				bool ret = pageHelper.RecordPageLock(pageContents.Root_ContentID, SiteData.CurrentSite.SiteID, SecurityData.CurrentUserGuid);

				cmsDivEditing.Visible = false;

				//BasicControlUtils.MakeXUACompatibleFirst(this.Page);
			} else {
				LoadJQ();

				pnlCMSEditZone.Visible = false;
				rpTools.Visible = false;
				btnToolboxSave1.Visible = false;
				btnToolboxSave2.Visible = false;
				btnToolboxSave3.Visible = false;
				btnTemplate.Visible = false;
				btnEditCoreInfo.Visible = false;
				cmsDivEditing.Visible = true;

				if (this.IsLocked && pageContents.Heartbeat_UserId != null) {
					var usr = SecurityData.GetProfileByUserID(pageContents.Heartbeat_UserId.Value);
					this.EditUserName = usr.UserName;
					litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.<br />" +
						" Click <b><a href=\"" + pageContents.FileName + "\">here</a></b> to return to the browse view.<br />";
				}
			}
		}

		protected void LoadJQ() {
			//this.Page.Header.Controls.Add(new AdminScriptInfo());
			var siteSkin = new CmsSkin() { WindowMode = CmsSkin.SkinMode.AdvEdit, SelectedColor = AdminBaseMasterPage.SiteSkin };
			this.Page.Header.Controls.Add(siteSkin);

			BasicControlUtils.InsertjQueryMain(this.Page);
			BasicControlUtils.InsertjQueryUI(this.Page);
		}

		protected string FlagSystemPlugin(object sysPlug) {
			if (sysPlug == null) return string.Empty;
			var isSystem = Convert.ToBoolean(sysPlug);

			return isSystem ? "ui-icon ui-icon-star" : "ui-icon ui-icon-tag";
		}
	}
}