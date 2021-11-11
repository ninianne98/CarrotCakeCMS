using Carrotware.CMS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

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

	public class ContentLocalTime {
		public DateTime GoLiveDate { get; set; }

		public DateTime GoLiveDateLocal { get; set; }
	}

	//===============================
	public class BlogPostPageUrl {
		public string PostPrefix { get; set; }

		public DateTime GoLiveDate { get; set; }

		public DateTime GoLiveDateLocal { get; set; }
	}

	//===============================
	public class TimeZoneContent {
		public List<ContentLocalTime> ContentLocalDates { get; set; }

		public List<BlogPostPageUrl> BlogPostUrls { get; set; }

		public Guid SiteID { get; set; }

		public TimeZoneContent() {
			this.ContentLocalDates = new List<ContentLocalTime>();
			this.BlogPostUrls = new List<BlogPostPageUrl>();
		}

		public TimeZoneContent(Guid siteID) {
			// use C# libraries for timezones rather than pass in offset as some dates are +/- an hour off because of DST

			this.SiteID = siteID;

			this.ContentLocalDates = new List<ContentLocalTime>();

			this.BlogPostUrls = new List<BlogPostPageUrl>();

			SiteData site = SiteData.GetSiteFromCache(siteID);

			List<carrot_RootContent> queryAllContent = null;

			using (CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext()) {
				queryAllContent = CannedQueries.GetAllRootTbl(db, siteID).ToList();
			}

			var allContentDates = (from p in queryAllContent
								   select p.GoLiveDate).Distinct().ToList();

			var blogDateList = (from p in queryAllContent
								where p.ContentTypeID == ContentPageType.GetIDByType(ContentPageType.PageType.BlogEntry)
								select p.GoLiveDate).Distinct().ToList();

			this.ContentLocalDates = (from d in allContentDates
									  select new ContentLocalTime() {
										  GoLiveDate = d,
										  GoLiveDateLocal = site.ConvertUTCToSiteTime(d)
									  }).ToList();

			this.BlogPostUrls = (from bd in blogDateList
								 join ld in this.ContentLocalDates on bd equals ld.GoLiveDate
								 select new BlogPostPageUrl() {
									 GoLiveDate = ld.GoLiveDate,
									 PostPrefix = CleanPostPrefix(ContentPageHelper.CreateFileNameFromSlug(siteID, ld.GoLiveDateLocal, string.Empty)),
									 GoLiveDateLocal = ld.GoLiveDateLocal
								 }).ToList();
		}

		private string CleanPostPrefix(string pathIn) {
			string pathOut = pathIn;

			if (!string.IsNullOrEmpty(pathIn) &&
					pathIn.ToLowerInvariant().EndsWith(SiteData.DefaultDirectoryFilename.ToLowerInvariant())) {
				pathOut = pathIn.Substring(0, pathIn.Length - SiteData.DefaultDirectoryFilename.Length);
			}

			return pathOut;
		}

		public void Save() {
			using (CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext()) {
				string xml = this.GetXml();

				xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

				db.carrot_UpdateGoLiveLocal(this.SiteID, XElement.Parse(xml));
			}
		}

		public string GetXml() {
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TimeZoneContent));
			string sXML = "";
			using (StringWriter stringWriter = new StringWriter()) {
				xmlSerializer.Serialize(stringWriter, this);
				sXML = stringWriter.ToString();
			}
			return sXML;
		}
	}
}