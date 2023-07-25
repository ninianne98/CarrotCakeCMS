using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
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

	public partial class EventCalendarDisplay : WidgetParmDataUserControl {
		public string SpecifiedCSSFile { get; set; }

		public string JavascriptFunctionNameDate { get; set; }

		public string LaunchURLWindow { get; set; }

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				SiteData site = SiteData.CurrentSite;

				Calendar1.CalendarDate = site.Now.Date;

				SetCalendar();

				List<int> lstYears = new List<int>();
				lstYears = Enumerable.Range(DateTime.Now.Year - 10, 20).ToList();
				List<string> monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames.Where(x => x.Length > 0).ToList();

				CalendarHelper.BindDropDownList(ddlYear, lstYears);
				CalendarHelper.BindDropDownList(ddlMonth, monthNames);

				SetDDLSelections();
			}
		}

		private void SetDDLSelections() {
			ddlMonth.SelectedValue = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Calendar1.CalendarDate.Month);
			ddlYear.SelectedValue = Calendar1.CalendarDate.Year.ToString();
		}

		private DateTime lastDate = DateTime.MinValue.Date;

		public string GetDateNameString(DateTime eventDate, string dateFormat) {
			if (lastDate != eventDate) {
				lastDate = eventDate;
				return string.Format(dateFormat, lastDate);
			} else {
				return "";
			}
		}

		protected override void OnInit(EventArgs e) {
			if (PublicParmValues.Count > 0) {
				try {
					string sFoundVal = GetParmValue("SpecifiedCSSFile", "");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						SpecifiedCSSFile = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("JavascriptFunctionNameDate", "");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						JavascriptFunctionNameDate = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("LaunchURLWindow", "");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						LaunchURLWindow = sFoundVal;
					}
				} catch (Exception ex) { }
			}

			if (!string.IsNullOrEmpty(LaunchURLWindow) && string.IsNullOrEmpty(JavascriptFunctionNameDate)) {
				JavascriptFunctionNameDate = "eventCalendarDateLaunch";
			}

			if (!string.IsNullOrEmpty(SpecifiedCSSFile)) {
				Calendar1.OverrideCSS = SpecifiedCSSFile;
			}

			if (!string.IsNullOrEmpty(JavascriptFunctionNameDate)) {
				Calendar1.JavascriptForDate = JavascriptFunctionNameDate;
			}

			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);
		}

		public DateTime GetTimeFromTimeSpan(TimeSpan? timeSpan) {
			return CalendarHelper.GetFullDateTime(timeSpan);
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

			DateTime dtStart = new DateTime(this.Calendar1.CalendarDate.Year, this.Calendar1.CalendarDate.Month, 1).Date;
			DateTime dtEnd = dtStart.AddMonths(1).Date;

			dtStart = site.ConvertSiteTimeToUTC(dtStart);
			dtEnd = site.ConvertSiteTimeToUTC(dtEnd);

			var events = CalendarHelper.GetDisplayEvents(this.SiteID, dtStart, dtEnd, -1, true).ToList();

			events.ForEach(x => x.EventDate = site.ConvertUTCToSiteTime(x.EventDate));

			List<DateTime> dates = (from dd in events select dd.EventDate.Date).Distinct().ToList();

			List<Guid> cats = (from dd in events select dd.CalendarEventCategoryID).Distinct().ToList();

			Calendar1.HilightDateList = dates;

			CalendarHelper.BindDataBoundControl(dgEvents, events);

			SetDDLSelections();

			CalendarHelper.BindRepeater(rpCat, CalendarHelper.GetCalendarCategories(this.SiteID).Where(x => cats.Contains(x.CalendarEventCategoryID)));
		}

		protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e) {
			Calendar1.CalendarDate = new DateTime(int.Parse(ddlYear.SelectedValue), ddlMonth.SelectedIndex + 1, 15);
			SetCalendar();
		}

		protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e) {
			Calendar1.CalendarDate = new DateTime(int.Parse(ddlYear.SelectedValue), ddlMonth.SelectedIndex + 1, 15);
			SetCalendar();
		}
	}
}