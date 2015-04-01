using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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
	public partial class CalendarAdminList : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {

			if (SiteID == Guid.Empty) {
				SiteID = SiteData.CurrentSiteID;
			}

			if (!IsPostBack) {
				txtDate.Text = SiteData.CurrentSite.Now.Date.ToShortDateString();

				Calendar1.CalendarDate = SiteData.CurrentSite.Now.Date;

				SetCalendar();
			}
		}

		public DateTime GetTimeFromTimeSpan(TimeSpan? timeSpan) {
			return CalendarHelper.GetFullDateTime(timeSpan);
		}

		protected void txtDate_TextChanged(object sender, EventArgs e) {
			Calendar1.CalendarDate = Convert.ToDateTime(txtDate.Text).Date;
			SetCalendar();
		}

		protected void btnLast_Click(object sender, EventArgs e) {
			Calendar1.CalendarDate = Calendar1.CalendarDate.AddMonths(-1);
			SetCalendar();
		}

		protected void btnNext_Click(object sender, EventArgs e) {
			Calendar1.CalendarDate = Calendar1.CalendarDate.AddMonths(1);
			SetCalendar();
		}


		protected void SetCalendar() {
			SiteData site = SiteData.CurrentSite;

			DateTime dtStart = Calendar1.CalendarDate.AddDays(1 - Calendar1.CalendarDate.Day).Date;
			DateTime dtEnd = dtStart.AddMonths(1).Date;

			dtStart = site.ConvertSiteTimeToUTC(dtStart);
			dtEnd = site.ConvertSiteTimeToUTC(dtEnd);

			using (CalendarDataContext db = CalendarDataContext.GetDataContext() ) {

				List<vw_carrot_CalendarEvent> lst = (from c in db.vw_carrot_CalendarEvents
													 where c.EventDate >= dtStart
													  && c.EventDate < dtEnd
													  && c.SiteID == SiteID
													 orderby c.EventDate, c.EventStartTime
													 select c).ToList();

				lst.ForEach(x => x.EventDate = site.ConvertUTCToSiteTime(x.EventDate));

				List<DateTime> dates = (from dd in lst select dd.EventDate.Date).Distinct().ToList();

				Calendar1.HilightDateList = dates;

				CalendarHelper.BindDataBoundControl(dgEvents, lst);
			}
		}


	}
}