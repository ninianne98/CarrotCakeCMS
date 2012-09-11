using System;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
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


namespace Carrotware.CMS.UI.Base {

	public class VirtualFileSystem : IHttpHandler, IRequiresSessionState {

		private const string REQ_PATH = "RewriteOrigPath";
		private const string REQ_QUERY = "RewriteOrigQuery";

		private SiteNavHelper navHelper = new SiteNavHelper();

		public bool IsReusable {
			get {
				return false;
			}
		}

		private string sVirtualReqFile = String.Empty;
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
				bool bIgnorePublishState = SecurityData.AdvancedEditMode || SecurityData.IsAdmin || SecurityData.IsEditor;

				string queryString = String.Empty;
				queryString = context.Request.QueryString.ToString();
				if (string.IsNullOrEmpty(queryString)) {
					queryString = String.Empty;
				}

				if (!File.Exists(context.Server.MapPath(sFileRequested)) || sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename) {

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

					SiteNav navData = null;

					try {

						if (sFileRequested.Length < 3 || sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename) {
							if (bIgnorePublishState) {
								navData = navHelper.FindHome(SiteData.CurrentSiteID, null);
							} else {
								navData = navHelper.FindHome(SiteData.CurrentSiteID, true);
							}
							if (sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename && navData != null) {
								sFileRequested = navData.FileName;
							}
						}

						string pageName = sFileRequested;
						if (bIgnorePublishState) {
							navData = navHelper.GetLatestVersion(SiteData.CurrentSiteID, null, pageName);
						} else {
							navData = navHelper.GetLatestVersion(SiteData.CurrentSiteID, true, pageName);
						}

						if (sFileRequested.ToLower() == SiteData.DefaultDirectoryFilename && navData == null) {
							navData = SiteNavHelper.GetEmptyHome();
						}

					} catch (Exception ex) {
						//assumption is database is probably empty / needs updating, so trigger the under construction view
						if (DatabaseUpdate.SystemNeedsChecking(ex)) {
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

						if (!File.Exists(context.Server.MapPath(sSelectedTemplate))) {
							sSelectedTemplate = SiteData.DefaultTemplateFilename;
						}

						sVirtualReqFile = sFileRequested;

						RewriteCMSPath(context, sSelectedTemplate, queryString);

					} else {

						SiteData.PerformRedirectToErrorPage(404, sFileRequested);
						SiteData.Show404MessageFull(true);

					}

					navData.Dispose();

				} else {
					sVirtualReqFile = sFileRequested;

					RewriteCMSPath(context, sVirtualReqFile, queryString);
				}
			}

			context.ApplicationInstance.CompleteRequest();
		}


		private void RewriteCMSPath(HttpContext context, string sTmplateFile, string sQuery) {

			context.RewritePath(sVirtualReqFile, string.Empty, sQuery);

			//cannot work in med trust
			//Page hand = (Page)PageParser.GetCompiledPageInstance(sFileRequested, context.Server.MapPath(sRealFile), context);

			Page hand = (Page)BuildManager.CreateInstanceFromVirtualPath(sTmplateFile, typeof(Page));
			hand.PreRenderComplete += new EventHandler(hand_PreRenderComplete);
			hand.ProcessRequest(context);
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


		public void Dispose() {

			if (navHelper != null) {
				navHelper.Dispose();
			}

			navHelper = null;
		}

	}
}
