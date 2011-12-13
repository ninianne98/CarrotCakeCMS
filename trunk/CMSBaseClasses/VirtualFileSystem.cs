using System;
using System.Web.Compilation;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Base {

	public class VirtualFileSystem : IHttpHandler, IRequiresSessionState {

		private const string DEFAULT_FILE = "/default.aspx";
		private const string DEFAULT_TEMPLATE = "/Manage/PlainTemplate.aspx";
		private const string REQ_PATH = "RewriteOrigPath";
		private const string REQ_QUERY = "RewriteOrigQuery";

		private ContentPage pageHelper = new ContentPage();

		private Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

		private string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
		}



		public bool IsReusable {
			get {
				return false;
			}
		}



		private string sVirtualReqFile = "";
		private bool bAlreadyDone = false;


		public void ProcessRequest(HttpContext context) {

			string sFileRequested = context.Request.Path;

			if (context.User.Identity.IsAuthenticated) {
				try {
					if (!string.IsNullOrEmpty(context.Request.UrlReferrer.AbsolutePath)) {
						if (context.Request.UrlReferrer.AbsolutePath.ToLower().Contains(FormsAuthentication.LoginUrl.ToLower())
							|| FormsAuthentication.LoginUrl.ToLower() == sFileRequested.ToLower()) {
							sFileRequested = "/Manage/default.aspx";
						}
					}
				} catch (Exception ex) { }
			}


			if (sFileRequested.ToLower().EndsWith(".aspx") || sFileRequested.Length < 3) {

				string queryString = "";
				queryString = context.Request.QueryString.ToString();
				if (string.IsNullOrEmpty(queryString)) {
					queryString = "";
				}

				//if (sFileRequested.Replace(@"/", "").Length == sFileRequested.Length - 1) {
				if (!File.Exists(context.Server.MapPath(sFileRequested))
					|| sFileRequested.ToLower() == DEFAULT_FILE) {

					context.Items[REQ_PATH] = context.Request.PathInfo;
					context.Items[REQ_QUERY] = context.Request.QueryString.ToString();

					ContentPage filePage = null;

					if (sFileRequested.Length < 3 || sFileRequested.ToLower() == DEFAULT_FILE) {
						string sParm = "";
						if (context.Request.QueryString["tag"] != null) {
							sParm = context.Request.QueryString["tag"].ToString();
						}
						if (context.Request.QueryString["pg"] != null) {
							sParm = context.Request.QueryString["pg"].ToString();
						}
						if (!string.IsNullOrEmpty(sParm)) {
							sFileRequested = "/" + sParm + ".aspx";

							context.Response.StatusCode = 301;
							context.Response.AppendHeader("Status", "301 Moved Permanently");
							context.Response.AppendHeader("Location", sFileRequested);
							context.Response.Cache.SetLastModified(DateTime.Today.Date);
							context.Response.Write("<h2>301 Moved Permanently</h2>");

							context.Response.Redirect(sFileRequested);
							context.Items[REQ_PATH] = sFileRequested;
							context.Items[REQ_QUERY] = "";
						}
					}


					if (sFileRequested.Length < 3 || sFileRequested.ToLower() == DEFAULT_FILE) {
						filePage = pageHelper.FindHome(SiteID);
						if (sFileRequested.ToLower() == DEFAULT_FILE && filePage != null) {
							var sRedirect = filePage.NavFileName;
							if (string.IsNullOrEmpty(queryString)) {
								context.Response.Redirect(sRedirect);
							} else {
								context.Response.Redirect(sRedirect + "?" + queryString);
							}
						}
					}

					var pageName = sFileRequested; // pageHelper.StripSiteFolder(sFileRequested);
					if (pageHelper.AdvancedEditMode || pageHelper.IsAdmin || pageHelper.IsEditor) {
						filePage = pageHelper.GetLatestContent(SiteID, null, pageName);
					} else {
						filePage = pageHelper.GetLatestContent(SiteID, true, pageName);
					}

					bool bNoHome = false;
					if (sFileRequested.ToLower() == DEFAULT_FILE && filePage == null) {
						filePage = new ContentPage();
						filePage.TemplateFile = DEFAULT_FILE;
						filePage.EditDate = DateTime.Now.AddMinutes(-10);
						bNoHome = true;
					}


					if (filePage != null) {
						if (!sFileRequested.ToLower().Contains(filePage.TemplateFile.ToLower()) || bNoHome) {
							string sRealFile = filePage.TemplateFile;

							// selectivly engage the cms helper only if in advance mode
							if (pageHelper.AdvancedEditMode) {
								CMSConfigHelper cmsHelper = new CMSConfigHelper();
								if (cmsHelper.cmsAdminContent != null) {
									try { sRealFile = cmsHelper.cmsAdminContent.TemplateFile.ToLower(); } catch { }
								}
							}

							if (!File.Exists(context.Server.MapPath(sRealFile))) {
								sRealFile = DEFAULT_TEMPLATE;
							}

							sVirtualReqFile = sFileRequested;

							context.RewritePath(sFileRequested, string.Empty, queryString);

							//cannot work in med trust
							//Page hand = (Page)PageParser.GetCompiledPageInstance(sFileRequested, context.Server.MapPath(sRealFile), context);

							Page hand = (Page)BuildManager.CreateInstanceFromVirtualPath(sRealFile, typeof(Page));
							hand.PreRenderComplete += new EventHandler(hand_PreRenderComplete);
							hand.ProcessRequest(context);
						}
					} else {
						context.Response.StatusCode = 404;
						context.Response.AppendHeader("Status", "HTTP/1.1 404 Object Not Found");
						context.Response.Cache.SetLastModified(DateTime.Today.Date);
						context.Response.Write("<h2>404 Not Found</h2>");
						context.Response.End();
					}
				} else {
					sVirtualReqFile = sFileRequested;
					context.RewritePath(sVirtualReqFile, string.Empty, queryString);

					Page hand = (Page)BuildManager.CreateInstanceFromVirtualPath(sVirtualReqFile, typeof(Page));
					hand.PreRenderComplete += new EventHandler(hand_PreRenderComplete);
					hand.ProcessRequest(context);
				}
			}

			context.ApplicationInstance.CompleteRequest();
		}


		void hand_PreRenderComplete(object sender, EventArgs e) {
			if (!bAlreadyDone) {
				try {
					HttpContext.Current.RewritePath(sVirtualReqFile,
							HttpContext.Current.Items[REQ_PATH].ToString(),
							HttpContext.Current.Items[REQ_QUERY].ToString());
				} catch (Exception ex) { }
				bAlreadyDone = true;
			}
		}


		public void Dispose() { }


	}
}
