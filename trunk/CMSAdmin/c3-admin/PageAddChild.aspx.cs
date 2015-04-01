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
		private ContentPage pageContents = null;

		protected void Page_Load(object sender, EventArgs e) {
			Master.UsesSaved = true;
			Master.HideSave();
			Master.SetSaveMessage("Page Created");

			guidContentID = GetGuidPageIDFromQuery();
			cmsHelper.OverrideKey(guidContentID);

			if (!IsPostBack) {
				ParentPagePicker.SelectedPage = guidContentID;
				lnkCreatePage.NavigateUrl = string.Format("{0}?pageid={1}", SiteData.CurrentScriptName, guidContentID);

				pnlAdd.Visible = true;
				pnlSaved.Visible = false;
			}
		}


		protected void btnSave_Click(object sender, EventArgs e) {
			pageContents = new ContentPage(SiteID, ContentPageType.PageType.ContentEntry);

			DateTime dtSite = CalcNearestFiveMinTime(SiteData.CurrentSite.Now);

			int iOrder = pageHelper.GetMaxNavOrder(SiteID) + 1;
			Guid? parentContentID = ParentPagePicker.SelectedPage;

			pageContents.Parent_ContentID = parentContentID;

			pageContents.TitleBar = txtTitle.Text;
			pageContents.NavMenuText = txtNav.Text;
			pageContents.PageHead = txtHead.Text;
			pageContents.FileName = txtFileName.Text;

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
			lnkNew.NavigateUrl = pageContents.FileName;

			if (pageContents.FileName.ToLower().EndsWith(SiteData.DefaultDirectoryFilename)) {
				VirtualDirectory.RegisterRoutes(true);
			}

			Master.ShowSave();
		}

	}
}
