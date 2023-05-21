using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

	[ToolboxData("<{0}:PostMetaWordList runat=server></{0}:PostMetaWordList>")]
	public class PostMetaWordList : BaseServerControl, IWidgetLimitedProperties {

		public PostMetaWordList()
			: base() {
			this.ContentType = MetaDataType.Category;
			this.HtmlTagNameInner = TagType.LI;
			this.HtmlTagNameOuter = TagType.UL;
			this.MetaDataTitle = string.Empty;
			this.TakeTop = 20;
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
		[DefaultValue("UL")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstTagType")]
		public TagType HtmlTagNameOuter {
			get {
				String s = (String)ViewState["HtmlTagNameOuter"];
				TagType c = TagType.UL;
				if (!string.IsNullOrEmpty(s)) {
					c = (TagType)Enum.Parse(typeof(TagType), s, true);
				}
				return c;
			}
			set {
				ViewState["HtmlTagNameOuter"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue("LI")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstTagType")]
		public TagType HtmlTagNameInner {
			get {
				String s = (String)ViewState["HtmlTagNameInner"];
				TagType c = TagType.LI;
				if (!string.IsNullOrEmpty(s)) {
					c = (TagType)Enum.Parse(typeof(TagType), s, true);
				}
				return c;
			}
			set {
				ViewState["HtmlTagNameInner"] = value.ToString();
			}
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstTagType {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict = EnumHelper.ToList<TagType>().OrderBy(x => x.Text).ToDictionary(k => k.Text, v => v.Description);

				return _dict;
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

		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("MetaDataTitle");
				lst.Add("HtmlTagNameInner");
				lst.Add("HtmlTagNameOuter");
				lst.Add("TakeTop");
				lst.Add("ShowUseCount");
				lst.Add("EnableViewState");
				lst.Add("ContentType");
				lst.Add("IsSimpleDisplayMode");

				return lst.Distinct().ToList();
			}
		}

		public enum MetaDataType {

			[Description("Tags")]
			Tag,

			[Description("Categories")]
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

				_dict = EnumHelper.ToList<MetaDataType>().OrderBy(x => x.Text).ToDictionary(k => k.Text, v => v.Description);

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

			string sOuter = this.HtmlTagNameOuter.ToString();
			string sInner = this.HtmlTagNameInner.ToString();

			if (this.IsSimpleDisplayMode) {
				sOuter = "div";
				sInner = "span";
			}

			List<IContentMetaInfo> lstNav = GetMetaInfo();

			output.Indent = indent + 3;
			output.WriteLine();

			var outerItem = new HtmlTag(sOuter);
			outerItem.SetAttribute("id", this.ClientID);
			outerItem.MergeAttribute("class", this.CssClass);

			output.WriteLine(outerItem.OpenTag());

			int blogCount = navHelper.GetSitePageCount(SiteData.CurrentSiteID, ContentPageType.PageType.BlogEntry);

			output.Indent++;

			if (lstNav != null && lstNav.Any() && !string.IsNullOrEmpty(this.MetaDataTitle)) {
				var head = new HtmlTag(sInner);
				head.MergeAttribute("class", "meta-caption");
				head.InnerHtml = HttpUtility.HtmlEncode(this.MetaDataTitle);
				output.WriteLine(head.RenderTag());
			}

			foreach (IContentMetaInfo c in lstNav) {
				var childItem = new HtmlTag(sInner);
				var childLink = new HtmlTag("a");

				childLink.Uri = c.MetaInfoURL;
				childLink.InnerHtml = HttpUtility.HtmlEncode(c.MetaInfoText);
				childItem.InnerHtml = childLink.RenderTag();

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

				childItem.MergeAttribute("class", string.Format("meta-item meta-perc-used-{0} meta-used-{1}", percUsed, c.MetaInfoCount));

				output.WriteLine(childItem.RenderTag());
			}

			output.Indent--;
			output.WriteLine(outerItem.CloseTag());

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {
			try {
				if (this.PublicParmValues.Any()) {
					this.TakeTop = int.Parse(GetParmValue("TakeTop", "20"));

					this.IsSimpleDisplayMode = Convert.ToBoolean(GetParmValue("IsSimpleDisplayMode", "false"));

					this.CssClass = GetParmValue("CssClass", string.Empty);

					this.MetaDataTitle = GetParmValue("MetaDataTitle", string.Empty);

					this.HtmlTagNameOuter = (TagType)Enum.Parse(typeof(TagType), GetParmValue("HtmlTagNameOuter", TagType.UL.ToString()), true);
					this.HtmlTagNameInner = (TagType)Enum.Parse(typeof(TagType), GetParmValue("HtmlTagNameInner", TagType.LI.ToString()), true);
					this.ContentType = (MetaDataType)Enum.Parse(typeof(MetaDataType), GetParmValue("ContentType", MetaDataType.Category.ToString()), true);
				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}
	}
}