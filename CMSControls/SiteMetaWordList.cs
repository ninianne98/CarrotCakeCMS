﻿using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

	[ToolboxData("<{0}:SiteMetaWordList runat=server></{0}:SiteMetaWordList>")]
	public class SiteMetaWordList : BaseServerControl, IHeadedList, IWidgetLimitedProperties {

		public SiteMetaWordList()
			: base() {
			this.ItemCount = -1;
			this.MetaDataTitle = string.Empty;
			this.CSSItem = string.Empty;
			this.ShowUseCount = false;
			this.TakeTop = 10;
			this.ContentType = MetaDataType.Category;
		}

		public int ItemCount { get; set; }

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
		[DefaultValue("H2")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstTagType")]
		public TagType HeadWrapTag {
			get {
				String s = (String)ViewState["HeadWrapTag"];
				TagType c = TagType.H2;
				if (!string.IsNullOrEmpty(s)) {
					c = (TagType)Enum.Parse(typeof(TagType), s, true);
				}
				return c;
			}

			set {
				ViewState["HeadWrapTag"] = value.ToString();
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
		[DefaultValue("")]
		public string CSSItem {
			get {
				string s = (string)ViewState["CSSItem"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSItem"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool ShowUseCount {
			get {
				String s = (String)ViewState["ShowUseCount"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}
			set {
				ViewState["ShowUseCount"] = value.ToString();
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
		[DefaultValue(10)]
		public int TakeTop {
			get {
				String s = (String)ViewState["TakeTop"];
				return ((s == null) ? 10 : int.Parse(s));
			}
			set {
				ViewState["TakeTop"] = value.ToString();
			}
		}

		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("MetaDataTitle");
				lst.Add("HeadWrapTag");
				lst.Add("CSSItem");
				lst.Add("ShowUseCount");
				lst.Add("EnableViewState");
				lst.Add("TakeTop");
				lst.Add("ContentType");
				lst.Add("ShowNonZeroCountOnly");

				return lst.Distinct().ToList();
			}
		}

		public enum MetaDataType {

			[Description("Tags")]
			Tag,

			[Description("Categories")]
			Category,

			[Description("Dates")]
			DateMonth,
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

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool ShowNonZeroCountOnly {
			get {
				bool s = true;
				if (ViewState["ShowNonZeroCountOnly"] != null) {
					try { s = (bool)ViewState["ShowNonZeroCountOnly"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["ShowNonZeroCountOnly"] = value;
			}
		}

		protected List<IContentMetaInfo> GetMetaInfo() {
			List<IContentMetaInfo> lstNav = new List<IContentMetaInfo>();

			int takeTop = this.TakeTop;
			if (this.TakeTop < 0) {
				takeTop = 300000;
			}

			switch (ContentType) {
				case MetaDataType.Tag:
					lstNav = navHelper.GetTagList(SiteData.CurrentSiteID, takeTop);
					break;

				case MetaDataType.Category:
					lstNav = navHelper.GetCategoryList(SiteData.CurrentSiteID, takeTop);
					break;

				case MetaDataType.DateMonth:
					lstNav = navHelper.GetMonthBlogUpdateList(SiteData.CurrentSiteID, takeTop, !SecurityData.IsAuthEditor);
					break;

				default:
					break;
			}

			if (lstNav != null) {
				if (SecurityData.IsAuthEditor) {
					lstNav.RemoveAll(x => x.MetaInfoCount < 1 && this.ShowNonZeroCountOnly);
					lstNav = lstNav.OrderByDescending(x => x.MetaInfoCount).ToList();
				} else {
					lstNav.RemoveAll(x => x.MetaPublicInfoCount < 1 && this.ShowNonZeroCountOnly);
					lstNav = lstNav.OrderByDescending(x => x.MetaPublicInfoCount).ToList();
				}

				if (ContentType == MetaDataType.DateMonth) {
					lstNav = lstNav.OrderByDescending(x => x.MetaDataDate).ToList();
				}

				this.ItemCount = lstNav.Count;
			}

			return lstNav;
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<IContentMetaInfo> lstNav = GetMetaInfo();

			output.Indent = indent + 3;
			output.WriteLine();

			var outerItem = new HtmlTag("ul");
			outerItem.MergeAttribute("id", this.ClientID);
			outerItem.MergeAttribute("class", this.CssClass);

			if (lstNav != null && lstNav.Any() && !string.IsNullOrEmpty(this.MetaDataTitle)) {
				var head = new HtmlTag(this.HeadWrapTag.ToString());
				head.MergeAttribute("class", "meta-caption");
				head.InnerHtml = HttpUtility.HtmlEncode(this.MetaDataTitle);
				output.WriteLine(head.RenderTag());
			}

			output.WriteLine(outerItem.OpenTag());
			output.Indent++;

			int contentCount = 0;

			if (this.ContentType == MetaDataType.DateMonth) {
				contentCount = navHelper.GetSitePageCount(SiteData.CurrentSiteID, ContentPageType.PageType.ContentEntry)
								+ navHelper.GetSitePageCount(SiteData.CurrentSiteID, ContentPageType.PageType.BlogEntry);
			} else {
				contentCount = navHelper.GetSitePageCount(SiteData.CurrentSiteID, ContentPageType.PageType.BlogEntry);
			}

			foreach (IContentMetaInfo c in lstNav) {
				var childItem = new HtmlTag("li");
				var childLink = new HtmlTag("a");

				int metaCount = 0;
				if (SecurityData.IsAuthEditor) {
					metaCount = c.MetaInfoCount;
				} else {
					metaCount = c.MetaPublicInfoCount;
				}

				childLink.Uri = c.MetaInfoURL;
				childLink.InnerHtml = HttpUtility.HtmlEncode(c.MetaInfoText) + (this.ShowUseCount ? string.Format("  ({0})", metaCount) : "");
				childItem.InnerHtml = childLink.RenderTag();

				double percUsed = Math.Ceiling(100 * (float)c.MetaInfoCount / (((float)contentCount + 0.000001)));
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

				childItem.MergeAttribute("class", this.CSSItem);
				childItem.MergeAttribute("class", string.Format("meta-item meta-perc-used-{0} meta-used-{1}", percUsed, metaCount));

				output.WriteLine(childItem.RenderTag());

				if (SiteData.IsFilenameCurrentPage(c.MetaInfoURL)) {
					childItem.MergeAttribute("class", "selected");
				}

				output.WriteLine(childItem.RenderTag());
			}

			output.Indent--;
			output.WriteLine(outerItem.CloseTag());

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {
			try {
				if (this.PublicParmValues.Any()) {
					this.TakeTop = int.Parse(GetParmValue("TakeTop", "10"));

					this.CssClass = GetParmValue("CssClass", string.Empty);

					this.MetaDataTitle = GetParmValue("MetaDataTitle", string.Empty);

					this.ShowUseCount = Convert.ToBoolean(GetParmValue("ShowUseCount", "false"));

					this.ContentType = (MetaDataType)Enum.Parse(typeof(MetaDataType), GetParmValue("ContentType", MetaDataType.Category.ToString()), true);

					this.HeadWrapTag = (TagType)Enum.Parse(typeof(TagType), GetParmValue("HeadWrapTag", TagType.H2.ToString()), true);
				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}
	}
}