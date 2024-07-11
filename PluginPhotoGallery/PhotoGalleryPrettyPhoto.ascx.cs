using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public partial class PhotoGalleryPrettyPhoto : PublicGallerySingleBase {

		public PhotoGalleryPrettyPhoto() {
			this.ScaleImage = true;
			this.ShowHeading = false;
			this.ThumbSize = 100;
			this.PrettyPhotoSkin = "light_rounded";
		}

		public string GetScale() {
			return ScaleImage.ToString().ToLower();
		}

		public string GetThumbSize() {
			return ThumbSize.ToString().ToLower();
		}

		[Description("Gallery appearance (pretty photo skin)")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstPrettySkins")]
		public string PrettyPhotoSkin { get; set; } = "light_rounded";

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstPrettySkins {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict.Add("pp_default", "default");
				_dict.Add("light_square", "light square");
				_dict.Add("light_rounded", "light rounded");
				_dict.Add("facebook", "facebook");
				_dict.Add("dark_square", "dark square");
				_dict.Add("dark_rounded", "dark rounded");
				return _dict;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			this.ScaleImage = true;
			this.ShowHeading = false;
			this.ThumbSize = 100;
			this.PrettyPhotoSkin = "light_rounded";

			GetPublicParmValues();

			if (this.PublicParmValues.Any()) {
				try {
					string sFoundVal = GetParmValue("PrettyPhotoSkin", "light_rounded");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.PrettyPhotoSkin = sFoundVal;
					}
				} catch (Exception ex) { }
			}

			if (string.IsNullOrEmpty(PrettyPhotoSkin)) {
				PrettyPhotoSkin = "light_rounded";
			}

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