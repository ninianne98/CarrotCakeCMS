using System;
using System.Linq;
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

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			SiteNav pageNav = GetParentPage();
			string sParent = pageNav.FileName.ToLower();

			List<SiteNav> lstNav = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);

			output.Indent = indent + 3;
			output.WriteLine();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
			output.Indent++;

			foreach (SiteNav c in lstNav) {
				IdentifyLinkAsInactive(c);
				if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, sParent)) {
					output.WriteLine("<li class=\"" + CSSSelected + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				} else {
					output.WriteLine("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				}
			}

			output.Indent--;
			output.WriteLine("</ul>");

			output.Indent = indent;
		}


	}
}
