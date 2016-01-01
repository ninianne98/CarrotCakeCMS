using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public partial class PhotoGalleryFancyBox2 : PublicGalleryBase {

		public PhotoGalleryFancyBox2() {
			this.ScaleImage = true;
			this.ShowHeading = false;
			this.ThumbSize = 100;
		}

		[Description("Galleries to display")]
		[Widget(WidgetAttribute.FieldMode.CheckBoxList, "lstGalleryID")]
		public List<Guid> GalleryIDs { get; set; }

		[Description("Gallery image pixel height/width")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize { get; set; }

		protected void Page_Load(object sender, EventArgs e) {
			GetPublicParmValues();

			if (this.PublicParmValues.Any()) {
				this.GalleryIDs = new List<Guid>();

				this.ThumbSize = 150;

				try {
					List<string> lstGallery = GetParmValueList("GalleryIDs");

					foreach (string sGallery in lstGallery) {
						if (!String.IsNullOrEmpty(sGallery)) {
							this.GalleryIDs.Add(new Guid(sGallery));
						}
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("ThumbSize", "150");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.ThumbSize = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }
			}

			if (this.GalleryIDs == null) {
				this.GalleryIDs = new List<Guid>();
			}

			GalleryHelper gh = new GalleryHelper(SiteID);
			var lstCont = gh.GalleryGroupListGetBySiteID();

			var gal = (from g in GalleryIDs
					   join gg in lstCont on g equals gg.GalleryID
					   orderby gg.GalleryTitle
					   select gg).ToList();

			if (gal != null) {
				rpGalleries.DataSource = gal;
				rpGalleries.DataBind();
			}

			if (rpGalleries.Items.Count > 0) {
				pnlGallery.Visible = true;
			} else {
				pnlGallery.Visible = false;
			}
		}
	}
}