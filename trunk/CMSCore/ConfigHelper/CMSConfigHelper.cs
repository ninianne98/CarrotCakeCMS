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
using System.Xml.Serialization;
using Carrotware.CMS.Data;
using Carrotware.CMS.Interface;
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

	public class CMSConfigHelper : IDisposable {

		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;

		public CMSConfigHelper() {
			//#if DEBUG
			//            db.Log = new DebugTextWriter();
			//#endif
		}

		private enum CMSConfigFileType {
			AdminMod,
			AdminModules,
			PublicControls,
			SkinDef,
			PublicCtrl,
			SiteSkins,
			SiteMapping
		}


		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

		private static string keyAdminMenuModules = "cms_AdminMenuModules";

		private static string keyAdminToolboxModules = "cms_AdminToolboxModules";

		private static string keyDynamicSite = "cms_DynamicSite";

		private static string keyTemplates = "cms_Templates";

		private static string keyDynSite = "cms_DynSite_";

		public static string keyAdminContent = "cmsAdminContent";

		public static string keyAdminWidget = "cmsAdminWidget";


		public void ResetConfigs() {

			HttpContext.Current.Cache.Remove(keyAdminMenuModules);

			HttpContext.Current.Cache.Remove(keyAdminToolboxModules);

			HttpContext.Current.Cache.Remove(keyDynamicSite);

			HttpContext.Current.Cache.Remove(keyTemplates);

			string ModuleKey = keyDynSite + DomainName;
			HttpContext.Current.Cache.Remove(ModuleKey);

			VirtualDirectory.RegisterRoutes(true);

			if (SiteData.CurrentTrustLevel == AspNetHostingPermissionLevel.Unrestricted) {
				System.Web.HttpRuntime.UnloadAppDomain();
			}
		}


		private static Page CachedPage {
			get {
				if (_CachedPage == null)
					_CachedPage = new Page();
				return _CachedPage;
			}
		}
		private static Page _CachedPage;

		public static string GetWebResourceUrl(Type type, string resource) {
			return CachedPage.ClientScript.GetWebResourceUrl(type, resource);
		}

		public static string DomainName {
			get {
				var domName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
				if (domName.IndexOf(":") > 0) {
					domName = domName.Substring(0, domName.IndexOf(":"));
				}

				return domName.ToLower();
			}
		}

		private static DataSet ReadDataSetConfig(CMSConfigFileType cfg, string sPath) {
			string sPlugCfg = "default.config";
			string sRealPath = HttpContext.Current.Server.MapPath(sPath);

			int iExpectedTblCount = 1;

			switch (cfg) {
				case CMSConfigFileType.AdminModules:
					sPlugCfg = sRealPath + "AdminModules.config";
					iExpectedTblCount = 2;
					break;
				case CMSConfigFileType.AdminMod:
					sPlugCfg = sRealPath + "Admin.config";
					iExpectedTblCount = 2;
					break;
				case CMSConfigFileType.PublicCtrl:
					sPlugCfg = sRealPath + "Public.config";
					break;
				case CMSConfigFileType.PublicControls:
					sPlugCfg = sRealPath + "PublicControls.config";
					break;
				case CMSConfigFileType.SkinDef:
					sPlugCfg = sRealPath + "Skin.config";
					break;
				case CMSConfigFileType.SiteSkins:
					sPlugCfg = sRealPath + "SiteSkins.config";
					break;
				case CMSConfigFileType.SiteMapping:
					sPlugCfg = sRealPath + "SiteMapping.config";
					break;
				default:
					sPlugCfg = sRealPath + "default.config";
					iExpectedTblCount = -1;
					break;
			}

			DataSet ds = new DataSet();
			if (File.Exists(sPlugCfg) && iExpectedTblCount > 0) {
				ds.ReadXml(sPlugCfg);
			}

			if (ds == null) {
				ds = new DataSet();
			}

			int iTblCount = ds.Tables.Count;

			// if dataset has wrong # of tables, build out more tables
			if (iTblCount < iExpectedTblCount) {
				for (int t = iTblCount; t <= iExpectedTblCount; t++) {
					ds.Tables.Add(new DataTable());
					ds.AcceptChanges();
				}
			}

			if (iExpectedTblCount > 0) {
				iTblCount = ds.Tables.Count;

				List<string> reqCols0 = new List<string>();
				List<string> reqCols1 = new List<string>();

				switch (cfg) {
					case CMSConfigFileType.AdminMod:
					case CMSConfigFileType.AdminModules:
						reqCols0.Add("caption");
						reqCols0.Add("pluginid");

						reqCols1.Add("pluginlabel");
						reqCols1.Add("menuorder");
						reqCols1.Add("parm");
						reqCols1.Add("plugincontrol");
						reqCols1.Add("useajax");
						reqCols1.Add("usepopup");
						reqCols1.Add("visible");
						reqCols1.Add("pluginid");
						break;
					case CMSConfigFileType.PublicCtrl:
					case CMSConfigFileType.PublicControls:
						reqCols0.Add("filepath");
						reqCols0.Add("crtldesc");
						break;
					case CMSConfigFileType.SkinDef:
					case CMSConfigFileType.SiteSkins:
						reqCols0.Add("templatefile");
						reqCols0.Add("filedesc");
						break;
					case CMSConfigFileType.SiteMapping:
						reqCols0.Add("domname");
						reqCols0.Add("siteid");
						break;
					default:
						reqCols0.Add("caption");
						reqCols0.Add("pluginid");
						break;
				}

				//validate that the dataset has the right table configuration
				DataTable dt0 = ds.Tables[0];
				foreach (string c in reqCols0) {
					if (!dt0.Columns.Contains(c)) {
						DataColumn dc = new DataColumn(c);
						dc.DataType = System.Type.GetType("System.String"); // add if not found

						dt0.Columns.Add(dc);
						dt0.AcceptChanges();
					}
				}

				for (int iTbl = 1; iTbl < iTblCount; iTbl++) {
					DataTable dt = ds.Tables[iTbl];
					foreach (string c in reqCols1) {
						if (!dt.Columns.Contains(c)) {
							DataColumn dc = new DataColumn(c);
							dc.DataType = System.Type.GetType("System.String"); // add if not found

							dt.Columns.Add(dc);
							dt.AcceptChanges();
						}
					}
				}

			}

			return ds;
		}

		public string GetFolderPrefix(string sDirPath) {
			return FileDataHelper.MakeWebFolderPath(sDirPath);
		}

		public List<CMSAdminModule> AdminModules {
			get {

				var _modules = new List<CMSAdminModule>();

				bool bCached = false;

				try {
					_modules = (List<CMSAdminModule>)HttpContext.Current.Cache[keyAdminMenuModules];
					if (_modules != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.AdminModules, "~/");

					List<CMSAdminModuleMenu> _ctrls = new List<CMSAdminModuleMenu>();

					_modules = (from d in ds.Tables[0].AsEnumerable()
								select new CMSAdminModule {
									PluginName = d.Field<string>("caption"),
									PluginID = new Guid(d.Field<string>("pluginid"))
								}).ToList();

					foreach (DataTable t in ds.Tables) {
						if (t.TableName.StartsWith("ID_") || t.TableName.StartsWith("plugincontrols")) {
							var _ctrl2 = (from d in t.AsEnumerable()
										  select new CMSAdminModuleMenu {
											  Caption = d.Field<string>("pluginlabel"),
											  SortOrder = string.IsNullOrEmpty(d.Field<string>("menuorder")) ? -1 : int.Parse(d.Field<string>("menuorder")),
											  PluginParm = d.Field<string>("parm"),
											  ControlFile = d.Field<string>("plugincontrol"),
											  UsePopup = string.IsNullOrEmpty(d.Field<string>("usepopup")) ? false : Convert.ToBoolean(d.Field<string>("usepopup")),
											  UseAjax = string.IsNullOrEmpty(d.Field<string>("useajax")) ? false : Convert.ToBoolean(d.Field<string>("useajax")),
											  IsVisible = string.IsNullOrEmpty(d.Field<string>("visible")) ? false : Convert.ToBoolean(d.Field<string>("visible")),
											  PluginID = string.IsNullOrEmpty(d.Field<string>("pluginid")) ? Guid.Empty : new Guid(d.Field<string>("pluginid"))
										  }).ToList();

							_ctrls = _ctrls.Union(_ctrl2).ToList();
						}
					}

					foreach (var p in _modules) {
						p.PluginMenus = (from c in _ctrls
										 where c.PluginID == p.PluginID
										 select c).ToList();
					}

					_modules = _modules.Union(GetModulesByDirectory()).ToList();

					HttpContext.Current.Cache.Insert(keyAdminMenuModules, _modules, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}

				return _modules.OrderBy(m => m.PluginName).ToList();
			}
		}


		private List<CMSPlugin> GetPluginsByDirectory() {
			var _plugins = new List<CMSPlugin>();

			string sPlugCfg = HttpContext.Current.Server.MapPath("~/cmsPlugins/");

			if (Directory.Exists(sPlugCfg)) {

				string[] subdirs;
				try {
					subdirs = Directory.GetDirectories(sPlugCfg);
				} catch {
					subdirs = null;
				}

				if (subdirs != null) {
					foreach (string theDir in subdirs) {

						string sTplDef = theDir + @"\Public.config";

						if (File.Exists(sTplDef)) {
							string sPathPrefix = GetFolderPrefix(theDir);
							DataSet ds = ReadDataSetConfig(CMSConfigFileType.PublicCtrl, sPathPrefix);

							var _p2 = (from d in ds.Tables[0].AsEnumerable()
									   select new CMSPlugin {
										   FilePath = "~" + sPathPrefix + d.Field<string>("filepath"),
										   Caption = d.Field<string>("crtldesc")
									   }).ToList();

							_plugins = _plugins.Union(_p2).ToList();
						}
					}
				}
			}

			return _plugins;
		}



		private List<CMSAdminModule> GetModulesByDirectory() {
			var _plugins = new List<CMSAdminModule>();

			string sPlugCfg = HttpContext.Current.Server.MapPath("~/cmsPlugins/");

			if (Directory.Exists(sPlugCfg)) {

				string[] subdirs;
				try {
					subdirs = Directory.GetDirectories(sPlugCfg);
				} catch {
					subdirs = null;
				}

				if (subdirs != null) {
					foreach (string theDir in subdirs) {

						string sTplDef = theDir + @"\Admin.config";

						if (File.Exists(sTplDef)) {
							string sPathPrefix = GetFolderPrefix(theDir);
							DataSet ds = ReadDataSetConfig(CMSConfigFileType.AdminMod, sPathPrefix);

							var _modules = (from d in ds.Tables[0].AsEnumerable()
											select new CMSAdminModule {
												PluginName = d.Field<string>("caption"),
												PluginID = new Guid(d.Field<string>("pluginid"))
											}).ToList();

							var _ctrls = (from d in ds.Tables[1].AsEnumerable()
										  select new CMSAdminModuleMenu {
											  Caption = d.Field<string>("pluginlabel"),
											  SortOrder = string.IsNullOrEmpty(d.Field<string>("menuorder")) ? -1 : int.Parse(d.Field<string>("menuorder")),
											  PluginParm = d.Field<string>("parm"),
											  ControlFile = "~" + sPathPrefix + d.Field<string>("plugincontrol"),
											  UsePopup = string.IsNullOrEmpty(d.Field<string>("usepopup")) ? false : Convert.ToBoolean(d.Field<string>("usepopup")),
											  UseAjax = string.IsNullOrEmpty(d.Field<string>("useajax")) ? false : Convert.ToBoolean(d.Field<string>("useajax")),
											  IsVisible = string.IsNullOrEmpty(d.Field<string>("visible")) ? false : Convert.ToBoolean(d.Field<string>("visible")),
											  PluginID = string.IsNullOrEmpty(d.Field<string>("pluginid")) ? Guid.Empty : new Guid(d.Field<string>("pluginid"))
										  }).ToList();

							foreach (var p in _modules) {
								p.PluginMenus = (from c in _ctrls
												 where c.PluginID == p.PluginID
												 select c).ToList();
							}

							_plugins = _plugins.Union(_modules).ToList();
						}
					}
				}
			}

			return _plugins;
		}



		public List<CMSPlugin> ToolboxPlugins {
			get {

				var _plugins = new List<CMSPlugin>();

				bool bCached = false;

				try {
					_plugins = (List<CMSPlugin>)HttpContext.Current.Cache[keyAdminToolboxModules];
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.PublicControls, "~/");

					List<CMSPlugin> _p1 = new List<CMSPlugin>();

					//_p1.Add(new CMSPlugin { Caption = "     Generic HTML &#0134;", FilePath = "~/Manage/ucGenericContent.ascx" });
					//_p1.Add(new CMSPlugin { Caption = "     Plain Text &#0134;", FilePath = "~/Manage/ucTextContent.ascx" });

					_p1.Add(new CMSPlugin { Caption = "     Generic HTML &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ContentRichText, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { Caption = "     Plain Text &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ContentPlainText, Carrotware.CMS.UI.Controls" });

					_p1.Add(new CMSPlugin { Caption = "   Top Level Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.TopLevelNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { Caption = "   Two Level Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.TwoLevelNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { Caption = "  Child Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ChildNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { Caption = "  Sibling Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.SiblingNavigation, Carrotware.CMS.UI.Controls" });

					List<CMSPlugin> _p2 = (from d in ds.Tables[0].AsEnumerable()
										   select new CMSPlugin {
											   FilePath = d.Field<string>("filepath"),
											   Caption = d.Field<string>("crtldesc")
										   }).ToList();

					_plugins = _p1.Union(_p2).Union(GetPluginsByDirectory()).ToList();

					HttpContext.Current.Cache.Insert(keyAdminToolboxModules, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}

				return _plugins.OrderBy(p => p.Caption).ToList();
			}
		}


		private List<CMSTemplate> GetTemplatesByDirectory() {
			var _plugins = new List<CMSTemplate>();

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

						if (File.Exists(sTplDef)) {
							string sPathPrefix = GetFolderPrefix(theDir);
							DataSet ds = ReadDataSetConfig(CMSConfigFileType.SkinDef, sPathPrefix);

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

				List<CMSTemplate> _plugins = null;
				string sDefTemplate = SiteData.DefaultTemplateFilename.ToLower();

				bool bCached = false;

				try {
					_plugins = (List<CMSTemplate>)HttpContext.Current.Cache[keyTemplates];
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					_plugins = new List<CMSTemplate>();
					CMSTemplate t = new CMSTemplate();
					t.TemplatePath = sDefTemplate;
					t.EncodedPath = EncodeBase64(sDefTemplate);
					t.Caption = "   Black 'n White - Plain L-R-C Content [*]   ";
					_plugins.Add(t);
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.SiteSkins, "~/");

					var _p1 = (from d in ds.Tables[0].AsEnumerable()
							   select new CMSTemplate {
								   TemplatePath = d.Field<string>("templatefile").ToLower(),
								   EncodedPath = EncodeBase64(d.Field<string>("templatefile").ToLower()),
								   Caption = d.Field<string>("filedesc").Trim()
							   }).ToList();

					var _p2 = GetTemplatesByDirectory();

					_plugins = _plugins.Union(_p1.Where(t => t.TemplatePath.ToLower() != sDefTemplate)).ToList().
						Union(_p2.Where(t => t.TemplatePath.ToLower() != sDefTemplate)).ToList();

					HttpContext.Current.Cache.Insert(keyTemplates, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}

				return _plugins.OrderBy(t => t.Caption).ToList();
			}
		}

		public static List<DynamicSite> SiteList {
			get {

				var _sites = new List<DynamicSite>();

				bool bCached = false;

				try {
					_sites = (List<DynamicSite>)HttpContext.Current.Cache[keyDynamicSite];
					if (_sites != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.SiteMapping, "~/");

					_sites = (from d in ds.Tables[0].AsEnumerable()
							  select new DynamicSite {
								  DomainName = string.IsNullOrEmpty(d.Field<string>("domname")) ? "" : d.Field<string>("domname").ToLower(),
								  SiteID = new Guid(d.Field<string>("siteid"))
							  }).ToList();

					HttpContext.Current.Cache.Insert(keyDynamicSite, _sites, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _sites;
			}
		}


		public static DynamicSite DynSite {
			get {
				DynamicSite _site = new DynamicSite();

				string ModuleKey = keyDynSite + DomainName;
				bool bCached = false;

				try {
					_site = (DynamicSite)HttpContext.Current.Cache[ModuleKey];
					if (_site != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if ((SiteList.Count > 0) && !bCached) {

					_site = (from ss in SiteList
							 where ss.DomainName == DomainName
							 select ss).FirstOrDefault();

					HttpContext.Current.Cache.Insert(ModuleKey, _site, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _site;
			}
		}


		public static string DecodeBase64(string ValIn) {
			string val = "";
			if (!string.IsNullOrEmpty(ValIn)) {
				ASCIIEncoding encoding = new ASCIIEncoding();
				val = encoding.GetString(Convert.FromBase64String(ValIn));
			}
			return val;
		}

		public static string EncodeBase64(string ValIn) {
			string val = "";
			if (!string.IsNullOrEmpty(ValIn)) {
				ASCIIEncoding encoding = new ASCIIEncoding();
				byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(ValIn);
				val = System.Convert.ToBase64String(toEncodeAsBytes);
			}
			return val;
		}


		public void OverrideKey(Guid guidContentID) {
			filePage = null;
			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				filePage = pageHelper.FindContentByID(SiteData.CurrentSiteID, guidContentID);
			}
		}

		public void OverrideKey(string sPageName) {
			filePage = null;
			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				filePage = pageHelper.FindByFilename(SiteData.CurrentSiteID, sPageName);
			}
		}

		protected ContentPage filePage = null;

		protected void LoadGuids() {
			if (filePage == null) {
				using (ContentPageHelper pageHelper = new ContentPageHelper()) {
					if (SiteData.CurrentScriptName.ToLower().StartsWith("/manage/")) {
						Guid guidPage = Guid.Empty;
						if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pageid"])) {
							guidPage = new Guid(HttpContext.Current.Request.QueryString["pageid"].ToString());
						}
						filePage = pageHelper.FindContentByID(SiteData.CurrentSiteID, guidPage);
					} else {
						filePage = pageHelper.FindByFilename(SiteData.CurrentSiteID, SiteData.CurrentScriptName);
					}
					if (SiteData.IsPageSampler && filePage == null) {
						filePage = ContentPageHelper.GetSamplerView();
					}
				}
			}

		}

		public ContentPage cmsAdminContent {
			get {
				ContentPage c = null;
				try {
					string sXML = GetSerialized(keyAdminContent);
					if (!string.IsNullOrEmpty(sXML)) {
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPage));
						Object genpref = null;
						using (StringReader stringReader = new StringReader(sXML)) {
							genpref = xmlSerializer.Deserialize(stringReader);
						}
						c = genpref as ContentPage;
					}
				} catch (Exception ex) { }
				return c;
			}
			set {
				if (value == null) {
					ClearSerialized(keyAdminContent);
				} else {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPage));
					string sXML = "";
					using (StringWriter stringWriter = new StringWriter()) {
						xmlSerializer.Serialize(stringWriter, value);
						sXML = stringWriter.ToString();
					}
					SaveSerialized(keyAdminContent, sXML);
				}
			}
		}

		public List<Widget> cmsAdminWidget {
			get {
				List<Widget> c = null;
				string sXML = GetSerialized(keyAdminWidget);
				if (!string.IsNullOrEmpty(sXML)) {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Widget>));
					Object genpref = null;
					using (StringReader stringReader = new StringReader(sXML)) {
						genpref = xmlSerializer.Deserialize(stringReader);
					}
					c = genpref as List<Widget>;
				}
				return c;
			}
			set {
				if (value == null) {
					ClearSerialized(keyAdminWidget);
				} else {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Widget>));
					string sXML = "";
					using (StringWriter stringWriter = new StringWriter()) {
						xmlSerializer.Serialize(stringWriter, value);
						sXML = stringWriter.ToString();
					}
					SaveSerialized(keyAdminWidget, sXML);
				}
			}
		}


		public static void SaveSerialized(Guid itemID, string sKey, string sData) {

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				bool bAdd = false;

				carrot_SerialCache itm = CompiledQueries.SearchSeriaCache(_db, itemID, sKey);

				if (itm == null) {
					bAdd = true;
					itm = new carrot_SerialCache();
					itm.SerialCacheID = Guid.NewGuid();
					itm.SiteID = SiteData.CurrentSiteID;
					itm.ItemID = itemID;
					itm.EditUserId = SecurityData.CurrentUserGuid;
					itm.KeyType = sKey;
				}

				itm.SerializedData = sData;
				itm.EditDate = DateTime.Now;

				if (bAdd) {
					_db.carrot_SerialCaches.InsertOnSubmit(itm);
				}
				_db.SubmitChanges();
			}
		}


		public static string GetSerialized(Guid itemID, string sKey) {
			string sData = "";
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {

				carrot_SerialCache itm = CompiledQueries.SearchSeriaCache(_db, itemID, sKey);

				if (itm != null) {
					sData = itm.SerializedData;
				}
			}
			return sData;
		}


		public static bool ClearSerialized(Guid itemID, string sKey) {
			bool bRet = false;
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {

				carrot_SerialCache itm = CompiledQueries.SearchSeriaCache(_db, itemID, sKey);

				if (itm != null) {
					_db.carrot_SerialCaches.DeleteOnSubmit(itm);
					_db.SubmitChanges();
					bRet = true;
				}
			}
			return bRet;
		}



		private void SaveSerialized(string sKey, string sData) {
			LoadGuids();

			CMSConfigHelper.SaveSerialized(filePage.Root_ContentID, sKey, sData);
		}


		private string GetSerialized(string sKey) {
			string sData = "";
			LoadGuids();

			sData = CMSConfigHelper.GetSerialized(filePage.Root_ContentID, sKey);

			return sData;
		}


		private bool ClearSerialized(string sKey) {
			LoadGuids();

			return CMSConfigHelper.ClearSerialized(filePage.Root_ContentID, sKey); ;
		}

	}





}