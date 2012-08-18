using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class ucEditNotifier : BaseUserControl {

		protected SiteNavHelper navHelper = new SiteNavHelper();

		public Guid CurrentPageID {
			get;
			set;
		}

		protected void Page_Load(object sender, EventArgs e) {
			var currentPage = pageHelper.GetLatestContent(SiteData.CurrentSiteID, null, SiteData.CurrentScriptName);
			CurrentPageID = currentPage.Root_ContentID;
			lnkCurrent.HRef = currentPage.FileName;

			if (!IsPostBack) {
				List<SiteNav> nav = navHelper.GetChildNavigation(SiteData.CurrentSiteID, CurrentPageID, !SecurityData.IsAuthEditor);

				//SiteNav pageContents1 = navHelper.GetPageNavigation(SiteData.CurrentSiteID, CurrentPageID);
				//if (pageContents1 != null) {
				//    pageContents1.NavMenuText = "Current: " + pageContents1.NavMenuText;
				//    pageContents1.NavOrder = -100;
				//    nav.Add(pageContents1);
				//}

				SiteNav pageContents2 = navHelper.GetParentPageNavigation(SiteData.CurrentSiteID, CurrentPageID);
				if (pageContents2 != null) {
					pageContents2.NavMenuText = "Parent: " + pageContents2.NavMenuText;
					pageContents2.NavOrder = -110;
					//nav.Add(pageContents2);
					lnkParent.Visible = true;
					lnkParent.HRef = pageContents2.FileName;
				} else {
					lnkParent.Visible = false;
				}

				var filePage = pageHelper.FindHome(SiteData.CurrentSiteID, null);

				List<SiteNav> lstNavTop = null;
				if (filePage != null && filePage.Root_ContentID == CurrentPageID) {
					lstNavTop = (from n in navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor)
								 where n.Root_ContentID != CurrentPageID
								 orderby n.NavOrder
								 select new SiteNav {
									 NavOrder = n.NavOrder,
									 NavFileName = n.NavFileName,
									 FileName = n.FileName,
									 NavMenuText = (n.NavOrder > 0 ? "  -- " : "") + n.NavFileName + "  [[" + (n.PageActive ? "" : "{*U*}  ") + n.NavMenuText + "]]",
									 PageActive = n.PageActive,
									 ContentID = n.ContentID,
									 Root_ContentID = n.Root_ContentID,
									 PageHead = n.PageHead,
									 SiteID = n.SiteID
								 }).ToList();
				}

				//SiteNav pageContents3 = new SiteNav();
				//pageContents3.PageActive = true;
				//pageContents3.ContentID = Guid.Empty;
				//pageContents3.Root_ContentID = Guid.Empty;
				//pageContents3.SiteID = SiteID;
				//pageContents3.FileName = "/default.aspx";
				//pageContents3.NavFileName = "/default.aspx";
				//pageContents3.NavMenuText = "Homepage";
				//pageContents3.PageHead = "Homepage";
				//pageContents3.NavOrder = -120;
				//nav.Add(pageContents3);

				List<SiteNav> lstNav = (from n in nav
										orderby n.NavOrder
										select new SiteNav {
											NavOrder = n.NavOrder,
											NavFileName = n.NavFileName,
											FileName = n.FileName,
											NavMenuText = (n.NavOrder > 0 ? "  -- " : "") + n.NavFileName + "  [[" + (n.PageActive ? "" : "{*U*}  ") + n.NavMenuText + "]]",
											PageActive = n.PageActive,
											ContentID = n.ContentID,
											Root_ContentID = n.Root_ContentID,
											PageHead = n.PageHead,
											SiteID = n.SiteID
										}).ToList();


				if (lstNavTop != null) {
					lstNav = lstNavTop.Union(lstNav).ToList();
				}


				ddlCMSLinks.DataSource = lstNav;
				ddlCMSLinks.DataBind();

				ddlCMSLinks.Items.Insert(0, new ListItem("-Navigate-", "00000"));

				if (lstNav.Count < 1) {
					ddlCMSLinks.Visible = false;
					lblChildDDL.Visible = false;
				}
			}

		}



	}
}