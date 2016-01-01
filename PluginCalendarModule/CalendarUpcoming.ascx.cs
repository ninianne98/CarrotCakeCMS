using System;
using System.Collections.Generic;
using System.Linq;
using Carrotware.CMS.Interface;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {

	public partial class CalendarUpcoming : WidgetParmDataUserControl {

		public CalendarUpcoming() {
			this.DaysInFuture = 30;
			this.DaysInPast = -3;
		}

		public int DaysInPast { get; set; }

		public int DaysInFuture { get; set; }

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				SetCalendar();
			}
		}

		protected override void OnInit(EventArgs e) {
			if (this.PublicParmValues.Count > 0) {
				try {
					string sFoundVal = GetParmValue("DaysInPast", "-3");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.DaysInPast = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("DaysInFuture", "30");

					if (!String.IsNullOrEmpty(sFoundVal)) {
						this.DaysInFuture = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }
			}

			base.OnInit(e);
		}

		protected void SetCalendar() {
			DateTime dtStart = DateTime.Now.Date.AddDays(this.DaysInPast);
			DateTime dtEnd = DateTime.Now.Date.AddDays(this.DaysInFuture);

			using (dbCalendarDataContext db = dbCalendarDataContext.GetDataContext()) {
				var lst = (from c in db.tblCalendars
						   where c.EventDate >= dtStart
							&& c.EventDate < dtEnd
							&& c.IsActive == true
							&& c.SiteID == this.SiteID
						   orderby c.EventDate
						   select c).ToList();

				dgEvents.DataSource = lst;
				dgEvents.DataBind();
			}
		}
	}
}