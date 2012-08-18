using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Controls {

	public abstract class BaseServerControl : WebControl {

		protected ContentPageHelper pageHelper = new ContentPageHelper();
		protected SiteData siteHelper = new SiteData();
		protected SiteNavHelper navHelper = new SiteNavHelper();

		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

		private List<ContentPage> _pages = null;
		protected List<ContentPage> lstActivePages {
			get {
				if (_pages == null) {
					if (SecurityData.IsAuthEditor) {
						_pages = pageHelper.GetLatestContentList(SiteID, null);
					} else {
						_pages = pageHelper.GetLatestContentList(SiteID, true);
					}
				}
				return _pages;
			}
		}


		public string Text {
			get {
				String s = (String)ViewState["Text"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["Text"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter output) {

		}
	}
}
