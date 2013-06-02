using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
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

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class PagedDataSummaryTitleOption {
		private PageViewType.ViewType _key = PageViewType.ViewType.SinglePage;
		private string _label = "";
		private string _format = "";

		public PagedDataSummaryTitleOption()
			: this(PageViewType.ViewType.SinglePage, String.Empty, String.Empty) {
		}

		public PagedDataSummaryTitleOption(PageViewType.ViewType key, string labelText) {
			_key = key;
			_label = labelText;
			_format = "";
		}

		public PagedDataSummaryTitleOption(PageViewType.ViewType key, string labelText, string formatText) {
			_key = key;
			_label = labelText;
			_format = formatText;
		}


		[
		Category("Behavior"),
		DefaultValue(PageViewType.ViewType.SinglePage),
		Description("KeyValue of PagedDataSummaryTitleOptions"),
		NotifyParentProperty(true)
		]
		public PageViewType.ViewType KeyValue {
			get {
				return _key;
			}
			set {
				_key = value;
			}
		}

		[
		Category("Behavior"),
		DefaultValue(""),
		Description("LabelText of PagedDataSummaryTitleOptions"),
		NotifyParentProperty(true)
		]
		public String LabelText {
			get {
				return _label;
			}
			set {
				_label = value;
			}
		}

		[
		Category("Behavior"),
		DefaultValue(""),
		Description("FormatText of PagedDataSummaryTitleOptions"),
		NotifyParentProperty(true)
		]
		public String FormatText {
			get {
				return _format;
			}
			set {
				_format = value;
			}
		}
	}

	//==================
	public class PagedDataSummaryTitleOptionEditor : CollectionEditor {
		public PagedDataSummaryTitleOptionEditor(Type type)
			: base(type) {
		}

		protected override bool CanSelectMultipleInstances() {
			return false;
		}

		protected override Type CreateCollectionItemType() {
			return typeof(PagedDataSummaryTitleOption);
		}
	}



	//==========================

	public class PagedDataNextPrevLinkPair {

		public PagedDataNextPrevLinkWrapper LinkWrapper { get; set; }

		public PagedDataNextPrevLink PageLink { get; set; }

	}


	//==========================
	[ToolboxData("<{0}:PagedDataNextPrevLinkWrapper runat=server></{0}:PagedDataNextPrevLinkWrapper>")]
	public class PagedDataNextPrevLinkWrapper : PlaceHolder {

		public enum PagedDataDirection {
			Unknown,
			Previous,
			Next,
			First,
			Last,
		}

	}


	//==========================
	[ToolboxData("<{0}:PagedDataNextPrevLink runat=server></{0}:PagedDataNextPrevLink>")]
	public class PagedDataNextPrevLink : HyperLink {

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool UseDefaultText {
			get {
				bool s = true;
				if (ViewState["UseDefaultText"] != null) {
					try { s = (bool)ViewState["UseDefaultText"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["UseDefaultText"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("NavDirection")]
		public PagedDataNextPrevLinkWrapper.PagedDataDirection NavDirection {
			get {
				string s = (string)ViewState["NavDirection"];
				PagedDataNextPrevLinkWrapper.PagedDataDirection c = PagedDataNextPrevLinkWrapper.PagedDataDirection.Unknown;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (PagedDataNextPrevLinkWrapper.PagedDataDirection)Enum.Parse(typeof(PagedDataNextPrevLinkWrapper.PagedDataDirection), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["NavDirection"] = value.ToString();
			}
		}

		public void SetText() {
			if (this.UseDefaultText) {
				this.Text = this.NavDirection.ToString();
			}
		}
		public void SetText(string linkText) {
			this.Text = linkText;
		}

		protected override void OnPreRender(EventArgs e) {

			this.SetText();

			if (!this.UseDefaultText) {
				ControlUtilities cu = new ControlUtilities(this.Page);
				PagedDataNextPrevText txt = (PagedDataNextPrevText)cu.FindControl(typeof(PagedDataNextPrevText), this);
				if (txt != null) {
					txt.NavDirection = this.NavDirection;
				}
			}

			base.OnPreRender(e);
		}

	}


	//==========================
	[ToolboxData("<{0}:PagedDataNextPrevText runat=server></{0}:PagedDataNextPrevText>")]
	public class PagedDataNextPrevText : Literal {

		[Category("Appearance")]
		[DefaultValue("NavDirection")]
		public PagedDataNextPrevLinkWrapper.PagedDataDirection NavDirection {
			get {
				string s = (string)ViewState["NavDirection"];
				PagedDataNextPrevLinkWrapper.PagedDataDirection c = PagedDataNextPrevLinkWrapper.PagedDataDirection.Unknown;
				if (!string.IsNullOrEmpty(s)) {
					try {
						c = (PagedDataNextPrevLinkWrapper.PagedDataDirection)Enum.Parse(typeof(PagedDataNextPrevLinkWrapper.PagedDataDirection), s, true);
					} catch (Exception ex) { }
				}
				return c;
			}
			set {
				ViewState["NavDirection"] = value.ToString();
			}
		}

		public void SetText() {
			if (string.IsNullOrEmpty(this.Text)) {
				this.Text = this.NavDirection.ToString();
			}
		}
		public void SetText(string linkText) {
			this.Text = linkText;
		}

		protected override void OnPreRender(EventArgs e) {

			if (string.IsNullOrEmpty(this.Text) && this.NavDirection != PagedDataNextPrevLinkWrapper.PagedDataDirection.Unknown) {
				this.SetText();
			}

			base.OnPreRender(e);
		}
	}


}