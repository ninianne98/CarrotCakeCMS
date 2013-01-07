using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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

	public class ContentPage : IDisposable, ISiteContent {

		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public ContentPage() { }


		internal ContentPage(vw_carrot_Content c) {

			if (c != null) {
				SiteData site = SiteData.GetSiteFromCache(c.SiteID);

				this.Root_ContentID = c.Root_ContentID;
				this.SiteID = c.SiteID;
				this.Heartbeat_UserId = c.Heartbeat_UserId;
				this.EditHeartbeat = c.EditHeartbeat;
				this.FileName = c.FileName;
				this.CreateDate = site.ConvertUTCToSiteTime(c.CreateDate);
				this.GoLiveDate = site.ConvertUTCToSiteTime(c.GoLiveDate);
				this.RetireDate = site.ConvertUTCToSiteTime(c.RetireDate);
				this.EditDate = site.ConvertUTCToSiteTime(c.EditDate);

				this.PageActive = c.PageActive;

				this.PageSlug = c.PageSlug;
				this.ContentType = ContentPageType.GetTypeByID(c.ContentTypeID);

				this.ContentID = c.ContentID;
				this.Parent_ContentID = c.Parent_ContentID;
				this.IsLatestVersion = c.IsLatestVersion;
				this.TitleBar = c.TitleBar;
				this.NavMenuText = c.NavMenuText;
				this.PageHead = c.PageHead;
				this.PageText = c.PageText;
				this.LeftPageText = c.LeftPageText;
				this.RightPageText = c.RightPageText;
				this.NavOrder = c.NavOrder;
				this.EditUserId = c.EditUserId;
				this.TemplateFile = c.TemplateFile;
				this.Thumbnail = c.PageThumbnail;

				if (string.IsNullOrEmpty(this.PageSlug) && this.ContentType == ContentPageType.PageType.BlogEntry) {
					this.PageSlug = c.FileName;
				}

				this.MetaDescription = c.MetaDescription;
				this.MetaKeyword = c.MetaKeyword;
			}
		}


		public SiteNav GetSiteNav() {
			SiteNav sd = null;
			if (SiteData.IsPageSampler || HttpContext.Current == null) {
				sd = SiteNavHelper.GetSamplerView();
			} else {
				using (SiteNavHelper sdh = new SiteNavHelper()) {
					sd = sdh.GetLatestVersion(this.SiteID, this.Root_ContentID);
				}
			}
			return sd;
		}


		public void ResetHeartbeatLock() {

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, this.SiteID, this.Root_ContentID);

			rc.EditHeartbeat = DateTime.UtcNow.AddHours(-2);
			rc.Heartbeat_UserId = null;
			db.SubmitChanges();
		}

		public void RecordHeartbeatLock(Guid currentUserID) {

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, this.SiteID, this.Root_ContentID);

			rc.Heartbeat_UserId = currentUserID;
			rc.EditHeartbeat = DateTime.UtcNow;

			db.SubmitChanges();
		}

		public bool IsPageLocked() {

			bool bLock = false;
			if (this.Heartbeat_UserId != null) {
				if (this.Heartbeat_UserId != SecurityData.CurrentUserGuid
						&& this.EditHeartbeat.Value > DateTime.UtcNow.AddMinutes(-2)) {
					bLock = true;
				}
				if (this.Heartbeat_UserId == SecurityData.CurrentUserGuid
					|| this.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}


		public void LoadAttributes() {
			var lstC = this.ContentCategories;
			var lstT = this.ContentTags;
		}

		public List<Widget> GetWidgetList() {
			List<Widget> widgets = null;

			using (WidgetHelper pwh = new WidgetHelper()) {
				widgets = pwh.GetWidgets(this.Root_ContentID, false);
			}

			return widgets;
		}


		public List<Widget> GetAllWidgetsOrUnsaved() {
			List<Widget> widgets = new List<Widget>();
			using (CMSConfigHelper ch = new CMSConfigHelper()) {
				if (ch.cmsAdminContent == null) {
					widgets = this.GetWidgetList();
				} else {
					widgets = (from w in ch.cmsAdminWidget
							   orderby w.WidgetOrder
							   select w).ToList();
				}
			}
			return widgets;
		}

		private void FixMeta() {
			this.MetaKeyword = string.IsNullOrEmpty(this.MetaKeyword) ? String.Empty : this.MetaKeyword;
			this.MetaDescription = string.IsNullOrEmpty(this.MetaDescription) ? String.Empty : this.MetaDescription;
		}


		private void SaveKeywordsAndTags() {
			IQueryable<carrot_TagContentMapping> oldContentTags = CannedQueries.GetContentTagMapByContentID(db, this.Root_ContentID);
			IQueryable<carrot_CategoryContentMapping> oldContentCategories = CannedQueries.GetContentCategoryMapByContentID(db, this.Root_ContentID);

			List<carrot_TagContentMapping> newContentTags = (from x in ContentTags
															 select new carrot_TagContentMapping {
																 ContentTagID = x.ContentTagID,
																 Root_ContentID = this.Root_ContentID,
																 TagContentMappingID = Guid.NewGuid()
															 }).ToList();

			List<carrot_CategoryContentMapping> newContentCategories = (from x in ContentCategories
																		select new carrot_CategoryContentMapping {
																			ContentCategoryID = x.ContentCategoryID,
																			Root_ContentID = this.Root_ContentID,
																			CategoryContentMappingID = Guid.NewGuid()
																		}).ToList();


			foreach (carrot_TagContentMapping s in newContentTags) {
				db.carrot_TagContentMappings.InsertOnSubmit(s);
			}
			foreach (carrot_CategoryContentMapping s in newContentCategories) {
				db.carrot_CategoryContentMappings.InsertOnSubmit(s);
			}

			db.carrot_TagContentMappings.DeleteBatch(oldContentTags);
			db.carrot_CategoryContentMappings.DeleteBatch(oldContentCategories);
		}


		private void PerformCommonSaveRoot(SiteData pageSite, carrot_RootContent rc) {

			rc.Root_ContentID = this.Root_ContentID;
			rc.PageActive = true;
			rc.SiteID = this.SiteID;
			rc.ContentTypeID = ContentPageType.GetIDByType(this.ContentType);

			rc.CreateDate = DateTime.UtcNow;
			rc.GoLiveDate = pageSite.ConvertSiteTimeToUTC(this.GoLiveDate);
			rc.RetireDate = pageSite.ConvertSiteTimeToUTC(this.RetireDate);

			if (this.CreateDate.Year > 1950) {
				rc.CreateDate = pageSite.ConvertSiteTimeToUTC(this.CreateDate);
			}

		}


		private void PerformCommonSave(SiteData pageSite, carrot_RootContent rc, carrot_Content c) {

			c.NavOrder = this.NavOrder;

			if (this.ContentType == ContentPageType.PageType.BlogEntry) {
				this.PageSlug = ContentPageHelper.ScrubFilename(this.Root_ContentID, this.PageSlug);
				this.FileName = ContentPageHelper.CreateFileNameFromSlug(this.SiteID, this.GoLiveDate, this.PageSlug);
				c.NavOrder = 10;
			}

			rc.GoLiveDateLocal = this.GoLiveDate;
			rc.GoLiveDate = pageSite.ConvertSiteTimeToUTC(this.GoLiveDate);
			rc.RetireDate = pageSite.ConvertSiteTimeToUTC(this.RetireDate);

			rc.PageSlug = this.PageSlug;
			rc.PageThumbnail = this.Thumbnail;

			c.Root_ContentID = this.Root_ContentID;

			rc.Heartbeat_UserId = this.Heartbeat_UserId;
			rc.EditHeartbeat = this.EditHeartbeat;

			rc.FileName = this.FileName;
			rc.PageActive = this.PageActive;

			rc.FileName = ContentPageHelper.ScrubFilename(this.Root_ContentID, rc.FileName);

			c.Parent_ContentID = this.Parent_ContentID;
			c.IsLatestVersion = true;
			c.TitleBar = this.TitleBar;
			c.NavMenuText = this.NavMenuText;
			c.PageHead = this.PageHead;
			c.PageText = this.PageText;
			c.LeftPageText = this.LeftPageText;
			c.RightPageText = this.RightPageText;

			c.EditUserId = this.EditUserId;
			c.EditDate = DateTime.UtcNow;
			c.TemplateFile = this.TemplateFile;

			FixMeta();
			c.MetaKeyword = this.MetaKeyword.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
			c.MetaDescription = this.MetaDescription.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");

			this.Root_ContentID = rc.Root_ContentID;
			this.ContentID = c.ContentID;
			this.FileName = rc.FileName;
			this.EditDate = pageSite.ConvertUTCToSiteTime(c.EditDate);
			this.CreateDate = pageSite.ConvertUTCToSiteTime(rc.CreateDate);
			this.GoLiveDate = pageSite.ConvertUTCToSiteTime(rc.GoLiveDate);
			this.RetireDate = pageSite.ConvertUTCToSiteTime(rc.RetireDate);

		}


		public void SaveTrackbacks() {

			SiteData site = SiteData.GetSiteFromCache(this.SiteID);

			if (!string.IsNullOrEmpty(this.NewTrackBackURLs)) {
				this.NewTrackBackURLs = this.NewTrackBackURLs.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n\n", "\n");
				string[] TBURLs = this.NewTrackBackURLs.Split('\n');
				foreach (string sURL in TBURLs) {
					List<TrackBackEntry> lstTB = TrackBackEntry.FindTrackbackByURL(this.Root_ContentID, sURL);
					if (lstTB.Count < 1) {
						TrackBackEntry tbe = new TrackBackEntry {
							Root_ContentID = this.Root_ContentID,
							TrackBackURL = sURL,
							TrackedBack = false
						};

						tbe.Save();
					}
				}
			}

			if (this.IsLatestVersion && site.SendTrackbacks) {
				List<TrackBackEntry> lstTBQ = TrackBackEntry.GetTrackBackQueue(this.Root_ContentID);
				foreach (TrackBackEntry t in lstTBQ) {
					if (t.CreateDate > site.Now.AddMinutes(-10)) {
						try {
							TrackBacker tb = new TrackBacker();
							t.TrackBackResponse = tb.SendTrackback(t.Root_ContentID, site.SiteID, t.TrackBackURL);
							t.TrackedBack = true;
							t.Save();
						} catch (Exception ex) { }
					}
				}
			}
		}


		public void SaveTrackbackTop() {

			SiteData site = SiteData.GetSiteFromCache(this.SiteID);

			if (this.IsLatestVersion && site.SendTrackbacks) {
				TrackBackEntry t = TrackBackEntry.GetTrackBackQueue(this.Root_ContentID).FirstOrDefault();
				if (t != null && t.CreateDate > site.Now.AddMinutes(-10)) {
					try {
						TrackBacker tb = new TrackBacker();
						t.TrackBackResponse = tb.SendTrackback(t.Root_ContentID, site.SiteID, t.TrackBackURL);
						t.TrackedBack = true;
						t.Save();
					} catch (Exception ex) { }
				}
			}
		}

		public void ApplyTemplate() {

			carrot_Content c = CompiledQueries.cqGetLatestContentTbl(db, this.SiteID, this.Root_ContentID);

			if (c != null) {
				c.TemplateFile = this.TemplateFile;

				db.SubmitChanges();
			}
		}

		public void SavePageEdit() {

			SiteData site = SiteData.GetSiteFromCache(this.SiteID);

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, this.SiteID, this.Root_ContentID);

			carrot_Content oldC = CompiledQueries.cqGetLatestContentTbl(db, this.SiteID, this.Root_ContentID);

			bool bNew = false;

			if (rc == null) {
				rc = new carrot_RootContent();

				PerformCommonSaveRoot(site, rc);

				db.carrot_RootContents.InsertOnSubmit(rc);
				bNew = true;
			}

			carrot_Content c = new carrot_Content();
			if (bNew) {
				c.ContentID = this.Root_ContentID;
			} else {
				c.ContentID = Guid.NewGuid();
				oldC.IsLatestVersion = false;
			}

			PerformCommonSave(site, rc, c);

			db.carrot_Contents.InsertOnSubmit(c);

			SaveKeywordsAndTags();

			db.SubmitChanges();

			SaveTrackbacks();
		}

		public void SavePageAsDraft() {

			SiteData site = SiteData.GetSiteFromCache(this.SiteID);

			carrot_RootContent rc = CompiledQueries.cqGetRootContentTbl(db, this.SiteID, this.Root_ContentID);

			bool bNew = false;

			if (rc == null) {
				rc = new carrot_RootContent();

				PerformCommonSaveRoot(site, rc);

				db.carrot_RootContents.InsertOnSubmit(rc);
				bNew = true;
			}

			carrot_Content c = new carrot_Content();
			if (bNew) {
				c.ContentID = this.Root_ContentID;
			} else {
				c.ContentID = Guid.NewGuid();
			}

			PerformCommonSave(site, rc, c);

			c.IsLatestVersion = false; // draft, leave existing version latest

			db.carrot_Contents.InsertOnSubmit(c);

			SaveKeywordsAndTags();

			db.SubmitChanges();

			this.IsLatestVersion = c.IsLatestVersion;
			SaveTrackbacks();
		}


		public ContentPageType.PageType ContentType { get; set; }
		public string PageSlug { get; set; }

		public Guid ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public DateTime EditDate { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime GoLiveDate { get; set; }
		public DateTime RetireDate { get; set; }
		public Guid? EditUserId { get; set; }
		public bool IsLatestVersion { get; set; }
		public string TemplateFile { get; set; }
		public string Thumbnail { get; set; }
		public string FileName { get; set; }
		public string PageHead { get; set; }
		public string TitleBar { get; set; }
		public string NavMenuText { get; set; }
		public string PageText { get; set; }
		public string LeftPageText { get; set; }
		public string RightPageText { get; set; }
		public int NavOrder { get; set; }
		public Guid? Parent_ContentID { get; set; }

		public DateTime? EditHeartbeat { get; set; }
		public Guid? Heartbeat_UserId { get; set; }
		public bool PageActive { get; set; }
		public Guid SiteID { get; set; }

		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }

		public string NewTrackBackURLs { get; set; }


		public bool IsRetired {
			get {
				if (this.RetireDate < SiteData.CurrentSite.Now) {
					return true;
				} else {
					return false;
				}
			}
		}
		public bool IsUnReleased {
			get {
				if (this.GoLiveDate > SiteData.CurrentSite.Now) {
					return true;
				} else {
					return false;
				}
			}
		}

		private int _commentCount = -1;
		public int CommentCount {
			get {
				if (_commentCount < 0) {
					_commentCount = PostComment.GetCommentCountByContent(this.Root_ContentID, !SecurityData.IsAuthEditor);
				}
				return _commentCount;
			}
			set {
				_commentCount = value;
			}
		}


		private List<ContentTag> _contentTags = null;
		public List<ContentTag> ContentTags {
			get {
				if (_contentTags == null) {
					_contentTags = ContentTag.BuildTagList(this.Root_ContentID);
				}
				return _contentTags;
			}
			set {
				_contentTags = value;
			}
		}

		private List<ContentCategory> _contentCategories = null;
		public List<ContentCategory> ContentCategories {
			get {
				if (_contentCategories == null) {
					_contentCategories = ContentCategory.BuildCategoryList(this.Root_ContentID);
				}
				return _contentCategories;
			}
			set {
				_contentCategories = value;
			}
		}

		ExtendedUserData _user = null;
		public ExtendedUserData GetUserInfo() {
			if (_user == null && this.EditUserId.HasValue) {
				_user = new ExtendedUserData(this.EditUserId.Value);
			}
			return _user;
		}


		public List<TrackBackEntry> GetTrackbacks() {

			return TrackBackEntry.GetPageTrackBackList(this.Root_ContentID);
		}


		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is ContentPage) {
				ContentPage p = (ContentPage)obj;
				return (this.ContentID == p.ContentID)
						&& (this.SiteID == p.SiteID)
						&& (this.FileName.ToLower() == p.FileName.ToLower());
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return ContentID.GetHashCode() ^ SiteID.GetHashCode() ^ Root_ContentID.GetHashCode() ^ FileName.GetHashCode();
		}


		public string PageTextPlainSummaryMedium {
			get {
				string txt = !string.IsNullOrEmpty(PageText) ? PageText : "";
				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("&nbsp;", " ").Replace('\u00A0', ' ');

				txt = Regex.Replace(txt, @"<!--(\n|.)*-->", " ");
				txt = Regex.Replace(txt, @"<(.|\n)*?>", " ");
				txt = txt.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ").Replace("  ", " ");

				if (txt.Length > 4096) {
					txt = txt.Substring(0, 4096);
				}

				if (txt.Length > 800) {
					return txt.Substring(0, 768).Trim() + "[.....]";
				} else {
					return txt;
				}
			}
		}

		public string PageTextPlainSummary {
			get {
				string txt = PageTextPlainSummaryMedium;

				if (txt.Length > 300) {
					return txt.Substring(0, 256).Trim() + "[.....]";
				} else {
					return txt;
				}
			}
		}


		public string TemplateFolderPath {
			get {
				if (!string.IsNullOrEmpty(TemplateFile)) {
					if (TemplateFile.LastIndexOf("/") >= 2) {
						return TemplateFile.Substring(0, TemplateFile.LastIndexOf("/") + 1);
					} else {
						return "/";
					}
				} else {
					return "/";
				}
			}
		}

		public string RequestedFileName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
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
