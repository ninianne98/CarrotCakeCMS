using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using Carrotware.CMS.Data;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.Core {
	public class SiteData {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteData() {
		}

		public void LoadSiteFromCache() {
			SiteData s = null;
			if (cmsSite == null) {
				s = (from r in db.tblSites
					 where r.SiteID == CurrentSiteID
					 select new SiteData(r)).FirstOrDefault();
				cmsSite = s;
			} else {
				s = cmsSite;
			}

			if (s != null) {
				this.SiteID = s.SiteID;
				this.MetaKeyword = s.MetaKeyword;
				this.MetaDescription = s.MetaDescription;
				this.SiteName = s.SiteName;
				this.SiteFolder = s.SiteFolder;
				this.MainURL = s.MainURL;
				this.BlockIndex = s.BlockIndex;
			}

		}


		public SiteData(tblSite s) {

			this.SiteID = s.SiteID;
			this.MetaKeyword = s.MetaKeyword;
			this.MetaDescription = s.MetaDescription;
			this.SiteName = s.SiteName;
			this.SiteFolder = s.SiteFolder;
			this.MainURL = s.MainURL;
			this.BlockIndex = s.BlockIndex;
		}



		private string ContentKey = "cms_SiteData_" + CurrentSiteID;
		private SiteData cmsSite {
			get {
				SiteData c = null;
				try { c = (SiteData)HttpContext.Current.Cache[ContentKey]; } catch { }
				return c;
			}
			set {
				if (value == null) {
					HttpContext.Current.Cache.Remove(ContentKey);
				} else {
					HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
				}
			}
		}


		public SiteData Get(Guid siteID) {
			var s = (from r in db.tblSites
					 where r.SiteID == siteID
					 select r).FirstOrDefault();

			if (s != null) {
				return new SiteData(s);
			} else {
				return null;
			}
		}


		public void Save() {

			var s = (from r in db.tblSites
					 where r.SiteID == this.SiteID
					 select r).FirstOrDefault();

			bool bNew = false;
			if (s == null) {
				s = new tblSite();
				if (this.SiteID == Guid.Empty) {
					this.SiteID = Guid.NewGuid();
				}
				bNew = true;
			}

			s.SiteID = this.SiteID;
			s.MetaKeyword = this.MetaKeyword;
			s.MetaDescription = this.MetaDescription;
			s.SiteName = this.SiteName;
			s.SiteFolder = this.SiteFolder;
			s.MainURL = this.MainURL;
			s.BlockIndex = this.BlockIndex;

			if (bNew) {
				db.tblSites.InsertOnSubmit(s);
			}
			db.SubmitChanges();

			//System.Web.HttpRuntime.UnloadAppDomain();
		}


		public static Guid CurrentSiteID {
			get {
				Guid _site = Guid.Empty;

				if (System.Configuration.ConfigurationManager.AppSettings["CarrotSiteID"] != null) {
					//try {
					_site = new Guid(System.Configuration.ConfigurationManager.AppSettings["CarrotSiteID"].ToString());
					//} catch {
					//    _site = Guid.Empty;
					//}
				}

				if (_site == Guid.Empty) {
					CMSConfigHelper h = new CMSConfigHelper();
					var s = h.DynSite;
					if (s != null) {
						_site = s.SiteID;
					}
				}

				return _site;
			}
		}

		public bool BlockIndex { get; set; }
		public string MainURL { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }
		public Guid SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteFolder { get; set; }

	}


}
