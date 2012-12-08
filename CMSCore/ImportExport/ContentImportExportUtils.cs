using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
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
	public class ContentImportExportUtils {

		public static string keyPageImport = "cmsContentPageExport";

		public static void AssignContentPageExportNewIDs(ContentPageExport cpe) {
			cpe.NewRootContentID = Guid.NewGuid();

			cpe.ThePage.Root_ContentID = cpe.NewRootContentID;
			cpe.ThePage.ContentID = cpe.NewRootContentID;
			cpe.ThePage.SiteID = SiteData.CurrentSiteID;
			cpe.ThePage.EditUserId = SecurityData.CurrentUserGuid;
			//cpe.ThePage.CreateDate = DateTime.Now;
			cpe.ThePage.EditDate = DateTime.Now;

			foreach (var w in cpe.ThePageWidgets) {
				w.Root_ContentID = cpe.NewRootContentID;
				w.Root_WidgetID = Guid.NewGuid();
				w.WidgetDataID = Guid.NewGuid();
			}
		}


		public static void AssignSiteExportNewIDs(SiteExport se) {
			se.NewSiteID = Guid.NewGuid();

			se.TheSite.SiteID = se.NewSiteID;

			foreach (var p in se.ThePages) {
				AssignContentPageExportNewIDs(p);
			}
		}

		public static void AssignWPExportNewIDs(SiteData sd, WordPressSite wps) {
			wps.NewSiteID = Guid.NewGuid();

			wps.Content.Where(p => p.PostType == WordPressPost.WPPostType.BlogPost).ToList()
				.ForEach(q => q.ImportFileName = ("/" + q.PostDate.ToString(sd.Blog_DatePattern) + "/" + q.ImportFileSlug));

			wps.Content.ToList().ForEach(r => r.ImportFileName = r.ImportFileName.Replace("//", "/"));

		}

		public static ContentPage CreateWPContentPage(SiteData site, WordPressPost c) {
			ContentPage cont = null;

			if (c != null) {
				cont = new ContentPage();
				cont.SiteID = site.SiteID;
				cont.ContentID = Guid.NewGuid();

				cont.EditUserId = SecurityData.CurrentUserGuid;
				cont.EditDate = DateTime.Now;
				cont.TemplateFile = SiteData.DefaultTemplateFilename;

				cont.Root_ContentID = c.ImportRootID;
				cont.FileName = c.ImportFileName.Replace("//", "/");
				cont.PageSlug = null;
				cont.NavOrder = c.PostOrder;
				cont.Parent_ContentID = c.ImportParentRootID;

				cont.CreateDate = c.PostDate;
				cont.PageActive = c.IsPublished;
				cont.ContentType = ContentPageType.PageType.Unknown;

				if (c.PostType == WordPressPost.WPPostType.BlogPost) {
					cont.ContentType = ContentPageType.PageType.BlogEntry;
					cont.PageSlug = c.ImportFileSlug.Replace("//", "/");
					cont.NavOrder = 10;
					cont.Parent_ContentID = null;
				}
				if (c.PostType == WordPressPost.WPPostType.Page) {
					cont.ContentType = ContentPageType.PageType.ContentEntry;
				}

				cont.IsLatestVersion = true;
				cont.TitleBar = c.PostTitle;
				cont.NavMenuText = c.PostTitle;
				cont.PageHead = c.PostTitle;
				cont.PageText = c.PostContent;
				cont.LeftPageText = "";
				cont.RightPageText = "";
				
				cont.MetaDescription = "";
				cont.MetaKeyword = "";

				cont.ContentCategories = new List<ContentCategory>();
				cont.ContentTags = new List<ContentTag>();

				foreach (string t in c.Categories) {
					ContentCategory e = site.GetCategoryList().Where(x => x.CategorySlug.ToLower() == t.ToLower()).FirstOrDefault();
					if (e != null) {
						cont.ContentCategories.Add(e);
					}
				}
				foreach (string t in c.Tags) {
					ContentTag e = site.GetTagList().Where(x => x.TagSlug.ToLower() == t.ToLower()).FirstOrDefault();
					if (e != null) {
						cont.ContentTags.Add(e);
					}
				}
			}

			return cont;
		}


		public static SiteExport GetExportSite(Guid siteID) {
			SiteExport site = new SiteExport(siteID);

			return site;
		}

		public static List<ContentPageExport> ExportSitePages(Guid siteID) {
			List<ContentPageExport> lst = null;
			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				var lst1 = (from c in pageHelper.GetAllLatestContentList(siteID)
							select new ContentPageExport(c, c.GetWidgetList())).ToList();

				var lst2 = (from c in pageHelper.GetAllLatestBlogList(siteID)
							select new ContentPageExport(c, c.GetWidgetList())).ToList();

				lst = lst1.Union(lst2).ToList();
			}

			return lst;
		}

		public static ContentPageExport GetExportPage(Guid siteID, Guid rootContentID) {
			ContentPageExport cpe = new ContentPageExport(siteID, rootContentID);

			return cpe;
		}

		public static string GetExportXML<T>(T cpe) {

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			string sXML = "";
			using (StringWriter stringWriter = new StringWriter()) {
				xmlSerializer.Serialize(stringWriter, cpe);
				sXML = stringWriter.ToString();
			}

			return sXML;
		}

		public static string GetContentPageExportXML(Guid siteID, Guid rootContentID) {
			ContentPageExport exp = GetExportPage(siteID, rootContentID);

			return GetExportXML<ContentPageExport>(exp);
		}

		public static string GetContentPageExportXML(Guid siteID) {
			SiteExport exp = GetExportSite(siteID);

			return GetExportXML<SiteExport>(exp);
		}

		public static void RemoveSerializedExportData(Guid rootContentID) {
			CMSConfigHelper.ClearSerialized(rootContentID, keyPageImport);
		}

		public static ContentPageExport GetSerializedContentPageExport(Guid rootContentID) {
			ContentPageExport c = null;
			try {
				string sXML = CMSConfigHelper.GetSerialized(rootContentID, keyPageImport);
				c = GetSerialData<ContentPageExport>(sXML) as ContentPageExport;
			} catch (Exception ex) { }
			return c;
		}
		public static SiteExport GetSerializedSiteExport(Guid siteID) {
			SiteExport c = null;
			try {
				string sXML = CMSConfigHelper.GetSerialized(siteID, keyPageImport);
				c = GetSerialData<SiteExport>(sXML) as SiteExport;
			} catch (Exception ex) { }
			return c;
		}
		public static WordPressSite GetSerializedWPExport(Guid siteID) {
			WordPressSite c = null;
			try {
				string sXML = CMSConfigHelper.GetSerialized(siteID, keyPageImport);
				c = GetSerialData<WordPressSite>(sXML) as WordPressSite;
			} catch (Exception ex) { }
			return c;
		}


		public static ContentPageExport DeserializeContentPageExport(string sXML) {
			ContentPageExport c = GetSerialData<ContentPageExport>(sXML) as ContentPageExport;
			return c;
		}
		public static SiteExport DeserializeSiteExport(string sXML) {
			SiteExport c = GetSerialData<SiteExport>(sXML) as SiteExport;
			return c;
		}
		public static WordPressSite DeserializeWPExport(string sXML) {
			WPBlogReader wbp = new WPBlogReader();
			XmlDocument doc = wbp.LoadText(sXML);
			WordPressSite site = wbp.ParseDoc(doc);
			return site;
		}


		public static void SaveSerializedDataExport<T>(Guid guidKey, T theData) {

			if (theData == null) {
				CMSConfigHelper.ClearSerialized(guidKey, keyPageImport);
			} else {
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				string sXML = "";
				using (StringWriter stringWriter = new StringWriter()) {
					xmlSerializer.Serialize(stringWriter, theData);
					sXML = stringWriter.ToString();
				}
				CMSConfigHelper.SaveSerialized(guidKey, keyPageImport, sXML);
			}
		}

		private static Object GetSerialData<T>(string sXML) {
			Object obj = null;
			try {
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

				using (StringReader stringReader = new StringReader(sXML)) {
					obj = xmlSerializer.Deserialize(stringReader);
				}
			} catch (Exception ex) { }
			return obj;
		}

	}
}
