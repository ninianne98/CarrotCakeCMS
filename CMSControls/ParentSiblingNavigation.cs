using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using Carrotware.CMS.Interface;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:ParentSiblingNavigation runat=server></{0}:ParentSiblingNavigation>")]
	public class ParentSiblingNavigation : BaseNavSelHeaded {

		public ParentSiblingNavigation()
			: base() {
			this.SortNavBy = SortOrder.DateAsc;
		}

		public enum SortOrder {

			[Description("Link Order Ascending")]
			SortAsc,

			[Description("Link Order Descending")]
			SortDesc,

			[Description("Go Live Date Ascending")]
			DateAsc,

			[Description("Go Live Date Descending")]
			DateDesc,

			[Description("Sort Link Text Ascending")]
			TitleAsc,

			[Description("Sort Link Text Descending")]
			TitleDesc,
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstSortOrder {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict = EnumHelper.ToList<SortOrder>().ToDictionary(k => k.Text, v => v.Description);

				return _dict;
			}
		}

		[Category("Appearance")]
		[DefaultValue("SortAsc")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSortOrder")]
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

		public override List<string> LimitedPropertyList {
			get {
				List<string> lst = base.LimitedPropertyList;
				lst.Add("SortNavBy");

				return lst.Distinct().ToList();
			}
		}

		protected override void OnPreRender(System.EventArgs e) {
			if (this.PublicParmValues.Any()) {
				string sTmp = "";
				try {
					sTmp = GetParmValue("SortNavBy", "");
					if (!String.IsNullOrEmpty(sTmp)) {
						this.SortNavBy = (SortOrder)Enum.Parse(typeof(SortOrder), sTmp, true);
					}
				} catch (Exception ex) {
				}
			}

			base.OnPreRender(e);
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