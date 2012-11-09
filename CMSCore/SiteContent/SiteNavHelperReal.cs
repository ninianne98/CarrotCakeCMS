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


		public SiteNavHelperReal() {
			//#if DEBUG
			//            db.Log = new DebugTextWriter();
			//#endif
		}

		/*
		internal static SiteNav MakeSiteNav(carrot_RootContent rc, carrot_Content c) {
			var nav = new SiteNav();

			if (rc == null) {
				rc = new carrot_RootContent();
				rc.Root_ContentID = Guid.NewGuid();
				rc.PageActive = true;
			}
			if (c == null) {
				c = new carrot_Content();
				c.ContentID = rc.Root_ContentID;
				c.Root_ContentID = rc.Root_ContentID;
			}

			if (c.Root_ContentID == rc.Root_ContentID) {

				nav.Root_ContentID = rc.Root_ContentID;
				nav.SiteID = rc.SiteID;
				nav.FileName = rc.FileName;
				nav.PageActive = rc.PageActive;
				nav.CreateDate = rc.CreateDate;

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
		*/


		internal static SiteNav MakeSiteNav(vw_carrot_Content c) {
			SiteNav nav = null;

			if (c != null) {
				nav = new SiteNav();

				nav.Root_ContentID = c.Root_ContentID;
				nav.SiteID = c.SiteID;
				nav.FileName = c.FileName;
				nav.PageActive = c.PageActive;
				nav.CreateDate = c.CreateDate;
				nav.EditUserId = c.EditUserId;

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
			List<SiteNav> lstContent = (from ct in CompiledQueries.cqSiteNavAll(db, siteID, bActiveOnly)
										select MakeSiteNav(ct)).ToList();

			return lstContent;
		}



		public List<SiteNav> GetTwoLevelNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = null;

			List<Guid> lstTop = CompiledQueries.cqTopLevelPages(db, siteID, false).Select(z => z.Root_ContentID).ToList();

			//IQueryable<vw_carrot_Content> lstSecondLevel = CompiledQueries.cqSecondLevel(db, siteID, bActiveOnly, lstTop);

			//lstContent = (from ct in lstSecondLevel
			//              select MakeSiteNav(ct)).ToList();

			lstContent = (from ct in CompiledQueries.cqSiteNavAll(db, siteID, bActiveOnly)
						  orderby ct.NavOrder, ct.NavMenuText
						  where ct.SiteID == siteID
								&& (ct.PageActive == bActiveOnly || bActiveOnly == false)
								&& ct.IsLatestVersion == true
								&& (lstTop.Contains(ct.Root_ContentID) || lstTop.Contains(ct.Parent_ContentID.Value))
						  select MakeSiteNav(ct)).ToList();

			//lstContent = (from ct in db.vw_carrot_Contents
			//              orderby ct.NavOrder, ct.NavMenuText
			//              where ct.SiteID == siteID
			//                    && (ct.PageActive == bActiveOnly || bActiveOnly == false)
			//                    && ct.IsLatestVersion == true
			//                    && (lstTop.Contains(ct.Root_ContentID) || lstTop.Contains(ct.Parent_ContentID.Value))
			//              select MakeSiteNav(ct)).ToList();


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

				lstSub = (from ct in CompiledQueries.cqSiteNavAll(db, siteID, bActiveOnly)
						  where ct.SiteID == siteID
								&& ct.IsLatestVersion == true
								&& (ct.PageActive == bActiveOnly || bActiveOnly == false)
								&& (!lstTop.Contains(ct.Root_ContentID) && lstTop.Contains(ct.Parent_ContentID.Value))
						  select ct.Root_ContentID).Distinct().ToList();

				lstTop = lstTop.Union(lstSub).ToList();

				iDepth--;
			}

			lstContent = (from ct in CompiledQueries.cqSiteNavAll(db, siteID, bActiveOnly)
						  orderby ct.NavOrder, ct.NavMenuText
						  where ct.SiteID == siteID
								&& (ct.PageActive == bActiveOnly || bActiveOnly == false)
								&& ct.IsLatestVersion == true
								&& lstTop.Contains(ct.Root_ContentID)
						  select MakeSiteNav(ct)).ToList();

			return lstContent;
		}


		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = CompiledQueries.cqTopLevelPages(db, siteID, bActiveOnly).Select(ct => MakeSiteNav(ct)).ToList();

			return lstContent;
		}

		private SiteNav GetPageNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			SiteNav content = MakeSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, bActiveOnly, rootContentID));

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

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid ParentID, bool bActiveOnly) {

			List<SiteNav> lstContent = (from ct in CompiledQueries.cqGetLatestContentByParent(db, siteID, ParentID, bActiveOnly).ToList()
										select MakeSiteNav(ct)).ToList();

			return lstContent;
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {

			SiteNav content = MakeSiteNav(CompiledQueries.cqGetLatestContentByURL(db, siteID, false, sPage));

			return content;
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {

			SiteNav content = MakeSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, false, rootContentID));

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

			SiteNav content = null;
			if (nav1 != null && nav1.Parent_ContentID.HasValue) {
				content = MakeSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, false, nav1.Parent_ContentID.Value));
			}

			return content;
		}


		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {

			vw_carrot_Content c = CompiledQueries.cqGetLatestContentByID(db, siteID, false, PageID);

			List<SiteNav> lstContent = new List<SiteNav>();

			if (c != null) {
				lstContent = (from ct in CompiledQueries.cqGetLatestContentByParent(db, siteID, c.Parent_ContentID, bActiveOnly).ToList()
							  select MakeSiteNav(ct)).ToList();
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
			List<SiteNav> lstContent = (from ct in CompiledQueries.cqSiteNavAll(db, siteID, bActiveOnly).ToList()
										select MakeSiteNav(ct)).Take(iUpdates).ToList();

			return lstContent;
		}


		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {
			SiteNav content = MakeSiteNav(CompiledQueries.cqGetLatestContentByID(db, siteID, false, rootContentID));

			return content;
		}

		public SiteNav GetLatestVersion(Guid siteID, bool bActiveOnly, string sPage) {
			SiteNav content = MakeSiteNav(CompiledQueries.cqGetLatestContentByURL(db, siteID, bActiveOnly, sPage));

			return content;
		}

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {
			SiteNav content = MakeSiteNav(CompiledQueries.cqGetLatestContentByURL(db, siteID, false, urlFileName));

			return content;
		}

		public SiteNav FindHome(Guid siteID) {
			SiteNav content = MakeSiteNav(CompiledQueries.cqFindHome(db, siteID, true));

			return content;
		}

		public SiteNav FindHome(Guid siteID, bool bActiveOnly) {
			SiteNav content = MakeSiteNav(CompiledQueries.cqFindHome(db, siteID, bActiveOnly));

			return content;
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
