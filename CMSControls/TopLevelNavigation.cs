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
	[ToolboxData("<{0}:TopLevelNavigation runat=server></{0}:TopLevelNavigation>")]
	public class TopLevelNavigation : BaseServerControl  {

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

			SiteNav pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName);
			var sParent = "";
			if (pageNav != null) {
				if (pageNav.Parent_ContentID == null) {
					sParent = pageNav.FileName.ToLower();
				}
			}

			List<SiteNav> lst = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.Write("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
			foreach (SiteNav c in lst) {
				IdentifyLinkAsInactive(c);
				if (SiteData.IsFilenameCurrentPage(c.FileName) || c.FileName.ToLower() == sParent) {
					output.Write("<li class=\"" + CSSSelected + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				} else {
					output.Write("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				}
			}
			output.Write("</ul>");
		}


	}
}
