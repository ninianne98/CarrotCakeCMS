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
	public class GalleryHelper : GalleryBase {

		public GalleryHelper() { }

		public GalleryHelper(SiteData theSite) {
			base.ThisSite = theSite;
		}

		public GalleryHelper(Guid siteID) {
			base.ThisSite = SiteData.GetSiteFromCache(siteID);
		}


		public GalleryImageEntry GalleryImageEntryGetByID(Guid galleryImageID) {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				GalleryImageEntry ge = (from c in db.tblGalleryImages
										where c.GalleryImageID == galleryImageID
										select new GalleryImageEntry(c)).FirstOrDefault();

				return ge;
			}
		}


		public List<GalleryImageEntry> GalleryImageEntryListGetByGalleryID(Guid galleryID) {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				List<GalleryImageEntry> ge = (from c in db.tblGalleryImages
											  where c.GalleryID == galleryID
											  select new GalleryImageEntry(c)).ToList();

				return ge;
			}
		}


		public GalleryImageEntry GalleryImageEntryGetByFilename(Guid galleryID, string galleryImage) {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				GalleryImageEntry ge = (from c in db.tblGalleryImages
										where c.GalleryID == galleryID
										&& c.GalleryImage.ToLower() == galleryImage.ToLower()
										orderby c.ImageOrder ascending
										select new GalleryImageEntry(c)).FirstOrDefault();

				return ge;
			}
		}

		public void GalleryImageCleanup(Guid galleryID, List<string> lst) {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				var lstDel = (from g in db.tblGalleryImages
							  where g.GalleryID == galleryID
							  && !lst.Contains(g.GalleryImage.ToLower())
							  select g).ToList();

				db.tblGalleryImages.DeleteAllOnSubmit(lstDel);

				db.SubmitChanges();
			}
		}

		public List<GalleryMetaData> GetGalleryMetaDataListByGalleryID(Guid galleryID) {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				List<GalleryMetaData> imageData = (from g in db.tblGalleryImageMetas
												   join gg in db.tblGalleryImages on g.GalleryImage.ToLower() equals gg.GalleryImage.ToLower()
												   where g.SiteID == this.ThisSite.SiteID
													   && gg.GalleryID == galleryID
												   select new GalleryMetaData(g)).ToList();

				return imageData;
			}
		}

		public GalleryGroup GalleryGroupGetByID(Guid galleryID) {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				GalleryGroup ge = (from c in db.tblGalleries
								   where c.SiteID == this.ThisSite.SiteID
								   && c.GalleryID == galleryID
								   select new GalleryGroup(c)).FirstOrDefault();

				return ge;
			}
		}

		public GalleryGroup GalleryGroupGetByName(string galleryTitle) {
			GalleryGroup ge = null;

			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				if (!string.IsNullOrEmpty(galleryTitle)) {
					ge = (from c in db.tblGalleries
						  where c.SiteID == this.ThisSite.SiteID
						  && c.GalleryTitle.ToLower() == galleryTitle.ToLower()
						  select new GalleryGroup(c)).FirstOrDefault();
				}
			}

			return ge;
		}

		public List<GalleryGroup> GalleryGroupListGetBySiteID() {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				List<GalleryGroup> ge = (from c in db.tblGalleries
										 where c.SiteID == this.ThisSite.SiteID
										 select new GalleryGroup(c)).ToList();

				return ge;
			}
		}

		public GalleryMetaData GalleryMetaDataGetByFilename(string galleryImage) {
			GalleryMetaData ge = null;

			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				if (!string.IsNullOrEmpty(galleryImage)) {
					ge = (from c in db.tblGalleryImageMetas
						  where c.SiteID == this.ThisSite.SiteID
						  && c.GalleryImage.ToLower() == galleryImage.ToLower()
						  select new GalleryMetaData(c)).FirstOrDefault();
				}
			}

			return ge;
		}

		public GalleryMetaData GalleryMetaDataGetByID(Guid galleryImageMetaID) {
			using (PhotoGalleryDataContext db = PhotoGalleryDataContext.GetDataContext()) {
				GalleryMetaData ge = (from c in db.tblGalleryImageMetas
									  where c.SiteID == this.ThisSite.SiteID
									  && c.GalleryImageMetaID == galleryImageMetaID
									  select new GalleryMetaData(c)).FirstOrDefault();

				return ge;
			}
		}


	}
}