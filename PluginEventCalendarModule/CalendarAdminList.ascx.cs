using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.Linq;

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

	public partial class CalendarAdminList : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {
			if (SiteID == Guid.Empty) {
				SiteID = SiteData.CurrentSiteID;
			}

			if (!IsPostBack) {
				CalendarHelper.SeedCalendarCategories(this.SiteID);

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

			var events = CalendarHelper.GetDisplayEvents(this.SiteID, dtStart, dtEnd, -1, false).ToList();

			events.ForEach(x => x.EventDate = site.ConvertUTCToSiteTime(x.EventDate));

			Calendar1.HilightDateList = (from dd in events select dd.EventDate.Date).Distinct().ToList();

			CalendarHelper.BindDataBoundControl(dgEvents, events);
		}
	}
}