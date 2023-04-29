using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
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

	public abstract class BaseNavCommon : BaseServerControl {

		public BaseNavCommon() {
			this.EnableViewState = false;
			this.RenderHTMLWithID = false;
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

		public virtual string HtmlClientID {
			get {
				if (this.RenderHTMLWithID) {
					return this.ID;
				} else {
					return this.ClientID;
				}
			}
		}

		[Browsable(false)]
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

		private bool _stripNotInSiteMap = false;

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
			this.NavigationData = CMSConfigHelper.TweakData(this.NavigationData, this.StripNotInSiteMap, this.StripNotInSiteNav);
		}

		protected HtmlTag _topTag = new HtmlTag("ul");

		protected virtual void WriteListPrefix(HtmlTextWriter output) {
			_topTag = new HtmlTag("ul");

			if (this.NavigationData != null && this.NavigationData.Any()) {
				_topTag.SetAttribute("id", this.HtmlClientID);
				_topTag.MergeAttribute("class", this.CssClass);

				output.WriteLine(_topTag.OpenTag());
			}
		}

		protected virtual void WriteListSuffix(HtmlTextWriter output) {
			if (this.NavigationData != null && this.NavigationData.Any()) {
				output.WriteLine(_topTag.CloseTag());
			}
		}

		protected override void OnInit(EventArgs e) {
			this.Controls.Clear();
			this.NavigationData = new List<SiteNav>();
			base.OnInit(e);
		}

		protected virtual void LoadAndTweakData() {
			LoadData();
			TweakData();
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			try {
				if (PublicParmValues.Any()) {
					this.CssClass = GetParmValue("CssClass", "");

					this.EnableViewState = Convert.ToBoolean(GetParmValue("EnableViewState", "false"));

					this.RenderHTMLWithID = Convert.ToBoolean(GetParmValue("RenderHTMLWithID", "false"));
				}
			} catch (Exception ex) {
			}
		}
	}
}