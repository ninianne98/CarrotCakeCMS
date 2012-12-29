using System.Web;
using System.Web.SessionState;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
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

			if (sFileRequested.ToLower().EndsWith("/rss.ashx")) {
				SiteData.CurrentSite.RenderRSSFeed(context);
			}

			if (sFileRequested.ToLower().EndsWith("/sitemap.ashx")) {
				context.Response.Write("Not Yet Supported\r\n");
				context.Response.Write(sFileRequested);
			}

			if (sFileRequested.ToLower().EndsWith("/trackback.ashx")) {
				TrackbackHelper tbh = new TrackbackHelper();
				if (SiteData.CurrentSite != null) {
					if (SiteData.CurrentSite.AcceptTrackbacks) {
						tbh.ProcessTrackback(context, true);
					} else {
						tbh.GenerateTrackBackDisabled(context);
					}
				}
			}

			//if (sFileRequested.ToLower().EndsWith("/xmlrpc.ashx")) {
			//    context.Response.Write("Not Yet Supported\r\n");
			//    context.Response.Write(sFileRequested);
			//}

			context.Response.End();
		}


	}
}
