using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;

//  http://msdn.microsoft.com/en-us/library/yhzc935f.aspx

namespace Carrotware.CMS.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:MostRecentUpdated runat=server></{0}:MostRecentUpdated>")]
	public class MostRecentUpdated : BaseServerControl {


		public string UpdateTitle { get; set; }

		public bool IncludeParent { get; set; }

		private int _TakeTop = 5;
		public int TakeTop {
			get { return _TakeTop; }
			set { _TakeTop = value; }
		}


		protected List<SiteNav> GetUpdates() {
			return navHelper.GetLatest(SiteData.CurrentSiteID, TakeTop, !SecurityData.IsAuthEditor);
		}

		protected override void RenderContents(HtmlTextWriter output) {
			var lst = GetUpdates();

			if (string.IsNullOrEmpty(UpdateTitle)) {
				output.Write("<h2>Most Recent Updates</h2>\r\n");
			} else {
				output.Write("<h2>" + UpdateTitle + "</h2>\r\n");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.Write("<ul " + sCSS + " id=\"" + this.ClientID + "\">\r\n");

			foreach (var c in lst) {
				if (!c.PageActive) {
					c.NavMenuText = "&#9746; " + c.NavMenuText;
				}

				if (c.NavFileName.ToLower() == SiteData.CurrentScriptName.ToLower()) {
					output.Write("<li class=\"selected\"><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				} else {
					output.Write("<li><a href=\"" + c.NavFileName + "\">" + c.NavMenuText + "</a></li>\r\n");
				}
			}
			output.Write("</ul>\r\n");
		}


	}
}
