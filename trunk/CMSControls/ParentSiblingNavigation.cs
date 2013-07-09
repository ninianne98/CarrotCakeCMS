using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:ParentSiblingNavigation runat=server></{0}:ParentSiblingNavigation>")]
	public class ParentSiblingNavigation : BaseNavSelHeaded {

		public enum SortOrder {
			SortAsc,
			SortDesc,
			DateAsc,
			DateDesc,
			TitleAsc,
			TitleDesc,
		}

		[Category("Appearance")]
		[DefaultValue("SortAsc")]
		public SortOrder SortNavBy {
			get {
				string s = (string)ViewState["SortNavBy"];
				SortOrder c = SortOrder.SortAsc;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (SortOrder)Enum.Parse(typeof(SortOrder), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["SortNavBy"] = value.ToString();
			}
		}

		protected override void LoadData() {
			base.LoadData();
			List<SiteNav> lstNav = null;
			SiteNav parentNav = GetParentPage();

			if (parentNav.Root_ContentID != Guid.Empty && parentNav.Parent_ContentID != null) {
				lstNav = navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, parentNav.FileName, !SecurityData.IsAuthEditor);
			} else {
				lstNav = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);
			}

			switch (this.SortNavBy) {
				case SortOrder.TitleAsc:
					this.NavigationData = lstNav.OrderBy(ct => ct.NavMenuText).ToList();
					break;
				case SortOrder.TitleDesc:
					this.NavigationData = lstNav.OrderByDescending(ct => ct.NavMenuText).ToList();
					break;
				case SortOrder.DateAsc:
					this.NavigationData = lstNav.OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.GoLiveDate).ToList();
					break;
				case SortOrder.DateDesc:
					this.NavigationData = lstNav.OrderBy(ct => ct.NavMenuText).OrderByDescending(ct => ct.GoLiveDate).ToList();
					break;
				case SortOrder.SortDesc:
					this.NavigationData = lstNav.OrderBy(ct => ct.NavMenuText).OrderByDescending(ct => ct.NavOrder).ToList();
					break;
				case SortOrder.SortAsc:
				default:
					this.NavigationData = lstNav.OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
					break;
			}
		}

	}
}
