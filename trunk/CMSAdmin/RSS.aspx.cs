using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin {
	public partial class RSS : BasePage {

		protected void Page_Load(object sender, EventArgs e) {
			SiteData.RSSFeedInclude FeedType = SiteData.RSSFeedInclude.BlogAndPages;

			if (!string.IsNullOrEmpty(Request.QueryString["type"])) {
				string feedType = Request.QueryString["type"].ToString();

				FeedType = (SiteData.RSSFeedInclude)Enum.Parse(typeof(SiteData.RSSFeedInclude), feedType, true);
			}

			string sRSSXML = SiteData.CurrentSite.GetRSSFeed(FeedType);

			Response.ContentType = SiteData.RssDocType;

			Response.Write(sRSSXML);

			Response.StatusCode = 200;
			Response.StatusDescription = "OK";
			Response.End();

		}
	}
}