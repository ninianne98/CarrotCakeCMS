using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
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

	[ToolboxData("<{0}:ChildNavigation runat=server></{0}:ChildNavigation>")]
	public class ChildNavigation : BaseNavHeaded {

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
				if (!String.IsNullOrEmpty(s)) {
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

		public bool IncludeParent { get; set; }

		protected override void LoadData() {
			base.LoadData();

			List<SiteNav> lstNav = navHelper.GetChildNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);

			if (this.IncludeParent && lstNav != null && lstNav.Where(x => x.ShowInSiteNav == true).Count() > 0) {
				SiteNav p = GetParent(lstNav.OrderByDescending(x => x.Parent_ContentID).FirstOrDefault().Parent_ContentID);
				if (p != null) {
					p.NavOrder = -100;
					lstNav.Add(p);
				}
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

		protected SiteNav GetParent(Guid? rootContentID) {
			SiteNav pageNav = null;
			if (rootContentID.HasValue) {
				pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, rootContentID.Value);
			}
			return pageNav;
		}
	}
}