﻿using System;
using System.Collections.Generic;
using System.Linq;
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

	public class SiteNavHelperReal : IDisposable, ISiteNavHelper {
		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteNavHelperReal() { }

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

			lstContent = (from ct in db.carrot_Contents
						  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						  orderby ct.NavOrder, ct.NavMenuText
						  where r.SiteID == siteID
							  && ct.Parent_ContentID == null
							  && (r.PageActive == bActiveOnly || bActiveOnly == false)
							  && ct.IsLatestVersion == true
						  select new SiteNav(r, ct)).ToList();

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

			return lstContent;
		}

		public SiteNav GetPageNavigation(Guid siteID, string sPage) {

			SiteNav content = null;

			content = (from ct in db.carrot_Contents
					   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
					   where r.SiteID == siteID
						   && r.FileName.ToLower() == sPage.ToLower()
						   && ct.IsLatestVersion == true
					   select new SiteNav(r, ct)).FirstOrDefault();

			return content;
		}

		public SiteNav GetPageNavigation(Guid siteID, Guid rootContentID) {

			SiteNav content = null;

			content = (from ct in db.carrot_Contents
					   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
					   where r.SiteID == siteID
						   && ct.Root_ContentID == rootContentID
						   && ct.IsLatestVersion == true
					   select new SiteNav(r, ct)).FirstOrDefault();

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

			return content;
		}

		public List<SiteNav> GetChildNavigation(Guid siteID, Guid ParentID, bool bActiveOnly) {
			List<SiteNav> lstContent = null;


			lstContent = (from ct in db.carrot_Contents
						  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						  orderby ct.NavOrder, ct.NavMenuText
						  where r.SiteID == siteID
							  && ct.Parent_ContentID == ParentID
							  && (r.PageActive == bActiveOnly || bActiveOnly == false)
							  && ct.IsLatestVersion == true
						  select new SiteNav(r, ct)).ToList();

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

			return lstContent;
		}

		public List<SiteNav> GetLatest(Guid siteID, int iUpdates, bool bActiveOnly) {

			List<SiteNav> lstContent = null;

			lstContent = (from ct in db.carrot_Contents
						  join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
						  orderby ct.EditDate descending
						  where r.SiteID == siteID
							  && ct.IsLatestVersion == true
							  && (r.PageActive == bActiveOnly || bActiveOnly == false)
						  select new SiteNav(r, ct)).Take(iUpdates).ToList();

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