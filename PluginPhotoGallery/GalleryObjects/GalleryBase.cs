using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;

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