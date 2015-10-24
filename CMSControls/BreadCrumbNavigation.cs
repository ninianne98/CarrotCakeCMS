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

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:BreadCrumbNavigation runat=server></{0}:BreadCrumbNavigation>")]
	public class BreadCrumbNavigation : BaseServerControl {

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue("&gt;")]
		public string TextDivider {
			get {
				String s = (String)ViewState["TextDivider"];
				return ((s == null) ? "&gt;" : s);
			}

			set {
				ViewState["TextDivider"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("selected")]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
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
			List<SiteNav> lstNav = new List<SiteNav>();

			if (SiteData.CurretSiteExists && SiteData.CurrentSite.Blog_Root_ContentID.HasValue &&
				pageNav.ContentType == ContentPageType.PageType.BlogEntry) {
				lstNav = navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, SiteData.CurrentSite.Blog_Root_ContentID.Value, !SecurityData.IsAuthEditor);

				if (lstNav != null && lstNav.Any()) {
					pageNav.NavOrder = lstNav.Max(x => x.NavOrder) + 100;
					lstNav.Add(pageNav);
				}
			} else {
				lstNav = navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, pageNav.Root_ContentID, !SecurityData.IsAuthEditor);
			}
			lstNav.RemoveAll(x => x.ShowInSiteNav == false && x.ContentType == ContentPageType.PageType.ContentEntry);
			lstNav.ToList().ForEach(q => IdentifyLinkAsInactive(q));

			string sCSS = String.Empty;
			if (!String.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}
			string sSelCSS = (CSSSelected + " " + CSSWrapper).Trim();

			string sWrapCSS = String.Empty;
			if (!String.IsNullOrEmpty(CSSWrapper)) {
				sWrapCSS = " class=\"" + CSSWrapper + "\" ";
			}

			if (DisplayAsList) {
				output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
				foreach (SiteNav c in lstNav) {
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
					if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, sParent)) {
						output.WriteLine("<span class=\"" + sSelCSS + "\">" + c.NavMenuText + " " + sDivider + "</span> ");
					} else {
						output.WriteLine("<span" + sWrapCSS + "><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a> " + sDivider + "</span> ");
					}
					iCtr++;

					if (iCtr == iMax) {
						sDivider = String.Empty;
					}
				}
				output.WriteLine("</div>");
			}
		}
	}
}