using Carrotware.CMS.DBUpdater;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Routing;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.Core {

	public class VirtualDirectory : IRouteHandler {

		public VirtualDirectory(string virtualPath) {
			this.VirtualPath = virtualPath;
		}

		public string VirtualPath { get; private set; }

		private static string _keyRegister = "cmsRegisterRoutes";

		private static string _dataTokenId = "cmsCarrotCakeRoute";

		public static bool HasRegisteredRoutes {
			get {
				bool reg = false;
				if (HttpContext.Current.Cache[_keyRegister] != null) {
					try { reg = (bool)HttpContext.Current.Cache[_keyRegister]; } catch { }
				}
				return reg;
			}
			set {
				HttpContext.Current.Cache.Insert(_keyRegister, value, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
			}
		}

		public static void RegisterWebApi(HttpConfiguration config) {
			if (config.Routes.ContainsKey("MS_attributerouteWebApi")) {
				SiteData.WriteDebugException("webapiconfig", new Exception("Routing already registered. Skipping."));
			} else {
				config.MapHttpAttributeRoutes();

				var apiPath = SiteData.ApiBasePath.TrimPathSlashes();

				config.Routes.MapHttpRoute(
					name: "C3_AdminApi_Default",
					routeTemplate: apiPath + "/{action}/{id}",
					defaults: new { controller = "CmsAdminApi", action = "Index", id = RouteParameter.Optional }
				);

				config.Formatters.Remove(config.Formatters.XmlFormatter);
			}
		}

		public static void RegisterRoutes(bool OverrideRefresh) {
			RegisterRoutes(RouteTable.Routes, OverrideRefresh);
			GlobalConfiguration.Configure(RegisterWebApi);
		}

		public static void RegisterRoutes() {
			RegisterRoutes(RouteTable.Routes);
		}

		public static void RegisterRoutes(RouteCollection routes) {
			RegisterRoutes(routes, false);
		}

		public static void RegisterRoutes(RouteCollection routes, bool overrideRefresh) {
			try {
				string key = "RouteName";

				if (!HasRegisteredRoutes || overrideRefresh) {
					var lstRoute = new List<Route>();
					List<string> lstFiles = SiteNavHelper.GetSiteDirectoryPaths();

					//routes.Clear();
					//only remove routes that are tagged as coming from the CMS
					foreach (Route rr in routes) {
						if (rr.DataTokens != null && rr.DataTokens.ContainsKey(key)
									&& rr.DataTokens[key].ToString().StartsWith(_dataTokenId)) {
							lstRoute.Add(rr);
						}
					}

					foreach (Route rr in lstRoute) {
						RouteTable.Routes.Remove(rr);
					}

					int routeId = 0;

					foreach (string fileName in lstFiles) {
						string routeKey = string.Format("{0}_{1}", _dataTokenId, routeId);

						var vd = new VirtualDirectory(fileName);
						var rr = new Route(fileName.Substring(1, fileName.LastIndexOf("/")), vd);

						if (rr.DataTokens == null) {
							rr.DataTokens = new RouteValueDictionary();
						}
						rr.DataTokens[key] = routeKey;
						routes.Add(routeKey, rr);

						routeId++;
					}

					HasRegisteredRoutes = true;
				}
			} catch (Exception ex) {
				var du = new DatabaseUpdate();
				//assumption is database is probably empty / needs updating, so trigger the under construction view
				if (ex.SystemNeedsChecking() || du.DatabaseNeedsUpdate()) {
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

		#endregion IRouteHandler Members
	}
}