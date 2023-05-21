using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.ComponentModel;
using System.Web;
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
				if (!String.IsNullOrEmpty(s)) {
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
				if (!String.IsNullOrEmpty(s)) {
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
				return ((s == null) ? SiteFilename.RssFeedUri : s);
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
				if (String.IsNullOrEmpty(s)) {
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

			switch (RenderRSSMode) {
				case RSSRenderAs.HeaderLink:

					switch (this.RSSFeedType) {
						case SiteData.RSSFeedInclude.BlogAndPages:
						case SiteData.RSSFeedInclude.BlogOnly:
						case SiteData.RSSFeedInclude.PageOnly:
							var link = new HtmlTag("link");
							link.Uri = string.Format("{0}?type={1}", this.RSSFeedURI, this.RSSFeedType);
							link.MergeAttribute("rel", "alternate");
							link.MergeAttribute("type", "application/rss+xml");
							link.MergeAttribute("title", EnumHelper.GetDescription<SiteData.RSSFeedInclude>(this.RSSFeedType.ToString()) + " RSS Feed");

							output.Write("<!-- RSS Header Feed --> " + link.RenderSelfClosingTag() + " \r\n");
							break;

						default:
							break;
					}

					break;

				case RSSRenderAs.ImageLink:

					switch (this.RSSFeedType) {
						case SiteData.RSSFeedInclude.BlogAndPages:
						case SiteData.RSSFeedInclude.BlogOnly:
						case SiteData.RSSFeedInclude.PageOnly:
							var img = new HtmlTag(HtmlTag.EasyTag.Image);
							img.Uri = GeneralUtilities.ResolvePath(this, this.ImageURI);
							img.MergeAttribute("title", EnumHelper.GetDescription<SiteData.RSSFeedInclude>(this.RSSFeedType.ToString()) + " RSS Feed");

							var link = new HtmlTag(HtmlTag.EasyTag.AnchorTag);
							link.Uri = string.Format("{0}?type={1}", this.RSSFeedURI, this.RSSFeedType);
							link.MergeAttribute("class", this.CssClass);
							link.MergeAttribute("rel", "alternate");
							link.MergeAttribute("type", "application/rss+xml");
							link.MergeAttribute("title", EnumHelper.GetDescription<SiteData.RSSFeedInclude>(this.RSSFeedType.ToString()) + " RSS Feed");
							link.InnerHtml = img.RenderSelfClosingTag();

							output.Write("<!-- RSS Feed Image Link--> " + link.RenderTag() + "\r\n");
							break;

						default:
							break;
					}

					break;

				case RSSRenderAs.TextLink:

					switch (this.RSSFeedType) {
						case SiteData.RSSFeedInclude.BlogAndPages:
						case SiteData.RSSFeedInclude.BlogOnly:
						case SiteData.RSSFeedInclude.PageOnly:
							var link = new HtmlTag(HtmlTag.EasyTag.AnchorTag);
							link.Uri = string.Format("{0}?type={1}", this.RSSFeedURI, this.RSSFeedType);
							link.MergeAttribute("class", this.CssClass);
							link.MergeAttribute("rel", "alternate");
							link.MergeAttribute("type", "application/rss+xml");
							link.MergeAttribute("title", EnumHelper.GetDescription<SiteData.RSSFeedInclude>(this.RSSFeedType.ToString()) + " RSS Feed");
							link.InnerHtml = this.LinkText;

							output.Write("<!-- RSS Feed Text Link--> " + link.RenderTag() + "\r\n");
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