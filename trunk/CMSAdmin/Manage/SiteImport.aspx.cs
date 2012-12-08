using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class SiteImport : AdminBasePage {

		public Guid guidImportID = Guid.Empty;
		SiteExport exSite = null;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["importid"])) {
				guidImportID = new Guid(Request.QueryString["importid"].ToString());
			}

			litMessage.Text = "";

			if (guidImportID != Guid.Empty) {
				exSite = ContentImportExportUtils.GetSerializedSiteExport(guidImportID);

				litName.Text = exSite.TheSite.SiteName;
				litDescription.Text = exSite.TheSite.SiteTagline;
				litImportSource.Text = exSite.CarrotCakeVersion;
				litDate.Text = exSite.ExportDate.ToString();

				if (!IsPostBack) {
					gvPages.DataSource = (from c in exSite.ThePages
										  where c.ThePage.ContentType == ContentPageType.PageType.ContentEntry
										  orderby c.ThePage.NavOrder
										  select c.ThePage).ToList();
					gvPages.DataBind();


					gvPosts.DataSource = (from c in exSite.ThePages
										  where c.ThePage.ContentType == ContentPageType.PageType.BlogEntry
										  orderby c.ThePage.CreateDate
										  select c.ThePage).ToList();
					gvPosts.DataBind();


					ddlTemplatePage.DataSource = cmsHelper.Templates;
					ddlTemplatePage.DataBind();

					ddlTemplatePost.DataSource = cmsHelper.Templates;
					ddlTemplatePost.DataBind();

					lblPages.Text = gvPages.Rows.Count.ToString();
					lblPosts.Text = gvPosts.Rows.Count.ToString();

				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			SiteData.CurrentSite = null;

			SiteData site = SiteData.CurrentSite;

			litMessage.Text = "<p>No Items Selected For Import</p>";

			if (chkSite.Checked || chkPages.Checked || chkPosts.Checked) {
				List<string> tags = site.GetTagList().Select(x => x.TagSlug.ToLower()).ToList();
				List<string> cats = site.GetCategoryList().Select(x => x.CategorySlug.ToLower()).ToList();

				exSite.TheTags.RemoveAll(x => tags.Contains(x.TagSlug.ToLower()));
				exSite.TheCategories.RemoveAll(x => cats.Contains(x.CategorySlug.ToLower()));

				litMessage.Text = "<p>Imported Tags and Categories</p>";

				List<ContentTag> lstTag = (from l in exSite.TheTags.Distinct()
										   select new ContentTag {
											   ContentTagID = Guid.NewGuid(),
											   SiteID = site.SiteID,
											   TagSlug = l.TagSlug,
											   TagText = l.TagText
										   }).ToList();

				List<ContentCategory> lstCat = (from l in exSite.TheCategories.Distinct()
												select new ContentCategory {
													ContentCategoryID = Guid.NewGuid(),
													SiteID = site.SiteID,
													CategorySlug = l.CategorySlug,
													CategoryText = l.CategoryText
												}).ToList();

				foreach (var v in lstTag) {
					v.Save();
				}
				foreach (var v in lstCat) {
					v.Save();
				}
			}


			if (chkSite.Checked) {
				litMessage.Text += "<p>Updated Site Name</p>";
				site.SiteName = exSite.TheSite.SiteName;
				site.SiteTagline = exSite.TheSite.SiteTagline;
				site.Save();
			}


			using (SiteNavHelper navHelper = new SiteNavHelper()) {

				if (chkPages.Checked) {

					litMessage.Text += "<p>Imported Pages</p>";

					int iOrder = 0;
					SiteNav navHome = navHelper.FindHome(site.SiteID, false);
					if (navHome != null) {
						iOrder = 2;
					}

					foreach (var impCP in (from c in exSite.ThePages
										   where c.ThePage.ContentType == ContentPageType.PageType.ContentEntry
										   orderby c.ThePage.NavOrder, c.ThePage.NavMenuText
										   select c).ToList()) {

						ContentPage cp = impCP.ThePage;
						cp.Root_ContentID = impCP.NewRootContentID;
						cp.ContentID = Guid.NewGuid();
						cp.SiteID = site.SiteID;
						cp.ContentType = ContentPageType.PageType.ContentEntry;
						cp.EditDate = DateTime.Now;
						cp.EditUserId = SecurityData.CurrentUserGuid;
						cp.NavOrder = iOrder;
						cp.TemplateFile = ddlTemplatePage.SelectedValue;

						ContentPageExport parent = (from c in exSite.ThePages
													where c.ThePage.ContentType == ContentPageType.PageType.ContentEntry
													  && c.ThePage.FileName.ToLower() == impCP.ParentFileName.ToLower()
													select c).FirstOrDefault();

						SiteNav navParent = null;

						SiteNav navData = navHelper.GetLatestVersion(site.SiteID, false, cp.FileName.ToLower());
						if (parent != null) {
							cp.Parent_ContentID = parent.NewRootContentID;
							navParent = navHelper.GetLatestVersion(site.SiteID, false, parent.ThePage.FileName.ToLower());
						}

						//if URL exists already, make this become a new version in the current series
						if (navData != null) {
							cp.Root_ContentID = navData.Root_ContentID;
							if (navData.NavOrder == 0) {
								cp.NavOrder = 0;
							}
						}
						//preserve homepage
						if (navHome != null && navHome.FileName.ToLower() == cp.FileName.ToLower()) {
							cp.NavOrder = 0;
						}
						//if the file url in the upload has an existing ID, use that, not the ID from the queue
						if (navParent != null) {
							cp.Parent_ContentID = navParent.Root_ContentID;
						}

						cp.SavePageEdit();

						iOrder++;
					}
				}


				if (chkPosts.Checked) {
					litMessage.Text += "<p>Imported Posts</p>";

					foreach (var impCP in (from c in exSite.ThePages
										   where c.ThePage.ContentType == ContentPageType.PageType.BlogEntry
										   orderby c.ThePage.CreateDate
										   select c).ToList()) {

						ContentPage cp = impCP.ThePage;
						cp.Root_ContentID = impCP.NewRootContentID;
						cp.ContentID = Guid.NewGuid();
						cp.SiteID = site.SiteID;
						cp.Parent_ContentID = null;
						cp.ContentType = ContentPageType.PageType.BlogEntry;
						cp.EditDate = DateTime.Now;
						cp.EditUserId = SecurityData.CurrentUserGuid;
						cp.NavOrder = 10;
						cp.TemplateFile = ddlTemplatePost.SelectedValue;

						SiteNav navData = navHelper.GetLatestVersion(site.SiteID, false, cp.FileName.ToLower());

						//if URL exists already, make this become a new version in the current series
						if (navData != null) {
							cp.Root_ContentID = navData.Root_ContentID;
						}

						cp.SavePageEdit();
					}

					using (ContentPageHelper cph = new ContentPageHelper()) {
						cph.BulkFileNameFromSlug(site.SiteID, site.Blog_DatePattern);
					}
				}

			}
		}

	}
}