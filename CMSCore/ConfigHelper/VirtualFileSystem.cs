using Carrotware.CMS.DBUpdater;
using System;
using System.Web;
using System.Web.Compilation;
using System.Web.Security;
using System.Web.SessionState;
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

namespace Carrotware.CMS.Core {

	public class VirtualFileSystem : IHttpHandler, IRequiresSessionState {
		private const string REQ_PATH = "RewriteOrigPath";
		private const string REQ_QUERY = "RewriteOrigQuery";

		public bool IsReusable {
			get {
				return false;
			}
		}

		private string _virtualReqFile = string.Empty;
		private string _requestedURL = string.Empty;
		private bool _alreadyDone = false;
		private bool _urlOverride = false;

		public void ProcessRequest(HttpContext context) {
			using (ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper()) {
				SiteNav navData = null;
				string fileRequested = context.Request.Path;
				_requestedURL = fileRequested;
				string scrubbedURL = fileRequested;

				_requestedURL = SiteData.AppendDefaultPath(_requestedURL);

				try {
					scrubbedURL = SiteData.AlternateCurrentScriptName;

					if (scrubbedURL.ToLowerInvariant() != _requestedURL.ToLowerInvariant()) {
						fileRequested = scrubbedURL;
						_urlOverride = true;
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

				fileRequested = SiteData.AppendDefaultPath(fileRequested);

				if (SecurityData.IsAuthenticated) {
					try {
						if (context.Request.UrlReferrer != null && !string.IsNullOrEmpty(context.Request.UrlReferrer.AbsolutePath)) {
							if (context.Request.UrlReferrer.AbsolutePath.ToLowerInvariant().Contains(FormsAuthentication.LoginUrl.ToLowerInvariant())
								|| FormsAuthentication.LoginUrl.ToLowerInvariant() == fileRequested.ToLowerInvariant()) {
								if (SiteFilename.DashboardURL.ToLowerInvariant() != fileRequested.ToLowerInvariant()
								&& SiteFilename.SiteInfoURL.ToLowerInvariant() != fileRequested.ToLowerInvariant()) {
									fileRequested = SiteData.AdminDefaultFile;
								}
							}
						}
					} catch (Exception ex) { }
				}

				if (fileRequested.ToLowerInvariant().EndsWith(".aspx") || SiteData.IsLikelyHomePage(fileRequested)) {
					bool bIgnorePublishState = SecurityData.AdvancedEditMode || SecurityData.IsAdmin || SecurityData.IsSiteEditor;

					string queryString = string.Empty;
					queryString = context.Request.QueryString.ToString();
					if (string.IsNullOrEmpty(queryString)) {
						queryString = string.Empty;
					}

					if (!CMSConfigHelper.CheckRequestedFileExistence(fileRequested, SiteData.CurrentSiteID) || SiteData.IsLikelyHomePage(fileRequested)) {
						context.Items[REQ_PATH] = context.Request.PathInfo;
						context.Items[REQ_QUERY] = context.Request.QueryString.ToString();

						// handle a case where this site was migrated from a format where all pages varied on a consistent querystring
						// allow this QS parm to be set in a config file.
						if (SiteData.IsLikelyHomePage(fileRequested)) {
							string sParm = string.Empty;
							if (SiteData.OldSiteQuerystring != string.Empty) {
								if (context.Request.QueryString[SiteData.OldSiteQuerystring] != null) {
									sParm = context.Request.QueryString[SiteData.OldSiteQuerystring].ToString();
								}
							}
							if (!string.IsNullOrEmpty(sParm)) {
								fileRequested = string.Format("/{0}.aspx", sParm.Trim());

								SiteData.Show301Message(fileRequested);

								context.Response.Redirect(fileRequested);
								context.Items[REQ_PATH] = fileRequested;
								context.Items[REQ_QUERY] = string.Empty;
							}
						}

						try {
							//periodic test of database up-to-dated-ness
							if (DatabaseUpdate.TablesIncomplete) {
								navData = SiteNavHelper.GetEmptyHome();
							} else {
								bool isHomePage = false;

								if (SiteData.IsLikelyHomePage(fileRequested)) {
									navData = navHelper.FindHome(SiteData.CurrentSiteID, !bIgnorePublishState);

									if (SiteData.IsLikelyHomePage(fileRequested) && navData != null) {
										fileRequested = navData.FileName;
										isHomePage = true;
									}
								}

								if (!isHomePage) {
									string pageName = fileRequested;
									navData = navHelper.GetLatestVersion(SiteData.CurrentSiteID, !bIgnorePublishState, pageName);
								}

								if (SiteData.IsLikelyHomePage(fileRequested) && navData == null) {
									navData = SiteNavHelper.GetEmptyHome();
								}
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

							// selectively engage the cms helper only if in advance mode
							if (SecurityData.AdvancedEditMode) {
								using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
									if (cmsHelper.cmsAdminContent != null) {
										try { sSelectedTemplate = cmsHelper.cmsAdminContent.TemplateFile.ToLowerInvariant(); } catch { }
									}
								}
							}

							if (!CMSConfigHelper.CheckFileExistence(sSelectedTemplate)) {
								sSelectedTemplate = SiteData.DefaultTemplateFilename;
							}

							_virtualReqFile = fileRequested;

							if (_urlOverride) {
								_virtualReqFile = _requestedURL;
								fileRequested = _requestedURL;
							}

							RewriteCMSPath(context, sSelectedTemplate, queryString);
						} else {
							SiteData.PerformRedirectToErrorPage(404, fileRequested);
							//SiteData.Show404MessageFull(true);
							SiteData.Show404MessageShort();
						}
					} else {
						_virtualReqFile = fileRequested;

						RewriteCMSPath(context, _virtualReqFile, queryString);
					}
				}

				context.ApplicationInstance.CompleteRequest();
			}
		}

		private void RewriteCMSPath(HttpContext context, string templateFile, string query) {
			try {
				if (string.IsNullOrEmpty(_virtualReqFile)) {
					_virtualReqFile = SiteData.DefaultDirectoryFilename;
				}
				if (string.IsNullOrEmpty(templateFile)) {
					templateFile = SiteData.DefaultTemplateFilename;
				}

				context.RewritePath(_virtualReqFile, string.Empty, query);

				//cannot work in med trust
				//Page hand = (Page)PageParser.GetCompiledPageInstance(sFileRequested, context.Server.MapPath(sRealFile), context);

				Page hand = (Page)BuildManager.CreateInstanceFromVirtualPath(templateFile, typeof(Page));
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

		protected void hand_PreRenderComplete(object sender, EventArgs e) {
			if (!_alreadyDone) {
				try {
					if (HttpContext.Current.Items[REQ_PATH] != null && HttpContext.Current.Items[REQ_QUERY] != null) {
						HttpContext.Current.RewritePath(_virtualReqFile,
								HttpContext.Current.Items[REQ_PATH].ToString(),
								HttpContext.Current.Items[REQ_QUERY].ToString());
					}
				} catch (Exception ex) { }
				_alreadyDone = true;
			}
		}
	}
}