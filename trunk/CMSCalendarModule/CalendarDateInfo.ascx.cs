using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarDateInfo : WidgetParmDataUserControl {

		public DateTime theEventDate = DateTime.Now.Date;

		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["calendardate"])) {
				theEventDate = Convert.ToDateTime(ParmParser.GetStringParameterFromQuery("calendardate"));
			}

			if (!IsPostBack) {
				SetCalendar();
			}
		}


		protected void SetCalendar() {
			using (dbCalendarDataContext db = new dbCalendarDataContext()) {

				var lst = (from c in db.tblCalendars
						   where c.EventDate == theEventDate
							&& c.IsActive == true
							&& c.SiteID == SiteID
						   orderby c.EventDate
						   select c).ToList();

				rEvents.DataSource = lst;
				rEvents.DataBind();
			}

		}


	}
}