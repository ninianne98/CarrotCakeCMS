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
	[ToolboxData("<{0}:SiblingNavigation runat=server></{0}:SiblingNavigation>")]
	public class SiblingNavigation : BaseServerControl {


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


		public string SectionTitle { get; set; }


		protected override void RenderContents(HtmlTextWriter output) {

			var lst = navHelper.GetSiblingNavigation(SiteID, CurrentScriptName, !IsAuthEditor);

			if (lst.Count > 0 && !string.IsNullOrEmpty(SectionTitle)) {
				output.Write("<h2>" + SectionTitle + "</h2>\r\n");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.Write("<ul " + sCSS + " id=\"" + this.ClientID + "\">");
			foreach (var c in lst) {
				if (!c.PageActive) {
					c.NavMenuText = "&#9746; " + c.NavMenuText;
				}
				if (c.NavFileName.ToLower() == CurrentScriptName.ToLower()) {
					output.Write("<li class=\"" + CSSSelected + "\"><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				} else {
					output.Write("<li><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				}
			}
			output.Write("</ul>");
		}


	}
}
