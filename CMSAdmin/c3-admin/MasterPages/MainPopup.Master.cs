﻿using Carrotware.CMS.Core;
using System;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin.MasterPages {

	public partial class MainPopup : AdminBaseMasterPage {
		public bool ShowSaved { get; set; }
		public bool UsesSaved { get; set; }

		public string AntiCache {
			get {
				return Helper.AntiCache;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			ResetSave();

			siteSkin.SelectedColor = AdminBaseMasterPage.SiteSkin;

			if (!IsPostBack) {
				pnlDirty.Visible = false;
			} else {
				pnlDirty.Visible = true;
			}

			if (!this.Page.Title.StartsWith(SiteData.CarrotCakeCMSVersionMM)) {
				this.Page.Title = string.Format("{0} - {1}", SiteData.CarrotCakeCMSVersionMM, this.Page.Title);
			}

			if (!this.UsesSaved) {
				HideSave();
			} else {
				if (this.ShowSaved || !string.IsNullOrEmpty(this.ShowSaveQuery)) {
					this.ShowSaved = true;
					ShowSave();
				}
			}

			LoadFooterCtrl(plcFooter, ControlLocation.PopupFooter);
		}

		protected string ShowSaveQuery {
			get {
				return Request.QueryString["showsaved"] != null
					? Request.QueryString["showsaved"].ToString()
					: string.Empty;
			}
		}

		public string SavedSuffix {
			get {
				return "&showsaved=true";
			}
		}

		public void SetSaveMessage(string saveMessage) {
			litSaveMessage.Text = saveMessage;
		}

		public void ShowSave(string saveMessage) {
			SetSaveMessage(saveMessage);

			ShowSave();
		}

		public void ShowSave() {
			this.ShowSaved = true;
			hdnShow.Value = "SHOW";
		}

		public void HideSave() {
			this.ShowSaved = false;
			ResetSave();
		}

		public void ResetSave() {
			hdnShow.Value = string.Empty;
		}

		// so that it is easy to toggle master pages
		public void ActivateTab(SectionID sectionID) { }

		protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e) {
			string sError = string.Empty;

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