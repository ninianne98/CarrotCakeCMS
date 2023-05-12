﻿using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

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

	public partial class ContentSnippetAddEdit : AdminBasePage {
		public Guid guidItemID = Guid.Empty;
		public Guid guidRootItemID = Guid.Empty;
		public Guid guidVersionItemID = Guid.Empty;
		public bool bLocked = false;
		private string sPageMode = string.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentSnippet);

			guidRootItemID = GetGuidIDFromQuery();
			guidVersionItemID = GetGuidParameterFromQuery("versionid");

			guidItemID = guidRootItemID;

			if (guidItemID == Guid.Empty) {
				guidItemID = guidVersionItemID;
			}

			if (!IsPostBack) {
				DateTime dtSite = CMSConfigHelper.CalcNearestFiveMinTime(SiteData.CurrentSite.Now);
				ucReleaseDate.SetDate(dtSite);
				ucRetireDate.SetDate(dtSite.AddYears(200));

				ContentSnippet item = null;

				if (guidRootItemID != Guid.Empty) {
					item = ContentSnippet.Get(guidRootItemID);
				} else {
					item = ContentSnippet.GetVersion(guidVersionItemID);
				}

				if (item != null) {
					bool bRet = item.RecordSnippetLock(SecurityData.CurrentUserGuid);
					bLocked = item.IsSnippetLocked();

					if (bLocked && item.Heartbeat_UserId != null) {
						MembershipUser usr = SecurityData.GetUserByGuid(item.Heartbeat_UserId.Value);
						litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the snippet.";
					}

					txtSlug.Text = item.ContentSnippetSlug;
					txtLabel.Text = item.ContentSnippetName;
					reBody.Text = item.ContentBody;
					guidRootItemID = item.Root_ContentSnippetID;

					ucReleaseDate.SetDate(item.GoLiveDate);
					ucRetireDate.SetDate(item.RetireDate);

					chkPublic.Checked = item.ContentSnippetActive;
					btnDeleteButton.Visible = true;
					pnlReview.Visible = true;

					Dictionary<string, string> dataVersions = (from v in ContentSnippet.GetHistory(guidRootItemID)
															   join u in ExtendedUserData.GetUserList() on v.EditUserId equals u.UserId
															   orderby v.EditDate descending
															   select new KeyValuePair<string, string>(v.ContentSnippetID.ToString(), string.Format("{0} ({1}) {2}", v.EditDate, u.UserName, (v.IsLatestVersion ? " [**] " : " ")))
															   ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					GeneralUtilities.BindListDefaultText(ddlVersions, dataVersions, null, "Versions", "00000");
				} else {
					btnDeleteButton.Visible = false;
					pnlReview.Visible = false;
				}
			}

			sPageMode = GetStringParameterFromQuery("mode");
			if (SiteData.IsRawMode(sPageMode)) {
				reBody.CssClass = "rawEditor";
				divCenter.Visible = false;
			}

			pnlHB.Visible = !bLocked;
			pnlHBEmpty.Visible = bLocked;
			pnlButtons.Visible = !bLocked;
			divEditing.Visible = bLocked;
		}

		protected void btnDelete_Click(object sender, EventArgs e) {
			ContentSnippet item = ContentSnippet.Get(guidRootItemID);
			item.Delete();

			Response.Redirect(SiteFilename.ContentSnippetIndexURL);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			ContentSnippet item = null;

			if (guidRootItemID != Guid.Empty) {
				item = ContentSnippet.Get(guidRootItemID);
			} else {
				item = ContentSnippet.GetVersion(guidVersionItemID);
			}

			if (item == null || (item != null && item.Root_ContentSnippetID == Guid.Empty)) {
				item = new ContentSnippet(SiteData.CurrentSiteID);
				item.CreateUserId = SecurityData.CurrentUserGuid;
			}

			item.ContentSnippetSlug = txtSlug.Text;
			item.ContentSnippetName = txtLabel.Text;
			item.ContentBody = reBody.Text;
			item.EditUserId = SecurityData.CurrentUserGuid;
			item.ContentSnippetActive = chkPublic.Checked;

			item.GoLiveDate = ucReleaseDate.GetDate();
			item.RetireDate = ucRetireDate.GetDate();

			item.Save();

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + item.Root_ContentSnippetID.ToString());
		}
	}
}