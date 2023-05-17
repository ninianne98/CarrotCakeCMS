﻿using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Carrotware.CMS.UI.Controls.CmsSkin;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin {

	public class AdminBasePage : BasePage {

		public SkinOption SiteSkin {
			get {
				return AdminBaseMasterPage.SiteSkin;
			}
		}

		public string MainColorCode {
			get {
				return AdminBaseMasterPage.MainColorCode;
			}
		}

		protected override void OnInit(EventArgs e) {
			if (SecurityData.IsAuthenticated) {
				bool bHasAccess = siteHelper.VerifyUserHasSiteAccess(SiteData.CurrentSiteID, SecurityData.CurrentUserGuid);

				if (!bHasAccess) {
					SecurityData.ResetAuth();
					Response.Redirect(SiteFilename.LogonURL);
				}
			}

			Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
			DateTime dtExpire = System.DateTime.Now.AddMinutes(-5);
			Response.Cache.SetExpires(dtExpire);

			base.OnInit(e);
		}

		protected Guid GetGuidPageIDFromQuery() {
			return GeneralUtilities.GetGuidPageIDFromQuery();
		}

		protected Guid GetGuidIDFromQuery() {
			return GeneralUtilities.GetGuidIDFromQuery();
		}

		protected Guid GetGuidParameterFromQuery(string ParmName) {
			return GeneralUtilities.GetGuidParameterFromQuery(ParmName);
		}

		protected string GetStringParameterFromQuery(string ParmName) {
			return GeneralUtilities.GetStringParameterFromQuery(ParmName);
		}

		protected void RedirectIfNoSite() {
			if (!SiteData.CurretSiteExists) {
				Response.Redirect(SiteFilename.SiteInfoURL);
			}
		}

		public void SetBlankText(ITextControl textControl) {
			if (String.IsNullOrEmpty(textControl.Text)) {
				textControl.Text = ContentPage.PageContentEmpty;
			}
		}

		public string ClickFileBrowser(Control ctrl) {
			return String.Format("cmsFileBrowserOpenReturn('{0}'); return false;", ctrl.ClientID);
		}

		public string ClickFileBrowserPop(Control ctrl) {
			return String.Format("cmsFileBrowserOpenReturnPop('{0}'); return false;", ctrl.ClientID);
		}

		public void PreselectCheckboxRepeater(Repeater repeater, List<IContentMetaInfo> lst) {
			foreach (RepeaterItem r in repeater.Items) {
				CheckBox chk = (CheckBox)r.FindControl("chk");
				Guid id = new Guid(chk.Attributes["value"].ToString());
				if (lst.Where(x => x.ContentMetaInfoID == id).Count() > 0) {
					chk.Checked = true;
				}
			}
		}

		public List<Guid> CollectCheckboxRepeater(Repeater repeater) {
			List<Guid> lst = new List<Guid>();
			foreach (RepeaterItem r in repeater.Items) {
				CheckBox chk = (CheckBox)r.FindControl("chk");
				Guid id = new Guid(chk.Attributes["value"].ToString());
				if (chk.Checked) {
					lst.Add(id);
				}
			}
			return lst;
		}
	}
}