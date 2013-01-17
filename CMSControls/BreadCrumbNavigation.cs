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
//  http://msdn.microsoft.com/en-us/library/yhzc935f.aspx

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:BreadCrumbNavigation runat=server></{0}:BreadCrumbNavigation>")]
	public class BreadCrumbNavigation : BaseServerControl {


		[DefaultValue(false)]
		[Themeable(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool DisplayAsList {
			get {
				bool s = false;
				if (ViewState["DisplayAsList"] != null) {
					try { s = (bool)ViewState["DisplayAsList"]; } catch { }
				}
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

			SiteNav pageNav = GetCurrentPage();
			string sParent = pageNav.FileName.ToLower();

			List<SiteNav> lstNav = navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, pageNav.Root_ContentID, !SecurityData.IsAuthEditor);
			lstNav.RemoveAll(x => x.ShowInSiteNav == false);

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

				output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
				foreach (SiteNav c in lstNav) {
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, sParent)) {
						output.WriteLine("<li class=\"" + sSelCSS + "\">" + c.NavMenuText + "</li> ");
					} else {
						output.WriteLine("<li" + sWrapCSS + "><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
					}
				}
				output.WriteLine("</ul>");

			} else {

				string sDivider = " " + TextDivider + " ";
				int iCtr = 1;
				int iMax = lstNav.Count;
				output.WriteLine("<div" + sCSS + " id=\"" + this.ClientID + "\">");
				foreach (SiteNav c in lstNav) {
					IdentifyLinkAsInactive(c);
					if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, sParent)) {
						output.WriteLine("<span class=\"" + sSelCSS + "\">" + c.NavMenuText + " " + sDivider + "</span> ");
					} else {
						output.WriteLine("<span" + sWrapCSS + "><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a> " + sDivider + "</span> ");
					}
					iCtr++;

					if (iCtr == iMax) {
						sDivider = "";
					}
				}
				output.WriteLine("</div>");

			}
		}


	}
}
