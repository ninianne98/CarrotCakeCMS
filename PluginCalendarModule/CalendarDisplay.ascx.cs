using System;
using System.Collections.Generic;
using System.Linq;
using Carrotware.CMS.Interface;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {

	public partial class CalendarDisplay : WidgetParmDataUserControl {
		public string SpecifiedCSSFile { get; set; }

		public string JavascriptFunctionNameDate { get; set; }

		public string LaunchURLWindow { get; set; }

		protected override void OnInit(EventArgs e) {
			if (this.PublicParmValues.Any()) {
				try {
					string sFoundVal = GetParmValue("SpecifiedCSSFile", "");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.SpecifiedCSSFile = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("JavascriptFunctionNameDate", "");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.JavascriptFunctionNameDate = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("LaunchURLWindow", "");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.LaunchURLWindow = sFoundVal;
					}
				} catch (Exception ex) { }
			}

			if (!String.IsNullOrEmpty(this.LaunchURLWindow) && String.IsNullOrEmpty(this.JavascriptFunctionNameDate)) {
				this.JavascriptFunctionNameDate = "eventCalendarDateLaunch";
			}

			if (!String.IsNullOrEmpty(this.SpecifiedCSSFile)) {
				Calendar1.OverrideCSS = this.SpecifiedCSSFile;
			}

			if (!String.IsNullOrEmpty(this.JavascriptFunctionNameDate)) {
				Calendar1.JavascriptForDate = this.JavascriptFunctionNameDate;
			}

			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);
		}

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				SetCalendar();
			}
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
			DateTime dtStart = Calendar1.CalendarDate.AddDays(1 - Calendar1.CalendarDate.Day);
			DateTime dtEnd = dtStart.AddMonths(1);

			using (dbCalendarDataContext db = dbCalendarDataContext.GetDataContext()) {
				var lst = (from c in db.tblCalendars
						   where c.EventDate >= dtStart
							&& c.EventDate < dtEnd
							&& c.IsActive == true
							&& c.SiteID == this.SiteID
						   orderby c.EventDate
						   select c).ToList();

				List<DateTime> dates = (from dd in lst
										select Convert.ToDateTime(dd.EventDate).Date).Distinct().ToList();

				Calendar1.HilightDateList = dates;

				dgEvents.DataSource = lst;
				dgEvents.DataBind();
			}
		}
	}
}