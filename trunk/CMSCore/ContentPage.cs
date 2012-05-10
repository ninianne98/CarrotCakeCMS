using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Carrotware.CMS.Data;
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

	public class ContentPage {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public ContentPage() {

		}

		public ContentPage(tblRootContent rc, tblContent c) {

			if (rc == null) {
				rc = new tblRootContent();
				rc.Root_ContentID = Guid.NewGuid();
				rc.PageActive = true;
			}
			if (c == null) {
				c = new tblContent();
				c.ContentID = rc.Root_ContentID;
				c.Root_ContentID = rc.Root_ContentID;
			}

			this.RootContent = rc;
			this.Content = c;

			this.Root_ContentID = rc.Root_ContentID;
			this.SiteID = rc.SiteID;
			this.Heartbeat_UserId = rc.Heartbeat_UserId;
			this.EditHeartbeat = rc.EditHeartbeat;
			this.FileName = rc.FileName;
			this.PageActive = Convert.ToBoolean(rc.PageActive);

			this.ContentID = c.ContentID;
			this.Parent_ContentID = c.Parent_ContentID;
			this.IsLatestVersion = Convert.ToBoolean(c.IsLatestVersion);
			this.TitleBar = c.TitleBar;
			this.NavMenuText = c.NavMenuText;
			this.PageHead = c.PageHead;
			this.PageText = c.PageText;
			this.LeftPageText = c.LeftPageText;
			this.RightPageText = c.RightPageText;
			this.NavOrder = c.NavOrder;
			this.EditUserId = c.EditUserId;
			this.EditDate = c.EditDate;
			this.TemplateFile = c.TemplateFile;
			//this.NavFileName = AppendSiteFolder(rc.FileName);
			this.NavFileName = rc.FileName;

			this.MetaDescription = c.MetaDescription;
			this.MetaKeyword = c.MetaKeyword;

		}

		/*
		private Guid _siteid = Guid.Empty;
		private SiteData siteHelper = null;
		private void InitSite() {
			if (_siteid != SiteData.CurrentSiteID) {
				siteHelper = new SiteData();
				siteHelper.LoadSiteFromCache();
				_siteid = siteHelper.SiteID;
			}
		}
		*/

		public bool AdvancedEditMode {
			get {
				bool _Advanced = false;
				if (HttpContext.Current.User.Identity.IsAuthenticated) {
					if (HttpContext.Current.Request.QueryString["carrotedit"] != null && (IsAdmin || IsEditor)) {
						_Advanced = true;
					} else {
						_Advanced = false;
					}
				}
				return _Advanced;
			}
		}



		public bool IsAdmin {
			get {
				try {
					return Roles.IsUserInRole("CarrotCMS Administrators");
				} catch {
					return false;
				}
			}
		}
		public bool IsEditor {
			get {
				try {
					return Roles.IsUserInRole("CarrotCMS Editors");
				} catch {
					return false;
				}
			}
		}
		public bool IsUsers {
			get {
				try {
					return Roles.IsUserInRole("CarrotCMS Users");
				} catch {
					return false;
				}
			}
		}




		/*
		public string AppendSiteFolder(string sReqPath) {
			InitSite();
			var prefix = "";
			if (!string.IsNullOrEmpty(siteHelper.SiteFolder)) {
				prefix = siteHelper.SiteFolder;
			}
			if (!sReqPath.ToLower().StartsWith(prefix.ToLower())) {
				sReqPath = prefix + sReqPath;
			}
			sReqPath = sReqPath.Replace("//", "/");
			return sReqPath;
		}

		public string StripSiteFolder(string sReqPath) {
			InitSite();
			var prefix = "";
			if (!string.IsNullOrEmpty(siteHelper.SiteFolder)) {
				prefix = siteHelper.SiteFolder;
				if (sReqPath.ToLower().StartsWith(prefix.ToLower())) {
					sReqPath = sReqPath.Replace(prefix, "/");
				}
				sReqPath = sReqPath.Replace("//", "/");
			}
			return sReqPath;
		}
		*/


		public List<ContentPage> GetLatestContentList(Guid siteID) {
			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						orderby ct.NavOrder, ct.NavMenuText
						where r.SiteID == siteID
						 && ct.IsLatestVersion == true
						select new ContentPage(r, ct)).ToList();
			return oldC;
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
						select ct).ToList();

			if (oldC.Count > 0) {
				foreach (var c in oldC) {
					db.tblContents.DeleteOnSubmit(c);
				}
				db.SubmitChanges();

				//System.Web.HttpRuntime.UnloadAppDomain();
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


		public List<PageWidget> GetAllWidgetsAndUnsaved() {
			CMSConfigHelper ch = new CMSConfigHelper();
			List<PageWidget> widgets = new List<PageWidget>();
			if (ch.cmsAdminContent == null) {
				widgets = (from w in db.tblPageWidgets
						   where w.Root_ContentID == this.Root_ContentID
						   orderby w.WidgetOrder
						   select new PageWidget(w)).ToList();
			} else {
				widgets = (from w in ch.cmsAdminWidget
						   orderby w.WidgetOrder
						   select w).ToList();
			}

			return widgets;
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

		public void SavePageEdit() {

			var rc = (from r in db.tblRootContents
					  where r.Root_ContentID == this.Root_ContentID
						&& r.SiteID == this.SiteID
					  select r).FirstOrDefault();

			var oldC = (from ct in db.tblContents
						join r in db.tblRootContents on ct.Root_ContentID equals r.Root_ContentID
						where ct.Root_ContentID == this.Root_ContentID
							&& r.SiteID == this.SiteID
							&& ct.IsLatestVersion == true
						select ct).FirstOrDefault();

			bool bNew = false;

			if (rc == null) {
				rc = new tblRootContent();
				rc.Root_ContentID = this.Root_ContentID;
				rc.PageActive = true;
				rc.SiteID = this.SiteID;
				db.tblRootContents.InsertOnSubmit(rc);
				bNew = true;
			}

			var c = new tblContent();
			if (bNew) {
				c.ContentID = this.Root_ContentID;
			} else {
				c.ContentID = Guid.NewGuid();
				oldC.IsLatestVersion = false;
			}
			c.Root_ContentID = this.Root_ContentID;

			//rc.Root_ContentID = this.Root_ContentID;
			//rc.SiteID = this.SiteID;
			rc.Heartbeat_UserId = this.Heartbeat_UserId;
			rc.EditHeartbeat = this.EditHeartbeat;
			rc.FileName = this.FileName;
			rc.PageActive = this.PageActive;

			//if (string.IsNullOrEmpty(rc.FileName)) {
			//    rc.FileName = this.Root_ContentID.ToString().Substring(0, 6) + "-" + this.Root_ContentID.ToString().Substring(28, 6);
			//}

			//rc.FileName = rc.FileName.Trim().Replace(" ", "_");
			//rc.FileName = rc.FileName.Trim().Replace("'", "-");
			//rc.FileName = rc.FileName.Trim().Replace("\"", "-");

			//if (!rc.FileName.ToLower().EndsWith(".aspx")) {
			//    rc.FileName = rc.FileName + ".aspx";
			//}
			//if (rc.FileName.ToLower().IndexOf(@"/") < 0) {
			//    rc.FileName = "/" + rc.FileName;
			//}

			rc.FileName = ContentPage.ScrubFilename(this.Root_ContentID, rc.FileName);

			//c.ContentID = this.ContentID;
			c.Parent_ContentID = this.Parent_ContentID;
			c.IsLatestVersion = true;  //this.IsLatestVersion;
			c.TitleBar = this.TitleBar;
			c.NavMenuText = this.NavMenuText;
			c.PageHead = this.PageHead;
			c.PageText = this.PageText;
			c.LeftPageText = this.LeftPageText;
			c.RightPageText = this.RightPageText;
			c.NavOrder = this.NavOrder;
			c.EditUserId = this.EditUserId;
			c.EditDate = DateTime.Now;
			c.TemplateFile = this.TemplateFile;

			c.MetaKeyword = this.MetaKeyword;
			c.MetaDescription = this.MetaDescription;

			db.tblContents.InsertOnSubmit(c);
			db.SubmitChanges();

			//System.Web.HttpRuntime.UnloadAppDomain();
		}


		public Guid ContentID { get; set; }
		public DateTime EditDate { get; set; }
		public Guid? EditUserId { get; set; }
		public bool IsLatestVersion { get; set; }
		public string LeftPageText { get; set; }
		public string NavMenuText { get; set; }
		public int? NavOrder { get; set; }
		public string PageHead { get; set; }
		public string PageText { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public string RightPageText { get; set; }
		public Guid Root_ContentID { get; set; }
		public string TemplateFile { get; set; }
		public string TitleBar { get; set; }


		public DateTime? EditHeartbeat { get; set; }
		public string FileName { get; set; }
		public Guid? Heartbeat_UserId { get; set; }
		public bool PageActive { get; set; }
		public Guid SiteID { get; set; }

		public string NavFileName { get; set; }

		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }

		private tblRootContent RootContent { get; set; }
		private tblContent Content { get; set; }

	}

	public class SiteNav {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteNav() { }

		public SiteNav(tblRootContent rc, tblContent c) {

			if (rc == null) {
				rc = new tblRootContent();
				rc.Root_ContentID = Guid.NewGuid();
				rc.PageActive = true;
			}
			if (c == null) {
				c = new tblContent();
				c.ContentID = rc.Root_ContentID;
				c.Root_ContentID = rc.Root_ContentID;
			}

			this.Root_ContentID = rc.Root_ContentID;
			this.SiteID = rc.SiteID;
			this.FileName = rc.FileName;
			this.PageActive = Convert.ToBoolean(rc.PageActive);

			this.ContentID = c.ContentID;
			this.Parent_ContentID = c.Parent_ContentID;
			this.TitleBar = c.TitleBar;
			this.NavMenuText = c.NavMenuText;
			this.PageHead = c.PageHead;
			this.PageText = c.PageText;
			this.NavOrder = c.NavOrder;
			this.EditDate = c.EditDate;
			this.NavFileName = rc.FileName;
		}

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

		public Guid ContentID { get; set; }
		public DateTime EditDate { get; set; }
		public string NavMenuText { get; set; }
		public int? NavOrder { get; set; }
		public string PageHead { get; set; }
		public string PageText { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public string TitleBar { get; set; }

		public string FileName { get; set; }
		public bool PageActive { get; set; }
		public Guid SiteID { get; set; }

		public string NavFileName { get; set; }

	}

	public class SiteMapOrder {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteMapOrder() { }

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

			//int top = 0;
			//foreach (var t in lstSiteMap) {
			//    t.NavOrder = top;
			//    top = top + iPageCt + 1;
			//}


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

			//foreach (var x in lstSiteMap) {
			//    x.NavMenuText = x.NavMenuText + " [" + x.NavOrder + "]";
			//}

			return (from m in lstSiteMap
					orderby m.NavOrder, m.NavMenuText
					select m).ToList();
		}


		public int? NavOrder { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public string NavMenuText { get; set; }
		public bool PageActive { get; set; }
		public int NavLevel { get; set; }

	}

}
