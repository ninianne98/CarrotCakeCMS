﻿using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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

namespace Carrotware.CMS.UI.Controls {

	public abstract class BaseServerControl : WidgetParmDataWebControl {
		protected ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper();

		protected void SetSiteID() {
			try {
				SiteID = SiteData.CurrentSiteID;
			} catch { }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPostBack {
			get {
				string sReq = "GET";
				try { sReq = HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"].ToString().ToUpperInvariant(); } catch { }
				return sReq != "GET" ? true : false;
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
					if (!String.IsNullOrEmpty(sTmp)) {
						this.CssClass = sTmp;
					}
				}
			} catch (Exception ex) {
			}
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

			return nav.FileName.ToLowerInvariant();
		}

		protected bool AreFilenamesSame(string sParm1, string sParm2) {
			if (sParm1 == null || sParm2 == null) {
				return false;
			}

			return (sParm1.ToLowerInvariant() == sParm2.ToLowerInvariant()) ? true : false;
		}

		protected List<SiteNav> GetPageNavTree() {
			return navHelper.GetPageCrumbNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);
		}

		protected SiteNav GetParentPage() {
			SiteNav pageNav = navHelper.GetParentPageNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName);

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
				pageNav = navHelper.FindByFilename(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName);
				//assign bogus page name for comp purposes
				if (pageNav == null) {
					pageNav = new SiteNav();
					pageNav.Root_ContentID = Guid.Empty;
					pageNav.FileName = "javascript:void(0);";
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

		public static SiteNav FixNavLinkText(SiteNav nav) {
			return CMSConfigHelper.FixNavLinkText(nav);
		}
	}
}