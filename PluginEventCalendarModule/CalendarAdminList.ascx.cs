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
			var theDate = Convert.ToDateTime(txtDate.Text).Date;
			var first = CalendarHelper.GetFirstOfMonthByDate(theDate);
			Calendar1.CalendarDate = first.Date;
			txtDate.Text = first.Date.ToShortDateString();
			SetCalendar();
		}

		protected void btnLast_Click(object sender, EventArgs e) {
			var first = CalendarHelper.GetFirstOfMonthByDate(Calendar1.CalendarDate);
			Calendar1.CalendarDate = first.AddDays(-1).Date;
			txtDate.Text = CalendarHelper.GetFirstOfMonthByDate(Calendar1.CalendarDate).ToShortDateString();
			SetCalendar();
		}

		protected void btnNext_Click(object sender, EventArgs e) {
			var last = CalendarHelper.GetEndOfMonthByDate(Calendar1.CalendarDate);
			Calendar1.CalendarDate = last.AddDays(1).Date;
			txtDate.Text = CalendarHelper.GetFirstOfMonthByDate(Calendar1.CalendarDate).ToShortDateString();
			SetCalendar();
		}

		protected void SetCalendar() {
			SiteData site = SiteData.CurrentSite;
			var first = CalendarHelper.GetFirstOfMonthByDate(Calendar1.CalendarDate).Date;
			DateTime dtStart = CalendarHelper.GetFirstOfMonthByDate(first).AddMinutes(-15);
			DateTime dtEnd = CalendarHelper.GetEndOfMonthByDate(first).AddMinutes(15);

			dtStart = site.ConvertSiteTimeToUTC(dtStart);
			dtEnd = site.ConvertSiteTimeToUTC(dtEnd);

			var events = CalendarHelper.GetDisplayEvents(this.SiteID, dtStart, dtEnd, -1, false).ToList();

			events.ForEach(x => x.EventDate = site.ConvertUTCToSiteTime(x.EventDate));

			Calendar1.HilightDateList = (from dd in events select dd.EventDate.Date).Distinct().ToList();

			CalendarHelper.BindDataBoundControl(dgEvents, events);
		}
	}
}