using System;
using System.Collections.Generic;
using System.Configuration;
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
	public class SiteData : IDisposable {

		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public SiteData() {
			//#if DEBUG
			//            db.Log = new DebugTextWriter();
			//#endif
		}

		public SiteData(carrot_Site s) {

			this.SiteID = s.SiteID;
			this.MetaKeyword = s.MetaKeyword;
			this.MetaDescription = s.MetaDescription;
			this.SiteName = s.SiteName;
			this.SiteTagline = s.SiteTagline;
			this.SiteTitlebarPattern = s.SiteTitlebarPattern;
			this.MainURL = s.MainURL;
			this.BlockIndex = s.BlockIndex;

			this.Blog_Root_ContentID = s.Blog_Root_ContentID;

			this.Blog_FolderPath = string.IsNullOrEmpty(s.Blog_FolderPath) ? "" : s.Blog_FolderPath;
			this.Blog_CategoryPath = string.IsNullOrEmpty(s.Blog_CategoryPath) ? "" : s.Blog_CategoryPath;
			this.Blog_TagPath = string.IsNullOrEmpty(s.Blog_TagPath) ? "" : s.Blog_TagPath;
			this.Blog_DatePattern = string.IsNullOrEmpty(s.Blog_DatePattern) ? "yyyy/MM/dd" : s.Blog_DatePattern;

			if (string.IsNullOrEmpty(this.SiteTitlebarPattern)) {
				this.SiteTitlebarPattern = PageTitlePattern;
			}
		}

		public static string PageTitlePattern {
			get {
				string pattern = "[[CARROT_SITENAME]] - [[CARROT_PAGE_TITLEBAR]]";
				if (ConfigurationManager.AppSettings["CarrotPageTitlePattern"] != null) {
					try { pattern = ConfigurationManager.AppSettings["CarrotPageTitlePattern"].ToString(); } catch { }
					if (pattern.Contains("{0}") || pattern.Contains("{1}")) {
						pattern = "[[CARROT_SITENAME]] - [[CARROT_PAGE_TITLEBAR]]";
					}
				}
				return pattern;
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


		public static SiteData GetSiteByID(Guid siteID) {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_Site s = CompiledQueries.cqGetSiteByID(_db, siteID);

				if (s != null) {
					return new SiteData(s);
				} else {
					return null;
				}
			}
		}



		public List<ContentCategory> GetCategoryList() {

			List<ContentCategory> _types = (from d in CompiledQueries.cqGetContentCategoryBySiteID(db, this.SiteID)
											select ContentCategory.CreateCategory(d)).ToList();

			return _types;
		}

		public List<ContentTag> GetTagList() {

			List<ContentTag> _types = (from d in CompiledQueries.cqGetContentTagBySiteID(db, this.SiteID)
									   select ContentTag.CreateTag(d)).ToList();

			return _types;
		}


		private static string SiteKeyPrefix = "cms_SiteData_";
		public static SiteData CurrentSite {
			get {
				string ContentKey = SiteKeyPrefix + CurrentSiteID.ToString();
				SiteData currentSite = null;
				try { currentSite = (SiteData)HttpContext.Current.Cache[ContentKey]; } catch { }
				if (currentSite == null) {
					currentSite = SiteData.GetSiteByID(CurrentSiteID);

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

			carrot_Site s = CompiledQueries.cqGetSiteByID(db, this.SiteID);

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
			s.SiteTagline = this.SiteTagline;
			s.SiteTitlebarPattern = this.SiteTitlebarPattern;
			s.MainURL = this.MainURL;
			s.BlockIndex = this.BlockIndex;

			s.Blog_FolderPath = ContentPageHelper.ScrubSlug(this.Blog_FolderPath);
			s.Blog_CategoryPath = ContentPageHelper.ScrubSlug(this.Blog_CategoryPath);
			s.Blog_TagPath = ContentPageHelper.ScrubSlug(this.Blog_TagPath);

			s.Blog_Root_ContentID = this.Blog_Root_ContentID;
			s.Blog_DatePattern = string.IsNullOrEmpty(this.Blog_DatePattern) ? "yyyy/MM/dd" : this.Blog_DatePattern;

			if (bNew) {
				db.carrot_Sites.InsertOnSubmit(s);
			}
			db.SubmitChanges();

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
			IQueryable<Guid> lstSiteIDs = (from l in db.carrot_UserSiteMappings
										   where l.UserId == userID
												&& l.SiteID == siteID
										   select l.SiteID);

			if (lstSiteIDs.Count() > 0) {
				return true;
			}

			return false;
		}


		public void CleanUpSerialData() {

			List<carrot_SerialCache> lst = (from c in db.carrot_SerialCaches
											where c.EditDate < DateTime.Now.AddHours(-6)
											&& c.SiteID == CurrentSiteID
											select c).ToList();

			if (lst.Count > 0) {
				db.carrot_SerialCaches.DeleteAllOnSubmit(lst);
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
					DynamicSite s = CMSConfigHelper.DynSite;
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
		public string SiteTagline { get; set; }
		public string SiteTitlebarPattern { get; set; }

		public Guid? Blog_Root_ContentID { get; set; }
		public string Blog_FolderPath { get; set; }
		public string Blog_CategoryPath { get; set; }
		public string Blog_TagPath { get; set; }
		public string Blog_DatePattern { get; set; }

		public string BlogFolderPath {
			get { return RemoveDupeSlashes("/" + this.Blog_FolderPath + "/"); }
		}
		public string BlogCategoryPath {
			get { return RemoveDupeSlashes("/" + this.BlogFolderPath + "/" + this.Blog_CategoryPath + "/"); }
		}
		public string BlogTagPath {
			get { return RemoveDupeSlashes("/" + this.BlogFolderPath + "/" + this.Blog_TagPath + "/"); }
		}
		public string BlogDateFolderPath {
			get { return RemoveDupeSlashes("/" + this.BlogFolderPath + "/date/"); }
		}

		public string DefaultCanonicalURL {
			get { return RemoveDupeSlashesURL(this.MainURL + "/" + CurrentScriptName); }
		}
		public string ConstructedCanonicalURL(ContentPage cp) {
			return RemoveDupeSlashesURL(this.MainURL + "/" + cp.FileName);
		}
		public string ConstructedCanonicalURL(SiteNav nav) {
			return RemoveDupeSlashesURL(this.MainURL + "/" + nav.FileName);
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

			if (sCurrentFile.ToLower() == SiteData.CurrentScriptName.ToLower()
				|| sCurrentFile.ToLower() == SiteData.AlternateCurrentScriptName.ToLower()) {
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
			get { return ("/carrotcake/edit-" + CurrentSiteID.ToString() + "/").ToLower(); }
		}
		public static string PreviewTemplateFilePage {
			get { return VirtualCMSEditPrefix + "templatepreview/Page.aspx"; }
		}

		public static string SiteSearchPageName {
			get { return "/search.aspx".ToLower(); }
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

		public static string CurrentDLLVersion {
			get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		public static string CarrotCakeCMSVersion {
			get { return string.Format("CarrotCake CMS {0}", CurrentDLLVersion); }
		}

		public static string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
		}

		public static string AlternateCurrentScriptName {
			get {
				string sCurrentPage = CurrentScriptName;
				if (!CurrentScriptName.ToLower().StartsWith("/manage/")) {
					string sScrubbedURL = CheckForSpecialURL(SiteData.CurrentSite);

					if (sScrubbedURL.ToLower() != sCurrentPage.ToLower()) {
						sCurrentPage = sScrubbedURL;
					}
				}

				return sCurrentPage;
			}
		}


		public static string CheckForSpecialURL(SiteData site) {
			string sRequestedURL = HttpContext.Current.Request.Path;
			string sFileRequested = sRequestedURL;

			if (site != null) {
				if (sFileRequested.ToLower().StartsWith(site.BlogFolderPath.ToLower())) {
					if (sFileRequested.ToLower().StartsWith(site.BlogCategoryPath.ToLower())
						|| sFileRequested.ToLower().StartsWith(site.BlogTagPath.ToLower())
						|| sFileRequested.ToLower().StartsWith(site.BlogDateFolderPath.ToLower())) {
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
					List<SiteNav> lst1 = navHelper.GetLatest(this.SiteID, 10, true);
					lst = lst.Union(lst1).ToList();
				}
				if (feedData == RSSFeedInclude.BlogOnly || feedData == RSSFeedInclude.BlogAndPages) {
					List<SiteNav> lst1 = navHelper.GetLatestPosts(this.SiteID, 15, true);
					lst = lst.Union(lst1).ToList();
				}
			}

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

		public DateTime dateBegin = DateTime.MinValue;
		public DateTime dateEnd = DateTime.MaxValue;

		public int? Month { get; set; }
		public int? Day { get; set; }
		public int? Year { get; set; }


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

			if (sFile.IndexOf(SiteData.SiteSearchPageName) > 0) {
				sFile = sFile.ToLower().Replace(SiteData.SiteSearchPageName, "");
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
				dateBegin = new DateTime(Convert.ToInt32(this.Year), 1, 1);
				dateEnd = dateBegin.AddYears(1).AddMilliseconds(-1);
			}
			if (Month != null && Day == null) {
				dateBegin = new DateTime(Convert.ToInt32(this.Year), Convert.ToInt32(this.Month), 1);
				dateEnd = dateBegin.AddMonths(1).AddMilliseconds(-1);
			}
			if (Month != null && Day != null) {
				dateBegin = new DateTime(Convert.ToInt32(this.Year), Convert.ToInt32(this.Month), Convert.ToInt32(this.Day));
				dateEnd = dateBegin.AddDays(1).AddMilliseconds(-1);
			}

		}



	}
}
