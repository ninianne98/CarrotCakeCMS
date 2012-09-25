using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

		public static string InactivePagePrefix {
			get {
				return "&#9746; ";
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


		public static string GetCtrlText(Control ctrl) {
			StringBuilder sb = new StringBuilder();
			StringWriter tw = new StringWriter(sb);
			HtmlTextWriter hw = new HtmlTextWriter(tw);

			ctrl.RenderControl(hw);

			return sb.ToString();
		}

		public static string IdentifyTextAsInactive(bool bStatus, string sLinkText) {

			if (!bStatus) {
				sLinkText = InactivePagePrefix + sLinkText;
			}

			return sLinkText;
		}

		public static SiteNav IdentifyLinkAsInactive(SiteNav nav) {

			if (!nav.PageActive) {
				nav.NavMenuText = InactivePagePrefix + nav.NavMenuText;
				nav.PageHead = InactivePagePrefix + nav.PageHead;
				nav.TitleBar = InactivePagePrefix + nav.TitleBar;
			}

			return nav;
		}


	}
}
