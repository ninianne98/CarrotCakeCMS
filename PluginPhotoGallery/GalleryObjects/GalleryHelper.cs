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
	public class GalleryHelper : GalleryBase, IDisposable {

		public GalleryHelper() { }

		public GalleryHelper(SiteData theSite) {
			base.ThisSite = theSite;
		}

		public GalleryHelper(Guid siteID) {
			base.ThisSite = SiteData.GetSiteByID(siteID);
		}


		public GalleryImageEntry GalleryImageEntryGetByID(Guid galleryImageID) {

			GalleryImageEntry ge = (from c in db.tblGalleryImages
									where c.GalleryImageID == galleryImageID
									select new GalleryImageEntry(c)).FirstOrDefault();

			return ge;
		}


		public List<GalleryImageEntry> GalleryImageEntryListGetByGalleryID(Guid galleryID) {

			List<GalleryImageEntry> ge = (from c in db.tblGalleryImages
										  where c.GalleryID == galleryID
										  select new GalleryImageEntry(c)).ToList();

			return ge;
		}


		public GalleryImageEntry GalleryImageEntryGetByFilename(Guid galleryID, string galleryImage) {

			GalleryImageEntry ge = (from c in db.tblGalleryImages
									where c.GalleryID == galleryID
									&& c.GalleryImage.ToLower() == galleryImage.ToLower()
									orderby c.ImageOrder ascending
									select new GalleryImageEntry(c)).FirstOrDefault();

			return ge;
		}

		public void GalleryImageCleanup(Guid galleryID, List<string> lst) {

			var lstDel = (from g in db.tblGalleryImages
						  where g.GalleryID == galleryID
						  && !lst.Contains(g.GalleryImage.ToLower())
						  select g).ToList();


			db.tblGalleryImages.DeleteAllOnSubmit(lstDel);

			db.SubmitChanges();

		}

		public List<GalleryMetaData> GetGalleryMetaDataListByGalleryID(Guid galleryID) {

			List<GalleryMetaData> imageData = (from g in db.tblGalleryImageMetas
											   join gg in db.tblGalleryImages on g.GalleryImage.ToLower() equals gg.GalleryImage.ToLower()
											   where g.SiteID == this.ThisSite.SiteID
												   && gg.GalleryID == galleryID
											   select new GalleryMetaData(g)).ToList();

			return imageData;
		}

		public GalleryGroup GalleryGroupGetByID(Guid galleryID) {

			GalleryGroup ge = (from c in db.tblGalleries
							   where c.SiteID == this.ThisSite.SiteID
							   && c.GalleryID == galleryID
							   select new GalleryGroup(c)).FirstOrDefault();

			return ge;
		}

		public List<GalleryGroup> GalleryGroupListGetBySiteID() {

			List<GalleryGroup> ge = (from c in db.tblGalleries
									 where c.SiteID == this.ThisSite.SiteID
									 select new GalleryGroup(c)).ToList();

			return ge;
		}

		public GalleryMetaData GalleryMetaDataGetByFilename(string galleryImage) {

			GalleryMetaData ge = (from c in db.tblGalleryImageMetas
								  where c.SiteID == this.ThisSite.SiteID
								  && c.GalleryImage.ToLower() == galleryImage.ToLower()
								  select new GalleryMetaData(c)).FirstOrDefault();

			return ge;
		}


		public GalleryMetaData GalleryMetaDataGetByID(Guid galleryImageMetaID) {

			GalleryMetaData ge = (from c in db.tblGalleryImageMetas
								  where c.SiteID == this.ThisSite.SiteID
								  && c.GalleryImageMetaID == galleryImageMetaID
								  select new GalleryMetaData(c)).FirstOrDefault();

			return ge;
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