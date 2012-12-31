﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
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
	public class SiteMapHelper {

		public SiteMapHelper() { }

		public void RenderSiteMap(HttpContext context) {
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;

			SiteData site = SiteData.CurrentSite;
			List<SiteNav> lstNav = new List<SiteNav>();

			using (SiteNavHelper navHelper = new SiteNavHelper()) {
				//lstNav = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, true);
				lstNav = navHelper.GetLevelDepthNavigation(SiteData.CurrentSiteID, 4, true);
			}

			DateTime dtMax = lstNav.Min(x => x.EditDate);
			string DateFormat = "yyyy-MM-dd";

			response.Buffer = false;
			response.Clear();
			response.ContentType = "application/xml";

			XmlWriter writer = XmlWriter.Create(response.Output);

			writer.WriteStartDocument();
			writer.WriteRaw("\n");
			writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
			writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
			writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9    http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

			writer.WriteRaw("\n");
			writer.WriteStartElement("url");
			writer.WriteElementString("loc", site.MainURL);
			writer.WriteElementString("lastmod", dtMax.ToString(DateFormat));
			writer.WriteElementString("priority", "1.0");
			writer.WriteEndElement();
			writer.WriteRaw("\n");

			// always, hourly, daily, weekly, monthly, yearly, never

			foreach (SiteNav n in lstNav) {
				writer.WriteStartElement("url");
				writer.WriteElementString("loc", site.ConstructedCanonicalURL(n));
				writer.WriteElementString("lastmod", n.EditDate.ToString(DateFormat));
				writer.WriteElementString("changefreq", "weekly");
				writer.WriteElementString("priority", n.Parent_ContentID.HasValue ? "0.60" : "0.80");
				writer.WriteEndElement();
				writer.WriteRaw("\n");
			}

			writer.WriteEndDocument();

			writer.Flush();
			writer.Close();

		}

	}
}
