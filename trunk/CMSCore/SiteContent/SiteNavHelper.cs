using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Data;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
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

	public class SiteNavHelper : IDisposable {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteNavHelper() { }

		public List<SiteNav> GetMasterNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = (from ct in db.carrot_Contents
										join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
										orderby ct.NavOrder, ct.NavMenuText
										where r.SiteID == siteID
											&& (r.PageActive == bActiveOnly || bActiveOnly == false)
											&& ct.IsLatestVersion == true
										select new SiteNav(r, ct)).ToList();
			return lstContent;
		}



		public List<SiteNav> GetTopNavigation(Guid siteID, bool bActiveOnly) {
			List<SiteNav> lstContent = null;

			if (SiteData.IsPageSampler) {
				lstContent = GetSamplerFakeNav();
			} else {
				lstContent = (from ct in db.carrot_Contents
							  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							  orderby ct.NavOrder, ct.NavMenuText
							  where r.SiteID == siteID
								  && ct.Parent_ContentID == null
								  && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  && ct.IsLatestVersion == true
							  select new SiteNav(r, ct)).ToList();
			}

			return lstContent;
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, string sParentID, bool bActiveOnly) {

			carrot_RootContent c = (from r in db.carrot_RootContents
									where r.SiteID == siteID
										   && r.FileName.ToLower() == sParentID.ToLower()
									select r).FirstOrDefault();

			List<SiteNav> lstContent = new List<SiteNav>();

			if (c != null) {
				lstContent = (from ct in db.carrot_Contents
							  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							  orderby ct.NavOrder, ct.NavMenuText
							  where r.SiteID == siteID
								  && ct.Parent_ContentID == c.Root_ContentID
								  && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  && ct.IsLatestVersion == true
							  select new SiteNav(r, ct)).ToList();
			}

			if (SiteData.IsPageSampler) {
				lstContent = GetSamplerFakeNav();
			}

			return lstContent;
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, string sPage, bool bActiveOnly) {

			carrot_Content c = (from ct in db.carrot_Contents
								join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
								where r.SiteID == siteID
									   && r.FileName.ToLower() == sPage.ToLower()
									   && ct.IsLatestVersion == true
								select ct).FirstOrDefault();

			List<SiteNav> lstContent = new List<SiteNav>();

			if (c != null) {
				lstContent = (from ct in db.carrot_Contents
							  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							  orderby ct.NavOrder, ct.NavMenuText
							  where r.SiteID == siteID
								  && ct.Parent_ContentID == c.Parent_ContentID
								  && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  && ct.IsLatestVersion == true
							  select new SiteNav(r, ct)).ToList();
			}

			if (SiteData.IsPageSampler) {
				lstContent = GetSamplerFakeNav();
			}

			return lstContent;
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {

			SiteNav content = null;
			if (SiteData.IsPageSampler) {
				content = GetSamplerView();
			} else {
				content = (from ct in db.carrot_Contents
						   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						   where r.SiteID == siteID
							   && r.FileName.ToLower() == sPage.ToLower()
							   && ct.IsLatestVersion == true
						   select new SiteNav(r, ct)).FirstOrDefault();
			}
			return content;
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {

			SiteNav content = null;
			if (SiteData.IsPageSampler) {
				content = GetSamplerView();
			} else {
				content = (from ct in db.carrot_Contents
						   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						   where r.SiteID == siteID
							   && ct.Root_ContentID == rootContentID
							   && ct.IsLatestVersion == true
						   select new SiteNav(r, ct)).FirstOrDefault();
			}
			return content;
		}


		public SiteNav GetParentPageNavigation(Guid siteID, string sPage) {
			SiteNav nav1 = GetPageNavigation(siteID, sPage);

			SiteNav content = null;
			if (nav1 != null) {
				content = (from ct in db.carrot_Contents
						   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						   where r.SiteID == siteID
							   && r.FileName.ToLower() == content.FileName.ToLower()
							   && ct.IsLatestVersion == true
						   select new SiteNav(r, ct)).FirstOrDefault();
			}
			if (SiteData.IsPageSampler) {
				content = GetSamplerView();
			}
			return content;
		}

		public SiteNav GetParentPageNavigation(Guid siteID, Guid rootContentID) {
			SiteNav nav1 = GetPageNavigation(siteID, rootContentID);

			SiteNav content = null;
			if (nav1 != null) {
				content = (from ct in db.carrot_Contents
						   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						   where r.SiteID == siteID
							   && ct.Root_ContentID == nav1.Parent_ContentID
							   && ct.IsLatestVersion == true
						   select new SiteNav(r, ct)).FirstOrDefault();
			}
			if (SiteData.IsPageSampler) {
				content = GetSamplerView();
			}
			return content;
		}


		public static SiteNav GetEmptyHome() {
			SiteNav navData = new SiteNav();
			navData.ContentID = Guid.Empty;
			navData.Root_ContentID = Guid.Empty;
			navData.SiteID = SiteData.CurrentSiteID;
			navData.TemplateFile = SiteData.DefaultDirectoryFilename;
			navData.FileName = SiteData.DefaultDirectoryFilename;
			navData.NavFileName = SiteData.DefaultDirectoryFilename;
			navData.NavMenuText = "NONE";
			navData.PageHead = "NONE";
			navData.TitleBar = "NONE";
			navData.PageActive = false;
			navData.PageText = "NO PAGE CONTENT";
			navData.EditDate = DateTime.Now.Date.AddMinutes(-15);
			navData.CreateDate = DateTime.Now.Date.AddMinutes(-30);

			return navData;
		}

		public List<SiteNav> GetSamplerFakeNav() {
			return GetSamplerFakeNav(null);
		}

		public List<SiteNav> GetSamplerFakeNav(Guid? rootParentID) {
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
					nav.FileName = "#";
					nav.NavFileName = nav.FileName;
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

		public SiteNav GetSamplerView() {

			string sFile2 = "";

			Assembly _assembly = Assembly.GetExecutingAssembly();
			Page p = new Page();

			using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.Core.SiteContent.Mock.SampleContent2.txt"))) {
				sFile2 = oTextStream.ReadToEnd();
			}

			List<string> imageNames = (from i in _assembly.GetManifestResourceNames()
									   where i.Contains("SiteContent.Mock.sample")
									   && i.EndsWith(".png")
									   select i).ToList();

			foreach (string img in imageNames) {
				var imgURL = p.ClientScript.GetWebResourceUrl(this.GetType(), img);
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
			navNew.NavFileName = navNew.FileName;

			return navNew;
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid ParentID, bool bActiveOnly) {
			List<SiteNav> lstContent = null;

			if (SiteData.IsPageSampler) {
				lstContent = GetSamplerFakeNav(ParentID);
			} else {
				lstContent = (from ct in db.carrot_Contents
							  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							  orderby ct.NavOrder, ct.NavMenuText
							  where r.SiteID == siteID
								  && ct.Parent_ContentID == ParentID
								  && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  && ct.IsLatestVersion == true
							  select new SiteNav(r, ct)).ToList();
			}
			return lstContent;
		}

		public List<SiteNav> GetSiblingNavigation(Guid siteID, Guid PageID, bool bActiveOnly) {

			carrot_Content c = (from ct in db.carrot_Contents
								where ct.Root_ContentID == PageID
								   && ct.IsLatestVersion == true
								select ct).FirstOrDefault();

			List<SiteNav> lstContent = new List<SiteNav>();

			if (c != null) {
				lstContent = (from ct in db.carrot_Contents
							  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							  orderby ct.NavOrder, ct.NavMenuText
							  where r.SiteID == siteID
								  && ct.Parent_ContentID == c.Parent_ContentID
								  && (r.PageActive == bActiveOnly || bActiveOnly == false)
								  && ct.IsLatestVersion == true
							  select new SiteNav(r, ct)).ToList();
			}

			if (SiteData.IsPageSampler) {
				lstContent = GetSamplerFakeNav(PageID);
			}

			return lstContent;
		}


		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {

			List<SiteNav> lstContent = null;

			if (SiteData.IsPageSampler) {
				lstContent = GetSamplerFakeNav();
			} else {
				lstContent = (from ct in db.carrot_Contents
							  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							  orderby ct.EditDate descending
							  where r.SiteID == siteID
								  && ct.IsLatestVersion == true
								  && (r.PageActive == bActiveOnly || bActiveOnly == false)
							  select new SiteNav(r, ct)).Take(iUpdates).ToList();
			}

			return lstContent;
		}


		public SiteNav GetLatestVersion(Guid siteID, Guid rootContentID) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && r.Root_ContentID == rootContentID
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}

		public SiteNav GetLatestVersion(Guid siteID, bool? active, string sPage) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && (r.PageActive == active || active == null)
								   && r.FileName.ToLower() == sPage.ToLower()
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}


		public SiteNav FindHome(Guid siteID) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   orderby ct.NavOrder ascending
							   where r.SiteID == siteID
								   && r.PageActive == true
								   && ct.NavOrder < 1
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();
			return content;
		}

		public SiteNav FindByFilename(Guid siteID, string urlFileName) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   where r.SiteID == siteID
								   && ct.IsLatestVersion == true
								   && r.FileName.ToLower() == urlFileName.ToLower()
							   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}

		public SiteNav FindHome(Guid siteID, bool? active) {
			SiteNav content = (from ct in db.carrot_Contents
							   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
							   orderby ct.NavOrder ascending
							   where r.SiteID == siteID
								   && (r.PageActive == active || active == null)
								   && ct.NavOrder < 1
								   && ct.IsLatestVersion == true
							   select new SiteNav(r, ct)).FirstOrDefault();
			return content;
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
