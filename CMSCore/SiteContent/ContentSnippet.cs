﻿using System;
using System.Collections.Generic;
using System.Linq;
using Carrotware.CMS.Data;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class ContentSnippet {

		public ContentSnippet() { }

		public ContentSnippet(Guid siteID) {
			this.Root_ContentSnippetID = Guid.NewGuid();
			this.ContentSnippetID = Guid.NewGuid();
			this.SiteID = siteID;
			this.CreateDate = SiteData.GetSiteByID(siteID).Now;
			this.EditDate = this.CreateDate;

			this.GoLiveDate = this.CreateDate.AddHours(-1);
			this.RetireDate = this.CreateDate.AddYears(2);
		}

		public Guid Root_ContentSnippetID { get; set; }
		public Guid SiteID { get; set; }
		public string ContentSnippetName { get; set; }
		public string ContentSnippetSlug { get; set; }
		public Guid CreateUserId { get; set; }
		public DateTime CreateDate { get; set; }
		public bool ContentSnippetActive { get; set; }
		public Guid ContentSnippetID { get; set; }
		public bool IsLatestVersion { get; set; }
		public Guid EditUserId { get; set; }
		public DateTime EditDate { get; set; }
		public string ContentBody { get; set; }
		public DateTime GoLiveDate { get; set; }
		public DateTime RetireDate { get; set; }
		public int? VersionCount { get; set; }

		public Guid? Heartbeat_UserId { get; set; }
		public DateTime? EditHeartbeat { get; set; }

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

		internal ContentSnippet(vw_carrot_ContentSnippet c) {
			if (c != null) {
				SiteData site = SiteData.GetSiteFromCache(c.SiteID);

				this.Root_ContentSnippetID = c.Root_ContentSnippetID;
				this.SiteID = c.SiteID;
				this.ContentSnippetID = c.ContentSnippetID;
				this.ContentSnippetName = c.ContentSnippetName;
				this.ContentSnippetSlug = c.ContentSnippetSlug;
				this.ContentSnippetActive = c.ContentSnippetActive;
				this.IsLatestVersion = c.IsLatestVersion;
				this.ContentBody = c.ContentBody;

				this.CreateUserId = c.CreateUserId;
				this.CreateDate = site.ConvertUTCToSiteTime(c.CreateDate);
				this.EditUserId = c.EditUserId;
				this.EditDate = site.ConvertUTCToSiteTime(c.EditDate);
				this.GoLiveDate = site.ConvertUTCToSiteTime(c.GoLiveDate);
				this.RetireDate = site.ConvertUTCToSiteTime(c.RetireDate);

				this.Heartbeat_UserId = c.Heartbeat_UserId;
				this.EditHeartbeat = c.EditHeartbeat;

				this.VersionCount = c.VersionCount;
			}
		}

		public void ResetHeartbeatLock() {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				carrot_RootContentSnippet rc = CompiledQueries.cqGetSnippetDataTbl(db, this.SiteID, this.Root_ContentSnippetID);

				rc.EditHeartbeat = DateTime.UtcNow.AddHours(-2);
				rc.Heartbeat_UserId = null;
				db.SubmitChanges();
			}
		}

		public void RecordHeartbeatLock(Guid currentUserID) {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				carrot_RootContentSnippet rc = CompiledQueries.cqGetSnippetDataTbl(db, this.SiteID, this.Root_ContentSnippetID);

				rc.Heartbeat_UserId = currentUserID;
				rc.EditHeartbeat = DateTime.UtcNow;

				db.SubmitChanges();
			}
		}

		public bool IsSnippetLocked() {
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

		public bool IsSnippetLocked(Guid currentUserID) {
			bool bLock = false;
			if (this.Heartbeat_UserId != null) {
				if (this.Heartbeat_UserId != currentUserID
						&& this.EditHeartbeat.Value > DateTime.UtcNow.AddMinutes(-2)) {
					bLock = true;
				}
				if (this.Heartbeat_UserId == currentUserID
					|| this.Heartbeat_UserId == null) {
					bLock = false;
				}
			}
			return bLock;
		}

		public bool RecordSnippetLock(Guid currentUserID) {
			bool bLock = IsSnippetLocked();
			bool bRet = false;

			if (!bLock) {
				ExtendedUserData usr = new ExtendedUserData(currentUserID);

				//only allow admin/editors to record a lock
				if (usr.IsAdmin || usr.IsEditor) {
					bRet = true;
					RecordHeartbeatLock(currentUserID);
				}
			}

			return bRet;
		}

		public Guid GetCurrentEditUser() {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				carrot_RootContentSnippet rc = CompiledQueries.cqGetSnippetDataTbl(db, this.SiteID, this.Root_ContentSnippetID);

				if (rc != null) {
					return (Guid)rc.Heartbeat_UserId;
				} else {
					return Guid.Empty;
				}
			}
		}

		public ExtendedUserData GetCurrentEditUserData() {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				carrot_RootContentSnippet rc = CompiledQueries.cqGetSnippetDataTbl(db, this.SiteID, this.Root_ContentSnippetID);

				if (rc != null) {
					return new ExtendedUserData((Guid)rc.Heartbeat_UserId);
				} else {
					return null;
				}
			}
		}

		public List<ContentSnippet> GetHistory() {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				List<ContentSnippet> _types = (from d in CompiledQueries.cqGetSnippetVersionHistory(db, this.Root_ContentSnippetID)
											   select new ContentSnippet(d)).ToList();

				return _types;
			}
		}

		public void Delete() {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				carrot_RootContentSnippet s = CompiledQueries.cqGetSnippetDataTbl(db, this.SiteID, this.Root_ContentSnippetID);

				if (s != null) {
					IQueryable<carrot_ContentSnippet> lst = (from m in db.carrot_ContentSnippets
															 where m.Root_ContentSnippetID == s.Root_ContentSnippetID
															 select m);

					db.carrot_ContentSnippets.BatchDelete(lst);
					db.carrot_RootContentSnippets.DeleteOnSubmit(s);
					db.SubmitChanges();
				}
			}
		}

		public void DeleteThisVersion() {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				carrot_RootContentSnippet s = CompiledQueries.cqGetSnippetDataTbl(db, this.SiteID, this.Root_ContentSnippetID);

				if (s != null) {
					IQueryable<carrot_ContentSnippet> lst = (from m in db.carrot_ContentSnippets
															 where m.ContentSnippetID == this.ContentSnippetID
																&& m.Root_ContentSnippetID == s.Root_ContentSnippetID
																&& m.IsLatestVersion != true
															 select m);

					db.carrot_ContentSnippets.BatchDelete(lst);
					db.SubmitChanges();
				}
			}
		}

		public void Save() {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				SiteData site = SiteData.GetSiteFromCache(this.SiteID);

				carrot_RootContentSnippet rc = CompiledQueries.cqGetSnippetDataTbl(db, this.SiteID, this.Root_ContentSnippetID);

				carrot_ContentSnippet oldC = CompiledQueries.cqGetLatestSnippetContentTbl(db, this.SiteID, this.Root_ContentSnippetID);

				bool bNew = false;

				if (rc == null) {
					rc = new carrot_RootContentSnippet();
					rc.Root_ContentSnippetID = Guid.NewGuid();
					rc.SiteID = site.SiteID;

					rc.CreateDate = DateTime.UtcNow;
					if (this.CreateUserId != Guid.Empty) {
						rc.CreateUserId = this.CreateUserId;
					} else {
						rc.CreateUserId = SecurityData.CurrentUserGuid;
					}

					db.carrot_RootContentSnippets.InsertOnSubmit(rc);
					bNew = true;
				}

				this.ContentSnippetSlug = ContentPageHelper.ScrubSlug(this.ContentSnippetSlug);

				rc.ContentSnippetActive = this.ContentSnippetActive;
				rc.ContentSnippetName = this.ContentSnippetName;
				rc.ContentSnippetSlug = this.ContentSnippetSlug;

				rc.GoLiveDate = site.ConvertSiteTimeToUTC(this.GoLiveDate);
				rc.RetireDate = site.ConvertSiteTimeToUTC(this.RetireDate);

				carrot_ContentSnippet c = new carrot_ContentSnippet();
				c.ContentSnippetID = Guid.NewGuid();
				c.Root_ContentSnippetID = rc.Root_ContentSnippetID;
				c.IsLatestVersion = true;

				if (!bNew) {
					oldC.IsLatestVersion = false;
				}

				c.EditDate = DateTime.UtcNow;
				if (this.EditUserId != Guid.Empty) {
					c.EditUserId = this.EditUserId;
				} else {
					c.EditUserId = SecurityData.CurrentUserGuid;
				}

				c.ContentBody = this.ContentBody;

				rc.Heartbeat_UserId = c.EditUserId;
				rc.EditHeartbeat = DateTime.UtcNow;

				db.carrot_ContentSnippets.InsertOnSubmit(c);

				db.SubmitChanges();

				var priorVersions = db.carrot_ContentSnippets.Where(x => x.IsLatestVersion == true
										&& x.ContentSnippetID != c.ContentSnippetID
										&& x.Root_ContentSnippetID == c.Root_ContentSnippetID);

				// make sure if there are any stray active rows besides the new one, mark them inactive
				if (priorVersions != null && priorVersions.Any()) {
					db.carrot_ContentSnippets.BatchUpdate(priorVersions, p => new carrot_ContentSnippet { IsLatestVersion = false });
					db.SubmitChanges();
				}

				this.ContentSnippetID = c.ContentSnippetID;
				this.Root_ContentSnippetID = rc.Root_ContentSnippetID;
			}
		}

		public static int GetSimilar(Guid siteID, Guid rootSnippetID, string categorySlug) {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<vw_carrot_ContentSnippet> query = CompiledQueries.cqGetContentSnippetNoMatch(db, siteID, rootSnippetID, categorySlug);

				return query.Count();
			}
		}

		public static ContentSnippet Get(Guid rootSnippetID) {
			ContentSnippet _item = null;
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				vw_carrot_ContentSnippet query = CompiledQueries.cqGetLatestSnippetVersion(db, rootSnippetID);
				if (query != null) {
					_item = new ContentSnippet(query);
				}
			}

			return _item;
		}

		public static ContentSnippet GetSnippetByID(Guid siteID, Guid rootSnippetID, bool bActiveOnly) {
			ContentSnippet _item = null;
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				vw_carrot_ContentSnippet query = CompiledQueries.GetLatestContentSnippetByID(db, siteID, bActiveOnly, rootSnippetID);
				if (query != null) {
					_item = new ContentSnippet(query);
				}
			}

			return _item;
		}

		public static ContentSnippet GetSnippetBySlug(Guid siteID, string categorySlug, bool bActiveOnly) {
			ContentSnippet _item = null;
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				vw_carrot_ContentSnippet query = CompiledQueries.GetLatestContentSnippetBySlug(db, siteID, bActiveOnly, categorySlug);
				if (query != null) {
					_item = new ContentSnippet(query);
				}
			}

			return _item;
		}

		public static ContentSnippet GetVersion(Guid snippetDataID) {
			ContentSnippet _item = null;
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				vw_carrot_ContentSnippet query = CompiledQueries.cqGetSnippetVersionByID(db, snippetDataID);
				if (query != null) {
					_item = new ContentSnippet(query);
				}
			}

			return _item;
		}

		public static List<ContentSnippet> GetHistory(Guid rootSnippetID) {
			using (var db = CarrotCMSDataContext.GetDataContext()) {
				List<ContentSnippet> _types = (from d in CompiledQueries.cqGetSnippetVersionHistory(db, rootSnippetID)
											   select new ContentSnippet(d)).ToList();

				return _types;
			}
		}
	}
}