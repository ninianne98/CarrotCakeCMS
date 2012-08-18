using System;
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

	public class ContentPageHelper : IDisposable {


		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public ContentPageHelper() {

		}

		public static string ScrubFilename(Guid rootContentID, string FileName) {
			string newFileName = FileName;

			if (string.IsNullOrEmpty(newFileName)) {
				newFileName = rootContentID.ToString();
			}

			newFileName = newFileName.Replace(" ", "-");
			newFileName = newFileName.Replace("'", "-");
			newFileName = newFileName.Replace("\"", "-");
			newFileName = newFileName.Replace(@"\", @"/");
			newFileName = newFileName.Replace("*", "-star-");
			newFileName = newFileName.Replace("%", "-percent-");
			newFileName = newFileName.Replace("&", "-n-");
			newFileName = newFileName.Replace("--", "-").Replace("--", "-");
			newFileName = newFileName.Replace(@"//", @"/").Replace(@"//", @"/");
			newFileName = newFileName.Trim();

			newFileName = Regex.Replace(newFileName, "[:\"*?<>|]+", "-");
			newFileName = Regex.Replace(newFileName, @"[^0-9a-zA-Z.-/_]+", "-");

			newFileName = newFileName.Replace("--", "-").Replace("--", "-");

			if (newFileName.ToLower().EndsWith(".htm")) {
				newFileName = newFileName.Substring(0, newFileName.Length - 4) + ".aspx";
			}
			if (newFileName.ToLower().EndsWith(".html")) {
				newFileName = newFileName.Substring(0, newFileName.Length - 5) + ".aspx";
			}

			if (!newFileName.ToLower().EndsWith(".aspx")) {
				newFileName = newFileName + ".aspx";
			}
			if (newFileName.ToLower().IndexOf(@"/") < 0) {
				newFileName = "/" + newFileName;
			}

			return newFileName;
		}

		public List<ContentPage> GetLatestContentList(Guid siteID) {
			List<ContentPage> lstContent = (from ct in db.carrot_Contents
											join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
											orderby ct.NavOrder, ct.NavMenuText
											where r.SiteID == siteID
											 && ct.IsLatestVersion == true
											select new ContentPage(r, ct)).ToList();
			return lstContent;
		}

		public int GetContentPagedListCount(Guid siteID, bool bActiveOnly) {
			int iCount = (from ct in db.carrot_Contents
						  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
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

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == rootContentID
								   && r.SiteID == siteID
								 select r).FirstOrDefault();

			rc.EditHeartbeat = DateTime.Now.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public bool RecordHeartbeatLock(Guid rootContentID, Guid siteID, Guid currentUserID) {

			carrot_RootContent rc = (from r in db.carrot_RootContents
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

		public bool IsPageLocked(Guid rootContentID) {

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == rootContentID
								 && r.SiteID == SiteData.CurrentSiteID
								 select r).FirstOrDefault();

			bool bLock = false;
			if (rc.Heartbeat_UserId != null) {
				if (rc.Heartbeat_UserId != SecurityData.CurrentUserGuid
						&& rc.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
					bLock = true;
				}
				if (rc.Heartbeat_UserId == SecurityData.CurrentUserGuid
					|| rc.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public bool IsPageLocked(Guid rootContentID, Guid siteID, Guid currentUserID) {

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == rootContentID
								 && r.SiteID == siteID
								 select r).FirstOrDefault();

			bool bLock = false;
			if (rc.Heartbeat_UserId != null) {
				if (rc.Heartbeat_UserId != currentUserID
						&& rc.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
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

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == rootContentID
								 && r.SiteID == siteID
								 select r).FirstOrDefault();

			bool bLock = false;
			if (rc.Heartbeat_UserId != null) {
				if (rc.Heartbeat_UserId != SecurityData.CurrentUserGuid
						&& rc.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
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
						&& cp.EditHeartbeat.Value > DateTime.Now.AddMinutes(-2)) {
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

			carrot_RootContent rc = (from r in db.carrot_RootContents
								 where r.Root_ContentID == rootContentID
								 && r.SiteID == siteID
								 select r).FirstOrDefault();

			if (rc != null) {
				return (Guid)rc.Heartbeat_UserId;
			} else {
				return Guid.Empty;
			}
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

			bool bIsContent = TestIfPropExists(new carrot_Content(), sortField);
			bool bIsRootContent = TestIfPropExists(new carrot_RootContent(), sortField);

			if (bIsRootContent) {
				if (sortDir.ToUpper().Trim().IndexOf("ASC") < 0) {
					query = (from enu in
								 (from ct in db.carrot_Contents.AsEnumerable()
								  join r in db.carrot_RootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
								  orderby GetPropertyValue(r, sortField) descending
								  where r.SiteID == siteID
									 && ct.IsLatestVersion == true
									 && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  select new ContentPage(r, ct)).AsQueryable()
							 select enu).AsQueryable().Skip(startRec).Take(pageSize);
				} else {
					query = (from enu in
								 (from ct in db.carrot_Contents.AsEnumerable()
								  join r in db.carrot_RootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
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
								 (from ct in db.carrot_Contents.AsEnumerable()
								  join r in db.carrot_RootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
								  orderby GetPropertyValue(ct, sortField) descending
								  where r.SiteID == siteID
									 && ct.IsLatestVersion == true
									 && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  select new ContentPage(r, ct)).AsQueryable()
							 select enu).AsQueryable().Skip(startRec).Take(pageSize);
				} else {
					query = (from enu in
								 (from ct in db.carrot_Contents.AsEnumerable()
								  join r in db.carrot_RootContents.AsEnumerable() on ct.Root_ContentID equals r.Root_ContentID
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
							 (from ct in db.carrot_Contents
							  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
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


		public List<ContentPage> GetVersionHistory(Guid siteID, Guid rootContentID) {
			List<ContentPage> content = (from ct in db.carrot_Contents
										 join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										 orderby ct.EditDate descending
										 where r.SiteID == siteID
										  && r.Root_ContentID == rootContentID
										 select new ContentPage(r, ct)).ToList();
			return content;
		}

		public ContentPage GetVersion(Guid siteID, Guid contentID) {
			ContentPage content = (from ct in db.carrot_Contents
								   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
								   orderby ct.EditDate descending
								   where r.SiteID == siteID
									&& ct.ContentID == contentID
								   select new ContentPage(r, ct)).FirstOrDefault();
			return content;
		}


		public List<ContentPage> GetLatestContentList(Guid siteID, bool? active) {
			List<ContentPage> lstContent = (from ct in db.carrot_Contents
											join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
											orderby ct.NavOrder, ct.NavMenuText
											where r.SiteID == siteID
											 && ct.IsLatestVersion == true
											 && (r.PageActive == active || active == null)
											select new ContentPage(r, ct)).ToList();

			return lstContent;
		}


		public void RemoveVersions(Guid siteID, List<Guid> lstDel) {

			List<carrot_Content> lstContent = (from ct in db.carrot_Contents
										   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										   orderby ct.EditDate descending
										   where r.SiteID == siteID
											&& lstDel.Contains(ct.ContentID)
											&& ct.IsLatestVersion != true
										   select ct).ToList();

			if (lstContent.Count > 0) {
				foreach (carrot_Content c in lstContent) {
					db.carrot_Contents.DeleteOnSubmit(c);
				}
				db.SubmitChanges();
			}
		}


		public void BulkUpdateTemplate(Guid siteID, List<Guid> lstUpd, string sTemplateFile) {

			List<carrot_Content> lstContent = (from ct in db.carrot_Contents
										   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										   where r.SiteID == siteID
											&& lstUpd.Contains(r.Root_ContentID)
											&& ct.IsLatestVersion == true
										   select ct).ToList();

			if (lstContent.Count > 0) {
				foreach (carrot_Content c in lstContent) {
					c.TemplateFile = sTemplateFile;
					//c.EditDate = DateTime.Now;
				}
				db.SubmitChanges();
			}
		}



		public ContentPage GetLatestContent(Guid siteID, Guid rootContentID) {
			ContentPage content = (from ct in db.carrot_Contents
								   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
								   where r.SiteID == siteID
									   && r.Root_ContentID == rootContentID
									   && ct.IsLatestVersion == true
								   select new ContentPage(r, ct)).FirstOrDefault();

			return content;
		}


		public ContentPage GetLatestContent(Guid siteID, bool? active, string sPage) {
			ContentPage content = (from ct in db.carrot_Contents
								   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
								   where r.SiteID == siteID
									   && (r.PageActive == active || active == null)
									   && r.FileName.ToLower() == sPage.ToLower()
									   && ct.IsLatestVersion == true
								   select new ContentPage(r, ct)).FirstOrDefault();

			return content;
		}



		public ContentPage FindHome(Guid siteID) {
			ContentPage content = (from ct in db.carrot_Contents
								   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
								   orderby ct.NavOrder ascending
								   where r.SiteID == siteID
									   && r.PageActive == true
									   && ct.NavOrder < 1
									   && ct.IsLatestVersion == true
								   select new ContentPage(r, ct)).FirstOrDefault();

			return content;
		}


		public int GetSitePageCount(Guid siteID) {
			int content = (from r in db.carrot_RootContents
						   where r.SiteID == siteID
						   select r).Count();
			return content;
		}

		public ContentPage FindByFilename(Guid siteID, string urlFileName) {
			ContentPage content = (from ct in db.carrot_Contents
								   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
								   where r.SiteID == siteID
									   && ct.IsLatestVersion == true
									   && r.FileName.ToLower() == urlFileName.ToLower()
								   select new ContentPage(r, ct)).FirstOrDefault();

			return content;
		}

		public ContentPage FindHome(Guid siteID, bool? active) {
			ContentPage content = (from ct in db.carrot_Contents
								   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
								   orderby ct.NavOrder ascending
								   where r.SiteID == siteID
									   && (r.PageActive == active || active == null)
									   && ct.NavOrder < 1
									   && ct.IsLatestVersion == true
								   select new ContentPage(r, ct)).FirstOrDefault();

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

	public class SiteNavHelper : IDisposable {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteNavHelper() { }

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.NavOrder, ct.NavMenuText
										where r.SiteID == siteID
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
											&& ct.IsLatestVersion == true
										select new SiteNav(r, ct)).ToList();
			return lstContent;
		}



		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.NavOrder, ct.NavMenuText
										where r.SiteID == siteID
											&& ct.Parent_ContentID == null
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
											&& ct.IsLatestVersion == true
										select new SiteNav(r, ct)).ToList();
			return lstContent;
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sParentID, bool bActiveOnly) {

			carrot_RootContent p = (from r in db.carrot_RootContents
								where r.SiteID == siteID
									   && r.FileName.ToLower() == sParentID.ToLower()
								select r).FirstOrDefault();

			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.NavOrder, ct.NavMenuText
										where r.SiteID == siteID
											&& ct.Parent_ContentID == p.Root_ContentID
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
											&& ct.IsLatestVersion == true
										select new SiteNav(r, ct)).ToList();

			return lstContent;
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {

			carrot_Content c = (from ct in db.carrot_Contents
							join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							where r.SiteID == siteID
								   && r.FileName.ToLower() == sPage.ToLower()
								   && ct.IsLatestVersion == true
							select ct).FirstOrDefault();

			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.NavOrder, ct.NavMenuText
										where r.SiteID == siteID
											&& ct.Parent_ContentID == c.Parent_ContentID
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
											&& ct.IsLatestVersion == true
										select new SiteNav(r, ct)).ToList();

			return lstContent;
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {

			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && r.FileName.ToLower() == sPage.ToLower()
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {

			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && ct.Root_ContentID == rootContentID
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}


		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {
			SiteNav nav1 = GetPageNavigation(siteID, sPage);

			SiteNav content = null;
			if (nav1 != null) {
				content = (from ct in db.carrot_Contents
						   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						   where r.SiteID == siteID
							   && r.FileName.ToLower() == content.FileName.ToLower()
							   && ct.IsLatestVersion == true
						   select new SiteNav(r, ct)).FirstOrDefault();
			}
			return content;
		}

		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID) {
			SiteNav nav1 = GetPageNavigation(siteID, rootContentID);

			SiteNav content = null;
			if (nav1 != null) {
				content = (from ct in db.carrot_Contents
						   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						   where r.SiteID == siteID
							   && ct.Root_ContentID == nav1.Parent_ContentID
							   && ct.IsLatestVersion == true
						   select new SiteNav(r, ct)).FirstOrDefault();
			}
			return content;
		}


		public List<SiteNav> GetChildNavigation(Guid siteID, Guid ParentID, bool bActiveOnly) {
			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.NavOrder, ct.NavMenuText
										where r.SiteID == siteID
											&& ct.Parent_ContentID == ParentID
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
											&& ct.IsLatestVersion == true
										select new SiteNav(r, ct)).ToList();
			return lstContent;
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {

			carrot_Content c = (from ct in db.carrot_Contents
							where ct.Root_ContentID == PageID
							   && ct.IsLatestVersion == true
							select ct).FirstOrDefault();

			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.NavOrder, ct.NavMenuText
										where r.SiteID == siteID
											&& ct.Parent_ContentID == c.Parent_ContentID
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
											&& ct.IsLatestVersion == true
										select new SiteNav(r, ct)).ToList();

			return lstContent;
		}


		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {

			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.EditDate descending
										where r.SiteID == siteID
											&& ct.IsLatestVersion == true
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
										select new SiteNav(r, ct)).Take(iUpdates).ToList();

			return lstContent;
		}


		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && r.Root_ContentID == rootContentID
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}

		public SiteNav GetLatestVersion(Guid siteID, bool? active, string sPage) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && (r.PageActive == active || active == null)
								   && r.FileName.ToLower() == sPage.ToLower()
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}


		public SiteNav FindHome(Guid siteID) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   orderby ct.NavOrder ascending
							   where r.SiteID == siteID
								   && r.PageActive == true
								   && ct.NavOrder < 1
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();
			return content;
		}

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && ct.IsLatestVersion == true
								   && r.FileName.ToLower() == urlFileName.ToLower()
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}

		public SiteNav FindHome(Guid siteID, bool? active) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   orderby ct.NavOrder ascending
							   where r.SiteID == siteID
								   && (r.PageActive == active || active == null)
								   && ct.NavOrder < 1
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();
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


		public List<SiteMapOrder> GetAdminPageList(Guid siteID, Guid contentID) {

			List<SiteMapOrder> lstSite = (from ct in db.carrot_Contents
										  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
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
