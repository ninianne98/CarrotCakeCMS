using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
using System.IO;

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class ucEditNotifier : BaseUserControl {

		public string EditPageURL = "";
		public string PageIndexURL = "";
		public bool IsPageTemplate = false;

		protected SiteNavHelper navHelper = new SiteNavHelper();

		public Guid CurrentPageID {
			get;
			set;
		}

		protected void Page_Load(object sender, EventArgs e) {

			string sCurrentPage = SiteData.CurrentScriptName;
			string sScrubbedURL = SiteData.AlternateCurrentScriptName;

			if (sScrubbedURL.ToLower() != sCurrentPage.ToLower()) {
				sCurrentPage = sScrubbedURL;
			}

			ContentPage currentPage = pageHelper.FindByFilename(SiteData.CurrentSiteID, sCurrentPage);

			if (currentPage == null && SiteData.IsPageReal) {
				IsPageTemplate = true;
			}

			if ((SiteData.IsPageSampler || IsPageTemplate) && currentPage == null) {
				currentPage = ContentPageHelper.GetSamplerView();
			}

			litVersion.Text = SiteData.CarrotCakeCMSVersion;
			litRelease.Text = currentPage.GoLiveDate.ToString();
			litRetire.Text = currentPage.RetireDate.ToString();
			litTemplate.Text = currentPage.TemplateFile;

			CurrentPageID = currentPage.Root_ContentID;
			lnkCurrent.HRef = SiteData.CurrentScriptName;

			EditPageURL = SiteFilename.PageAddEditURL;
			PageIndexURL = SiteFilename.PageIndexURL;

			if (currentPage.ContentType == ContentPageType.PageType.BlogEntry) {
				EditPageURL = SiteFilename.BlogPostAddEditURL;
				PageIndexURL = SiteFilename.BlogPostIndexURL;
			}

			if (!IsPostBack) {
				List<SiteNav> nav = navHelper.GetChildNavigation(SiteData.CurrentSiteID, CurrentPageID, !SecurityData.IsAuthEditor);

				SiteNav pageContents2 = navHelper.GetParentPageNavigation(SiteData.CurrentSiteID, CurrentPageID);
				if (pageContents2 != null) {
					pageContents2.NavMenuText = "Parent: " + pageContents2.NavMenuText;
					pageContents2.NavOrder = -110;
					lnkParent.Visible = true;
					lnkParent.HRef = pageContents2.FileName;
				} else {
					lnkParent.Visible = false;
				}

				ContentPage homePage = pageHelper.FindHome(SiteData.CurrentSiteID);

				List<SiteNav> lstNavTop = null;
				if (homePage != null && homePage.Root_ContentID == CurrentPageID) {
					lstNavTop = (from n in navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor)
								 where n.Root_ContentID != CurrentPageID
								 orderby n.NavOrder
								 select new SiteNav {
									 NavOrder = n.NavOrder,
									 FileName = n.FileName,
									 NavMenuText = (n.NavOrder > 0 ? "  -- " : "") + n.FileName + "  [[" + (n.PageActive ? "" : "{*U*}  ") + n.NavMenuText + "]]",
									 PageActive = n.PageActive,
									 ContentID = n.ContentID,
									 Root_ContentID = n.Root_ContentID,
									 PageHead = n.PageHead,
									 SiteID = n.SiteID
								 }).ToList();
				}

				List<SiteNav> lstNav = (from n in nav
										orderby n.NavOrder
										select new SiteNav {
											NavOrder = n.NavOrder,
											FileName = n.FileName,
											NavMenuText = (n.NavOrder > 0 ? "  -- " : "") + n.FileName + "  [[" + (n.PageActive ? "" : "{*U*}  ") + n.NavMenuText + "]]",
											PageActive = n.PageActive,
											ContentID = n.ContentID,
											Root_ContentID = n.Root_ContentID,
											PageHead = n.PageHead,
											SiteID = n.SiteID
										}).ToList();


				if (lstNavTop != null) {
					lstNav = lstNavTop.Union(lstNav).ToList();
				}

				GeneralUtilities.BindListDefaultText(ddlCMSLinks, lstNav, null, "Navigate", "00000");

				if (lstNav.Count < 1) {
					ddlCMSLinks.Visible = false;
					lblChildDDL.Visible = false;
				}
			}

		}

	}
}