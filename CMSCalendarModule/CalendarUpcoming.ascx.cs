using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;



namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarUpcoming : WidgetParmDataUserControl {

		private int _past = -3;
		public int DaysInPast {
			get { return _past; }
			set { _past = value; }
		}

		private int _future = 30;
		public int DaysInFuture {
			get { return _future; }
			set { _future = value; }
		}



		protected void Page_Load(object sender, EventArgs e) {

			if (!IsPostBack) {
				SetCalendar();
			}
		}

		protected override void OnInit(EventArgs e) {

			if (PublicParmValues.Count > 0) {
				try {
					string sFoundVal = GetParmValue("DaysInPast", "-3");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						DaysInPast = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("DaysInFuture", "30");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						DaysInFuture = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }
			}

			base.OnInit(e);
		}


		protected void SetCalendar() {

			DateTime dtStart = DateTime.Now.AddDays(DaysInPast);
			DateTime dtEnd = DateTime.Now.AddDays(DaysInFuture);

			using (dbCalendarDataContext db = new dbCalendarDataContext()) {
				var lst = (from c in db.tblCalendars
						   where c.EventDate >= dtStart
							&& c.EventDate < dtEnd
							&& c.IsActive == true
							&& c.SiteID == SiteID
						   orderby c.EventDate
						   select c).ToList();


				dgEvents.DataSource = lst;
				dgEvents.DataBind();
			}

		}


	}
}