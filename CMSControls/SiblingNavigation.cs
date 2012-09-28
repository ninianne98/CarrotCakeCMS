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
	[ToolboxData("<{0}:SiblingNavigation runat=server></{0}:SiblingNavigation>")]
	public class SiblingNavigation : BaseServerControl  {

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

			var lst = navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName, !SecurityData.IsAuthEditor);

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
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName)) {
						output.Write("<li class=\"" + CSSSelected + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
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
