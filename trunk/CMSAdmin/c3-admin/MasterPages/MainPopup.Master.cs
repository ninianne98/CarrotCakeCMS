using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;

namespace Carrotware.CMS.UI.Admin.c3_admin.MasterPages {
	public partial class MainPopup : AdminBaseMasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				pnlDirty.Visible = false; 
			} else {
				pnlDirty.Visible = true;
			}

			LoadFooterCtrl(plcFooter, "Carrotware.CMS.UI.Admin.c3_admin.MasterPages.MainPopup.Ctrl");
		}

		// so that it is easy to toggle master pages
		public void ActivateTab(SectionID sectionID) {
		}

		protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e) {
			string sError = String.Empty;

			if (e.Exception != null) {
				Exception objErr = e.Exception;
				sError = objErr.Message;
				if (objErr.StackTrace != null) {
					sError += "\r\n<hr />\r\n" + objErr.StackTrace;
				}

				if (objErr.InnerException != null) {
					sError += "\r\n<hr />\r\n" + objErr.InnerException;
				}
			} else {
				sError = " An error occurred. ";
			}

			ScriptManager1.AsyncPostBackErrorMessage = sError;
		}

	}
}
