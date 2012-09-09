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
		PhotoGalleryDataContext db = new PhotoGalleryDataContext();

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			}
			if (!IsPostBack) {
				var gal = (from c in db.tblGalleries
						   where c.SiteID == SiteID
						   && c.GalleryID == gTheID
						   select c).FirstOrDefault();

				if (gal != null) {
					txtGallery.Text = gal.GalleryTitle;
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			var gal = (from c in db.tblGalleries
					   where c.SiteID == SiteID
					   && c.GalleryID == gTheID
					   select c).FirstOrDefault();

			if (gal == null || gTheID == Guid.Empty) {
				gal = new tblGallery();
				gal.SiteID = SiteID;
				gal.GalleryID = Guid.NewGuid();
			}

			gal.GalleryTitle = txtGallery.Text;

			if (gal.GalleryID != gTheID) {
				db.tblGalleries.InsertOnSubmit(gal);
			}

			db.SubmitChanges();

			var QueryStringFile = CreateLink("CategoryList");

			Response.Redirect(QueryStringFile);
		}




	}
}