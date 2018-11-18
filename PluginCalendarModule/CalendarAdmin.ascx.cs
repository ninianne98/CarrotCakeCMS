using Carrotware.CMS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {

	public partial class CalendarAdmin : AdminModule {
		protected dbCalendarDataContext db = dbCalendarDataContext.GetDataContext();

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion IDisposable Members

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				txtDate.Text = DateTime.Now.ToShortDateString();
				LoadData();
			}
		}

		protected void btnLast_Click(object sender, EventArgs e) {
			Calendar1.CalendarDate = Calendar1.CalendarDate.AddMonths(-1);
			LoadData();
		}

		protected void btnNext_Click(object sender, EventArgs e) {
			Calendar1.CalendarDate = Calendar1.CalendarDate.AddMonths(1);
			LoadData();
		}

		protected void LoadData() {
			DateTime dtStart = Calendar1.CalendarDate.AddDays(1 - Calendar1.CalendarDate.Day);
			DateTime dtEnd = dtStart.AddMonths(1);

			var lst = (from c in db.tblCalendars
					   where c.EventDate >= dtStart
						&& c.EventDate < dtEnd
						&& c.SiteID == SiteID
					   orderby c.EventDate
					   select c).ToList();

			List<DateTime> dates = (from dd in lst
									select Convert.ToDateTime(dd.EventDate).Date).Distinct().ToList();

			Calendar1.HilightDateList = dates;

			dgEvents.DataSource = lst;
			dgEvents.DataBind();

			txtDate.Text = dtStart.AddDays(1 - dtStart.Day).ToShortDateString();
		}

		protected void txtDate_TextChanged(object sender, EventArgs e) {
			Calendar1.CalendarDate = Convert.ToDateTime(txtDate.Text).Date;
			LoadData();
		}
	}
}