using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public abstract class PublicGalleryBase : WidgetParmDataUserControl, IWidgetEditStatus {

		[Description("Display gallery heading")]
		[Widget(WidgetAttribute.FieldMode.CheckBox)]
		public bool ShowHeading { get; set; }

		[Description("Scale gallery images")]
		[Widget(WidgetAttribute.FieldMode.CheckBox)]
		public bool ScaleImage { get; set; }

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

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public virtual Dictionary<string, string> lstSizes {
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

		public virtual void GetPublicParmValues() {
			if (this.PublicParmValues.Any()) {
				this.ScaleImage = false;
				this.ShowHeading = false;

				try {
					string sFoundVal = GetParmValue("ShowHeading", "false");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.ShowHeading = Convert.ToBoolean(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("ScaleImage", "false");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.ScaleImage = Convert.ToBoolean(sFoundVal);
					}
				} catch (Exception ex) { }
			}
		}

		#region IWidgetEditStatus Members

		public bool IsBeingEdited { get; set; }

		#endregion IWidgetEditStatus Members
	}
}