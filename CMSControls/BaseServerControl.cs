using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	public abstract class BaseServerControl : WidgetParmDataWebControl {
		protected ISiteNavHelper _navHelper = SiteNavFactory.GetSiteNavHelper();

		protected void SetSiteID() {
			try {
				this.SiteID = SiteData.CurrentSiteID;
			} catch { }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPostBack {
			get {
				string method = "GET";
				try { method = HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"].ToString().ToUpperInvariant(); } catch { }
				return method != "GET" ? true : false;
			}
		}

		protected override void OnInit(EventArgs e) {
			SetSiteID();

			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			try {
				if (this.PublicParmValues.Any()) {
					string sTmp = "";

					sTmp = GetParmValue("CssClass", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CssClass = sTmp;
					}
				}
			} catch (Exception ex) {
			}
		}

		public override void Dispose() {
			base.Dispose();

			if (_navHelper != null) {
				_navHelper.Dispose();
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer) {
		}

		protected void BaseRender(HtmlTextWriter writer) {
			base.Render(writer);
		}

		protected void BaseRenderContents(HtmlTextWriter writer) {
			base.RenderContents(writer);
		}

		protected string GetParentPageName() {
			SiteNav nav = GetParentPage();

			return nav.FileName.ToLowerInvariant();
		}

		protected bool AreFilenamesSame(string parm1, string parm2) {
			if (parm1 == null || parm2 == null) {
				return false;
			}

			return (parm1.ToLowerInvariant() == parm2.ToLowerInvariant()) ? true : false;
		}

		protected List<SiteNav> GetPageNavTree() {
			return _navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);
		}

		protected SiteNav GetParentPage() {
			SiteNav pageNav = _navHelper.GetParentPageNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName);

			//assign bogus page name for comp purposes
			if (pageNav == null) {
				pageNav = new SiteNav();
				pageNav.Root_ContentID = Guid.Empty;
				pageNav.FileName = "javascript:void(0);";
				pageNav.TemplateFile = "/##/##/";
			}

			return pageNav;
		}

		protected SiteNav GetCurrentPage() {
			SiteNav pageNav = null;
			ControlUtilities cu = new ControlUtilities(this);
			ContentPage cp = cu.GetContainerContentPage(this);

			if (cp != null) {
				pageNav = cp.GetSiteNav();
			} else {
				pageNav = _navHelper.FindByFilename(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName);
				//assign bogus page name for comp purposes
				if (pageNav == null) {
					pageNav = new SiteNav();
					pageNav.Root_ContentID = Guid.Empty;
					pageNav.FileName = "javascript:void(0);";
					pageNav.TemplateFile = "/##/##/";
				}
			}
			if (pageNav == null && SiteData.IsLikelyFakeSearch()) {
				pageNav = SiteNavHelper.GetEmptySearch();
			}

			pageNav.SiteID = SiteData.CurrentSiteID;

			return pageNav;
		}

		public static string GetCtrlText(Control ctrl) {
			return ctrl.RenderControl();
		}

		public static SiteNav FixNavLinkText(SiteNav nav) {
			return CMSConfigHelper.FixNavLinkText(nav);
		}
	}
}