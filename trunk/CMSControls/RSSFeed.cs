using System;
using System.ComponentModel;
using System.Web;
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
	[ToolboxData("<{0}:RSSFeed runat=server></{0}:RSSFeed>")]
	public class RSSFeed : BaseServerControl {

		public enum RSSRenderAs {
			Unknown,
			HeaderLink,
			ImageLink,
			TextLink
		}

		[Category("Appearance")]
		[DefaultValue("HeaderLink")]
		public RSSRenderAs RenderRSSMode {
			get {
				String s = (String)ViewState["RenderRSSMode"];
				RSSRenderAs c = RSSRenderAs.HeaderLink;
				if (!string.IsNullOrEmpty(s)) {
					c = (RSSRenderAs)Enum.Parse(typeof(RSSRenderAs), s, true);
				}
				return c;
			}
			set {
				ViewState["RenderRSSMode"] = value.ToString();
			}
		}

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
		[DefaultValue("BlogAndPages")]
		public SiteData.RSSFeedInclude RSSFeedType {
			get {
				String s = (String)ViewState["RSSFeedType"];
				SiteData.RSSFeedInclude c = SiteData.RSSFeedInclude.BlogAndPages;
				if (!string.IsNullOrEmpty(s)) {
					c = (SiteData.RSSFeedInclude)Enum.Parse(typeof(SiteData.RSSFeedInclude), s, true);
				}
				return c;
			}
			set {
				ViewState["RSSFeedType"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue("/rss.ashx")]
		public string RSSFeedURI {
			get {
				string s = (string)ViewState["RSSFeedURI"];
				return ((s == null) ? "/rss.ashx" : s);
			}
			set {
				ViewState["RSSFeedURI"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string ImageURI {
			get {
				string s = (string)ViewState["ImageURI"];
				if (string.IsNullOrEmpty(s)) {
					s = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.CMS.UI.Controls.feed.png");
				}
				try {
					s = HttpUtility.HtmlEncode(s);
				} catch { }
				return s;
			}
			set {
				ViewState["ImageURI"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string LinkText {
			get {
				string s = (string)ViewState["LinkText"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["LinkText"] = value;
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			switch (RenderRSSMode) {
				case RSSRenderAs.HeaderLink:

					switch (RSSFeedType) {
						case SiteData.RSSFeedInclude.BlogAndPages:
						case SiteData.RSSFeedInclude.BlogOnly:
						case SiteData.RSSFeedInclude.PageOnly:
							output.Write("<!-- RSS Header Feed --> <link rel=\"alternate\" type=\"application/rss+xml\" title=\"RSS Feed\" href=\"" + RSSFeedURI + "?type=" + RSSFeedType.ToString() + "\" /> \r\n");
							break;
						default:
							break;
					}

					break;
				case RSSRenderAs.ImageLink:

					switch (RSSFeedType) {
						case SiteData.RSSFeedInclude.BlogAndPages:
						case SiteData.RSSFeedInclude.BlogOnly:
						case SiteData.RSSFeedInclude.PageOnly:
							output.Write("<!-- RSS Feed Image Link--> <a " + sCSS + " title=\"" + RSSFeedType.ToString() + " RSS Feed\" href=\"" + RSSFeedURI + "?type=" + RSSFeedType.ToString() + "\" ><img alt=\"" + RSSFeedType.ToString() + "\" src=\"" + ImageURI + "\" /></a>\r\n");
							break;
						default:
							break;
					}

					break;
				case RSSRenderAs.TextLink:

					switch (RSSFeedType) {
						case SiteData.RSSFeedInclude.BlogAndPages:
						case SiteData.RSSFeedInclude.BlogOnly:
						case SiteData.RSSFeedInclude.PageOnly:
							output.Write("<!-- RSS Feed Text Link--> <a " + sCSS + " title=\"" + RSSFeedType.ToString() + " RSS Feed\" href=\"" + RSSFeedURI + "?type=" + RSSFeedType.ToString() + "\" >" + LinkText + "</a>\r\n");
							break;
						default:
							break;
					}

					break;
				default:
					break;
			}

		}

	}

}
