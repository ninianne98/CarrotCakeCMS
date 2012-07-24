using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class ucAdvancedEdit : BaseUserControl {

		public Guid guidContentID = Guid.Empty;
		public bool bLocked = false;

		public bool IsPageLocked(ContentPage cp) {

			bool bLock = false;
			if (cp.Heartbeat_UserId != null) {
				if (cp.Heartbeat_UserId != SiteData.CurrentUserGuid
						&& cp.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
					bLock = true;
				}
				if (cp.Heartbeat_UserId == SiteData.CurrentUserGuid
					|| cp.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public UserEditState EditorPrefs = null;

		protected void Page_Load(object sender, EventArgs e) {

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

			//if (!IsPostBack) {

			var pageContents = new ContentPage();
			if (guidContentID == Guid.Empty) {
				var pageName = SiteData.CurrentScriptName;
				pageContents = pageHelper.GetLatestContent(SiteData.CurrentSiteID, null, pageName);
			} else {
				pageContents = pageHelper.GetLatestContent(SiteData.CurrentSiteID, guidContentID);
			}

			if (cmsHelper.ToolboxPlugins.Count > 0) {
				rpTools.DataSource = cmsHelper.ToolboxPlugins;
				rpTools.DataBind();
			} else {
				rpTools.Visible = false;
			}

			bLocked = IsPageLocked(pageContents);

			ddlTemplate.DataSource = cmsHelper.Templates;
			ddlTemplate.DataBind();

			try { ddlTemplate.SelectedValue = cmsHelper.cmsAdminContent.TemplateFile.ToLower(); } catch { }

			if (!bLocked) {
				guidContentID = pageContents.Root_ContentID;

				if (cmsHelper.cmsAdminContent == null) {
					cmsHelper.cmsAdminContent = pageContents;
				} else {
					pageContents = cmsHelper.cmsAdminContent;
				}

				cmsDivEditing.Visible = false;

			} else {
				pnlCMSEditZone.Visible = false;
				rpTools.Visible = false;
				btnToolboxSave.Visible = false;
				btnTemplate.Visible = false;
				btnEditCoreInfo.Visible = false;
				cmsDivEditing.Visible = true;

				if (bLocked && pageContents.Heartbeat_UserId != null) {
					var usr = ProfileManager.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
					litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.<br />" +
						" Click <b><a href=\"" + pageContents.FileName + "\">here</a></b> to return to the browse view.<br />";
				}
			}
			//}

		}


		private string ParseEdit(string sContent) {
			string sFixed = "";
			if (!string.IsNullOrEmpty(sContent)) {
				sFixed = sContent;
				var b = "<!-- <#|BEGIN_CARROT_CMS|#> -->";
				var iB = sFixed.IndexOf(b);
				if (iB > 0) {
					sFixed = sFixed.Substring(iB + b.Length);

					var e = "<!-- <#|END_CARROT_CMS|#> -->";
					var iE = sFixed.IndexOf(e);
					sFixed = sFixed.Substring(0, iE);
				}
			}
			return sFixed.Trim();
		}


	}
}