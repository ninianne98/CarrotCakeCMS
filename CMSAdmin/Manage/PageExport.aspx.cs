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

			var content = ContentPageExport.GetExportPage(guidContentID);
			var theXML = ContentPageExport.GetExportXML(content);

			string fileName = content.ThePage.NavMenuText + ".xml";
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