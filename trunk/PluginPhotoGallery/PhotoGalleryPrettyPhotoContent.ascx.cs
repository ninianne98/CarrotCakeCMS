using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryPrettyPhotoContent : WidgetParmData, IWidget, IWidgetEditStatus {

		PhotoGalleryDataContext db = new PhotoGalleryDataContext();

		public bool ShowHeading { get; set; }

		public bool ScaleImage { get; set; }

		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstGalleryID")]
		public Guid GalleryID { get; set; }


		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstGalleryID {
			get {
				if (SiteID == Guid.Empty) {
					SiteID = SiteData.CurrentSiteID;
				}

				Dictionary<string, string> _dict = (from c in db.tblGalleries
													orderby c.GalleryTitle
													where c.SiteID == SiteID
													select c).ToList().ToDictionary(k => k.GalleryID.ToString(), v => v.GalleryTitle);

				return _dict;
			}
		}

		private int iCtrl = 0;

		protected string CtrlId {
			get {
				return "Gallery_" + (iCtrl++);
			}
		}

		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize2 { get; set; }

		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize { get; set; }

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstSizes {
			get {

				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict.Add("25", "25px");
				_dict.Add("50", "50px");
				_dict.Add("75", "75px");
				_dict.Add("100", "100px");
				_dict.Add("125", "125px");
				_dict.Add("150", "150px");
				_dict.Add("175", "175px");
				_dict.Add("200", "200px");
				_dict.Add("225", "225px");
				_dict.Add("250", "250px");
				_dict.Add("275", "275px");
				_dict.Add("300", "300px");
				_dict.Add("325", "325px");
				_dict.Add("350", "350px");
				return _dict;
			}
		}

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

		public string GetScale() {
			return ScaleImage.ToString().ToLower();
		}


		public string GetThumbSize() {
			return ThumbSize.ToString().ToLower();
		}

		public string GetThumbSize2() {
			return ThumbSize2.ToString().ToLower();
		}

		public string GetWindowWidth() {
			return WindowWidth.ToString().ToLower();
		}

		public string GetImageBody(string sImg) {
			var imgData = (from g in db.tblGalleryImageMetas
						   where g.SiteID == SiteData.CurrentSiteID
							   && g.GalleryImage.ToLower() == sImg.ToLower()
						   select g).FirstOrDefault();

			if (imgData == null) {
				return "";
			} else {
				return imgData.ImageMetaData;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {

			if (PublicParmValues.Count > 0) {

				try {
					string sFoundVal = GetParmValue("GalleryID", Guid.Empty.ToString());

					if (!string.IsNullOrEmpty(sFoundVal)) {
						GalleryID = new Guid(sFoundVal);
					}
				} catch (Exception ex) { }


				try {
					string sFoundVal = GetParmValue("ShowHeading", "false");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ShowHeading = Convert.ToBoolean(sFoundVal);
					}
				} catch (Exception ex) { }


				try {
					string sFoundVal = GetParmValue("ScaleImage", "false");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ScaleImage = Convert.ToBoolean(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("WindowWidth", "500");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						WindowWidth = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("ThumbSize", "150");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ThumbSize = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("ThumbSize2", "200");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ThumbSize2 = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }
			}

			var gal = (from g in db.tblGalleries
					   where g.GalleryID == GalleryID
							&& g.SiteID == SiteData.CurrentSiteID
					   select g).FirstOrDefault();

			if (gal != null) {

				litGalleryName.Text = gal.GalleryTitle;
				pnlGalleryHead.Visible = ShowHeading;

				var gallery = (from g in db.tblGalleryImages
							   join gg in db.tblGalleries on g.GalleryID equals gg.GalleryID
							   where g.GalleryID == GalleryID
								   && gg.SiteID == SiteData.CurrentSiteID
							   orderby g.ImageOrder ascending
							   select g).ToList();
				iCtrl = 0;
				rpGallery.DataSource = gallery;
				rpGallery.DataBind();

				iCtrl = 0;
				rpGalleryDetail.DataSource = gallery;
				rpGalleryDetail.DataBind();
			}

			if (rpGallery.Items.Count > 0) {
				pnlGallery.Visible = true;
			} else {
				pnlGallery.Visible = false;
			}

			if (!IsBeingEdited) {
				pnlScript.Visible = true;
			} else {
				pnlScript.Visible = false;
			}
		}


		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		public Guid SiteID { get; set; }

		public string JSEditFunction {
			get { return ""; }
		}
		public bool EnableEdit {
			get { return true; }
		}

		#endregion

		#region IWidgetEditStatus Members

		public bool IsBeingEdited { get; set; }

		#endregion
	}
}