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

	[ToolboxData("<{0}:MostRecentUpdated runat=server></{0}:MostRecentUpdated>")]
	public class MostRecentUpdated : BaseServerControl, IHeadedList {

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

		public enum ListContentType {
			Unknown,
			Blog,
			ContentPage,
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		[Localizable(true)]
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

		protected List<SiteNav> GetUpdates() {

			switch (ContentType) {
				case ListContentType.Blog:
					return navHelper.GetLatestPosts(SiteData.CurrentSiteID, TakeTop, !SecurityData.IsAuthEditor);
				case ListContentType.ContentPage:
					return navHelper.GetLatest(SiteData.CurrentSiteID, TakeTop, !SecurityData.IsAuthEditor);
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

			output.WriteLine("<ul" + sCSS + " id=\"" + this.ClientID + "\"> ");
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


	}
}
