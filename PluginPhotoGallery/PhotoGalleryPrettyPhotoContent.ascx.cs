using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public partial class PhotoGalleryPrettyPhotoContent : PublicGalleryBase {

		public PhotoGalleryPrettyPhotoContent() {
			this.ScaleImage = true;
			this.ShowHeading = false;
			this.ThumbSize1 = 150;
			this.ThumbSize2 = 200;
			this.WindowWidth = 500;
			this.PrettyPhotoSkin = "light_rounded";
		}

		[Description("Gallery to display")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstGalleryID")]
		public Guid GalleryID { get; set; }

		private int iCtrl1 = 0;

		protected string CtrlTopId {
			get {
				return "GalleryContent_" + (iCtrl1++);
			}
		}

		private int iCtrl2 = 0;

		protected string CtrlSubId {
			get {
				return "GalleryContent_" + (iCtrl2++);
			}
		}

		[Description("Gallery main image pixel height/width")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize1 { get; set; }

		[Description("Gallery detail image pixel height/width")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize2 { get; set; }

		[Description("Gallery popup window width in pixels")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes2")]
		public int WindowWidth { get; set; }

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstSizes2 {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict.Add("500", "500px");
				_dict.Add("550", "550px");
				_dict.Add("600", "600px");
				_dict.Add("650", "650px");
				_dict.Add("700", "700px");
				_dict.Add("750", "750px");
				return _dict;
			}
		}

		[Description("Gallery appearance (pretty photo skin)")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstPrettySkins")]
		public string PrettyPhotoSkin { get; set; }

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

		public string GetScale() {
			return this.ScaleImage.ToString().ToLower();
		}

		public string GetThumbSize() {
			return this.ThumbSize1.ToString().ToLower();
		}

		public string GetThumbSize2() {
			return this.ThumbSize2.ToString().ToLower();
		}

		public string GetWindowWidth() {
			return this.WindowWidth.ToString().ToLower();
		}

		private List<GalleryMetaData> imageData = new List<GalleryMetaData>();

		public string GetImageBody(string sImg) {
			var imgData = (from g in imageData
						   where g.GalleryImage.ToLower() == sImg.ToLower()
						   select g).FirstOrDefault();

			if (imgData == null) {
				return "";
			} else {
				imgData.ValidateGalleryImage();
				return imgData.ImageMetaData;
			}
		}

		public string GetImageTitle(string sImg) {
			var imgData = (from g in imageData
						   where g.GalleryImage.ToLower() == sImg.ToLower()
						   select g).FirstOrDefault();

			if (imgData == null) {
				return sImg;
			} else {
				imgData.ValidateGalleryImage();
				return imgData.ImageTitle;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			GetPublicParmValues();

			if (this.PublicParmValues.Any()) {
				try {
					string sFoundVal = GetParmValue("GalleryID", Guid.Empty.ToString());

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.GalleryID = new Guid(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValueDefaultEmpty("WindowWidth", "500");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.WindowWidth = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValueDefaultEmpty("ThumbSize1", "150");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.ThumbSize1 = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValueDefaultEmpty("ThumbSize2", "200");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.ThumbSize2 = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("PrettyPhotoSkin", "light_rounded");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.PrettyPhotoSkin = sFoundVal;
					}
				} catch (Exception ex) { }
			}

			if (string.IsNullOrEmpty(PrettyPhotoSkin)) {
				this.PrettyPhotoSkin = "light_rounded";
			}

			imageData = new List<GalleryMetaData>();

			List<GalleryImageEntry> gallery = null;

			GalleryHelper gh = new GalleryHelper(this.SiteID);

			var gal = gh.GalleryGroupGetByID(this.GalleryID);

			if (gal != null) {
				litGalleryName.Text = gal.GalleryTitle;
				pnlGalleryHead.Visible = ShowHeading;

				gallery = (from g in gal.GalleryImages
						   orderby g.ImageOrder ascending
						   select g).ToList();

				imageData = gh.GetGalleryMetaDataListByGalleryID(GalleryID);

				rpGallery.DataSource = gallery;
				rpGallery.DataBind();

				rpGalleryDetail.DataSource = gallery;
				rpGalleryDetail.DataBind();
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