using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryTest : System.Web.UI.Page {

		Guid gTheID = Guid.Empty;
		PhotoGalleryDataContext db = new PhotoGalleryDataContext();

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			}

			GalleryFancyBox1.GalleryID = gTheID;
			GalleryFancyBox1.ShowHeading = true;
			GalleryFancyBox1.ScaleImage = true;
			GalleryFancyBox1.ThumbSize = 100;
			GalleryFancyBox1.SiteID = SiteData.CurrentSiteID;

			if (gTheID != Guid.Empty) {
				GalleryFancyBox2.ShowHeading = true;
				GalleryFancyBox2.ScaleImage = true;
				GalleryFancyBox2.ThumbSize = 50;
				GalleryFancyBox2.SiteID = SiteData.CurrentSiteID;

				GalleryFancyBox2.GalleryIDs = (from c in db.tblGalleries
											   where c.SiteID == SiteData.CurrentSiteID
											   select c.GalleryID).ToList();

			}

		}
	}
}
