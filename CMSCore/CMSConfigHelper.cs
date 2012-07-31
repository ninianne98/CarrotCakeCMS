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
using Carrotware.CMS.Interface;

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

	public class CMSConfigHelper : IDisposable {

		private System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

		public CMSConfigHelper() {

		}


		private CarrotCMSDataContext db = new CarrotCMSDataContext();

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

		public void ResetConfigs() {
			string ModuleKey = "";

			ModuleKey = "cms_AdminMenuModules";
			HttpContext.Current.Cache.Remove(ModuleKey);

			ModuleKey = "cms_AdminToolboxModules";
			HttpContext.Current.Cache.Remove(ModuleKey);

			ModuleKey = "cms_DynamicSite";
			HttpContext.Current.Cache.Remove(ModuleKey);

			ModuleKey = "cms_Templates";
			HttpContext.Current.Cache.Remove(ModuleKey);

			ModuleKey = "cms_DynSite_" + DomainName;
			HttpContext.Current.Cache.Remove(ModuleKey);

			System.Web.HttpRuntime.UnloadAppDomain();
		}


		public string DomainName {
			get {
				var domName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
				if (domName.IndexOf(":") > 0) {
					domName = domName.Substring(0, domName.IndexOf(":"));
				}

				return domName.ToLower();
			}
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

					_p1.Add(new CMSPlugin { Caption = "Generic HTML &#0134;", FilePath = "~/Manage/ucGenericContent.ascx" });
					_p1.Add(new CMSPlugin { Caption = "Plain Text &#0134;", FilePath = "~/Manage/ucTextContent.ascx" });
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


		private List<CMSTemplate> GetTemplatesByDirectory() {
			var _plugins = new List<CMSTemplate>();

			var wwwpath = HttpContext.Current.Server.MapPath("~/");
			string sPlugCfg = HttpContext.Current.Server.MapPath("~/cmsTemplates/");

			if (Directory.Exists(sPlugCfg)) {

				string[] subdirs;
				try {
					subdirs = Directory.GetDirectories(sPlugCfg);
				} catch {
					subdirs = null;
				}


				if (subdirs != null) {
					foreach (string theDir in subdirs) {

						string sTplDef = theDir + @"\Skin.config";

						string sPathPrefix = theDir.Replace(wwwpath, @"\").Replace(@"\", @"/") + "/";

						if (File.Exists(sTplDef)) {

							DataSet ds = new DataSet();
							ds.ReadXml(sTplDef);

							var _p2 = (from d in ds.Tables[0].AsEnumerable()
									   select new CMSTemplate {
										   TemplatePath = (sPathPrefix + d.Field<string>("templatefile").ToLower()).ToLower(),
										   EncodedPath = EncodeBase64((sPathPrefix + d.Field<string>("templatefile").ToLower()).ToLower()),
										   Caption = d.Field<string>("filedesc")
									   }).ToList();

							_plugins = _plugins.Union(_p2).ToList();

						}
					}
				}
			}

			return _plugins;
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

					var _p1 = (from d in ds.Tables[0].AsEnumerable()
							   select new CMSTemplate {
								   TemplatePath = d.Field<string>("templatefile").ToLower(),
								   EncodedPath = EncodeBase64(d.Field<string>("templatefile").ToLower()),
								   Caption = d.Field<string>("filedesc")
							   }).ToList();

					var _p2 = GetTemplatesByDirectory();

					_plugins = _p1.Union(_p2).ToList();

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

				string ModuleKey = "cms_DynSite_" + DomainName;
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
								where ss.DomainName == DomainName
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

		public string EncodeBase64(string ValIn) {
			string val = "";
			if (!string.IsNullOrEmpty(ValIn)) {
				byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(ValIn);

				val = System.Convert.ToBase64String(toEncodeAsBytes);
			}
			return val;
		}


		public List<ObjectProperty> GetProperties(Object theObject) {
			PropertyInfo[] info = theObject.GetType().GetProperties();

			List<ObjectProperty> props = (from i in info.AsEnumerable()
										  select GetCustProps(theObject, i)).ToList();

			return props;
		}


		protected ObjectProperty GetCustProps(Object obj, PropertyInfo prop) {

			ObjectProperty objprop = new ObjectProperty {
				Name = prop.Name,
				DefValue = obj.GetType().GetProperty(prop.Name).GetValue(obj, null),
				PropertyType = prop.PropertyType,
				CanRead = prop.CanRead,
				CanWrite = prop.CanWrite,
				Props = prop,
				CompanionSourceFieldName = "",
				FieldMode = (prop.PropertyType.ToString().ToLower() == "system.boolean") ?
						WidgetAttribute.FieldMode.CheckBox : WidgetAttribute.FieldMode.TextBox
			};

			foreach (Attribute attr in objprop.Props.GetCustomAttributes(true)) {
				if (attr is WidgetAttribute) {
					var widgetAttrib = attr as WidgetAttribute;
					if (null != widgetAttrib) {
						try { objprop.CompanionSourceFieldName = widgetAttrib.SelectFieldSource; } catch { objprop.CompanionSourceFieldName = ""; }
						try { objprop.FieldMode = widgetAttrib.Mode; } catch { objprop.FieldMode = WidgetAttribute.FieldMode.Unknown; }
					}
				}
			}

			return objprop;
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


		private string ContentKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + SiteData.CurrentScriptName.ToString().ToLower() + "_cmsAdminContent";
		private string WidgetKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + SiteData.CurrentScriptName.ToString().ToLower() + "_cmsAdminWidget";

		public void OverrideKey(Guid guidContentID) {
			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				ContentPage pageContents = pageHelper.GetLatestContent(SiteData.CurrentSiteID, guidContentID);
				if (pageContents != null) {
					ContentKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + pageContents.FileName.ToLower() + "_cmsAdminContent";
					WidgetKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + pageContents.FileName.ToLower() + "_cmsAdminWidget";
				}
			}
		}

		public void OverrideKey(string sPageName) {
			ContentKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + sPageName.ToLower() + "_cmsAdminContent";
			WidgetKey = HttpContext.Current.User.Identity.Name.ToString() + "_" + sPageName.ToLower() + "_cmsAdminWidget";

			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				filePage = pageHelper.GetLatestContent(SiteData.CurrentSiteID, null, sPageName.ToLower());
			}
		}

		protected ContentPage filePage = null;

		protected void LoadGuids() {

			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				if (SiteData.CurrentScriptName.ToString().ToLower().StartsWith("/manage/")) {
					Guid guidPage = Guid.Empty;
					if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pageid"])) {
						guidPage = new Guid(HttpContext.Current.Request.QueryString["pageid"].ToString());
					}
					filePage = pageHelper.GetLatestContent(SiteData.CurrentSiteID, guidPage);
				} else {
					filePage = pageHelper.GetLatestContent(SiteData.CurrentSiteID, null, SiteData.CurrentScriptName.ToString().ToLower());
				}
			}

			var lst = (from c in db.tblSerialCaches
					   where c.ItemID == filePage.Root_ContentID
					   && c.EditUserId == SiteData.CurrentUserGuid
					   && c.EditDate < DateTime.Now.AddHours(-2)
					   && c.SiteID == SiteData.CurrentSiteID
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
				} catch (Exception ex) { }
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
					   && c.EditUserId == SiteData.CurrentUserGuid
					   && c.KeyType == sKey
					   && c.SiteID == SiteID
					   select c).FirstOrDefault();

			if (itm == null) {
				bAdd = true;
				itm = new tblSerialCache();
				itm.SerialCacheID = Guid.NewGuid();
				itm.SiteID = SiteData.CurrentSiteID;
				itm.ItemID = filePage.Root_ContentID;
				itm.EditUserId = SiteData.CurrentUserGuid;
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
					   && c.EditUserId == SiteData.CurrentUserGuid
					   && c.KeyType == sKey
					   && c.SiteID == SiteData.CurrentSiteID
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
					   && c.EditUserId == SiteData.CurrentUserGuid
					   && c.KeyType == sKey
					   && c.SiteID == SiteData.CurrentSiteID
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

		public PropertyInfo Props { get; set; }

		public string CompanionSourceFieldName { get; set; }

		public WidgetAttribute.FieldMode FieldMode { get; set; }


		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is ObjectProperty) {
				ObjectProperty p = (ObjectProperty)obj;
				return (Name == p.Name) && (PropertyType == p.PropertyType);
			} else {
				return false;
			}
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
		public string EncodedPath { get; set; }
	}

	public class DynamicSite {
		public Guid SiteID { get; set; }
		public string DomainName { get; set; }

	}

}