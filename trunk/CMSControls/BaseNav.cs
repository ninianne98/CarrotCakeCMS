using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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

	public abstract class BaseNav : BaseServerControl {

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

		public List<SiteNav> NavigationData { get; set; }

		//important to override so as to do any assignment of your data in your implementing class
		protected virtual void LoadData() {
			this.NavigationData = new List<SiteNav>();
		}

		protected virtual void TweakData() {
			if (this.NavigationData != null) {
				this.NavigationData.RemoveAll(x => x.ShowInSiteNav == false);
				this.NavigationData.ToList().ForEach(q => IdentifyLinkAsInactive(q));
			}
		}

		protected virtual void WriteListPrefix(HtmlTextWriter output) {

			if (this.NavigationData != null && this.NavigationData.Count > 0) {

				string sCSS = "";

				if (!string.IsNullOrEmpty(this.CssClass)) {
					sCSS = " class=\"" + this.CssClass + "\" ";
				}
				output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
			}
		}

		protected virtual void WriteListSuffix(HtmlTextWriter output) {
			if (this.NavigationData != null && this.NavigationData.Count > 0) {
				output.WriteLine("</ul>");
			}
		}

		protected override void OnInit(EventArgs e) {
			this.Controls.Clear();

			base.OnInit(e);

			LoadData();

			TweakData();
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<SiteNav> lstNav = this.NavigationData;

			output.Indent = indent + 3;
			output.WriteLine();

			WriteListPrefix(output);

			if (lstNav != null && lstNav.Count > 0) {
				output.Indent++;

				foreach (SiteNav c in lstNav) {
					if (c.NavOrder >= 0) {
						output.WriteLine("<li class=\"child-nav\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
					} else {
						output.WriteLine("<li class=\"parent-nav\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
					}
				}
				output.Indent--;

			} else {
				output.WriteLine("<span style=\"display: none;\" id=\"" + this.ClientID + "\"></span>");
			}

			WriteListSuffix(output);

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {

			base.OnPreRender(e);

			try {

				if (PublicParmValues.Count > 0) {

					this.CssClass = GetParmValue("CssClass", "");

					this.EnableViewState = Convert.ToBoolean(GetParmValue("EnableViewState", "false"));

				}
			} catch (Exception ex) {
			}
		}

	}
}
