using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class PageExport : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		public Guid guidNodeID = Guid.Empty;
		public DateTime dateBegin = DateTime.MinValue;
		public DateTime dateEnd = DateTime.MaxValue;
		public SiteExport.ExportType ExportWhat = SiteExport.ExportType.AllData;
		public bool bExportComments = false;

		protected void Page_Load(object sender, EventArgs e) {
			guidContentID = GetGuidIDFromQuery();

			guidNodeID = GetGuidParameterFromQuery("node");

			if (!string.IsNullOrEmpty(Request.QueryString["comment"])) {
				bExportComments = true;
			}

			if (!string.IsNullOrEmpty(Request.QueryString["datebegin"])) {
				dateBegin = Convert.ToDateTime(Request.QueryString["datebegin"].ToString()).Date;
			}
			if (!string.IsNullOrEmpty(Request.QueryString["dateend"])) {
				dateEnd = Convert.ToDateTime(Request.QueryString["dateend"].ToString()).Date;
			}
			if (!string.IsNullOrEmpty(Request.QueryString["exportwhat"])) {
				ExportWhat = (SiteExport.ExportType)Enum.Parse(typeof(SiteExport.ExportType), Request.QueryString["exportwhat"].ToString(), true); ;
			}

			string theXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
			string fileName = "export.xml";

			if (guidContentID != Guid.Empty) {
				ContentPageExport content = ContentImportExportUtils.GetExportPage(SiteData.CurrentSiteID, guidContentID);
				theXML = ContentImportExportUtils.GetExportXML<ContentPageExport>(content);

				fileName = "page_" + content.ThePage.NavMenuText + "_" + guidContentID.ToString();
			} else {
				SiteExport site = ContentImportExportUtils.GetExportSite(SiteData.CurrentSiteID, ExportWhat);

				site.ThePages.RemoveAll(x => x.ThePage.GoLiveDate < dateBegin);
				site.ThePages.RemoveAll(x => x.ThePage.GoLiveDate > dateEnd);

				if (guidNodeID != Guid.Empty) {
					List<Guid> lst = pageHelper.GetPageHierarchy(SiteData.CurrentSiteID, guidNodeID);
					site.ThePages.RemoveAll(x => !lst.Contains(x.OriginalRootContentID) && x.ThePage.ContentType == ContentPageType.PageType.ContentEntry);
				}

				if (ExportWhat == SiteExport.ExportType.BlogData) {
					site.ThePages.RemoveAll(x => x.ThePage.ContentType == ContentPageType.PageType.ContentEntry);
				}
				if (ExportWhat == SiteExport.ExportType.ContentData) {
					site.ThePages.RemoveAll(x => x.ThePage.ContentType == ContentPageType.PageType.BlogEntry);
				}

				if (bExportComments) {
					site.LoadComments();
				}

				theXML = ContentImportExportUtils.GetExportXML<SiteExport>(site);

				fileName = "site_" + site.TheSite.SiteName + "_" + site.TheSite.SiteID.ToString();
			}

			fileName = fileName + "-" + SiteData.CurrentSite.Now.ToString("yyyy-MM-dd") + ".xml";

			fileName = fileName.Replace(" ", "_");

			Response.Expires = 5;
			Response.ContentType = "application/octet-stream";
			Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName));

			Response.Write(theXML);

			Response.StatusCode = 200;
			Response.StatusDescription = "OK";
			Response.End();
		}

	}
}