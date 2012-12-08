using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
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

		protected override void RenderContents(HtmlTextWriter output) {

			switch (RSSFeedType) {
				case SiteData.RSSFeedInclude.BlogAndPages:
					output.Write("<!-- RSS Unified Feed --> <link rel=\"alternate\" type=\"application/rss+xml\" title=\"RSS Feed\" href=\"/rss.aspx?type=BlogAndPages\" /> \r\n");
					break;
				case SiteData.RSSFeedInclude.BlogOnly:
					output.Write("<!-- RSS Blog Feed --> <link rel=\"alternate\" type=\"application/rss+xml\" title=\"RSS Feed\" href=\"/rss.aspx?type=BlogOnly\" /> \r\n");
					break;
				case SiteData.RSSFeedInclude.PageOnly:
					output.Write("<!-- RSS Content Feed --> <link rel=\"alternate\" type=\"application/rss+xml\" title=\"RSS Feed\" href=\"/rss.aspx?type=PageOnly\" /> \r\n");
					break;
				default:
					break;
			}
		}


	}

}
