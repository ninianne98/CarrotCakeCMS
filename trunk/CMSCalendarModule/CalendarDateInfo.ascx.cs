using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;



namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarDateInfo : BaseShellUserControl, IWidget {

		protected dbCalendarDataContext db = new dbCalendarDataContext();

		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		public Guid SiteID { get; set; }

		public string JSEditFunction {
			get { return ""; }
		}
		public bool EnableEdit {
			get { return false; }
		}

		#endregion

		public DateTime theEventDate = DateTime.Now;

		protected void Page_Load(object sender, EventArgs e) {
			if (SiteID == Guid.Empty) {
				SiteID = SiteData.CurrentSiteID;
			}

			if (!string.IsNullOrEmpty(Request.QueryString["calendardate"])) {
				theEventDate = Convert.ToDateTime(Request.QueryString["calendardate"].ToString());
			}

			if (!IsPostBack) {
				SetCalendar();
			}
		}


		protected void SetCalendar() {

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