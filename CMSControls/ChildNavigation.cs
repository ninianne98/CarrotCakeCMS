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
	[ToolboxData("<{0}:ChildNavigation runat=server></{0}:ChildNavigation>")]
	public class ChildNavigation : BaseServerControl {


		public bool IncludeParent { get; set; }

		public string SectionTitle { get; set; }


		protected List<SiteNav> GetSubNav() {
			return navHelper.GetChildNavigation(SiteID, CurrentScriptName, !AdvancedEditMode);
		}

		protected override void RenderContents(HtmlTextWriter output) {
			var lst = GetSubNav();

			if (lst.Count > 0 && !string.IsNullOrEmpty(SectionTitle)) {
				output.Write("<h2>" + SectionTitle + "</h2>\r\n");
			}
			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.Write("<ul " + sCSS + " id=\"" + this.ClientID + "\">");

			foreach (var c in lst) {
				if (c.NavFileName.ToLower() == CurrentScriptName.ToLower()) {
					output.Write("<li class=\"selected\"><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				} else {
					output.Write("<li><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				}
			}
			output.Write("</ul>");
		}


	}
}
