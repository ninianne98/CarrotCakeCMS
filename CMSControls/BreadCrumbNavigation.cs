using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
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
			var lstNav = new List<SiteNav>();
			var pageNav = GetCurrentPage();
			string currentPageFile = pageNav.FileName.ToLowerInvariant();

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

			lstNav = CMSConfigHelper.TweakData(lstNav, false, true);

			var outerTag = new HtmlTag("ul");
			outerTag.SetAttribute("id", this.ClientID);
			outerTag.MergeAttribute("class", this.CssClass);

			if (this.DisplayAsList) {
				outerTag = new HtmlTag("ul");

				output.WriteLine(outerTag.OpenTag());

				foreach (SiteNav c in lstNav) {
					var item = new HtmlTag("li");
					var link = new HtmlTag("a");

					item.MergeAttribute("class", this.CSSWrapper);
					link.Uri = c.FileName;
					link.InnerHtml = c.NavMenuText;

					if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, currentPageFile)) {
						item.MergeAttribute("class", this.CSSSelected);
						item.InnerHtml = c.NavMenuText;
					} else {
						item.InnerHtml = link.RenderTag();
					}

					output.WriteLine(item.RenderTag());
				}

				output.WriteLine(outerTag.CloseTag());
			} else {
				outerTag = new HtmlTag("div");

				output.WriteLine(outerTag.OpenTag());

				foreach (SiteNav c in lstNav) {
					var item = new HtmlTag("span");
					var link = new HtmlTag("a");

					item.MergeAttribute("class", this.CSSWrapper);
					link.Uri = c.FileName;
					link.InnerHtml = c.NavMenuText;

					if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, currentPageFile)) {
						item.MergeAttribute("class", this.CSSSelected);
						item.InnerHtml = c.NavMenuText;
					} else {
						item.InnerHtml = link.RenderTag() + string.Format(" {0} ", this.TextDivider);
					}

					output.WriteLine(item.RenderTag());
				}

				output.WriteLine(outerTag.CloseTag());
			}
		}
	}
}