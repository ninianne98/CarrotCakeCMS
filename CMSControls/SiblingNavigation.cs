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
	public class SiblingNavigation : BaseServerControl {

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


		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;
			List<SiteNav> lstNav = navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);

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
				foreach (SiteNav c in lstNav) {
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName)) {
						output.WriteLine("<li class=\"" + CSSSelected + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
					} else {
						output.WriteLine("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
					}
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
