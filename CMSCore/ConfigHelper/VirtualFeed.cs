using System.Web;
using System.Web.SessionState;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class VirtualFeed : IHttpHandler, IRequiresSessionState {

		public bool IsReusable {
			get {
				return false;
			}
		}

		public void ProcessRequest(HttpContext context) {
			string sFileRequested = context.Request.Path;

			if (sFileRequested.ToLowerInvariant().EndsWith("/rss.ashx")) {
				SiteData.CurrentSite.RenderRSSFeed(context);
			}

			if (sFileRequested.ToLowerInvariant().EndsWith("/sitemap.ashx")) {
				SiteMapHelper smh = new SiteMapHelper();
				smh.RenderSiteMap(context);
			}

			if (sFileRequested.ToLowerInvariant().EndsWith("/trackback.ashx")) {
				TrackbackHelper tbh = new TrackbackHelper();
				if (SiteData.CurretSiteExists) {
					if (SiteData.CurrentSite.AcceptTrackbacks) {
						tbh.ProcessTrackback(context, true);
					} else {
						tbh.GenerateTrackBackDisabled(context);
					}
				}
			}

			context.Response.End();
		}
	}
}