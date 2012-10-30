using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Carrotware.CMS.Data;

namespace Carrotware.CMS.Core {
	public static class CompiledQueries {

		internal static readonly Func<CarrotCMSDataContext, Guid, Guid, carrot_RootContent> cqGetRootContent =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID) =>
					  (from r in ctx.carrot_RootContents
					   where r.SiteID == siteID
						   && r.Root_ContentID == rootContentID
					   select r).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid, carrot_Content> cqGetLatestContent =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, Guid rootContentID) =>
					  (from ct in ctx.carrot_Contents
					   join r in ctx.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
					   where r.SiteID == siteID
						   && ct.Root_ContentID == rootContentID
						   && ct.IsLatestVersion == true
					   select ct).FirstOrDefault());


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<vw_carrot_Content>> cqTopLevelNavigationAll =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder, ct.NavMenuText
					 where ct.SiteID == siteID
						 && ct.Parent_ContentID == null
						 && ct.IsLatestVersion == true
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, IQueryable<vw_carrot_Content>> cqTopLevelNavigationAllActive =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder, ct.NavMenuText
					 where ct.SiteID == siteID
						 && ct.Parent_ContentID == null
						 && ct.IsLatestVersion == true
						 && (ct.PageActive == bActiveOnly || bActiveOnly == false)
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool?, string, IQueryable<vw_carrot_Content>> cqGetLatestContentByURL =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool? active, string sPage) =>
					  (from ct in ctx.vw_carrot_Contents
					   where ct.SiteID == siteID
							&& ct.FileName.ToLower() == sPage.ToLower()
							&& ct.IsLatestVersion == true
							&& (ct.PageActive == active || active == null)
					   select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool?, Guid?, IQueryable<vw_carrot_Content>> cqGetLatestContentByID =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool? active, Guid? rootContentID) =>
					(from ct in ctx.vw_carrot_Contents
					 where ct.SiteID == siteID
					 && ct.Root_ContentID == rootContentID
					 && ct.IsLatestVersion == true
					 && (ct.PageActive == active || active == null)
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid?, bool, IQueryable<vw_carrot_Content>> cqGetLatestActiveContent =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, Guid? rootContentID, bool bActiveOnly) =>
					(from ct in ctx.vw_carrot_Contents
					 where ct.SiteID == siteID
						 && ct.Root_ContentID == rootContentID
						 && ct.IsLatestVersion == true
						 && (ct.PageActive == bActiveOnly || bActiveOnly == false)
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, Guid?, bool, IQueryable<vw_carrot_Content>> cqGetLatestActiveParentContent =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, Guid? ParentID, bool bActiveOnly) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder, ct.NavMenuText
					 where ct.SiteID == siteID
						 && ct.Parent_ContentID == ParentID
						 && ct.IsLatestVersion == true
						 && (ct.PageActive == bActiveOnly || bActiveOnly == false)
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, IQueryable<vw_carrot_Content>> cqSiteNavAll =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) =>
					  (from ct in ctx.vw_carrot_Contents
					   orderby ct.NavOrder, ct.NavMenuText
					   where ct.SiteID == siteID
							 && ct.IsLatestVersion == true
							 && (ct.PageActive == bActiveOnly || bActiveOnly == false)
					   select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool?, IQueryable<vw_carrot_Content>> cqFindHome =
		CompiledQuery.Compile(
				(CarrotCMSDataContext ctx, Guid siteID, bool? active) =>
					(from ct in ctx.vw_carrot_Contents
					 orderby ct.NavOrder ascending
					 where ct.SiteID == siteID
							&& ct.NavOrder < 1
							&& ct.IsLatestVersion == true
							&& (ct.PageActive == active || active == null)
					 select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool?, IQueryable<vw_carrot_Content>> cqGetLatestContentListA =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool? active) =>
					  (from ct in ctx.vw_carrot_Contents
					   orderby ct.NavOrder, ct.NavMenuText
					   where ct.SiteID == siteID
						&& ct.IsLatestVersion == true
						&& (ct.PageActive == active || active == null)
					   select ct));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool, IQueryable<vw_carrot_Content>> cqGetLatestContentListB =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly) =>
					  (from c in ctx.vw_carrot_Contents
					   where c.SiteID == siteID
						  && c.IsLatestVersion == true
						  && (c.PageActive == bActiveOnly || bActiveOnly == false)
					   select c));


		//===============================


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<carrot_Widget>> cqGetRootWidget =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.carrot_Widgets
				 where r.Root_WidgetID == rootWidgetID
				 select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<vw_carrot_Widget>> cqGetLatestWidget =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.vw_carrot_Widgets
				 where r.Root_WidgetID == rootWidgetID
					&& r.IsLatestVersion == true
				 select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<vw_carrot_Widget>> cqGetWidgetData =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid widgetDataID) =>
				(from r in ctx.vw_carrot_Widgets
				 where r.WidgetDataID == widgetDataID
				 select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<carrot_WidgetData>> cqGetWidgetDataByID =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid widgetDataID) =>
				(from r in ctx.carrot_WidgetDatas
				 where r.WidgetDataID == widgetDataID
				 select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<carrot_WidgetData>> cqGetWidgetDataByRootID =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.carrot_WidgetDatas
				 where r.Root_WidgetID == rootWidgetID
					&& r.IsLatestVersion == true
				 select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<carrot_WidgetData>> cqGetWidgetDataByRootAll =
		CompiledQuery.Compile(
			(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
				(from r in ctx.carrot_WidgetDatas
				 where r.Root_WidgetID == rootWidgetID
				 select r));

		internal static readonly Func<CarrotCMSDataContext, Guid, IQueryable<vw_carrot_Widget>> cqGetWidgetVersionHistory =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid rootWidgetID) =>
					  (from r in ctx.vw_carrot_Widgets
					   orderby r.EditDate descending
					   where r.Root_WidgetID == rootWidgetID
					   select r));


		internal static readonly Func<CarrotCMSDataContext, Guid, bool?, IQueryable<vw_carrot_Widget>> cqGetWidgets =
		CompiledQuery.Compile(
					(CarrotCMSDataContext ctx, Guid rootContentID, bool? active) =>
					  (from r in ctx.vw_carrot_Widgets
					   orderby r.WidgetOrder
					   where r.Root_ContentID == rootContentID
						  && r.IsLatestVersion == true
						  && (r.WidgetActive == active || active == null)
					   select r));


		//internal static readonly Func<CarrotCMSDataContext, Guid, bool, List<Guid>, IQueryable<vw_carrot_Content>> cqSecondLevel =
		//CompiledQuery.Compile(
		//            (CarrotCMSDataContext ctx, Guid siteID, bool bActiveOnly, List<Guid> lstTop) =>
		//              (from ct in ctx.vw_carrot_Contents
		//               orderby ct.NavOrder, ct.NavMenuText
		//               where ct.SiteID == siteID
		//                     && (ct.PageActive == bActiveOnly || bActiveOnly == false)
		//                     && ct.IsLatestVersion == true
		//                     && (lstTop.Contains(ct.Root_ContentID) || lstTop.Contains(ct.Parent_ContentID.Value))
		//               select ct));
	}
}
