using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
	public class PostMetaWordList : BaseServerControl {

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
		[DefaultValue("")]
		public string MetaDataTitle {
			get {
				string s = (string)ViewState["MetaDataTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["MetaDataTitle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("ul")]
		public string HtmlTagNameOuter {
			get {
				string s = (string)ViewState["HtmlTagNameOuter"];
				return ((s == null) ? "ul" : s);
			}
			set {
				ViewState["HtmlTagNameOuter"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("li")]
		public string HtmlTagNameInner {
			get {
				string s = (string)ViewState["HtmlTagNameInner"];
				return ((s == null) ? "li" : s);
			}
			set {
				ViewState["HtmlTagNameInner"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(20)]
		public int TakeTop {
			get {
				String s = (String)ViewState["TakeTop"];
				return ((s == null) ? 20 : int.Parse(s));
			}
			set {
				ViewState["TakeTop"] = value.ToString();
			}
		}

		public enum MetaDataType {
			Tag,
			Category,
		}

		[Category("Appearance")]
		[DefaultValue(null)]
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

		[Category("Appearance")]
		[DefaultValue(false)]
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

		[Category("Appearance")]
		[DefaultValue("Category")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstContentType")]
		public MetaDataType ContentType {
			get {
				String s = (String)ViewState["ContentType"];
				MetaDataType c = MetaDataType.Category;
				if (!String.IsNullOrEmpty(s)) {
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
				_dict.Add(MetaDataType.Category.ToString(), "Categories");
				_dict.Add(MetaDataType.Tag.ToString(), "Tags");

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
			int takeTop = this.TakeTop;
			if (this.TakeTop < 0) {
				takeTop = 300000;
			}
			if (this.AssignedRootContentID == Guid.Empty) {
				if (this.ContentType == MetaDataType.Tag) {
					return navHelper.GetTagListForPost(SiteData.CurrentSiteID, takeTop, SiteData.CurrentScriptName);
				} else {
					return navHelper.GetCategoryListForPost(SiteData.CurrentSiteID, takeTop, SiteData.CurrentScriptName);
				}
			} else {
				if (this.ContentType == MetaDataType.Tag) {
					return navHelper.GetTagListForPost(SiteData.CurrentSiteID, takeTop, AssignedRootContentID);
				} else {
					return navHelper.GetCategoryListForPost(SiteData.CurrentSiteID, takeTop, AssignedRootContentID);
				}
			}
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<IContentMetaInfo> lstNav = GetMetaInfo();

			output.Indent = indent + 3;
			output.WriteLine();

			string sCSS = String.Empty;
			if (!String.IsNullOrEmpty(this.CssClass)) {
				sCSS = " class=\"" + this.CssClass + "\" ";
			}
			string sOuter = this.HtmlTagNameOuter;
			string sInner = this.HtmlTagNameInner;

			if (this.IsSimpleDisplayMode) {
				sOuter = "div";
				sInner = "span";
			}

			int blogCount = navHelper.GetSitePageCount(SiteData.CurrentSiteID, ContentPageType.PageType.BlogEntry);

			output.WriteLine("<" + sOuter + sCSS + " id=\"" + this.ClientID + "\"> ");
			output.Indent++;

			if (!String.IsNullOrEmpty(this.MetaDataTitle) && lstNav.Any()) {
				output.WriteLine("<" + sInner + " class=\"meta-caption\">" + this.MetaDataTitle + "  </" + sInner + "> ");
			}

			foreach (IContentMetaInfo c in lstNav) {
				double percUsed = Math.Ceiling(100 * (float)c.MetaInfoCount / (((float)blogCount + 0.000001)));
				percUsed = Math.Round(percUsed / 5) * 5;
				if (percUsed < 1 && c.MetaInfoCount > 0) {
					percUsed = 1;
				}
				if (c.MetaInfoCount <= 0) {
					percUsed = 0;
				}
				if (percUsed > 100) {
					percUsed = 100;
				}

				output.WriteLine("<" + sInner + " class=\"meta-item meta-perc-used-" + percUsed.ToString() + " meta-used-" + c.MetaInfoCount.ToString() + "\"><a href=\"" + c.MetaInfoURL + "\">" + c.MetaInfoText + "</a></" + sInner + ">  ");
			}

			output.Indent--;
			output.WriteLine("</" + sOuter + "> ");

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {
			try {
				if (this.PublicParmValues.Any()) {
					this.TakeTop = int.Parse(GetParmValue("TakeTop", "20"));

					this.IsSimpleDisplayMode = Convert.ToBoolean(GetParmValue("IsSimpleDisplayMode", "false"));

					this.CssClass = GetParmValue("CssClass", "");

					this.HtmlTagNameOuter = GetParmValue("HtmlTagNameOuter", "ul");
					this.HtmlTagNameInner = GetParmValue("HtmlTagNameInner", "li");

					this.MetaDataTitle = GetParmValue("MetaDataTitle", "");

					this.ContentType = (MetaDataType)Enum.Parse(typeof(MetaDataType), GetParmValue("ContentType", "MetaDataType.Category"), true);
				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}
	}
}