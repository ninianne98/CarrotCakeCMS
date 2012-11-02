using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
	[ToolboxData("<{0}:ContentContainer runat=server></{0}:ContentContainer>")]
	public class ContentContainer : Literal {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsAdminMode {
			get {
				bool s = false;
				if (ViewState["IsAdminMode"] != null) {
					try { s = (bool)ViewState["IsAdminMode"]; } catch { }
				}
				return s;
			}
			set {
				ViewState["IsAdminMode"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Guid DatabaseKey {
			get {
				Guid s = Guid.Empty;
				try { s = new Guid(ViewState["DatabaseKey"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["DatabaseKey"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string ZoneChar {
			get {
				String s = (String)ViewState["ZoneChar"];
				return ((s == null) ? String.Empty : s);
			}

			set {
				ViewState["ZoneChar"] = value;
			}
		}



		protected override void Render(HtmlTextWriter w) {
			if (IsAdminMode) {


				string sPrefix = "<div id=\"cms_" + this.ClientID + "\" class=\"cmsContentContainer\">\r\n" +
							" <div class=\"cmsContentContainerTitle\">\r\n" +
							"<a title=\"Edit " + this.ClientID + "\" id=\"cmsContentAreaLink_" + this.ClientID + "\" class=\"cmsContentContainerInnerTitle\" " +
							" href=\"javascript:cmsShowEditContentForm('" + ZoneChar + "','html'); \"> " +
							" Edit " + this.ClientID + " <span class=\"cmsWidgetBarIconPencil2\"  /></span> </a></div> \r\n" +
							"<div class=\"cmsWidgetControl\" id=\"cmsAdmin_" + this.ClientID + "\" ><div>\r\n" +
							"<!-- <#|BEGIN_CARROT_CMS|#> -->\r\n";

				//w.Write("<div id=\"" + this.ClientID + "\" style=\"width:250px; background:#DAE816; color:#CF26DB; \"><div style=\"background:#CF26DB; color:#DAE816; \">title</div><br />\r\n"); 
				w.Write(sPrefix);

			} else {
				w.Write("\r\n");
			}

			base.Render(w);

			if (IsAdminMode) {
				w.Write("\r\n<!-- <#|END_CARROT_CMS|#> -->\r\n</div>\r\n<div style=\"clear: both; \"></div>\r\n</div></div>\r\n");
			} else {
				w.Write("\r\n");
			}
		}


	}
}
