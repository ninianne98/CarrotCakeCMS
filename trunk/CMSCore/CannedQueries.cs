using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
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
	static internal class CannedQueries {

		internal static IQueryable<vw_carrot_Content> GetAllByTypeList(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, ContentPageType.PageType entryType) {
			return (from ct in ctx.vw_carrot_Contents
					orderby ct.NavOrder, ct.NavMenuText
					where ct.SiteID == siteID
					&& ct.IsLatestVersion == true
					&& (ct.PageActive == true || bActiveOnly == false)
					&& (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
					&& (ct.RetireDate > DateTime.Now || bActiveOnly == false)
					&& ct.IsLatestVersion == true
					&& ct.ContentTypeID == ContentPageType.GetIDByType(entryType)
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetAllContentList(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.vw_carrot_Contents
					orderby ct.NavOrder, ct.NavMenuText
					where ct.SiteID == siteID
					 && ct.IsLatestVersion == true
					 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.ContentEntry)
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetAllBlogList(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.vw_carrot_Contents
					orderby ct.NavOrder, ct.NavMenuText
					where ct.SiteID == siteID
					 && ct.IsLatestVersion == true
					 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.BlogEntry)
					select ct);
		}

		internal static IQueryable<vw_carrot_ContentDateTally> GetBlogContentTallies(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.vw_carrot_ContentDateTallies
					orderby ct.DateMonth descending
					where ct.SiteID == siteID
					 && ct.IsLatestVersion == true
					 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.BlogEntry)
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetLatestContentList(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) {
			return (from ct in ctx.vw_carrot_Contents
					orderby ct.NavOrder, ct.NavMenuText
					where ct.SiteID == siteID
					 && ct.IsLatestVersion == true
					 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.ContentEntry)
					 && (ct.PageActive == true || bActiveOnly == false)
					 && (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
					 && (ct.RetireDate > DateTime.Now || bActiveOnly == false)
					select ct);
		}

		internal static IQueryable<carrot_WidgetData> GetWidgetDataByRootAll(CarrotCMSDataContext ctx, Guid rootWidgetID) {
			return (from r in ctx.carrot_WidgetDatas
					where r.Root_WidgetID == rootWidgetID
					select r);
		}

		internal static IQueryable<vw_carrot_Content> GetLatestBlogListDateRange(CarrotCMSDataContext ctx, Guid siteID, DateTime dateBegin, DateTime dateEnd, bool bActiveOnly) {
			return (from ct in ctx.vw_carrot_Contents
					where ct.SiteID == siteID
					 && ct.IsLatestVersion == true
					 && ct.CreateDate.Date >= dateBegin.Date
					 && ct.CreateDate.Date <= dateEnd.Date
					 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.BlogEntry)
					 && (ct.PageActive == true || bActiveOnly == false)
					 && (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
					 && (ct.RetireDate > DateTime.Now || bActiveOnly == false)
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetLatestBlogList(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) {
			return (from ct in ctx.vw_carrot_Contents
					orderby ct.NavOrder, ct.NavMenuText
					where ct.SiteID == siteID
					 && ct.IsLatestVersion == true
					 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.BlogEntry)
					 && (ct.PageActive == true || bActiveOnly == false)
					 && (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
					 && (ct.RetireDate > DateTime.Now || bActiveOnly == false)
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetContentByTagURL(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, string sTagURL) {
			return (from r in ctx.vw_carrot_TagURLs
					join m in ctx.carrot_TagContentMappings on r.ContentTagID equals m.ContentTagID
					join ct in ctx.vw_carrot_Contents on m.Root_ContentID equals ct.Root_ContentID
					where r.SiteID == siteID
						&& ct.SiteID == siteID
						&& r.TagUrl.ToLower() == sTagURL.ToLower()
						&& (ct.PageActive == true || bActiveOnly == false)
						&& (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
						&& (ct.RetireDate > DateTime.Now || bActiveOnly == false)
						&& ct.IsLatestVersion == true
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetContentByCategoryURL(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, string sCatURL) {
			return (from r in ctx.vw_carrot_CategoryURLs
					join m in ctx.carrot_CategoryContentMappings on r.ContentCategoryID equals m.ContentCategoryID
					join ct in ctx.vw_carrot_Contents on m.Root_ContentID equals ct.Root_ContentID
					where r.SiteID == siteID
						&& ct.SiteID == siteID
						&& r.CategoryUrl.ToLower() == sCatURL.ToLower()
						&& (ct.PageActive == true || bActiveOnly == false)
						&& (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
						&& (ct.RetireDate > DateTime.Now || bActiveOnly == false)
						&& ct.IsLatestVersion == true
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetContentByCategoryIDs(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, List<Guid> lstCategories) {
			return (from r in ctx.vw_carrot_CategoryURLs
					join m in ctx.carrot_CategoryContentMappings on r.ContentCategoryID equals m.ContentCategoryID
					join ct in ctx.vw_carrot_Contents on m.Root_ContentID equals ct.Root_ContentID
					where r.SiteID == siteID
						&& ct.SiteID == siteID
						&& lstCategories.Contains(r.ContentCategoryID)
						&& (ct.PageActive == true || bActiveOnly == false)
						&& (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
						&& (ct.RetireDate > DateTime.Now || bActiveOnly == false)
						&& ct.IsLatestVersion == true
					select ct);
		}

		internal static IQueryable<vw_carrot_Content> GetContentSiteSearch(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, string searchTerm) {
			return (from ct in ctx.vw_carrot_Contents
					where ct.SiteID == siteID
						&& (ct.PageText.Contains(searchTerm)
								|| ct.LeftPageText.Contains(searchTerm)
								|| ct.RightPageText.Contains(searchTerm)
								|| ct.TitleBar.Contains(searchTerm)
								|| ct.MetaDescription.Contains(searchTerm)
								|| ct.MetaKeyword.Contains(searchTerm)
							)
						&& (ct.PageActive == true || bActiveOnly == false)
						&& (ct.GoLiveDate < DateTime.Now || bActiveOnly == false)
						&& (ct.RetireDate > DateTime.Now || bActiveOnly == false)
						&& ct.IsLatestVersion == true
					select ct);
		}

		internal static IQueryable<carrot_CategoryContentMapping> GetContentCategoryMapByContentID(CarrotCMSDataContext ctx, Guid rootContentID) {
			return (from r in ctx.carrot_ContentCategories
					join c in ctx.carrot_CategoryContentMappings on r.ContentCategoryID equals c.ContentCategoryID
					where c.Root_ContentID == rootContentID
					select c);
		}

		internal static IQueryable<carrot_TagContentMapping> GetContentTagMapByContentID(CarrotCMSDataContext ctx, Guid rootContentID) {
			return (from r in ctx.carrot_ContentTags
					join c in ctx.carrot_TagContentMappings on r.ContentTagID equals c.ContentTagID
					where c.Root_ContentID == rootContentID
					select c);
		}

		internal static IQueryable<carrot_Content> GetBlogAllContentTbl(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.carrot_RootContents
					join c in ctx.carrot_Contents on ct.Root_ContentID equals c.Root_ContentID
					where ct.SiteID == siteID
					where c.IsLatestVersion == true
						 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.BlogEntry)
					select c);
		}

		internal static IQueryable<carrot_Content> GetContentAllContentTbl(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.carrot_RootContents
					join c in ctx.carrot_Contents on ct.Root_ContentID equals c.Root_ContentID
					where ct.SiteID == siteID
					where c.IsLatestVersion == true
						 && ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.ContentEntry)
					select c);
		}

		internal static IQueryable<carrot_Content> GetContentTopContentTbl(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.carrot_RootContents
					join c in ctx.carrot_Contents on ct.Root_ContentID equals c.Root_ContentID
					where ct.SiteID == siteID
					where c.IsLatestVersion == true
						&& c.Parent_ContentID == null
						&& ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.ContentEntry)
					select c);
		}

		internal static IQueryable<carrot_Content> GetContentSubContentTbl(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.carrot_RootContents
					join c in ctx.carrot_Contents on ct.Root_ContentID equals c.Root_ContentID
					where ct.SiteID == siteID
					where c.IsLatestVersion == true
						&& c.Parent_ContentID != null
						&& ct.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.ContentEntry)
					select c);
		}

		internal static IQueryable<vw_carrot_CategoryURL> GetCategoryURLs(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.vw_carrot_CategoryURLs
					where ct.SiteID == siteID
					select ct);
		}

		internal static IQueryable<vw_carrot_TagURL> GetTagURLs(CarrotCMSDataContext ctx, Guid siteID) {
			return (from ct in ctx.vw_carrot_TagURLs
					where ct.SiteID == siteID
					select ct);
		}

		internal static IQueryable<vw_carrot_CategoryURL> GetPostCategoryURL(CarrotCMSDataContext ctx, Guid siteID, string urlFileName) {
			return (from ct in ctx.vw_carrot_CategoryURLs
					join m in ctx.carrot_CategoryContentMappings on ct.ContentCategoryID equals m.ContentCategoryID
					join c in ctx.carrot_RootContents on m.Root_ContentID equals c.Root_ContentID
					where c.FileName.ToLower() == urlFileName.ToLower()
						&& ct.SiteID == siteID
					select ct);
		}

		internal static IQueryable<vw_carrot_TagURL> GetPostTagURLs(CarrotCMSDataContext ctx, Guid siteID, string urlFileName) {
			return (from ct in ctx.vw_carrot_TagURLs
					join m in ctx.carrot_TagContentMappings on ct.ContentTagID equals m.ContentTagID
					join c in ctx.carrot_RootContents on m.Root_ContentID equals c.Root_ContentID
					where c.FileName.ToLower() == urlFileName.ToLower()
						&& ct.SiteID == siteID
					select ct);
		}

		internal static IQueryable<vw_carrot_CategoryURL> GetPostCategoryURL(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID) {
			return (from ct in ctx.vw_carrot_CategoryURLs
					join m in ctx.carrot_CategoryContentMappings on ct.ContentCategoryID equals m.ContentCategoryID
					where m.Root_ContentID == rootContentID
						&& ct.SiteID == siteID
					select ct);
		}

		internal static IQueryable<vw_carrot_TagURL> GetPostTagURLs(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID) {
			return (from ct in ctx.vw_carrot_TagURLs
					join m in ctx.carrot_TagContentMappings on ct.ContentTagID equals m.ContentTagID
					where m.Root_ContentID == rootContentID
						&& ct.SiteID == siteID
					select ct);
		}

		internal static IQueryable<carrot_ContentComment> GetSiteContentComments(CarrotCMSDataContext ctx, Guid siteID) {
			return (from r in ctx.carrot_ContentComments
					join c in ctx.carrot_RootContents on r.Root_ContentID equals c.Root_ContentID
					orderby r.CreateDate descending
					where c.SiteID == siteID
					select r);
		}

		internal static IQueryable<carrot_ContentComment> GetContentPageComments(CarrotCMSDataContext ctx, Guid rootContentID, bool bActiveOnly) {
			return (from r in ctx.carrot_ContentComments
					orderby r.CreateDate descending
					where r.Root_ContentID == rootContentID
					&& (r.IsApproved == true || bActiveOnly == false)
					select r);
		}

		internal static IQueryable<carrot_ContentComment> GetSiteContentCommentsByPostType(CarrotCMSDataContext ctx, Guid siteID, ContentPageType.PageType contentEntry) {
			return (from r in ctx.carrot_ContentComments
					join c in ctx.carrot_RootContents on r.Root_ContentID equals c.Root_ContentID
					orderby r.CreateDate descending
					where c.SiteID == siteID
					 && c.ContentTypeID == ContentPageType.GetIDByType(contentEntry)
					select r);
		}


	}
}
