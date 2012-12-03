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
	public static class CompiledQueries {

		//internal static CarrotCMSDataContext dbConn = new CarrotCMSDataContext();

		internal static readonly Func<CarrotCMSDataContext, Guid, Guid, carrot_RootContent> cqGetRootContentTbl =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID) =>
					  (from r in ctx.carrot_RootContents
					   where r.SiteID == siteID
						   && r.Root_ContentID == rootContentID
					   select r).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid, carrot_Content> cqGetLatestContentTbl =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID) =>
					  (from ct in ctx.carrot_Contents
					   join r in ctx.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
					   where r.SiteID == siteID
						   && r.Root_ContentID == rootContentID
						   && ct.IsLatestVersion == true
					   select ct).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid, IQueryable<vw_carrot_Content>> cqGetVersionHistory =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.EditDate descending
					 where ct.SiteID == siteID
						&& ct.Root_ContentID == rootContentID
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid, vw_carrot_Content> cqGetContentByContentID =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, Guid contentID) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.EditDate descending
					 where ct.SiteID == siteID
						&& ct.ContentID == contentID
					 select ct).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, IQueryable<vw_carrot_Content>> cqTopLevelPages =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder, ct.NavMenuText
					 where ct.SiteID == siteID
						 && ct.Parent_ContentID == null
						 && ct.IsLatestVersion == true
						 && (ct.PageActive == true || bActiveOnly == false)
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, string, vw_carrot_Content> cqGetLatestContentByURL =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, string sPage) =>
					  (from ct in ctx.vw_carrot_Contents
					   where ct.SiteID == siteID
							&& ct.FileName.ToLower() == sPage.ToLower()
							&& ct.IsLatestVersion == true
							&& (ct.PageActive == true || bActiveOnly == false)
					   select ct).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, Guid, vw_carrot_Content> cqGetLatestContentByID =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, Guid rootContentID) =>
					(from ct in ctx.vw_carrot_Contents
					 where ct.SiteID == siteID
						 && ct.Root_ContentID == rootContentID
						 && ct.IsLatestVersion == true
						 && (ct.PageActive == true || bActiveOnly == false)
					 select ct).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid?, bool, IQueryable<vw_carrot_Content>> cqGetLatestContentByParent =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, Guid? parentContentID, bool bActiveOnly) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder, ct.NavMenuText
					 where ct.SiteID == siteID
						 && ct.Parent_ContentID == parentContentID
						 && ct.IsLatestVersion == true
						 && (ct.PageActive == true || bActiveOnly == false)
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid?, bool, IQueryable<vw_carrot_Content>> cqGetLatestContentWithParent =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, Guid? parentContentID, bool bActiveOnly) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder, ct.NavMenuText
					 where ct.SiteID == siteID
						 && (ct.Parent_ContentID == parentContentID || ct.Root_ContentID == parentContentID)
						 && ct.IsLatestVersion == true
						 && (ct.PageActive == true || bActiveOnly == false)
					 select ct));

		internal static readonly Func<CarrotCMSDataContext, Guid, bool, IQueryable<vw_carrot_Content>> cqSiteNavAll =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) =>
					  (from ct in ctx.vw_carrot_Contents
					   orderby ct.NavOrder, ct.NavMenuText
					   where ct.SiteID == siteID
							 && ct.IsLatestVersion == true
							 && (ct.PageActive == true || bActiveOnly == false)
					   select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, vw_carrot_Content> cqFindHome =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder ascending
					 where ct.SiteID == siteID
							&& ct.NavOrder < 1
							&& ct.IsLatestVersion == true
							&& (ct.PageActive == true || bActiveOnly == false)
					 select ct).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, IQueryable<vw_carrot_Content>> cqGetLatestContentList =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) =>
					  (from ct in ctx.vw_carrot_Contents
					   orderby ct.NavOrder, ct.NavMenuText
					   where ct.SiteID == siteID
						&& ct.IsLatestVersion == true
						&& (ct.PageActive == true || bActiveOnly == false)
					   select ct));


		//===============================

		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<carrot_Widget>> cqGetOldEditContentWidgets =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootContentID) =>
				(from r in ctx.carrot_Widgets
				 where r.Root_ContentID == rootContentID
				 && (r.ControlPath.ToLower().Contains("/manage/ucgenericcontent.ascx")
						|| r.ControlPath.ToLower().Contains("/manage/uctextcontent.ascx"))
				 select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, carrot_Widget> cqGetRootWidget =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.carrot_Widgets
				 where r.Root_WidgetID == rootWidgetID
				 select r).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, vw_carrot_Widget> cqGetLatestWidget =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.vw_carrot_Widgets
				 where r.Root_WidgetID == rootWidgetID
					&& r.IsLatestVersion == true
				 select r).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, vw_carrot_Widget> cqGetWidgetDataByID_VW =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid widgetDataID) =>
				(from r in ctx.vw_carrot_Widgets
				 where r.WidgetDataID == widgetDataID
				 select r).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, carrot_WidgetData> cqGetWidgetDataByID_TBL =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid widgetDataID) =>
				(from r in ctx.carrot_WidgetDatas
				 where r.WidgetDataID == widgetDataID
				 select r).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, carrot_WidgetData> cqGetWidgetDataByRootID =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.carrot_WidgetDatas
				 where r.Root_WidgetID == rootWidgetID
					&& r.IsLatestVersion == true
				 select r).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<carrot_WidgetData>> cqGetWidgetDataByRootAll =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.carrot_WidgetDatas
				 where r.Root_WidgetID == rootWidgetID
				 select r));

		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<vw_carrot_Widget>> cqGetWidgetVersionHistory_VW =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
					  (from r in ctx.vw_carrot_Widgets
					   orderby r.EditDate descending
					   where r.Root_WidgetID == rootWidgetID
					   select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, IQueryable<vw_carrot_Widget>> cqGetLatestWidgets =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid rootContentID, bool bActiveOnly) =>
					  (from r in ctx.vw_carrot_Widgets
					   orderby r.WidgetOrder
					   where r.Root_ContentID == rootContentID
						  && r.IsLatestVersion == true
						  && (r.WidgetActive == true || bActiveOnly == false)
					   select r));


		//internal static readonly Func<CarrotCMSDataContext, Guid, bool, List<Guid>, IQueryable<vw_carrot_Content>> cqSecondLevel =
		//CompiledQuery.Compile(
		//            (CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, List<Guid> lstTop) =>
		//              (from ct in ctx.vw_carrot_Contents
		//               orderby ct.NavOrder, ct.NavMenuText
		//               where ct.SiteID == siteID
		//                     && (ct.PageActive == true || bActiveOnly == false)
		//                     && ct.IsLatestVersion == true
		//                     && (lstTop.Contains(ct.Root_ContentID) || lstTop.Contains(ct.Parent_ContentID.Value))
		//               select ct));


		//=================================

		//internal static readonly Func<CarrotCMSDataContext, Guid, Guid, Guid, string, carrot_SerialCache> cqGetSerialCacheTbl123 =
		//CompiledQuery.Compile(
		//    (CarrotCMSDataContext ctx, Guid siteID, Guid userID, Guid itemID, string sKey) =>
		//        (from c in ctx.carrot_SerialCaches
		//         where c.ItemID == itemID
		//             && c.KeyType == sKey
		//             && c.SiteID == siteID
		//             && c.EditUserId == userID
		//         select c).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, SearchParameterObject, carrot_SerialCache> cqGetSerialCacheTbl =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, SearchParameterObject searchParm) =>
			(from c in ctx.carrot_SerialCaches
			 where c.ItemID == searchParm.ItemID
					&& c.KeyType == searchParm.KeyType
					&& c.SiteID == searchParm.SiteID
					&& c.EditUserId == searchParm.UserId
			 select c).FirstOrDefault());


		internal static carrot_SerialCache SearchSeriaCache(CarrotCMSDataContext ctx, Guid siteID, Guid userID, Guid itemID, string keyType) {
			SearchParameterObject searchParm = new SearchParameterObject();
			searchParm.UserId = userID;
			searchParm.SiteID = siteID;
			searchParm.ItemID = itemID;
			searchParm.KeyType = keyType;

			return cqGetSerialCacheTbl(ctx, searchParm);
		}

		internal static carrot_SerialCache SearchSeriaCache(CarrotCMSDataContext ctx, Guid itemID, string keyType) {

			return SearchSeriaCache(ctx, SiteData.CurrentSiteID, SecurityData.CurrentUserGuid, itemID, keyType);
		}


		//=====================

		internal static readonly Func<CarrotCMSDataContext, Guid, carrot_Site> cqGetSiteByID =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid siteID) =>
				(from r in ctx.carrot_Sites
				 where r.SiteID == siteID
				 select r).FirstOrDefault());




		//=====================

		internal static readonly Func<CarrotCMSDataContext, Guid, Guid, Guid?, IQueryable<vw_carrot_Content>> cqGetOtherNotPage =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID, Guid? parentContentID) =>
					  (from r in ctx.vw_carrot_Contents
					   orderby r.NavOrder, r.NavMenuText
					   where r.SiteID == siteID
							&& r.IsLatestVersion == true
							&& r.Root_ContentID != rootContentID
							&& (r.Parent_ContentID == parentContentID
								 || (r.Parent_ContentID == null && parentContentID == Guid.Empty))
					   select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid?, IQueryable<vw_carrot_Content>> cqGetLatestPage =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, Guid? rootContentID) =>
					  (from r in ctx.vw_carrot_Contents
					   orderby r.NavOrder, r.NavMenuText
					   where r.SiteID == siteID
						   && r.IsLatestVersion == true
						   && r.Root_ContentID == rootContentID
					   select r));


	}


	internal class SearchParameterObject {
		public Guid ItemID { get; set; }
		public Guid SiteID { get; set; }
		public Guid UserId { get; set; }
		public string KeyType { get; set; }

		public Guid ParentContentID { get; set; }
		public bool ActiveOnly { get; set; }
		public Guid RootContentID { get; set; }
		public string FileName { get; set; }
		public Guid ContentID { get; set; }
	}

}
