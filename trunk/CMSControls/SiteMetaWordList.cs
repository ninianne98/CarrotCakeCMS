using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
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

	[ToolboxData("<{0}:SiteMetaWordList runat=server></{0}:SiteMetaWordList>")]
	public class SiteMetaWordList : BaseServerControl, IHeadedList {

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

		public enum MetaDataType {
			Tag,
			Category,
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
				_dict.Add("Category", "Categories");
				_dict.Add("Tag", "Tags");
				_dict.Add("DateMonth", "Dates");

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

			int iTakeTop = TakeTop;
			if (TakeTop < 0) {
				iTakeTop = 100000;
			}

			switch (ContentType) {
				case MetaDataType.Tag:
					lstNav = navHelper.GetTagList(SiteData.CurrentSiteID, iTakeTop);
					break;
				case MetaDataType.Category:
					lstNav = navHelper.GetCategoryList(SiteData.CurrentSiteID, iTakeTop);
					break;
				case MetaDataType.DateMonth:
					lstNav = navHelper.GetMonthBlogUpdateList(SiteData.CurrentSiteID, iTakeTop, !SecurityData.IsAuthEditor);
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

			if (lstNav != null && lstNav.Count > 0 && !string.IsNullOrEmpty(MetaDataTitle)) {
				output.WriteLine("<" + this.HeadWrapTag.ToString().ToLower() + ">" + this.MetaDataTitle + "</" + this.HeadWrapTag.ToString().ToLower() + ">\r\n");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			string sItemCSS = "";
			if (!string.IsNullOrEmpty(CSSItem)) {
				sItemCSS = string.Format(" {0} ", CSSItem);
			}

			output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\"> ");
			output.Indent++;

			foreach (IContentMetaInfo c in lstNav) {
				string sText = c.MetaInfoText;
				string sCount = "0";

				if (SecurityData.IsAuthEditor) {
					sCount = c.MetaInfoCount.ToString();
				} else {
					sCount = c.MetaPublicInfoCount.ToString();
				}
				if (ShowUseCount) {
					sText = string.Format("{0}  ({1})", c.MetaInfoText, sCount);
				}

				if (SiteData.IsFilenameCurrentPage(c.MetaInfoURL)) {
					output.WriteLine("<li class=\"meta-used-" + sCount + sItemCSS + " selected\"><a href=\"" + c.MetaInfoURL + "\">" + sText + "</a></li> ");
				} else {
					output.WriteLine("<li class=\"meta-used-" + sCount + sItemCSS + "\"><a href=\"" + c.MetaInfoURL + "\">" + sText + "</a></li> ");
				}
			}

			output.Indent--;
			output.WriteLine("</ul> ");

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {

			try {

				if (PublicParmValues.Count > 0) {

					TakeTop = int.Parse(GetParmValue("TakeTop", "10"));

					CssClass = GetParmValue("CssClass", "");

					MetaDataTitle = GetParmValue("MetaDataTitle", "");

					ShowUseCount = Convert.ToBoolean(GetParmValue("ShowUseCount", "false"));

					ContentType = (MetaDataType)Enum.Parse(typeof(MetaDataType), GetParmValue("ContentType", "MetaDataType.Category"), true);

				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}

	}
}
