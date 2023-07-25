using Carrotware.CMS.Interface;
using System;

/*
* CarrotCake CMS - Event Calendar
* http://www.carrotware.com/
*
* Copyright 2013, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: June 2013
*/

namespace Carrotware.CMS.UI.Plugins.EventCalendarModule {

	public partial class CalendarAdminProfileList : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {
			LoadData();
		}

		protected void LoadData() {
			if (!IsPostBack) {
				CalendarHelper.SeedCalendarCategories(this.SiteID);

				var lst2 = CalendarHelper.GetYears(SiteID);
				CalendarHelper.BindDataBoundControl(ddlFilter, lst2);
				ddlFilter.SelectedValue = "-2";
			}

			var lst = CalendarHelper.GetProfileView(SiteID, int.Parse(ddlFilter.SelectedValue));
			CalendarHelper.BindDataBoundControl(dgEvents, lst);
		}

		protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e) {
			LoadData();
		}
	}
}