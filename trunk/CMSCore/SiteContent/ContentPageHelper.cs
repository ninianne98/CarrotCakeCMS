using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml.Serialization;
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


		public ContentPageHelper() {
			//#if DEBUG
			//            db.Log = new DebugTextWriter();
			//#endif
		}

		/*
		internal static ContentPage CreateContentPage(carrot_RootContent rc, carrot_Content c) {
			ContentPage cont = new ContentPage();

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

				cont.Root_ContentID = rc.Root_ContentID;
				cont.SiteID = rc.SiteID;
				cont.Heartbeat_UserId = rc.Heartbeat_UserId;
				cont.EditHeartbeat = rc.EditHeartbeat;
				cont.FileName = rc.FileName;
				cont.CreateDate = rc.CreateDate;

				cont.PageActive = rc.PageActive;

				cont.ContentID = c.ContentID;
				cont.Parent_ContentID = c.Parent_ContentID;
				cont.IsLatestVersion = c.IsLatestVersion.Value;
				cont.TitleBar = c.TitleBar;
				cont.NavMenuText = c.NavMenuText;
				cont.PageHead = c.PageHead;
				cont.PageText = c.PageText;
				cont.LeftPageText = c.LeftPageText;
				cont.RightPageText = c.RightPageText;
				cont.NavOrder = c.NavOrder;
				cont.EditUserId = c.EditUserId;
				cont.EditDate = c.EditDate;
				cont.TemplateFile = c.TemplateFile;

				cont.MetaDescription = c.MetaDescription;
				cont.MetaKeyword = c.MetaKeyword;
			}

			return cont;
		}
		*/

		internal static ContentPage CreateContentPage(vw_carrot_Content c) {
			ContentPage cont = null;

			if (c != null) {
				cont = new ContentPage();

				cont.Root_ContentID = c.Root_ContentID;
				cont.SiteID = c.SiteID;
				cont.Heartbeat_UserId = c.Heartbeat_UserId;
				cont.EditHeartbeat = c.EditHeartbeat;
				cont.FileName = c.FileName;
				cont.CreateDate = c.CreateDate;

				cont.PageActive = c.PageActive;

				cont.ContentID = c.ContentID;
				cont.Parent_ContentID = c.Parent_ContentID;
				cont.IsLatestVersion = c.IsLatestVersion.Value;
				cont.TitleBar = c.TitleBar;
				cont.NavMenuText = c.NavMenuText;
				cont.PageHead = c.PageHead;
				cont.PageText = c.PageText;
				cont.LeftPageText = c.LeftPageText;
				cont.RightPageText = c.RightPageText;
				cont.NavOrder = c.NavOrder;
				cont.EditUserId = c.EditUserId;
				cont.EditDate = c.EditDate;
				cont.TemplateFile = c.TemplateFile;

				cont.MetaDescription = c.MetaDescription;
				cont.MetaKeyword = c.MetaKeyword;
			}

			return cont;
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

			newFileName = newFileName.Replace(" ", "-");
			newFileName = newFileName.Replace("'", "-");
			newFileName = newFileName.Replace("\"", "-");
			newFileName = newFileName.Replace("*", "-star-");
			newFileName = newFileName.Replace("%", "-percent-");
			newFileName = newFileName.Replace("&", "-n-");
			newFileName = newFileName.Replace("--", "-").Replace("--", "-");
			newFileName = newFileName.Replace(@"//", @"/").Replace(@"//", @"/");
			newFileName = newFileName.Trim();

			newFileName = Regex.Replace(newFileName, "[:\"*?<>|]+", "-");
			newFileName = Regex.Replace(newFileName, @"[^0-9a-zA-Z.-/_]+", "-");

			newFileName = newFileName.Replace("--", "-").Replace("--", "-");


			if (newFileName.EndsWith(@"/")) {
				newFileName = newFileName + SiteData.DefaultDirectoryFilename;
				newFileName = newFileName.Replace(@"//", @"/");
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


		public List<ContentPage> GetLatestContentList(Guid siteID) {
			List<ContentPage> lstContent = CompiledQueries.cqGetLatestContentListA(db, siteID, null).Select(ct => CreateContentPage(ct)).ToList();

			return lstContent;
		}


		public int GetSitePageCount(Guid siteID, bool bActiveOnly) {
			int iCount = CompiledQueries.cqGetLatestContentListB(db, siteID, bActiveOnly).Count();

			return iCount;
		}

		public int GetSitePageCount(Guid siteID) {
			int iCount = CompiledQueries.cqGetLatestContentListB(db, siteID, false).Count();

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

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, siteID, rootContentID);

			rc.EditHeartbeat = DateTime.Now.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public bool RecordHeartbeatLock(Guid rootContentID, Guid siteID, Guid currentUserID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, siteID, rootContentID);

			if (rc != null) {
				rc.Heartbeat_UserId = currentUserID;
				rc.EditHeartbeat = DateTime.Now;
				db.SubmitChanges();
				return true;
			}

			return false;
		}

		public bool IsPageLocked(Guid rootContentID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, SiteData.CurrentSiteID, rootContentID);

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

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, siteID, rootContentID);

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

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, siteID, rootContentID);

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

			carrot_RootContent rc = CompiledQueries.cqGetRootContent(db, siteID, rootContentID);

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

			bool IsContentProp = false;

			sortDir = sortDir.ToUpper();

			IQueryable<vw_carrot_Content> query1 = null;
			IEnumerable<ContentPage> query2 = new List<ContentPage>();

			sortField = (from p in ReflectionUtilities.GetPropertyStrings(typeof(vw_carrot_Content))
						 where p.ToLower().Trim() == sortField.ToLower().Trim()
						 select p).FirstOrDefault();

			if (!string.IsNullOrEmpty(sortField)) {
				IsContentProp = ReflectionUtilities.DoesPropertyExist(typeof(vw_carrot_Content), sortField);
			}

			query1 = CompiledQueries.cqGetLatestContentListB(db, siteID, bActiveOnly);

			if (IsContentProp) {
				query1 = ReflectionUtilities.SortByParm<vw_carrot_Content>(query1, sortField, sortDir);
			}

			if (!IsContentProp) {
				query1 = (from c in query1
						  orderby c.CreateDate descending
						  where c.SiteID == siteID
							 && c.IsLatestVersion == true
							 && (c.PageActive == bActiveOnly || bActiveOnly == false)
						  select c).AsQueryable();
			}

			query2 = (from q in query1
					  select CreateContentPage(q)).Skip(startRec).Take(pageSize);

			return query2.ToList();
		}

		public ContentPage CopyContentPageToNew(ContentPage pageSource) {
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
			pageNew.EditDate = DateTime.Now;
			pageNew.CreateDate = pageSource.CreateDate;

			pageNew.TemplateFile = pageSource.TemplateFile;
			pageNew.MetaDescription = pageSource.MetaDescription;
			pageNew.MetaKeyword = pageSource.MetaKeyword;

			return pageNew;
		}


		public static ContentPage GetSamplerView() {

			string sFile1 = "";
			string sFile2 = "";

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
			pageNew.EditDate = DateTime.Now.AddMinutes(-30);
			pageNew.CreateDate = DateTime.Today.AddDays(-1);

			pageNew.TemplateFile = SiteData.PreviewTemplateFile;
			pageNew.FileName = SiteData.PreviewTemplateFilePage;
			pageNew.MetaDescription = "Meta Description";
			pageNew.MetaKeyword = "Meta Keyword";

			return pageNew;
		}


		public List<ContentPage> GetVersionHistory(Guid siteID, Guid rootContentID) {
			List<ContentPage> content = CompiledQueries.cqGetVersionHistory(db, siteID, rootContentID).Select(ct => CreateContentPage(ct)).ToList();

			return content;
		}

		public ContentPage GetVersion(Guid siteID, Guid contentID) {
			ContentPage content = CreateContentPage(CompiledQueries.cqGetContentByContentID(db, siteID, contentID));

			return content;
		}

		public List<ContentPage> GetLatestContentList(Guid siteID, bool? active) {
			List<ContentPage> lstContent = CompiledQueries.cqGetLatestContentListA(db, siteID, active).Select(ct => CreateContentPage(ct)).ToList();

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


		public ContentPage FindContentByID(Guid siteID, Guid rootContentID) {
			ContentPage content = CreateContentPage(CompiledQueries.cqGetLatestContentByID(db, siteID, null, rootContentID));

			return content;
		}

		public ContentPage GetLatestContentByURL(Guid siteID, bool? active, string sPage) {
			ContentPage content = CreateContentPage(CompiledQueries.cqGetLatestContentByURL(db, siteID, active, sPage));

			return content;
		}

		public ContentPage FindByFilename(Guid siteID, string urlFileName) {
			ContentPage content = CreateContentPage(CompiledQueries.cqGetLatestContentByURL(db, siteID, null, urlFileName));

			return content;
		}

		public ContentPage FindHome(Guid siteID) {
			ContentPage content = CompiledQueries.cqFindHome(db, siteID, true).Select(ct => CreateContentPage(ct)).FirstOrDefault();

			return content;
		}

		public ContentPage FindHome(Guid siteID, bool? active) {
			ContentPage content = CompiledQueries.cqFindHome(db, siteID, active).Select(ct => CreateContentPage(ct)).FirstOrDefault();

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
