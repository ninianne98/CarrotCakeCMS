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
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class SiteTemplateUpdate : AdminBasePage {
		ContentPage pageHome = null;
		ContentPage pageIndex = null;


		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.SiteTemplate);

			pageHome = pageHelper.FindHome(SiteData.CurrentSiteID, true);

			if (SiteData.CurrentSite.Blog_Root_ContentID.HasValue) {
				pageIndex = pageHelper.FindContentByID(SiteData.CurrentSiteID, SiteData.CurrentSite.Blog_Root_ContentID.Value);
			}

			if (pageHome != null && pageHome.Root_ContentID != Guid.Empty) {
				if (!IsPostBack) {

					litHomepage.Text = pageHome.NavMenuText + "  [" + pageHome.FileName + "]";

					GeneralUtilities.BindListDefaultText(ddlHome, cmsHelper.Templates, pageHome.TemplateFile, "Select Template", "0");

					GeneralUtilities.BindListDefaultText(ddlBlog, cmsHelper.Templates, null, "Select Template", "0");

					if (pageIndex != null) {
						litBlogIndex.Text = pageIndex.NavMenuText + "  [" + pageIndex.FileName + "]";
						GeneralUtilities.SelectListValue(ddlBlog, pageIndex.TemplateFile);
					} else {
						litBlogIndex.Text = "No Index ***";
						ddlBlog.Enabled = false;
					}

					GeneralUtilities.BindListDefaultText(ddlAll, cmsHelper.Templates, null, "Select Template", "0");
					GeneralUtilities.BindListDefaultText(ddlTop, cmsHelper.Templates, null, "Select Template", "0");
					GeneralUtilities.BindListDefaultText(ddlSub, cmsHelper.Templates, null, "Select Template", "0");
					GeneralUtilities.BindListDefaultText(ddlPages, cmsHelper.Templates, null, "Select Template", "0");
					GeneralUtilities.BindListDefaultText(ddlPosts, cmsHelper.Templates, null, "Select Template", "0");

				}
			}

		}



		protected void btnSave_Click(object sender, EventArgs e) {

			if (ddlPosts.SelectedValue.Length > 2) {
				pageHelper.UpdateAllBlogTemplates(SiteData.CurrentSiteID, ddlPosts.SelectedValue);
			}
			if (ddlPages.SelectedValue.Length > 2) {
				pageHelper.UpdateAllPageTemplates(SiteData.CurrentSiteID, ddlPages.SelectedValue);
			}

			if (ddlTop.SelectedValue.Length > 2) {
				pageHelper.UpdateTopPageTemplates(SiteData.CurrentSiteID, ddlTop.SelectedValue);
			}
			if (ddlSub.SelectedValue.Length > 2) {
				pageHelper.UpdateSubPageTemplates(SiteData.CurrentSiteID, ddlSub.SelectedValue);
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
				pageHelper.UpdateAllContentTemplates(SiteData.CurrentSiteID, ddlAll.SelectedValue);
			}

			Response.Redirect(SiteData.CurrentScriptName);

		}




	}
}