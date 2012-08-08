using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carrotware.CMS.Interface {
	public abstract class AdminModule : BaseShellUserControl, IAdminModule {


		public string CreateLink(string sModuleName, string sIDParm) {
			var sQueryStringFile = CurrentScriptName + "?" + string.Format(QueryStringPattern, sModuleName) + "&" + sIDParm;

			return sQueryStringFile;
		}

		public string CreateLink(string sScriptName, string sModuleName, string sIDParm) {
			var sQueryStringFile = sScriptName + "?" + string.Format(QueryStringPattern, sModuleName) + "&" + sIDParm;

			return sQueryStringFile;
		}

		public string CreateLink(string sModuleName) {
			var sQueryStringFile = CurrentScriptName + "?" + string.Format(QueryStringPattern, sModuleName);

			return sQueryStringFile;
		}


		#region IAdminModule Members

		public Guid SiteID { get; set; }

		public Guid ModuleID { get; set; }

		public string QueryStringFragment { get; set; }

		public string QueryStringPattern { get; set; }

		public string ModuleName { get; set; }

		#endregion

	}
}
