using System;
using System.Collections.Generic;
using System.Linq;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public partial class PhotoGalleryFancyBox : PublicGallerySingleBase {

		public PhotoGalleryFancyBox() {
			this.ScaleImage = true;
			this.ShowHeading = false;
			this.ThumbSize = 100;
		}

		public string GetScale() {
			return this.ScaleImage.ToString().ToLower();
		}

		public string GetThumbSize() {
			return this.ThumbSize.ToString().ToLower();
		}

		protected void Page_Load(object sender, EventArgs e) {
			GetPublicParmValues();

			GalleryHelper gh = new GalleryHelper(SiteID);

			var gal = gh.GalleryGroupGetByID(GalleryID);

			if (gal != null) {
				litGalleryName.Text = gal.GalleryTitle;
				pnlGalleryHead.Visible = ShowHeading;

				rpGallery.DataSource = (from g in gal.GalleryImages
										where g.GalleryID == GalleryID
										orderby g.ImageOrder ascending
										select g).ToList();

				rpGallery.DataBind();
			}

			if (rpGallery.Items.Count > 0) {
				pnlGallery.Visible = true;
			} else {
				pnlGallery.Visible = false;
			}

			if (!this.IsBeingEdited) {
				pnlScript.Visible = true;
			} else {
				pnlScript.Visible = false;
			}
		}
	}
}