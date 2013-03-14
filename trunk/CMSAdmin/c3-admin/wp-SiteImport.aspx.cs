using System;
using System.Collections.Generic;
using System.IO;
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
	public partial class wp_SiteImport : AdminBasePage {

		public Guid guidImportID = Guid.Empty;
		WordPressSite wpSite = null;
		private int iPageCount = 0;

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["importid"])) {
				guidImportID = new Guid(Request.QueryString["importid"].ToString());
			}

			iPageCount = pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.ContentEntry);

			litTrust.Visible = false;
			if (SiteData.CurrentTrustLevel != AspNetHostingPermissionLevel.Unrestricted) {
				chkFileGrab.Checked = false;
				chkFileGrab.Enabled = false;
				litTrust.Visible = true;
			}

			litMessage.Text = "";

			if (guidImportID != Guid.Empty) {
				wpSite = ContentImportExportUtils.GetSerializedWPExport(guidImportID);

				litName.Text = wpSite.SiteTitle;
				litDescription.Text = wpSite.SiteDescription;
				litImportSource.Text = wpSite.ImportSource;
				litWXR.Text = wpSite.wxrVersion;
				litDate.Text = wpSite.ExtractDate.ToString();

				if (!IsPostBack) {

					BuildFolderList();

					gvPages.DataSource = (from c in wpSite.Content
										  where c.PostType == WordPressPost.WPPostType.Page
										  orderby c.PostOrder
										  select c).ToList();
					gvPages.DataBind();


					gvPosts.DataSource = (from c in wpSite.Content
										  where c.PostType == WordPressPost.WPPostType.BlogPost
										  orderby c.PostDateUTC
										  select c).ToList();
					gvPosts.DataBind();

					GeneralUtilities.BindList(ddlTemplatePage, cmsHelper.Templates);
					GeneralUtilities.BindList(ddlTemplatePost, cmsHelper.Templates);

					lblPages.Text = gvPages.Rows.Count.ToString();
					lblPosts.Text = gvPosts.Rows.Count.ToString();

					SetDDLDefaultTemplates();
				}
			}
		}

		protected void SetDDLDefaultTemplates() {
			float iThird = (float)(iPageCount - 1) / (float)3;
			Dictionary<string, float> dictTemplates = null;

			dictTemplates = pageHelper.GetPopularTemplateList(SiteID, ContentPageType.PageType.ContentEntry);
			if (dictTemplates.Count > 0 && dictTemplates.First().Value >= iThird) {
				try { ddlTemplatePage.SelectedValue = dictTemplates.First().Key; } catch { }
			}

			dictTemplates = pageHelper.GetPopularTemplateList(SiteID, ContentPageType.PageType.BlogEntry);
			if (dictTemplates.Count > 0) {
				try { ddlTemplatePost.SelectedValue = dictTemplates.First().Key; } catch { }
			}
		}

		protected void GrabAttachments(WordPressPost wpPage) {
			if (chkFileGrab.Checked) {

				int iPost = wpPage.PostID;

				List<WordPressPost> lstA = (from a in wpSite.Content
											where a.PostType == WordPressPost.WPPostType.Attachment
											&& a.ParentPostID == iPost
											select a).Distinct().ToList();

				lstA.ToList().ForEach(q => q.ImportFileSlug = ddlFolders.SelectedValue + "/" + q.ImportFileSlug);
				lstA.ToList().ForEach(q => q.ImportFileSlug = q.ImportFileSlug.Replace("//", "/").Replace("//", "/"));

				foreach (var img in lstA) {
					img.ImportFileSlug = img.ImportFileSlug.Replace("//", "/").Replace("//", "/");

					cmsHelper.GetFile(img.AttachmentURL, img.ImportFileSlug);
					string sURL1 = img.AttachmentURL.Substring(img.AttachmentURL.IndexOf("://") + 3);
					string sURLLess = sURL1.Substring(sURL1.IndexOf("/"));

					wpPage.PostContent = wpPage.PostContent.Replace(img.AttachmentURL, img.ImportFileSlug);
				}

			}
		}

		protected void BuildFolderList() {
			List<FileData> lstFolders = new List<FileData>();

			string sRoot = Server.MapPath("~/");

			string[] subdirs;
			try {
				subdirs = Directory.GetDirectories(sRoot);
			} catch {
				subdirs = null;
			}

			if (subdirs != null) {
				foreach (string theDir in subdirs) {
					string w = FileDataHelper.MakeWebFolderPath(theDir);
					lstFolders.Add(new FileData { FileName = w, FolderPath = w, FileDate = DateTime.Now });
				}
			}

			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith(SiteData.AdminFolderPath));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/bin/"));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/obj/"));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/app_data/"));

			GeneralUtilities.BindListDefaultText(ddlFolders, lstFolders.OrderBy(f => f.FileName), null, "Folders", "-[none]-");
		}


		protected void RepairBody(WordPressPost wpp) {
			wpp.CleanBody();

			if (chkFixBodies.Checked) {
				wpp.RepairBody();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			try {
				ImportStuff();
			} catch (Exception ex) {
				litMessage.Text += ex.ToString();
			}
		}


		private void ImportStuff() {
			SiteData.CurrentSite = null;

			SiteData site = SiteData.CurrentSite;

			litMessage.Text = "<p>No Items Selected For Import</p>";

			if (chkSite.Checked || chkPages.Checked || chkPosts.Checked) {
				List<string> tags = site.GetTagList().Select(x => x.TagSlug.ToLower()).ToList();
				List<string> cats = site.GetCategoryList().Select(x => x.CategorySlug.ToLower()).ToList();

				wpSite.Tags.RemoveAll(x => tags.Contains(x.InfoKey.ToLower()));
				wpSite.Categories.RemoveAll(x => cats.Contains(x.InfoKey.ToLower()));

				litMessage.Text = "<p>Imported Tags and Categories</p>";

				List<ContentTag> lstTag = (from l in wpSite.Tags.Distinct()
										   select new ContentTag {
											   ContentTagID = Guid.NewGuid(),
											   IsPublic = true,
											   SiteID = site.SiteID,
											   TagSlug = l.InfoKey,
											   TagText = l.InfoLabel
										   }).Distinct().ToList();

				List<ContentCategory> lstCat = (from l in wpSite.Categories.Distinct()
												select new ContentCategory {
													ContentCategoryID = Guid.NewGuid(),
													IsPublic = true,
													SiteID = site.SiteID,
													CategorySlug = l.InfoKey,
													CategoryText = l.InfoLabel
												}).Distinct().ToList();

				foreach (var v in lstTag) {
					v.Save();
				}
				foreach (var v in lstCat) {
					v.Save();
				}

			}


			if (chkSite.Checked) {
				litMessage.Text += "<p>Updated Site Name</p>";
				site.SiteName = wpSite.SiteTitle;
				site.SiteTagline = wpSite.SiteDescription;
				site.Save();
			}

			wpSite.Comments.ForEach(r => r.ImportRootID = Guid.Empty);

			using (SiteNavHelper navHelper = new SiteNavHelper()) {

				if (chkPages.Checked) {

					litMessage.Text += "<p>Imported Pages</p>";

					int iOrder = 0;
					SiteNav navHome = navHelper.FindHome(site.SiteID, false);
					if (navHome != null) {
						iOrder = 2;
					}

					foreach (var wpp in (from c in wpSite.Content
										 where c.PostType == WordPressPost.WPPostType.Page
										 orderby c.PostOrder, c.PostTitle
										 select c).ToList()) {

						GrabAttachments(wpp);
						RepairBody(wpp);

						ContentPage cp = ContentImportExportUtils.CreateWPContentPage(site, wpp);
						cp.SiteID = site.SiteID;
						cp.ContentType = ContentPageType.PageType.ContentEntry;
						cp.EditDate = SiteData.CurrentSite.Now;
						cp.EditUserId = SecurityData.CurrentUserGuid;
						cp.NavOrder = iOrder;
						cp.TemplateFile = ddlTemplatePage.SelectedValue;

						WordPressPost parent = (from c in wpSite.Content
												where c.PostType == WordPressPost.WPPostType.Page
												  && c.PostID == wpp.ParentPostID
												select c).FirstOrDefault();

						SiteNav navParent = null;

						SiteNav navData = navHelper.GetLatestVersion(site.SiteID, false, cp.FileName.ToLower());
						if (parent != null) {
							navParent = navHelper.GetLatestVersion(site.SiteID, false, parent.ImportFileName.ToLower());
						}

						//if URL exists already, make this become a new version in the current series
						if (navData != null) {
							cp.Root_ContentID = navData.Root_ContentID;
							if (navData.NavOrder == 0) {
								cp.NavOrder = 0;
							}
						}

						if (navParent != null) {
							cp.Parent_ContentID = navParent.Root_ContentID;
						} else {
							if (parent != null) {
								cp.Parent_ContentID = parent.ImportRootID;
							}
						}
						//preserve homepage
						if (navHome != null && navHome.FileName.ToLower() == cp.FileName.ToLower()) {
							cp.NavOrder = 0;
						}

						cp.RetireDate = CalcNearestFiveMinTime(cp.CreateDate).AddYears(200);
						cp.GoLiveDate = CalcNearestFiveMinTime(cp.CreateDate).AddMinutes(-5);

						//if URL exists already, make this become a new version in the current series
						if (navData != null) {
							cp.Root_ContentID = navData.Root_ContentID;
							cp.RetireDate = navData.RetireDate;
							cp.GoLiveDate = navData.GoLiveDate;
						}

						cp.SavePageEdit();

						wpSite.Comments.Where(x => x.PostID == wpp.PostID).ToList().ForEach(r => r.ImportRootID = cp.Root_ContentID);

						iOrder++;
					}
				}

				if (chkPosts.Checked) {
					litMessage.Text += "<p>Imported Posts</p>";

					foreach (var wpp in (from c in wpSite.Content
										 where c.PostType == WordPressPost.WPPostType.BlogPost
										 orderby c.PostOrder
										 select c).ToList()) {

						GrabAttachments(wpp);
						RepairBody(wpp);

						ContentPage cp = ContentImportExportUtils.CreateWPContentPage(site, wpp);
						cp.SiteID = site.SiteID;
						cp.Parent_ContentID = null;
						cp.ContentType = ContentPageType.PageType.BlogEntry;
						cp.EditDate = SiteData.CurrentSite.Now;
						cp.EditUserId = SecurityData.CurrentUserGuid;
						cp.NavOrder = 10;
						cp.TemplateFile = ddlTemplatePost.SelectedValue;

						SiteNav navData = navHelper.GetLatestVersion(site.SiteID, false, cp.FileName.ToLower());

						cp.RetireDate = CalcNearestFiveMinTime(cp.CreateDate).AddYears(200);
						cp.GoLiveDate = CalcNearestFiveMinTime(cp.CreateDate).AddMinutes(-5);

						//if URL exists already, make this become a new version in the current series
						if (navData != null) {
							cp.Root_ContentID = navData.Root_ContentID;
							cp.RetireDate = navData.RetireDate;
							cp.GoLiveDate = navData.GoLiveDate;
						}

						cp.SavePageEdit();

						wpSite.Comments.Where(x => x.PostID == wpp.PostID).ToList().ForEach(r => r.ImportRootID = cp.Root_ContentID);
					}

					using (ContentPageHelper cph = new ContentPageHelper()) {
						cph.BulkBlogFileNameUpdateFromDate(site.SiteID);
					}

				}
			}


			wpSite.Comments.RemoveAll(r => r.ImportRootID == Guid.Empty);

			if (wpSite.Comments.Count > 0) {
				litMessage.Text += "<p>Imported Comments</p>";
			}

			foreach (WordPressComment wpc in wpSite.Comments) {
				int iCommentCount = -1;

				iCommentCount = PostComment.GetCommentCountByContent(site.SiteID, wpc.ImportRootID, wpc.CommentDateUTC, wpc.AuthorIP, wpc.CommentContent);
				if (iCommentCount < 1) {
					iCommentCount = PostComment.GetCommentCountByContent(site.SiteID, wpc.ImportRootID, wpc.CommentDateUTC, wpc.AuthorIP);
				}

				if (iCommentCount < 1) {
					PostComment pc = new PostComment();
					pc.ContentCommentID = Guid.NewGuid();
					pc.Root_ContentID = wpc.ImportRootID;
					pc.CreateDate = site.ConvertUTCToSiteTime(wpc.CommentDateUTC);
					pc.IsApproved = false;
					pc.IsSpam = false;

					pc.CommenterIP = wpc.AuthorIP;
					pc.CommenterName = wpc.Author;
					pc.CommenterEmail = wpc.AuthorEmail;
					pc.PostCommentText = wpc.CommentContent;
					pc.CommenterURL = wpc.AuthorURL;

					if (wpc.Approved == "1") {
						pc.IsApproved = true;
					}
					if (wpc.Approved.ToLower() == "trash") {
						pc.IsSpam = true;
					}
					if (wpc.Type.ToLower() == "trackback" || wpc.Type.ToLower() == "pingback") {
						pc.CommenterEmail = wpc.Type;
					}

					pc.Save();
				}
			}

		}

	}
}