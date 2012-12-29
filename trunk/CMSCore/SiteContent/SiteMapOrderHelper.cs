﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Data;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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

	public class SiteMapOrderHelper : IDisposable {
		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public SiteMapOrderHelper() { }


		public List<SiteMapOrder> CreateSiteMapList(string sMapText) {
			List<SiteMapOrder> m = new List<SiteMapOrder>();
			sMapText = sMapText.Trim();

			if (!string.IsNullOrEmpty(sMapText)) {
				sMapText = sMapText.Replace("\r\n", "\n");
				var rows = sMapText.Split('\n');
				foreach (string r in rows) {
					if (!string.IsNullOrEmpty(r)) {
						var rr = r.Split('\t');
						SiteMapOrder s = new SiteMapOrder();
						s.NavOrder = int.Parse(rr[0]);
						s.Parent_ContentID = new Guid(rr[1]);
						s.Root_ContentID = new Guid(rr[2]);
						if (s.Parent_ContentID == Guid.Empty) {
							s.Parent_ContentID = null;
						}
						m.Add(s);
					}
				}
			}

			return m;
		}

		public List<SiteMapOrder> ParseChildPageData(string sMapText, Guid contentID) {
			List<SiteMapOrder> m = new List<SiteMapOrder>();
			sMapText = sMapText.Trim();

			carrot_Content c = (from ct in db.carrot_Contents
								where ct.Root_ContentID == contentID
								   && ct.IsLatestVersion == true
								select ct).FirstOrDefault();

			int iOrder = Convert.ToInt32(c.NavOrder) + 2;

			if (!string.IsNullOrEmpty(sMapText)) {
				sMapText = sMapText.Replace("\r\n", "\n");
				var rows = sMapText.Split('\n');
				foreach (string r in rows) {
					if (!string.IsNullOrEmpty(r)) {
						var rr = r.Split('\t');
						SiteMapOrder s = new SiteMapOrder();
						s.NavOrder = iOrder + int.Parse(rr[0]);
						s.Root_ContentID = new Guid(rr[1]);
						s.Parent_ContentID = contentID;
						m.Add(s);
					}
				}
			}

			return m;
		}

		public void UpdateSiteMap(Guid siteID, List<SiteMapOrder> oMap) {

			foreach (SiteMapOrder m in oMap) {

				carrot_Content c = (from ct in db.carrot_Contents
									join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
									where r.SiteID == siteID
										&& r.Root_ContentID == m.Root_ContentID
										&& ct.IsLatestVersion == true
									select ct).FirstOrDefault();

				c.Parent_ContentID = m.Parent_ContentID;
				c.NavOrder = (m.NavOrder * 10);
			}

			db.SubmitChanges();
		}

		public List<SiteMapOrder> GetChildPages(Guid siteID, Guid? parentID, Guid contentID) {

			List<vw_carrot_Content> lstOtherPages = CompiledQueries.cqGetOtherNotPage(db, siteID, contentID, parentID).ToList();

			if (lstOtherPages.Count < 1 && parentID == Guid.Empty) {
				lstOtherPages = CompiledQueries.TopLevelPages(db, siteID, false).ToList();
			}

			lstOtherPages.RemoveAll(x => x.Root_ContentID == contentID);
			lstOtherPages.RemoveAll(x => x.Parent_ContentID == contentID);

			List<SiteMapOrder> lst = (from ct in lstOtherPages
									  select new SiteMapOrder {
										  NavLevel = -1,
										  NavMenuText = (ct.PageActive ? "" : "{*U*} ") + ct.NavMenuText,
										  NavOrder = ct.NavOrder,
										  PageActive = Convert.ToBoolean(ct.PageActive),
										  Parent_ContentID = ct.Parent_ContentID,
										  Root_ContentID = ct.Root_ContentID
									  }).ToList();

			return lst;
		}

		public SiteMapOrder GetPageWithLevel(Guid siteID, Guid? contentID, int iLevel) {

			SiteMapOrder cont = (from ct in CompiledQueries.cqGetLatestContentPages(db, siteID, contentID).ToList()
								 select new SiteMapOrder {
									 NavLevel = iLevel,
									 NavMenuText = (ct.PageActive ? "" : "{*U*} ") + ct.NavMenuText,
									 NavOrder = ct.NavOrder,
									 PageActive = Convert.ToBoolean(ct.PageActive),
									 Parent_ContentID = ct.Parent_ContentID,
									 Root_ContentID = ct.Root_ContentID
								 }).FirstOrDefault();

			return cont;
		}


		public List<SiteMapOrder> GetAdminPageList(Guid siteID, Guid contentID) {

			List<SiteMapOrder> lstSite = (from ct in CompiledQueries.ContentNavAll(db, siteID, false).ToList()
										  select new SiteMapOrder {
											  NavLevel = -1,
											  NavMenuText = ct.NavMenuText,
											  NavOrder = ct.NavOrder,
											  PageActive = ct.PageActive,
											  Parent_ContentID = ct.Parent_ContentID,
											  Root_ContentID = ct.Root_ContentID
										  }).ToList();

			List<SiteMapOrder> lstSiteMap = new List<SiteMapOrder>();
			int iLevel = 0;
			int iBefore = 0;
			int iAfter = -10;
			int iLvlCounter = 0;
			int iPageCt = lstSite.Count;

			lstSiteMap = (from c in lstSite
						  orderby c.NavOrder, c.NavMenuText
						  where c.Parent_ContentID == null
						   && (c.Root_ContentID != contentID || contentID == Guid.Empty)
						  select new SiteMapOrder {
							  NavLevel = iLevel,
							  NavMenuText = c.NavMenuText,
							  NavOrder = (iLvlCounter++) * iPageCt,
							  PageActive = c.PageActive,
							  Parent_ContentID = c.Parent_ContentID,
							  Root_ContentID = c.Root_ContentID
						  }).ToList();


			while (iBefore != iAfter) {
				List<SiteMapOrder> lstLevel = (from z in lstSiteMap
											   where z.NavLevel == iLevel
											   select z).ToList();

				iBefore = lstSiteMap.Count;
				iLevel++;

				iLvlCounter = 0;

				List<SiteMapOrder> lstChild = (from s in lstSite
											   join l in lstLevel on s.Parent_ContentID equals l.Root_ContentID
											   orderby s.NavOrder, s.NavMenuText
											   where (s.Root_ContentID != contentID || contentID == Guid.Empty)
											   select new SiteMapOrder {
												   NavLevel = iLevel,
												   NavMenuText = l.NavMenuText + " > " + s.NavMenuText,
												   NavOrder = l.NavOrder + (iLvlCounter++)
													   + (from s2 in lstSite
														  join l2 in lstLevel on s2.Parent_ContentID equals l2.Root_ContentID
														  where s.Parent_ContentID == s2.Parent_ContentID
																 && s.Root_ContentID != s2.Root_ContentID
														  select s.Root_ContentID).ToList().Count,
												   PageActive = s.PageActive,
												   Parent_ContentID = s.Parent_ContentID,
												   Root_ContentID = s.Root_ContentID
											   }).ToList();

				lstSiteMap = (from m in lstSiteMap.Union(lstChild).ToList()
							  orderby m.NavOrder, m.NavMenuText
							  select m).ToList();

				iAfter = lstSiteMap.Count;
			}

			return (from m in lstSiteMap
					orderby m.NavOrder, m.NavMenuText
					select m).ToList();
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
