using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:OpenGraph runat=server></{0}:OpenGraph>")]
	public class OpenGraph : BaseServerControl {

		public enum OpenGraphTypeDef {
			Default,
			Article,
			Blog,
			Website,
			Book,
			Video,
			Movie,
			Profile
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				string s = (string)ViewState["EnableViewState"];
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
		public bool ShowExpirationDate {
			get {
				string s = (string)ViewState["ShowExpirationDate"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				return b;
			}

			set {
				ViewState["ShowExpirationDate"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue("Default")]
		public OpenGraphTypeDef OpenGraphType {
			get {
				string s = (string)ViewState["OpenGraphType"];
				OpenGraphTypeDef c = OpenGraphTypeDef.Default;
				if (!string.IsNullOrEmpty(s)) {
					c = (OpenGraphTypeDef)Enum.Parse(typeof(OpenGraphTypeDef), s, true);
				}
				return c;
			}
			set {
				ViewState["OpenGraphType"] = value.ToString();
			}
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			foreach (Control c in this.Controls) {
				c.RenderControl(writer);
				writer.WriteLine();
			}
		}

		private ControlUtilities _cu = new ControlUtilities();

		protected override void OnPreRender(EventArgs e) {
			this.Controls.Clear();

			try {
				ContentPage cp = _cu.GetContainerContentPage(this);
				SiteData theSite = SiteData.CurrentSite;

				if (cp != null) {
					this.Controls.AddHtmlMetaProp("og:description", string.IsNullOrEmpty(cp.MetaDescription) ? theSite.MetaDescription : cp.MetaDescription);

					this.Controls.AddHtmlMetaProp("og:url", theSite.DefaultCanonicalURL);

					string ogTypeContent;
					if (this.OpenGraphType == OpenGraphTypeDef.Default) {
						if (cp.ContentType == ContentPageType.PageType.BlogEntry) {
							ogTypeContent = OpenGraphTypeDef.Blog.ToString().ToLowerInvariant();
						} else {
							ogTypeContent = OpenGraphTypeDef.Article.ToString().ToLowerInvariant();
						}
						if (theSite.Blog_Root_ContentID.HasValue && cp.Root_ContentID == theSite.Blog_Root_ContentID) {
							ogTypeContent = OpenGraphTypeDef.Website.ToString().ToLowerInvariant();
						}
					} else {
						ogTypeContent = this.OpenGraphType.ToString().ToLowerInvariant();
					}
					this.Controls.AddHtmlMetaProp("og:type", ogTypeContent);

					if (!string.IsNullOrEmpty(this.Page.Title)) {
						this.Controls.AddHtmlMetaProp("og:title", cp.TitleBar);
					}

					if (!string.IsNullOrEmpty(cp.Thumbnail)) {
						this.Controls.AddHtmlMetaProp("og:image", string.Format("{0}/{1}", theSite.MainCanonicalURL, cp.Thumbnail).Replace(@"//", @"/").Replace(@"//", @"/").Replace(@":/", @"://"));
					}

					if (!string.IsNullOrEmpty(theSite.SiteName)) {
						this.Controls.AddHtmlMetaProp("og:site_name", theSite.SiteName);
					}

					this.Controls.AddHtmlMetaProp("article:published_time", theSite.ConvertSiteTimeToISO8601(cp.GoLiveDate));
					this.Controls.AddHtmlMetaProp("article:modified_time", theSite.ConvertSiteTimeToISO8601(cp.EditDate));

					if (ShowExpirationDate) {
						this.Controls.AddHtmlMetaProp("article:expiration_time", theSite.ConvertSiteTimeToISO8601(cp.RetireDate));
					}
				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}
	}

	//=====================
	[ToolboxData("<{0}:SocialMetaTag runat=server></{0}:SocialMetaTag>")]
	public class SocialMetaTag : BaseServerControl {

		[Category("Appearance")]
		[DefaultValue("")]
		public string TwitterSite {
			get {
				string s = (string)ViewState["TwitterSite"];
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["TwitterSite"] = value;
			}
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			foreach (Control c in this.Controls) {
				c.RenderControl(writer);
				writer.WriteLine();
			}
		}

		private ControlUtilities _cu = new ControlUtilities();
		private ISiteNavHelper _navHelper = SiteNavFactory.GetSiteNavHelper();

		protected override void OnPreRender(EventArgs e) {
			this.Controls.Clear();

			try {
				ContentPage cp = _cu.GetContainerContentPage(this);
				SiteData site = SiteData.CurrentSite;
				string culture = CultureInfo.CurrentUICulture.Name.Replace("-", "_");

				if (cp != null) {
					// Resolve Page Mapping
					bool isHome = cp.NavOrder == 0;
					string siteName = site != null ? site.SiteName : string.Empty;
					string pageTitle = !string.IsNullOrEmpty(cp.TitleBar) ? cp.TitleBar : (cp.PageHead ?? cp.NavMenuText ?? string.Empty);
					string pageDesc = cp.MetaDescription ?? string.Empty;
					if (string.IsNullOrEmpty(pageDesc)) {
						pageDesc = cp.PageTextPlainSummary.ToString() ?? string.Empty;
					}
					if (string.IsNullOrEmpty(pageDesc)) {
						pageDesc = cp.NavMenuText;
					}

					//string absoluteUrl = HttpContext.Current.Request.Url.AbsoluteUri;
					string absoluteUrl = cp.GetDefaultUri();
					string absoluteImageUrl = string.IsNullOrWhiteSpace(cp.Thumbnail) == false ?
										VirtualPathUtility.ToAbsolute(cp.Thumbnail) : string.Empty;

					string twitterHandle = this.TwitterSite;
					if (string.IsNullOrEmpty(twitterHandle)) {
						if (ConfigurationManager.AppSettings["carrot:TwitterAccount"] != null) {
							var ta = ConfigurationManager.AppSettings["carrot:TwitterAccount"].ToString();
							if (!string.IsNullOrWhiteSpace(ta)) {
								twitterHandle = ta;
							}
						}
					}

					if (!string.IsNullOrWhiteSpace(twitterHandle) && !twitterHandle.StartsWith("@")) {
						twitterHandle = "@" + twitterHandle;
					}

					// Common Open Graph Tags
					this.Controls.AddLiteral(Environment.NewLine + "<!-- Open Graph Meta Tags -->");
					this.Controls.AddHtmlMetaProp("og:site_name", siteName);
					this.Controls.AddHtmlMetaProp("og:locale", culture);
					this.Controls.AddHtmlMetaProp("og:title", pageTitle);
					this.Controls.AddHtmlMetaProp("og:description", pageDesc);
					this.Controls.AddHtmlMetaProp("og:url", absoluteUrl);

					var pageType = isHome ? "frontpage" : (cp.IsBlogPost ? "article" : "website");
					this.Controls.AddHtmlMetaProp("og:type", pageType);

					if (cp.IsBlogPost) {
						this.Controls.AddHtmlMetaProp("article:published_time", site.ConvertSiteTimeToISO8601(cp.GoLiveDate));
						this.Controls.AddHtmlMetaProp("article:modified_time", site.ConvertSiteTimeToISO8601(cp.EditDate));

						if (cp.BylineUser != null && !string.IsNullOrEmpty(cp.BylineUser.FullName_FirstLast)
									&& cp.BylineUser.FullName_FirstLast != cp.BylineUser.UserName) {
							this.Controls.AddHtmlMetaProp("article:author", cp.BylineUser.FullName_FirstLast);
						}

						var tags = base._navHelper.GetTagListForPost(SiteData.CurrentSiteID, 10, SiteData.CurrentScriptName);
						var cats = base._navHelper.GetCategoryListForPost(SiteData.CurrentSiteID, 10, SiteData.CurrentScriptName);

						foreach (var pc in cats) {
							this.Controls.AddHtmlMetaProp("article:section", pc.MetaInfoText);
						}
						foreach (var pt in tags) {
							this.Controls.AddHtmlMetaProp("article:tag", pt.MetaInfoText);
						}
					}

					// Convert and resolve virtual paths to absolute application URLs
					if (!string.IsNullOrEmpty(absoluteImageUrl)) {
						this.Controls.AddHtmlMetaProp("og:image", absoluteImageUrl);
					}

					// Output Twitter Card Tags
					this.Controls.AddLiteral(Environment.NewLine + "<!-- Twitter Card Meta Tags -->");
					if (!string.IsNullOrEmpty(twitterHandle)) {
						this.Controls.AddHtmlMetaProp("twitter:site", twitterHandle);
					}
					this.Controls.AddHtmlMetaProp("twitter:card", "summary"); // or summary_large_image
					this.Controls.AddHtmlMetaProp("twitter:title", pageTitle);
					this.Controls.AddHtmlMetaProp("twitter:description", pageDesc);

					if (!string.IsNullOrEmpty(absoluteImageUrl)) {
						this.Controls.AddHtmlMetaProp("twitter:image", absoluteImageUrl);
					}
					this.Controls.AddLiteral(string.Empty);
				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}
	}
}