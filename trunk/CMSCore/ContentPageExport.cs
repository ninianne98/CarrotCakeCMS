using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Data;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
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

	public class ContentPageExport {

		public ContentPageExport() {
			ThePage = new ContentPage();
			ThePageWidgets = new List<Widget>();
		}

		public ContentPageExport(Guid rootContentID) {
			ContentPage cp = null;
			List<Widget> widgets = null;

			using (ContentPageHelper cph = new ContentPageHelper()) {
				cp = cph.GetLatestContent(SiteData.CurrentSiteID, rootContentID);
			}

			using (WidgetHelper pwh = new WidgetHelper()) {
				widgets = pwh.GetWidgets(rootContentID, null);
			}

			SetVals(cp, widgets);
		}

		public ContentPageExport(ContentPage cp, List<Widget> widgets) {

			SetVals(cp, widgets);
		}

		private void SetVals(ContentPage cp, List<Widget> widgets) {

			NewRootContentID = Guid.NewGuid();

			ThePage = cp;
			ThePageWidgets = widgets;

			if (ThePage == null) {
				ThePage = new ContentPage();
				ThePage.Root_ContentID = Guid.NewGuid();
				ThePage.ContentID = ThePage.Root_ContentID;
			}
			if (ThePageWidgets == null) {
				ThePageWidgets = new List<Widget>();
			}

			OriginalRootContentID = ThePage.Root_ContentID;
			OriginalSiteID = ThePage.SiteID;
			OriginalParentContentID = Guid.Empty;
			ParentFileName = "";

			if (ThePage.Parent_ContentID != null) {
				var parent = new ContentPage();
				using (ContentPageHelper cph = new ContentPageHelper()) {
					parent = cph.GetLatestContent(SiteData.CurrentSiteID, (Guid)ThePage.Parent_ContentID);
				}
				ParentFileName = parent.FileName;
				OriginalParentContentID = parent.Root_ContentID;
			}

			ThePage.Root_ContentID = NewRootContentID;
			ThePage.ContentID = NewRootContentID;
			ThePage.CreateDate = DateTime.Now;

			foreach (var w in ThePageWidgets) {
				w.Root_ContentID = NewRootContentID;
				w.Root_WidgetID = Guid.NewGuid();
				w.WidgetDataID = Guid.NewGuid();
			}
		}

		public Guid NewRootContentID { get; set; }

		public Guid OriginalRootContentID { get; set; }

		public Guid OriginalSiteID { get; set; }

		public Guid OriginalParentContentID { get; set; }

		public string ParentFileName { get; set; }

		public ContentPage ThePage { get; set; }

		public List<Widget> ThePageWidgets { get; set; }


		public static string keyPageImport = "cmsContentPageExport";


		public static void AssignNewIDs(ContentPageExport cpe) {
			cpe.NewRootContentID = Guid.NewGuid();

			cpe.ThePage.Root_ContentID = cpe.NewRootContentID;
			cpe.ThePage.ContentID = cpe.NewRootContentID;
			cpe.ThePage.SiteID = SiteData.CurrentSiteID;
			cpe.ThePage.EditUserId = SecurityData.CurrentUserGuid;
			cpe.ThePage.CreateDate = DateTime.Now;

			foreach (var w in cpe.ThePageWidgets) {
				w.Root_ContentID = cpe.NewRootContentID;
				w.Root_WidgetID = Guid.NewGuid();
				w.WidgetDataID = Guid.NewGuid();
			}
		}


		public static ContentPageExport GetExportPage(Guid rootContentID) {
			ContentPageExport cpe = new ContentPageExport(rootContentID);

			return cpe;
		}

		public static string GetExportXML(ContentPageExport cpe) {

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPageExport));
			string sXML = "";
			using (StringWriter stringWriter = new StringWriter()) {
				xmlSerializer.Serialize(stringWriter, cpe);
				sXML = stringWriter.ToString();
			}

			return sXML;
		}

		public static string GetExportXML(Guid rootContentID) {
			ContentPageExport cpe = GetExportPage(rootContentID);

			return GetExportXML(cpe);
		}

		public static void RemoveSerializedContentPageExport(Guid rootContentID) {
			CMSConfigHelper.ClearSerialized(rootContentID, keyPageImport);
		}

		public static void SaveSerializedContentPageExport(ContentPageExport cpe) {

			if (cpe == null) {
				CMSConfigHelper.ClearSerialized(cpe.ThePage.Root_ContentID, keyPageImport);
			} else {
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPageExport));
				string sXML = "";
				using (StringWriter stringWriter = new StringWriter()) {
					xmlSerializer.Serialize(stringWriter, cpe);
					sXML = stringWriter.ToString();
				}
				CMSConfigHelper.SaveSerialized(cpe.ThePage.Root_ContentID, keyPageImport, sXML);
			}
		}

		public static ContentPageExport GetSerializedContentPageExport(Guid rootContentID) {

			ContentPageExport c = null;
			try {
				var sXML = CMSConfigHelper.GetSerialized(rootContentID, keyPageImport);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPageExport));
				Object genpref = null;
				using (StringReader stringReader = new StringReader(sXML)) {
					genpref = xmlSerializer.Deserialize(stringReader);
				}
				c = genpref as ContentPageExport;
			} catch (Exception ex) { }
			return c;
		}

		public static ContentPageExport GetSerializedContentPageExport(string sData) {
			ContentPageExport c = null;
			try {
				var sXML = sData;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPageExport));
				Object genpref = null;
				using (StringReader stringReader = new StringReader(sXML)) {
					genpref = xmlSerializer.Deserialize(stringReader);
				}
				c = genpref as ContentPageExport;
			} catch (Exception ex) { }
			return c;
		}

	}
}
