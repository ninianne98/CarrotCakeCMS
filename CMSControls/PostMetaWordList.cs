using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

	[ToolboxData("<{0}:PostMetaWordList runat=server></{0}:PostMetaWordList>")]
	[AspNetHostingPermissionAttribute(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermissionAttribute(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class PostMetaWordList : BaseServerControl {


		[DefaultValue(false)]
		[Themeable(false)]
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

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string MetaDataTitle {
			get {
				string s = (string)ViewState["MetaDataTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["MetaDataTitle"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string HtmlTagNameOuter {
			get {
				string s = (string)ViewState["HtmlTagNameOuter"];
				return ((s == null) ? "ul" : s);
			}
			set {
				ViewState["HtmlTagNameOuter"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string HtmlTagNameInner {
			get {
				string s = (string)ViewState["HtmlTagNameInner"];
				return ((s == null) ? "li" : s);
			}
			set {
				ViewState["HtmlTagNameInner"] = value;
			}
		}


		private int _TakeTop = 20;
		public int TakeTop {
			get { return _TakeTop; }
			set { _TakeTop = value; }
		}

		public enum MetaDataType {
			Tag,
			Category,
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Guid AssignedRootContentID {
			get {
				Guid s = Guid.Empty;
				if (ViewState["AssignedRootContentID"] != null) {
					try { s = new Guid(ViewState["AssignedRootContentID"].ToString()); } catch { }
				}
				return s;
			}
			set {
				ViewState["AssignedRootContentID"] = value.ToString();
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSimpleDisplayMode {
			get {
				bool s = false;
				if (ViewState["IsSimpleDisplayMode"] != null) {
					try { s = (bool)ViewState["IsSimpleDisplayMode"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsSimpleDisplayMode"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstContentType")]
		public MetaDataType ContentType {
			get {
				String s = (String)ViewState["ContentType"];
				MetaDataType c = MetaDataType.Category;
				if (!string.IsNullOrEmpty(s)) {
					c = (MetaDataType)Enum.Parse(typeof(MetaDataType), s, true);
				}
				return c;
			}

			set {
				ViewState["ContentType"] = value.ToString();
			}
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstContentType {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();
				_dict.Add("Category", "Categories");
				_dict.Add("Tag", "Tags");

				return _dict;
			}
		}

		protected override void OnDataBinding(EventArgs e) {

			RepeaterItem container = (RepeaterItem)this.NamingContainer;

			Guid pageID = new Guid(DataBinder.Eval(container, "DataItem.Root_ContentID").ToString());

			this.AssignedRootContentID = pageID;

			base.OnDataBinding(e);
		}

		protected List<IContentMetaInfo> GetMetaInfo() {
			if (AssignedRootContentID == Guid.Empty) {
				if (ContentType == MetaDataType.Tag) {
					return navHelper.GetTagListForPost(SiteData.CurrentSiteID, TakeTop, SiteData.CurrentScriptName);
				} else {
					return navHelper.GetCategoryListForPost(SiteData.CurrentSiteID, TakeTop, SiteData.CurrentScriptName);
				}
			} else {
				if (ContentType == MetaDataType.Tag) {
					return navHelper.GetTagListForPost(SiteData.CurrentSiteID, TakeTop, AssignedRootContentID);
				} else {
					return navHelper.GetCategoryListForPost(SiteData.CurrentSiteID, TakeTop, AssignedRootContentID);
				}
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<IContentMetaInfo> lstNav = GetMetaInfo();

			output.Indent = indent + 3;
			output.WriteLine();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}
			string sOuter = HtmlTagNameOuter;
			string sInner = HtmlTagNameInner;

			if (IsSimpleDisplayMode) {
				sOuter = "div";
				sInner = "span";
			}

			output.WriteLine("<" + sOuter + sCSS + " id=\"" + this.ClientID + "\"> ");
			output.Indent++;

			if (!string.IsNullOrEmpty(MetaDataTitle) && lstNav.Count > 0) {
				output.WriteLine("<" + sInner + " class=\"meta-caption\">" + MetaDataTitle + "  </" + sInner + "> ");
			}

			foreach (IContentMetaInfo c in lstNav) {
				output.WriteLine("<" + sInner + " class=\"meta-used-" + c.MetaInfoCount.ToString() + "\"><a href=\"" + c.MetaInfoURL + "\">" + c.MetaInfoText + "</a></" + sInner + ">  ");
			}

			output.Indent--;
			output.WriteLine("</" + sOuter + "> ");

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {

			try {

				if (PublicParmValues.Count > 0) {

					TakeTop = int.Parse(GetParmValue("TakeTop", "20"));

					IsSimpleDisplayMode = Convert.ToBoolean(GetParmValue("IsSimpleDisplayMode", "false"));

					CssClass = GetParmValue("CssClass", "");

					HtmlTagNameOuter = GetParmValue("HtmlTagNameOuter", "ul");
					HtmlTagNameInner = GetParmValue("HtmlTagNameInner", "li");

					MetaDataTitle = GetParmValue("MetaDataTitle", "");

					ContentType = (MetaDataType)Enum.Parse(typeof(MetaDataType), GetParmValue("ContentType", "MetaDataType.Category"), true);

				}
			} catch (Exception ex) {
			}


			base.OnPreRender(e);
		}


	}
}
