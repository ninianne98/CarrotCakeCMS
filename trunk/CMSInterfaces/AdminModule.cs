using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carrotware.CMS.Interface {
	public abstract class AdminModule : BaseShellUserControl, IAdminModule {

		public string CreateLink(string sModuleName, string sIDParm) {

			return CreateLink(false, sModuleName, sIDParm);
		}

		public string CreateLink(string sScriptName, string sModuleName, string sIDParm) {

			return CreateLink(false, sScriptName, sModuleName, sIDParm);
		}

		public string CreateLink(string sModuleName) {

			return CreateLink(false, sModuleName);
		}


		public string CreatePopupLink(string sModuleName, string sIDParm) {

			return CreateLink(true, sModuleName, sIDParm);
		}

		public string CreatePopupLink(string sScriptName, string sModuleName, string sIDParm) {

			return CreateLink(true, sScriptName, sModuleName, sIDParm);
		}

		public string CreatePopupLink(string sModuleName) {

			return CreateLink(true, sModuleName);
		}

		private string CreateLink(bool bPop, string sModuleName, string sIDParm) {
			string sQueryStringFile = "";
			string sSuffix = string.Format(QueryStringPattern, sModuleName) + "&" + sIDParm;
			if (bPop) {
				sQueryStringFile = String.Format("javascript:ShowWindowNoRefresh('{0}');", "./ModulePopup.aspx?" + sSuffix);
			} else {
				sQueryStringFile = CurrentScriptName + "?" + sSuffix;
			}
			return sQueryStringFile;
		}

		private string CreateLink(bool bPop, string sScriptName, string sModuleName, string sIDParm) {
			string sQueryStringFile = "";
			string sSuffix = string.Format(QueryStringPattern, sModuleName) + "&" + sIDParm;
			if (bPop) {
				sQueryStringFile = String.Format("javascript:ShowWindowNoRefresh('{0}');", sScriptName + "?" + sSuffix);
			} else {
				sQueryStringFile = sScriptName + "?" + sSuffix;
			}
			return sQueryStringFile;
		}

		private string CreateLink(bool bPop, string sModuleName) {
			string sQueryStringFile = "";
			string sSuffix = string.Format(QueryStringPattern, sModuleName);
			if (bPop) {
				sQueryStringFile = String.Format("javascript:ShowWindowNoRefresh('{0}');", "./ModulePopup.aspx?" + sSuffix);
			} else {
				sQueryStringFile = CurrentScriptName + "?" + sSuffix;
			}
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
