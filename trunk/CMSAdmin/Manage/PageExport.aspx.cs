using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class PageExport : AdminBasePage {

		public Guid guidContentID = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidContentID = new Guid(Request.QueryString["id"].ToString());
			}

			string theXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
			string fileName = "export.xml";

			if (guidContentID != Guid.Empty) {
				ContentPageExport content = ContentImportExportUtils.GetExportPage(SiteData.CurrentSiteID, guidContentID);
				theXML = ContentImportExportUtils.GetExportXML<ContentPageExport>(content);

				fileName = "page_" + content.ThePage.NavMenuText + "_" + guidContentID.ToString() + ".xml";
			} else {
				SiteExport site = ContentImportExportUtils.GetExportSite(SiteData.CurrentSiteID);
				theXML = ContentImportExportUtils.GetExportXML<SiteExport>(site);

				fileName = "site_" + site.TheSite.SiteName + "_" + site.TheSite.SiteID.ToString() + ".xml";
			}

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