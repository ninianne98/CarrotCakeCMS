using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
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
namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class ucAdvancedEdit : BaseUserControl {

		public Guid guidContentID = Guid.Empty;
		public bool bLocked = false;

		public bool IsPageLocked(ContentPage cp) {

			bool bLock = false;
			if (cp.Heartbeat_UserId != null) {
				if (cp.Heartbeat_UserId != CurrentUserGuid
						&& cp.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
					bLock = true;
				}
				if (cp.Heartbeat_UserId == CurrentUserGuid
					|| cp.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}


		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidContentID = new Guid(Request.QueryString["id"].ToString());
			}

			if (!IsPostBack) {

				var pageContents = new ContentPage();
				if (guidContentID == Guid.Empty) {
					var pageName = CurrentScriptName; //pageHelper.StripSiteFolder(CurrentScriptName);
					pageContents = pageHelper.GetLatestContent(SiteID, null, pageName);
				} else {
					pageContents = pageHelper.GetLatestContent(SiteID, guidContentID);
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

					txtCenter.Text = ParseEdit(pageContents.PageText);
					txtLeft.Text = ParseEdit(pageContents.LeftPageText);
					txtRight.Text = ParseEdit(pageContents.RightPageText);

					divEditing.Visible = false;

				} else {
					rpTools.Visible = false;
					btnToolboxSave.Visible = false;
					divEditing.Visible = true;

					if (bLocked && pageContents.Heartbeat_UserId != null) {
						var usr = ProfileManager.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
						litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.";
					}
				}
			}

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


		protected void btnSave_Click(object sender, EventArgs e) {
			var pageContents = pageHelper.GetLatestContent(SiteID, guidContentID);

			pageContents.PageText = reBody.Text;
		}


	}
}