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


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class SiteImport : AdminBasePage {

		public Guid guidImportID = Guid.Empty;
		SiteExport exSite = null;
		List<BasicContentData> sitePageList = new List<BasicContentData>();

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


					GeneralUtilities.BindList(ddlTemplatePage, cmsHelper.Templates);
					GeneralUtilities.BindList(ddlTemplatePost, cmsHelper.Templates);

					lblPages.Text = gvPages.Rows.Count.ToString();
					lblPosts.Text = gvPosts.Rows.Count.ToString();
					lblComments.Text = exSite.TheComments.Count.ToString();
				}
			}
		}

		private int iAccessCounter = 0;

		protected BasicContentData GetFileInfoFromList(SiteData site, string sFilename) {

			if (sitePageList == null || sitePageList.Count < 1 || iAccessCounter % 25 == 0) {
				sitePageList = site.GetFullSiteFileList();
				iAccessCounter = 0;
			}
			iAccessCounter++;

			BasicContentData pageData = (from m in sitePageList
										 where m.FileName.ToLower() == sFilename.ToLower()
										 select m).FirstOrDefault();

			if (pageData == null) {
				using (SiteNavHelper navHelper = new SiteNavHelper()) {
					pageData = BasicContentData.CreateBasicContentDataFromSiteNav(navHelper.GetLatestVersion(site.SiteID, false, sFilename.ToLower()));
				}
			}

			return pageData;
		}

		SiteNav _navHome = null;
		protected SiteNav GetHomePage(SiteData site) {
			if (_navHome == null) {
				using (SiteNavHelper navHelper = new SiteNavHelper()) {
					_navHome = navHelper.FindHome(site.SiteID, false);
				}
			}
			return _navHome;
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

			if (chkPages.Checked) {
				litMessage.Text += "<p>Imported Pages</p>";
				sitePageList = site.GetFullSiteFileList();

				int iOrder = 0;

				SiteNav navHome = GetHomePage(site);

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
					cp.EditDate = SiteData.CurrentSite.Now;
					cp.EditUserId = SecurityData.CurrentUserGuid;
					cp.NavOrder = iOrder;
					cp.TemplateFile = ddlTemplatePage.SelectedValue;

					ContentPageExport parent = (from c in exSite.ThePages
												where c.ThePage.ContentType == ContentPageType.PageType.ContentEntry
												  && c.ThePage.FileName.ToLower() == impCP.ParentFileName.ToLower()
												select c).FirstOrDefault();

					BasicContentData navParent = null;
					BasicContentData navData = GetFileInfoFromList(site, cp.FileName);

					if (parent != null) {
						cp.Parent_ContentID = parent.NewRootContentID;
						navParent = GetFileInfoFromList(site, parent.ThePage.FileName);
					}

					//if URL exists already, make this become a new version in the current series
					if (navData != null) {
						cp.Root_ContentID = navData.Root_ContentID;

						impCP.ThePage.RetireDate = navData.RetireDate;
						impCP.ThePage.GoLiveDate = navData.GoLiveDate;

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

					cp.RetireDate = impCP.ThePage.RetireDate;
					cp.GoLiveDate = impCP.ThePage.GoLiveDate;

					cp.SavePageEdit();

					iOrder++;
				}
			}


			if (chkPosts.Checked) {
				litMessage.Text += "<p>Imported Posts</p>";
				sitePageList = site.GetFullSiteFileList();

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
					cp.EditDate = SiteData.CurrentSite.Now;
					cp.EditUserId = SecurityData.CurrentUserGuid;
					cp.NavOrder = 10;
					cp.TemplateFile = ddlTemplatePost.SelectedValue;

					BasicContentData navData = GetFileInfoFromList(site, cp.FileName);

					//if URL exists already, make this become a new version in the current series
					if (navData != null) {
						cp.Root_ContentID = navData.Root_ContentID;

						impCP.ThePage.RetireDate = navData.RetireDate;
						impCP.ThePage.GoLiveDate = navData.GoLiveDate;
					}

					cp.RetireDate = impCP.ThePage.RetireDate;
					cp.GoLiveDate = impCP.ThePage.GoLiveDate;

					cp.SavePageEdit();
				}

				using (ContentPageHelper cph = new ContentPageHelper()) {
					cph.BulkBlogFileNameUpdateFromDate(site.SiteID);
				}
			}

			if (chkComments.Checked) {
				litMessage.Text += "<p>Imported Comments</p>";
				sitePageList = site.GetFullSiteFileList();

				foreach (var impCP in (from c in exSite.TheComments
									   orderby c.TheComment.CreateDate
									   select c).ToList()) {

					int iCommentCount = -1;
					PostComment pc = impCP.TheComment;
					BasicContentData navData = GetFileInfoFromList(site, pc.FileName);
					if (navData != null) {
						pc.Root_ContentID = navData.Root_ContentID;
						pc.ContentCommentID = Guid.NewGuid();

						iCommentCount = PostComment.GetCommentCountByContent(site.SiteID, pc.Root_ContentID, pc.CreateDate, pc.CommenterIP, pc.PostCommentText);
						if (iCommentCount < 1) {
							iCommentCount = PostComment.GetCommentCountByContent(site.SiteID, pc.Root_ContentID, pc.CreateDate, pc.CommenterIP);
						}

						if (iCommentCount < 1) {
							pc.Save();
						}
					}
				}
			}

		}



	}
}