using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;



namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarUpcoming : BaseShellUserControl, IWidgetParmData, IWidget {

		protected dbCalendarDataContext db = new dbCalendarDataContext();

		#region IWidgetParmData Members

		private Dictionary<string, string> _parms = new Dictionary<string, string>();
		public Dictionary<string, string> PublicParmValues {
			get { return _parms; }
			set { _parms = value; }
		}

		#endregion


		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		public Guid SiteID { get; set; }

		public string JSEditFunction {
			get { return ""; }
		}
		public bool EnableEdit {
			get { return true; }
		}

		#endregion

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
			if (SiteID == Guid.Empty) {
				SiteID = SiteData.CurrentSiteID;
			}

			if (!IsPostBack) {
				SetCalendar();
			}

		}


		protected void SetCalendar() {

			try {
				if (PublicParmValues.Count > 0) {
					DaysInPast = Convert.ToInt32((from c in PublicParmValues
												  where c.Key.ToLower() == "daysinpast"
												  select c.Value).FirstOrDefault());

					DaysInFuture = Convert.ToInt32((from c in PublicParmValues
													where c.Key.ToLower() == "daysinfuture"
													select c.Value).FirstOrDefault());
				}

			} catch (Exception ex) {
			}


			DateTime dtStart = DateTime.Now.AddDays(DaysInPast);
			DateTime dtEnd = DateTime.Now.AddDays(DaysInFuture);


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