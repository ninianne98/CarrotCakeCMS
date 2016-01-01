using System;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {

	public partial class WebForm1 : System.Web.UI.Page {

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			ucCalendarAdmin.SiteID = Master.TestSiteID;
			ucCalendarAdmin.ModuleName = "CalendarAdmin";
		}

		protected void Page_Load(object sender, EventArgs e) {
		}
	}
}