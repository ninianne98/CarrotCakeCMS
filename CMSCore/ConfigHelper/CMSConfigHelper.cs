﻿using Carrotware.CMS.Data;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Xml.Serialization;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class CMSConfigHelper : IDisposable {
		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;

		public CMSConfigHelper() {
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
		}

		private enum CMSConfigFileType {
			AdminMod,
			AdminModules,
			PublicControls,
			SkinDef,
			PublicCtrl,
			SiteSkins,
			SiteTextWidgets,
			SiteMapping
		}

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion IDisposable Members

		private static string keyAdminMenuModules = "cms_AdminMenuModules";

		private static string keyAdminToolboxModules = "cms_AdminToolboxModules";

		private static string keyDynamicSite = "cms_DynamicSite";

		private static string keyTemplateFiles = "cms_TemplateFiles";

		private static string keyTemplates = "cms_Templates";

		private static string keyTxtWidgets = "cms_TxtWidgets";

		private static string keyDynSite = "cms_DynSite_";

		public static string keyAdminContent = "cmsAdminContent";

		public static string keyAdminWidget = "cmsAdminWidget";

		public void ResetConfigs() {
			HttpContext.Current.Cache.Remove(keyAdminMenuModules);

			HttpContext.Current.Cache.Remove(keyAdminToolboxModules);

			HttpContext.Current.Cache.Remove(keyDynamicSite);

			HttpContext.Current.Cache.Remove(keyTemplates);

			HttpContext.Current.Cache.Remove(keyTxtWidgets);

			HttpContext.Current.Cache.Remove(keyTemplateFiles);

			string ModuleKey = keyDynSite + DomainName;
			HttpContext.Current.Cache.Remove(ModuleKey);

			try {
				VirtualDirectory.RegisterRoutes(true);

				if (SiteData.CurretSiteExists) {
					SiteData.CurrentSite.LoadTextWidgets();
				}
			} catch (Exception ex) { }

			if (SiteData.CurrentTrustLevel == AspNetHostingPermissionLevel.Unrestricted) {
				System.Web.HttpRuntime.UnloadAppDomain();
			}
		}

		private static Page CachedPage {
			get {
				if (_CachedPage == null) {
					_CachedPage = new Page();
					_CachedPage.AppRelativeVirtualPath = "~/";
				}
				return _CachedPage;
			}
		}

		private static Page _CachedPage;

		public static string GetWebResourceUrl(Type type, string resource) {
			string sPath = string.Empty;

			try {
				sPath = CachedPage.ClientScript.GetWebResourceUrl(type, resource);
				sPath = HttpUtility.HtmlEncode(sPath);
			} catch { }

			return sPath;
		}

		public static string GetWebResourceUrl(Control X, Type type, string resource) {
			string sPath = string.Empty;

			if (X != null && X.Page != null) {
				sPath = X.Page.ClientScript.GetWebResourceUrl(type, resource);
			} else {
				sPath = GetWebResourceUrl(type, resource);
			}

			try {
				sPath = HttpUtility.HtmlEncode(sPath);
			} catch { }

			return sPath;
		}

		public static string DomainName {
			get {
				var domName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
				if (domName.IndexOf(":") > 0) {
					domName = domName.Substring(0, domName.IndexOf(":"));
				}

				return domName.ToLowerInvariant();
			}
		}

		public static bool HasAdminModules() {
			using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
				return cmsHelper.AdminModules.Any();
			}
		}

		public static FileDataHelper GetFileDataHelper() {
			string fileTypes = null;

			CarrotCakeConfig config = CarrotCakeConfig.GetConfig();
			if (config.FileManagerConfig != null && !string.IsNullOrEmpty(config.FileManagerConfig.BlockedExtensions)) {
				fileTypes = config.FileManagerConfig.BlockedExtensions;
			}

			return new FileDataHelper(fileTypes);
		}

		private static DataSet ReadDataSetConfig(CMSConfigFileType cfg, string sPath) {
			string sPlugCfg = "default.config";
			string sRealPath = HttpContext.Current.Server.MapPath(sPath);
			CarrotCakeConfig config = CarrotCakeConfig.GetConfig();

			int iExpectedTblCount = 1;

			switch (cfg) {
				case CMSConfigFileType.AdminModules:
					sPlugCfg = sRealPath + config.ConfigFileLocation.AdminModules;
					iExpectedTblCount = 2;
					break;

				case CMSConfigFileType.PublicControls:
					sPlugCfg = sRealPath + config.ConfigFileLocation.PublicControls;
					break;

				case CMSConfigFileType.SiteSkins:
					sPlugCfg = sRealPath + config.ConfigFileLocation.SiteSkins;
					break;

				case CMSConfigFileType.SiteTextWidgets:
					sPlugCfg = sRealPath + config.ConfigFileLocation.TextContentProcessors;
					break;

				case CMSConfigFileType.SiteMapping:
					sPlugCfg = sRealPath + config.ConfigFileLocation.SiteMapping;
					break;

				case CMSConfigFileType.AdminMod:
					sPlugCfg = sRealPath + "Admin.config";
					iExpectedTblCount = 2;
					break;

				case CMSConfigFileType.PublicCtrl:
					sPlugCfg = sRealPath + "Public.config";
					break;

				case CMSConfigFileType.SkinDef:
					sPlugCfg = sRealPath + "Skin.config";
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

				string table1Name = string.Empty;

				List<string> reqCols0 = new List<string>();
				List<string> reqCols1 = new List<string>();

				switch (cfg) {
					case CMSConfigFileType.AdminMod:
					case CMSConfigFileType.AdminModules:
						reqCols0.Add("caption");
						reqCols0.Add("pluginid");
						table1Name = "pluginlist";

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
						table1Name = "ctrlfile";

						break;

					case CMSConfigFileType.SkinDef:
					case CMSConfigFileType.SiteSkins:
						reqCols0.Add("templatefile");
						reqCols0.Add("filedesc");
						table1Name = "pagenames";

						break;

					case CMSConfigFileType.SiteTextWidgets:
						reqCols0.Add("pluginassembly");
						reqCols0.Add("pluginname");
						table1Name = "plugin";

						break;

					case CMSConfigFileType.SiteMapping:
						reqCols0.Add("domname");
						reqCols0.Add("siteid");
						table1Name = "sitedetail";

						break;

					default:
						reqCols0.Add("caption");
						reqCols0.Add("pluginid");
						table1Name = "none";

						break;
				}

				if (ds.Tables.Contains(table1Name)) {
					//validate that the dataset has the right table configuration
					DataTable dt0 = ds.Tables[table1Name];
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
			}

			return ds;
		}

		public string GetFolderPrefix(string sDirPath) {
			return FileDataHelper.MakeWebFolderPath(sDirPath);
		}

		public static object GetCacheItem(string key) {
			if (HttpContext.Current.Cache[key] != null) {
				return HttpContext.Current.Cache[key];
			}
			return null;
		}

		public static string GetCacheItemString(string key) {
			var item = GetCacheItem(key);
			return item != null ? item.ToString() : null;
		}

		public List<CMSAdminModule> AdminModules {
			get {
				var _modules = new List<CMSAdminModule>();

				bool bCached = false;

				try {
					_modules = (List<CMSAdminModule>)GetCacheItem(keyAdminMenuModules);
					if (_modules != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.AdminModules, "~/");

					List<CMSAdminModuleMenu> _ctrls = new List<CMSAdminModuleMenu>();
					_modules = new List<CMSAdminModule>();

					if (ds.Tables.Count == 2) {
						_modules = (from d in ds.Tables[0].AsEnumerable()
									select new CMSAdminModule {
										PluginName = d.Field<string>("caption"),
										PluginID = new Guid(d.Field<string>("pluginid"))
									}).OrderBy(x => x.PluginName).ToList();

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
											  }).OrderBy(x => x.Caption).OrderBy(x => x.SortOrder).ToList();

								_ctrls = _ctrls.Union(_ctrl2).ToList();
							}
						}
					}

					foreach (var p in _modules) {
						p.PluginMenus = (from c in _ctrls
										 where c.PluginID == p.PluginID
										 orderby c.Caption, c.SortOrder
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

			CarrotCakeConfig config = CarrotCakeConfig.GetConfig();

			string sPlugCfg = HttpContext.Current.Server.MapPath(config.ConfigFileLocation.PluginPath);

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
										   SortOrder = 100,
										   FilePath = "~" + sPathPrefix + d.Field<string>("filepath"),
										   Caption = d.Field<string>("crtldesc")
									   }).ToList();

							_plugins = _plugins.Union(_p2).ToList();
						}
					}
				}
			}

			_plugins.Where(x => x.FilePath.StartsWith("~~/")).ToList().ForEach(r => r.FilePath = r.FilePath.Replace("~~/", "~/"));

			return _plugins;
		}

		public List<CMSPlugin> GetPluginsInFolder(string sPathPrefix) {
			var _plugins = new List<CMSPlugin>();

			if (!sPathPrefix.EndsWith("/")) {
				sPathPrefix = sPathPrefix + "/";
			}

			if (!string.IsNullOrEmpty(sPathPrefix)) {
				DataSet ds = ReadDataSetConfig(CMSConfigFileType.PublicCtrl, sPathPrefix);

				_plugins = (from d in ds.Tables[0].AsEnumerable()
							select new CMSPlugin {
								SortOrder = 100,
								FilePath = "~" + sPathPrefix + d.Field<string>("filepath"),
								Caption = d.Field<string>("crtldesc")
							}).ToList();
			}

			_plugins.Where(x => x.FilePath.StartsWith("~~/")).ToList().ForEach(r => r.FilePath = r.FilePath.Replace("~~/", "~/"));

			return _plugins;
		}

		public CMSAdminModuleMenu GetCurrentAdminModuleControl() {
			Guid moduleId = Guid.Empty;
			var context = HttpContext.Current;
			var request = HttpContext.Current.Request;
			string pf = string.Empty;
			CMSAdminModuleMenu cc = null;

			moduleId = context.GetGuidParmFromQuery("pi");

			if (request.QueryString["pf"] != null) {
				pf = request.QueryString["pf"].ToString();

				CMSAdminModule mod = (from m in this.AdminModules
									  where m.PluginID == moduleId
									  select m).FirstOrDefault();

				cc = (from m in mod.PluginMenus
					  orderby m.Caption, m.SortOrder
					  where m.PluginParm == pf
					  select m).FirstOrDefault();
			}

			return cc;
		}

		public List<CMSAdminModuleMenu> GetCurrentAdminModuleControlList() {
			Guid moduleId = Guid.Empty;
			var context = HttpContext.Current;
			var request = HttpContext.Current.Request;
			string pf = string.Empty;
			List<CMSAdminModuleMenu> cc = null;

			moduleId = context.GetGuidParmFromQuery("pi");

			if (request.QueryString["pf"] != null) {
				pf = request.QueryString["pf"].ToString();

				CMSAdminModule mod = (from m in this.AdminModules
									  where m.PluginID == moduleId
									  select m).FirstOrDefault();

				cc = (from m in mod.PluginMenus
					  orderby m.Caption, m.SortOrder
					  select m).ToList();
			}

			return cc;
		}

		public void GetFile(string remoteFile, string localFile) {
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

			Uri remoteUri = new Uri(remoteFile);
			string serverPath = HttpContext.Current.Server.MapPath(localFile);
			bool bExists = File.Exists(serverPath);

			if (!bExists) {
				using (var webClient = new WebClient()) {
					try {
						webClient.DownloadFile(remoteUri, serverPath);
					} catch (Exception ex) {
						if (ex is WebException) {
							WebException webException = (WebException)ex;
							var resp = (HttpWebResponse)webException.Response;
							if (!(resp.StatusCode == HttpStatusCode.NotFound)) {
								throw;
							}
						} else {
							throw;
						}
					}
				}
			}
		}

		private List<CMSAdminModule> GetModulesByDirectory() {
			var _plugins = new List<CMSAdminModule>();

			CarrotCakeConfig config = CarrotCakeConfig.GetConfig();

			string sPlugCfg = HttpContext.Current.Server.MapPath(config.ConfigFileLocation.PluginPath);

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
											}).OrderBy(x => x.PluginName).ToList();

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
										  }).OrderBy(x => x.Caption).OrderBy(x => x.SortOrder).ToList();

							foreach (var p in _modules) {
								p.PluginMenus = (from c in _ctrls
												 where c.PluginID == p.PluginID
												 orderby c.Caption, c.SortOrder
												 select c).ToList();
							}

							_plugins = _plugins.Union(_modules).ToList();
						}
					}
				}
			}

			return _plugins;
		}

		public List<CMSTextWidgetPicker> GetAllWidgetSettings(Guid siteID) {
			List<TextWidget> lstPreferenced = TextWidget.GetSiteTextWidgets(siteID);

			List<string> lstInUse = lstPreferenced.Select(x => x.TextWidgetAssembly).Distinct().ToList();

			List<string> lstAvail = TextWidgets.Select(x => x.AssemblyString).Distinct().ToList();

			List<CMSTextWidgetPicker> lstExisting = (from p in lstPreferenced
													 join t in TextWidgets on p.TextWidgetAssembly equals t.AssemblyString
													 select new CMSTextWidgetPicker {
														 TextWidgetPickerID = p.TextWidgetID,
														 AssemblyString = p.TextWidgetAssembly,
														 DisplayName = t.DisplayName,
														 ProcessBody = p.ProcessBody,
														 ProcessPlainText = p.ProcessPlainText,
														 ProcessHTMLText = p.ProcessHTMLText,
														 ProcessComment = p.ProcessComment,
														 ProcessSnippet = p.ProcessSnippet,
													 }).ToList();

			List<CMSTextWidgetPicker> lstConfigured1 = (from t in TextWidgets
														where !lstInUse.Contains(t.AssemblyString)
														select new CMSTextWidgetPicker {
															TextWidgetPickerID = Guid.NewGuid(),
															AssemblyString = t.AssemblyString,
															DisplayName = t.DisplayName,
															ProcessBody = false,
															ProcessPlainText = false,
															ProcessHTMLText = false,
															ProcessComment = false,
															ProcessSnippet = false,
														}).ToList();

			lstExisting = lstExisting.Union(lstConfigured1).ToList();

			List<CMSTextWidgetPicker> lstConfigured2 = (from p in lstPreferenced
														where !lstAvail.Contains(p.TextWidgetAssembly)
														select new CMSTextWidgetPicker {
															TextWidgetPickerID = p.TextWidgetID,
															AssemblyString = p.TextWidgetAssembly,
															DisplayName = string.Empty,
															ProcessBody = p.ProcessBody,
															ProcessPlainText = p.ProcessPlainText,
															ProcessHTMLText = p.ProcessHTMLText,
															ProcessComment = p.ProcessComment,
															ProcessSnippet = p.ProcessSnippet,
														}).ToList();

			lstExisting = lstExisting.Union(lstConfigured2).ToList();

			return lstExisting;
		}

		public List<CMSPlugin> ToolboxPlugins {
			get {
				var _plugins = new List<CMSPlugin>();

				bool bCached = false;

				try {
					_plugins = (List<CMSPlugin>)GetCacheItem(keyAdminToolboxModules);
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.PublicControls, "~/");
					int iSortOrder = 0;

					List<CMSPlugin> _p1 = new List<CMSPlugin>();

					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Generic HTML &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ContentRichText, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Plain Text &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ContentPlainText, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Content Snippet &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ContentSnippetText, Carrotware.CMS.UI.Controls" });

					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Top Level Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.TopLevelNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Two Level Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.TwoLevelNavigation, Carrotware.CMS.UI.Controls" });

					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Child Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ChildNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Sibling Navigation &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.SiblingNavigation, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Most Recent Updated &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.MostRecentUpdated, Carrotware.CMS.UI.Controls" });

					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Multi Level Nav List &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.MultiLevelNavigation, Carrotware.CMS.UI.Controls" });

					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Paged Data &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.PagedDataSummary, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Paged Comments &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.PagedComments, Carrotware.CMS.UI.Controls" });

					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Meta List - Site &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.SiteMetaWordList, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Meta List - Post &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.PostMetaWordList, Carrotware.CMS.UI.Controls" });

					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "Comment Form &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.ContentCommentForm, Carrotware.CMS.UI.Controls" });
					_p1.Add(new CMSPlugin { SortOrder = iSortOrder++, Caption = "IFRAME content wrapper &#0134;", FilePath = "CLASS:Carrotware.CMS.UI.Controls.IFrameWidgetWrapper, Carrotware.CMS.UI.Controls" });

					List<CMSPlugin> _p2 = (from d in ds.Tables[0].AsEnumerable()
										   select new CMSPlugin {
											   SortOrder = 100,
											   FilePath = d.Field<string>("filepath"),
											   Caption = d.Field<string>("crtldesc")
										   }).ToList();

					_plugins = _p1.Union(_p2).Union(GetPluginsByDirectory()).ToList();

					_plugins.Where(x => x.FilePath.StartsWith("~~/")).ToList().ForEach(r => r.FilePath = r.FilePath.Replace("~~/", "~/"));

					HttpContext.Current.Cache.Insert(keyAdminToolboxModules, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}

				return _plugins.OrderBy(p => p.Caption).OrderBy(p => p.SortOrder).ToList();
			}
		}

		private List<CMSTemplate> GetTemplatesByDirectory() {
			var _plugins = new List<CMSTemplate>();

			CarrotCakeConfig config = CarrotCakeConfig.GetConfig();

			string sPlugCfg = HttpContext.Current.Server.MapPath(config.ConfigFileLocation.TemplatePath);

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
										   TemplatePath = (sPathPrefix + d.Field<string>("templatefile").ToLowerInvariant()).ToLowerInvariant(),
										   EncodedPath = EncodeBase64((sPathPrefix + d.Field<string>("templatefile").ToLowerInvariant()).ToLowerInvariant()),
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

				bool bCached = false;

				try {
					_plugins = (List<CMSTemplate>)GetCacheItem(keyTemplates);
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					var site = SiteData.CurrentSite;
					_plugins = new List<CMSTemplate>();

					var t1 = new CMSTemplate();
					t1.TemplatePath = site.TemplateFilename;
					t1.EncodedPath = EncodeBase64(site.TemplateFilename);
					t1.Caption = string.Format("    {0} [*]  ", ReflectionUtilities.DescriptionFor<SiteData>(x => x.TemplateFilename));
					_plugins.Add(t1);

					var t2 = new CMSTemplate();
					t2.TemplatePath = site.TemplateBWFilename;
					t2.EncodedPath = EncodeBase64(site.TemplateBWFilename);
					t2.Caption = string.Format("   {0} [*]  ", ReflectionUtilities.DescriptionFor<SiteData>(x => x.TemplateBWFilename));
					_plugins.Add(t2);
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.SiteSkins, "~/");

					var _p1 = (from d in ds.Tables[0].AsEnumerable()
							   select new CMSTemplate {
								   TemplatePath = d.Field<string>("templatefile").ToLowerInvariant(),
								   EncodedPath = EncodeBase64(d.Field<string>("templatefile").ToLowerInvariant()),
								   Caption = d.Field<string>("filedesc").Trim()
							   }).ToList();

					var _p2 = GetTemplatesByDirectory();

					_plugins = _plugins.Union(_p1.Where(t => !SiteData.DefaultTemplates.Contains(t.TemplatePath.ToLowerInvariant()))).ToList()
										.Union(_p2.Where(t => !SiteData.DefaultTemplates.Contains(t.TemplatePath.ToLowerInvariant()))).ToList();

					HttpContext.Current.Cache.Insert(keyTemplates, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}

				return _plugins.OrderBy(t => t.Caption).ToList();
			}
		}

		public List<CMSTextWidget> TextWidgets {
			get {
				List<CMSTextWidget> _plugins = null;
				bool bCached = false;

				try {
					_plugins = (List<CMSTextWidget>)GetCacheItem(keyTxtWidgets);
					if (_plugins != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if (!bCached) {
					_plugins = new List<CMSTextWidget>();
				}

				if (!bCached) {
					DataSet ds = ReadDataSetConfig(CMSConfigFileType.SiteTextWidgets, "~/");

					_plugins = (from d in ds.Tables[0].AsEnumerable()
								select new CMSTextWidget {
									AssemblyString = d.Field<string>("pluginassembly"),
									DisplayName = d.Field<string>("pluginname")
								}).ToList();

					HttpContext.Current.Cache.Insert(keyTxtWidgets, _plugins, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}

				return _plugins.OrderBy(t => t.DisplayName).ToList();
			}
		}

		public static List<DynamicSite> SiteList {
			get {
				var _sites = new List<DynamicSite>();

				bool bCached = false;

				try {
					_sites = (List<DynamicSite>)GetCacheItem(keyDynamicSite);
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
								  DomainName = string.IsNullOrEmpty(d.Field<string>("domname")) ? string.Empty : d.Field<string>("domname").ToLowerInvariant(),
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
					_site = (DynamicSite)GetCacheItem(ModuleKey);
					if (_site != null) {
						bCached = true;
					}
				} catch {
					bCached = false;
				}

				if ((SiteList.Any()) && !bCached) {
					_site = (from ss in SiteList
							 where ss.DomainName == DomainName
							 select ss).FirstOrDefault();

					HttpContext.Current.Cache.Insert(ModuleKey, _site, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
				}
				return _site;
			}
		}

		public static bool CheckRequestedFileExistence(string templateFileName, Guid siteID) {
			var _tmplts = GetTmplateStatus();

			CMSFilePath tmp = _tmplts.Where(x => x.TemplateFile.ToLowerInvariant() == templateFileName.ToLowerInvariant() && x.SiteID == siteID).FirstOrDefault();

			if (tmp == null) {
				tmp = new CMSFilePath(templateFileName, siteID);
				_tmplts.Add(tmp);
#if DEBUG
				Debug.WriteLine(" ================ " + DateTime.UtcNow.ToString() + " ================");
				Debug.WriteLine("Grabbed file : CheckRequestedFileExistence(string templateFileName, Guid siteID) " + templateFileName);
#endif
			}

			SaveTmplateStatus(_tmplts);

			return tmp.FileExists;
		}

		public static bool CheckFileExistence(string templateFileName) {
			var _tmplts = GetTmplateStatus();

			CMSFilePath tmp = _tmplts.Where(x => x.TemplateFile.ToLowerInvariant() == templateFileName.ToLowerInvariant() && x.SiteID == Guid.Empty).FirstOrDefault();

			if (tmp == null) {
				tmp = new CMSFilePath(templateFileName);
				_tmplts.Add(tmp);
#if DEBUG
				Debug.WriteLine(" ================ " + DateTime.UtcNow.ToString() + " ================");
				Debug.WriteLine("Grabbed file : CheckFileExistence(string templateFileName) " + templateFileName);
#endif
			}

			SaveTmplateStatus(_tmplts);

			return tmp.FileExists;
		}

		private static void SaveTmplateStatus(List<CMSFilePath> fileState) {
			HttpContext.Current.Cache.Insert(keyTemplateFiles, fileState, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);
		}

		private static List<CMSFilePath> GetTmplateStatus() {
			var _tmplts = new List<CMSFilePath>();

			try { _tmplts = (List<CMSFilePath>)GetCacheItem(keyTemplateFiles); } catch { }

			if (_tmplts == null) {
				_tmplts = new List<CMSFilePath>();
			}

			_tmplts.RemoveAll(x => x.DateChecked < DateTime.UtcNow.AddSeconds(-30));

			_tmplts.RemoveAll(x => x.DateChecked < DateTime.UtcNow.AddSeconds(-10) && x.SiteID != Guid.Empty);

			return _tmplts;
		}

		//=========================

		public static TimeZoneInfo GetLocalTimeZoneInfo() {
			TimeZoneInfo oTZ = TimeZoneInfo.Local;

			return oTZ;
		}

		public static TimeZoneInfo GetSiteTimeZoneInfo(string timeZoneIdentifier) {
			TimeZoneInfo oTZ = GetLocalTimeZoneInfo();

			if (!string.IsNullOrEmpty(timeZoneIdentifier)) {
				try { oTZ = TimeZoneInfo.FindSystemTimeZoneById(timeZoneIdentifier); } catch { }
			}

			return oTZ;
		}

		public static DateTime ConvertUTCToSiteTime(DateTime dateUTC, string timeZoneIdentifier) {
			TimeZoneInfo oTZ = GetSiteTimeZoneInfo(timeZoneIdentifier);

			return TimeZoneInfo.ConvertTimeFromUtc(dateUTC, oTZ);
		}

		public static DateTime ConvertSiteTimeToUTC(DateTime dateSite, string timeZoneIdentifier) {
			TimeZoneInfo oTZ = GetSiteTimeZoneInfo(timeZoneIdentifier);

			return TimeZoneInfo.ConvertTimeToUtc(dateSite, oTZ);
		}

		//===================

		public static string InactivePagePrefix {
			get {
				return "&#9746; ";
			}
		}

		public static string RetiredPagePrefix {
			get {
				return "&#9851; ";
			}
		}

		public static string UnreleasedPagePrefix {
			get {
				return "&#9888; ";
			}
		}

		public static string PendingDeletePrefix {
			get {
				return "&#9940; ";
			}
		}

		public static List<SiteNav> TweakData(List<SiteNav> navs, bool siteMap, bool siteNav) {
			if (navs != null) {
				if (siteMap) {
					navs.RemoveAll(x => x.ShowInSiteMap == false && x.ContentType == ContentPageType.PageType.ContentEntry);
				}
				if (siteNav) {
					navs.RemoveAll(x => x.ShowInSiteNav == false && x.ContentType == ContentPageType.PageType.ContentEntry);
				}

				navs.ForEach(q => FixNavLinkText(q));
			}

			return navs;
		}

		public static List<SiteNav> TweakData(List<SiteNav> navs) {
			return TweakData(navs, true, true);
		}

		public static SiteNav FixNavLinkText(SiteNav nav) {
			if (nav != null && !nav.MadeSafe) {
				nav.MadeSafe = true;
				nav.NavMenuText = HttpUtility.HtmlEncode(nav.NavMenuText);
				nav.PageHead = HttpUtility.HtmlEncode(nav.PageHead);
				nav.TitleBar = HttpUtility.HtmlEncode(nav.TitleBar);

				if (!nav.PageActive) {
					nav.NavMenuText = InactivePagePrefix + nav.NavMenuText;
					nav.PageHead = InactivePagePrefix + nav.PageHead;
					nav.TitleBar = InactivePagePrefix + nav.TitleBar;
				}
				if (nav.IsRetired) {
					nav.NavMenuText = RetiredPagePrefix + nav.NavMenuText;
					nav.PageHead = RetiredPagePrefix + nav.PageHead;
					nav.TitleBar = RetiredPagePrefix + nav.TitleBar;
				}
				if (nav.IsUnReleased) {
					nav.NavMenuText = UnreleasedPagePrefix + nav.NavMenuText;
					nav.PageHead = UnreleasedPagePrefix + nav.PageHead;
					nav.TitleBar = UnreleasedPagePrefix + nav.TitleBar;
				}
			}
			return nav;
		}

		public static ContentPage FixNavLinkText(ContentPage cp) {
			if (cp != null && !cp.MadeSafe) {
				cp.MadeSafe = true;
				cp.NavMenuText = HttpUtility.HtmlEncode(cp.NavMenuText);
				cp.PageHead = HttpUtility.HtmlEncode(cp.PageHead);
				cp.TitleBar = HttpUtility.HtmlEncode(cp.TitleBar);

				if (!cp.PageActive) {
					cp.NavMenuText = InactivePagePrefix + cp.NavMenuText;
					cp.PageHead = InactivePagePrefix + cp.PageHead;
					cp.TitleBar = InactivePagePrefix + cp.TitleBar;
				}
				if (cp.IsRetired) {
					cp.NavMenuText = RetiredPagePrefix + cp.NavMenuText;
					cp.PageHead = RetiredPagePrefix + cp.PageHead;
					cp.TitleBar = RetiredPagePrefix + cp.TitleBar;
				}
				if (cp.IsUnReleased) {
					cp.NavMenuText = UnreleasedPagePrefix + cp.NavMenuText;
					cp.PageHead = UnreleasedPagePrefix + cp.PageHead;
					cp.TitleBar = UnreleasedPagePrefix + cp.TitleBar;
				}
			}

			return cp;
		}

		//=====================

		public static DateTime CalcNearestFiveMinTime(DateTime dateIn) {
			DateTime dateOut = dateIn.AddMinutes(-2);
			int iMin = 5 * (dateOut.Minute / 5);

			dateOut = dateOut.AddMinutes(0 - dateOut.Minute).AddMinutes(iMin);

			return dateOut;
		}

		public static string DecodeBase64(string text) {
			return text.DecodeBase64();
		}

		public static string EncodeBase64(string text) {
			return text.EncodeBase64();
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
					if (SiteData.CurrentScriptName.ToLowerInvariant().StartsWith(SiteData.AdminFolderPath)) {
						var pageId = HttpContext.Current.GetGuidParmFromQuery("pageid");
						filePage = pageHelper.FindContentByID(SiteData.CurrentSiteID, pageId);
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
					string sXML = string.Empty;
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
					string sXML = string.Empty;
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
				itm.EditDate = DateTime.UtcNow;

				if (bAdd) {
					_db.carrot_SerialCaches.InsertOnSubmit(itm);
				}
				_db.SubmitChanges();
			}
		}

		public static string GetSerialized(Guid itemID, string sKey) {
			string sData = string.Empty;
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
			if (filePage != null) {
				CMSConfigHelper.SaveSerialized(filePage.Root_ContentID, sKey, sData);
			}
		}

		private string GetSerialized(string sKey) {
			string sData = string.Empty;
			LoadGuids();

			if (filePage != null) {
				sData = CMSConfigHelper.GetSerialized(filePage.Root_ContentID, sKey);
			}
			return sData;
		}

		private bool ClearSerialized(string sKey) {
			LoadGuids();
			if (filePage != null) {
				return CMSConfigHelper.ClearSerialized(filePage.Root_ContentID, sKey);
			} else {
				return false;
			}
		}

		public static void CleanUpSerialData() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<carrot_SerialCache> lst = (from c in _db.carrot_SerialCaches
													  where c.EditDate < DateTime.UtcNow.AddHours(-6)
													  select c);

				_db.carrot_SerialCaches.BatchDelete(lst);
				_db.SubmitChanges();
			}
		}
	}
}