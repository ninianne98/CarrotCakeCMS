using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
	public class SiteNavHelper : IDisposable, ISiteNavHelper {

		private ISiteNavHelper _navHelper = null;

		public SiteNavHelper() {
			if (SiteData.IsPageSampler) {
				_navHelper = new SiteNavHelperMock();
			} else {
				_navHelper = new SiteNavHelperReal();
			}
		}


		#region ISiteNavHelper Members

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {
			return _navHelper.FindByFilename(siteID, urlFileName);
		}

		public SiteNav FindHome(Guid siteID) {
			return _navHelper.FindHome(siteID);
		}

		public SiteNav FindHome(Guid siteID, bool? active) {
			return _navHelper.FindHome(siteID, active);
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid ParentID, bool bActiveOnly) {
			return _navHelper.GetChildNavigation(siteID, ParentID, bActiveOnly);
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sParentID, bool bActiveOnly) {
			return _navHelper.GetChildNavigation(siteID, sParentID, bActiveOnly);
		}

		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {
			return _navHelper.GetLatest(siteID, iUpdates, bActiveOnly);
		}

		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {
			return _navHelper.GetLatestVersion(siteID, rootContentID);
		}

		public SiteNav GetLatestVersion(Guid siteID, bool? active, string sPage) {
			return _navHelper.GetLatestVersion(siteID, active, sPage);
		}

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {
			return _navHelper.GetMasterNavigation(siteID, bActiveOnly);
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {
			return _navHelper.GetPageNavigation(siteID, rootContentID);
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {
			return _navHelper.GetPageNavigation(siteID, sPage);
		}

		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID) {
			return _navHelper.GetParentPageNavigation(siteID, rootContentID);
		}

		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {
			return _navHelper.GetParentPageNavigation(siteID, sPage);
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {
			return _navHelper.GetSiblingNavigation(siteID, PageID, bActiveOnly);
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {
			return _navHelper.GetSiblingNavigation(siteID, sPage, bActiveOnly);
		}

		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			return _navHelper.GetTopNavigation(siteID, bActiveOnly);
		}

		public List<SiteNav> GetTwoLevelNavigation(Guid siteID, bool bActiveOnly) {
			return _navHelper.GetTwoLevelNavigation(siteID, bActiveOnly);
		}

		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, Guid rootContentID, bool bActiveOnly) {
			return _navHelper.GetPageCrumbNavigation(siteID, rootContentID, bActiveOnly);
		}

		public List<SiteNav> GetPageCrumbNavigation(Guid siteID, string sPage, bool bActiveOnly) {
			return _navHelper.GetPageCrumbNavigation(siteID, sPage, bActiveOnly);
		}

		#endregion

		public static List<string> GetSiteDirectoryPaths() {
			List<string> lstContent = null;

			using (CarrotCMSDataContext _db = new CarrotCMSDataContext()) {

				lstContent = (from ct in _db.vw_carrot_Contents
							  where ct.IsLatestVersion == true
								  && ct.FileName.ToLower().EndsWith(SiteData.DefaultDirectoryFilename)
							  select ct.FileName.ToLower()).Distinct().ToList();
			}

			return lstContent;
		}

		public static SiteNav GetEmptyHome() {
			SiteNav navData = new SiteNav();
			navData.ContentID = Guid.Empty;
			navData.Root_ContentID = Guid.Empty;
			navData.SiteID = SiteData.CurrentSiteID;
			navData.TemplateFile = SiteData.DefaultDirectoryFilename;
			navData.FileName = SiteData.DefaultDirectoryFilename;
			navData.NavMenuText = "NONE";
			navData.PageHead = "NONE";
			navData.TitleBar = "NONE";
			navData.PageActive = false;
			navData.PageText = "NO PAGE CONTENT";
			navData.EditDate = DateTime.Now.Date.AddMinutes(-15);
			navData.CreateDate = DateTime.Now.Date.AddMinutes(-30);

			return navData;
		}

		public static List<SiteNav> GetSamplerFakeNav() {
			return GetSamplerFakeNav(null);
		}

		public static List<SiteNav> GetSamplerFakeNav(Guid? rootParentID) {
			List<SiteNav> navList = new List<SiteNav>();
			int n = 0;
			int nMax = 3;

			while (n < nMax) {
				SiteNav nav = GetSamplerView();
				nav.NavOrder = n;
				nav.NavMenuText = nav.NavMenuText + " " + n.ToString();

				if (n > 0 || rootParentID != null) {
					nav.Root_ContentID = Guid.NewGuid();
					nav.ContentID = Guid.NewGuid();
					//nav.FileName = nav.FileName.Replace(".aspx", nav.NavOrder.ToString() + ".aspx");
					nav.FileName = "/#";
					if (rootParentID != null) {
						nav.NavMenuText = nav.NavMenuText + " - " + rootParentID.Value.ToString().Substring(0, 4);
					}
				}
				nav.Parent_ContentID = rootParentID;

				navList.Add(nav);
				n++;
			}

			return navList;
		}

		public static SiteNav GetSamplerView(Guid rootParentID) {
			var sn = GetSamplerView();

			sn.Parent_ContentID = rootParentID;

			return sn;

		}

		public static SiteNav GetSamplerView() {

			string sFile2 = "";

			Assembly _assembly = Assembly.GetExecutingAssembly();

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.Core.SiteContent.Mock.SampleContent2.txt"))) {
				sFile2 = oTextStream.ReadToEnd();
			}

			List<string> imageNames = (from i in _assembly.GetManifestResourceNames()
									   where i.Contains("SiteContent.Mock.sample")
									   && i.EndsWith(".png")
									   select i).ToList();

			foreach (string img in imageNames) {
				var imgURL = CMSConfigHelper.GetWebResourceUrl(typeof(SiteNav), img);
				sFile2 = sFile2.Replace(img, imgURL);
			}

			SiteNav navNew = new SiteNav();
			navNew.Root_ContentID = SiteData.CurrentSiteID;
			navNew.ContentID = Guid.NewGuid();
			navNew.SiteID = SiteData.CurrentSiteID;
			navNew.Parent_ContentID = null;

			navNew.PageText = "<h2>Content CENTER</h2>\r\n" + sFile2;

			navNew.NavOrder = -1;
			navNew.TitleBar = "Template Preview - TITLE";
			navNew.NavMenuText = "Template PV - NAV";
			navNew.PageHead = "Template Preview - HEAD";
			navNew.PageActive = true;
			navNew.EditDate = DateTime.Now.AddMinutes(-30);
			navNew.CreateDate = DateTime.Today.AddDays(-1);

			navNew.TemplateFile = SiteData.PreviewTemplateFile;
			navNew.FileName = SiteData.PreviewTemplateFilePage + "?" + HttpContext.Current.Request.QueryString.ToString();

			return navNew;
		}


		#region IDisposable Members

		public void Dispose() {
			if (_navHelper is IDisposable) {
				((IDisposable)_navHelper).Dispose();
			}
		}

		#endregion
	}
}
