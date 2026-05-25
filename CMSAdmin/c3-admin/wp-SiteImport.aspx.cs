using Carrotware.CMS.Core;
using Carrotware.CMS.Security.Models;
using Carrotware.CMS.UI.Controls;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class wp_SiteImport : AdminBasePage {
		public Guid guidImportID = Guid.Empty;
		private WordPressSite wpSite = null;
		private int iPageCount = 0;

		protected void Page_Load(object sender, EventArgs e) {
			guidImportID = GetGuidImportFromQuery();

			iPageCount = pageHelper.GetSitePageCount(this.SiteID, ContentPageType.PageType.ContentEntry);

			litTrust.Visible = false;
			if (SiteData.CurrentTrustLevel != AspNetHostingPermissionLevel.Unrestricted) {
				chkFileGrab.Checked = false;
				chkFileGrab.Enabled = false;
				litTrust.Visible = true;
			}

			litMessage.Text = string.Empty;

			if (guidImportID != Guid.Empty) {
				wpSite = ContentImportExportUtils.GetSerializedWPExport(guidImportID);

				litName.Text = wpSite.SiteTitle;
				litDescription.Text = wpSite.SiteDescription;
				litImportSource.Text = wpSite.ImportSource;
				litWXR.Text = wpSite.wxrVersion;
				litDate.Text = wpSite.ExtractDate.ToString();

				if (!IsPostBack) {
					BuildFolderList();

					BindData();
				}
			}
		}

		private void BindData() {
			GeneralUtilities.BindDataBoundControl(gvPages, wpSite.ContentPages);
			GeneralUtilities.BindDataBoundControl(gvPosts, wpSite.ContentPosts);

			GeneralUtilities.BindList(ddlTemplatePage, cmsHelper.Templates);
			GeneralUtilities.BindList(ddlTemplatePost, cmsHelper.Templates);

			lblPages.Text = gvPages.Rows.Count.ToString();
			lblPosts.Text = gvPosts.Rows.Count.ToString();

			SetDDLDefaultTemplates();
		}

		protected void SetDDLDefaultTemplates() {
			float iThird = (float)(iPageCount - 1) / (float)3;
			Dictionary<string, float> dictTemplates = null;

			dictTemplates = pageHelper.GetPopularTemplateList(this.SiteID, ContentPageType.PageType.ContentEntry);
			if (dictTemplates.Any() && dictTemplates.First().Value >= iThird) {
				try { ddlTemplatePage.SelectedValue = dictTemplates.First().Key; } catch { }
			}

			dictTemplates = pageHelper.GetPopularTemplateList(this.SiteID, ContentPageType.PageType.BlogEntry);
			if (dictTemplates.Any()) {
				try { ddlTemplatePost.SelectedValue = dictTemplates.First().Key; } catch { }
			}
		}

		protected void GrabAttachments(WordPressPost wpPage) {
			if (chkFileGrab.Checked) {
				wpPage.GrabAttachments(ddlFolders.SelectedValue, wpSite);
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

			lstFolders.RemoveAll(f => f.FileName.ToLowerInvariant().StartsWith(SiteData.AdminFolderPath));
			lstFolders.RemoveAll(f => f.FileName.ToLowerInvariant().StartsWith("/app_code/"));
			lstFolders.RemoveAll(f => f.FileName.ToLowerInvariant().StartsWith("/app_data/"));
			lstFolders.RemoveAll(f => f.FileName.ToLowerInvariant().StartsWith("/aspnet_client/"));
			lstFolders.RemoveAll(f => f.FileName.ToLowerInvariant().StartsWith("/bin/"));
			lstFolders.RemoveAll(f => f.FileName.ToLowerInvariant().StartsWith("/obj/"));

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

		private void SetMsg(string message) {
			if (!string.IsNullOrEmpty(message)) {
				litMessage.Text = string.Format("<p>{0}</p>", message);
			} else {
				litMessage.Text = string.Empty;
			}
		}

		private void SetMsg(List<string> messages) {
			if (messages != null && messages.Any()) {
				var htmlString = string.Join(Environment.NewLine, messages.Select(x => string.Format("<li>{0}</li>", x)));
				litMessage.Text = "<ul>" + Environment.NewLine + htmlString + Environment.NewLine + "<ul>";
			} else {
				litMessage.Text = string.Empty;
			}
		}

		private void ImportStuff() {
			SiteData.CurrentSite = null;

			var site = SiteData.CurrentSite;

			var lstMsg = new List<string>();
			SetMsg("No Items Selected For Import");

			if (chkSite.Checked || chkPages.Checked || chkPosts.Checked) {
				List<string> tags = site.GetTagList().Select(x => x.TagSlug.ToLowerInvariant()).ToList();
				List<string> cats = site.GetCategoryList().Select(x => x.CategorySlug.ToLowerInvariant()).ToList();

				wpSite.Tags.RemoveAll(x => tags.Contains(x.InfoKey.ToLowerInvariant()));
				wpSite.Categories.RemoveAll(x => cats.Contains(x.InfoKey.ToLowerInvariant()));

				lstMsg.Add("Imported Tags and Categories");

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
			SetMsg(lstMsg);

			if (chkSite.Checked) {
				lstMsg.Add("Updated Site Name");

				site.SiteName = wpSite.SiteTitle;
				site.SiteTagline = wpSite.SiteDescription;
				site.Save();
			}
			SetMsg(lstMsg);

			if (!chkMapAuthor.Checked) {
				wpSite.Authors = new List<WordPressUser>();
			}

			var sd = new SecurityData();

			//itterate author collection and find if in the system
			foreach (WordPressUser wpu in wpSite.Authors) {
				ExtendedUserData usr = null;
				wpu.ImportUserID = Guid.Empty;

				//attempt to find the user in the userbase
				usr = ExtendedUserData.FindByEmail(wpu.Email);
				if (usr != null && usr.UserId != Guid.Empty) {
					wpu.ImportUserID = usr.UserId;
				} else {
					usr = ExtendedUserData.FindByUsername(wpu.Login);
					if (usr != null && usr.UserId != Guid.Empty) {
						wpu.ImportUserID = usr.UserId;
					}
				}

				if (chkAuthors.Checked) {
					if (wpu.ImportUserID == Guid.Empty) {
						var user = new ApplicationUser { UserName = wpu.Login, Email = wpu.Email };
						var nu = sd.CreateApplicationUser(user);
						var result = nu.IdentityResult;

						if (result.Succeeded) {
							user = nu.User;
							var exUser = nu.ExtendedUserData;
							exUser.AddToRole(SecurityData.CMSGroup_Users);
							wpu.ImportUserID = exUser.UserId;
						} else {
							throw new Exception(string.Format("Could not create user: {0} ({1}) \r\n{2}", wpu.Login, wpu.Email, string.Join("\r\n", result.Errors)));
						}
					}

					if (wpu.ImportUserID != Guid.Empty) {
						if (!string.IsNullOrEmpty(wpu.FirstName) || !string.IsNullOrEmpty(wpu.LastName)) {
							var ud = new ExtendedUserData(wpu.ImportUserID);
							ud.FirstName = wpu.FirstName;
							ud.LastName = wpu.LastName;
							ud.Save();
						}
					}
				}
			}

			wpSite.Comments.ForEach(r => r.ImportRootID = Guid.Empty);

			using (ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper()) {
				if (chkPages.Checked) {
					lstMsg.Add("Imported Pages");

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

						ContentPage cp = ContentImportExportUtils.CreateWPContentPage(wpSite, wpp, site);
						cp.SiteID = site.SiteID;
						cp.ContentType = ContentPageType.PageType.ContentEntry;
						cp.EditDate = SiteData.CurrentSite.Now;
						cp.NavOrder = iOrder;
						cp.TemplateFile = ddlTemplatePage.SelectedValue;

						WordPressPost parent = (from c in wpSite.Content
												where c.PostType == WordPressPost.WPPostType.Page
												  && c.PostID == wpp.ParentPostID
												select c).FirstOrDefault();

						SiteNav navParent = null;

						SiteNav navData = navHelper.GetLatestVersion(site.SiteID, false, cp.FileName.ToLowerInvariant());
						if (parent != null) {
							navParent = navHelper.GetLatestVersion(site.SiteID, false, parent.ImportFileName.ToLowerInvariant());
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
						if (navHome != null && navHome.FileName.ToLowerInvariant() == cp.FileName.ToLowerInvariant()) {
							cp.NavOrder = 0;
						}

						cp.RetireDate = CMSConfigHelper.CalcNearestFiveMinTime(cp.CreateDate).AddYears(200);
						cp.GoLiveDate = CMSConfigHelper.CalcNearestFiveMinTime(cp.CreateDate).AddMinutes(-5);

						//if URL exists already, make this become a new version in the current series
						if (navData != null) {
							cp.Root_ContentID = navData.Root_ContentID;
							cp.RetireDate = navData.RetireDate;
							cp.GoLiveDate = navData.GoLiveDate;
						}

						//cp.SavePageEdit();
						//wpp.ImportRootID = cp.Root_ContentID;
						//wpSite.Comments.Where(x => x.PostID == wpp.PostID).ToList().ForEach(r => r.ImportRootID = cp.Root_ContentID);
						//wpp.SavePageEdit(wpSite, cp);
						wpSite.SavePageEdit(wpp, cp);

						iOrder++;
					}
				}

				if (chkPosts.Checked) {
					lstMsg.Add("Imported Posts");

					foreach (var wpp in (from c in wpSite.Content
										 where c.PostType == WordPressPost.WPPostType.BlogPost
										 orderby c.PostOrder
										 select c).ToList()) {
						GrabAttachments(wpp);
						RepairBody(wpp);

						ContentPage cp = ContentImportExportUtils.CreateWPContentPage(wpSite, wpp, site);
						cp.SiteID = site.SiteID;
						cp.Parent_ContentID = null;
						cp.ContentType = ContentPageType.PageType.BlogEntry;
						cp.EditDate = SiteData.CurrentSite.Now;
						cp.NavOrder = SiteData.BlogSortOrderNumber;
						cp.TemplateFile = ddlTemplatePost.SelectedValue;

						SiteNav navData = navHelper.GetLatestVersion(site.SiteID, false, cp.FileName.ToLowerInvariant());

						cp.RetireDate = CMSConfigHelper.CalcNearestFiveMinTime(cp.CreateDate).AddYears(200);
						cp.GoLiveDate = CMSConfigHelper.CalcNearestFiveMinTime(cp.CreateDate).AddMinutes(-5);

						//if URL exists already, make this become a new version in the current series
						if (navData != null) {
							cp.Root_ContentID = navData.Root_ContentID;
							cp.RetireDate = navData.RetireDate;
							cp.GoLiveDate = navData.GoLiveDate;
						}

						//cp.SavePageEdit();
						//wpp.ImportRootID = cp.Root_ContentID;
						//wpSite.Comments.Where(x => x.PostID == wpp.PostID).ToList().ForEach(r => r.ImportRootID = cp.Root_ContentID);
						//wpp.SavePageEdit(wpSite, cp);
						wpSite.SavePageEdit(wpp, cp);
					}

					using (var cph = new ContentPageHelper()) {
						cph.ResolveDuplicateBlogURLs(site.SiteID);
						cph.FixBlogNavOrder(site.SiteID);
					}
				}
			}
			SetMsg(lstMsg);

			wpSite.Comments.RemoveAll(r => r.ImportRootID == Guid.Empty);

			if (wpSite.Comments.Any()) {
				lstMsg.Add("Imported Comments");
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
					if (wpc.Approved.ToLowerInvariant() == "trash") {
						pc.IsSpam = true;
					}
					if (wpc.Type.ToLowerInvariant() == "trackback" || wpc.Type.ToLowerInvariant() == "pingback") {
						pc.CommenterEmail = wpc.Type;
					}

					pc.Save();
				}
			}
			SetMsg(lstMsg);

			BindData();
		}
	}
}