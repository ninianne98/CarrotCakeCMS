﻿using Carrotware.CMS.Core;
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

	public partial class EventCalendarDisplayUpcoming : WidgetParmDataUserControl {
		private int _past = -2;

		public int DaysInPast {
			get { return _past; }
			set { _past = value; }
		}

		private int _future = 14;

		public int DaysInFuture {
			get { return _future; }
			set { _future = value; }
		}

		private int _top = 10;

		public int TakeTop {
			get { return _top; }
			set { _top = value; }
		}

		public string CalendarURL { get; set; }

		private DateTime lastDate = DateTime.MinValue.Date;

		public string GetDateNameString(DateTime eventDate, string dateFormat) {
			if (lastDate != eventDate) {
				lastDate = eventDate;
				return string.Format(dateFormat, lastDate);
			} else {
				return "";
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				SetCalendar();
			}
		}

		protected override void OnInit(EventArgs e) {
			if (PublicParmValues.Count > 0) {
				try {
					string sFoundVal = GetParmValue("CalendarURL", "");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.CalendarURL = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("DaysInPast", "-2");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.DaysInPast = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("DaysInFuture", "14");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.DaysInFuture = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("TakeTop", "10");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.TakeTop = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }
			}

			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);
		}

		public DateTime GetTimeFromTimeSpan(TimeSpan? timeSpan) {
			return CalendarHelper.GetFullDateTime(timeSpan);
		}

		protected void SetCalendar() {
			SiteData site = SiteData.CurrentSite;

			DateTime dtStart = site.Now.AddDays(this.DaysInPast).Date;
			DateTime dtEnd = site.Now.AddDays(this.DaysInFuture).Date;

			dtStart = site.ConvertSiteTimeToUTC(dtStart);
			dtEnd = site.ConvertSiteTimeToUTC(dtEnd);

			var events = CalendarHelper.GetDisplayEvents(this.SiteID, dtStart, dtEnd, this.TakeTop, true);

			events.ForEach(x => x.EventDate = site.ConvertUTCToSiteTime(x.EventDate));

			CalendarHelper.BindRepeater(rpDates, events);

			if (!string.IsNullOrEmpty(this.CalendarURL)) {
				lnkHyper.NavigateUrl = this.CalendarURL;
				phLink.Visible = true;
			} else {
				phLink.Visible = false;
			}
		}
	}
}