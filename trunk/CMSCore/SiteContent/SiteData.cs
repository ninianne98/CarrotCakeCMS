using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Carrotware.CMS.Data;
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
	public class SiteData : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteData() {
#if DEBUG
			db.Log = new DebugTextWriter();
#endif
		}

		public SiteData(carrot_Site s) {

			this.SiteID = s.SiteID;
			this.MetaKeyword = s.MetaKeyword;
			this.MetaDescription = s.MetaDescription;
			this.SiteName = s.SiteName;
			this.SiteFolder = s.SiteFolder;
			this.MainURL = s.MainURL;
			this.BlockIndex = s.BlockIndex;
		}

		public SiteData Get(Guid siteID) {
			var s = (from r in db.carrot_Sites
					 where r.SiteID == siteID
					 select r).FirstOrDefault();

			if (s != null) {
				return new SiteData(s);
			} else {
				return null;
			}
		}


		private static string SiteKeyPrefix = "cms_SiteData_";
		public static SiteData CurrentSite {
			get {
				string ContentKey = SiteKeyPrefix + CurrentSiteID.ToString();
				SiteData currentSite = null;
				try { currentSite = (SiteData)HttpContext.Current.Cache[ContentKey]; } catch { }
				if (currentSite == null) {
					using (SiteData sd = new SiteData()) {
						currentSite = sd.Get(CurrentSiteID);
					}
					if (currentSite != null) {
						HttpContext.Current.Cache.Insert(ContentKey, currentSite, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
					} else {
						HttpContext.Current.Cache.Remove(ContentKey);
					}
				}
				return currentSite;
			}
			set {
				string ContentKey = SiteKeyPrefix + CurrentSiteID.ToString();
				if (value == null) {
					HttpContext.Current.Cache.Remove(ContentKey);
				} else {
					HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
			}
		}


		public SiteData GetCurrentSite() {
			//return Get(CurrentSiteID);
			return CurrentSite;
		}

		public void Save() {

			var s = (from r in db.carrot_Sites
					 where r.SiteID == this.SiteID
					 select r).FirstOrDefault();

			bool bNew = false;
			if (s == null) {
				s = new carrot_Site();
				if (this.SiteID == Guid.Empty) {
					this.SiteID = Guid.NewGuid();
				}
				bNew = true;
			}

			// if updating the current site then blank out its cache
			if (CurrentSiteID == this.SiteID) {
				CurrentSite = null;
			}

			s.SiteID = this.SiteID;

			FixMeta();
			s.MetaKeyword = this.MetaKeyword.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
			s.MetaDescription = this.MetaDescription.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");

			s.SiteName = this.SiteName;
			s.SiteFolder = this.SiteFolder;
			s.MainURL = this.MainURL;
			s.BlockIndex = this.BlockIndex;

			if (bNew) {
				db.carrot_Sites.InsertOnSubmit(s);
			}
			db.SubmitChanges();

			//if (SiteData.CurrentTrustLevel == AspNetHostingPermissionLevel.Unrestricted) {
			//    System.Web.HttpRuntime.UnloadAppDomain();
			//}
		}

		private void FixMeta() {
			this.MetaKeyword = string.IsNullOrEmpty(this.MetaKeyword) ? String.Empty : this.MetaKeyword;
			this.MetaDescription = string.IsNullOrEmpty(this.MetaDescription) ? String.Empty : this.MetaDescription;

		}

		public bool VerifyUserHasSiteAccess(Guid siteID, Guid userID) {

			//all admins have rights to all sites
			if (SecurityData.IsAdmin) {
				return true;
			}

			// if user is neither admin nor editor, they should not be in the backend of the site
			if (!(SecurityData.IsEditor || SecurityData.IsAdmin)) {
				return false;
			}

			// by this point, the user is probably an editor, make sure they have rights to this site
			var lstSites = (from l in db.carrot_UserSiteMappings
							where l.UserId == userID
								 && l.SiteID == siteID
							select l.SiteID).ToList();

			if (lstSites.Count > 0) {
				return true;
			}

			return false;
		}


		public void CleanUpSerialData() {

			var lst = (from c in db.carrot_SerialCaches
					   where c.EditDate < DateTime.Now.AddHours(-6)
					   && c.SiteID == CurrentSiteID
					   select c).ToList();

			if (lst.Count > 0) {
				foreach (var l in lst) {
					db.carrot_SerialCaches.DeleteOnSubmit(l);
				}
				db.SubmitChanges();
			}

		}


		public void MapUserToSite(Guid siteID, Guid userID) {

			carrot_UserSiteMapping map = new carrot_UserSiteMapping();
			map.UserSiteMappingID = Guid.NewGuid();
			map.SiteID = siteID;
			map.UserId = userID;

			db.carrot_UserSiteMappings.InsertOnSubmit(map);
			db.SubmitChanges();

		}

		public static Guid CurrentSiteID {
			get {
				Guid _site = Guid.Empty;

				if (ConfigurationManager.AppSettings["CarrotSiteID"] != null) {
					_site = new Guid(ConfigurationManager.AppSettings["CarrotSiteID"].ToString());
				}

				if (_site == Guid.Empty) {
					var s = CMSConfigHelper.DynSite;
					if (s != null) {
						_site = s.SiteID;
					}
				}

				return _site;
			}
		}

		private static string _siteQS = null;
		public static string OldSiteQuerystring {
			get {
				if (_siteQS == null) {
					_siteQS = String.Empty;
					if (ConfigurationManager.AppSettings["CarrotOldSiteQuerystring"] != null) {
						_siteQS = ConfigurationManager.AppSettings["CarrotOldSiteQuerystring"].ToString().ToLower();
					}
				}
				return _siteQS;
			}
		}


		public static AspNetHostingPermissionLevel CurrentTrustLevel {
			get {

				foreach (AspNetHostingPermissionLevel trustLevel in
					new AspNetHostingPermissionLevel[] {
						AspNetHostingPermissionLevel.Unrestricted,
						AspNetHostingPermissionLevel.High,
						AspNetHostingPermissionLevel.Medium,
						AspNetHostingPermissionLevel.Low,
						AspNetHostingPermissionLevel.Minimal 
					  }) {
					try {
						new AspNetHostingPermission(trustLevel).Demand();
					} catch (System.Security.SecurityException) {
						continue;
					}

					return trustLevel;
				}

				return AspNetHostingPermissionLevel.None;
			}
		}


		public bool BlockIndex { get; set; }
		public string MainURL { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }
		public Guid SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteFolder { get; set; }

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion


		private static string FormatToHTML(string inputString) {
			string outputString = String.Empty;
			if (!string.IsNullOrEmpty(inputString)) {
				outputString = inputString;
				outputString = outputString.Replace("\r\n", " <br \\> \r\n");
				outputString = outputString.Replace("   ", "&nbsp;&nbsp;&nbsp;");
				outputString = outputString.Replace("  ", "&nbsp;&nbsp;");
				outputString = outputString.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
			}
			return outputString;
		}

		public static string FormatErrorOutput(Exception objErr) {
			Assembly _assembly = Assembly.GetExecutingAssembly();

			string sBody = String.Empty;
			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.Core.SiteContent.ErrorFormat.htm"))) {
				sBody = oTextStream.ReadToEnd();
			}

			if (objErr is HttpException) {
				HttpException httpEx = (HttpException)objErr;

				sBody = sBody.Replace("{PAGE_TITLE}", httpEx.Message);
				sBody = sBody.Replace("{SHORT_NAME}", httpEx.Message);
				sBody = sBody.Replace("{LONG_NAME}", "HTTP " + httpEx.GetHttpCode() + " - " + FormatToHTML(httpEx.Message));

			} else {

				sBody = sBody.Replace("{PAGE_TITLE}", objErr.Message);
				sBody = sBody.Replace("{SHORT_NAME}", objErr.Message);
				sBody = sBody.Replace("{LONG_NAME}", FormatToHTML(" [" + objErr.GetType().ToString() + "] " + objErr.Message));

			}

			if (objErr.StackTrace != null) {
				sBody = sBody.Replace("{STACK_TRACE}", FormatToHTML(objErr.StackTrace));
			}

			if (objErr.InnerException != null) {
				sBody = sBody.Replace("{CONTENT_DETAIL}", FormatToHTML(objErr.InnerException.Message));
			}

			sBody = sBody.Replace("{TIME_STAMP}", DateTime.Now.ToString());

			sBody = sBody.Replace("{CONTENT_DETAIL}", "");
			sBody = sBody.Replace("{STACK_TRACE}", "");

			return sBody;
		}

		public static void Show404MessageFull(bool bResponseEnd) {
			HttpContext context = HttpContext.Current;
			context.Response.StatusCode = 404;
			context.Response.AppendHeader("Status", "HTTP/1.1 404 Object Not Found");
			context.Response.Cache.SetLastModified(DateTime.Today.Date);
			//context.Response.Write("<h2>404 Not Found</h2><p>HTTP 404. The resource you are looking for (or one of its dependencies) could have been removed, had its name changed, or is temporarily unavailable.  Please review the following URL and make sure that it is spelled correctly. </p>");

			Exception errInner = new Exception("The resource you are looking for (or one of its dependencies) could have been removed, had its name changed, or is temporarily unavailable. Please review the following URL and make sure that it is spelled correctly.");
			HttpException err = new HttpException(404, "File or directory not found.", errInner);

			context.Response.Write(FormatErrorOutput(err));

			if (bResponseEnd) {
				context.Response.End();
			}
		}

		public static void Show404MessageShort() {
			HttpContext context = HttpContext.Current;
			context.Response.StatusCode = 404;
			context.Response.StatusDescription = "Not Found";
		}

		public static void Show301Message(string sFileRequested) {
			HttpContext context = HttpContext.Current;
			context.Response.StatusCode = 301;
			context.Response.AppendHeader("Status", "301 Moved Permanently");
			context.Response.AppendHeader("Location", sFileRequested);
			context.Response.Cache.SetLastModified(DateTime.Today.Date);
			//context.Response.Write("<h2>301 Moved Permanently</h2>");

			HttpException ex = new HttpException(301, "301 Moved Permanently");
			context.Response.Write(FormatErrorOutput(ex));
		}


		public static void PerformRedirectToErrorPage(int ErrorKey, string sReqURL) {
			PerformRedirectToErrorPage(ErrorKey.ToString(), sReqURL);
		}

		public static void PerformRedirectToErrorPage(string sErrorKey, string sReqURL) {

			//parse web.config as XML because of medium trust issues
			HttpContext context = HttpContext.Current;

			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(context.Server.MapPath("~/Web.config"));

			XmlElement xmlCustomErrors = xDoc.SelectSingleNode("//system.web/customErrors") as XmlElement;

			if (xmlCustomErrors != null) {
				string redirectPage = "";

				if (xmlCustomErrors.Attributes["mode"] != null && xmlCustomErrors.Attributes["mode"].Value.ToLower() != "off") {
					if (xmlCustomErrors.Attributes["defaultRedirect"] != null) {
						redirectPage = xmlCustomErrors.Attributes["defaultRedirect"].Value;
					}

					if (xmlCustomErrors.HasChildNodes) {
						XmlNode xmlErrNode = xmlCustomErrors.SelectSingleNode("//system.web/customErrors/error[@statusCode='" + sErrorKey + "']");
						if (xmlErrNode != null) {
							redirectPage = xmlErrNode.Attributes["redirect"].Value;
						}
					}
					string sQS = "";
					if (context.Request.QueryString != null) {
						if (!string.IsNullOrEmpty(context.Request.QueryString.ToString())) {
							sQS = HttpUtility.UrlEncode("?" + context.Request.QueryString.ToString());
						}
					}

					if (!string.IsNullOrEmpty(redirectPage)) {
						context.Response.Redirect(redirectPage + "?aspxerrorpath=" + sReqURL + sQS);
					}
				}
			}

			/*
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			CustomErrorsSection section = (CustomErrorsSection)config.GetSection("system.web/customErrors");

			if (section != null) {
				if (section.Mode != CustomErrorsMode.Off) {
					CustomError configuredError = section.Errors[sErrorKey];
					if (configuredError != null) {
						if (!string.IsNullOrEmpty(configuredError.Redirect)) {
							context.Response.Redirect(configuredError.Redirect + "?aspxerrorpath=" + sReqURL);
						}
					} else {
						if (!string.IsNullOrEmpty(section.DefaultRedirect)) {
							context.Response.Redirect(section.DefaultRedirect + "?aspxerrorpath=" + sReqURL);
						}
					}
				}
			}
			*/
		}
		
		public static bool IsFilenameCurrentPage(string sCurrentFile) {
			
			if (string.IsNullOrEmpty(sCurrentFile)) {
				return false;
			}

			if (sCurrentFile.Contains("?")) {
				sCurrentFile = sCurrentFile.Substring(0, sCurrentFile.IndexOf("?"));
			}

			if (sCurrentFile.ToLower() == SiteData.CurrentScriptName.ToLower()) {
				return true;
			}
			return false;
		}

		public static string DefaultDirectoryFilename {
			get { return "/default.aspx".ToLower(); }
		}
		public static string DefaultTemplateFilename {
			get { return "/Manage/PlainTemplate.aspx".ToLower(); }
		}
		public static string VirtualCMSEditPrefix {
			get { return "/cms/carrotcake/edit/".ToLower(); }
		}
		public static string PreviewTemplateFilePage {
			get { return VirtualCMSEditPrefix + "templatepreview/Page.aspx"; }
		}

		public static bool IsPageSampler {
			get {
				string _prefix = (SiteData.VirtualCMSEditPrefix + "templatepreview/").ToLower();
				return SiteData.CurrentScriptName.ToLower().StartsWith(_prefix);
			}
		}

		public static string PreviewTemplateFile {
			get {
				string _preview = DefaultTemplateFilename;

				if (HttpContext.Current.Request.QueryString["carrot_templatepreview"] != null) {
					_preview = HttpContext.Current.Request.QueryString["carrot_templatepreview"].ToString();
					_preview = CMSConfigHelper.DecodeBase64(_preview);
				}

				return _preview;
			}
		}


		public static string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
		}

		public static string ReferringPage {
			get {
				var r = SiteData.CurrentScriptName;
				try { r = HttpContext.Current.Request.ServerVariables["http_referer"].ToString(); } catch { }
				if (string.IsNullOrEmpty(r))
					r = DefaultDirectoryFilename;
				return r;
			}
		}

	}


}
