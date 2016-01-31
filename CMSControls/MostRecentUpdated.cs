using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Web.UI;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;

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

	[ToolboxData("<{0}:MostRecentUpdated runat=server></{0}:MostRecentUpdated>")]
	public class MostRecentUpdated : BaseNavHeaded {

		public MostRecentUpdated()
			: base() {
			this.ContentType = ListContentType.Blog;
			this.TakeTop = 5;
			this.ShowUpdateDate = false;
			this.DateFormat = "({0:d})";
		}

		public enum ListContentType {
			Unknown,

			[Description("Blog")]
			Blog,

			[Description("Content Page")]
			ContentPage,

			[Description("Specified Categories")]
			SpecifiedCategories
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstContentType {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict = EnumHelper.ToList<ListContentType>().Where(x => x.Value != (int)ListContentType.Unknown).ToDictionary(k => k.Text, v => v.Description);

				return _dict;
			}
		}

		[Category("Appearance")]
		[DefaultValue("Blog")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstContentType")]
		public ListContentType ContentType {
			get {
				String s = (String)ViewState["ContentType"];
				ListContentType c = ListContentType.Blog;
				if (!String.IsNullOrEmpty(s)) {
					c = (ListContentType)Enum.Parse(typeof(ListContentType), s, true);
				}
				return c;
			}

			set {
				ViewState["ContentType"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(5)]
		public int TakeTop {
			get {
				String s = (String)ViewState["TakeTop"];
				return ((s == null) ? 5 : int.Parse(s));
			}
			set {
				ViewState["TakeTop"] = value.ToString();
			}
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

		private List<StringItem> stringList = null;

		[
		Category("Behavior"),
		Description("The StringItem collection"),
		Browsable(false),
		DefaultValue(null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(StringItemCollectionEditor), typeof(UITypeEditor)),
		NotifyParentProperty(true),
		TemplateContainer(typeof(StringItem)),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public List<StringItem> CategorySlugList {
			get {
				if (stringList == null) {
					stringList = new List<StringItem>();
				}
				return stringList;
			}
		}

		private List<Guid> _guids = null;

		[Widget(WidgetAttribute.FieldMode.CheckBoxList, "lstCategories")]
		public List<Guid> SelectedCategories {
			get {
				if (_guids == null) {
					if (CategoryGuidList.Any()) {
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

		private List<string> _slugs = null;

		public List<string> SelectedCategorySlugs {
			get {
				if (_slugs == null) {
					if (CategorySlugList.Any()) {
						_slugs = (from n in CategorySlugList select n.StringValue).ToList();
					} else {
						_slugs = new List<string>();
					}
				}
				return _slugs;
			}
			set {
				_slugs = value;
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

		protected override void LoadData() {
			base.LoadData();

			this.NavigationData = GetUpdates();
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		[Widget(WidgetAttribute.FieldMode.CheckBox)]
		public bool ShowUpdateDate {
			get {
				bool s = false;
				if (ViewState["ShowUpdateDate"] != null) {
					try { s = (bool)ViewState["ShowUpdateDate"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["ShowUpdateDate"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("({0:d})")]
		[Widget(WidgetAttribute.FieldMode.TextBox)]
		public string DateFormat {
			get {
				String s = ViewState["DateFormat"] as String;
				return ((s == null) ? "({0:d})" : s);
			}
			set {
				ViewState["DateFormat"] = value;
			}
		}

		protected List<SiteNav> GetUpdates() {
			List<SiteNav> lst = new List<SiteNav>();

			switch (ContentType) {
				case ListContentType.Blog:
					lst = navHelper.GetLatestPosts(SiteData.CurrentSiteID, TakeTop, !SecurityData.IsAuthEditor);
					break;

				case ListContentType.ContentPage:
					lst = navHelper.GetLatest(SiteData.CurrentSiteID, TakeTop, !SecurityData.IsAuthEditor);
					break;

				case ListContentType.SpecifiedCategories:
					if (TakeTop > 0) {
						lst = navHelper.GetFilteredContentByIDPagedList(SiteData.CurrentSite, SelectedCategories, SelectedCategorySlugs, !SecurityData.IsAuthEditor, TakeTop, 0, "GoLiveDate", "DESC");
					} else {
						lst = navHelper.GetFilteredContentByIDPagedList(SiteData.CurrentSite, SelectedCategories, SelectedCategorySlugs, !SecurityData.IsAuthEditor, 100000, 0, "NavMenuText", "ASC");
					}
					break;
			}

			if (this.ShowUpdateDate && String.IsNullOrEmpty(this.DateFormat)) {
				this.DateFormat = "({0:d})";
			}

			if (this.ShowUpdateDate && !String.IsNullOrEmpty(this.DateFormat)) {
				lst.ForEach(x => x.NavMenuText = String.Format("{0}  {1}", x.NavMenuText, String.Format(this.DateFormat, x.GoLiveDate)));
			}

			return lst;
		}

		public override List<string> LimitedPropertyList {
			get {
				List<string> lst = base.LimitedPropertyList;
				lst.Add("TakeTop");
				lst.Add("MetaDataTitle");
				lst.Add("CssClass");
				lst.Add("ContentType");
				lst.Add("SelectedCategories");
				lst.Add("ShowUpdateDate");
				lst.Add("DateFormat");

				return lst.Distinct().ToList();
			}
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			try {
				if (this.PublicParmValues.Any()) {
					this.TakeTop = int.Parse(GetParmValue("TakeTop", "5"));

					this.DateFormat = GetParmValue("DateFormat", "({0:d})");

					this.ShowUpdateDate = Convert.ToBoolean(GetParmValue("ShowUpdateDate", "false"));

					this.ContentType = (ListContentType)Enum.Parse(typeof(ListContentType), GetParmValue("ContentType", ListContentType.Blog.ToString()), true);

					this.SelectedCategories = new List<Guid>();

					List<string> lstCategories = GetParmValueList("SelectedCategories");
					foreach (string sCat in lstCategories) {
						if (!String.IsNullOrEmpty(sCat)) {
							this.SelectedCategories.Add(new Guid(sCat));
						}
					}
				}
				if (this.SelectedCategories.Any()) {
					this.ContentType = ListContentType.SpecifiedCategories;
				}
			} catch (Exception ex) {
			}
		}
	}
}