using System;
using System.Collections.Generic;
using System.Globalization;
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
	public partial class EventCalendarDisplay2 : WidgetParmDataUserControl {

		public string SpecifiedCSSFile { get; set; }

		public string JavascriptFunctionNameDate { get; set; }

		public string LaunchURLWindow { get; set; }

		protected void Page_Load(object sender, EventArgs e) {

			if (!IsPostBack) {
				Calendar1.CalendarDate = SiteData.CurrentSite.Now.Date;

				SetCalendar();

				SiteData site = SiteData.CurrentSite;

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
				return String.Format(dateFormat, lastDate);
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

			DateTime dtStart = Calendar1.CalendarDate.AddDays(1 - Calendar1.CalendarDate.Day).Date;
			DateTime dtEnd = dtStart.AddMonths(1).Date;

			dtStart = site.ConvertSiteTimeToUTC(dtStart);
			dtEnd = site.ConvertSiteTimeToUTC(dtEnd);

			List<vw_carrot_CalendarEvent> lst = null;

			using (CalendarDataContext db = new CalendarDataContext()) {

				lst = (from c in db.vw_carrot_CalendarEvents
					   where c.EventDate >= dtStart
						&& c.EventDate < dtEnd
						&& c.SiteID == SiteID
						&& c.IsPublic == true
						&& (!c.IsCancelledEvent || c.IsCancelledPublic)
						&& (!c.IsCancelledSeries || c.IsCancelledPublic)
					   orderby c.EventDate ascending, c.EventStartTime ascending, c.IsCancelledEvent ascending
					   select c).ToList();

			}

			lst.ForEach(x => x.EventDate = site.ConvertUTCToSiteTime(x.EventDate));

			List<DateTime> dates = (from dd in lst select dd.EventDate.Date).Distinct().ToList();

			List<Guid> cats = (from dd in lst select dd.CalendarEventCategoryID).Distinct().ToList();

			Calendar1.HilightDateList = dates;

			CalendarHelper.BindRepeater(rpEvent, lst);

			if (lst.Count > 0) {
				phNone.Visible = false;
			} else {
				phNone.Visible = true;
			}

			SetDDLSelections();

			CalendarHelper.BindRepeater(rpCat, CalendarHelper.GetCalendarCategories(SiteID).Where(x => cats.Contains(x.CalendarEventCategoryID)));
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