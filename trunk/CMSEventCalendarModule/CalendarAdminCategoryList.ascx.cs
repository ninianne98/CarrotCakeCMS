using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS - Event Calendar
* http://www.carrotware.com/
*
* Copyright 2013, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: June 2013
*/


namespace Carrotware.CMS.UI.Plugins.EventCalendarModule {
	public partial class CalendarAdminCategoryList : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {

			LoadData();
		}

		protected void LoadData() {
			var lst = CalendarHelper.GetCalendarCategories(SiteID);
			CalendarHelper.BindDataBoundControl(dgMenu, lst);
		}

	}
}