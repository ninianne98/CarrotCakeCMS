using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Carrotware.CMS.Data;
using System.Reflection;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class ContentPageHelper : IDisposable {


		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public ContentPageHelper() {

		}

		public static string ScrubFilename(Guid rootContentID, string FileName) {
			string newFileName = FileName;

			if (string.IsNullOrEmpty(newFileName)) {
				newFileName = rootContentID.ToString().Substring(0, 6) + "-" + rootContentID.ToString().Substring(28, 6);
			}

			newFileName = newFileName.Trim().Replace(" ", "_");
			newFileName = newFileName.Trim().Replace("'", "-");
			newFileName = newFileName.Trim().Replace("\"", "-");
			newFileName = newFileName.Trim().Replace("&", "-n-");

			if (!newFileName.ToLower().EndsWith(".aspx")) {
				newFileName = newFileName + ".aspx";
			}
			if (newFileName.ToLower().IndexOf(@"/") < 0) {
				newFileName = "/" + newFileName;
			}

			return newFileName;
		}

		public List<ContentPage> GetLatestContentList(Guid siteID) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
						 && ct.IsLatestVersion == true
						select new ContentPage(r, ct)).ToList();
			return oldC;
		}


		public int GetContentPagedListCount(Guid siteID, bool bActiveOnly) {
			int iCount = (from ct in db.tblContents
						  join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						  where r.SiteID == siteID
							  && ct.IsLatestVersion == true
							  && (r.PageActive == bActiveOnly || bActiveOnly == false)
						  select r).Count();
			return iCount;
		}


		public List<ContentPage> GetLatestContentPagedList(Guid siteID, bool bActiveOnly, int pageNumber, string sortField, string sortDir) {
			return GetLatestContentPagedList(siteID, bActiveOnly, 10, pageNumber, sortField, sortDir);
		}

		public List<ContentPage> GetLatestContentPagedList(Guid siteID, bool bActiveOnly, int pageNumber) {
			return GetLatestContentPagedList(siteID, bActiveOnly, 10, pageNumber, "", "");
		}

		public List<ContentPage> GetLatestContentPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber) {
			return GetLatestContentPagedList(siteID, bActiveOnly, pageSize, pageNumber, "", "");
		}

		public void ResetHeartbeatLock(Guid rootContentID, Guid siteID) {

			var rc = (from r in db.tblRootContents
					  where r.Root_ContentID == rootContentID
						&& r.SiteID == siteID
					  select r).FirstOrDefault();

			rc.EditHeartbeat = DateTime.Now.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public bool RecordHeartbeatLock(Guid rootContentID, Guid siteID, Guid currentUserID) {

			var rc = (from r in db.tblRootContents
					 where r.Root_ContentID == rootContentID
					 && r.SiteID == siteID
					 select r).FirstOrDefault();

			if (rc != null) {
				rc.Heartbeat_UserId = currentUserID;
				rc.EditHeartbeat = DateTime.Now;
				db.SubmitChanges();
				return true;
			}

			return false;
		}

		public bool IsPageLocked(ContentPage cp) {

			bool bLock = false;
			if (cp.Heartbeat_UserId != null) {
				if (cp.Heartbeat_UserId != SiteData.CurrentUserGuid
						&& cp.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
					bLock = true;
				}
				if (cp.Heartbeat_UserId == SiteData.CurrentUserGuid
					|| cp.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public List<ContentPage> GetLatestContentPagedList(Guid siteID, bool bActiveOnly, int pageSize, int pageNumber, string sortField, string sortDir) {
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

			IEnumerable<ContentPage> query = new List<ContentPage>();

			bool bIsContent = TestIfPropExists(new tblContent(), sortField);
			bool bIsRootContent = TestIfPropExists(new tblRootContent(), sortField);

			if (bIsRootContent) {
				if (sortDir.ToUpper().Trim().IndexOf("ASC") < 0) {
					query = (from enu in
								 (from ct in db.tblContents.AsEnumerable()
								  join r in db.tblRootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
								  orderby GetPropertyValue(r, sortField) descending
								  where r.SiteID == siteID
									 && ct.IsLatestVersion == true
									 && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  select new ContentPage(r, ct)).AsQueryable()
							 select enu).AsQueryable().Skip(startRec).Take(pageSize);
				} else {
					query = (from enu in
								 (from ct in db.tblContents.AsEnumerable()
								  join r in db.tblRootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
								  orderby GetPropertyValue(r, sortField) ascending
								  where r.SiteID == siteID
									 && ct.IsLatestVersion == true
									 && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  select new ContentPage(r, ct)).AsQueryable()
							 select enu).AsQueryable().Skip(startRec).Take(pageSize);
				}
			}

			if (bIsContent && !bIsRootContent) {
				if (sortDir.ToUpper().Trim().IndexOf("ASC") < 0) {
					query = (from enu in
								 (from ct in db.tblContents.AsEnumerable()
								  join r in db.tblRootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
								  orderby GetPropertyValue(ct, sortField) descending
								  where r.SiteID == siteID
									 && ct.IsLatestVersion == true
									 && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  select new ContentPage(r, ct)).AsQueryable()
							 select enu).AsQueryable().Skip(startRec).Take(pageSize);
				} else {
					query = (from enu in
								 (from ct in db.tblContents.AsEnumerable()
								  join r in db.tblRootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
								  orderby GetPropertyValue(ct, sortField) ascending
								  where r.SiteID == siteID
									 && ct.IsLatestVersion == true
									 && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  select new ContentPage(r, ct)).AsQueryable()
							 select enu).AsQueryable().Skip(startRec).Take(pageSize);
				}
			}

			if (!bIsContent && !bIsRootContent) {
				query = (from enu in
							 (from ct in db.tblContents
							  join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
							  orderby r.CreateDate descending
							  where r.SiteID == siteID
								 && ct.IsLatestVersion == true
								 && (r.PageActive == bActiveOnly || bActiveOnly == false)
							  select new ContentPage(r, ct)).AsQueryable()
						 select enu).AsQueryable().Skip(startRec).Take(pageSize);
			}

			return query.ToList();
		}


		private object GetPropertyValue(object obj, string property) {
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo.GetValue(obj, null);
		}

		private bool TestIfPropExists(object obj, string property) {
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo == null ? false : true;
		}


		public List<ContentPage> GetVersionHistory(Guid siteID, Guid rootID) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.EditDate descending
						where r.SiteID == siteID
						 && r.Root_ContentID == rootID
						select new ContentPage(r, ct)).ToList();
			return oldC;
		}

		public ContentPage GetVersion(Guid siteID, Guid contentID) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.EditDate descending
						where r.SiteID == siteID
						 && ct.ContentID == contentID
						select new ContentPage(r, ct)).FirstOrDefault();
			return oldC;
		}


		public List<ContentPage> GetLatestContentList(Guid siteID, bool? active) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
						 && ct.IsLatestVersion == true
						 && (r.PageActive == active || active == null)
						select new ContentPage(r, ct)).ToList();
			return oldC;
		}

		public void RemoveVersions(Guid siteID, List<Guid> lstDel) {

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.EditDate descending
						where r.SiteID == siteID
						 && lstDel.Contains(ct.ContentID)
						 && ct.IsLatestVersion != true
						select ct).ToList();

			if (oldC.Count > 0) {
				foreach (var c in oldC) {
					db.tblContents.DeleteOnSubmit(c);
				}
				db.SubmitChanges();
			}
		}


		public void BulkUpdateTemplate(Guid siteID, List<Guid> lstUpd, string sTemplateFile) {

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
						 && lstUpd.Contains(r.Root_ContentID)
						 && ct.IsLatestVersion == true
						select ct).ToList();

			if (oldC.Count > 0) {
				foreach (var c in oldC) {
					c.TemplateFile = sTemplateFile;
					//c.EditDate = DateTime.Now;
				}
				db.SubmitChanges();
			}
		}



		public ContentPage GetLatestContent(Guid siteID, Guid rootContentID) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
							&& r.Root_ContentID == rootContentID
							&& ct.IsLatestVersion == true
						select new ContentPage(r, ct)).FirstOrDefault();

			return oldC;
		}

		public ContentPage GetLatestContent(Guid siteID, bool? active, string sPage) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
							&& (r.PageActive == active || active == null)
							&& r.FileName.ToLower() == sPage.ToLower()
							&& ct.IsLatestVersion == true
						select new ContentPage(r, ct)).FirstOrDefault();

			return oldC;
		}


		public ContentPage FindHome(Guid siteID) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder ascending
						where r.SiteID == siteID
							&& r.PageActive == true
							&& ct.NavOrder < 1
							&& ct.IsLatestVersion == true
						select new ContentPage(r, ct)).FirstOrDefault();
			return oldC;
		}

		public ContentPage FindByFilename(Guid siteID, string urlFileName) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
							&& ct.IsLatestVersion == true
							&& r.FileName.ToLower() == urlFileName.ToLower()
						select new ContentPage(r, ct)).FirstOrDefault();

			return oldC;
		}

		public ContentPage FindHome(Guid siteID, bool? active) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder ascending
						where r.SiteID == siteID
							&& (r.PageActive == active || active == null)
							&& ct.NavOrder < 1
							&& ct.IsLatestVersion == true
						select new ContentPage(r, ct)).FirstOrDefault();
			return oldC;
		}


		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}

	public class SiteNavHelper : IDisposable {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteNavHelper() { }

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
							&& (r.PageActive == bActiveOnly || bActiveOnly == false)
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).ToList();
			return oldC;
		}



		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
							&& ct.Parent_ContentID == null
							&& (r.PageActive == bActiveOnly || bActiveOnly == false)
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).ToList();
			return oldC;
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sParentID, bool bActiveOnly) {

			var p = (from r in db.tblRootContents
					 where r.SiteID == siteID
							&& r.FileName.ToLower() == sParentID.ToLower()
					 select r).FirstOrDefault();

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
							&& ct.Parent_ContentID == p.Root_ContentID
							&& (r.PageActive == bActiveOnly || bActiveOnly == false)
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).ToList();

			return oldC;
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {

			var c = (from ct in db.tblContents
					 join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
					 where r.SiteID == siteID
							&& r.FileName.ToLower() == sPage.ToLower()
							&& ct.IsLatestVersion == true
					 select ct).FirstOrDefault();

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
							&& ct.Parent_ContentID == c.Parent_ContentID
							&& (r.PageActive == bActiveOnly || bActiveOnly == false)
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).ToList();

			return oldC;
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
							&& r.FileName.ToLower() == sPage.ToLower()
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).FirstOrDefault();

			return oldC;
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootID) {

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
							&& ct.Root_ContentID == rootID
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).FirstOrDefault();

			return oldC;
		}


		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {
			var oldC1 = GetPageNavigation(siteID, sPage);

			SiteNav oldC = null;
			if (oldC1 != null) {
				oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
							&& r.FileName.ToLower() == oldC.FileName.ToLower()
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).FirstOrDefault();
			}
			return oldC;
		}

		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootID) {
			var oldC1 = GetPageNavigation(siteID, rootID);

			SiteNav oldC = null;
			if (oldC1 != null) {
				oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where r.SiteID == siteID
							&& ct.Root_ContentID == oldC1.Parent_ContentID
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).FirstOrDefault();
			}
			return oldC;
		}


		public List<SiteNav> GetChildNavigation(Guid siteID, Guid ParentID, bool bActiveOnly) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
							&& ct.Parent_ContentID == ParentID
							&& (r.PageActive == bActiveOnly || bActiveOnly == false)
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).ToList();
			return oldC;
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {

			var c = (from ct in db.tblContents
					 where ct.Root_ContentID == PageID
						&& ct.IsLatestVersion == true
					 select ct).FirstOrDefault();

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
							&& ct.Parent_ContentID == c.Parent_ContentID
							&& (r.PageActive == bActiveOnly || bActiveOnly == false)
							&& ct.IsLatestVersion == true
						select new SiteNav(r, ct)).ToList();

			return oldC;
		}


		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.EditDate descending
						where r.SiteID == siteID
							&& ct.IsLatestVersion == true
							&& (r.PageActive == bActiveOnly || bActiveOnly == false)
						select new SiteNav(r, ct)).Take(iUpdates).ToList();

			return oldC;
		}

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}

	public class SiteMapOrderHelper : IDisposable {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

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

			var c = (from ct in db.tblContents
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

			foreach (var m in oMap) {

				var c = (from ct in db.tblContents
						 join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						 where r.SiteID == siteID
							 && r.Root_ContentID == m.Root_ContentID
							 && ct.IsLatestVersion == true
						 select ct).FirstOrDefault();

				c.Parent_ContentID = m.Parent_ContentID;
				c.NavOrder = (m.NavOrder * 10);
			}

			db.SubmitChanges();
		}


		public List<SiteMapOrder> GetAdminPageList(Guid siteID, Guid contentID) {

			var lstSite = (from ct in db.tblContents
						   join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						   orderby ct.NavOrder, ct.NavMenuText
						   where r.SiteID == siteID
							   && ct.IsLatestVersion == true
						   select new SiteMapOrder {
							   NavLevel = -1,
							   NavMenuText = ct.NavMenuText,
							   NavOrder = ct.NavOrder,
							   PageActive = Convert.ToBoolean(r.PageActive),
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
				var lstLevel = (from z in lstSiteMap
								where z.NavLevel == iLevel
								select z).ToList();

				iBefore = lstSiteMap.Count;
				iLevel++;

				iLvlCounter = 0;

				var lstChild = (from s in lstSite
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
