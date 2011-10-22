using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/
namespace Carrotware.CMS.UI.Admin {

	public partial class Default : AdminBasePage {


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Home);

			litID.Text = SiteData.CurrentSiteID.ToString();

			//pnlFolder.Visible = IsAdmin;

			if (!IsPostBack) {
				var site = siteHelper.Get(SiteData.CurrentSiteID);
				txtURL.Text = Request.ServerVariables["SERVER_NAME"];
				if (site != null) {
					txtSiteName.Text = site.SiteName;
					txtURL.Text = site.MainURL;
					txtKey.Text = site.MetaKeyword;
					txtDescription.Text = site.MetaDescription;
					//txtFolder.Text = site.SiteFolder;
					chkHide.Checked = site.BlockIndex;
				}
			}

			var lst = (from c in db.tblSerialCaches
					   where c.EditDate < DateTime.Now.AddHours(-3)
					   && c.SiteID == SiteID
					   select c).ToList();

			if (lst.Count > 0) {
				foreach (var l in lst) {
					db.tblSerialCaches.DeleteOnSubmit(l);
				}
				db.SubmitChanges();
			}

		}

		protected void btnSave_Click(object sender, EventArgs e) {

			var site = siteHelper.Get(SiteData.CurrentSiteID);


			if (site == null) {
				site = new SiteData();
				site.SiteID = SiteID;
			}

			if (site != null) {
				site.SiteName = txtSiteName.Text;
				site.MainURL = txtURL.Text;
				site.MetaKeyword = txtKey.Text;
				//site.SiteFolder = txtFolder.Text;
				site.MetaDescription = txtDescription.Text;
				site.BlockIndex = chkHide.Checked;
			}

			site.Save();
		}


	}
}
