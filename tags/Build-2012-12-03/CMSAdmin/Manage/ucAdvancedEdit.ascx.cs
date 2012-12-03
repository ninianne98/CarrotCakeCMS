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
* http://www.carrotware.com/
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

			ContentPage pageContents = new ContentPage();
			if (guidContentID == Guid.Empty) {
				pageContents = pageHelper.FindByFilename(SiteData.CurrentSiteID, SiteData.CurrentScriptName);
			} else {
				pageContents = pageHelper.FindContentByID(SiteData.CurrentSiteID, guidContentID);
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

				jqueryui link1 = new jqueryui();
				Page.Header.Controls.AddAt(0, link1);

				jquery link2 = new jquery();
				Page.Header.Controls.AddAt(0, link2);

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