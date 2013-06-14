using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryFancyBox2 : WidgetParmData, IWidget, IWidgetEditStatus {

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

		public bool ShowHeading { get; set; }

		public bool ScaleImage { get; set; }

		[Widget(WidgetAttribute.FieldMode.CheckBoxList, "lstGalleryID")]
		public List<Guid> GalleryIDs { get; set; }

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstGalleryID {
			get {
				if (SiteID == Guid.Empty) {
					SiteID = SiteData.CurrentSiteID;
				}
				Dictionary<string, string> _dict = null;

				using (GalleryHelper gh = new GalleryHelper(SiteID)) {

					_dict = (from c in gh.GalleryGroupListGetBySiteID()
							 orderby c.GalleryTitle
							 where c.SiteID == SiteID
							 select c).ToList().ToDictionary(k => k.GalleryID.ToString(), v => v.GalleryTitle);
				}

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


		protected void Page_Load(object sender, EventArgs e) {

			if (PublicParmValues.Count > 0) {

				GalleryIDs = new List<Guid>();

				try {
					List<string> lstGallery = GetParmValueList("GalleryIDs");

					foreach (string sGallery in lstGallery) {
						if (!string.IsNullOrEmpty(sGallery)) {
							GalleryIDs.Add(new Guid(sGallery));
						}
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

			if (GalleryIDs == null) {
				GalleryIDs = new List<Guid>();
			}

			using (GalleryHelper gh = new GalleryHelper(SiteID)) {
				var lstCont = gh.GalleryGroupListGetBySiteID();

				var gal = (from g in GalleryIDs
						   join gg in lstCont on g equals gg.GalleryID
						   orderby gg.GalleryTitle
						   select gg).ToList();

				if (gal != null) {
					rpGalleries.DataSource = gal;
					rpGalleries.DataBind();
				}
			}

			if (rpGalleries.Items.Count > 0) {
				pnlGallery.Visible = true;
			} else {
				pnlGallery.Visible = false;
			}

		}

	}
}