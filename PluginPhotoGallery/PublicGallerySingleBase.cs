using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.ComponentModel;
using System.Linq;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public abstract class PublicGallerySingleBase : PublicGalleryBase {

		[Description("Gallery to display")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstGalleryID")]
		public Guid GalleryID { get; set; }

		[Description("Gallery image pixel height/width")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize { get; set; }

		public override void GetPublicParmValues() {
			base.GetPublicParmValues();

			if (this.PublicParmValues.Any()) {
				this.ThumbSize = 100;

				try {
					string sFoundVal = GetParmValue("GalleryID", Guid.Empty.ToString());

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.GalleryID = new Guid(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValueDefaultEmpty("ThumbSize", "150");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.ThumbSize = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }
			}
		}
	}
}