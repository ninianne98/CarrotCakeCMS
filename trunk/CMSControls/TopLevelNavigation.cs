using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;

//  http://msdn.microsoft.com/en-us/library/yhzc935f.aspx

namespace Carrotware.CMS.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:TopLevelNavigation runat=server></{0}:TopLevelNavigation>")]
	public class TopLevelNavigation : BaseServerControl {


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}

		protected List<SiteNav> GetTopNav() {
			return navHelper.GetTopNavigation(SiteID, !IsAuthEditor);
		}

		protected override void RenderContents(HtmlTextWriter output) {

			var pageContents = navHelper.GetPageNavigation(SiteID, CurrentScriptName);
			var sParent = "";
			if (pageContents != null) {
				if (pageContents.Parent_ContentID == null) {
					sParent = pageContents.FileName.ToLower();
				}
			}
			var lst = GetTopNav();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.Write("<ul " + sCSS + " id=\"" + this.ClientID + "\">");
			foreach (var c in lst) {
				if (!c.PageActive) {
					c.NavMenuText = "&#9746; " + c.NavMenuText;
				}
				if (c.NavFileName.ToLower() == CurrentScriptName.ToLower() || c.NavFileName.ToLower() == sParent) {
					output.Write("<li class=\"" + CSSSelected + "\"><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				} else {
					output.Write("<li><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				}
			}
			output.Write("</ul>");
		}


	}
}
