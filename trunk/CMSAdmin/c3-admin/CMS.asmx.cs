using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Xml.Serialization;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {
	/// <summary>
	/// Summary description for CMS
	/// </summary>
	[WebService(Namespace = "http://carrotware.com/cms/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[System.Web.Script.Services.ScriptService]

	public class CMS : System.Web.Services.WebService {

		//private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;

		private ContentPageHelper pageHelper = new ContentPageHelper();
		private WidgetHelper widgetHelper = new WidgetHelper();
		private SiteMapOrderHelper sitemapHelper = new SiteMapOrderHelper();

		private Guid CurrentPageGuid = Guid.Empty;
		private ContentPage filePage = null;

		private List<ContentPage> _pages = null;
		protected List<ContentPage> lstActivePages {
			get {
				if (_pages == null) {
					_pages = pageHelper.GetLatestContentList(SiteData.CurrentSite.SiteID, true);
				}
				return _pages;
			}
		}

		public ContentPage cmsAdminContent {
			get {
				ContentPage c = null;
				try {
					string sXML = GetSerialized(CMSConfigHelper.keyAdminContent);
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPage));
					Object genpref = null;
					using (StringReader stringReader = new StringReader(sXML)) {
						genpref = xmlSerializer.Deserialize(stringReader);
					}
					c = genpref as ContentPage;
				} catch { }
				return c;
			}
			set {
				if (value == null) {
					ClearSerialized(CMSConfigHelper.keyAdminContent);
				} else {
					string sXML = "";
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContentPage));
					using (StringWriter stringWriter = new StringWriter()) {
						xmlSerializer.Serialize(stringWriter, value);
						sXML = stringWriter.ToString();
					}
					SaveSerialized(CMSConfigHelper.keyAdminContent, sXML);

				}
			}
		}

		public List<Widget> cmsAdminWidget {
			get {
				List<Widget> c = null;
				string sXML = GetSerialized(CMSConfigHelper.keyAdminWidget);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Widget>));
				Object genpref = null;
				using (StringReader stringReader = new StringReader(sXML)) {
					genpref = xmlSerializer.Deserialize(stringReader);
				}
				c = genpref as List<Widget>;
				return c;
			}
			set {
				if (value == null) {
					ClearSerialized(CMSConfigHelper.keyAdminWidget);
				} else {
					string sXML = "";
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Widget>));
					using (StringWriter stringWriter = new StringWriter()) {
						xmlSerializer.Serialize(stringWriter, value);
						sXML = stringWriter.ToString();
					}
					SaveSerialized(CMSConfigHelper.keyAdminWidget, sXML);

				}
			}
		}


		private void SaveSerialized(string sKey, string sData) {
			LoadGuids();

			CMSConfigHelper.SaveSerialized(CurrentPageGuid, sKey, sData);
		}


		private string GetSerialized(string sKey) {
			string sData = "";
			LoadGuids();

			sData = CMSConfigHelper.GetSerialized(CurrentPageGuid, sKey);

			return sData;
		}


		private bool ClearSerialized(string sKey) {
			LoadGuids();

			return CMSConfigHelper.ClearSerialized(CurrentPageGuid, sKey); ;
		}

		private void LoadGuids() {
			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				if (!string.IsNullOrEmpty(CurrentEditPage)) {
					filePage = pageHelper.FindByFilename(SiteData.CurrentSite.SiteID, CurrentEditPage);
					if (filePage != null) {
						CurrentPageGuid = filePage.Root_ContentID;
					}
				} else {
					if (CurrentPageGuid != Guid.Empty) {
						filePage = pageHelper.FindContentByID(SiteData.CurrentSite.SiteID, CurrentPageGuid);
						if (filePage != null) {
							CurrentEditPage = filePage.FileName;
						}
					} else {
						filePage = new ContentPage();
					}
				}
			}
		}


		private string CurrentEditPage = "";


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string RecordHeartbeat(string PageID) {
			try {
				CurrentPageGuid = new Guid(PageID);

				bool bLock = pageHelper.IsPageLocked(CurrentPageGuid, SiteData.CurrentSite.SiteID, SecurityData.CurrentUserGuid);

				//only allow admin/editors to record a lock
				if ((SecurityData.IsAdmin || SecurityData.IsEditor) && !bLock) {
					bool bRet = pageHelper.RecordHeartbeatLock(CurrentPageGuid, SiteData.CurrentSite.SiteID, SecurityData.CurrentUserGuid);

					if (bRet) {
						return SiteData.CurrentSite.Now.ToString();
					} else {
						return Convert.ToDateTime("12/31/1899").ToString();
					}
				} else {
					return DateTime.MinValue.ToString();
				}

			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return DateTime.MinValue.ToString();
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string CancelEditing(string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);

				pageHelper.ResetHeartbeatLock(CurrentPageGuid, SiteData.CurrentSite.SiteID);

				GetSetUserEditState("", "", "", "");

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string SendTrackbackPageBatch(string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);
				if (CurrentPageGuid != Guid.Empty) {
					ContentPage cp = pageHelper.FindContentByID(SiteData.CurrentSite.SiteID, CurrentPageGuid);
					cp.SaveTrackbackTop();
				}
				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string SendTrackbackBatch() {
			try {
				if (SiteData.CurrentSite != null) {
					SiteData.CurrentSite.SendTrackbackQueue();
				}
				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string RecordEditorPosition(string ToolbarState, string ToolbarMargin, string ToolbarScroll, string SelTabID) {
			try {
				GetSetUserEditState(ToolbarState, ToolbarMargin, ToolbarScroll, SelTabID);

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		private void GetSetUserEditState(string ToolbarState, string ToolbarMargin, string ToolbarScroll, string SelTabID) {
			UserEditState editor = UserEditState.cmsUserEditState;

			if (editor == null) {
				editor = new UserEditState();
				editor.EditorMargin = "L";
				editor.EditorOpen = "true";
				editor.EditorScrollPosition = "0";
				editor.EditorSelectedTabIdx = "0";
			}

			editor.EditorMargin = string.IsNullOrEmpty(ToolbarMargin) ? "L" : ToolbarMargin.ToUpper();
			editor.EditorOpen = string.IsNullOrEmpty(ToolbarState) ? "true" : ToolbarState.ToLower();
			editor.EditorScrollPosition = string.IsNullOrEmpty(ToolbarScroll) ? "0" : ToolbarScroll.ToLower();
			editor.EditorSelectedTabIdx = string.IsNullOrEmpty(SelTabID) ? "0" : SelTabID.ToLower();

			if (string.IsNullOrEmpty(ToolbarMargin) && string.IsNullOrEmpty(ToolbarState)) {
				UserEditState.cmsUserEditState = null;
			} else {
				UserEditState.cmsUserEditState = editor;
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public List<SiteMapOrder> GetChildPages(string PageID, string CurrPageID) {

			Guid? ParentID = Guid.Empty;
			if (!string.IsNullOrEmpty(PageID)) {
				if (PageID.Length > 20) {
					ParentID = new Guid(PageID);
				}
			}

			Guid ContPageID = Guid.Empty;
			if (!string.IsNullOrEmpty(CurrPageID)) {
				if (CurrPageID.Length > 20) {
					ContPageID = new Guid(CurrPageID);
				}
			}

			List<SiteMapOrder> lstSiteMap = new List<SiteMapOrder>();
			if (SiteData.CurrentSite != null) {
				List<SiteMapOrder> lst = sitemapHelper.GetChildPages(SiteData.CurrentSite.SiteID, ParentID, ContPageID);

				lstSiteMap = (from l in lst
							  orderby l.NavOrder, l.NavMenuText
							  where l.Parent_ContentID != ContPageID || l.Parent_ContentID == null
							  select l).ToList();
			}

			return lstSiteMap;
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public List<SiteMapOrder> GetPageCrumbs(string PageID, string CurrPageID) {

			Guid? ContentPageID = Guid.Empty;
			if (!string.IsNullOrEmpty(PageID)) {
				if (PageID.Length > 20) {
					ContentPageID = new Guid(PageID);
				}
			}

			Guid ContPageID = Guid.Empty;
			if (!string.IsNullOrEmpty(CurrPageID)) {
				if (CurrPageID.Length > 20) {
					ContPageID = new Guid(CurrPageID);
				}
			}

			List<SiteMapOrder> lstSiteMap = new List<SiteMapOrder>();

			int iLevel = 0;

			int iLenB = 0;
			int iLenA = 1;

			while (iLenB < iLenA && SiteData.CurrentSite != null) {
				iLenB = lstSiteMap.Count;

				SiteMapOrder cont = sitemapHelper.GetPageWithLevel(SiteData.CurrentSite.SiteID, ContentPageID, iLevel);

				iLevel++;
				if (cont != null) {
					ContentPageID = cont.Parent_ContentID;
					lstSiteMap.Add(cont);
				}

				iLenA = lstSiteMap.Count;
			}

			return lstSiteMap.OrderByDescending(y => y.NavLevel).ToList();
		}



		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string UpdatePageTemplate(string TheTemplate, string ThisPage) {
			try {
				TheTemplate = CMSConfigHelper.DecodeBase64(TheTemplate);
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();

				ContentPage c = cmsAdminContent;

				c.TemplateFile = TheTemplate;

				cmsAdminContent = c;

				GetSetUserEditState("", "", "", "");

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string ValidateBlogFolders(string FolderPath, string CategoryPath, string TagPath) {
			try {
				string sFolderPath = ContentPageHelper.ScrubSlug(CMSConfigHelper.DecodeBase64(FolderPath));
				string sCategoryPath = ContentPageHelper.ScrubSlug(CMSConfigHelper.DecodeBase64(CategoryPath));
				string sTagPath = ContentPageHelper.ScrubSlug(CMSConfigHelper.DecodeBase64(TagPath));

				if (string.IsNullOrEmpty(sFolderPath) || string.IsNullOrEmpty(sCategoryPath) || string.IsNullOrEmpty(sTagPath)) {
					return "FAIL";
				}
				if (sFolderPath.Length < 1 || sCategoryPath.Length < 1 || sTagPath.Length < 1) {
					return "FAIL";
				}


				if (SiteData.CurrentSite != null
					&& !string.IsNullOrEmpty(sFolderPath)
					&& sCategoryPath.ToLower() != sTagPath.ToLower()) {

					var i1 = pageHelper.FindCountPagesBeginingWith(SiteData.CurrentSite.SiteID, sFolderPath);

					if (i1 < 1) {
						return "OK";
					}
				}

				if (SiteData.CurrentSite == null) {
					return "OK";
				}

				return "FAIL";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public List<MembershipUser> FindUsers(string searchTerm) {

			string search = CMSConfigHelper.DecodeBase64(searchTerm);

			List<MembershipUser> lstUsers = SecurityData.GetUserSearch(search);

			return lstUsers.OrderBy(x => x.UserName).ToList();
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string ValidateUniqueCategory(string TheSlug, string ItemID) {
			try {
				Guid CurrentItemGuid = new Guid(ItemID);
				TheSlug = CMSConfigHelper.DecodeBase64(TheSlug);
				TheSlug = ContentPageHelper.ScrubSlug(TheSlug);

				if (string.IsNullOrEmpty(TheSlug)) {
					return "FAIL";
				}
				if (TheSlug.Length < 1) {
					return "FAIL";
				}

				int iCount = ContentCategory.GetSimilar(SiteData.CurrentSite.SiteID, CurrentItemGuid, TheSlug);

				if (iCount < 1) {
					return "OK";
				}

				return "FAIL";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string ValidateUniqueTag(string TheSlug, string ItemID) {
			try {
				Guid CurrentItemGuid = new Guid(ItemID);
				TheSlug = CMSConfigHelper.DecodeBase64(TheSlug);
				TheSlug = ContentPageHelper.ScrubSlug(TheSlug);

				if (string.IsNullOrEmpty(TheSlug)) {
					return "FAIL";
				}
				if (TheSlug.Length < 1) {
					return "FAIL";
				}

				int iCount = ContentTag.GetSimilar(SiteData.CurrentSite.SiteID, CurrentItemGuid, TheSlug);

				if (iCount < 1) {
					return "OK";
				}

				return "FAIL";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string GenerateNewFilename(string ThePageTitle, string GoLiveDate, string PageID, string Mode) {
			try {
				CurrentPageGuid = new Guid(PageID);

				DateTime goLiveDate = Convert.ToDateTime(GoLiveDate);
				string sThePageTitle = CMSConfigHelper.DecodeBase64(ThePageTitle);
				if (string.IsNullOrEmpty(sThePageTitle)) {
					sThePageTitle = CurrentPageGuid.ToString();
				}
				sThePageTitle = sThePageTitle.Replace("/", "-");
				string sTheFileName = ContentPageHelper.ScrubFilename(CurrentPageGuid, sThePageTitle);

				if (Mode.ToLower() == "page") {
					string sTestRes = ValidateUniqueFilename(CMSConfigHelper.EncodeBase64(sTheFileName), PageID);
					if (sTestRes != "OK") {
						for (int i = 1; i < 1000; i++) {
							string sTestFile = sThePageTitle + "-" + i.ToString();
							sTestRes = ValidateUniqueFilename(CMSConfigHelper.EncodeBase64(sTestFile), PageID);
							if (sTestRes == "OK") {
								sTheFileName = ContentPageHelper.ScrubFilename(CurrentPageGuid, sTestFile);
								break;
							} else {
								sTheFileName = "";
							}
						}
					}
				} else {
					string sTestRes = ValidateUniqueBlogFilename(CMSConfigHelper.EncodeBase64(sTheFileName), GoLiveDate, PageID);
					if (sTestRes != "OK") {
						for (int i = 1; i < 1000; i++) {
							string sTestFile = sThePageTitle + "-" + i.ToString();
							sTestRes = ValidateUniqueBlogFilename(CMSConfigHelper.EncodeBase64(sTestFile), GoLiveDate, PageID);
							if (sTestRes == "OK") {
								sTheFileName = ContentPageHelper.ScrubFilename(CurrentPageGuid, sTestFile);
								break;
							} else {
								sTheFileName = "";
							}
						}
					}
				}

				return ContentPageHelper.ScrubFilename(CurrentPageGuid, sTheFileName).ToLower();
			} catch (Exception ex) {

				SiteData.WriteDebugException("webservice", ex);
				return "FAIL";
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string ValidateUniqueFilename(string TheFileName, string PageID) {
			try {
				CurrentPageGuid = new Guid(PageID);
				TheFileName = CMSConfigHelper.DecodeBase64(TheFileName);
				TheFileName = ContentPageHelper.ScrubFilename(CurrentPageGuid, TheFileName);

				TheFileName = TheFileName.ToLower();

				if (SiteData.IsPageSpecial(TheFileName) || TheFileName.Length < 6) {
					return "FAIL";
				}

				if (TheFileName.StartsWith(SiteData.CurrentSite.BlogFolderPath.ToLower())
					|| TheFileName.StartsWith(SiteData.CurrentSite.BlogCategoryPath.ToLower())
					|| TheFileName.StartsWith(SiteData.CurrentSite.BlogTagPath.ToLower())) {

					return "FAIL";
				}

				ContentPage fn = pageHelper.FindByFilename(SiteData.CurrentSite.SiteID, TheFileName);

				ContentPage cp = pageHelper.FindContentByID(SiteData.CurrentSite.SiteID, CurrentPageGuid);

				if (cp == null && CurrentPageGuid != Guid.Empty) {
					cp = pageHelper.GetVersion(SiteData.CurrentSite.SiteID, CurrentPageGuid);
				}

				if (fn == null || (fn != null && cp != null && fn.Root_ContentID == cp.Root_ContentID)) {
					return "OK";
				} else {
					return "FAIL";
				}
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string GenerateBlogFilePrefix(string ThePageSlug, string GoLiveDate) {
			try {
				DateTime goLiveDate = Convert.ToDateTime(GoLiveDate);
				ThePageSlug = CMSConfigHelper.DecodeBase64(ThePageSlug);


				return ContentPageHelper.CreateFileNameFromSlug(SiteData.CurrentSite.SiteID, goLiveDate, ThePageSlug);

			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return "FAIL";
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string ValidateUniqueBlogFilename(string ThePageSlug, string GoLiveDate, string PageID) {
			try {
				CurrentPageGuid = new Guid(PageID);
				DateTime goLiveDate = Convert.ToDateTime(GoLiveDate);
				ThePageSlug = CMSConfigHelper.DecodeBase64(ThePageSlug);
				ThePageSlug = ContentPageHelper.ScrubFilename(CurrentPageGuid, ThePageSlug);

				ThePageSlug = ThePageSlug.ToLower();

				string TheFileName = ThePageSlug;

				ContentPage cp = pageHelper.FindContentByID(SiteData.CurrentSite.SiteID, CurrentPageGuid);

				if (cp != null) {
					goLiveDate = cp.GoLiveDate;
				}
				if (cp == null && CurrentPageGuid != Guid.Empty) {
					ContentPageExport cpe = ContentImportExportUtils.GetSerializedContentPageExport(CurrentPageGuid);
					if (cpe != null) {
						goLiveDate = cpe.ThePage.GoLiveDate;
					}
				}

				TheFileName = ContentPageHelper.CreateFileNameFromSlug(SiteData.CurrentSite.SiteID, goLiveDate, ThePageSlug);

				if (SiteData.IsPageSpecial(TheFileName) || TheFileName.Length < 6) {
					return "FAIL";
				}

				ContentPage fn1 = pageHelper.FindByFilename(SiteData.CurrentSite.SiteID, TheFileName);

				if (cp == null && CurrentPageGuid != Guid.Empty) {
					cp = pageHelper.GetVersion(SiteData.CurrentSite.SiteID, CurrentPageGuid);
				}

				if (fn1 == null || (fn1 != null && cp != null && fn1.Root_ContentID == cp.Root_ContentID)) {
					return "OK";
				} else {
					return "FAIL";
				}
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string MoveWidgetToNewZone(string WidgetTarget, string WidgetDropped, string ThisPage) {
			try {
				//WidgetAddition = CMSConfigHelper.DecodeBase64(WidgetAddition);
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				string[] w = WidgetDropped.Split('\t');

				Guid guidWidget = Guid.Empty;
				if (w.Length > 2) {
					if (w[2].ToString().Length == Guid.Empty.ToString().Length) {
						guidWidget = new Guid(w[2]);
					}
				} else {
					if (w[0].ToString().Length == Guid.Empty.ToString().Length) {
						guidWidget = new Guid(w[0]);
					}
				}

				List<Widget> cacheWidget = cmsAdminWidget;

				Widget ww1 = (from w1 in cacheWidget
							  where w1.Root_WidgetID == guidWidget
							  select w1).FirstOrDefault();

				if (ww1 != null) {
					ww1.WidgetOrder = -1;
					ww1.PlaceholderName = WidgetTarget;
				}

				List<Widget> ww2 = (from w1 in cacheWidget
									where w1.PlaceholderName.ToLower() == WidgetTarget.ToLower()
									&& w1.WidgetOrder >= 0
									orderby w1.WidgetOrder
									select w1).ToList();

				int iW = 1;
				foreach (var w2 in ww2) {
					w2.WidgetOrder = iW++;
				}

				cmsAdminWidget = cacheWidget;
				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string CacheWidgetUpdate(string WidgetAddition, string ThisPage) {
			try {
				WidgetAddition = CMSConfigHelper.DecodeBase64(WidgetAddition);
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();

				List<Widget> cacheWidget = cmsAdminWidget;

				List<Widget> inputWid = new List<Widget>();
				Dictionary<Guid, int> dictOrder = new Dictionary<Guid, int>();
				int iW = 0;

				WidgetAddition = WidgetAddition.Replace("\r\n", "\n");
				WidgetAddition = WidgetAddition.Replace("\r", "\n");
				var arrWidgRows = WidgetAddition.Split('\n');

				foreach (string arrWidgCell in arrWidgRows) {
					if (!string.IsNullOrEmpty(arrWidgCell)) {
						bool bGoodWidget = false;
						var w = arrWidgCell.Split('\t');
						var rWidg = new Widget();
						if (w[2].ToLower().EndsWith(".ascx") || w[2].ToLower().StartsWith("class:")) {
							rWidg.ControlPath = w[2];
							rWidg.Root_WidgetID = Guid.NewGuid();
							bGoodWidget = true;
						} else {
							if (w[2].ToString().Length == Guid.Empty.ToString().Length) {
								rWidg.Root_WidgetID = new Guid(w[2]);
								bGoodWidget = true;
							}
						}
						if (bGoodWidget) {
							dictOrder.Add(rWidg.Root_WidgetID, iW);

							rWidg.WidgetDataID = Guid.NewGuid();
							rWidg.PlaceholderName = w[1].Substring(4);
							rWidg.WidgetOrder = int.Parse(w[0]);
							rWidg.Root_ContentID = CurrentPageGuid;
							rWidg.IsWidgetActive = true;
							rWidg.IsLatestVersion = true;
							rWidg.EditDate = SiteData.CurrentSite.Now;
							inputWid.Add(rWidg);
						}
						iW++;
					}
				}

				foreach (var wd in inputWid) {
					var z = (from d in cacheWidget where d.Root_WidgetID == wd.Root_WidgetID select d).FirstOrDefault();
					if (z == null) {
						cacheWidget.Add(wd);
					} else {
						z.EditDate = SiteData.CurrentSite.Now;
						z.PlaceholderName = wd.PlaceholderName; // if moving zones

						var i = cacheWidget.IndexOf(z);
						cacheWidget[i].WidgetOrder = wd.WidgetOrder;

						int? mainSort = (from entry in dictOrder
										 where entry.Key == wd.Root_WidgetID
										 select entry.Value).FirstOrDefault();
						if (mainSort != null) {
							cacheWidget[i].WidgetOrder = Convert.ToInt32(mainSort);
						}
					}
				}

				cmsAdminWidget = cacheWidget;
				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string GetWidgetText(string DBKey, string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				Guid guidWidget = new Guid(DBKey);

				var ww = (from w in cmsAdminWidget
						  where w.Root_WidgetID == guidWidget
						  select w).FirstOrDefault();

				if (ww != null) {
					if (string.IsNullOrEmpty(ww.ControlProperties)) {
						return "No Data";
					} else {
						if (ww.ControlProperties.Length < 768) {
							return ww.ControlProperties;
						} else {
							return ww.ControlProperties.Substring(0, 700) + "[.....]";
						}
					}
				}

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return "FAIL";
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string GetWidgetVersionText(string DBKey, string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				Guid guidWidget = new Guid(DBKey);

				var ww = (from w in cmsAdminWidget
						  where w.WidgetDataID == guidWidget
						  select w).FirstOrDefault();

				if (ww == null) {
					ww = widgetHelper.GetWidgetVersion(guidWidget);
				}

				if (ww != null) {
					if (string.IsNullOrEmpty(ww.ControlProperties)) {
						return "No Data";
					} else {
						if (ww.ControlProperties.Length < 768) {
							return ww.ControlProperties;
						} else {
							return ww.ControlProperties.Substring(0, 700) + "[.....]";
						}
					}
				}

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return "FAIL";
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string DeleteWidget(string DBKey, string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				Guid guidWidget = new Guid(DBKey);

				var cacheWidget = cmsAdminWidget;

				var ww = (from w in cacheWidget
						  where w.Root_WidgetID == guidWidget
						  select w).ToList();

				if (ww != null) {
					foreach (var w in ww) {
						w.IsWidgetPendingDelete = true;
						w.IsWidgetActive = false;
						w.EditDate = SiteData.CurrentSite.Now;
					}
				}

				cmsAdminWidget = cacheWidget;

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string RemoveWidget(string DBKey, string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				Guid guidWidget = new Guid(DBKey);

				var cacheWidget = cmsAdminWidget;

				var ww = (from w in cacheWidget
						  where w.Root_WidgetID == guidWidget
						  select w).ToList();

				if (ww != null) {
					foreach (var w in ww) {
						w.IsWidgetActive = false;
						w.EditDate = SiteData.CurrentSite.Now;
					}
				}

				cmsAdminWidget = cacheWidget;

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string CacheGenericContent(string ZoneText, string DBKey, string ThisPage) {
			try {
				ZoneText = CMSConfigHelper.DecodeBase64(ZoneText);
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				Guid guidWidget = new Guid(DBKey);

				var cacheWidget = cmsAdminWidget;

				var c = (from w in cacheWidget
						 where w.Root_WidgetID == guidWidget
						 select w).FirstOrDefault();

				c.ControlProperties = ZoneText;
				c.EditDate = SiteData.CurrentSite.Now;

				cmsAdminWidget = cacheWidget;

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}




		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string CacheContentZoneText(string ZoneText, string Zone, string ThisPage) {
			try {
				ZoneText = CMSConfigHelper.DecodeBase64(ZoneText);
				Zone = CMSConfigHelper.DecodeBase64(Zone);
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				CurrentEditPage = filePage.FileName.ToLower();

				var c = cmsAdminContent;
				c.EditDate = SiteData.CurrentSite.Now;
				c.EditUserId = SecurityData.CurrentUserGuid;
				c.ContentID = Guid.NewGuid();

				if (Zone.ToLower() == "c")
					c.PageText = ZoneText;

				if (Zone.ToLower() == "l")
					c.LeftPageText = ZoneText;

				if (Zone.ToLower() == "r")
					c.RightPageText = ZoneText;

				cmsAdminContent = c;

				return "OK";
			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}



		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string PublishChanges(string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);
				LoadGuids();
				CurrentEditPage = filePage.FileName.ToLower();

				bool bLock = pageHelper.IsPageLocked(CurrentPageGuid, SiteData.CurrentSite.SiteID, SecurityData.CurrentUserGuid);
				Guid guidUser = pageHelper.GetCurrentEditUser(CurrentPageGuid, SiteData.CurrentSite.SiteID);

				if (bLock || guidUser != SecurityData.CurrentUserGuid) {
					return "Cannot publish changes, not current editing user.";
				}

				List<Widget> pageWidgets = widgetHelper.GetWidgets(CurrentPageGuid, true);

				if (cmsAdminContent != null) {
					ContentPage oldContent = pageHelper.FindContentByID(SiteData.CurrentSiteID, CurrentPageGuid);

					ContentPage newContent = pageHelper.CopyContentPageToNew(cmsAdminContent);
					newContent.ContentID = Guid.NewGuid();
					newContent.NavOrder = oldContent.NavOrder;
					newContent.Parent_ContentID = oldContent.Parent_ContentID;

					foreach (var wd in cmsAdminWidget) {
						wd.Save();
					}

					newContent.SavePageEdit();

					if (newContent.ContentType == ContentPageType.PageType.BlogEntry) {
						pageHelper.ResolveDuplicateBlogURLs(newContent.SiteID);
					}

					cmsAdminWidget = new List<Widget>();
					cmsAdminContent = null;
				}

				GetSetUserEditState("", "", "", "");

				return "OK";

			} catch (Exception ex) {
				SiteData.WriteDebugException("webservice", ex);

				return ex.ToString();
			}
		}




	}
}
