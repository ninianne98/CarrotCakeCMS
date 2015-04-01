using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryFancyBox : WidgetParmDataUserControl, IWidgetEditStatus {

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
				Dictionary<string, string> _dict = null;

				GalleryHelper gh = new GalleryHelper(SiteID);

				_dict = (from c in gh.GalleryGroupListGetBySiteID()
						 orderby c.GalleryTitle
						 where c.SiteID == SiteID
						 select c).ToList().ToDictionary(k => k.GalleryID.ToString(), v => v.GalleryTitle);


				return _dict;
			}
		}


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

				return _dict;
			}
		}


		#region IWidgetEditStatus Members

		public bool IsBeingEdited { get; set; }

		#endregion


		public string GetScale() {
			return ScaleImage.ToString().ToLower();
		}

		public string GetThumbSize() {
			return ThumbSize.ToString().ToLower();
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
					string sFoundVal = GetParmValue("ThumbSize", "150");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ThumbSize = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }
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

			if (!IsBeingEdited) {
				pnlScript.Visible = true;
			} else {
				pnlScript.Visible = false;
			}
		}

	}
}