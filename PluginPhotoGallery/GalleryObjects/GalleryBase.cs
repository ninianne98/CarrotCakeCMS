using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;

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