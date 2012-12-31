using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdminExport : AdminModule {


		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				LoadData();
			}
		}

		private void LoadData() {

			using (GalleryHelper gh = new GalleryHelper(SiteID)) {
				var lstCont = gh.GalleryGroupListGetBySiteID();

				gvPages.DataSource = lstCont;
				gvPages.DataBind();
			}
		}


		protected void btnExport_Click(object sender, EventArgs e) {
			List<Guid> lstSel = new List<Guid>();

			foreach (GridViewRow row in gvPages.Rows) {
				var chkReMap = (CheckBox)row.FindControl("chkSelect");

				if (chkReMap.Checked) {
					var hdnGalleryID = (HiddenField)row.FindControl("hdnGalleryID");
					Guid gGallery = new Guid(hdnGalleryID.Value);

					lstSel.Add(gGallery);
				}
			}

			string theXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
			string fileName = "galleryexport-" + SiteID.ToString() + ".xml";

			theXML = GalleryExportList.GetGalleryExportXML(SiteID, lstSel);

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