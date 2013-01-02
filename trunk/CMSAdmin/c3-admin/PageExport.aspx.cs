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

		public enum ExportType {
			BlogData,
			ContentData,
			AllData,
		}

		public Guid guidContentID = Guid.Empty;
		public DateTime dateBegin = DateTime.MinValue;
		public DateTime dateEnd = DateTime.MaxValue;
		public ExportType ExportWhat = ExportType.AllData;


		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidContentID = new Guid(Request.QueryString["id"].ToString());
			}

			if (!string.IsNullOrEmpty(Request.QueryString["datebegin"])) {
				dateBegin = Convert.ToDateTime(Request.QueryString["datebegin"].ToString()).Date;
			}
			if (!string.IsNullOrEmpty(Request.QueryString["dateend"])) {
				dateEnd = Convert.ToDateTime(Request.QueryString["dateend"].ToString()).Date;
			}
			if (!string.IsNullOrEmpty(Request.QueryString["exportwhat"])) {
				ExportWhat = (ExportType)Enum.Parse(typeof(ExportType), Request.QueryString["exportwhat"].ToString(), true); ;
			}


			string theXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
			string fileName = "export.xml";

			if (guidContentID != Guid.Empty) {
				ContentPageExport content = ContentImportExportUtils.GetExportPage(SiteData.CurrentSiteID, guidContentID);
				theXML = ContentImportExportUtils.GetExportXML<ContentPageExport>(content);

				fileName = "page_" + content.ThePage.NavMenuText + "_" + guidContentID.ToString();
			} else {
				SiteExport site = ContentImportExportUtils.GetExportSite(SiteData.CurrentSiteID);

				site.ThePages.RemoveAll(x => x.ThePage.GoLiveDate < dateBegin);
				site.ThePages.RemoveAll(x => x.ThePage.GoLiveDate > dateEnd);

				if (ExportWhat == ExportType.BlogData) {
					site.ThePages.RemoveAll(x => x.ThePage.ContentType == ContentPageType.PageType.ContentEntry);
				}
				if (ExportWhat == ExportType.ContentData) {
					site.ThePages.RemoveAll(x => x.ThePage.ContentType == ContentPageType.PageType.BlogEntry);
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