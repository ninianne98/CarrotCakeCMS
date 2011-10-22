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

	public class BaseServerControl : WebControl {

		protected ContentPage pageHelper = new ContentPage();
		protected SiteData siteHelper = new SiteData();
		protected SiteNav navHelper = new SiteNav();

		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

		private List<ContentPage> _pages = null;
		protected List<ContentPage> lstActivePages {
			get {
				if (_pages == null) {
					if (IsAuthEditor) {
						_pages = pageHelper.GetLatestContentList(SiteID, null);
					} else {
						_pages = pageHelper.GetLatestContentList(SiteID, true);
					}
				}
				return _pages;
			}
		}

		protected bool IsAuthEditor {
			get {
				return AdvancedEditMode || IsAdmin || IsEditor;
			}
		}

		protected bool AdvancedEditMode {
			get {
				return pageHelper.AdvancedEditMode;
			}
		}
		protected bool IsAdmin {
			get {
				return pageHelper.IsAdmin;
			}
		}
		protected bool IsEditor {
			get {
				return pageHelper.IsEditor;
			}
		}


		protected string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
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
