using System;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {

	public partial class WebForm2 : System.Web.UI.Page {

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			ucCalendarAdminAddEdit.SiteID = Master.TestSiteID;
			ucCalendarAdminAddEdit.ModuleName = "CalendarAdminAddEdit";
		}

		protected void Page_Load(object sender, EventArgs e) {
		}
	}
}