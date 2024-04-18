using System.Configuration;

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
