using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarDisplay : BaseShellUserControl, IWidgetParmData, IWidget {

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

		public string SpecifiedCSSFile { get; set; }


		protected override void OnPreRender(EventArgs e) {

			if (SiteID == Guid.Empty) {
				SiteID = SiteData.CurrentSiteID;
			}

			try {
				string sCSS = "";
				if (PublicParmValues.Count > 0) {
					sCSS = (from c in PublicParmValues
							where c.Key.ToLower() == "specifiedcssfile"
							select c.Value).FirstOrDefault();
				}
				if (!string.IsNullOrEmpty(sCSS)) {
					SpecifiedCSSFile = sCSS;
				}

			} catch (Exception ex) {
			}

			if (!string.IsNullOrEmpty(SpecifiedCSSFile)) {
				Calendar1.OverrideCSS = SpecifiedCSSFile;
			}

			base.OnInit(e);
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


			var lst = (from c in db.tblCalendars
					   where c.EventDate >= dtStart
						&& c.EventDate < dtEnd
						&& c.IsActive == true
						&& c.SiteID == SiteID
					   orderby c.EventDate
					   select c).ToList();

			List<DateTime> dates = (from dd in lst
									select Convert.ToDateTime(dd.EventDate)).ToList();

			Calendar1.HilightDateList = dates;

			dgEvents.DataSource = lst;
			dgEvents.DataBind();

		}



	}
}