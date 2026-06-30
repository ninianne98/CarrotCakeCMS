using Carrotware.CMS.Core;
using Carrotware.CMS.Security;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin {

	public class AdminBaseMasterPage : BaseMasterPage {
		protected SiteData siteHelper = new SiteData();
		protected SecurityHelper securityHelper = new SecurityHelper();

		public enum SectionID {
			SiteDashboard,
			SiteInfo,
			SiteTemplate,
			ContentIndex,
			ContentAdd,
			ContentSnippet,
			PageComment,
			StatusChange,
			ContentHistory,
			ContentTemplate,
			ContentSkinEdit,
			ContentSiteMap,
			SiteExport,
			DataImport,
			SiteImport,
			TextWidget,
			UserAdmin,
			GroupAdmin,
			SiteIndex,
			UserFn,
			Modules,
			BlogIndex,
			BlogContentAdd,
			BlogCategory,
			BlogTag,
			BlogTemplate,
			BlogComment
		}

		protected enum ControlLocation {
			PublicFooter,
			PopupFooter,
			MainFooter,
		}

		public static CmsSkin.SkinOption SiteSkin {
			get {
				return Helper.SiteSkin;
			}
		}

		public static string MainColorCode {
			get {
				return Helper.MainColorCode;
			}
		}

		protected void LoadFooterCtrl(PlaceHolder plcHolder, ControlLocation ctrlKey) {
			string controlPath = string.Empty;
			var config = CarrotCakeConfig.GetConfig();

			switch (ctrlKey) {
				case ControlLocation.PublicFooter:
					controlPath = config.AdminFooterControls.ControlPathPublic;
					break;

				case ControlLocation.PopupFooter:
					controlPath = config.AdminFooterControls.ControlPathPopup;
					break;

				case ControlLocation.MainFooter:
					controlPath = config.AdminFooterControls.ControlPathMain;
					break;
			}

			if (!string.IsNullOrEmpty(controlPath)) {
				if (File.Exists(Server.MapPath(controlPath))) {
					Control ctrl = new Control();
					ctrl = Page.LoadControl(controlPath);
					plcHolder.Controls.Add(ctrl);
				}
			}
		}

		public override void Dispose() {
			base.Dispose();

			if (securityHelper != null) {
				securityHelper.Dispose();
			}
		}
	}
}