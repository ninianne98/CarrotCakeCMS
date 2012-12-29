using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdminCategory : AdminModule {

		Guid gTheID = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			}
			if (!IsPostBack) {
				using (GalleryHelper gh = new GalleryHelper(SiteID)) {
					var gal = gh.GalleryGroupGetByID(gTheID);

					if (gal != null) {
						txtGallery.Text = gal.GalleryTitle;
					}
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			using (GalleryHelper gh = new GalleryHelper(SiteID)) {

				var gal = gh.GalleryGroupGetByID(gTheID);

				if (gal == null || gTheID == Guid.Empty) {
					gal = new GalleryGroup();
					gal.SiteID = SiteID;
					gal.GalleryID = Guid.Empty;
				}

				gal.GalleryTitle = txtGallery.Text;

				gal.Save();
			}

			string QueryStringFile = CreateLink("CategoryList");

			Response.Redirect(QueryStringFile);
		}




	}
}