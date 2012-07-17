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
	[ToolboxData("<{0}:SecondLevelNavigation runat=server></{0}:SecondLevelNavigation>")]
	public class SecondLevelNavigation : BaseServerControl {


		public bool IncludeParent { get; set; }

		public string SectionTitle { get; set; }


		protected List<SiteNav> GetSubNav() {
			return navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName, !SiteData.IsAuthEditor);
		}

		protected SiteNav GetParent(Guid? Root_ContentID) {
			SiteNav pageContents = null;
			if (Root_ContentID != null) {
				pageContents = navHelper.GetPageNavigation(SiteData.CurrentSiteID, new Guid(Root_ContentID.ToString()));
			}
			return pageContents;
		}


		protected override void RenderContents(HtmlTextWriter output) {
			var lst = GetSubNav();

			if (lst.Count > 0 && !string.IsNullOrEmpty(SectionTitle)) {
				output.Write("<h2>" + SectionTitle + "</h2>\r\n");
			}

			if (lst.Count > 0) {

				output.Write("<ul id=\"" + this.ClientID + "\">");

				if (IncludeParent) {
					if (lst.Count > 0) {
						var p = GetParent(lst.OrderByDescending(x => x.Parent_ContentID).FirstOrDefault().Parent_ContentID);
						if (!p.PageActive) {
							p.NavMenuText = "&#9746; " + p.NavMenuText;
						}
						if (p != null) {
							output.Write("<li class=\"parent-nav\"><a href=\"" + p.NavFileName + "\">" + p.NavMenuText + "</a></li>\r\n");
						}
					}
				}

				foreach (var c in lst) {
					if (!c.PageActive) {
						c.NavMenuText = "&#9746; " + c.NavMenuText;
					}
					if (c.NavFileName.ToLower() == SiteData.CurrentScriptName.ToLower()) {
						output.Write("<li class=\"selected\"><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					} else {
						output.Write("<li><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					}
				}
				output.Write("</ul>");
			} else {
				output.Write("<!--span id=\"" + this.ClientID + "\"></span -->");
			}
		}


	}
}
