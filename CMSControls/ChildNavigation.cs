using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
//  http://msdn.microsoft.com/en-us/library/yhzc935f.aspx

namespace Carrotware.CMS.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ChildNavigation runat=server></{0}:ChildNavigation>")]
	public class ChildNavigation : BaseServerControl, IWidget {

		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		Guid IWidget.SiteID { get; set; }

		public string JSEditFunction {
			get { return ""; }
		}
		public bool EnableEdit {
			get { return true; }
		}
		#endregion

		public bool IncludeParent { get; set; }

		public string SectionTitle { get; set; }


		protected List<SiteNav> GetSubNav() {
			return navHelper.GetChildNavigation(SiteID, CurrentScriptName, !IsAuthEditor);
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

			if (lst.Count > 0) {
				output.Write("<ul " + sCSS + " id=\"" + this.ClientID + "\">");
				foreach (var c in lst) {
					if (!c.PageActive) {
						c.NavMenuText = "&#9746; " + c.NavMenuText;
					}
					//if (c.NavFileName.ToLower() == CurrentScriptName.ToLower()) {
					//	output.Write("<li class=\"selected\"><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					//} else {
					output.Write("<li><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					//}
				}
				output.Write("</ul>");
			} else {
				output.Write("<!--span id=\"" + this.ClientID + "\"></span -->");
			}
		}


	}
}
