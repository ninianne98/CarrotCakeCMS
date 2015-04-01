using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.Web.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin.MasterPages {
	public partial class MainPopup : AdminBaseMasterPage {

		public bool ShowSaved { get; set; }
		public bool UsesSaved { get; set; }

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				pnlDirty.Visible = false;
			} else {
				pnlDirty.Visible = true;
			}


			if (!this.UsesSaved) {
				HideSave();
			} else {

				if (this.ShowSaved || !string.IsNullOrEmpty(Request.QueryString["showsaved"])) {
					this.ShowSaved = true;
					ShowSave();
				}
			}

			LoadFooterCtrl(plcFooter, ControlLocation.PopupFooter);
		}

		public string SavedSuffix {
			get {

				return "&showsaved=true";
			}
		}

		public void SetSaveMessage(string saveMessage) {
			litSaveMessage.Text = saveMessage;
		}

		public void ShowSave() {
			this.ShowSaved = true;
			hdnShow.Value = "SHOW";
		}


		public void HideSave() {
			this.ShowSaved = false;
			hdnShow.Value = String.Empty;
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

				SiteData.WriteDebugException("popup master - AsyncPostBackError", objErr);
			} else {
				sError = " An error occurred. (Generic Main) ";
			}

			ScriptManager1.AsyncPostBackErrorMessage = sError;
		}

	}
}
