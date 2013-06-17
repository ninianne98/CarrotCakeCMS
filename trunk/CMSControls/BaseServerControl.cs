using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	public abstract class BaseServerControl : WidgetParmDataWebControl {

		protected SiteNavHelper navHelper = new SiteNavHelper();

		protected void SetSiteID() {
			try {
				SiteID = SiteData.CurrentSiteID;
			} catch { }
		}

		public bool IsPostBack {
			get {
				string sReq = "GET";
				try { sReq = HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"].ToString().ToUpper(); } catch { }
				return sReq != "GET" ? true : false;
			}
		}

		protected override void OnInit(EventArgs e) {
			SetSiteID();

			base.OnInit(e);
		}

		public override void Dispose() {
			base.Dispose();

			if (navHelper != null) {
				navHelper.Dispose();
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter output) {

		}

		protected void BaseRender(HtmlTextWriter writer) {
			base.Render(writer);
		}

		protected void BaseRenderContents(HtmlTextWriter output) {
			base.RenderContents(output);
		}

		protected string GetParentPageName() {
			SiteNav nav = GetParentPage();

			return nav.FileName.ToLower();
		}

		protected bool AreFilenamesSame(string sParm1, string sParm2) {

			if (sParm1 == null || sParm2 == null) {
				return false;
			}

			return (sParm1.ToLower() == sParm2.ToLower()) ? true : false;
		}

		protected SiteNav GetParentPage() {

			SiteNav pageNav = navHelper.GetParentPageNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName);

			//assign bogus page name for comp purposes
			if (pageNav == null) {
				pageNav = new SiteNav();
				pageNav.Root_ContentID = Guid.Empty;
				pageNav.FileName = "/##/";
				pageNav.TemplateFile = "/##/";
			}

			return pageNav;
		}

		protected SiteNav GetCurrentPage() {
			SiteNav pageNav = null;
			ControlUtilities cu = new ControlUtilities(this);
			ContentPage cp = cu.GetContainerContentPage(this);

			if (cp != null) {
				pageNav = new SiteNav();
				pageNav.Root_ContentID = cp.Root_ContentID;
				pageNav.FileName = cp.FileName;
				pageNav.TemplateFile = cp.TemplateFile;
			} else {
				pageNav = navHelper.FindByFilename(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName);
				//assign bogus page name for comp purposes
				if (pageNav == null) {
					pageNav = new SiteNav();
					pageNav.Root_ContentID = Guid.Empty;
					pageNav.FileName = "/##/##/";
					pageNav.TemplateFile = "/##/##/";
				}
			}
			pageNav.SiteID = SiteData.CurrentSiteID;

			return pageNav;
		}

		public static string GetCtrlText(Control ctrl) {
			StringBuilder sb = new StringBuilder();
			StringWriter tw = new StringWriter(sb);
			HtmlTextWriter hw = new HtmlTextWriter(tw);

			ctrl.RenderControl(hw);

			return sb.ToString();
		}

		public static SiteNav IdentifyLinkAsInactive(SiteNav nav) {
			return CMSConfigHelper.IdentifyLinkAsInactive(nav);
		}

	}
}
