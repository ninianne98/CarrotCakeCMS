using System.Configuration;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryDataContext {

		private static string connString = ConfigurationManager.ConnectionStrings["CarrotwareCMSConnectionString"].ConnectionString;

		public static PhotoGalleryDataContext GetDataContext() {

			return GetDataContext(connString);
		}


		public static PhotoGalleryDataContext GetDataContext(string connection) {

			return new PhotoGalleryDataContext(connection);
		}

	}
}
