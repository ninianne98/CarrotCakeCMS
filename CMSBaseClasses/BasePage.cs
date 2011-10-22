using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {
	public class BasePage : System.Web.UI.Page {

		protected GridSorting gs = new GridSorting();

		protected ContentPage pageHelper = new ContentPage();
		protected SiteData siteHelper = new SiteData();
		protected PageWidget widgetHelper = new PageWidget();
		protected CMSConfigHelper cmsHelper = new CMSConfigHelper();


		protected Guid CurrentUserGuid = Guid.Empty;

		protected MembershipUser CurrentUser { get; set; }



		protected string CurrentDLLVersion {
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		private List<ContentPage> _pages = null;
		protected List<ContentPage> lstActivePages {
			get {
				if (_pages == null) {
					_pages = pageHelper.GetLatestContentList(SiteID, true);
				}
				return _pages;
			}
		}

		protected bool AdvancedEditMode {
			get {
				bool _Advanced = false;
				if (Page.User.Identity.IsAuthenticated) {
					if (Request.QueryString["carrotedit"] != null && (IsAdmin || IsEditor)) {
						_Advanced = true;
					} else {
						_Advanced = false;
					}
				}
				return _Advanced;
			}
		}


		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}



		public string CurrentScriptName {
			get { return Request.ServerVariables["script_name"].ToString(); }
		}

		public string ReferringPage {
			get {
				var r = CurrentScriptName;
				try { r = Request.ServerVariables["http_referer"].ToString(); } catch { }
				if (string.IsNullOrEmpty(r))
					r = "./default.aspx";
				return r;
			}
		}


		public bool IsPageRefreshJavaScript {
			get {
				var arg = Request["__EVENTARGUMENT"].ToString();
				var tgt = Request["__EVENTTARGET"].ToString();
				if (!string.IsNullOrEmpty(arg) && !string.IsNullOrEmpty(tgt)) {
					if (tgt.ToLower() == "pagerefresh" && arg.ToLower() == "javascript") {
						return true;
					}
				}
				return false;
			}
		}

		public bool IsAdmin {
			get { return Roles.IsUserInRole("CarrotCMS Administrators"); }
		}
		public bool IsEditor {
			get { return Roles.IsUserInRole("CarrotCMS Editors"); }
		}
		public bool IsUsers {
			get { return Roles.IsUserInRole("CarrotCMS Users"); }
		}

		//protected override void OnLoad(EventArgs e) {
		protected override void OnLoad(EventArgs e) {

			LoadGuids();
			//SetPageButtons(this);
			base.OnLoad(e);
		}


		protected void LoadGuids() {
			if (Page.User.Identity.IsAuthenticated) {
				if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {
					CurrentUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
					CurrentUserGuid = new Guid(CurrentUser.ProviderUserKey.ToString());
				}
			} else {
				CurrentUser = null;
				CurrentUserGuid = Guid.Empty;
			}
		}

		public List<MembershipUser> GetUserList() {
			List<MembershipUser> usrs = new List<MembershipUser>();
			foreach (MembershipUser usr in Membership.GetAllUsers()) {
				usrs.Add(usr);
			}
			return usrs;
		}


		public void LoadGrid<T>(GridView TheGrid, HiddenField SortValue, List<T> lst, string sSortKey) {
			List<T> lstVals = null;
			string VSKey = TheGrid.ClientID + "_Data";
			gs.DefaultSort = SortValue.Value;
			if (lst != null) {
				lstVals = lst;
				gs.Sort = SortValue.Value;
			} else {
				SortValue.Value = sSortKey;
				gs.Sort = sSortKey;
				lstVals = (List<T>)ViewState[VSKey];
				lstVals = gs.SortDataList<T>(lstVals);
			}
			ViewState[VSKey] = lstVals;
			TheGrid.DataSource = lstVals;
			TheGrid.DataBind();
		}


		public void LoadGridLive<T>(GridView TheGrid, HiddenField SortValue, List<T> lstVals, string sSortKey) {
			gs.DefaultSort = SortValue.Value;

			SortValue.Value = sSortKey;
			gs.Sort = sSortKey;
			lstVals = gs.SortDataList<T>(lstVals);

			TheGrid.DataSource = lstVals;
			TheGrid.DataBind();
		}




		bool bFound = false;
		WidgetContainer x = new WidgetContainer();
		protected WidgetContainer FindTheControl(string ControlName, Control X) {

			if (X is Page) {
				bFound = false;
				x = new WidgetContainer();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is WidgetContainer) {
					bFound = true;
					x = (WidgetContainer)c;
					return x;
				} else {
					if (!bFound) {
						FindTheControl(ControlName, c);
					}
				}
			}
			return x;
		}





	}
}