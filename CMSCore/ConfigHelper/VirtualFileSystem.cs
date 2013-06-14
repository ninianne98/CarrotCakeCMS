using System;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
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

	public class VirtualFileSystem : IHttpHandler, IRequiresSessionState, IDisposable {

		private const string REQ_PATH = "RewriteOrigPath";
		private const string REQ_QUERY = "RewriteOrigQuery";

		private SiteNavHelper navHelper = new SiteNavHelper();

		public bool IsReusable {
			get {
				return false;
			}
		}

		private string sVirtualReqFile = String.Empty;
		private string sRequestedURL = String.Empty;
		private bool bAlreadyDone = false;
		private bool bURLOverride = false;


		public void ProcessRequest(HttpContext context) {
			SiteNav navData = null;
			string sFileRequested = context.Request.Path;
			sRequestedURL = sFileRequested;
			string sScrubbedURL = sFileRequested;

			sRequestedURL = SiteData.AppendDefaultPath(sRequestedURL);

			try {
				sScrubbedURL = SiteData.AlternateCurrentScriptName;

				if (sScrubbedURL.ToLower() != sRequestedURL.ToLower()) {
					sFileRequested = sScrubbedURL;
					bURLOverride = true;
				}

				VirtualDirectory.RegisterRoutes();

			} catch (Exception ex) {
				//assumption is database is probably empty / needs updating, so trigger the under construction view
				if (DatabaseUpdate.SystemNeedsChecking(ex) || DatabaseUpdate.AreCMSTablesIncomplete()) {
					if (navData == null) {
						navData = SiteNavHelper.GetEmptyHome();
					}
				} else {
					//something bad has gone down, toss back the error
					throw;
				}
			}

			sFileRequested = SiteData.AppendDefaultPath(sFileRequested);

			if (context.User.Identity.IsAuthenticated) {
				try {
					if (context.Request.UrlReferrer != null && !string.IsNullOrEmpty(context.Request.UrlReferrer.AbsolutePath)) {
						if (context.Request.UrlReferrer.AbsolutePath.ToLower().Contains(FormsAuthentication.LoginUrl.ToLower())
							|| FormsAuthentication.LoginUrl.ToLower() == sFileRequested.ToLower()) {
							if (SiteFilename.DashboardURL.ToLower() != sFileRequested.ToLower()
							&& SiteFilename.SiteInfoURL.ToLower() != sFileRequested.ToLower()) {
								sFileRequested = SiteData.AdminDefaultFile;
							}
						}
					}
				} catch (Exception ex) { }
			}


			if (sFileRequested.ToLower().EndsWith(".aspx") || sFileRequested.Length < 3) {
				bool bIgnorePublishState = SecurityData.AdvancedEditMode || SecurityData.IsAdmin || SecurityData.IsEditor;

				string queryString = String.Empty;
				queryString = context.Request.QueryString.ToString();
				if (string.IsNullOrEmpty(queryString)) {
					queryString = String.Empty;
				}

				if (!CMSConfigHelper.CheckRequestedFileExistence(sFileRequested, SiteData.CurrentSiteID) || sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename) {

					context.Items[REQ_PATH] = context.Request.PathInfo;
					context.Items[REQ_QUERY] = context.Request.QueryString.ToString();

					// handle a case where this site was migrated from a format where all pages varied on a consistent querystring
					// allow this QS parm to be set in a config file.
					if (sFileRequested.Length < 3 || sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename) {
						string sParm = String.Empty;
						if (SiteData.OldSiteQuerystring != string.Empty) {
							if (context.Request.QueryString[SiteData.OldSiteQuerystring] != null) {
								sParm = context.Request.QueryString[SiteData.OldSiteQuerystring].ToString();
							}
						}
						if (!string.IsNullOrEmpty(sParm)) {
							sFileRequested = "/" + sParm + ".aspx";

							SiteData.Show301Message(sFileRequested);

							context.Response.Redirect(sFileRequested);
							context.Items[REQ_PATH] = sFileRequested;
							context.Items[REQ_QUERY] = String.Empty;
						}
					}

					try {
						bool bIsHomePage = false;
						if (sFileRequested.Length < 3 || sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename) {

							navData = navHelper.FindHome(SiteData.CurrentSiteID, !bIgnorePublishState);

							if (sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename && navData != null) {
								sFileRequested = navData.FileName;
								bIsHomePage = true;
							}
						}

						if (!bIsHomePage) {
							string pageName = sFileRequested;
							navData = navHelper.GetLatestVersion(SiteData.CurrentSiteID, !bIgnorePublishState, pageName);
						}

						if (sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename && navData == null) {
							navData = SiteNavHelper.GetEmptyHome();
						}

					} catch (Exception ex) {
						//assumption is database is probably empty / needs updating, so trigger the under construction view
						if (DatabaseUpdate.SystemNeedsChecking(ex) || DatabaseUpdate.AreCMSTablesIncomplete()) {
							if (navData == null) {
								navData = SiteNavHelper.GetEmptyHome();
							}
						} else {
							//something bad has gone down, toss back the error
							throw;
						}
					}

					if (navData != null) {
						string sSelectedTemplate = navData.TemplateFile;

						// selectivly engage the cms helper only if in advance mode
						if (SecurityData.AdvancedEditMode) {
							using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
								if (cmsHelper.cmsAdminContent != null) {
									try { sSelectedTemplate = cmsHelper.cmsAdminContent.TemplateFile.ToLower(); } catch { }
								}
							}
						}

						if (!CMSConfigHelper.CheckFileExistence(sSelectedTemplate)) {
							sSelectedTemplate = SiteData.DefaultTemplateFilename;
						}

						sVirtualReqFile = sFileRequested;

						if (bURLOverride) {
							sVirtualReqFile = sRequestedURL;
							sFileRequested = sRequestedURL;
						}

						RewriteCMSPath(context, sSelectedTemplate, queryString);

					} else {

						SiteData.PerformRedirectToErrorPage(404, sFileRequested);
						SiteData.Show404MessageFull(true);

					}

				} else {
					sVirtualReqFile = sFileRequested;

					RewriteCMSPath(context, sVirtualReqFile, queryString);
				}
			}

			context.ApplicationInstance.CompleteRequest();
		}


		private void RewriteCMSPath(HttpContext context, string sTmplateFile, string sQuery) {

			try {
				if (string.IsNullOrEmpty(sVirtualReqFile)) {
					sVirtualReqFile = SiteData.DefaultDirectoryFilename;
				}
				if (string.IsNullOrEmpty(sTmplateFile)) {
					sTmplateFile = SiteData.DefaultTemplateFilename;
				}

				context.RewritePath(sVirtualReqFile, string.Empty, sQuery);

				//cannot work in med trust
				//Page hand = (Page)PageParser.GetCompiledPageInstance(sFileRequested, context.Server.MapPath(sRealFile), context);

				Page hand = (Page)BuildManager.CreateInstanceFromVirtualPath(sTmplateFile, typeof(Page));
				hand.PreRenderComplete += new EventHandler(hand_PreRenderComplete);
				hand.ProcessRequest(context);

			} catch (Exception ex) {
				//assumption is database is probably empty / needs updating, so trigger the under construction view
				if (DatabaseUpdate.SystemNeedsChecking(ex) || DatabaseUpdate.AreCMSTablesIncomplete()) {
					SiteData.ManuallyWriteDefaultFile(context, ex);
				} else {
					//something bad has gone down, toss back the error
					throw;
				}
			}

		}


		void hand_PreRenderComplete(object sender, EventArgs e) {
			if (!bAlreadyDone) {
				try {
					if (HttpContext.Current.Items[REQ_PATH] != null && HttpContext.Current.Items[REQ_QUERY] != null) {
						HttpContext.Current.RewritePath(sVirtualReqFile,
								HttpContext.Current.Items[REQ_PATH].ToString(),
								HttpContext.Current.Items[REQ_QUERY].ToString());
					}
				} catch (Exception ex) { }
				bAlreadyDone = true;
			}
		}


		public void Dispose() {

			if (navHelper != null) {
				navHelper.Dispose();
			}

			navHelper = null;
		}

	}
}
