using System;
using System.Collections.Generic;
using System.Linq;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {

	public partial class WebForm3 : System.Web.UI.Page {

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			ucCalendarDisplay.SiteID = Master.TestSiteID;
		}

		protected void Page_Load(object sender, EventArgs e) {
		}
	}
}