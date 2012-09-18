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
* http://www.carrotware.com/
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
	public class ChildNavigation : BaseServerControl, IWidgetParmData, IWidget {

		#region IWidgetParmData Members

		private Dictionary<string, string> _parms = new Dictionary<string, string>();
		public Dictionary<string, string> PublicParmValues {
			get { return _parms; }
			set { _parms = value; }
		}

		#endregion

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
			return navHelper.GetChildNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor);
		}

		protected SiteNav GetParent(Guid? Root_ContentID) {
			SiteNav pageContents = null;
			if (Root_ContentID != null) {
				pageContents = navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, new Guid(Root_ContentID.ToString()));
			}
			return pageContents;
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

				if (IncludeParent) {
					if (lst.Count > 0) {
						var p = GetParent(lst.OrderByDescending(x => x.Parent_ContentID).FirstOrDefault().Parent_ContentID);
						if (!p.PageActive) {
							p.NavMenuText = InactivePagePrefix + p.NavMenuText;
						}
						if (p != null) {
							output.Write("<li class=\"parent-nav\"><a href=\"" + p.NavFileName + "\">" + p.NavMenuText + "</a></li>\r\n");
						}
					}
				}

				foreach (var c in lst) {
					if (!c.PageActive) {
						c.NavMenuText = InactivePagePrefix + c.NavMenuText;
					}

					output.Write("<li class=\"child-nav\"><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				}
				output.Write("</ul>");
			} else {
				output.Write("<!--span id=\"" + this.ClientID + "\"></span -->");
			}
		}


	}
}
