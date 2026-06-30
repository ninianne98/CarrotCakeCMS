using Carrotware.CMS.Core;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	[Designer(typeof(SearchBoxDesigner))]
	[ParseChildren(true, "SearchTemplate"), PersistChildren(true)]
	[ToolboxData("<{0}:SearchBox runat=server></{0}:SearchBox>")]
	public class SearchBox : BaseServerControl, INamingContainer {

		public class Fields {
			public const string SearchText = "SearchText";
		}

		//=================

		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateInstance(TemplateInstance.Single)]
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[TemplateContainer(typeof(SearchBox))]
		public ITemplate SearchTemplate { get; set; }

		[Category("Appearance")]
		[DefaultValue("")]
		public string OverrideTextboxName {
			get {
				string s = (string)ViewState["OverrideTextboxName"];
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["OverrideTextboxName"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				string s = (string)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}

		protected PlaceHolder _phEntry = new PlaceHolder();
		protected Literal _litScript = new Literal();

		protected List<Control> _entryFormControls = new List<Control>();

		protected string JS_SearchName {
			get {
				return "CarrotCakeSiteSearch_" + this.ClientID;
			}
		}

		protected string JS_EnterSearch {
			get {
				return "CarrotCakeSiteSearchEnter_" + this.ClientID;
			}
		}

		protected string JS_EnterSearch2 {
			get {
				return "CarrotCakeSiteSearchEnter2_" + this.ClientID;
			}
		}

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			if (this.SearchTemplate == null) {
				this.SearchTemplate = new DefaultSearchBoxForm();
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			base.BaseRender(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			base.BaseRenderContents(writer);
		}

		protected override void CreateChildControls() {
			if (this.SearchTemplate != null) {
				this.Controls.Clear();
			}
			_phEntry.Controls.Clear();

			_phEntry.Controls.Add(new jsHelperLib());
			_phEntry.Controls.Add(_litScript);
			this.Controls.Add(_phEntry);

			_phEntry.Visible = true;
			if (this.SearchTemplate != null) {
				this.SearchTemplate.InstantiateIn(_phEntry);
			}

			FindEntryFormCtrls(_phEntry);

			TextBox searchText = null;
			if (string.IsNullOrWhiteSpace(this.OverrideTextboxName)) {
				searchText = (TextBox)GetEntryFormControl(Fields.SearchText);

				if (searchText == null) {
					searchText = (TextBox)GetEntryFormControl(typeof(TextBox));
				}
			} else {
				searchText = new TextBox();
				searchText.ID = "over_" + this.OverrideTextboxName;
			}

			var sb = new StringBuilder();
			sb.Append(ControlUtilities.GetManifestResourceStream("SearchBoxJS.txt"));

			if (sb.Length > 1) {
				sb.Replace("{SEARCH_PARAM}", SiteData.SearchQueryParameter);
				sb.Replace("{SEARCH_FUNC}", this.JS_SearchName);
				sb.Replace("{SEARCH_ENTERFUNC}", this.JS_EnterSearch);
				sb.Replace("{SEARCH_ENTERFUNC2}", this.JS_EnterSearch2);

				if (searchText != null) {
					if (string.IsNullOrEmpty(this.OverrideTextboxName)) {
						sb.Replace("{SEARCH_TEXT}", this.ClientID + "_" + searchText.ID);
					} else {
						sb.Replace("{SEARCH_TEXT}", this.OverrideTextboxName);
					}
				}

				sb.Replace("{SEARCH_URL}", SiteData.CurrentSite.SiteSearchPath);

				sb.Replace("{EXEC_SEARCH_FUNCTION}", "return " + this.JS_SearchName + "()");
				sb.Replace("{EXEC_SEARCH_FUNCTION_ENTER}", "return " + this.JS_EnterSearch + "(event)");

				_litScript.Text = sb.ToString();
			}

			base.CreateChildControls();
		}

		protected Control GetEntryFormControl(string controlName) {
			return (from c in _entryFormControls
					where c.ID != null
							&& c.ID.ToLowerInvariant() == controlName.ToLowerInvariant()
					select c).FirstOrDefault();
		}

		protected Control GetEntryFormControl(Type type) {
			return (from c in _entryFormControls
					where c.ID != null
							&& c.GetType() == type
					select c).FirstOrDefault();
		}

		private void FindEntryFormCtrls(Control ctrls) {
			foreach (Control c in ctrls.Controls) {
				_entryFormControls.Add(c);

				if (string.IsNullOrWhiteSpace(c.ID) == false
							&& c.ID.ToLowerInvariant().Contains("search")) {
					if (c is TextBox) {
						var ctrl = (TextBox)c;
						ctrl.Attributes["onkeypress"] = "return " + this.JS_EnterSearch + "()";
					}

					if (c is Button) {
						var ctrl = (Button)c;
						ctrl.OnClientClick = "return " + this.JS_SearchName + "()";
					}
				}

				FindEntryFormCtrls(c);
			}
		}
	}

	//======================================

	public class SearchBoxDesigner : ControlDesigner {

		public override void Initialize(IComponent Component) {
			base.Initialize(Component);
			SetViewFlags(ViewFlags.TemplateEditing, true);
		}

		public override string GetDesignTimeHtml() {
			Control myctrl = (Control)base.ViewControl;
			string sType = myctrl.GetType().ToString().Replace(myctrl.GetType().Namespace + ".", "CMS, ");
			string sID = myctrl.ID;

			string sTextOut = "[" + sType + " - " + sID + "]";

			return "<span>" + sTextOut + "</span>";
		}

		public override TemplateGroupCollection TemplateGroups {
			get {
				TemplateGroupCollection collection = new TemplateGroupCollection();
				TemplateGroup group;
				TemplateDefinition template;
				SearchBox control;

				control = (SearchBox)Component;
				group = new TemplateGroup("Item");
				template = new TemplateDefinition(this, nameof(SearchBox.SearchTemplate), control, nameof(SearchBox.SearchTemplate), true);
				group.AddTemplateDefinition(template);
				collection.Add(group);

				return collection;
			}
		}
	}
}