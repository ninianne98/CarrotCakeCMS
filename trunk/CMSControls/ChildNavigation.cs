using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Carrotware.CMS.Core;
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
	public class ChildNavigation : BaseServerControl {


		public bool IncludeParent { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string SectionTitle {
			get {
				string s = (string)ViewState["SectionTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["SectionTitle"] = value;
			}
		}


		protected List<SiteNav> GetSubNav() {
			return navHelper.GetChildNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor);
		}

		protected SiteNav GetParent(Guid? Root_ContentID) {
			SiteNav pageNav = null;
			if (Root_ContentID != null) {
				pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, new Guid(Root_ContentID.ToString()));
			}
			return pageNav;
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<SiteNav> lstNav = GetSubNav();

			output.Indent = indent + 3;
			output.WriteLine();

			if (lstNav.Count > 0 && !string.IsNullOrEmpty(SectionTitle)) {
				output.WriteLine("<h2>" + SectionTitle + "</h2> ");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			if (lstNav.Count > 0) {
				output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
				output.Indent++;

				if (IncludeParent) {
					if (lstNav.Count > 0) {
						SiteNav p = GetParent(lstNav.OrderByDescending(x => x.Parent_ContentID).FirstOrDefault().Parent_ContentID);
						IdentifyLinkAsInactive(p);
						if (p != null) {
							output.WriteLine("<li class=\"parent-nav\"><a href=\"" + p.FileName + "\">" + p.NavMenuText + "</a></li> ");
						}
					}
				}

				foreach (SiteNav c in lstNav) {
					IdentifyLinkAsInactive(c);

					output.WriteLine("<li class=\"child-nav\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				}
				output.Indent--;
				output.WriteLine("</ul>");
			} else {
				output.WriteLine("<!--span id=\"" + this.ClientID + "\"></span -->");
			}

			output.Indent = indent;
		}


	}
}
