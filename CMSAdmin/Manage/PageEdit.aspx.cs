using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class PageEdit : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		ContentPage pageContents = null;

		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["pageid"])) {
				guidContentID = new Guid(Request.QueryString["pageid"].ToString());
			}
			cmsHelper.OverrideKey(guidContentID);

			if (cmsHelper.cmsAdminContent != null) {
				pageContents = cmsHelper.cmsAdminContent;
				litPageName.Text = pageContents.FileName;

				if (!IsPostBack) {

					txtTitle.Text = pageContents.TitleBar;
					txtNav.Text = pageContents.NavMenuText;
					txtHead.Text = pageContents.PageHead;

					txtDescription.Text = pageContents.MetaDescription;
					txtKey.Text = pageContents.MetaKeyword;

					lblUpdated.Text = pageContents.EditDate.ToString();

					chkActive.Checked = Convert.ToBoolean(pageContents.PageActive);
				}
			}

		}


		protected void btnSave_Click(object sender, EventArgs e) {

			if (pageContents != null) {
				pageContents.TitleBar = txtTitle.Text;
				pageContents.NavMenuText = txtNav.Text;
				pageContents.PageHead = txtHead.Text;

				pageContents.MetaDescription = txtDescription.Text;
				pageContents.MetaKeyword = txtKey.Text;

				pageContents.EditDate = DateTime.Now;

				pageContents.PageActive = chkActive.Checked;

				cmsHelper.cmsAdminContent = pageContents;

				Response.Redirect(SiteData.CurrentScriptName + "?pageid=" + pageContents.Root_ContentID.ToString());
			}

		}
	}
}
