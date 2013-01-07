using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
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

	public class ContentPageHelper : IDisposable {


		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public ContentPageHelper() { }


		public void BulkUpdateTemplate(Guid siteID, List<Guid> lstUpd, string sTemplateFile) {

			IQueryable<carrot_Content> queryCont = (from ct in db.carrot_Contents
													join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
													where r.SiteID == siteID
														   && lstUpd.Contains(r.Root_ContentID)
														   && ct.IsLatestVersion == true
													select ct);


			db.carrot_Contents.UpdateBatch(queryCont, p => new carrot_Content { TemplateFile = sTemplateFile });

			db.SubmitChanges();

		}


		public void UpdateAllContentTemplates(Guid siteID, string sTemplateFile) {

			UpdateAllBlogTemplates(siteID, sTemplateFile);

			UpdateAllPageTemplates(siteID, sTemplateFile);

		}

		public void UpdateAllBlogTemplates(Guid siteID, string sTemplateFile) {
			IQueryable<carrot_Content> queryBlog = CannedQueries.GetBlogAllContentTbl(db, siteID);

			db.carrot_Contents.UpdateBatch(queryBlog, p => new carrot_Content { TemplateFile = sTemplateFile });

			db.SubmitChanges();
		}

		public void UpdateAllPageTemplates(Guid siteID, string sTemplateFile) {
			IQueryable<carrot_Content> queryContent = CannedQueries.GetContentAllContentTbl(db, siteID);

			db.carrot_Contents.UpdateBatch(queryContent, p => new carrot_Content { TemplateFile = sTemplateFile });

			db.SubmitChanges();
		}

		public void UpdateTopPageTemplates(Guid siteID, string sTemplateFile) {
			IQueryable<carrot_Content> queryContent = CannedQueries.GetContentTopContentTbl(db, siteID);

			db.carrot_Contents.UpdateBatch(queryContent, p => new carrot_Content { TemplateFile = sTemplateFile });

			db.SubmitChanges();
		}

		public void UpdateSubPageTemplates(Guid siteID, string sTemplateFile) {
			IQueryable<carrot_Content> queryContent = CannedQueries.GetContentSubContentTbl(db, siteID);

			db.carrot_Contents.UpdateBatch(queryContent, p => new carrot_Content { TemplateFile = sTemplateFile });

			db.SubmitChanges();
		}

		public static string CreateBlogDatePrefix(Guid siteID, DateTime goLiveDate) {
			string FileName = "";

			var ss = SiteData.GetSiteFromCache(siteID);
			if (ss.Blog_DatePattern.Length > 1) {
				FileName = "/" + goLiveDate.ToString(ss.Blog_DatePattern) + "/";
			} else {
				FileName = "/";
			}

			return ScrubPath(FileName);
		}

		public static string CreateFileNameFromSlug(Guid siteID, DateTime goLiveDate, string PageSlug) {
			string FileName = "";

			FileName = "/" + CreateBlogDatePrefix(siteID, goLiveDate) + "/" + PageSlug;

			return ScrubFilename(Guid.Empty, FileName);
		}

		public void BulkBlogFileNameUpdateFromDate(Guid siteID) {

			SiteData site = SiteData.GetSiteFromCache(siteID);

			db.carrot_UpdateGoLiveLocal(siteID, (int)site.SiteTimeZoneInfo.BaseUtcOffset.TotalMinutes);

			db.carrot_BlogDateFilenameUpdate(siteID);

			ResolveDuplicateBlogURLs(siteID);
		}

		public void ResolveDuplicateBlogURLs(Guid siteID) {
			SiteData site = SiteData.GetSiteFromCache(siteID);
			string SiteDatePattern = site.Blog_DatePattern;

			IQueryable<string> queryFindDups = CompiledQueries.cqBlogDupFileNames(db, siteID);

			Guid contentTypeID = ContentPageType.GetIDByType(ContentPageType.PageType.BlogEntry);

			foreach (string fileName in queryFindDups) {
				int iDupCtr = 1;
				IQueryable<carrot_RootContent> queryContentShareFilename = CompiledQueries.cqGeRootContentListByURLTbl(db, siteID, contentTypeID, fileName);

				foreach (carrot_RootContent item in queryContentShareFilename) {
					int c = -1;
					string sNewFilename = ScrubFilename(item.Root_ContentID, "/" + item.GoLiveDate.ToString(SiteDatePattern) + "/" + item.PageSlug);
					string sSlug = item.PageSlug;

					c = CompiledQueries.cqGeRootContentListNoMatchByURL(db, siteID, item.Root_ContentID, sNewFilename).Count();

					if (c > 0) {
						sNewFilename = sNewFilename.Substring(0, sNewFilename.Length - 5) + "-" + item.CreateDate.ToString("yyyy-MM-dd") + ".aspx";
						sSlug = sSlug.Substring(0, sSlug.Length - 5) + "-" + item.CreateDate.ToString("yyyy-MM-dd") + ".aspx";

						c = CompiledQueries.cqGeRootContentListNoMatchByURL(db, siteID, item.Root_ContentID, sNewFilename).Count();

						if (c > 0) {
							sNewFilename = sNewFilename.Substring(0, sNewFilename.Length - 5) + "-" + iDupCtr.ToString() + ".aspx";
							sSlug = sSlug.Substring(0, sSlug.Length - 5) + "-" + iDupCtr.ToString() + ".aspx";
						}
					}

					item.PageSlug = sSlug;
					item.FileName = sNewFilename;
					iDupCtr++;
					db.SubmitChanges();
				}
			}
		}

		public static string ScrubFilename(Guid rootContentID, string FileName) {
			string newFileName = FileName;

			if (string.IsNullOrEmpty(newFileName)) {
				newFileName = rootContentID.ToString();
			}
			newFileName = newFileName.Replace(@"\", @"/");

			if (!newFileName.StartsWith(@"/")) {
				newFileName = @"/" + newFileName;
			}

			newFileName = ScrubSpecial(newFileName);

			if (newFileName.EndsWith(@"/")) {
				newFileName = newFileName + SiteData.DefaultDirectoryFilename;
				newFileName = newFileName.Replace("//", "/");
			}

			if (newFileName.ToLower().EndsWith(".htm")) {
				newFileName = newFileName.Substring(0, newFileName.Length - 4) + ".aspx";
			}
			if (newFileName.ToLower().EndsWith(".html")) {
				newFileName = newFileName.Substring(0, newFileName.Length - 5) + ".aspx";
			}

			if (!newFileName.ToLower().EndsWith(".aspx")) {
				newFileName = newFileName + ".aspx";
			}

			return newFileName;
		}

		private static string ScrubSpecial(string sInput) {
			string sOutput = sInput;

			sOutput = sOutput.Replace("...", ".").Replace("...", ".").Replace("..", ".");
			sOutput = sOutput.Replace(" ", "-");
			sOutput = sOutput.Replace("'", "-");
			sOutput = sOutput.Replace("\"", "-");
			sOutput = sOutput.Replace("*", "-star-");
			sOutput = sOutput.Replace("%", "-percent-");
			sOutput = sOutput.Replace("&", "-n-");

			sOutput = sOutput.Replace("--", "-").Replace("--", "-");
			sOutput = sOutput.Replace("//", "/").Replace("//", "/");
			sOutput = sOutput.Trim();

			sOutput = Regex.Replace(sOutput, "[:\"*?<>|]+", "-");
			sOutput = Regex.Replace(sOutput, @"[^0-9a-zA-Z.-/_]+", "-");

			sOutput = sOutput.Replace("--", "-").Replace("--", "-");
			sOutput = sOutput.Replace("//", "/").Replace("//", "/");
			sOutput = sOutput.Trim();

			return sOutput;
		}


		public static string ScrubSlug(string SlugValue) {
			string newSlug = SlugValue;

			newSlug = newSlug.Replace(@"\", "");
			newSlug = newSlug.Replace(@"/", "");

			newSlug = ScrubSpecial(newSlug);

			return newSlug;
		}


		public static string ScrubPath(string FilePath) {
			string newFilePath = FilePath;

			newFilePath = newFilePath.Replace(@"\", @"/");

			if (!newFilePath.StartsWith(@"/")) {
				newFilePath = @"/" + newFilePath;
			}
			if (!newFilePath.EndsWith(@"/")) {
				newFilePath = newFilePath + @"/";
			}

			newFilePath = ScrubSpecial(newFilePath);

			newFilePath = newFilePath.Replace("//", "/");

			return newFilePath;
		}


		public string GetBlogHeadingFromURL(SiteData currentSite, string sFilterPath) {
			Guid siteID = currentSite.SiteID;

			string sTitle = String.Empty;

			if (sFilterPath.ToLower().StartsWith(currentSite.BlogCategoryPath.ToLower())) {
				vw_carrot_CategoryURL query = CompiledQueries.cqGetCategoryByURL(db, siteID, sFilterPath);
				sTitle = query.CategoryText;
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogTagPath.ToLower())) {
				vw_carrot_TagURL query = CompiledQueries.cqGetTagByURL(db, siteID, sFilterPath);
				sTitle = query.TagText;
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogDateFolderPath.ToLower())) {
				BlogDatePathParser p = new BlogDatePathParser(currentSite, sFilterPath);
				TimeSpan ts = p.dateEnd - p.dateBegin;

				int daysDelta = ts.Days;
				if (daysDelta < 370 && daysDelta > 90) {
					sTitle = "Year " + p.dateBegin.ToString("yyyy");
				}
				if (daysDelta < 36 && daysDelta > 3) {
					sTitle = p.dateBegin.ToString("MMMM yyyy");
				}
				if (daysDelta < 5) {
					sTitle = p.dateBegin.ToString("MMMM d, yyyy");
				}
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.SiteSearchPath.ToLower())) {
				string sSearchTerm = "";
				if (HttpContext.Current.Request.QueryString["search"] != null) {
					sSearchTerm = HttpContext.Current.Request.QueryString["search"].ToString();
				}

				sTitle = "Search results for '" + sSearchTerm + "' ";
			}

			return sTitle;
		}


		public Dictionary<string, float> GetPopularTemplateList(Guid siteID, ContentPageType.PageType pageType) {

			Dictionary<string, float> lstTemps = CannedQueries.GetTemplateCounts(db, siteID, pageType);

			return lstTemps;
		}

		public List<ContentPage> GetAllLatestContentList(Guid siteID) {
			List<ContentPage> lstContent = CannedQueries.GetAllContentList(db, siteID).Select(ct => new ContentPage(ct)).ToList();

			return lstContent;
		}

		public List<ContentPage> GetAllLatestBlogList(Guid siteID) {
			List<ContentPage> lstContent = CannedQueries.GetAllBlogList(db, siteID).Select(ct => new ContentPage(ct)).ToList();

			return lstContent;
		}

		public List<ContentPage> GetPagedSortedContent(Guid siteID, ContentPageType.PageType entryType, bool bActiveOnly, int pageSize, int pageNumber, string sSortParm) {

			string sortField = "";
			string sortDir = "";
			IQueryable<vw_carrot_Content> query1 = null;

			if (!string.IsNullOrEmpty(sSortParm)) {
				int pos = sSortParm.LastIndexOf(" ");
				sortField = sSortParm.Substring(0, pos).Trim();
				sortDir = sSortParm.Substring(pos).Trim();
			}

			query1 = CannedQueries.GetAllByTypeList(db, siteID, bActiveOnly, entryType);

			return PerformDataPagingQueryableContent(siteID, bActiveOnly, pageSize, pageNumber, sortField, sortDir, query1);
		}


		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType, bool bActiveOnly) {
			int iCount = CannedQueries.GetAllByTypeList(db, siteID, bActiveOnly, entryType).Count();
			return iCount;
		}

		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType) {
			int iCount = CannedQueries.GetAllByTypeList(db, siteID, false, entryType).Count();
			return iCount;
		}

		public int GetSiteContentCount(Guid siteID) {
			int iCount = CannedQueries.GetLatestContentList(db, siteID, false).Count();
			return iCount;
		}
		public int GetMaxNavOrder(Guid siteID) {
			int iCount = CompiledQueries.cqGetMaxOrderID(db, siteID);

			return iCount;
		}

		public List<ContentPage> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageNumber, sortField, sortDir);
		}

		public List<ContentPage> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber) {
			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageNumber);
		}

		public List<ContentPage> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber) {
			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageSize, pageNumber);
		}

		public List<ContentPage> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return GetLatestContentPagedList(siteID, postType, bActiveOnly, 10, pageNumber, sortField, sortDir);
		}

		public List<ContentPage> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber) {
			return GetLatestContentPagedList(siteID, postType, bActiveOnly, 10, pageNumber, "", "");
		}

		public List<ContentPage> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageSize, int pageNumber) {
			return GetLatestContentPagedList(siteID, postType, bActiveOnly, pageSize, pageNumber, "", "");
		}

		public List<ContentPage> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {

			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageSize, pageNumber, sortField, sortDir);
		}

		public List<ContentPage> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly,
					int pageSize, int pageNumber, string sortField, string sortDir) {

			IQueryable<vw_carrot_Content> query1 = null;

			if (postType == ContentPageType.PageType.ContentEntry) {
				query1 = CannedQueries.GetLatestContentList(db, siteID, bActiveOnly);
			} else {
				query1 = CannedQueries.GetLatestBlogList(db, siteID, bActiveOnly);
			}

			return PerformDataPagingQueryableContent(siteID, bActiveOnly, pageSize, pageNumber, sortField, sortDir, query1);
		}

		public int GetFilteredContentPagedCount(SiteData currentSite, string sFilterPath, bool bActiveOnly) {

			IQueryable<vw_carrot_Content> query1 = null;
			Guid siteID = currentSite.SiteID;
			bool bFound = false;

			if (sFilterPath.ToLower().StartsWith(currentSite.BlogCategoryPath.ToLower())) {
				query1 = CannedQueries.GetContentByCategoryURL(db, siteID, bActiveOnly, sFilterPath);
				bFound = true;
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogTagPath.ToLower())) {
				query1 = CannedQueries.GetContentByTagURL(db, siteID, bActiveOnly, sFilterPath);
				bFound = true;
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogDateFolderPath.ToLower())) {
				BlogDatePathParser p = new BlogDatePathParser(currentSite, sFilterPath);
				query1 = CannedQueries.GetLatestBlogListDateRange(db, siteID, p.dateBegin, p.dateEnd, bActiveOnly);
				bFound = true;
			}
			if (!bFound) {
				query1 = CannedQueries.GetLatestBlogList(db, siteID, bActiveOnly);
			}

			return query1.Count();
		}


		public List<ContentPage> GetFilteredContentPagedList(SiteData currentSite, string sFilterPath, bool bActiveOnly,
			int pageSize, int pageNumber, string sortField, string sortDir) {

			IQueryable<vw_carrot_Content> query1 = null;
			Guid siteID = currentSite.SiteID;
			bool bFound = false;

			if (sFilterPath.ToLower().StartsWith(currentSite.BlogCategoryPath.ToLower())) {
				query1 = CannedQueries.GetContentByCategoryURL(db, siteID, bActiveOnly, sFilterPath);
				bFound = true;
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogTagPath.ToLower())) {
				query1 = CannedQueries.GetContentByTagURL(db, siteID, bActiveOnly, sFilterPath);
				bFound = true;
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogDateFolderPath.ToLower())) {
				BlogDatePathParser p = new BlogDatePathParser(currentSite, sFilterPath);
				query1 = CannedQueries.GetLatestBlogListDateRange(db, siteID, p.dateBegin, p.dateEnd, bActiveOnly);
				bFound = true;
			}
			if (!bFound) {
				query1 = CannedQueries.GetLatestBlogList(db, siteID, bActiveOnly);
			}

			return PerformDataPagingQueryableContent(siteID, bActiveOnly, pageSize, pageNumber, sortField, sortDir, query1);
		}


		public List<ContentPage> PerformDataPagingQueryableContent(Guid siteID, bool bActiveOnly,
				int pageSize, int pageNumber, string sortField, string sortDir, IQueryable<vw_carrot_Content> QueryInput) {

			IEnumerable<ContentPage> lstContent = new List<ContentPage>();

			int startRec = pageNumber * pageSize;

			if (pageSize < 0 || pageSize > 200) {
				pageSize = 25;
			}

			if (pageNumber < 0 || pageNumber > 10000) {
				pageNumber = 0;
			}

			if (string.IsNullOrEmpty(sortField)) {
				sortField = "CreateDate";
			}

			if (string.IsNullOrEmpty(sortDir)) {
				sortDir = "DESC";
			}

			bool IsContentProp = false;

			sortDir = sortDir.ToUpper();

			sortField = (from p in ReflectionUtilities.GetPropertyStrings(typeof(vw_carrot_Content))
						 where p.ToLower().Trim() == sortField.ToLower().Trim()
						 select p).FirstOrDefault();

			if (!string.IsNullOrEmpty(sortField)) {
				IsContentProp = ReflectionUtilities.DoesPropertyExist(typeof(vw_carrot_Content), sortField);
			}

			if (IsContentProp) {
				QueryInput = ReflectionUtilities.SortByParm<vw_carrot_Content>(QueryInput, sortField, sortDir);
			} else {
				QueryInput = (from c in QueryInput
							  orderby c.CreateDate descending
							  where c.SiteID == siteID
								 && c.IsLatestVersion == true
								 && (c.PageActive == bActiveOnly || bActiveOnly == false)
							  select c).AsQueryable();
			}

			lstContent = (from q in QueryInput
						  select new ContentPage(q)).Skip(startRec).Take(pageSize);

			return lstContent.ToList();
		}


		public void ResetHeartbeatLock(Guid rootContentID, Guid siteID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, siteID, rootContentID);

			rc.EditHeartbeat = DateTime.UtcNow.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public bool RecordHeartbeatLock(Guid rootContentID, Guid siteID, Guid currentUserID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, siteID, rootContentID);

			if (rc != null) {
				rc.Heartbeat_UserId = currentUserID;
				rc.EditHeartbeat = DateTime.UtcNow;
				db.SubmitChanges();
				return true;
			}

			return false;
		}


		public bool IsPageLocked(Guid rootContentID, Guid siteID, Guid currentUserID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, siteID, rootContentID);

			bool bLock = false;
			if (rc != null && rc.Heartbeat_UserId != null) {
				if (rc.Heartbeat_UserId != currentUserID
						&& rc.EditHeartbeat.Value > DateTime.UtcNow.AddMinutes(-2)) {
					bLock = true;
				}
				if (rc.Heartbeat_UserId == currentUserID
					|| rc.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public bool IsPageLocked(Guid rootContentID, Guid siteID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, siteID, rootContentID);

			bool bLock = false;
			if (rc.Heartbeat_UserId != null) {
				if (rc.Heartbeat_UserId != SecurityData.CurrentUserGuid
						&& rc.EditHeartbeat.Value > DateTime.UtcNow.AddMinutes(-2)) {
					bLock = true;
				}
				if (rc.Heartbeat_UserId == SecurityData.CurrentUserGuid
					|| rc.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public bool IsPageLocked(ContentPage cp) {

			bool bLock = false;
			if (cp.Heartbeat_UserId != null) {
				if (cp.Heartbeat_UserId != SecurityData.CurrentUserGuid
						&& cp.EditHeartbeat.Value > DateTime.UtcNow.AddMinutes(-2)) {
					bLock = true;
				}
				if (cp.Heartbeat_UserId == SecurityData.CurrentUserGuid
					|| cp.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public Guid GetCurrentEditUser(Guid rootContentID, Guid siteID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, siteID, rootContentID);

			if (rc != null) {
				return (Guid)rc.Heartbeat_UserId;
			} else {
				return Guid.Empty;
			}
		}


		public ContentPage CopyContentPageToNew(ContentPage pageSource) {

			SiteData site = SiteData.GetSiteFromCache(pageSource.SiteID);

			ContentPage pageNew = new ContentPage();
			pageNew.ContentID = Guid.NewGuid();
			pageNew.SiteID = pageSource.SiteID;
			pageNew.Parent_ContentID = pageSource.Parent_ContentID;
			pageNew.Root_ContentID = pageSource.Root_ContentID;

			pageNew.PageText = pageSource.PageText;
			pageNew.LeftPageText = pageSource.LeftPageText;
			pageNew.RightPageText = pageSource.RightPageText;

			pageNew.IsLatestVersion = true;
			pageNew.FileName = pageSource.FileName;
			pageNew.TitleBar = pageSource.TitleBar;
			pageNew.NavMenuText = pageSource.NavMenuText;
			pageNew.NavOrder = pageSource.NavOrder;
			pageNew.PageHead = pageSource.PageHead;
			pageNew.PageActive = pageSource.PageActive;
			pageNew.EditUserId = SecurityData.CurrentUserGuid;
			pageNew.EditDate = site.Now;
			pageNew.CreateDate = pageSource.CreateDate;
			pageNew.GoLiveDate = pageSource.GoLiveDate;
			pageNew.RetireDate = pageSource.RetireDate;

			pageNew.TemplateFile = pageSource.TemplateFile;
			pageNew.MetaDescription = pageSource.MetaDescription;
			pageNew.MetaKeyword = pageSource.MetaKeyword;

			pageNew.ContentType = pageSource.ContentType;
			pageNew.PageSlug = pageSource.PageSlug;

			pageNew.ContentCategories = pageSource.ContentCategories;
			pageNew.ContentTags = pageSource.ContentTags;

			return pageNew;
		}


		public static ContentPage GetSamplerView() {

			string sFile1 = "";
			string sFile2 = "";

			try {
				Assembly _assembly = Assembly.GetExecutingAssembly();

				using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.Core.SiteContent.Mock.SampleContent1.txt"))) {
					sFile1 = oTextStream.ReadToEnd();
				}
				using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.Core.SiteContent.Mock.SampleContent2.txt"))) {
					sFile2 = oTextStream.ReadToEnd();
				}

				List<string> imageNames = (from i in _assembly.GetManifestResourceNames()
										   where i.Contains("SiteContent.Mock.sample")
										   && i.EndsWith(".png")
										   select i).ToList();

				foreach (string img in imageNames) {
					var imgURL = CMSConfigHelper.GetWebResourceUrl(typeof(ContentPage), img);
					sFile1 = sFile1.Replace(img, imgURL);
					sFile2 = sFile2.Replace(img, imgURL);
				}

			} catch { }

			ContentPage pageNew = new ContentPage();
			pageNew.Root_ContentID = SiteData.CurrentSiteID;
			pageNew.ContentID = pageNew.Root_ContentID;
			pageNew.SiteID = SiteData.CurrentSiteID;
			pageNew.Parent_ContentID = null;

			pageNew.PageText = "<h2>Content CENTER</h2>\r\n" + sFile1;
			pageNew.LeftPageText = "<h2>Content LEFT</h2>\r\n" + sFile2;
			pageNew.RightPageText = "<h2>Content RIGHT</h2>\r\n" + sFile2;

			pageNew.IsLatestVersion = true;
			pageNew.NavOrder = -1;
			pageNew.TitleBar = "Template Preview - TITLE";
			pageNew.NavMenuText = "Template PV - NAV"; ;
			pageNew.PageHead = "Template Preview - HEAD";
			pageNew.PageActive = true;
			pageNew.EditUserId = SecurityData.CurrentUserGuid;
			pageNew.EditDate = DateTime.Now.AddDays(-1);
			pageNew.CreateDate = DateTime.Now.AddDays(-14);
			pageNew.GoLiveDate = pageNew.EditDate.AddHours(-5);
			pageNew.RetireDate = pageNew.CreateDate.AddYears(5);

			pageNew.TemplateFile = SiteData.PreviewTemplateFile;
			pageNew.FileName = SiteData.PreviewTemplateFilePage;
			pageNew.MetaDescription = "Meta Description";
			pageNew.MetaKeyword = "Meta Keyword";

			pageNew.ContentType = ContentPageType.PageType.BlogEntry;
			pageNew.PageSlug = "sampler-page-view";

			List<ContentCategory> lstK = new List<ContentCategory>();
			List<ContentTag> lstT = new List<ContentTag>();

			for (int i = 0; i < 5; i++) {
				ContentCategory k = new ContentCategory {
					ContentCategoryID = Guid.NewGuid(),
					CategoryText = "Keyword Text " + i.ToString(),
					CategorySlug = "keyword-slug-" + i.ToString()
				};
				ContentTag t = new ContentTag {
					ContentTagID = Guid.NewGuid(),
					TagText = "Tag Text " + i.ToString(),
					TagSlug = "tag-slug-" + i.ToString()
				};

				lstK.Add(k);
				lstT.Add(t);
			}


			pageNew.ContentCategories = lstK;
			pageNew.ContentTags = lstT;

			return pageNew;
		}


		public List<ContentPage> GetVersionHistory(Guid siteID, Guid rootContentID) {
			List<ContentPage> content = CompiledQueries.cqGetVersionHistory(db, siteID, rootContentID).Select(ct => new ContentPage(ct)).ToList();

			return content;
		}

		public ContentPage GetVersion(Guid siteID, Guid contentID) {
			ContentPage content = null;
			vw_carrot_Content cont = CompiledQueries.cqGetContentByContentID(db, siteID, contentID);
			if (cont != null) {
				content = new ContentPage(cont);
			}
			return content;
		}

		public List<ContentPage> GetLatestContentList(Guid siteID, bool bActiveOnly) {
			List<ContentPage> lstContent = CompiledQueries.GetLatestContentList(db, siteID, bActiveOnly).Select(ct => new ContentPage(ct)).ToList();

			return lstContent;
		}


		public void RemoveVersions(Guid siteID, List<Guid> lstDel) {

			//List<carrot_Content> lstContent = (from ct in db.carrot_Contents
			//                                   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
			//                                   orderby ct.EditDate descending
			//                                   where r.SiteID == siteID
			//                                    && lstDel.Contains(ct.ContentID)
			//                                    && ct.IsLatestVersion != true
			//                                   select ct).ToList();

			//if (lstContent.Count > 0) {
			//    db.carrot_Contents.DeleteAllOnSubmit(lstContent);
			//    db.SubmitChanges();
			//}

			IQueryable<carrot_Content> queryCont = (from ct in db.carrot_Contents
													join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
													orderby ct.EditDate descending
													where r.SiteID == siteID
													 && lstDel.Contains(ct.ContentID)
													 && ct.IsLatestVersion != true
													select ct);

			if (lstDel.Count > 0) {
				db.carrot_Contents.DeleteBatch(queryCont);
				db.SubmitChanges();
			}

		}

		public ContentPage FindContentByID(Guid siteID, Guid rootContentID) {
			ContentPage content = null;
			vw_carrot_Content cont = CompiledQueries.GetLatestContentByID(db, siteID, false, rootContentID);
			if (cont != null) {
				content = new ContentPage(cont);
			}
			return content;
		}

		public ContentPage GetLatestContentByURL(Guid siteID, bool bActiveOnly, string sPage) {
			ContentPage content = null;
			vw_carrot_Content cont = CompiledQueries.GetLatestContentByURL(db, siteID, bActiveOnly, sPage);
			if (cont != null) {
				content = new ContentPage(cont);
			}
			return content;
		}

		public ContentPage FindByFilename(Guid siteID, string urlFileName) {
			ContentPage content = null;
			vw_carrot_Content cont = CompiledQueries.GetLatestContentByURL(db, siteID, false, urlFileName);
			if (cont != null) {
				content = new ContentPage(cont);
			}
			return content;
		}

		public ContentPage FindByPageSlug(Guid siteID, DateTime datePublished, string urlPageSlug) {
			ContentPage content = null;
			vw_carrot_Content cont = CompiledQueries.cqGetLatestContentBySlug(db, siteID, datePublished, urlPageSlug);
			if (cont != null) {
				content = new ContentPage(cont);
			}
			return content;
		}

		public List<ContentPage> FindPagesBeginingWith(Guid siteID, string sFolderPath) {
			List<ContentPage> lstContent = (from ct in GetPagesBeginingWith(siteID, sFolderPath).ToList()
											select new ContentPage(ct)).ToList();

			return lstContent;
		}

		public int FindCountPagesBeginingWith(Guid siteID, string sFolderPath) {
			sFolderPath = ("/" + sFolderPath.ToLower() + "/").Replace("//", "/");
			return GetPagesBeginingWith(siteID, sFolderPath).Count();
		}

		private IQueryable<vw_carrot_Content> GetPagesBeginingWith(Guid siteID, string sFolderPath) {
			IQueryable<vw_carrot_Content> query = (from ct in db.vw_carrot_Contents
												   where ct.SiteID == siteID
														&& ct.FileName.ToLower().StartsWith(sFolderPath.ToLower())
														&& ct.IsLatestVersion == true
												   select ct);

			return query;
		}

		public ContentPage FindHome(Guid siteID) {
			ContentPage content = null;
			vw_carrot_Content cont = CompiledQueries.FindHome(db, siteID, true);
			if (cont != null) {
				content = new ContentPage(cont);
			}
			return content;
		}

		public ContentPage FindHome(Guid siteID, bool bActiveOnly) {
			ContentPage content = null;
			vw_carrot_Content cont = CompiledQueries.FindHome(db, siteID, bActiveOnly);
			if (cont != null) {
				content = new ContentPage(cont);
			}
			return content;
		}

		public List<ContentPage> FindPageByTitleAndDate(Guid siteID, string sTitle, string sFileNameFrag, DateTime dateCreate) {
			SiteData site = SiteData.GetSiteFromCache(siteID);

			//DateTime dateUTC = site.ConvertSiteTimeToUTC(dateCreate);

			List<ContentPage> lstContent = (from ct in CannedQueries.FindPageByTitleAndDate(db, siteID, sTitle, sFileNameFrag, dateCreate).ToList()
											select new ContentPage(ct)).ToList();

			return lstContent;
		}

		public List<ContentPage> GetChildNavigation(Guid siteID, string sParentPage, bool bActiveOnly) {

			vw_carrot_Content c = CompiledQueries.GetLatestContentByURL(db, siteID, false, sParentPage);

			if (c != null) {
				return GetChildNavigation(siteID, c.Root_ContentID, bActiveOnly);
			} else {
				return null;
			}
		}

		public List<ContentPage> GetChildNavigation(Guid siteID, Guid? ParentID, bool bActiveOnly) {

			List<ContentPage> lstContent = (from ct in CompiledQueries.GetLatestContentByParent(db, siteID, ParentID, bActiveOnly).ToList()
											select new ContentPage(ct)).ToList();

			return lstContent;
		}

		public List<ContentPage> GetParentWithChildNavigation(Guid siteID, Guid? ParentID, bool bActiveOnly) {

			List<ContentPage> lstContent = (from ct in CompiledQueries.GetLatestContentWithParent(db, siteID, ParentID, bActiveOnly).ToList()
											select new ContentPage(ct)).ToList();

			return lstContent;
		}

		public List<ContentPage> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			List<ContentPage> lstContent = CompiledQueries.TopLevelPages(db, siteID, bActiveOnly).Select(ct => new ContentPage(ct)).ToList();

			return lstContent;
		}

		public List<ContentPage> GetPostsByDateRange(Guid siteID, DateTime dateMidpoint, int iDayRange, bool bActiveOnly) {

			DateTime dateBegin = dateMidpoint.AddDays(0 - iDayRange);
			DateTime dateEnd = dateMidpoint.AddDays(iDayRange);

			List<ContentPage> lstContent = CompiledQueries.PostsByDateRange(db, siteID, dateBegin, dateEnd, bActiveOnly).Select(ct => new ContentPage(ct)).ToList();

			return lstContent;
		}

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}

}
