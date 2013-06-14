using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class SiteTemplateUpdate : AdminBasePage {
		protected ContentPage pageHome = null;
		protected ContentPage pageIndex = null;
		protected string sSelTemplate = "Select Template";

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteTemplate);

			Guid siteID = SiteData.CurrentSiteID;

			pageHome = pageHelper.FindHome(siteID, true);

			if (pageHome != null && pageHome.Root_ContentID != Guid.Empty) {
				if (!IsPostBack) {

					if (SiteData.CurrentSite.Blog_Root_ContentID.HasValue) {
						pageIndex = pageHelper.FindContentByID(siteID, SiteData.CurrentSite.Blog_Root_ContentID.Value);
					}

					litHomepage.Text = pageHome.NavMenuText + "  [" + pageHome.FileName + "]";

					GeneralUtilities.BindListDefaultText(ddlHome, cmsHelper.Templates, pageHome.TemplateFile, sSelTemplate, "0");

					GeneralUtilities.BindListDefaultText(ddlBlog, cmsHelper.Templates, null, sSelTemplate, "0");

					if (pageIndex != null) {
						ParentPagePicker.SelectedPage = pageIndex.Root_ContentID;
						GeneralUtilities.SelectListValue(ddlBlog, pageIndex.TemplateFile);
					} else {
						ParentPagePicker.SelectedPage = null;
						ddlBlog.Enabled = false;
					}

					GeneralUtilities.BindListDefaultText(ddlAll, cmsHelper.Templates, null, sSelTemplate, "0");
					GeneralUtilities.BindListDefaultText(ddlTop, cmsHelper.Templates, null, sSelTemplate, "0");
					GeneralUtilities.BindListDefaultText(ddlSub, cmsHelper.Templates, null, sSelTemplate, "0");
					GeneralUtilities.BindListDefaultText(ddlPages, cmsHelper.Templates, null, sSelTemplate, "0");
					GeneralUtilities.BindListDefaultText(ddlPosts, cmsHelper.Templates, null, sSelTemplate, "0");

				}
			}
		}



		protected void btnSave_Click(object sender, EventArgs e) {
			Guid siteID = SiteData.CurrentSiteID;

			SiteData.CurrentSite.Blog_Root_ContentID = ParentPagePicker.SelectedPage;
			if (ParentPagePicker.SelectedPage.HasValue) {
				pageIndex = pageHelper.FindContentByID(siteID, ParentPagePicker.SelectedPage.Value);
			}
			SiteData.CurrentSite.Save();

			if (ddlPosts.SelectedValue.Length > 2) {
				pageHelper.UpdateAllBlogTemplates(siteID, ddlPosts.SelectedValue);
			}
			if (ddlPages.SelectedValue.Length > 2) {
				pageHelper.UpdateAllPageTemplates(siteID, ddlPages.SelectedValue);
			}

			if (ddlTop.SelectedValue.Length > 2) {
				pageHelper.UpdateTopPageTemplates(siteID, ddlTop.SelectedValue);
			}
			if (ddlSub.SelectedValue.Length > 2) {
				pageHelper.UpdateSubPageTemplates(siteID, ddlSub.SelectedValue);
			}

			if (ddlHome.SelectedValue.Length > 2 && pageHome != null) {
				pageHome.TemplateFile = ddlHome.SelectedValue;
				pageHome.ApplyTemplate();
			}
			if (ddlBlog.SelectedValue.Length > 2 && pageIndex != null) {
				pageIndex.TemplateFile = ddlBlog.SelectedValue;
				pageIndex.ApplyTemplate();
			}

			if (ddlAll.SelectedValue.Length > 2) {
				pageHelper.UpdateAllContentTemplates(siteID, ddlAll.SelectedValue);
			}

			Response.Redirect(SiteData.CurrentScriptName);

		}




	}
}