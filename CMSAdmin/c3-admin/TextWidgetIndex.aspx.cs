using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class TextWidgetIndex : AdminBasePage {

		protected List<CMSTextWidgetPicker> PickerValues {
			get { return (List<CMSTextWidgetPicker>)ViewState["TextWidgetPicker"]; }
			set {
				ViewState["TextWidgetPicker"] = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.TextWidget);

			if (!IsPostBack) {
				using (CMSConfigHelper cfg = new CMSConfigHelper()) {
					PickerValues = cfg.GetAllWidgetSettings(SiteID);
					gvContent.DataSource = PickerValues;
					gvContent.DataBind();
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {

			List<CMSTextWidgetPicker> lst = PickerValues;

			List<Guid> lstUpd1 = GeneralUtilities.GetCheckedItemGuidsByValue(gvContent, true, "chkSelect1");
			List<Guid> lstUpd2 = GeneralUtilities.GetCheckedItemGuidsByValue(gvContent, true, "chkSelect2");
			List<Guid> lstUpd3 = GeneralUtilities.GetCheckedItemGuidsByValue(gvContent, true, "chkSelect3");
			List<Guid> lstUpd4 = GeneralUtilities.GetCheckedItemGuidsByValue(gvContent, true, "chkSelect4");
			List<Guid> lstUpd5 = GeneralUtilities.GetCheckedItemGuidsByValue(gvContent, true, "chkSelect5");

			lst.ForEach(x => {
				x.ProcessBody = lstUpd1.Contains(x.TextWidgetPickerID);
				x.ProcessPlainText = lstUpd2.Contains(x.TextWidgetPickerID);
				x.ProcessHTMLText = lstUpd3.Contains(x.TextWidgetPickerID);
				x.ProcessComment = lstUpd4.Contains(x.TextWidgetPickerID);
				x.ProcessSnippet = lstUpd5.Contains(x.TextWidgetPickerID);
			});

			foreach (CMSTextWidgetPicker w in lst) {
				TextWidget ww = new TextWidget();
				ww.SiteID = SiteID;
				ww.TextWidgetID = w.TextWidgetPickerID;
				ww.TextWidgetAssembly = w.AssemblyString;

				ww.ProcessBody = w.ProcessBody;
				ww.ProcessPlainText = w.ProcessPlainText;
				ww.ProcessHTMLText = w.ProcessHTMLText;
				ww.ProcessComment = w.ProcessComment;
				ww.ProcessSnippet = w.ProcessSnippet;

				if (ww.ProcessBody || ww.ProcessPlainText || ww.ProcessHTMLText || ww.ProcessComment || ww.ProcessSnippet) {
					ww.Save();
				} else {
					ww.Delete();
				}
			}

			if (SiteData.CurretSiteExists) {
				SiteData.CurrentSite.LoadTextWidgets();
			}

			Response.Redirect(SiteData.CurrentScriptName);
		}


	}
}