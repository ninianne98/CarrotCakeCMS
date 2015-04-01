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
	public class GalleryMetaData : GalleryBase {

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

			if (!string.IsNullOrEmpty(this.GalleryImage)) {
				using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
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

					this.GalleryImageMetaID = gal.GalleryImageMetaID;
				}
			}
		}


		public override string ToString() {
			return ImageTitle;
		}

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is GalleryMetaData) {
				GalleryMetaData p = (GalleryMetaData)obj;
				return (this.GalleryImageMetaID == p.GalleryImageMetaID)
						&& (this.SiteID == p.SiteID)
						&& (this.ImageTitle == p.ImageTitle);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return GalleryImageMetaID.GetHashCode() ^ SiteID.GetHashCode() ^ ImageTitle.GetHashCode();
		}


	}
}