using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class ContentEdit : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		public Guid guidWidgetID = Guid.Empty;
		ContentPage pageContents = null;
		public string sFieldName = "";
		public string sMode = "";

		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["pageid"])) {
				guidContentID = new Guid(Request.QueryString["pageid"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["widgetid"])) {
				guidWidgetID = new Guid(Request.QueryString["widgetid"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["field"])) {
				sFieldName = Request.QueryString["field"].ToString().ToLower();
			}
			if (!string.IsNullOrEmpty(Request.QueryString["mode"])) {
				sMode = Request.QueryString["mode"].ToString().ToLower();
			}

			cmsHelper.OverrideKey(guidContentID);

			if (cmsHelper.cmsAdminContent != null) {
				pageContents = cmsHelper.cmsAdminContent;

				if (sMode == "plain") {
					divCenter.Visible = false;
					reBody.CssClass = "mcePlainText";
					pnlPlain.Visible = true;
				} else {
					pnlRichEdit.Visible = true;
				}

				if (!IsPostBack) {
					if (guidWidgetID != Guid.Empty) {
						Widget pageWidget = (from w in cmsHelper.cmsAdminWidget
												 where w.Root_WidgetID == guidWidgetID
												 select w).FirstOrDefault();

						reBody.Text = pageWidget.ControlProperties;

					} else {
						switch (sFieldName) {
							case "c":
								reBody.Text = pageContents.PageText;
								break;
							case "l":
								reBody.Text = pageContents.LeftPageText;
								break;
							case "r":
								reBody.Text = pageContents.RightPageText;
								break;
						}
					}
				}

			}

		}


		protected void btnSave_Click(object sender, EventArgs e) {

			if (pageContents != null) {
				if (guidWidgetID != Guid.Empty) {
					List<Widget> lstWidgets = cmsHelper.cmsAdminWidget;

					Widget pageWidget = (from w in lstWidgets
											 where w.Root_WidgetID == guidWidgetID
											 select w).FirstOrDefault();

					pageWidget.ControlProperties = reBody.Text;
					pageWidget.WidgetDataID = Guid.NewGuid();
					
					lstWidgets.RemoveAll(x => x.Root_WidgetID == guidWidgetID);

					lstWidgets.Add(pageWidget);

					cmsHelper.cmsAdminWidget = lstWidgets;
				} else {
					switch (sFieldName) {
						case "c":
							pageContents.PageText = reBody.Text;
							break;
						case "l":
							pageContents.LeftPageText = reBody.Text;
							break;
						case "r":
							pageContents.RightPageText = reBody.Text;
							break;
					}

					cmsHelper.cmsAdminContent = pageContents;
				}

				Response.Redirect(SiteData.CurrentScriptName + "?" + Request.QueryString.ToString());
			}

		}
	}
}
