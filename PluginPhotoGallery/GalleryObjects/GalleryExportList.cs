using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public class GalleryExportList {

		public GalleryExportList() {
			TheGalleries = new List<GalleryExport>();
		}

		public SiteData OriginalSite { get; set; }

		public List<GalleryExport> TheGalleries { get; set; }

		public string CarrotCakeVersion { get; set; }
		public DateTime ExportDate { get; set; }


		//========================
		public static string GetGalleryExportXML(Guid siteID, List<Guid> GalleryIDs) {

			GalleryExportList lstGE = new GalleryExportList();
			lstGE.ExportDate = SiteData.CurrentSite.Now;
			lstGE.CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;
			lstGE.OriginalSite = SiteData.GetSiteFromCache(siteID);

			GalleryHelper gh = new GalleryHelper(siteID);
			
			foreach (Guid galleryID in GalleryIDs) {
				GalleryExport ge = new GalleryExport();
				GalleryGroup gal = gh.GalleryGroupGetByID(galleryID);

				ge.TheGallery = gal;
				ge.OriginalGalleryID = gal.GalleryID;
				ge.ExportDate = SiteData.CurrentSite.Now;
				ge.CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;

				lstGE.TheGalleries.Add(ge);
			}


			return ContentImportExportUtils.GetExportXML<GalleryExportList>(lstGE);
		}

		public static GalleryExportList GetGalleryExport(Guid galleryID) {
			GalleryExportList lstG = null;
			try {
				string sXML = ContentImportExportUtils.GetSerialized(galleryID);
				lstG = ContentImportExportUtils.GetSerialData<GalleryExportList>(sXML) as GalleryExportList;
			} catch (Exception ex) { }
			return lstG;
		}

		public static GalleryExportList DeserializeGalleryExport(string sXML) {
			GalleryExportList lstG = ContentImportExportUtils.GetSerialData<GalleryExportList>(sXML) as GalleryExportList;

			return lstG;
		}

		public static Guid SaveSerializedDataExport(GalleryExportList lstGalleries) {
			Guid gKey = Guid.NewGuid();

			foreach (GalleryExport g in lstGalleries.TheGalleries) {
				g.TheGallery.GalleryID = Guid.NewGuid();
				g.TheGallery.GalleryImages.ForEach(q => q.GalleryID = g.TheGallery.GalleryID);
				g.TheGallery.GalleryImages.ForEach(q => q.GalleryImageID = Guid.NewGuid());
			}

			ContentImportExportUtils.SaveSerializedDataExport<GalleryExportList>(gKey, lstGalleries);
			return gKey;
		}
	}


	//=================================

	public class GalleryExport {

		public GalleryGroup TheGallery { get; set; }
		public Guid OriginalGalleryID { get; set; }

		public string CarrotCakeVersion { get; set; }
		public DateTime ExportDate { get; set; }

	}


}