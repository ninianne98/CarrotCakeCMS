using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarDisplay : WidgetParmDataUserControl {

		public string SpecifiedCSSFile { get; set; }

		public string JavascriptFunctionNameDate { get; set; }

		public string LaunchURLWindow { get; set; }

		protected override void OnInit(EventArgs e) {

			if (PublicParmValues.Count > 0) {
				try {
					string sFoundVal = GetParmValue("SpecifiedCSSFile", "");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.SpecifiedCSSFile = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("JavascriptFunctionNameDate", "");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.JavascriptFunctionNameDate = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("LaunchURLWindow", "");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						this.LaunchURLWindow = sFoundVal;
					}
				} catch (Exception ex) { }
			}


			if (!string.IsNullOrEmpty(this.LaunchURLWindow) && string.IsNullOrEmpty(this.JavascriptFunctionNameDate)) {
				this.JavascriptFunctionNameDate = "eventCalendarDateLaunch";
			}

			if (!string.IsNullOrEmpty(this.SpecifiedCSSFile)) {
				Calendar1.OverrideCSS = this.SpecifiedCSSFile;
			}

			if (!string.IsNullOrEmpty(this.JavascriptFunctionNameDate)) {
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
							&& c.SiteID == SiteID
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