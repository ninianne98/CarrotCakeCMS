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

	public class GalleryBase {

		protected FileDataHelper fileHelper = new FileDataHelper();

		private SiteData _site = null;

		protected SiteData ThisSite {
			get {
				if (_site == null) {
					_site = SiteData.CurrentSite;
				}
				return _site;
			}
			set {
				_site = value;
			}
		}


	}
}