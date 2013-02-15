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

	public abstract class BaseNavCommon : BaseServerControl {

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

		private bool _stripNotInSiteNav = true;
		protected virtual bool StripNotInSiteNav {
			get {
				return _stripNotInSiteNav;
			}
			set {
				_stripNotInSiteNav = value;
			}
		}
		private bool _stripNotInSiteMap = true;
		protected virtual bool StripNotInSiteMap {
			get {
				return _stripNotInSiteMap;
			}
			set {
				_stripNotInSiteMap = value;
			}
		}

		//important to override so as to do any assignment of your data in your implementing class
		protected virtual void LoadData() {
			this.NavigationData = new List<SiteNav>();
		}

		protected virtual void TweakData() {
			if (this.NavigationData != null) {
				if (this.StripNotInSiteNav) {
					this.NavigationData.RemoveAll(x => x.ShowInSiteNav == false);
				}
				if (this.StripNotInSiteMap) {
					this.NavigationData.RemoveAll(x => x.ShowInSiteMap == false);
				}
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
