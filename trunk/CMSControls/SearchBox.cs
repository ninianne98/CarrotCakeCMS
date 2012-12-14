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
	[ToolboxData("<{0}:SearchBox runat=server></{0}:SearchBox>")]
	public class SearchBox : BaseServerControl, INamingContainer {

		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateInstance(TemplateInstance.Single)]
		[MergableProperty(false)]
		[Browsable(false)]
		[TemplateContainer(typeof(SearchBox))]
		public ITemplate SearchTemplate { get; set; }


		protected PlaceHolder phEntry = new PlaceHolder();
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

		protected override void OnInit(EventArgs e) {

			base.OnInit(e);

			if (SearchTemplate == null) {
				SearchTemplate = new DefaultSearchBoxForm();
			}

		}

		protected override void Render(HtmlTextWriter writer) {
			base.BaseRender(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			base.BaseRenderContents(writer);
		}


		protected override void CreateChildControls() {

			if (SearchTemplate != null) {
				this.Controls.Clear();
			}

			phEntry.Visible = true;
			phEntry.Controls.Clear();
			if (SearchTemplate != null) {
				SearchTemplate.InstantiateIn(phEntry);
			}

			FindEntryFormCtrls(phEntry);

			phEntry.Controls.Add(new jsHelperLib());

			TextBox txtSearchText = (TextBox)GetEntryFormControl("SearchText");

			if (txtSearchText != null) {
				string sJS = "\r\n\r\n <script type=\"text/javascript\"> \r\n" +
					"\t function " + JS_SearchName + "() { \r\n" +
					"\t\t var theSearchTerm = $('#" + this.ClientID + "_" + txtSearchText.ID + "').val(); \r\n" +
					"\t\t __carrotware_RedirectWithQuerystringParm('" + SiteData.CurrentSite.SiteSearchPath + "', 'search', theSearchTerm); \r\n" +
					"\t\t return false; \r\n " +
					"\t } \r\n" +
					"\r\n " +
					"\t function " + JS_EnterSearch + "(e) { \r\n" +
					"\t 	var obj = window.event ? event : e; \r\n" +
					"\t 	var key = (window.event) ? event.keyCode : e.which; \r\n" +
					"\t 	if ((key == 13) || (key == 10)) { \r\n" +
					"\t 		obj.returnValue = false; \r\n" +
					"\t 		obj.cancelBubble = true; \r\n" +
					"\t 		" + JS_SearchName + "(); \r\n" +
					"\t 		return false; \r\n" +
					"\t 	} \r\n" +
					"\t 	return true; \r\n" +
					"\t } \r\n" +
					" </script> \r\n\r\n";

				phEntry.Controls.Add(new Literal { Text = sJS });
			}

			this.Controls.Add(phEntry);

			base.CreateChildControls();
		}


		protected Control GetEntryFormControl(string ControlName) {

			return (from x in EntryFormControls
					where x.ID != null
					&& x.ID.ToLower() == ControlName.ToLower()
					select x).FirstOrDefault();
		}

		private void FindEntryFormCtrls(Control X) {

			foreach (Control c in X.Controls) {
				EntryFormControls.Add(c);

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
}