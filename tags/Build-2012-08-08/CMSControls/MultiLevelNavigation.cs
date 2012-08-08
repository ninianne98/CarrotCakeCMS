using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:MultiLevelNavigation runat=server></{0}:MultiLevelNavigation>")]
	public class MultiLevelNavigation : BaseServerControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}


		protected List<SiteNav> GetTopNav() {
			return navHelper.GetTopNavigation(SiteData.CurrentSiteID, !SiteData.IsAuthEditor);
		}

		protected List<SiteNav> GetChildren(Guid Root_ContentID) {
			return navHelper.GetChildNavigation(SiteData.CurrentSiteID, Root_ContentID, !SiteData.IsAuthEditor);
		}


		private void LoadChildren(HtmlTextWriter output, Guid rootContentID, int iLevel) {
			var cc = GetChildren(rootContentID);

			if (cc.Count > 0) {
				output.Write("\r\n\t<ul class=\"children level-" + iLevel + "\">\r\n");
				foreach (var c2 in cc) {
					if (!c2.PageActive) {
						c2.NavMenuText = "&#9746; " + c2.NavMenuText;
					}
					if (c2.NavFileName.ToLower() == SiteData.CurrentScriptName.ToLower()) {
						output.Write("\t\t<li class=\"" + CSSSelected + " level-" + iLevel + "\"> ");
					} else {
						output.Write("\t\t<li class=\"level-" + iLevel + "\"> ");
					}
					output.Write("<a href=\"" + c2.NavFileName + "\">" + c2.NavMenuText + "</a> \r\n");
					LoadChildren(output, c2.Root_ContentID, iLevel + 1);
					output.Write("\r\n</li>\r\n");
				}
				output.Write("\t</ul>\r\n\t");
			}
		}


		protected override void RenderContents(HtmlTextWriter output) {

			var pageContents = navHelper.GetPageNavigation(SiteData.CurrentSiteID, SiteData.CurrentScriptName);

			var sParent = "";
			if (pageContents != null) {
				sParent = pageContents.FileName.ToLower();
			}

			var lst = GetTopNav();
			output.Write("<div name=\"" + this.UniqueID + "\" id=\"" + this.ClientID + "\">\r\n");
			output.Write("<div id=\"" + this.ClientID + "-inner\">\r\n");

			output.Write("<ul class=\"parent\">\r\n");
			foreach (var c1 in lst) {

				if (!c1.PageActive) {
					c1.NavMenuText = "&#9746; " + c1.NavMenuText;
				}
				if (c1.NavFileName.ToLower() == SiteData.CurrentScriptName.ToLower() || c1.NavFileName.ToLower() == sParent) {
					output.Write("\t<li class=\"" + CSSSelected + "\"> ");
				} else {
					output.Write("\t<li> ");
				}

				output.Write("<a href=\"" + c1.NavFileName + "\">" + c1.NavMenuText + "</a>");
				LoadChildren(output, c1.Root_ContentID, 1);
				output.Write("</li>\r\n");
			}
			output.Write("</ul>\r\n");

			output.Write("</div>\r\n");
			output.Write("</div>\r\n");

		}


	}
}
