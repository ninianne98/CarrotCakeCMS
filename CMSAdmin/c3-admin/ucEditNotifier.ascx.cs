using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ucEditNotifier : AdminBaseUserControl {
		public string EditPageURL { get; set; } = string.Empty;
		public string AdvEditPageURL { get; set; } = string.Empty;
		public string PageIndexURL { get; set; } = string.Empty;
		public string PageIndexText { get; set; } = string.Empty;

		public string AntiCache {
			get {
				return Helper.AntiCache;
			}
		}

		public bool IsPageTemplate { get; set; } = false;
		public Guid CurrentPageID { get; set; } = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			siteSkin.SelectedColor = AdminBaseMasterPage.SiteSkin;

			using (ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper()) {
				string sCurrentPage = SiteData.CurrentScriptName;
				string sScrubbedURL = SiteData.AlternateCurrentScriptName;

				if (sScrubbedURL.ToLowerInvariant() != sCurrentPage.ToLowerInvariant()) {
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

				this.CurrentPageID = currentPage.Root_ContentID;
				lnkCurrent.HRef = SiteData.CurrentScriptName;

				this.AdvEditPageURL = string.Format("{0}?{1}=true", SiteData.AlternateCurrentScriptName, SiteData.AdvancedEditParameter);

				this.EditPageURL = string.Format("{0}?id={1}", SiteFilename.PageAddEditURL, this.CurrentPageID);
				this.PageIndexURL = SiteFilename.PageIndexURL;
				this.PageIndexText = "CONTENT INDEX";

				if (currentPage.ContentType == ContentPageType.PageType.BlogEntry) {
					this.EditPageURL = string.Format("{0}?id={1}", SiteFilename.BlogPostAddEditURL, this.CurrentPageID);
					this.PageIndexURL = SiteFilename.BlogPostIndexURL;
					this.PageIndexText = "BLOG INDEX";
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
}