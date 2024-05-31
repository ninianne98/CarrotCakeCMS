using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

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

	public class AdminBaseMasterPage : BaseMasterPage {
		protected SiteData siteHelper = new SiteData();

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

		public static CmsSkin.SkinOption _theme = CmsSkin.SkinOption.None;

		public static CmsSkin.SkinOption SiteSkin {
			get {
				if (_theme == CmsSkin.SkinOption.None) {
					var config = CarrotCakeConfig.GetConfig();
					string skin = config.MainConfig.SiteSkin;
					var actualSkin = CmsSkin.SkinOption.Classic;
					try { actualSkin = (CmsSkin.SkinOption)Enum.Parse(typeof(CmsSkin.SkinOption), skin, true); } catch { }

					_theme = actualSkin;
				}

				return _theme;
			}
		}

		public static string MainColorCode {
			get {
				return CmsSkin.GetPrimaryColorCode(SiteSkin);
			}
		}

		protected void LoadFooterCtrl(PlaceHolder plcHolder, ControlLocation CtrlKey) {
			string controlPath = string.Empty;
			var config = CarrotCakeConfig.GetConfig();

			switch (CtrlKey) {
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
	}
}