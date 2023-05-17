using Carrotware.CMS.Core;
using System;

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

	public partial class Public : AdminBaseMasterPage {

		public string AntiCache {
			get {
				return Helper.AntiCache;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			LoadFooterCtrl(plcFooter, ControlLocation.PublicFooter);

			siteSkin.SelectedColor = AdminBaseMasterPage.SiteSkin;

			if (!this.Page.Title.StartsWith(SiteData.CarrotCakeCMSVersionMM)) {
				this.Page.Title = string.Format("{0} - {1}", SiteData.CarrotCakeCMSVersionMM, this.Page.Title);
			}

			litCMSBuildInfo.Text = SiteData.CarrotCakeCMSVersion;

			phLogin.Visible = !SecurityData.IsAuthenticated;

#if DEBUG
			metaCrawl.Visible = false;
#endif
		}
	}
}