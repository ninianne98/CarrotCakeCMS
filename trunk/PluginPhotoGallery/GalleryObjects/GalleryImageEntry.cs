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
	public class GalleryImageEntry : GalleryBase {

		public GalleryImageEntry() { }

		internal GalleryImageEntry(tblGalleryImage gal) {
			if (gal != null) {
				this.GalleryID = gal.GalleryID.Value;
				this.GalleryImageID = gal.GalleryImageID;

				this.GalleryImage = gal.GalleryImage;
				this.ImageOrder = gal.ImageOrder.Value;
			}
		}

		public Guid GalleryID { get; set; }
		public Guid GalleryImageID { get; set; }

		public string GalleryImage { get; set; }
		public int ImageOrder { get; set; }


		public void Save() {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				tblGalleryImage gal = (from c in db.tblGalleryImages
									   where c.GalleryImageID == this.GalleryImageID
									   select c).FirstOrDefault();

				if (gal == null || this.GalleryID == Guid.Empty) {
					gal = new tblGalleryImage();
					gal.GalleryID = this.GalleryID;
					gal.GalleryImageID = Guid.NewGuid();
				}

				gal.GalleryImage = this.GalleryImage;
				gal.ImageOrder = this.ImageOrder;

				if (gal.GalleryImageID != this.GalleryImageID) {
					db.tblGalleryImages.InsertOnSubmit(gal);
				}

				db.SubmitChanges();

				this.GalleryImageID = gal.GalleryImageID;
			}
		}

		public override string ToString() {
			return GalleryImage;
		}

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is GalleryImageEntry) {
				GalleryImageEntry p = (GalleryImageEntry)obj;
				return (this.GalleryImageID == p.GalleryImageID)
						&& (this.GalleryID == p.GalleryID)
						&& (this.GalleryImage == p.GalleryImage);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return GalleryImageID.GetHashCode() ^ GalleryID.GetHashCode() ^ GalleryImage.GetHashCode();
		}


	}
}