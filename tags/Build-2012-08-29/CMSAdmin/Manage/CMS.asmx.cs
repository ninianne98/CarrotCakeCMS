using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Xml.Serialization;
using Carrotware.CMS.Core;
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

namespace Carrotware.CMS.UI.Admin {
	/// <summary>
	/// Summary description for CMS
	/// </summary>
	[WebService(Namespace = "http://carrotware.com/cms/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[System.Web.Script.Services.ScriptService]

	public class CMS : System.Web.Services.WebService {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		private ContentPageHelper pageHelper = new ContentPageHelper();
		private WidgetHelper widgetHelper = new WidgetHelper();

		private Guid CurrentPageGuid = Guid.Empty;
		private ContentPage filePage = null;

		private List<ContentPage> _pages = null;
		protected List<ContentPage> lstActivePages {
			get {
				if (_pages == null) {
					_pages = pageHelper.GetLatestContentList(SiteData.CurrentSiteID, true);
				}
				return _pages;
			}
		}

		public ContentPage cmsAdminContent {
			get {
				ContentPage c = null;
				try {
					var sXML = GetSerialized(CMSConfigHelper.keyAdminContent);
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
				var sXML = GetSerialized(CMSConfigHelper.keyAdminWidget);
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
			if (!string.IsNullOrEmpty(CurrentEditPage)) {
				ContentPageHelper pageHelper = new ContentPageHelper();
				filePage = pageHelper.GetLatestContent(SiteData.CurrentSiteID, null, CurrentEditPage.ToString().ToLower());
				CurrentPageGuid = filePage.Root_ContentID;
			} else {
				if (CurrentPageGuid != Guid.Empty) {
					ContentPageHelper pageHelper = new ContentPageHelper();
					filePage = pageHelper.GetLatestContent(SiteData.CurrentSiteID, CurrentPageGuid);
					CurrentEditPage = filePage.FileName;
				} else {
					filePage = new ContentPage();
				}
			}

		}


		private string CurrentEditPage = "";


		private string ParseEdit(string sContent) {
			string sFixed = "";
			if (!string.IsNullOrEmpty(sContent)) {
				sFixed = sContent;
				var b = "<!-- <#|BEGIN_CARROT_CMS|#> -->";
				var iB = sFixed.IndexOf(b);
				if (iB > 0) {
					sFixed = sFixed.Substring(iB + b.Length);

					var e = "<!-- <#|END_CARROT_CMS|#> -->";
					var iE = sFixed.IndexOf(e);
					sFixed = sFixed.Substring(0, iE);
				}
			}
			return sFixed.Trim();
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string RecordHeartbeat(string PageID) {
			try {
				CurrentPageGuid = new Guid(PageID);

				var bLock = pageHelper.IsPageLocked(CurrentPageGuid, SiteData.CurrentSiteID, SecurityData.CurrentUserGuid);

				//only allow admin/editors to record a lock
				if ((SecurityData.IsAdmin || SecurityData.IsEditor) && !bLock) {
					var bRet = pageHelper.RecordHeartbeatLock(CurrentPageGuid, SiteData.CurrentSiteID, SecurityData.CurrentUserGuid);

					if (bRet) {
						return DateTime.Now.ToString();
					} else {
						return Convert.ToDateTime("12/31/1899").ToString();
					}
				} else {
					return DateTime.MinValue.ToString();
				}

			} catch (Exception ex) {
				return DateTime.MinValue.ToString();
			}
		}


		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public string CancelEditing(string ThisPage) {
			try {
				CurrentPageGuid = new Guid(ThisPage);

				pageHelper.ResetHeartbeatLock(CurrentPageGuid, SiteData.CurrentSiteID);

				GetSetUserEditState("", "", "", "");

				return "OK";
			} catch (Exception ex) {
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

			var lst = (from ct in db.carrot_Contents
					   join r in db.carrot_RootContents on ct.Root_ContentID equals r.Root_ContentID
					   orderby ct.NavOrder, ct.NavMenuText
					   where r.SiteID == SiteData.CurrentSiteID
							 && r.Root_ContentID != ContPageID
							 && ct.IsLatestVersion == true
							 && (ct.Parent_ContentID == ParentID
								 || (ct.Parent_ContentID == null && ParentID == Guid.Empty))
					   select new SiteMapOrder {
						   NavLevel = -1,
						   NavMenuText = (r.PageActive ? "" : "{*U*} ") + ct.NavMenuText,
						   NavOrder = ct.NavOrder,
						   PageActive = Convert.ToBoolean(r.PageActive),
						   Parent_ContentID = ct.Parent_ContentID,
						   Root_ContentID = ct.Root_ContentID
					   }).ToList();

			lstSiteMap = (from l in lst
						  orderby l.NavOrder, l.NavMenuText
						  where l.Parent_ContentID != ContPageID || l.Parent_ContentID == null
						  select l).ToList();

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

			while (iLenB < iLenA) {
				iLenB = lstSiteMap.Count;

				var cont = (from r in db.carrot_RootContents
							join ct in db.carrot_Contents on r.Root_ContentID equals ct.Root_ContentID
							orderby ct.NavOrder, ct.NavMenuText
							where r.SiteID == SiteData.CurrentSiteID
								&& ct.IsLatestVersion == true
								&& ct.Root_ContentID == ContentPageID

							select new SiteMapOrder {
								NavLevel = iLevel,
								NavMenuText = (r.PageActive ? "" : "{*U*} ") + ct.NavMenuText,
								NavOrder = ct.NavOrder,
								PageActive = Convert.ToBoolean(r.PageActive),
								Parent_ContentID = ct.Parent_ContentID,
								Root_ContentID = ct.Root_ContentID
							}).FirstOrDefault();

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

				var c = cmsAdminContent;

				c.TemplateFile = TheTemplate;

				cmsAdminContent = c;

				GetSetUserEditState("", "", "", "");

				return "OK";
			} catch (Exception ex) {
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
		public string ValidateUniqueFilename(string TheFileName, string PageID) {
			try {
				CurrentPageGuid = new Guid(PageID);
				TheFileName = CMSConfigHelper.DecodeBase64(TheFileName);
				TheFileName = ContentPageHelper.ScrubFilename(CurrentPageGuid, TheFileName);

				TheFileName = TheFileName.ToLower();

				if (TheFileName == "/default.aspx") {
					return "FAIL";
				}

				var fn = pageHelper.FindByFilename(SiteData.CurrentSiteID, TheFileName);

				var cp = pageHelper.GetLatestContent(SiteData.CurrentSiteID, CurrentPageGuid);

				if (cp == null) {
					cp = pageHelper.GetVersion(SiteData.CurrentSiteID, CurrentPageGuid);
				}

				if (fn == null || (fn != null && cp != null && fn.Root_ContentID == cp.Root_ContentID)) {
					return "PASS";
				} else {
					return "FAIL";
				}
			} catch (Exception ex) {
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
				var w = WidgetDropped.Split('\t');

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

				var cacheWidget = cmsAdminWidget;

				var ww1 = (from w1 in cacheWidget
						   where w1.Root_WidgetID == guidWidget
						   select w1).FirstOrDefault();

				if (ww1 != null) {
					ww1.WidgetOrder = -1;
					ww1.PlaceholderName = WidgetTarget;
				}

				var ww2 = (from w1 in cacheWidget
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

				var cacheWidget = cmsAdminWidget;

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
							rWidg.Root_ContentID = cmsAdminContent.Root_ContentID;
							rWidg.IsWidgetActive = true;
							rWidg.IsLatestVersion = true;
							rWidg.EditDate = DateTime.Now;
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
						z.EditDate = DateTime.Now;
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
							return ww.ControlProperties.Substring(0, 700) + ".........";
						}
					}
				}

				return "OK";
			} catch (Exception ex) {
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
							return ww.ControlProperties.Substring(0, 700) + ".........";
						}
					}
				}

				return "OK";
			} catch (Exception ex) {
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
						w.EditDate = DateTime.Now;
					}
				}

				cmsAdminWidget = cacheWidget;

				return "OK";
			} catch (Exception ex) {
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
						w.EditDate = DateTime.Now;
					}
				}

				cmsAdminWidget = cacheWidget;

				return "OK";
			} catch (Exception ex) {
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
				c.EditDate = DateTime.Now;

				cmsAdminWidget = cacheWidget;

				return "OK";
			} catch (Exception ex) {
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
				c.EditDate = DateTime.Now;
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

				bool bLock = pageHelper.IsPageLocked(CurrentPageGuid, SiteData.CurrentSiteID, SecurityData.CurrentUserGuid);
				Guid guidUser = pageHelper.GetCurrentEditUser(CurrentPageGuid, SiteData.CurrentSiteID);

				if (bLock || guidUser != SecurityData.CurrentUserGuid) {
					return "Cannot publish changes, not current editing user.";
				}

				List<Widget> pageWidgets = widgetHelper.GetWidgets(CurrentPageGuid, true);

				if (cmsAdminContent != null) {

					ContentPage newContent = pageHelper.CopyContentPageToNew(cmsAdminContent);

					foreach (var wd in cmsAdminWidget) {
						wd.Save();
					}

					newContent.SavePageEdit();

					cmsAdminWidget = new List<Widget>();
					cmsAdminContent = null;
				}

				GetSetUserEditState("", "", "", "");

				return "OK";

			} catch (Exception ex) {
				return ex.ToString();
			}
		}




	}
}
