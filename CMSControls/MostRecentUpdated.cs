using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
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

	[ToolboxData("<{0}:MostRecentUpdated runat=server></{0}:MostRecentUpdated>")]
	public class MostRecentUpdated : BaseServerControl, IHeadedList, IWidgetLimitedProperties {

		public int ItemCount { get; set; }

		[Obsolete("This property is obsolete, do not use.")]
		public string UpdateTitle {
			get {
				string s = (string)ViewState["UpdateTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["UpdateTitle"] = value;
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
		[DefaultValue(true)]
		[Localizable(true)]
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

		public enum ListContentType {
			Unknown,
			Blog,
			ContentPage,
			SpecifiedCategories
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstContentType {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();
				_dict.Add("Blog", "Blog");
				_dict.Add("ContentPage", "Content Page");
				_dict.Add("SpecifiedCategories", "Specified Categories");
				return _dict;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstContentType")]
		public ListContentType ContentType {
			get {
				String s = (String)ViewState["ContentType"];
				ListContentType c = ListContentType.ContentPage;
				if (!string.IsNullOrEmpty(s)) {
					c = (ListContentType)Enum.Parse(typeof(ListContentType), s, true);
				}
				return c;
			}

			set {
				ViewState["ContentType"] = value.ToString();
			}
		}

		public bool IncludeParent { get; set; }

		private int _TakeTop = 5;
		public int TakeTop {
			get { return _TakeTop; }
			set { _TakeTop = value; }
		}

		private List<GuidItem> guidList = null;
		[
		Category("Behavior"),
		Description("The GuidItem collection"),
		Browsable(false),
		DefaultValue(null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(GuidItemCollectionEditor), typeof(UITypeEditor)),
		NotifyParentProperty(true),
		TemplateContainer(typeof(GuidItem)),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public List<GuidItem> CategoryGuidList {
			get {
				if (guidList == null) {
					guidList = new List<GuidItem>();
				}
				return guidList;
			}
		}


		private List<Guid> _guids = null;

		[Widget(WidgetAttribute.FieldMode.CheckBoxList, "lstCategories")]
		public List<Guid> SelectedCategories {
			get {
				if (_guids == null) {
					if (CategoryGuidList.Count > 0) {
						_guids = (from n in CategoryGuidList select n.GuidValue).ToList();
					} else {
						_guids = new List<Guid>();
					}
				}
				return _guids;
			}
			set {
				_guids = value;
			}
		}


		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstCategories {
			get {

				Dictionary<string, string> _dict = (from c in SiteData.CurrentSite.GetCategoryList()
													orderby c.CategoryText
													where c.SiteID == SiteData.CurrentSiteID
													select c).ToList().ToDictionary(k => k.ContentCategoryID.ToString(), v => v.CategoryText + " (" + v.CategorySlug + ")");

				return _dict;
			}
		}

		protected List<SiteNav> GetUpdates() {

			switch (ContentType) {
				case ListContentType.Blog:
					return navHelper.GetLatestPosts(SiteData.CurrentSiteID, TakeTop, !SecurityData.IsAuthEditor);
				case ListContentType.ContentPage:
					return navHelper.GetLatest(SiteData.CurrentSiteID, TakeTop, !SecurityData.IsAuthEditor);
				case ListContentType.SpecifiedCategories:
					if (TakeTop > 0) {
						return navHelper.GetFilteredContentByIDPagedList(SiteData.CurrentSite, SelectedCategories, !SecurityData.IsAuthEditor, TakeTop, 0, "GoLiveDate", "DESC");
					} else {
						return navHelper.GetFilteredContentByIDPagedList(SiteData.CurrentSite, SelectedCategories, !SecurityData.IsAuthEditor, 100000, 0, "NavMenuText", "ASC");
					}
			}

			return new List<SiteNav>();
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<SiteNav> lstNav = GetUpdates();
			lstNav.RemoveAll(x => x.ShowInSiteNav == false);
			lstNav.ToList().ForEach(q => IdentifyLinkAsInactive(q));

			if (lstNav != null) {
				this.ItemCount = lstNav.Count;
			}

			output.Indent = indent + 3;
			output.WriteLine();

			if (lstNav != null && lstNav.Count > 0 && !string.IsNullOrEmpty(this.MetaDataTitle)) {
				output.WriteLine("<" + this.HeadWrapTag.ToString().ToLower() + ">" + this.MetaDataTitle + "</" + this.HeadWrapTag.ToString().ToLower() + ">\r\n");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\">");
			output.Indent++;

			foreach (SiteNav c in lstNav) {
				if (SiteData.IsFilenameCurrentPage(c.FileName)) {
					output.WriteLine("<li class=\"selected\"><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				} else {
					output.WriteLine("<li><a href=\"" + c.FileName + "\">" + c.NavMenuText + "</a></li> ");
				}
			}

			output.Indent--;
			output.WriteLine("</ul> ");

			output.Indent = indent;
		}


		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("TakeTop");
				lst.Add("MetaDataTitle");
				lst.Add("CssClass");
				lst.Add("EnableViewState");
				lst.Add("ContentType");
				lst.Add("SelectedCategories");
				return lst;
			}
		}


		protected override void OnPreRender(EventArgs e) {

			try {

				if (PublicParmValues.Count > 0) {

					TakeTop = int.Parse(GetParmValue("TakeTop", "5"));

					MetaDataTitle = GetParmValue("MetaDataTitle", "");

					CssClass = GetParmValue("CssClass", "");

					EnableViewState = Convert.ToBoolean(GetParmValue("EnableViewState", "false"));

					ContentType = (ListContentType)Enum.Parse(typeof(ListContentType), GetParmValue("ContentType", "Blog"), true);

					SelectedCategories = new List<Guid>();

					List<string> lstCategories = GetParmValueList("SelectedCategories");
					foreach (string sCat in lstCategories) {
						if (!string.IsNullOrEmpty(sCat)) {
							SelectedCategories.Add(new Guid(sCat));
						}
					}
				}
				if (SelectedCategories.Count > 0) {
					ContentType = ListContentType.SpecifiedCategories;
				}
			} catch (Exception ex) {
			}


			base.OnPreRender(e);
		}
	}
}
