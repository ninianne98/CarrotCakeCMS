using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;
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
	public partial class PageAddChild : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		ContentPage pageContents = null;
		ContentPage parentPageContents = null;
		public bool bAddTopLevelPage = false;

		protected void Page_Load(object sender, EventArgs e) {

			guidContentID = GetGuidPageIDFromQuery();

			if (!string.IsNullOrEmpty(Request.QueryString["addtoplevel"])) {
				bAddTopLevelPage = Convert.ToBoolean(Request.QueryString["addtoplevel"].ToString());
			}

			cmsHelper.OverrideKey(guidContentID);

			if (cmsHelper.cmsAdminContent != null) {
				parentPageContents = cmsHelper.cmsAdminContent;
			}

			if (!IsPostBack) {
				pnlAdd.Visible = true;
				pnlSaved.Visible = false;
			}
		}


		protected void btnSave_Click(object sender, EventArgs e) {
			pageContents = new ContentPage(SiteData.CurrentSiteID, ContentPageType.PageType.ContentEntry);

			DateTime dtSite = CalcNearestFiveMinTime(SiteData.CurrentSite.Now);

			Guid? parentContentID = null;
			int iOrder = pageHelper.GetMaxNavOrder(SiteData.CurrentSite.SiteID) + 2;

			if (!bAddTopLevelPage) {
				if (parentPageContents != null) {
					parentContentID = parentPageContents.Root_ContentID;
					iOrder = parentPageContents.NavOrder + 2;
				}
			}

			pageContents.Root_ContentID = Guid.NewGuid();
			pageContents.ContentID = pageContents.Root_ContentID;
			pageContents.Parent_ContentID = parentContentID;

			pageContents.SiteID = SiteData.CurrentSiteID;

			pageContents.TemplateFile = SiteData.DefaultTemplateFilename;

			pageContents.TitleBar = txtTitle.Text;
			pageContents.NavMenuText = txtNav.Text;
			pageContents.PageHead = txtHead.Text;
			pageContents.FileName = txtFileName.Text;

			pageContents.RightPageText = "<p>&nbsp;</p>";
			pageContents.LeftPageText = "<p>&nbsp;</p>";
			pageContents.PageText = "<p>&nbsp;</p>";

			pageContents.MetaDescription = txtDescription.Text;
			pageContents.MetaKeyword = txtKey.Text;

			pageContents.Heartbeat_UserId = SecurityData.CurrentUserGuid;
			pageContents.EditHeartbeat = dtSite.AddMinutes(5);

			pageContents.EditUserId = SecurityData.CurrentUserGuid;
			pageContents.IsLatestVersion = true;
			pageContents.EditDate = SiteData.CurrentSite.Now;
			pageContents.NavOrder = iOrder;
			pageContents.PageActive = false;
			pageContents.ShowInSiteMap = true;
			pageContents.ShowInSiteNav = true;
			pageContents.ContentType = ContentPageType.PageType.ContentEntry;

			pageContents.RetireDate = dtSite.AddYears(200);
			pageContents.GoLiveDate = dtSite;

			pageContents.SavePageEdit();

			pnlAdd.Visible = false;
			pnlSaved.Visible = true;

			litPageName.Text = pageContents.FileName;

			if (pageContents.FileName.ToLower().EndsWith(SiteData.DefaultDirectoryFilename)) {
				VirtualDirectory.RegisterRoutes(true);
			}
		}

	}
}
