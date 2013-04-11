﻿using System;
using System.Collections.Generic;
using System.Linq;
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

	public class SiteExport {

		public enum ExportType {
			BlogData,
			ContentData,
			AllData,
		}

		public SiteExport() {
			CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;
			ExportDate = DateTime.UtcNow;

			TheSite = new SiteData();
			ThePages = new List<ContentPageExport>();

			TheCategories = new List<ContentCategory>();
			TheTags = new List<ContentTag>();
		}

		public SiteExport(Guid siteID) {
			SiteData s = null;
			List<ContentPageExport> pages = null;

			s = SiteData.GetSiteByID(siteID);

			pages = ContentImportExportUtils.ExportAllSiteContent(siteID);

			SetVals(s, pages);
		}

		public SiteExport(Guid siteID, ExportType exportWhat) {
			SiteData s = null;
			List<ContentPageExport> pages = new List<ContentPageExport>();

			s = SiteData.GetSiteByID(siteID);

			if (exportWhat == ExportType.AllData) {
				pages = ContentImportExportUtils.ExportAllSiteContent(siteID);
			} else {
				if (exportWhat == ExportType.ContentData) {
					List<ContentPageExport> lst = ContentImportExportUtils.ExportPages(siteID);
					pages = pages.Union(lst).ToList();
				}
				if (exportWhat == ExportType.BlogData) {
					List<ContentPageExport> lst = ContentImportExportUtils.ExportPosts(siteID);
					pages = pages.Union(lst).ToList();
				}
			}

			SetVals(s, pages);
		}

		public SiteExport(SiteData s, List<ContentPageExport> pages) {

			SetVals(s, pages);
		}

		private void SetVals(SiteData s, List<ContentPageExport> pages) {
			CarrotCakeVersion = SiteData.CarrotCakeCMSVersion;
			ExportDate = DateTime.UtcNow;

			NewSiteID = Guid.NewGuid();

			TheSite = s;
			ThePages = pages;

			if (TheSite == null) {
				TheSite = new SiteData();
				TheSite.SiteID = Guid.NewGuid();
			}
			if (ThePages == null) {
				ThePages = new List<ContentPageExport>();
			}

			OriginalSiteID = TheSite.SiteID;

			foreach (var w in ThePages) {
				w.OriginalSiteID = NewSiteID;
			}

			TheCategories = s.GetCategoryList();
			TheTags = s.GetTagList();
		}

		public void LoadComments() {
			if (ThePages != null) {
				TheComments = new List<CommentExport>();
				foreach (ContentPageExport cpe in ThePages) {
					TheComments = TheComments.Union(CommentExport.GetPageCommentExport(cpe.OriginalRootContentID)).ToList();
				}
			}
		}

		public string CarrotCakeVersion { get; set; }

		public DateTime ExportDate { get; set; }

		public Guid NewSiteID { get; set; }

		public Guid OriginalSiteID { get; set; }

		public SiteData TheSite { get; set; }

		public List<ContentPageExport> ThePages { get; set; }


		public List<ContentPageExport> TheContentPages {
			get {
				return (from c in this.ThePages
						where c.ThePage.ContentType == ContentPageType.PageType.ContentEntry
						orderby c.ThePage.NavOrder ascending
						select c).ToList();
			}
		}
		public List<ContentPageExport> TheBlogPages {
			get {
				return (from c in this.ThePages
						where c.ThePage.ContentType == ContentPageType.PageType.BlogEntry
						orderby c.ThePage.CreateDate descending
						select c).ToList();
			}
		}


		public List<CommentExport> TheComments { get; set; }

		public List<ContentCategory> TheCategories { get; set; }

		public List<ContentTag> TheTags { get; set; }
	}
}
