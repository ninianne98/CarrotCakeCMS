using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.Profile;
using System.Web.Security;
using System.Xml.Serialization;
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

	public class CMSConfigHelper {

		private System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

		public CMSConfigHelper() {

		}

		public List<CMSAdminModule> AdminModules {
			get {

				var _modules = new List<CMSAdminModule>();

				string ModuleKey = "cms_AdminMenuModules";
				bool bCached = false;

				string sPlugCfg = HttpContext.Current.Server.MapPath("~/AdminModules.config");

				try {
					_modules = (List<CMSAdminModule>)HttpContext.Current.Cache[ModuleKey];
					if (_modules != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (File.Exists(sPlugCfg) && !bCached) {
					DataSet ds = new DataSet();
					ds.ReadXml(sPlugCfg);

					_modules = (from d in ds.Tables[0].AsEnumerable()
								select new CMSAdminModule {
									PluginName = d.Field<string>("caption"),
									PluginID = new Guid(d.Field<string>("pluginid"))
								}).ToList();

					foreach (var p in _modules) {
						p.PluginMenus = (from d in ds.Tables["ID_" + p.PluginID.ToString()].AsEnumerable()
										 select new CMSAdminModuleMenu {
											 Caption = d.Field<string>("pluginlabel"),
											 SortOrder = int.Parse(d.Field<string>("menuorder")),
											 PluginParm = d.Field<string>("parm"),
											 ControlFile = d.Field<string>("plugincontrol"),
											 UseAjax = Convert.ToBoolean(d.Field<string>("useajax")),
											 IsVisible = Convert.ToBoolean(d.Field<string>("visible")),
											 PluginID = new Guid(d.Field<string>("pluginid"))
										 }).ToList();
					}
					HttpContext.Current.Cache.Insert(ModuleKey, _modules, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _modules;
			}
		}

		public List<CMSPlugin> ToolboxPlugins {
			get {

				var _plugins = new List<CMSPlugin>();

				string ModuleKey = "cms_AdminToolboxModules";
				bool bCached = false;

				string sPlugCfg = HttpContext.Current.Server.MapPath("~/PublicControls.config");

				try {
					_plugins = (List<CMSPlugin>)HttpContext.Current.Cache[ModuleKey];
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (File.Exists(sPlugCfg) && !bCached) {
					DataSet ds = new DataSet();
					ds.ReadXml(sPlugCfg);

					List<CMSPlugin> _p1 = new List<CMSPlugin>();

					_p1.Add(new CMSPlugin { Caption = "Generic Content &#0134;", FilePath = "~/Manage/ucGenericContent.ascx" });
					_p1.Add(new CMSPlugin { Caption = "Top Level Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.TopLevelNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { Caption = "Two Level Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.TwoLevelNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { Caption = "Child Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ChildNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { Caption = "Sibling Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.SiblingNavigation, Carrotware.CMS.UI.Controls" });
		
					List<CMSPlugin> _p2 = (from d in ds.Tables[0].AsEnumerable()
								select new CMSPlugin {
									FilePath = d.Field<string>("filepath"),
									Caption = d.Field<string>("crtldesc")
								}).ToList();

					_plugins = _p1.Union(_p2).ToList();

					HttpContext.Current.Cache.Insert(ModuleKey, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _plugins;
			}
		}

		public List<CMSTemplate> Templates {
			get {

				var _plugins = new List<CMSTemplate>();

				string ModuleKey = "cms_Templates";
				bool bCached = false;

				string sPlugCfg = HttpContext.Current.Server.MapPath("~/SiteSkins.config");

				try {
					_plugins = (List<CMSTemplate>)HttpContext.Current.Cache[ModuleKey];
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (File.Exists(sPlugCfg) && !bCached) {
					DataSet ds = new DataSet();
					ds.ReadXml(sPlugCfg);

					_plugins = (from d in ds.Tables[0].AsEnumerable()
								select new CMSTemplate {
									TemplatePath = d.Field<string>("templatefile").ToLower(),
									Caption = d.Field<string>("filedesc")
								}).ToList();

					HttpContext.Current.Cache.Insert(ModuleKey, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _plugins;
			}
		}

		public List<DynamicSite> SiteList {
			get {

				var _plugins = new List<DynamicSite>();

				string ModuleKey = "cms_DynamicSite";
				bool bCached = false;

				string sPlugCfg = HttpContext.Current.Server.MapPath("~/SiteMapping.config");

				try {
					_plugins = (List<DynamicSite>)HttpContext.Current.Cache[ModuleKey];
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (File.Exists(sPlugCfg) && !bCached) {
					DataSet ds = new DataSet();
					ds.ReadXml(sPlugCfg);

					_plugins = (from d in ds.Tables[0].AsEnumerable()
								select new DynamicSite {
									DomainName = d.Field<string>("domname"),
									SiteID = new Guid(d.Field<string>("siteid"))
								}).ToList();

					HttpContext.Current.Cache.Insert(ModuleKey, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _plugins;
			}
		}


		public DynamicSite DynSite {
			get {
				var _plugins = new DynamicSite();
				var domName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
				if (domName.IndexOf(":") > 0) {
					domName = domName.Substring(0, domName.IndexOf(":"));
				}

				string ModuleKey = "cms_DynSite_" + domName;
				bool bCached = false;

				try {
					_plugins = (DynamicSite)HttpContext.Current.Cache[ModuleKey];
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if ((SiteList.Count > 0) && !bCached) {

					_plugins = (from ss in SiteList
								where ss.DomainName == domName
								select ss).FirstOrDefault();

					HttpContext.Current.Cache.Insert(ModuleKey, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _plugins;
			}
		}





		public string DecodeBase64(string ValIn) {
			string val = "";

			if (!string.IsNullOrEmpty(ValIn)) {
				val = encoding.GetString(Convert.FromBase64String(ValIn));
			}
			return val;
		}

		public List<ObjectProperty> GetProperties(Object theObject) {
			PropertyInfo[] info = theObject.GetType().GetProperties();

			List<ObjectProperty> props = (from i in info.AsEnumerable()
										  select new ObjectProperty {
											  Name = i.Name,
											  DefValue = theObject.GetType().GetProperty(i.Name).GetValue(theObject, null),
											  PropertyType = i.PropertyType,
											  CanRead = i.CanRead,
											  CanWrite = i.CanWrite
										  }).ToList();
			return props;
		}

		public List<ObjectProperty> GetTypeProperties(Type theType) {
			PropertyInfo[] info = theType.GetProperties();

			List<ObjectProperty> props = (from i in info.AsEnumerable()
										  select new ObjectProperty {
											  Name = i.Name,
											  //DefValue = theType.GetProperty(i.Name).GetValue(theType, null).ToString(),
											  PropertyType = i.PropertyType,
											  CanRead = i.CanRead,
											  CanWrite = i.CanWrite
										  }).ToList();
			return props;
		}

		private CarrotCMSDataContext db = new CarrotCMSDataContext();

		private string ContentKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + HttpContext.Current.Request.Path.ToString().ToLower() + "_cmsAdminContent";
		private string WidgetKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + HttpContext.Current.Request.Path.ToString().ToLower() + "_cmsAdminWidget";

		public void OverrideKey(string sPageName) {

			ContentKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + sPageName.ToLower() + "_cmsAdminContent";
			WidgetKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + sPageName.ToLower() + "_cmsAdminWidget";

		}

		protected Guid CurrentUserGuid = Guid.Empty;

		protected ContentPage filePage = null;

		protected MembershipUser CurrentUser { get; set; }

		protected void LoadGuids() {

			ContentPage pageHelper = new ContentPage();
			if (HttpContext.Current.Request.Path.ToString().ToLower().StartsWith("/manage/")) {
				Guid guidPage = Guid.Empty;
				if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pageid"])) {
					guidPage = new Guid(HttpContext.Current.Request.QueryString["pageid"].ToString());
				}
				filePage = pageHelper.GetLatestContent(SiteID, guidPage);
			} else {
				filePage = pageHelper.GetLatestContent(SiteID, null, HttpContext.Current.Request.Path.ToString().ToLower());
			}

			if (HttpContext.Current.User.Identity.IsAuthenticated) {
				if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {
					CurrentUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
					CurrentUserGuid = new Guid(CurrentUser.ProviderUserKey.ToString());
				}
			} else {
				CurrentUser = null;
				CurrentUserGuid = Guid.Empty;
			}


			var lst = (from c in db.tblSerialCaches
					   where c.ItemID == filePage.Root_ContentID
					   && c.EditUserId == CurrentUserGuid
					   && c.EditDate < DateTime.Now.AddHours(-2)
					   && c.SiteID == SiteID
					   select c).ToList();

			if (lst.Count > 0) {
				foreach (var l in lst) {
					db.tblSerialCaches.DeleteOnSubmit(l);
				}
				db.SubmitChanges();
			}

		}

		public ContentPage cmsAdminContent {
			get {
				ContentPage c = null;
				try {
					//c = (ContentPage)HttpContext.Current.Cache[ContentKey];
					var sXML = GetSerialized("cmsAdminContent");
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPage));
					StringReader stringReader = new StringReader(sXML);
					Object genpref = xmlSerializer.Deserialize(stringReader);
					stringReader.Close();
					c = genpref as ContentPage;
				} catch { }
				return c;
			}
			set {
				if (value == null) {
					//HttpContext.Current.Cache.Remove(ContentKey);
					ClearSerialized("cmsAdminContent");
				} else {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPage));
					StringWriter stringWriter = new StringWriter();
					xmlSerializer.Serialize(stringWriter, value);
					string sXML = stringWriter.ToString();
					stringWriter.Close();
					SaveSerialized("cmsAdminContent", sXML);

					//HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(360), Cache.NoSlidingExpiration);
				}
			}
		}

		public List<PageWidget> cmsAdminWidget {
			get {
				List<PageWidget> c = null;
				//try { c = (List<PageWidget>)HttpContext.Current.Cache[WidgetKey]; } catch { }
				var sXML = GetSerialized("cmsAdminWidget");
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PageWidget>));
				StringReader stringReader = new StringReader(sXML);
				Object genpref = xmlSerializer.Deserialize(stringReader);
				stringReader.Close();
				c = genpref as List<PageWidget>;
				return c;
			}
			set {
				if (value == null) {
					//HttpContext.Current.Cache.Remove(WidgetKey);
					ClearSerialized("cmsAdminWidget");
				} else {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PageWidget>));
					StringWriter stringWriter = new StringWriter();
					xmlSerializer.Serialize(stringWriter, value);
					string sXML = stringWriter.ToString();
					stringWriter.Close();
					SaveSerialized("cmsAdminWidget", sXML);

					//HttpContext.Current.Cache.Insert(WidgetKey, value, null, DateTime.Now.AddMinutes(360), Cache.NoSlidingExpiration);
				}
			}
		}

		private Guid SiteID { get { return SiteData.CurrentSiteID; } }

		private void SaveSerialized(string sKey, string sData) {
			bool bAdd = false;
			LoadGuids();

			var itm = (from c in db.tblSerialCaches
					   where c.ItemID == filePage.Root_ContentID
					   && c.EditUserId == CurrentUserGuid
					   && c.KeyType == sKey
					   && c.SiteID == SiteID
					   select c).FirstOrDefault();

			if (itm == null) {
				bAdd = true;
				itm = new tblSerialCache();
				itm.SerialCacheID = Guid.NewGuid();
				itm.SiteID = SiteID;
				itm.ItemID = filePage.Root_ContentID;
				itm.EditUserId = CurrentUserGuid;
				itm.KeyType = sKey;
			}

			itm.SerializedData = sData;
			itm.EditDate = DateTime.Now;

			if (bAdd) {
				db.tblSerialCaches.InsertOnSubmit(itm);
			}
			db.SubmitChanges();
		}


		private string GetSerialized(string sKey) {
			string sData = "";
			LoadGuids();

			var itm = (from c in db.tblSerialCaches
					   where c.ItemID == filePage.Root_ContentID
					   && c.EditUserId == CurrentUserGuid
					   && c.KeyType == sKey
					   && c.SiteID == SiteID
					   select c).FirstOrDefault();

			if (itm != null) {
				sData = itm.SerializedData;
			}

			return sData;
		}


		private bool ClearSerialized(string sKey) {
			LoadGuids();

			var itm = (from c in db.tblSerialCaches
					   where c.ItemID == filePage.Root_ContentID
					   && c.EditUserId == CurrentUserGuid
					   && c.KeyType == sKey
					   && c.SiteID == SiteID
					   select c).FirstOrDefault();

			if (itm != null) {
				db.tblSerialCaches.DeleteOnSubmit(itm);
				db.SubmitChanges();
				return true;
			}

			return false;
		}

	}




	public class ObjectProperty {
		public ObjectProperty() { }
		public string Name { get; set; }
		public bool CanWrite { get; set; }
		public bool CanRead { get; set; }
		public Type PropertyType { get; set; }

		public Object DefValue { get; set; }


		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			ObjectProperty p = (ObjectProperty)obj;
			return (Name == p.Name) && (PropertyType == p.PropertyType);
		}

		public override int GetHashCode() {
			return Name.GetHashCode() ^ PropertyType.ToString().GetHashCode();
		}

	}


	public class CMSAdminModule {
		public CMSAdminModule() {
			PluginMenus = new List<CMSAdminModuleMenu>();
		}
		public Guid PluginID { get; set; }
		public string PluginName { get; set; }
		public List<CMSAdminModuleMenu> PluginMenus { get; set; }
	}

	public class CMSAdminModuleMenu {
		public Guid PluginID { get; set; }
		public int SortOrder { get; set; }
		public string Caption { get; set; }
		public string PluginParm { get; set; }
		public string ControlFile { get; set; }
		public bool UseAjax { get; set; }
		public bool IsVisible { get; set; }

	}

	public class CMSPlugin {
		public string FilePath { get; set; }
		public string Caption { get; set; }
	}

	public class CMSTemplate {
		public string TemplatePath { get; set; }
		public string Caption { get; set; }
	}

	public class DynamicSite {
		public Guid SiteID { get; set; }
		public string DomainName { get; set; }

	}

}