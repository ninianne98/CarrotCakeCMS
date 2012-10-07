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


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string UpdateTitle {
			get {
				string s = (string)ViewState["UpdateTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["UpdateTitle"] = value;
			}
		}

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
			int indent = output.Indent;

			List<SiteNav> lstNav = GetUpdates();

			output.Indent = indent + 3;
			output.WriteLine();

			if (string.IsNullOrEmpty(UpdateTitle)) {
				output.WriteLine("<h2>Most Recent Updates</h2> ");
			} else {
				output.WriteLine("<h2>" + UpdateTitle + "</h2> ");
			}

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\"> ");
			output.Indent++;

			foreach (SiteNav c in lstNav) {
				IdentifyLinkAsInactive(c);
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


	}
}
