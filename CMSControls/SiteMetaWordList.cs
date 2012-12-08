using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:SiteMetaWordList runat=server></{0}:SiteMetaWordList>")]
	public class SiteMetaWordList : BaseServerControl {


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
		public bool ShowUseCount {
			get {
				String s = (String)ViewState["ShowUseCount"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}
			set {
				ViewState["ShowUseCount"] = value.ToString();
			}
		}

		private int _TakeTop = 10;
		public int TakeTop {
			get { return _TakeTop; }
			set { _TakeTop = value; }
		}

		public enum MetaDataType {
			Tag,
			Category,
			DateMonth,
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
				_dict.Add("DateMonth", "Dates");

				return _dict;
			}
		}

		protected List<IContentMetaInfo> GetMetaInfo() {
			List<IContentMetaInfo> lst = null;

			switch (ContentType) {
				case MetaDataType.Tag:
					lst = navHelper.GetTagList(SiteData.CurrentSiteID, TakeTop);
					break;
				case MetaDataType.Category:
					lst = navHelper.GetCategoryList(SiteData.CurrentSiteID, TakeTop);
					break;
				case MetaDataType.DateMonth:
					lst = navHelper.GetMonthBlogUpdateList(SiteData.CurrentSiteID, TakeTop);
					break;
				default:
					break;
			}

			return lst;
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<IContentMetaInfo> lstNav = GetMetaInfo();

			output.Indent = indent + 3;
			output.WriteLine();

			if (!string.IsNullOrEmpty(MetaDataTitle)) {
				output.WriteLine("<h2>" + MetaDataTitle + "</h2> ");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\"> ");
			output.Indent++;

			foreach (IContentMetaInfo c in lstNav) {
				string sText = c.MetaInfoText;
				if (ShowUseCount) {
					sText = c.MetaInfoText + "  (" + c.MetaInfoCount.ToString() + ") ";
				}

				if (SiteData.IsFilenameCurrentPage(c.MetaInfoURL)) {
					output.WriteLine("<li class=\"meta-used-" + c.MetaInfoCount.ToString() + " selected\"><a href=\"" + c.MetaInfoURL + "\">" + sText + "</a></li> ");
				} else {
					output.WriteLine("<li class=\"meta-used-" + c.MetaInfoCount.ToString() + "\"><a href=\"" + c.MetaInfoURL + "\">" + sText + "</a></li> ");
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
