using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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

	public class SiteExport {

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

			pages = ContentImportExportUtils.ExportSitePages(siteID);

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

		public string CarrotCakeVersion { get; set; }

		public DateTime ExportDate { get; set; }

		public Guid NewSiteID { get; set; }

		public Guid OriginalSiteID { get; set; }

		public SiteData TheSite { get; set; }

		public List<ContentPageExport> ThePages { get; set; }

		public List<ContentCategory> TheCategories { get; set; }

		public List<ContentTag> TheTags { get; set; }
	}
}
