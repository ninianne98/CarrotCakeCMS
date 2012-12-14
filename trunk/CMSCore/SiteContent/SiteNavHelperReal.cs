using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
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

	public class SiteNavHelperReal : IDisposable, ISiteNavHelper {
		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public SiteNavHelperReal() { }

		internal static SiteNav CreateSiteNav(vw_carrot_Content c) {
			SiteNav nav = null;

			if (c != null) {
				nav = new SiteNav();

				nav.Root_ContentID = c.Root_ContentID;
				nav.SiteID = c.SiteID;
				nav.FileName = c.FileName;
				nav.PageActive = c.PageActive;
				nav.CreateDate = c.CreateDate;
				nav.EditUserId = c.EditUserId;
				nav.ContentType = ContentPageType.GetTypeByID(c.ContentTypeID);
				nav.ContentID = c.ContentID;
				nav.Parent_ContentID = c.Parent_ContentID;
				nav.TitleBar = c.TitleBar;
				nav.NavMenuText = c.NavMenuText;
				nav.PageHead = c.PageHead;
				nav.PageText = c.PageText;
				nav.NavOrder = c.NavOrder;
				nav.EditDate = c.EditDate;
				nav.TemplateFile = c.TemplateFile;
			}

			return nav;
		}

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = (from ct in CompiledQueries.cqContentNavAll(db, siteID, bActiveOnly)
										select CreateSiteNav(ct)).ToList();

			return lstContent;
		}



		public List<SiteNav> GetTwoLevelNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = null;

			List<Guid> lstTop = CompiledQueries.cqTopLevelPages(db, siteID, false).Select(z => z.Root_ContentID).ToList();


			lstContent = (from ct in CompiledQueries.cqContentNavAll(db, siteID, bActiveOnly)
						  orderby ct.NavOrder, ct.NavMenuText
						  where ct.SiteID == siteID
								&& (ct.PageActive == bActiveOnly || bActiveOnly == false)
								&& ct.IsLatestVersion == true
								&& (lstTop.Contains(ct.Root_ContentID) || lstTop.Contains(ct.Parent_ContentID.Value))
						  select CreateSiteNav(ct)).ToList();

			return lstContent;
		}

		public List<SiteNav> GetLevelDepthNavigation(Guid siteID, int iDepth, bool bActiveOnly) {

			List<SiteNav> lstContent = null;
			List<Guid> lstSub = new List<Guid>();

			if (iDepth < 1) {
				iDepth = 1;
			}

			if (iDepth > 10) {
				iDepth = 10;
			}

			List<Guid> lstTop = CompiledQueries.cqTopLevelPages(db, siteID, false).Select(z => z.Root_ContentID).ToList();

			while (iDepth > 1) {

				lstSub = (from ct in CompiledQueries.cqContentNavAll(db, siteID, bActiveOnly)
						  where ct.SiteID == siteID
								&& ct.IsLatestVersion == true
								&& (ct.PageActive == bActiveOnly || bActiveOnly == false)
								&& (!lstTop.Contains(ct.Root_ContentID) && lstTop.Contains(ct.Parent_ContentID.Value))
						  select ct.Root_ContentID).Distinct().ToList();

				lstTop = lstTop.Union(lstSub).ToList();

				iDepth--;
			}

			lstContent = (from ct in CompiledQueries.cqContentNavAll(db, siteID, bActiveOnly)
						  orderby ct.NavOrder, ct.NavMenuText
						  where ct.SiteID == siteID
								&& (ct.PageActive == bActiveOnly || bActiveOnly == false)
								&& ct.IsLatestVersion == true
								&& lstTop.Contains(ct.Root_ContentID)
						  select CreateSiteNav(ct)).ToList();

			return lstContent;
		}


		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = CompiledQueries.cqTopLevelPages(db, siteID, bActiveOnly).Select(ct => CreateSiteNav(ct)).ToList();

			return lstContent;
		}

		private SiteNav GetPageNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			SiteNav content = CreateSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, bActiveOnly, rootContentID));

			return content;
		}


		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly) {

			List<SiteNav> lstContent = new List<SiteNav>();

			int iOrder = 1000000;

			if (rootContentID != Guid.Empty) {
				Guid? gLast = rootContentID;

				while (gLast.HasValue) {
					SiteNav nav = GetPageNavigation(siteID, gLast.Value, false);
					gLast = null;

					if (nav != null) {
						nav.NavOrder = iOrder;
						lstContent.Add(nav);
						iOrder--;

						gLast = nav.Parent_ContentID;
					}
				}

				SiteNav home = FindHome(siteID, false);
				home.NavOrder = 0;

				if (lstContent.Where(x => x.Root_ContentID == home.Root_ContentID).Count() < 1) {
					lstContent.Add(home);
				}

			}

			return lstContent.OrderBy(x => x.NavOrder).ToList();

		}

		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, string sPage, bool bActiveOnly) {

			vw_carrot_Content c = CompiledQueries.cqGetLatestContentByURL(db, siteID, false, sPage);

			if (c != null) {
				return GetPageCrumbNavigation(siteID, c.Root_ContentID, bActiveOnly);
			} else {
				return null;
			}
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sParentPage, bool bActiveOnly) {

			vw_carrot_Content c = CompiledQueries.cqGetLatestContentByURL(db, siteID, false, sParentPage);

			if (c != null) {
				return GetChildNavigation(siteID, c.Root_ContentID, bActiveOnly);
			} else {
				return null;
			}
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid? ParentID, bool bActiveOnly) {

			List<SiteNav> lstContent = (from ct in CompiledQueries.cqGetLatestContentByParent(db, siteID, ParentID, bActiveOnly).ToList()
										select CreateSiteNav(ct)).ToList();

			return lstContent;
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {

			SiteNav content = CreateSiteNav(CompiledQueries.cqGetLatestContentByURL(db, siteID, false, sPage));

			return content;
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {

			SiteNav content = CreateSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, false, rootContentID));

			return content;
		}


		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {
			SiteNav nav1 = GetPageNavigation(siteID, sPage);

			if (nav1 != null) {
				return GetParentPageNavigation(siteID, nav1.Root_ContentID);
			} else {
				return null;
			}
		}


		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID) {
			SiteNav nav1 = GetPageNavigation(siteID, rootContentID);

			if (nav1.ContentType == ContentPageType.PageType.BlogEntry) {
				Guid? parentid = SiteData.GetSiteByID(siteID).Blog_Root_ContentID;
				nav1.Parent_ContentID = parentid;
			}

			SiteNav content = null;
			if (nav1 != null && nav1.Parent_ContentID.HasValue) {
				content = CreateSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, false, nav1.Parent_ContentID.Value));
			}

			return content;
		}


		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {

			vw_carrot_Content c = CompiledQueries.cqGetLatestContentByID(db, siteID, false, PageID);

			List<SiteNav> lstContent = new List<SiteNav>();

			if (c != null) {
				lstContent = (from ct in CompiledQueries.cqGetLatestContentByParent(db, siteID, c.Parent_ContentID, bActiveOnly).ToList()
							  select CreateSiteNav(ct)).ToList();
			}

			return lstContent;
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {

			vw_carrot_Content c = CompiledQueries.cqGetLatestContentByURL(db, siteID, false, sPage);

			if (c != null) {
				return GetSiblingNavigation(siteID, c.Root_ContentID, bActiveOnly);
			} else {
				return null;
			}
		}

		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {
			List<SiteNav> lstContent = (from ct in CannedQueries.GetLatestContentListUnordered(db, siteID, bActiveOnly)
										orderby ct.EditDate descending
										select CreateSiteNav(ct)).Take(iUpdates).ToList();

			return lstContent;
		}

		public List<SiteNav> GetLatestPosts(Guid siteID, int iUpdates, bool bActiveOnly) {
			List<SiteNav> lstContent = (from ct in CannedQueries.GetLatestBlogListUnordered(db, siteID, bActiveOnly)
										orderby ct.EditDate descending
										select CreateSiteNav(ct)).Take(iUpdates).ToList();

			return lstContent;
		}

		public List<IContentMetaInfo> GetTagList(Guid siteID, int iUpdates) {
			List<IContentMetaInfo> lstContent = (from ct in CannedQueries.GetTagURLs(db, siteID)
												 orderby ct.UseCount descending
												 select (IContentMetaInfo)ContentTag.CreateTag(ct)).Take(iUpdates).ToList();

			return lstContent;
		}

		public List<IContentMetaInfo> GetCategoryList(Guid siteID, int iUpdates) {
			List<IContentMetaInfo> lstContent = (from ct in CannedQueries.GetCategoryURLs(db, siteID)
												 orderby ct.UseCount descending
												 select (IContentMetaInfo)ContentCategory.CreateCategory(ct)).Take(iUpdates).ToList();

			return lstContent;
		}

		public List<IContentMetaInfo> GetMonthBlogUpdateList(Guid siteID, int iUpdates) {
			//List<IContentMetaInfo> lstContent = new List<IContentMetaInfo>();

			SiteData site = SiteData.GetSiteByID(siteID);

			List<IContentMetaInfo> lstContent = (from ct in CannedQueries.GetBlogContentTallies(db, siteID)
												 orderby ct.DateMonth descending
												 select (IContentMetaInfo)(
												 new ContentDateTally {
													 DateCaption = ct.DateSlug,
													 CreateDate = Convert.ToDateTime(ct.DateMonth),
													 UseCount = Convert.ToInt32(ct.ContentCount),
													 TheSite = site,
													 TallyID = Guid.NewGuid()
												 })).Take(iUpdates).ToList();


			return lstContent;
		}

		public List<IContentMetaInfo> GetTagListForPost(Guid siteID, int iUpdates, string urlFileName) {
			List<IContentMetaInfo> lstContent = (from ct in CannedQueries.GetPostTagURLs(db, siteID, urlFileName)
												 orderby ct.TagText
												 select (IContentMetaInfo)ContentTag.CreateTag(ct)).Take(iUpdates).ToList();

			return lstContent;
		}
		public List<IContentMetaInfo> GetCategoryListForPost(Guid siteID, int iUpdates, string urlFileName) {
			List<IContentMetaInfo> lstContent = (from ct in CannedQueries.GetPostCategoryURL(db, siteID, urlFileName)
												 orderby ct.CategoryText
												 select (IContentMetaInfo)ContentCategory.CreateCategory(ct)).Take(iUpdates).ToList();

			return lstContent;
		}
		public List<IContentMetaInfo> GetTagListForPost(Guid siteID, int iUpdates, Guid rootContentID) {
			List<IContentMetaInfo> lstContent = (from ct in CannedQueries.GetPostTagURLs(db, siteID, rootContentID)
												 orderby ct.TagText
												 select (IContentMetaInfo)ContentTag.CreateTag(ct)).Take(iUpdates).ToList();

			return lstContent;
		}
		public List<IContentMetaInfo> GetCategoryListForPost(Guid siteID, int iUpdates, Guid rootContentID) {
			List<IContentMetaInfo> lstContent = (from ct in CannedQueries.GetPostCategoryURL(db, siteID, rootContentID)
												 orderby ct.CategoryText
												 select (IContentMetaInfo)ContentCategory.CreateCategory(ct)).Take(iUpdates).ToList();

			return lstContent;
		}

		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {
			SiteNav content = CreateSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, false, rootContentID));

			return content;
		}

		public SiteNav GetLatestVersion(Guid siteID, bool bActiveOnly, string sPage) {
			SiteNav content = CreateSiteNav(CompiledQueries.cqGetLatestContentByURL(db, siteID, bActiveOnly, sPage));

			return content;
		}

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {
			SiteNav content = CreateSiteNav(CompiledQueries.cqGetLatestContentByURL(db, siteID, false, urlFileName));

			return content;
		}

		public SiteNav FindHome(Guid siteID) {
			SiteNav content = CreateSiteNav(CompiledQueries.cqFindHome(db, siteID, true));

			return content;
		}

		public SiteNav FindHome(Guid siteID, bool bActiveOnly) {
			SiteNav content = CreateSiteNav(CompiledQueries.cqFindHome(db, siteID, bActiveOnly));

			return content;
		}

		public List<ContentDateLinks> GetSingleMonthBlogUpdateList(SiteData currentSite, DateTime monthDate, bool bActiveOnly) {
			List<ContentDateLinks> lstContent = new List<ContentDateLinks>();
			DateTime dateBegin = monthDate.AddDays(0 - monthDate.Day).AddDays(1);
			DateTime dateEnd = dateBegin.AddMonths(1).AddMilliseconds(-1);

			IQueryable<vw_carrot_Content> query1 = CannedQueries.GetLatestBlogListDateRange(db, currentSite.SiteID, dateBegin, dateEnd, bActiveOnly);

			lstContent = (from p in query1
						  group p by p.CreateDate.Date into g
						  select new ContentDateLinks { PostDate = g.Key, UseCount = g.Count() }).ToList();

			lstContent.ToList().ForEach(q => q.TheSite = currentSite);

			return lstContent;
		}

		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType, bool bActiveOnly) {
			Guid contentTypeID = ContentPageType.GetIDByType(entryType);
			int iCount = CompiledQueries.cqGetAllRootByType(db, siteID, contentTypeID, bActiveOnly).Count();

			return iCount;
		}

		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType) {
			Guid contentTypeID = ContentPageType.GetIDByType(entryType);
			int iCount = CompiledQueries.cqGetAllRootByType(db, siteID, contentTypeID, false).Count();

			return iCount;
		}

		public int GetSiteContentCount(Guid siteID) {
			int iCount = CannedQueries.GetLatestContentListUnordered(db, siteID, false).Count();

			return iCount;
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageNumber, sortField, sortDir);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber) {
			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageNumber);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber) {
			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageSize, pageNumber);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return GetLatestContentPagedList(siteID, postType, bActiveOnly, 10, pageNumber, sortField, sortDir);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber) {
			return GetLatestContentPagedList(siteID, postType, bActiveOnly, 10, pageNumber, "", "");
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageSize, int pageNumber) {
			return GetLatestContentPagedList(siteID, postType, bActiveOnly, pageSize, pageNumber, "", "");
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {

			return GetLatestContentPagedList(siteID, ContentPageType.PageType.BlogEntry, bActiveOnly, pageSize, pageNumber, sortField, sortDir);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly,
					int pageSize, int pageNumber, string sortField, string sortDir) {

			IQueryable<vw_carrot_Content> query1 = null;

			if (postType == ContentPageType.PageType.ContentEntry) {
				query1 = CannedQueries.GetLatestContentListUnordered(db, siteID, bActiveOnly);
			} else {
				query1 = CannedQueries.GetLatestBlogListUnordered(db, siteID, bActiveOnly);
			}

			return PerformDataPagingQueryableContent(siteID, bActiveOnly, pageSize, pageNumber, sortField, sortDir, query1);
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
				if (daysDelta > 90) {
					sTitle = "Year " + p.dateBegin.ToString("yyyy");
				}
				if (daysDelta < 36) {
					sTitle = p.dateBegin.ToString("MMMM yyyy");
				}
				if (daysDelta < 5) {
					sTitle = p.dateBegin.ToString("MMMM d, yyyy");
				}
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.SiteSearchPath.ToLower())) {
				sTitle = "Search Results";
			}

			return sTitle;
		}


		public int GetSiteSearchCount(Guid siteID, string searchTerm, bool bActiveOnly) {

			IQueryable<vw_carrot_Content> query1 = CannedQueries.GetContentSiteSearch(db, siteID, bActiveOnly, searchTerm);

			return query1.Count();
		}

		public List<SiteNav> GetLatestContentSearchList(Guid siteID, string searchTerm, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {

			IQueryable<vw_carrot_Content> query1 = CannedQueries.GetContentSiteSearch(db, siteID, bActiveOnly, searchTerm);

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
				query1 = CannedQueries.GetLatestBlogListUnordered(db, siteID, bActiveOnly);
			}

			return query1.Count();
		}

		public int GetFilteredContentByIDPagedCount(SiteData currentSite, List<Guid> lstCategories, bool bActiveOnly) {

			Guid siteID = currentSite.SiteID;

			IQueryable<vw_carrot_Content> query1 = CannedQueries.GetContentByCategoryIDs(db, siteID, bActiveOnly, lstCategories);

			return query1.Count();
		}

		public List<SiteNav> GetFilteredContentPagedList(SiteData currentSite, string sFilterPath, bool bActiveOnly,
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
				query1 = CannedQueries.GetLatestBlogListUnordered(db, siteID, bActiveOnly);
			}

			return PerformDataPagingQueryableContent(siteID, bActiveOnly, pageSize, pageNumber, sortField, sortDir, query1);
		}

		public List<SiteNav> GetFilteredContentByIDPagedList(SiteData currentSite, List<Guid> lstCategories, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			Guid siteID = currentSite.SiteID;

			IQueryable<vw_carrot_Content> query1 = CannedQueries.GetContentByCategoryIDs(db, siteID, bActiveOnly, lstCategories);

			return PerformDataPagingQueryableContent(siteID, bActiveOnly, pageSize, pageNumber, sortField, sortDir, query1);
		}

		public List<SiteNav> PerformDataPagingQueryableContent(Guid siteID, bool bActiveOnly,
				int pageSize, int pageNumber, string sortField, string sortDir, IQueryable<vw_carrot_Content> QueryInput) {

			IEnumerable<SiteNav> lstContent = new List<SiteNav>();

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
						  select CreateSiteNav(q)).Skip(startRec).Take(pageSize);

			return lstContent.ToList();
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
