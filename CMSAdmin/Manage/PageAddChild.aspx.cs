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

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class PageAddChild : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		ContentPage pageContents = null;
		ContentPage parentPageContents = null;

		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["pageid"])) {
				guidContentID = new Guid(Request.QueryString["pageid"].ToString());
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
			pageContents = new ContentPage();

			if (parentPageContents != null) {
				pageContents.Root_ContentID = Guid.NewGuid();
				pageContents.ContentID = pageContents.Root_ContentID;
				pageContents.Parent_ContentID = parentPageContents.Root_ContentID;

				pageContents.SiteID = SiteData.CurrentSiteID;

				pageContents.TemplateFile = parentPageContents.TemplateFile;

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
				pageContents.EditHeartbeat = DateTime.Now;

				pageContents.EditUserId = SecurityData.CurrentUserGuid;
				pageContents.IsLatestVersion = true;
				pageContents.EditDate = DateTime.Now;
				pageContents.NavOrder = parentPageContents.NavOrder + 1;
				pageContents.PageActive = false;
				pageContents.ContentType = ContentPageType.PageType.ContentEntry;

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
}
