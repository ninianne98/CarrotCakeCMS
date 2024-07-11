using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.ComponentModel;
using System.Linq;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public abstract class PublicGallerySingleBase : PublicGalleryBase {

		[Description("Gallery to display")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstGalleryID")]
		public Guid GalleryID { get; set; }

		[Description("Gallery image pixel height/width")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize { get; set; } = 100;

		public override void GetPublicParmValues() {
			base.GetPublicParmValues();

			if (this.PublicParmValues.Any()) {
				this.ThumbSize = 100;

				try {
					var foundVal = this.GetValue(x => x.GalleryID, Guid.Empty);

					if (foundVal != Guid.Empty && this.GalleryID == Guid.Empty) {
						this.SetGuidValue(x => x.GalleryID, foundVal);
					}
				} catch (Exception ex) { }

				try {
					var foundVal = this.GetValue(x => x.ThumbSize, this.ThumbSize);
					this.SetIntValue(x => x.ThumbSize, foundVal);
				} catch (Exception ex) { }
			}
		}
	}
}