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
	[ToolboxData("<{0}:BreadCrumbNavigation runat=server></{0}:BreadCrumbNavigation>")]
	public class BreadCrumbNavigation : BaseServerControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public bool DisplayAsList {
			get {
				bool s = false;
				try { s = (bool)ViewState["DisplayAsList"]; } catch { }
				return s;
			}
			set {
				ViewState["DisplayAsList"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string TextDivider {
			get {
				String s = (String)ViewState["TextDivider"];
				return ((s == null) ? "&gt;" : s);
			}

			set {
				ViewState["TextDivider"] = value;
			}
		}

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
		public string CSSWrapper {
			get {
				string s = (string)ViewState["CSSWrapper"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSWrapper"] = value;
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {

			SiteNav pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName);
			string sParent = "";
			if (pageNav != null) {
				if (pageNav.Parent_ContentID == null) {
					sParent = pageNav.FileName.ToLower();
				}
			}

			List<SiteNav> lst = navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, pageNav.Root_ContentID, !SecurityData.IsAuthEditor);

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}
			string sSelCSS = (CSSSelected + " " + CSSWrapper).Trim();

			string sWrapCSS = "";
			if (!string.IsNullOrEmpty(CSSWrapper)) {
				sWrapCSS = " class=\"" + CSSWrapper + "\" ";
			}

			if (DisplayAsList) {

				output.Write("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
				foreach (SiteNav c in lst) {
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName) || c.FileName.ToLower() == sParent) {
						output.Write("<li class=\"" + sSelCSS + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					} else {
						output.Write("<li" + sWrapCSS + "><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li>\r\n");
					}
				}
				output.Write("</ul>");

			} else {

				string sDivider = " " + TextDivider + " ";
				int iCtr = 1;
				int iMax = lst.Count;
				output.Write("<div" + sCSS + " id=\"" + this.ClientID + "\">");
				foreach (SiteNav c in lst) {
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName) || c.FileName.ToLower() == sParent) {
						output.Write("<span class=\"" + sSelCSS + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a>" + sDivider + "</span> \r\n");
					} else {
						output.Write("<span" + sWrapCSS + "><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a>" + sDivider + "</span> \r\n");
					}
					iCtr++;

					if (iCtr == iMax) {
						sDivider = "";
					}
				}
				output.Write("</div>");

			}
		}


	}
}
