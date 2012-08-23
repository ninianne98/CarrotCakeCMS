using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using Carrotware.CMS.Data;
using System.Collections.Specialized;
using System.Xml;
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
	public class SiteData : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public SiteData() { }

		public SiteData(carrot_Site s) {

			this.SiteID = s.SiteID;
			this.MetaKeyword = s.MetaKeyword;
			this.MetaDescription = s.MetaDescription;
			this.SiteName = s.SiteName;
			this.SiteFolder = s.SiteFolder;
			this.MainURL = s.MainURL;
			this.BlockIndex = s.BlockIndex;
		}

		public SiteData Get(Guid siteID) {
			var s = (from r in db.carrot_Sites
					 where r.SiteID == siteID
					 select r).FirstOrDefault();

			if (s != null) {
				return new SiteData(s);
			} else {
				return null;
			}
		}


		private static string SiteKeyPrefix = "cms_SiteData_";
		public static SiteData CurrentSite {
			get {
				string ContentKey = SiteKeyPrefix + CurrentSiteID.ToString();
				SiteData currentSite = null;
				try { currentSite = (SiteData)HttpContext.Current.Cache[ContentKey]; } catch { }
				if (currentSite == null) {
					using (SiteData sd = new SiteData()) {
						currentSite = sd.Get(CurrentSiteID);
					}
					if (currentSite != null) {
						HttpContext.Current.Cache.Insert(ContentKey, currentSite, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
					} else {
						HttpContext.Current.Cache.Remove(ContentKey);
					}
				}
				return currentSite;
			}
			set {
				string ContentKey = SiteKeyPrefix + CurrentSiteID.ToString();
				if (value == null) {
					HttpContext.Current.Cache.Remove(ContentKey);
				} else {
					HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
			}
		}


		public SiteData GetCurrentSite() {
			//return Get(CurrentSiteID);
			return CurrentSite;
		}

		public void Save() {

			var s = (from r in db.carrot_Sites
					 where r.SiteID == this.SiteID
					 select r).FirstOrDefault();

			bool bNew = false;
			if (s == null) {
				s = new carrot_Site();
				if (this.SiteID == Guid.Empty) {
					this.SiteID = Guid.NewGuid();
				}
				bNew = true;
			}

			// if updating the current site then blank out its cache
			if (CurrentSiteID == this.SiteID) {
				CurrentSite = null;
			}

			s.SiteID = this.SiteID;
			s.MetaKeyword = this.MetaKeyword;
			s.MetaDescription = this.MetaDescription;
			s.SiteName = this.SiteName;
			s.SiteFolder = this.SiteFolder;
			s.MainURL = this.MainURL;
			s.BlockIndex = this.BlockIndex;

			if (bNew) {
				db.carrot_Sites.InsertOnSubmit(s);
			}
			db.SubmitChanges();

			//System.Web.HttpRuntime.UnloadAppDomain();
		}


		public bool VerifyUserHasSiteAccess(Guid siteID, Guid userID) {

			//all admins have rights to all sites
			if (SecurityData.IsAdmin) {
				return true;
			}

			// if user is neither admin nor editor, they should not be in the backend of the site
			if (!(SecurityData.IsEditor || SecurityData.IsAdmin)) {
				return false;
			}

			// by this point, the user is probably an editor, make sure they have rights to this site
			var lstSites = (from l in db.carrot_UserSiteMappings
							where l.UserId == userID
								 && l.SiteID == siteID
							select l.SiteID).ToList();

			if (lstSites.Count > 0) {
				return true;
			}

			return false;
		}


		public void CleanUpSerialData() {

			var lst = (from c in db.carrot_SerialCaches
					   where c.EditDate < DateTime.Now.AddHours(-6)
					   && c.SiteID == CurrentSiteID
					   select c).ToList();

			if (lst.Count > 0) {
				foreach (var l in lst) {
					db.carrot_SerialCaches.DeleteOnSubmit(l);
				}
				db.SubmitChanges();
			}

		}


		public void MapUserToSite(Guid siteID, Guid userID) {

			carrot_UserSiteMapping map = new carrot_UserSiteMapping();
			map.UserSiteMappingID = Guid.NewGuid();
			map.SiteID = siteID;
			map.UserId = userID;

			db.carrot_UserSiteMappings.InsertOnSubmit(map);
			db.SubmitChanges();

		}

		public static Guid CurrentSiteID {
			get {
				Guid _site = Guid.Empty;

				if (ConfigurationManager.AppSettings["CarrotSiteID"] != null) {
					_site = new Guid(ConfigurationManager.AppSettings["CarrotSiteID"].ToString());
				}

				if (_site == Guid.Empty) {
					var s = CMSConfigHelper.DynSite;
					if (s != null) {
						_site = s.SiteID;
					}
				}

				return _site;
			}
		}

		private static string _siteQS = null;
		public static string OldSiteQuerystring {
			get {
				if (_siteQS == null) {
					_siteQS = String.Empty;
					if (ConfigurationManager.AppSettings["CarrotOldSiteQuerystring"] != null) {
						_siteQS = ConfigurationManager.AppSettings["CarrotOldSiteQuerystring"].ToString().ToLower();
					}
				}
				return _siteQS;
			}
		}


		public static AspNetHostingPermissionLevel CurrentTrustLevel {
			get {

				foreach (AspNetHostingPermissionLevel trustLevel in
					new AspNetHostingPermissionLevel[] {
						AspNetHostingPermissionLevel.Unrestricted,
						AspNetHostingPermissionLevel.High,
						AspNetHostingPermissionLevel.Medium,
						AspNetHostingPermissionLevel.Low,
						AspNetHostingPermissionLevel.Minimal 
					  }) {
					try {
						new AspNetHostingPermission(trustLevel).Demand();
					} catch (System.Security.SecurityException) {
						continue;
					}

					return trustLevel;
				}

				return AspNetHostingPermissionLevel.None;
			}
		}


		public bool BlockIndex { get; set; }
		public string MainURL { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeyword { get; set; }
		public Guid SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteFolder { get; set; }

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion


		public static void PerformRedirectToErrorPage(string sErrorKey, string sReqURL) {

			//parse web.config as XML because of medium trust issues

			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(HttpContext.Current.Server.MapPath("~/Web.config"));

			XmlElement xmlCustomErrors = xDoc.SelectSingleNode("//system.web/customErrors") as XmlElement;

			if (xmlCustomErrors != null) {
				string defaultRedirect = "";
				string çonfigRedirect = "";

				if (xmlCustomErrors.Attributes["mode"] != null && xmlCustomErrors.Attributes["mode"].Value.ToLower() != "off") {
					if (xmlCustomErrors.Attributes["defaultRedirect"] != null) {
						defaultRedirect = xmlCustomErrors.Attributes["defaultRedirect"].Value;
					}

					if (xmlCustomErrors.HasChildNodes) {
						XmlNode errNode = xmlCustomErrors.SelectSingleNode("/configuration/system.web/customErrors/error[@statusCode='" + sErrorKey + "']");
						if (errNode != null) {
							çonfigRedirect = errNode.Attributes["redirect"].Value;
						}
					}

					if (!string.IsNullOrEmpty(çonfigRedirect)) {
						HttpContext.Current.Response.Redirect(çonfigRedirect + "?aspxerrorpath=" + sReqURL);
					} else {
						if (!string.IsNullOrEmpty(defaultRedirect)) {
							HttpContext.Current.Response.Redirect(defaultRedirect + "?aspxerrorpath=" + sReqURL);
						}
					}
				}
			}

			/*
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			CustomErrorsSection section = (CustomErrorsSection)config.GetSection("system.web/customErrors");

			if (section != null) {
				if (section.Mode != CustomErrorsMode.Off) {
					CustomError configuredError = section.Errors[sErrorKey];
					if (configuredError != null) {
						if (!string.IsNullOrEmpty(configuredError.Redirect)) {
							HttpContext.Current.Response.Redirect(configuredError.Redirect + "?aspxerrorpath=" + sReqURL);
						}
					} else {
						if (!string.IsNullOrEmpty(section.DefaultRedirect)) {
							HttpContext.Current.Response.Redirect(section.DefaultRedirect + "?aspxerrorpath=" + sReqURL);
						}
					}
				}
			}
			*/
		}

		public static string DefaultDirectoryFilename {
			get { return "/default.aspx"; }
		}
		public static string DefaultTemplateFilename {
			get { return "/Manage/PlainTemplate.aspx"; }
		}

		public static string CurrentScriptName {
			get { return HttpContext.Current.Request.ServerVariables["script_name"].ToString(); }
		}

		public static string ReferringPage {
			get {
				var r = SiteData.CurrentScriptName;
				try { r = HttpContext.Current.Request.ServerVariables["http_referer"].ToString(); } catch { }
				if (string.IsNullOrEmpty(r))
					r = DefaultDirectoryFilename;
				return r;
			}
		}



	}


}
