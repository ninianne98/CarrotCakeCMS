using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Plugins.FAQ2Module {

	public class FaqBase {
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