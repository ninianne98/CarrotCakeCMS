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
	public abstract class BaseUserControl : System.Web.UI.UserControl {

		protected GridSorting gs = new GridSorting();
		protected ContentPage pageHelper = new ContentPage();
		protected PageWidget widgetHelper = new PageWidget();
		protected CMSConfigHelper cmsHelper = new CMSConfigHelper();


		protected Guid SiteID {
			get {
				return SiteData.CurrentSiteID;
			}
		}

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
		PlaceHolder x = new PlaceHolder();
		protected PlaceHolder FindTheControl(string ControlName, Control X) {

			if (X is Page) {
				bFound = false;
				x = new PlaceHolder();
			}

			foreach (Control c in X.Controls) {
				if (c.ID == ControlName && c is PlaceHolder) {
					bFound = true;
					x = (PlaceHolder)c;
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