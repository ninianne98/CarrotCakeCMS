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
			return navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor);
		}

		protected SiteNav GetParent(Guid? Root_ContentID) {
			SiteNav pageNav = null;
			if (Root_ContentID != null) {
				pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, new Guid(Root_ContentID.ToString()));
			}
			return pageNav;
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
						IdentifyLinkAsInactive(p);
						if (p != null) {
							output.Write("<li class=\"parent-nav\"><a href=\"" + p.FileName + "\">" + p.NavMenuText + "</a></li>\r\n");
						}
					}
				}

				foreach (var c in lst) {
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName)) {
						output.Write("<li class=\"selected\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					} else {
						output.Write("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					}
				}
				output.Write("</ul>");
			} else {
				output.Write("<!--span id=\"" + this.ClientID + "\"></span -->");
			}
		}


	}
}
