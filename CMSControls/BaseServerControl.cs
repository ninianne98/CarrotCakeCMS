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


		protected ContentPageHelper pageHelper = new ContentPageHelper();
		protected SiteData siteHelper = new SiteData();
		protected SiteNavHelper navHelper = new SiteNavHelper();

		protected void SetSiteID() {
			SiteID = SiteData.CurrentSiteID;
		}

		protected bool IsPostBack {
			get {
				return HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"].ToString().ToUpper() != "GET" ? true : false;
			}
		}

		private static Page CachedPage {
			get {
				if (_CachedPage == null)
					_CachedPage = new Page();
				return _CachedPage;
			}
		}
		private static Page _CachedPage;

		public static string GetWebResourceUrl(Type type, string resource) {
			return CachedPage.ClientScript.GetWebResourceUrl(type, resource);
		}

		protected override void OnInit(EventArgs e) {
			SetSiteID();

			base.OnInit(e);
		}

		protected override void Render(HtmlTextWriter writer) {
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
