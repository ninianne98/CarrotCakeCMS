using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public class GalleryMetaData : GalleryBase, IDisposable {

		public GalleryMetaData() { }

		internal GalleryMetaData(tblGalleryImageMeta gal) {
			if (gal != null) {
				this.GalleryImageMetaID = gal.GalleryImageMetaID;
				this.SiteID = gal.SiteID.Value;

				this.GalleryImage = gal.GalleryImage;
				this.ImageTitle = gal.ImageTitle;
				this.ImageMetaData = gal.ImageMetaData;
			}
		}

		public Guid GalleryImageMetaID { get; set; }
		public Guid SiteID { get; set; }

		public string GalleryImage { get; set; }
		public string ImageTitle { get; set; }
		public string ImageMetaData { get; set; }



		public void Save() {

			tblGalleryImageMeta gal = (from c in db.tblGalleryImageMetas
									   where c.GalleryImage.ToLower() == this.GalleryImage.ToLower()
									   select c).FirstOrDefault();

			if (gal == null || this.GalleryImageMetaID == Guid.Empty) {
				gal = new tblGalleryImageMeta();
				gal.SiteID = this.SiteID;
				gal.GalleryImageMetaID = Guid.NewGuid();
				gal.GalleryImage = this.GalleryImage;
			}

			gal.ImageTitle = this.ImageTitle;
			gal.ImageMetaData = this.ImageMetaData;

			if (gal.GalleryImageMetaID != this.GalleryImageMetaID) {
				db.tblGalleryImageMetas.InsertOnSubmit(gal);
			}

			db.SubmitChanges();

		}

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}
}