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
	public class GalleryGroup : GalleryBase, IDisposable {

		public GalleryGroup() { }

		internal GalleryGroup(tblGallery gal) {
			if (gal != null) {
				this.GalleryID = gal.GalleryID;
				this.SiteID = gal.SiteID.Value;

				this.GalleryTitle = gal.GalleryTitle;

				using (GalleryHelper gh = new GalleryHelper(this.SiteID)) {
					this.GalleryImages = gh.GalleryImageEntryListGetByGalleryID(this.GalleryID);
				}

			}
		}

		public Guid GalleryID { get; set; }
		public Guid SiteID { get; set; }

		public string GalleryTitle { get; set; }

		public List<GalleryImageEntry> GalleryImages { get; set; }


		public void Save() {

			tblGallery gal = (from c in db.tblGalleries
							  where c.GalleryID == this.GalleryID
							  select c).FirstOrDefault();

			if (gal == null || this.GalleryID == Guid.Empty) {
				gal = new tblGallery();
				gal.SiteID = this.SiteID;
				gal.GalleryID = Guid.NewGuid();
			}

			gal.GalleryTitle = this.GalleryTitle;

			if (gal.GalleryID != this.GalleryID) {
				db.tblGalleries.InsertOnSubmit(gal);
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