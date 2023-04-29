using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

	[ToolboxData("<{0}:TopLevelNavigation runat=server></{0}:TopLevelNavigation>")]
	public class TopLevelNavigation : BaseServerControl, IWidgetLimitedProperties {

		[Category("Appearance")]
		[DefaultValue("selected")]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}

		public virtual List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("CssClass");
				lst.Add("CSSSelected");
				lst.Add("RenderHTMLWithID");

				return lst.Distinct().ToList();
			}
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

					sTmp = GetParmValue("CSSSelected", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.CSSSelected = sTmp;
					}

					sTmp = GetParmValue("RenderHTMLWithID", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.RenderHTMLWithID = Convert.ToBoolean(sTmp);
					}
				}
			} catch (Exception ex) {
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool RenderHTMLWithID {
			get {
				String s = (String)ViewState["RenderHTMLWithID"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["RenderHTMLWithID"] = value.ToString();
			}
		}

		public string HtmlClientID {
			get {
				if (RenderHTMLWithID) {
					return this.ID;
				} else {
					return this.ClientID;
				}
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			SiteNav pageNav = GetParentPage();
			string sParent = pageNav.FileName.ToLowerInvariant();

			List<SiteNav> lstNav = navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);

			lstNav = CMSConfigHelper.TweakData(lstNav, false, true);

			output.Indent = indent + 3;
			output.WriteLine();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.WriteLine("<ul" + sCSS + " id=\"" + this.HtmlClientID + "\">");
			output.Indent++;

			foreach (SiteNav c in lstNav) {
				if (SiteData.IsFilenameCurrentPage(c.FileName) || AreFilenamesSame(c.FileName, sParent)) {
					output.WriteLine("<li class=\"" + CSSSelected + "\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				} else {
					output.WriteLine("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				}
			}

			output.Indent--;
			output.WriteLine("</ul>");

			output.Indent = indent;
		}
	}
}