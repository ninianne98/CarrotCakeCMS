using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
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

	[Designer(typeof(SearchBoxDesigner))]
	[ParseChildren(true, "SearchTemplate"), PersistChildren(true)]
	[ToolboxData("<{0}:SearchBox runat=server></{0}:SearchBox>")]
	public class SearchBox : BaseServerControl, INamingContainer {

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
				String s = (String)ViewState["OverrideTextboxName"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["OverrideTextboxName"] = value;
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

		protected PlaceHolder phEntry = new PlaceHolder();
		protected Literal litScript = new Literal();

		protected List<Control> EntryFormControls = new List<Control>();

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

			if (SearchTemplate == null) {
				SearchTemplate = new DefaultSearchBoxForm();
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

			string sScript = ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.SearchBoxJS.txt");

			if (SearchTemplate != null) {
				this.Controls.Clear();
			}
			phEntry.Controls.Clear();

			phEntry.Controls.Add(new jsHelperLib());
			phEntry.Controls.Add(litScript);
			this.Controls.Add(phEntry);

			phEntry.Visible = true;
			if (this.SearchTemplate != null) {
				this.SearchTemplate.InstantiateIn(phEntry);
			}

			FindEntryFormCtrls(phEntry);

			TextBox txtSearchText = null;
			if (string.IsNullOrEmpty(OverrideTextboxName)) {
				txtSearchText = (TextBox)GetEntryFormControl("SearchText");

				if (txtSearchText == null) {
					txtSearchText = (TextBox)GetEntryFormControl(typeof(TextBox));
				}
			} else {
				txtSearchText = new TextBox();
				txtSearchText.ID = "over_" + OverrideTextboxName;
			}

			if (txtSearchText != null) {
				sScript = sScript.Replace("{SEARCH_PARAM}", SiteData.SearchQueryParameter);
				sScript = sScript.Replace("{SEARCH_FUNC}", JS_SearchName);
				sScript = sScript.Replace("{SEARCH_ENTERFUNC}", JS_EnterSearch);
				sScript = sScript.Replace("{SEARCH_ENTERFUNC2}", JS_EnterSearch2);

				if (string.IsNullOrEmpty(OverrideTextboxName)) {
					sScript = sScript.Replace("{SEARCH_TEXT}", this.ClientID + "_" + txtSearchText.ID);
				} else {
					sScript = sScript.Replace("{SEARCH_TEXT}", OverrideTextboxName);
				}

				sScript = sScript.Replace("{SEARCH_URL}", SiteData.CurrentSite.SiteSearchPath);

				litScript.Text = sScript;
			}

			base.CreateChildControls();
		}

		protected Control GetEntryFormControl(string ControlName) {

			return (from x in EntryFormControls
					where x.ID != null
					&& x.ID.ToLower() == ControlName.ToLower()
					select x).FirstOrDefault();
		}

		protected Control GetEntryFormControl(Type type) {

			return (from x in EntryFormControls
					where x.ID != null
					&& x.GetType() == type
					select x).FirstOrDefault();
		}

		private void FindEntryFormCtrls(Control X) {

			foreach (Control c in X.Controls) {
				EntryFormControls.Add(c);

				if (c is LiteralControl) {
					LiteralControl z = (LiteralControl)c;
					z.Text = z.Text.Replace("{EXEC_SEARCH_FUNCTION}", "return " + JS_SearchName + "()");
					z.Text = z.Text.Replace("{EXEC_SEARCH_FUNCTION_ENTER}", "return " + JS_EnterSearch + "(event)");
				}

				if (c is TextBox && c.ID != null) {
					TextBox z = (TextBox)c;
					if (z.ID.ToLower().Contains("search")) {
						z.Attributes["onkeypress"] = "return " + JS_EnterSearch + "()";
					}
				}

				if (c is Button && c.ID != null) {
					Button z = (Button)c;
					if (z.ID.ToLower().Contains("search")) {
						z.OnClientClick = "return " + JS_SearchName + "()";
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
				template = new TemplateDefinition(this, "SearchTemplate", control, "SearchTemplate", true);
				group.AddTemplateDefinition(template);
				collection.Add(group);

				return collection;
			}
		}

	}

}