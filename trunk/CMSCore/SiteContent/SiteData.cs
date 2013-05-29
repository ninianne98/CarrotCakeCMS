using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
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
	public class SiteData {

		public SiteData() { }

		public SiteData(carrot_Site s) {

			if (s != null && string.IsNullOrEmpty(s.TimeZone)) {
				s.TimeZone = SiteTimeZoneInfo.Id;
			}

			this.TimeZoneIdentifier = s.TimeZone;
			this.SiteID = s.SiteID;
			this.MetaKeyword = s.MetaKeyword;
			this.MetaDescription = s.MetaDescription;
			this.SiteName = s.SiteName;
			this.SiteTagline = s.SiteTagline;
			this.SiteTitlebarPattern = s.SiteTitlebarPattern;
			this.MainURL = s.MainURL;
			this.BlockIndex = s.BlockIndex;
			this.SendTrackbacks = s.SendTrackbacks;
			this.AcceptTrackbacks = s.AcceptTrackbacks;

			this.Blog_Root_ContentID = s.Blog_Root_ContentID;

			this.Blog_FolderPath = string.IsNullOrEmpty(s.Blog_FolderPath) ? "" : s.Blog_FolderPath;
			this.Blog_CategoryPath = string.IsNullOrEmpty(s.Blog_CategoryPath) ? "" : s.Blog_CategoryPath;
			this.Blog_TagPath = string.IsNullOrEmpty(s.Blog_TagPath) ? "" : s.Blog_TagPath;
			this.Blog_DatePath = string.IsNullOrEmpty(s.Blog_DatePath) ? "" : s.Blog_DatePath;
			this.Blog_DatePattern = string.IsNullOrEmpty(s.Blog_DatePattern) ? "yyyy/MM/dd" : s.Blog_DatePattern;

			if (string.IsNullOrEmpty(this.SiteTitlebarPattern)) {
				this.SiteTitlebarPattern = DefaultPageTitlePattern;
			}
		}

		public static string DefaultPageTitlePattern {
			get {
				return "[[CARROT_SITENAME]] - [[CARROT_PAGE_TITLEBAR]]";
			}
		}


		public static string CurrentTitlePattern {
			get {
				string pattern = "{0} - {1}";
				SiteData s = CurrentSite;
				if (!string.IsNullOrEmpty(s.SiteTitlebarPattern)) {
					pattern = s.SiteTitlebarPattern;
					pattern = pattern.Replace("[[CARROT_SITENAME]]", "{0}");
					pattern = pattern.Replace("[[CARROT_PAGE_TITLEBAR]]", "{1}");
					pattern = pattern.Replace("[[CARROT_PAGE_PAGEHEAD]]", "{2}");
					pattern = pattern.Replace("[[CARROT_PAGE_NAVMENUTEXT]]", "{3}");
				}

				return pattern;
			}
		}

		public static List<SiteData> GetSiteList() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {

				return (from l in _db.carrot_Sites orderby l.SiteName select new SiteData(l)).ToList();
			}
		}

		public static SiteData GetSiteByID(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_Site s = CompiledQueries.cqGetSiteByID(_db, siteID);

				if (s != null) {
#if DEBUG
					Debug.WriteLine(" ================ " + DateTime.UtcNow.ToString() + " ================");
					Debug.WriteLine("Grabbed site : GetSiteByID(Guid siteID) " + siteID.ToString());
#endif
					return new SiteData(s);
				} else {
					return null;
				}
			}
		}


		public List<ContentCategory> GetCategoryList() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				List<ContentCategory> _types = (from d in CompiledQueries.cqGetContentCategoryBySiteID(_db, this.SiteID)
												select new ContentCategory(d)).ToList();

				return _types;
			}
		}

		public List<ContentTag> GetTagList() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				List<ContentTag> _types = (from d in CompiledQueries.cqGetContentTagBySiteID(_db, this.SiteID)
										   select new ContentTag(d)).ToList();

				return _types;
			}
		}

		public void SendTrackbackQueue() {
			if (this.SendTrackbacks) {

				List<TrackBackEntry> lstTBQ = TrackBackEntry.GetTrackBackSiteQueue(this.SiteID);

				foreach (TrackBackEntry t in lstTBQ) {
					if (t.CreateDate > this.Now.AddMinutes(-30)) {
						try {
							TrackBacker tb = new TrackBacker();
							t.TrackBackResponse = tb.SendTrackback(t.Root_ContentID, this.SiteID, t.TrackBackURL);
							t.TrackedBack = true;
							t.Save();
						} catch (Exception ex) { }

					}
				}
			}
		}

		public static bool IsWebView {
			get { return (HttpContext.Current != null); }
		}

		private static string SiteKeyPrefix = "cms_SiteData_";

		public static void RemoveSiteFromCache(Guid siteID) {
			string ContentKey = SiteKeyPrefix + siteID.ToString();
			try {
				HttpContext.Current.Cache.Remove(ContentKey);
			} catch { }
		}

		public static SiteData GetSiteFromCache(Guid siteID) {

			string ContentKey = SiteKeyPrefix + siteID.ToString();
			SiteData currentSite = null;
			if (IsWebView) {
				try { currentSite = (SiteData)HttpContext.Current.Cache[ContentKey]; } catch { }
				if (currentSite == null) {
					currentSite = GetSiteByID(siteID);
					if (currentSite != null) {
						HttpContext.Current.Cache.Insert(ContentKey, currentSite, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
					} else {
						HttpContext.Current.Cache.Remove(ContentKey);
					}
				}
			} else {
				currentSite = new SiteData();
				currentSite.SiteID = Guid.Empty;
				currentSite.SiteName = "MOCK SITE";
				currentSite.SiteTagline = "MOCK SITE TAGLINE";
				currentSite.MainURL = "http://localhost";
				currentSite.Blog_CategoryPath = "cat";
				currentSite.Blog_TagPath = "tag";
				currentSite.Blog_FolderPath = "archive";
				currentSite.Blog_DatePath = "date";
				currentSite.TimeZoneIdentifier = "UTC";
			}
			return currentSite;

		}

		public static SiteData CurrentSite {
			get {
				return GetSiteFromCache(CurrentSiteID);
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

		public static bool CurretSiteExists {
			get {
				return CurrentSite != null ? true : false;
			}
		}


		public SiteData GetCurrentSite() {
			//return Get(CurrentSiteID);
			return CurrentSite;
		}

		public void Save() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_Site s = CompiledQueries.cqGetSiteByID(_db, this.SiteID);

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

				s.TimeZone = this.TimeZoneIdentifier;

				FixMeta();
				s.MetaKeyword = this.MetaKeyword.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
				s.MetaDescription = this.MetaDescription.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");

				s.SiteName = this.SiteName;
				s.SiteTagline = this.SiteTagline;
				s.SiteTitlebarPattern = this.SiteTitlebarPattern;
				s.MainURL = this.MainURL;
				s.BlockIndex = this.BlockIndex;
				s.SendTrackbacks = this.SendTrackbacks;
				s.AcceptTrackbacks = this.AcceptTrackbacks;

				s.Blog_FolderPath = ContentPageHelper.ScrubSlug(this.Blog_FolderPath);
				s.Blog_CategoryPath = ContentPageHelper.ScrubSlug(this.Blog_CategoryPath);
				s.Blog_TagPath = ContentPageHelper.ScrubSlug(this.Blog_TagPath);
				s.Blog_DatePath = ContentPageHelper.ScrubSlug(this.Blog_DatePath);

				s.Blog_Root_ContentID = this.Blog_Root_ContentID;
				s.Blog_DatePattern = string.IsNullOrEmpty(this.Blog_DatePattern) ? "yyyy/MM/dd" : this.Blog_DatePattern;

				if (bNew) {
					_db.carrot_Sites.InsertOnSubmit(s);
				}
				_db.SubmitChanges();
			}
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
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				// by this point, the user is probably an editor, make sure they have rights to this site
				IQueryable<Guid> lstSiteIDs = (from l in _db.carrot_UserSiteMappings
											   where l.UserId == userID
													&& l.SiteID == siteID
											   select l.SiteID);

				if (lstSiteIDs.Count() > 0) {
					return true;
				}
			}

			return false;
		}


		public void CleanUpSerialData() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<carrot_SerialCache> lst = (from c in _db.carrot_SerialCaches
													  where c.EditDate < DateTime.UtcNow.AddHours(-6)
													  && c.SiteID == CurrentSiteID
													  select c);


				_db.carrot_SerialCaches.DeleteBatch(lst);
				_db.SubmitChanges();
			}

		}

		public int GetSitePageCount(ContentPageType.PageType entryType) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				int iCount = CannedQueries.GetAllByTypeList(_db, this.SiteID, false, entryType).Count();
				return iCount;
			}
		}


		public void MapUserToSite(Guid siteID, Guid userID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {

				carrot_UserSiteMapping map = new carrot_UserSiteMapping();
				map.UserSiteMappingID = Guid.NewGuid();
				map.SiteID = siteID;
				map.UserId = userID;

				_db.carrot_UserSiteMappings.InsertOnSubmit(map);
				_db.SubmitChanges();

			}
		}


		public static ContentPage GetCurrentPage() {

			ContentPage pageContents = null;

			if (IsWebView) {
				using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
					if (SecurityData.AdvancedEditMode) {
						if (cmsHelper.cmsAdminContent == null) {
							pageContents = GetCurrentLivePage();
							pageContents.LoadAttributes();
							cmsHelper.cmsAdminContent = pageContents;
						} else {
							pageContents = cmsHelper.cmsAdminContent;
						}
					} else {
						pageContents = GetCurrentLivePage();
						if (SecurityData.CurrentUserGuid != Guid.Empty) {
							cmsHelper.cmsAdminContent = null;
						}
					}
				}
			} else {
				pageContents = ContentPageHelper.GetSamplerView();
			}

			return pageContents;
		}

		public static List<Widget> GetCurrentPageWidgets(Guid guidContentID) {

			List<Widget> pageWidgets = new List<Widget>();

			if (IsWebView) {
				using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
					if (SecurityData.AdvancedEditMode) {
						if (cmsHelper.cmsAdminWidget == null) {
							pageWidgets = GetCurrentPageLiveWidgets(guidContentID);
							cmsHelper.cmsAdminWidget = (from w in pageWidgets
														orderby w.WidgetOrder
														select w).ToList();
						} else {
							pageWidgets = (from w in cmsHelper.cmsAdminWidget
										   orderby w.WidgetOrder
										   select w).ToList();
						}
					} else {
						pageWidgets = GetCurrentPageLiveWidgets(guidContentID);
						if (SecurityData.CurrentUserGuid != Guid.Empty) {
							cmsHelper.cmsAdminWidget = null;
						}
					}
				}
			}

			return pageWidgets;
		}

		public static ContentPage GetCurrentLivePage() {

			ContentPage pageContents = null;

			using (ContentPageHelper pageHelper = new ContentPageHelper()) {

				bool IsPageTemplate = false;
				string sCurrentPage = SiteData.CurrentScriptName;
				string sScrubbedURL = SiteData.AlternateCurrentScriptName;

				if (sScrubbedURL.ToLower() != sCurrentPage.ToLower()) {
					sCurrentPage = sScrubbedURL;
				}

				if (SecurityData.IsAdmin || SecurityData.IsEditor) {
					pageContents = pageHelper.FindByFilename(SiteData.CurrentSiteID, sCurrentPage);
				} else {
					pageContents = pageHelper.GetLatestContentByURL(SiteData.CurrentSiteID, true, sCurrentPage);
				}

				if (pageContents == null && SiteData.IsPageReal) {
					IsPageTemplate = true;
				}

				if ((SiteData.IsPageSampler || IsPageTemplate || !IsWebView) && pageContents == null) {
					pageContents = ContentPageHelper.GetSamplerView();
				}

				if (IsPageTemplate) {
					pageContents.TemplateFile = sCurrentPage;
				}
			}

			return pageContents;
		}

		public static List<Widget> GetCurrentPageLiveWidgets(Guid guidContentID) {
			List<Widget> pageWidgets = new List<Widget>();

			using (WidgetHelper widgetHelper = new WidgetHelper()) {
				pageWidgets = widgetHelper.GetWidgets(guidContentID, !SecurityData.AdvancedEditMode);
			}

			return pageWidgets;
		}



		public List<BasicContentData> GetFullSiteFileList() {
			List<BasicContentData> map = new List<BasicContentData>();

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<vw_carrot_Content> queryAllFiles = CompiledQueries.cqGetAllContent(_db, this.SiteID);
				map = queryAllFiles.Select(x => new BasicContentData(x)).ToList();
			}

			return map;
		}

		public static Guid CurrentSiteID {
			get {
				Guid _site = Guid.Empty;
				if (IsWebView) {
					CarrotCakeConfig config = CarrotCakeConfig.GetConfig();
					if (config.MainConfig != null
						&& config.MainConfig.SiteID != null) {
						_site = config.MainConfig.SiteID.Value;
					}

					if (_site == Guid.Empty) {
						try {
							DynamicSite s = CMSConfigHelper.DynSite;
							if (s != null) {
								_site = s.SiteID;
							}
						} catch { }
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
					CarrotCakeConfig config = CarrotCakeConfig.GetConfig();
					if (config.ExtraOptions != null
						&& !string.IsNullOrEmpty(config.ExtraOptions.OldSiteQuerystring)) {
						_siteQS = config.ExtraOptions.OldSiteQuerystring.ToLower();
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

		public DateTime Now {
			get {
				if (IsWebView) {
					return SiteData.CurrentSite.ConvertUTCToSiteTime(DateTime.UtcNow);
				} else {
					return DateTime.Now;
				}
			}
		}

		public TimeZoneInfo SiteTimeZoneInfo {
			get {
				TimeZoneInfo oTZ = TimeZoneInfo.Local;
				if (IsWebView) {
					if (!string.IsNullOrEmpty(this.TimeZoneIdentifier)) {
						try { oTZ = TimeZoneInfo.FindSystemTimeZoneById(this.TimeZoneIdentifier); } catch { }
					}
				}
				return oTZ;
			}
		}

		public DateTime ConvertUTCToSiteTime(DateTime dateUTC) {
			return TimeZoneInfo.ConvertTimeFromUtc(dateUTC, SiteTimeZoneInfo);
		}
		public DateTime ConvertSiteTimeToUTC(DateTime dateSite) {
			DateTime dateSiteSrc = DateTime.SpecifyKind(dateSite, DateTimeKind.Unspecified);
			return TimeZoneInfo.ConvertTimeToUtc(dateSiteSrc, SiteTimeZoneInfo);
		}
		public string ConvertSiteTimeToISO8601(DateTime dateSite) {
			return ConvertSiteTimeToUTC(dateSite).ToString("s") + "Z";
		}
		public DateTime ConvertSiteTimeToLocalServer(DateTime dateSite) {
			DateTime dateSiteSrc = DateTime.SpecifyKind(dateSite, DateTimeKind.Unspecified);
			DateTime utc = TimeZoneInfo.ConvertTimeToUtc(dateSiteSrc, SiteTimeZoneInfo);

			return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.Local);
		}
		public DateTime ConvertUTCToLocalServer(DateTime dateUTC) {

			return TimeZoneInfo.ConvertTimeFromUtc(dateUTC, TimeZoneInfo.Local);
		}


		public bool SendTrackbacks { get; set; }
		public bool AcceptTrackbacks { get; set; }
		public bool BlockIndex { get; set; }
		public string MainURL { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }
		public Guid SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteTagline { get; set; }
		public string SiteTitlebarPattern { get; set; }
		public string TimeZoneIdentifier { get; set; }

		public Guid? Blog_Root_ContentID { get; set; }
		public string Blog_FolderPath { get; set; }
		public string Blog_CategoryPath { get; set; }
		public string Blog_TagPath { get; set; }
		public string Blog_DatePattern { get; set; }
		public string Blog_DatePath { get; set; }

		public string BlogFolderPath {
			get { return RemoveDupeSlashes("/" + this.Blog_FolderPath + "/"); }
		}
		public string BlogCategoryPath {
			get { return RemoveDupeSlashes(BlogFolderPath + this.Blog_CategoryPath + "/"); }
		}
		public string BlogTagPath {
			get { return RemoveDupeSlashes(BlogFolderPath + this.Blog_TagPath + "/"); }
		}
		public string BlogDateFolderPath {
			get { return RemoveDupeSlashes(BlogFolderPath + this.Blog_DatePath + "/"); }
		}
		public string SiteSearchPath {
			get { return RemoveDupeSlashes(BlogFolderPath + SiteSearchPageName); }
		}

		protected static string SiteSearchPageName {
			get { return "/search.aspx".ToLower(); }
		}

		public string DefaultCanonicalURL {
			get { return RemoveDupeSlashesURL(this.MainURL + "/" + CurrentScriptName); }
		}
		public string ConstructedCanonicalURL(string sFileName) {
			return RemoveDupeSlashesURL(this.MainURL + "/" + sFileName);
		}
		public string ConstructedCanonicalURL(ContentPage cp) {
			return RemoveDupeSlashesURL(this.MainURL + "/" + cp.FileName);
		}
		public string ConstructedCanonicalURL(SiteNav nav) {
			return RemoveDupeSlashesURL(this.MainURL + "/" + nav.FileName);
		}

		public string BuildDateSearchLink(DateTime postDate) {
			return RemoveDupeSlashes(this.BlogDateFolderPath + postDate.ToString("/yyyy/MM/dd/") + SiteData.SiteSearchPageName);
		}
		public string BuildMonthSearchLink(DateTime postDate) {
			return RemoveDupeSlashes(this.BlogDateFolderPath + postDate.ToString("/yyyy/MM/") + SiteData.SiteSearchPageName);
		}

		private string RemoveDupeSlashes(string sInput) {
			if (!string.IsNullOrEmpty(sInput)) {
				return sInput.Replace("//", "/").Replace("//", "/");
			} else {
				return String.Empty;
			}
		}

		private string RemoveDupeSlashesURL(string sInput) {
			if (!string.IsNullOrEmpty(sInput)) {
				if (!sInput.ToLower().StartsWith("http")) {
					sInput = "http://" + sInput;
				}
				return RemoveDupeSlashes(sInput.Replace("://", "¤¤¤")).Replace("¤¤¤", "://");
			} else {
				return String.Empty;
			}
		}

		public static void ManuallyWriteDefaultFile(HttpContext context) {
			Assembly _assembly = Assembly.GetExecutingAssembly();

			string sBody = String.Empty;

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.Core.SiteContent.Default.htm"))) {
				sBody = oTextStream.ReadToEnd();
			}
			try {
				if (CurretSiteExists) {
					sBody = sBody.Replace("{TIME_STAMP}", CurrentSite.Now.ToString());
				}
			} catch {
				sBody = sBody.Replace("{TIME_STAMP}", DateTime.Now.ToString());
			}

			sBody = sBody.Replace("{SITE_ROOT_PATH}", SiteData.AdminFolderPath);

			context.Response.ContentType = "text/html";
			context.Response.Clear();
			context.Response.BufferOutput = true;

			context.Response.Write(sBody);
			context.Response.Flush();
			context.Response.End();
		}

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

			if (CurretSiteExists) {
				sBody = sBody.Replace("{TIME_STAMP}", CurrentSite.Now.ToString());
			} else {
				sBody = sBody.Replace("{TIME_STAMP}", DateTime.Now.ToString());
			}

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


		public static void WriteDebugException(string sSrc, Exception objErr) {
			bool bWriteError = false;

			CarrotCakeConfig config = CarrotCakeConfig.GetConfig();

			if (config.ExtraOptions != null && config.ExtraOptions.WriteErrorLog) {
				bWriteError = config.ExtraOptions.WriteErrorLog;
			}
#if DEBUG
			bWriteError = true; // always write erros when debug build
#endif

			if (bWriteError && objErr != null) {
				StringBuilder sb = new StringBuilder();

				sb.AppendLine("----------------  " + sSrc.ToUpper() + " - " + DateTime.Now.ToString() + "  ----------------");

				sb.AppendLine("[" + objErr.GetType().ToString() + "] " + objErr.Message);

				if (objErr.StackTrace != null) {
					sb.AppendLine(objErr.StackTrace);
				}

				if (objErr.InnerException != null) {
					sb.AppendLine(objErr.InnerException.Message);
				}

				string sDir = HttpContext.Current.Server.MapPath("~/carrot_errors.txt");

				Encoding encode = Encoding.Default;
				using (StreamWriter oWriter = new StreamWriter(sDir, true, encode)) {
					oWriter.Write(sb.ToString());
				}
			}
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

					if (!string.IsNullOrEmpty(redirectPage) && !sQS.ToLower().Contains("aspxerrorpath")) {
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

			if (sCurrentFile.ToLower() == SiteData.CurrentScriptName.ToLower()
				|| sCurrentFile.ToLower() == SiteData.AlternateCurrentScriptName.ToLower()) {
				return true;
			}
			return false;
		}

		public static string SearchQueryParameter {
			get { return "search".ToLower(); }
		}
		public static string DefaultDirectoryFilename {
			get { return "/default.aspx".ToLower(); }
		}
		public static string DefaultTemplateFilename {
			get { return SiteData.AdminFolderPath + "PlainTemplate.aspx".ToLower(); }
		}
		public static string VirtualCMSEditPrefix {
			get { return ("/carrotcake/edit-" + CurrentSiteID.ToString() + "/").ToLower(); }
		}
		public static string PreviewTemplateFilePage {
			get { return VirtualCMSEditPrefix + "templatepreview/Page.aspx"; }
		}

		public static bool IsPageSampler {
			get {
				string _prefix = (VirtualCMSEditPrefix + "templatepreview/").ToLower();
				return CurrentScriptName.ToLower().StartsWith(_prefix);
			}
		}

		public static bool IsPageReal {
			get {
				if (IsWebView
					&& CurrentScriptName.ToLower() != DefaultDirectoryFilename.ToLower()
					&& File.Exists(HttpContext.Current.Server.MapPath(CurrentScriptName))) {
					return true;
				} else {
					return false;
				}
			}
		}

		static private List<string> _specialFiles = null;

		public static List<string> SpecialFiles {
			get {
				if (_specialFiles == null) {
					_specialFiles = new List<string>();
					_specialFiles.Add(DefaultTemplateFilename);
					_specialFiles.Add(DefaultDirectoryFilename);
					//_specialFiles.Add("/feed/rss.ashx");
					//_specialFiles.Add("/feed/sitemap.ashx");
					//_specialFiles.Add("/feed/xmlrpc.ashx");
				}

				return _specialFiles;
			}
		}

		public static bool IsCurrentPageSpecial {
			get {
				return SiteData.SpecialFiles.Contains(CurrentScriptName.ToLower()) || CurrentScriptName.ToLower().StartsWith(AdminFolderPath);
			}
		}

		public static bool IsPageSpecial(string sPageName) {

			return SiteData.SpecialFiles.Contains(sPageName.ToLower()) || sPageName.ToLower().StartsWith(AdminFolderPath);
		}

		public static string PreviewTemplateFile {
			get {
				string _preview = DefaultTemplateFilename;

				if (IsWebView) {
					if (HttpContext.Current.Request.QueryString["carrot_templatepreview"] != null) {
						_preview = HttpContext.Current.Request.QueryString["carrot_templatepreview"].ToString();
						_preview = CMSConfigHelper.DecodeBase64(_preview);
					}
				}

				return _preview;
			}
		}

		private static Version CurrentVersion {
			get { return Assembly.GetExecutingAssembly().GetName().Version; }
		}

		public static string CurrentDLLVersion {
			get { return CurrentVersion.ToString(); }
		}

		public static string CurrentDLLMajorMinorVersion {
			get {
				Version v = CurrentVersion;
				return v.Major.ToString() + "." + v.Minor.ToString();
			}
		}

		public static string CarrotCakeCMSVersion {
			get {
#if DEBUG
				return string.Format("CarrotCake CMS {0} DEBUG MODE", CurrentDLLVersion);
#endif
				return string.Format("CarrotCake CMS {0}", CurrentDLLVersion);
			}
		}

		public static string CarrotCakeCMSVersionMM {
			get {
#if DEBUG
				return string.Format("CarrotCake CMS {0} (debug)", CurrentDLLMajorMinorVersion);
#endif
				return string.Format("CarrotCake CMS {0}", CurrentDLLMajorMinorVersion);
			}
		}

		public static string CurrentScriptName {
			get {
				string sPath = "/";
				try { sPath = HttpContext.Current.Request.ServerVariables["script_name"].ToString(); } catch { }
				return sPath;
			}
		}

		public static string AppendDefaultPath(string sRequestedURL) {
			if (!string.IsNullOrEmpty(sRequestedURL)) {
				sRequestedURL = sRequestedURL.Replace(@"\", @"/");
				if (sRequestedURL.EndsWith("/") || !sRequestedURL.ToLower().EndsWith(".aspx")) {
					sRequestedURL = (sRequestedURL + DefaultDirectoryFilename).Replace("//", "/");
				}
			}

			return sRequestedURL;
		}

		public static string AdminDefaultFile {
			get {
				return (AdminFolderPath + DefaultDirectoryFilename).Replace("//", "/");
			}
		}

		private static string _adminFolderPath = null;
		public static string AdminFolderPath {
			get {
				if (_adminFolderPath == null) {
					CarrotCakeConfig config = CarrotCakeConfig.GetConfig();
					if (config.MainConfig != null && !string.IsNullOrEmpty(config.MainConfig.AdminFolderPath)) {
						_adminFolderPath = config.MainConfig.AdminFolderPath;
						_adminFolderPath = ("/" + _adminFolderPath + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
					} else {
						_adminFolderPath = "/c3-admin/";
					}
					if (string.IsNullOrEmpty(_adminFolderPath) || _adminFolderPath.Length < 2) {
						_adminFolderPath = "/c3-admin/";
					}
				}
				return _adminFolderPath;
			}
		}

		public static string AlternateCurrentScriptName {
			get {
				string sCurrentPage = CurrentScriptName;

				if (IsWebView) {
					if (!CurrentScriptName.ToLower().StartsWith(AdminFolderPath)) {

						string sScrubbedURL = CheckForSpecialURL(CurrentSite);

						if (sScrubbedURL.ToLower() == sCurrentPage.ToLower()) {
							sCurrentPage = AppendDefaultPath(sCurrentPage);
						}

						if (!sScrubbedURL.ToLower().StartsWith(sCurrentPage.ToLower())
							&& !sCurrentPage.ToLower().EndsWith(DefaultDirectoryFilename)) {

							if (sScrubbedURL.ToLower() != sCurrentPage.ToLower()) {
								sCurrentPage = sScrubbedURL;
							}
						}
					}
				}

				return sCurrentPage;
			}
		}


		public static string CheckForSpecialURL(SiteData site) {
			string sRequestedURL = "/";

			if (IsWebView) {
				sRequestedURL = CurrentScriptName;
				string sFileRequested = sRequestedURL;

				if (!sRequestedURL.ToLower().StartsWith(AdminFolderPath) && site != null) {
					if (sFileRequested.ToLower().StartsWith(site.BlogFolderPath.ToLower())) {
						if (sFileRequested.ToLower().StartsWith(site.BlogCategoryPath.ToLower())
							|| sFileRequested.ToLower().StartsWith(site.BlogTagPath.ToLower())
							|| sFileRequested.ToLower().StartsWith(site.BlogDateFolderPath.ToLower())
							|| sFileRequested.ToLower().StartsWith(site.SiteSearchPath.ToLower())) {
							if (site.Blog_Root_ContentID.HasValue) {
								using (SiteNavHelper navHelper = new SiteNavHelper()) {
									SiteNav blogNavPage = navHelper.GetLatestVersion(site.SiteID, site.Blog_Root_ContentID.Value);
									if (blogNavPage != null) {
										sRequestedURL = blogNavPage.FileName;
									}
								}
							}
						}
					}
				}
			}

			return sRequestedURL;
		}

		public static string ReferringPage {
			get {
				string r = SiteData.CurrentScriptName;
				try { r = HttpContext.Current.Request.ServerVariables["http_referer"].ToString(); } catch { }
				if (string.IsNullOrEmpty(r))
					r = DefaultDirectoryFilename;
				return r;
			}
		}

		//==========BEGIN RSS=================

		public enum RSSFeedInclude {
			Unknown,
			BlogAndPages,
			BlogOnly,
			PageOnly
		}

		public void RenderRSSFeed(HttpContext context) {
			SiteData.RSSFeedInclude FeedType = SiteData.RSSFeedInclude.BlogAndPages;

			if (!string.IsNullOrEmpty(context.Request.QueryString["type"])) {
				string feedType = context.Request.QueryString["type"].ToString();

				FeedType = (SiteData.RSSFeedInclude)Enum.Parse(typeof(SiteData.RSSFeedInclude), feedType, true);
			}

			string sRSSXML = SiteData.CurrentSite.GetRSSFeed(FeedType);

			context.Response.ContentType = SiteData.RssDocType;

			context.Response.Write(sRSSXML);

			context.Response.StatusCode = 200;
			context.Response.StatusDescription = "OK";

		}


		public static string RssDocType { get { return "application/rss+xml"; } }

		public string GetRSSFeed(RSSFeedInclude feedData) {
			SyndicationFeed feed = CreateRecentItemFeed(feedData);

			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.Encoding = Encoding.UTF8;
			settings.CheckCharacters = true;

			using (XmlWriter xw = XmlWriter.Create(sb, settings)) {
				Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(feed);
				rssFormatter.WriteTo(xw);
			}

			string xml = sb.ToString();
			xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"", "<?xml version=\"1.0\" encoding=\"utf-8\"");

			return xml;
		}

		private SyndicationFeed CreateRecentItemFeed(RSSFeedInclude feedData) {
			List<SyndicationItem> syndicationItems = GetRecentPagesOrPosts(feedData);

			return new SyndicationFeed(syndicationItems) {
				Title = new TextSyndicationContent(this.SiteName),
				Description = new TextSyndicationContent(this.SiteTagline)
			};
		}

		private List<SyndicationItem> GetRecentPagesOrPosts(RSSFeedInclude feedData) {

			List<SyndicationItem> syndRSS = new List<SyndicationItem>();
			List<SiteNav> lst = new List<SiteNav>();

			ContentPageType PageType = new ContentPageType();

			using (SiteNavHelper navHelper = new SiteNavHelper()) {
				if (feedData == RSSFeedInclude.PageOnly || feedData == RSSFeedInclude.BlogAndPages) {
					List<SiteNav> lst1 = navHelper.GetLatest(this.SiteID, 8, true);
					lst = lst.Union(lst1).ToList();
					List<SiteNav> lst2 = navHelper.GetLatestUpdates(this.SiteID, 10, true);
					lst = lst.Union(lst2).ToList();
				}
				if (feedData == RSSFeedInclude.BlogOnly || feedData == RSSFeedInclude.BlogAndPages) {
					List<SiteNav> lst1 = navHelper.GetLatestPosts(this.SiteID, 8, true);
					lst = lst.Union(lst1).ToList();
					List<SiteNav> lst2 = navHelper.GetLatestPostUpdates(this.SiteID, 10, true);
					lst = lst.Union(lst2).ToList();
				}
			}

			lst.RemoveAll(x => x.ShowInSiteMap == false && x.ContentType == ContentPageType.PageType.ContentEntry);
			lst.RemoveAll(x => x.BlockIndex == true);

			foreach (SiteNav sn in lst) {
				SyndicationItem si = new SyndicationItem();

				string sPageURI = RemoveDupeSlashesURL(this.ConstructedCanonicalURL(sn));

				Uri PageURI = new Uri(sPageURI);

				si.Content = new TextSyndicationContent(sn.PageTextPlainSummaryMedium);
				si.Title = new TextSyndicationContent(sn.NavMenuText);
				si.Links.Add(SyndicationLink.CreateSelfLink(PageURI));
				si.AddPermalink(PageURI);

				si.LastUpdatedTime = sn.EditDate;
				si.PublishDate = sn.CreateDate;

				syndRSS.Add(si);
			}

			return syndRSS.OrderByDescending(p => p.PublishDate).ToList();
		}

		//==========END RSS=================

	}


	//============================================

	public class BlogDatePathParser {
		string _FileName = String.Empty;
		SiteData _site = new SiteData();

		private DateTime _dateBegin = DateTime.MinValue;
		private DateTime _dateEnd = DateTime.MaxValue;

		public int? Month { get; set; }
		public int? Day { get; set; }
		public int? Year { get; set; }

		public DateTime DateBegin { get { return _dateBegin; } }
		public DateTime DateEnd { get { return _dateEnd; } }

		public DateTime DateBeginUTC {
			get {
				if (_site != null) {
					return _site.ConvertSiteTimeToUTC(_dateBegin);
				} else {
					return _dateBegin;
				}
			}
		}
		public DateTime DateEndUTC {
			get {
				if (_site != null) {
					return _site.ConvertSiteTimeToUTC(_dateEnd);
				} else {
					return _dateEnd;
				}
			}
		}

		public BlogDatePathParser() {
			_FileName = SiteData.CurrentScriptName;
			_site = SiteData.CurrentSite;

			ParseString();
		}

		public BlogDatePathParser(SiteData site) {
			_FileName = SiteData.CurrentScriptName;
			_site = site;

			ParseString();
		}

		public BlogDatePathParser(string FolderPath) {
			_FileName = FolderPath;
			_site = SiteData.CurrentSite;

			ParseString();
		}

		public BlogDatePathParser(SiteData site, string FolderPath) {
			_FileName = FolderPath;
			_site = site;

			ParseString();
		}

		private void ParseString() {
			_FileName = _FileName.Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
			string sFile = _FileName.ToLower().Replace(_site.BlogDateFolderPath, "");

			if (sFile.ToLower().EndsWith(".aspx")) {
				sFile = sFile.ToLower().Substring(0, sFile.ToLower().LastIndexOf("/"));
			}

			string[] parms = sFile.Split('/');
			if (parms.Length > 2) {
				Day = int.Parse(parms[2]);
			}
			if (parms.Length > 1) {
				Month = int.Parse(parms[1]);
			}
			if (parms.Length > 0) {
				Year = int.Parse(parms[0]);
			}

			if (Month == null && Day == null) {
				_dateBegin = new DateTime(Convert.ToInt32(this.Year), 1, 1);
				_dateEnd = _dateBegin.AddYears(1).AddMilliseconds(-1);
			}
			if (Month != null && Day == null) {
				_dateBegin = new DateTime(Convert.ToInt32(this.Year), Convert.ToInt32(this.Month), 1);
				_dateEnd = _dateBegin.AddMonths(1).AddMilliseconds(-1);
			}
			if (Month != null && Day != null) {
				_dateBegin = new DateTime(Convert.ToInt32(this.Year), Convert.ToInt32(this.Month), Convert.ToInt32(this.Day));
				_dateEnd = _dateBegin.AddDays(1).AddMilliseconds(-1);
			}
		}


	}
}
