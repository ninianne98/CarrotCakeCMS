using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carrotware.CMS.Interface;

namespace Carrotware.CMS.UI.Plugins.CalendarModule {

	public partial class Site1 : System.Web.UI.MasterPage {

		public Guid TestSiteID {
			get {
				return ConfigurationManager.AppSettings["TestSiteID"] != null
					? new Guid(ConfigurationManager.AppSettings["TestSiteID"].ToString())
					: Guid.Empty;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			string pf = AdminModuleQueryStringRoutines.GetPluginFile();
			Guid id = ParmParser.GetGuidIDFromQuery();

			if (!String.IsNullOrEmpty(pf)) {
				Response.Redirect(String.Format("/{0}.aspx?id={1}", pf, id));
			}
		}
	}
}