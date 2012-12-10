using System;
using System.Collections.Generic;
using System.Linq;
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

	public class SiteNavHelperMock : ISiteNavHelper {

		public SiteNavHelperMock() { }

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav();
		}

		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav();
		}

		public List<SiteNav> GetTwoLevelNavigation(Guid siteID, bool bActiveOnly) {

			List<SiteNav> lstNav = SiteNavHelper.GetSamplerFakeNav();
			List<SiteNav> lstNav2 = new List<SiteNav>();

			foreach (SiteNav l in lstNav) {
				lstNav2 = lstNav2.Union(SiteNavHelper.GetSamplerFakeNav(l.Root_ContentID)).ToList();
			}

			lstNav = lstNav.Union(lstNav2).ToList();
			return lstNav;
		}

		public List<SiteNav> GetLevelDepthNavigation(Guid siteID, int iDepth, bool bActiveOnly) {

			List<SiteNav> lstNav = SiteNavHelper.GetSamplerFakeNav();
			List<SiteNav> lstNav2 = new List<SiteNav>();

			foreach (SiteNav l1 in lstNav) {
				List<SiteNav> lst = SiteNavHelper.GetSamplerFakeNav(l1.Root_ContentID);
				lstNav2 = lstNav2.Union(lst).ToList();

				foreach (SiteNav l2 in lst) {
					List<SiteNav> lst2 = SiteNavHelper.GetSamplerFakeNav(l2.Root_ContentID);
					lstNav2 = lstNav2.Union(lst2).ToList();
				}
			}

			lstNav = lstNav.Union(lstNav2).ToList();
			return lstNav;
		}


		public List<IContentMetaInfo> GetCategoryList(Guid siteID, int iUpdates) {
			List<IContentMetaInfo> lstContent = (from ct in SiteNavHelper.GetSamplerFakeNav(8)
												 orderby ct.NavOrder descending
												 select (IContentMetaInfo)new ContentCategory {
													 SiteID = ct.SiteID,
													 CategoryURL = ct.FileName,
													 CategoryText = ct.NavMenuText,
													 UseCount = ct.NavOrder + 2
												 }).Take(iUpdates).ToList();

			return lstContent;
		}

		public List<IContentMetaInfo> GetTagList(Guid siteID, int iUpdates) {
			List<IContentMetaInfo> lstContent = (from ct in SiteNavHelper.GetSamplerFakeNav(8)
												 orderby ct.NavOrder descending
												 select (IContentMetaInfo)new ContentTag {
													 SiteID = ct.SiteID,
													 TagURL = ct.FileName,
													 TagText = ct.NavMenuText,
													 UseCount = ct.NavOrder + 2
												 }).Take(iUpdates).ToList();

			return lstContent;
		}

		public List<IContentMetaInfo> GetTagListForPost(Guid siteID, int iUpdates, string urlFileName) {
			return GetTagList(siteID, 3);
		}
		public List<IContentMetaInfo> GetCategoryListForPost(Guid siteID, int iUpdates, string urlFileName) {
			return GetCategoryList(siteID, 5);
		}
		public List<IContentMetaInfo> GetTagListForPost(Guid siteID, int iUpdates, Guid rootContentID) {
			return GetTagList(siteID, 3);
		}
		public List<IContentMetaInfo> GetCategoryListForPost(Guid siteID, int iUpdates, Guid rootContentID) {
			return GetCategoryList(siteID, 5);
		}

		public List<IContentMetaInfo> GetMonthBlogUpdateList(Guid siteID, int iUpdates) {
			List<IContentMetaInfo> lstContent = new List<IContentMetaInfo>();
			int n = 0;
			DateTime dateNow = DateTime.Now.Date;

			while (n < iUpdates) {
				dateNow = DateTime.Now.Date.AddMonths(0 - n);

				ContentCategory cc = new ContentCategory();
				cc.SiteID = siteID;
				cc.UseCount = n * 3;
				cc.ContentCategoryID = Guid.NewGuid();
				cc.CategoryText = dateNow.ToString("MMMM yyyy");
				cc.CategoryURL = "#" + SiteData.PreviewTemplateFilePage;

				lstContent.Add(cc);
				n++;
			}

			return lstContent;
		}

		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(rootContentID);
		}

		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, string sPage, bool bActiveOnly) {
			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sParentID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(Guid.NewGuid());
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {

			return SiteNavHelper.GetSamplerView(rootContentID);
		}

		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID) {

			return SiteNavHelper.GetSamplerView();
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid? ParentID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(ParentID);
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(PageID);
		}

		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(iUpdates);
		}

		public List<SiteNav> GetLatestPosts(Guid siteID, int iUpdates, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerFakeNav(iUpdates);
		}

		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav GetLatestVersion(Guid siteID, bool bActiveOnly, string sPage) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindHome(Guid siteID) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {

			return SiteNavHelper.GetSamplerView();
		}

		public SiteNav FindHome(Guid siteID, bool bActiveOnly) {

			return SiteNavHelper.GetSamplerView();
		}

		public int GetFilteredContentPagedCount(SiteData currentSite, string sFilterPath, bool bActiveOnly) {
			return 10;
		}
		public int GetFilteredContentByIDPagedCount(SiteData currentSite, List<Guid> lstCategories, bool bActiveOnly) {
			return 10;
		}

		public string GetBlogHeadingFromURL(SiteData currentSite, string sFilterPath) {
			string sTitle = String.Empty;

			if (sFilterPath.ToLower().StartsWith(currentSite.BlogCategoryPath.ToLower())) {
				sTitle = "Category 1";
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogTagPath.ToLower())) {
				sTitle = "Tag 1";
			}
			if (sFilterPath.ToLower().StartsWith(currentSite.BlogDateFolderPath.ToLower())) {
				sTitle = DateTime.Now.ToString("MMMM yyyy");
			}

			return sTitle;
		}

		public List<SiteNav> GetFilteredContentPagedList(SiteData currentSite, string sFilterPath, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}
		public List<SiteNav> GetFilteredContentByIDPagedList(SiteData currentSite, List<Guid> lstCategories, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestBlogPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(10);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageSize, int pageNumber) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public List<SiteNav> GetLatestContentPagedList(Guid siteID, ContentPageType.PageType postType, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

		public int GetSiteContentCount(Guid siteID) {
			return 10;
		}

		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType) {
			return 10;
		}

		public int GetSitePageCount(Guid siteID, ContentPageType.PageType entryType, bool bActiveOnly) {
			return 10;
		}

		public List<SiteNav> PerformDataPagingQueryableContent(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir, IQueryable<Data.vw_carrot_Content> QueryInput) {
			return SiteNavHelper.GetSamplerFakeNav(pageSize);
		}

	}

}
