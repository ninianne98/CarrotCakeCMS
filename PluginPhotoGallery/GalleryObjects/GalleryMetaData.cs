using System;
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

		public void ValidateGalleryImage() {
			if (string.IsNullOrEmpty(this.GalleryImage)) {
				throw new Exception("Image path must be provided.");
			}
			if (this.GalleryImage.Contains("../") || this.GalleryImage.Contains(@"..\")) {
				throw new Exception("Cannot use relative paths.");
			}
			if (this.GalleryImage.Contains(":")) {
				throw new Exception("Cannot specify drive letters or other protocols.");
			}
			if (this.GalleryImage.Contains("//") || this.GalleryImage.Contains(@"\\")) {
				throw new Exception("Cannot use UNC paths.");
			}
			if (this.GalleryImage.Contains("<") || this.GalleryImage.Contains(">")) {
				throw new Exception("Cannot include html tags.");
			}
		}

		public void Save() {
			if (!string.IsNullOrEmpty(this.GalleryImage)) {
				this.ValidateGalleryImage();

				using (var db = PhotoGalleryDataContext.GetDataContext()) {
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