using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using Carrotware.CMS.DBUpdater;
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
	public class VirtualDirectory : IRouteHandler {


		public VirtualDirectory(string virtualPath) {
			this.VirtualPath = virtualPath;
		}


		public string VirtualPath { get; private set; }


		private static string ContentKey = "cms_RegisterRoutes";
		public static bool HasRegisteredRoutes {
			get {
				bool c = false;
				if (HttpContext.Current.Cache[ContentKey] != null) {
					try { c = (bool)HttpContext.Current.Cache[ContentKey]; } catch { }
				}
				return c;
			}
			set {
				HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration);
			}
		}

		public static void RegisterRoutes(bool OverrideRefresh) {
			RegisterRoutes(RouteTable.Routes, OverrideRefresh);
		}

		public static void RegisterRoutes() {
			RegisterRoutes(RouteTable.Routes);
		}

		public static void RegisterRoutes(RouteCollection routes) {
			RegisterRoutes(routes, false);
		}


		public static void RegisterRoutes(RouteCollection routes, bool OverrideRefresh) {

			try {
				string sKeyPrefix = "CarrotCakeCMS_";

				if (!HasRegisteredRoutes || OverrideRefresh) {

					List<string> listFiles = SiteNavHelper.GetSiteDirectoryPaths();
					int iRoute = 0;
					List<Route> lstRoute = new List<Route>();

					//routes.Clear();
					//only remove routes that are tagged as coming from the CMS
					foreach (Route rr in routes) {
						if (rr.DataTokens != null && rr.DataTokens["RouteName"] != null && rr.DataTokens["RouteName"].ToString().StartsWith(sKeyPrefix)) {
							lstRoute.Add(rr);
						}
					}
					foreach (Route rr in lstRoute) {
						RouteTable.Routes.Remove(rr);
					}

					foreach (string fileName in listFiles) {
						string sKeyName = sKeyPrefix + iRoute.ToString();

						VirtualDirectory vd = new VirtualDirectory(fileName);
						Route r = new Route(fileName.Substring(1, fileName.LastIndexOf("/")), vd);
						if (r.DataTokens == null) {
							r.DataTokens = new RouteValueDictionary();
						}
						r.DataTokens["RouteName"] = sKeyName;
						routes.Add(sKeyName, r);

						iRoute++;
					}

					HasRegisteredRoutes = true;
				}

			} catch (Exception ex) {
				//assumption is database is probably empty / needs updating, so trigger the under construction view
				if (DatabaseUpdate.SystemNeedsChecking(ex) || DatabaseUpdate.AreCMSTablesIncomplete()) {
					routes.Clear();
					HasRegisteredRoutes = false;
				} else {
					//something bad has gone down, toss back the error
					throw;
				}
			}
		}


		#region IRouteHandler Members

		public IHttpHandler GetHttpHandler(RequestContext requestContext) {

			IHttpHandler p = new VirtualFileSystem();

			return p;
		}

		#endregion
	}
}