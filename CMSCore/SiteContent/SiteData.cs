using Carrotware.CMS.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;

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

	public partial class SiteData {

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
			this.Blog_EditorPath = string.IsNullOrEmpty(s.Blog_TagPath) ? "" : s.Blog_EditorPath;
			this.Blog_DatePath = string.IsNullOrEmpty(s.Blog_DatePath) ? "" : s.Blog_DatePath;
			this.Blog_DatePattern = string.IsNullOrEmpty(s.Blog_DatePattern) ? "yyyy/MM/dd" : s.Blog_DatePattern;

			if (string.IsNullOrEmpty(this.SiteTitlebarPattern)) {
				this.SiteTitlebarPattern = DefaultPageTitlePattern;
			}

			this.LoadTextWidgets();
		}

		public void LoadTextWidgets() {
			try {
				this.SiteTextWidgets = TextWidget.GetSiteTextWidgets(this.SiteID);
			} catch (Exception ex) {
				this.SiteTextWidgets = new List<TextWidget>();
			}
		}

		public string UpdateContent(string textContent) {
			if (!string.IsNullOrEmpty(textContent)) {
				foreach (TextWidget o in this.SiteTextWidgets.Where(x => x.ProcessBody && x.TextProcessor != null)) {
					textContent = o.TextProcessor.UpdateContent(textContent);
				}
			}
			return textContent;
		}

		public string UpdateContentPlainText(string textContent) {
			if (!string.IsNullOrEmpty(textContent)) {
				foreach (TextWidget o in this.SiteTextWidgets.Where(x => x.ProcessPlainText && x.TextProcessor != null)) {
					textContent = o.TextProcessor.UpdateContentPlainText(textContent);
				}
			}
			return textContent;
		}

		public string UpdateContentRichText(string textContent) {
			if (!string.IsNullOrEmpty(textContent)) {
				foreach (TextWidget o in this.SiteTextWidgets.Where(x => x.ProcessHTMLText && x.TextProcessor != null)) {
					textContent = o.TextProcessor.UpdateContentRichText(textContent);
				}
			}
			return textContent;
		}

		public string UpdateContentComment(string textContent) {
			if (!string.IsNullOrEmpty(textContent)) {
				foreach (TextWidget o in this.SiteTextWidgets.Where(x => x.ProcessComment && x.TextProcessor != null)) {
					textContent = o.TextProcessor.UpdateContentComment(textContent);
				}
			}
			return textContent;
		}

		public string UpdateContentSnippet(string textContent) {
			if (!string.IsNullOrEmpty(textContent)) {
				foreach (TextWidget o in this.SiteTextWidgets.Where(x => x.ProcessSnippet && x.TextProcessor != null)) {
					textContent = o.TextProcessor.UpdateContentSnippet(textContent);
				}
			}
			return textContent;
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

		public List<ContentSnippet> GetContentSnippetList() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				List<ContentSnippet> _types = (from d in CompiledQueries.cqGetSnippetsBySiteID(_db, this.SiteID)
											   select new ContentSnippet(d)).ToList();

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
				s.Blog_EditorPath = ContentPageHelper.ScrubSlug(this.Blog_EditorPath);
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
			this.MetaKeyword = string.IsNullOrEmpty(this.MetaKeyword) ? string.Empty : this.MetaKeyword;
			this.MetaDescription = string.IsNullOrEmpty(this.MetaDescription) ? string.Empty : this.MetaDescription;
		}

		public List<ExtendedUserData> GetMappedUsers() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				return (from l in _db.carrot_UserSiteMappings
						join u in _db.vw_carrot_UserDatas on l.UserId equals u.UserId
						where l.SiteID == this.SiteID
						select new ExtendedUserData(u)).ToList();
			}
		}

		public bool VerifyUserHasSiteAccess(Guid siteID, Guid userID) {
			//all admins have rights to all sites
			if (SecurityData.IsAdmin) {
				return true;
			}

			// if user is neither admin nor editor, they should not be in the backend of the site
			if (!(SecurityData.IsSiteEditor || SecurityData.IsAdmin)) {
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

				_db.carrot_SerialCaches.BatchDelete(lst);
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

		public List<BasicContentData> GetFullSiteFileList() {
			List<BasicContentData> map = new List<BasicContentData>();

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<vw_carrot_Content> queryAllFiles = CompiledQueries.cqGetAllContent(_db, this.SiteID);
				map = queryAllFiles.Select(x => new BasicContentData(x)).ToList();
			}

			return map;
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

		private List<TextWidget> _lstTextWidgets = null;

		internal List<TextWidget> SiteTextWidgets {
			get {
				if (_lstTextWidgets == null) {
					_lstTextWidgets = new List<TextWidget>();
				}

				return _lstTextWidgets;
			}

			set {
				_lstTextWidgets = value;
			}
		}

		public Guid? Blog_Root_ContentID { get; set; }
		public string Blog_FolderPath { get; set; }
		public string Blog_CategoryPath { get; set; }
		public string Blog_TagPath { get; set; }
		public string Blog_DatePattern { get; set; }
		public string Blog_EditorPath { get; set; }
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

		public string BlogEditorFolderPath {
			get { return RemoveDupeSlashes(BlogFolderPath + this.Blog_EditorPath + "/"); }
		}

		public string SiteSearchPath {
			get { return RemoveDupeSlashes(BlogFolderPath + SiteSearchPageName); }
		}

		public bool IsBlogCategoryPath {
			get { return SiteData.CurrentScriptName.ToLowerInvariant().StartsWith(this.BlogCategoryPath); }
		}

		public bool IsBlogTagPath {
			get { return SiteData.CurrentScriptName.ToLowerInvariant().StartsWith(this.BlogTagPath); }
		}

		public bool IsBlogDateFolderPath {
			get { return SiteData.CurrentScriptName.ToLowerInvariant().StartsWith(this.BlogDateFolderPath); }
		}

		public bool IsBlogEditorFolderPath {
			get { return SiteData.CurrentScriptName.ToLowerInvariant().StartsWith(this.BlogEditorFolderPath); }
		}

		public bool IsSiteSearchPath {
			get { return SiteData.CurrentScriptName.ToLowerInvariant().StartsWith(this.SiteSearchPath); }
		}

		public bool CheckIsBlogCategoryPath(string sFilterPath) {
			return sFilterPath.ToLowerInvariant().StartsWith(this.BlogCategoryPath);
		}

		public bool CheckIsBlogTagPath(string sFilterPath) {
			return sFilterPath.ToLowerInvariant().StartsWith(this.BlogTagPath);
		}

		public bool CheckIsBlogDateFolderPath(string sFilterPath) {
			return sFilterPath.ToLowerInvariant().StartsWith(this.BlogDateFolderPath);
		}

		public bool CheckIsBlogEditorFolderPath(string sFilterPath) {
			return sFilterPath.ToLowerInvariant().StartsWith(this.BlogEditorFolderPath);
		}

		public bool CheckIsSiteSearchPath(string sFilterPath) {
			return sFilterPath.ToLowerInvariant().StartsWith(this.SiteSearchPath);
		}

		public List<string> GetSpecialFilePathPrefixes() {
			List<string> lst = new List<string>();

			lst.Add(this.BlogCategoryPath.ToLowerInvariant());
			lst.Add(this.BlogTagPath.ToLowerInvariant());
			lst.Add(this.BlogDateFolderPath.ToLowerInvariant());
			lst.Add(this.BlogEditorFolderPath.ToLowerInvariant());
			lst.Add(this.SiteSearchPath.ToLowerInvariant());

			return lst;
		}

		public string MainCanonicalURL {
			get { return RemoveDupeSlashesURL(this.MainURL + "/"); }
		}

		public string DefaultCanonicalURL {
			get { return RemoveDupeSlashesURL(this.MainCanonicalURL + CurrentScriptName); }
		}

		public string ConstructedCanonicalURL(string sFileName) {
			return RemoveDupeSlashesURL(this.MainCanonicalURL + sFileName);
		}

		public string ConstructedCanonicalURL(ContentPage cp) {
			return RemoveDupeSlashesURL(this.MainCanonicalURL + cp.FileName);
		}

		public string ConstructedCanonicalURL(SiteNav nav) {
			return RemoveDupeSlashesURL(this.MainCanonicalURL + nav.FileName);
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
				return string.Empty;
			}
		}

		private string RemoveDupeSlashesURL(string sInput) {
			if (!string.IsNullOrEmpty(sInput)) {
				if (!sInput.ToLowerInvariant().StartsWith("http")) {
					sInput = "http://" + sInput;
				}
				return RemoveDupeSlashes(sInput.Replace("://", "¤¤¤")).Replace("¤¤¤", "://");
			} else {
				return string.Empty;
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

			using (ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper()) {
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
		private string _filename = string.Empty;
		private SiteData _site = new SiteData();

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
			_filename = SiteData.CurrentScriptName;
			_site = SiteData.CurrentSite;

			ParseString();
		}

		public BlogDatePathParser(SiteData site) {
			_filename = SiteData.CurrentScriptName;
			_site = site;

			ParseString();
		}

		public BlogDatePathParser(string folderPath) {
			_filename = folderPath;
			_site = SiteData.CurrentSite;

			ParseString();
		}

		public BlogDatePathParser(SiteData site, string folderPath) {
			_filename = folderPath;
			_site = site;

			ParseString();
		}

		private void ParseString() {
			_filename = _filename.Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
			string sFile = _filename.ToLowerInvariant().Replace(_site.BlogDateFolderPath, string.Empty);

			if (sFile.EndsWith(".aspx")) {
				sFile = sFile.Substring(0, sFile.ToLowerInvariant().LastIndexOf("/"));
			}

			string[] parms = sFile.Split('/');
			if (parms.Length > 2) {
				this.Day = int.Parse(parms[2]);
			}
			if (parms.Length > 1) {
				this.Month = int.Parse(parms[1]);
			}
			if (parms.Length > 0) {
				this.Year = int.Parse(parms[0]);
			}

			if (this.Month == null && this.Day == null) {
				_dateBegin = new DateTime(Convert.ToInt32(this.Year), 1, 1);
				_dateEnd = _dateBegin.AddYears(1).AddMilliseconds(-1);
			}
			if (this.Month != null && this.Day == null) {
				_dateBegin = new DateTime(Convert.ToInt32(this.Year), Convert.ToInt32(this.Month), 1);
				_dateEnd = _dateBegin.AddMonths(1).AddMilliseconds(-1);
			}
			if (this.Month != null && this.Day != null) {
				_dateBegin = new DateTime(Convert.ToInt32(this.Year), Convert.ToInt32(this.Month), Convert.ToInt32(this.Day));
				_dateEnd = _dateBegin.AddDays(1).AddMilliseconds(-1);
			}
		}
	}
}