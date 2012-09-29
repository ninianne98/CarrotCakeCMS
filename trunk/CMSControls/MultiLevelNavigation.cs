using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Carrotware.CMS.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:MultiLevelNavigation runat=server></{0}:MultiLevelNavigation>")]
	public class MultiLevelNavigation : BaseServerControl {

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
			return navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
		}

		protected List<SiteNav> GetChildren(Guid rootContentID) {
			return navHelper.GetChildNavigation(SiteData.CurrentSiteID, rootContentID, !SecurityData.IsAuthEditor);
		}


		private void LoadChildren(HtmlTextWriter output, Guid rootContentID, int iLevel) {
			var cc = GetChildren(rootContentID);

			if (cc.Count > 0) {
				output.Write("\r\n\t<ul class=\"children level-" + iLevel + "\">\r\n");
				foreach (var c2 in cc) {
					IdentifyLinkAsInactive(c2);
					if (SiteData.IsFilenameCurrentPage(c2.FileName)) {
						output.Write("\t\t<li class=\"" + CSSSelected + " level-" + iLevel + "\"> ");
					} else {
						output.Write("\t\t<li class=\"level-" + iLevel + "\"> ");
					}
					output.Write("<a href=\"" + c2.FileName + "\">" + c2.NavMenuText + "</a> \r\n");
					LoadChildren(output, c2.Root_ContentID, iLevel + 1);
					output.Write("\r\n</li>\r\n");
				}
				output.Write("\t</ul>\r\n\t");
			}
		}


		protected override void RenderContents(HtmlTextWriter output) {

			SiteNav pageNav = GetParentPage();
			string sParent = pageNav.FileName.ToLower();

			var lst = GetTopNav();
			output.Write("<div name=\"" + this.UniqueID + "\" id=\"" + this.ClientID + "\">\r\n");
			output.Write("<div id=\"" + this.ClientID + "-inner\">\r\n");

			output.Write("<ul class=\"parent\">\r\n");
			foreach (var c1 in lst) {

				IdentifyLinkAsInactive(c1);
				if (SiteData.IsFilenameCurrentPage(c1.FileName) || AreFilenamesSame(c1.FileName, sParent)) {
					output.Write("\t<li class=\"" + CSSSelected + "\"> ");
				} else {
					output.Write("\t<li> ");
				}

				output.Write("<a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a>");
				LoadChildren(output, c1.Root_ContentID, 1);
				output.Write("</li>\r\n");
			}
			output.Write("</ul>\r\n");

			output.Write("</div>\r\n");
			output.Write("</div>\r\n");

		}


	}
}
