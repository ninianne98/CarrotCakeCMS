using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class BlogPostAddEdit : AdminBasePage {
		public Guid guidContentID = Guid.Empty;
		public Guid guidRootContentID = Guid.Empty;
		public Guid guidVersionContentID = Guid.Empty;

		public Guid guidImportContentID = Guid.Empty;

		public bool bLocked = false;
		private string sPageMode = String.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogContentAdd);

			RedirectIfNoSite();

			lblUpdated.Text = SiteData.CurrentSite.Now.ToString();
			lblCreateDate.Text = SiteData.CurrentSite.Now.ToString();

			guidContentID = GetGuidIDFromQuery();
			guidVersionContentID = GetGuidParameterFromQuery("versionid");
			guidImportContentID = GetGuidParameterFromQuery("importid");

			var site = SiteData.CurrentSite;
			phTrackback1.Visible = (site.AcceptTrackbacks || site.SendTrackbacks);
			phTrackback2.Visible = phTrackback1.Visible;

			sPageMode = GetStringParameterFromQuery("mode");
			if (SiteData.IsRawMode(sPageMode)) {
				reBody.CssClass = "rawEditor";
				reLeftBody.CssClass = "rawEditor";
				reRightBody.CssClass = "rawEditor";
				//divCenter.Visible = false;
				//divRight.Visible = false;
				//divLeft.Visible = false;
			}

			if (!IsPostBack) {
				DateTime dtSite = CMSConfigHelper.CalcNearestFiveMinTime(SiteData.CurrentSite.Now);

				ucReleaseDate.SetDate(dtSite);
				ucRetireDate.SetDate(dtSite.AddYears(200));

				hdnRootID.Value = Guid.Empty.ToString();

				GeneralUtilities.BindList(listCats, SiteData.CurrentSite.GetCategoryList().OrderBy(x => x.CategoryText));
				GeneralUtilities.BindList(listTags, SiteData.CurrentSite.GetTagList().OrderBy(x => x.TagText));

				ContentPage pageContents = null;
				if (guidVersionContentID != Guid.Empty) {
					pageContents = pageHelper.GetVersion(SiteID, guidVersionContentID);
				}
				if (guidContentID != Guid.Empty && pageContents == null) {
					pageContents = pageHelper.FindContentByID(SiteID, guidContentID);
				}

				if (guidImportContentID != Guid.Empty) {
					ContentPageExport cpe = ContentImportExportUtils.GetSerializedContentPageExport(guidImportContentID);

					if (cpe != null) {
						pageContents = cpe.ThePage;
						pageContents.EditDate = SiteData.CurrentSite.Now;
						pageContents.Parent_ContentID = null;
					}

					var rp = pageHelper.GetLatestContentByURL(SiteID, false, pageContents.FileName);
					if (rp != null) {
						pageContents.Root_ContentID = rp.Root_ContentID;
						pageContents.ContentID = rp.ContentID;
					} else {
						pageContents.Root_ContentID = Guid.Empty;
						pageContents.ContentID = Guid.Empty;
					}
					pageContents.Parent_ContentID = null;
					pageContents.NavOrder = SiteData.BlogSortOrderNumber;
				}

				//if (pageContents == null) {
				//    pageContents = new ContentPage(SiteData.CurrentSiteID, ContentPageType.PageType.BlogEntry);
				//}

				List<ContentPage> lstContent = pageHelper.GetAllLatestContentList(SiteID);

				GeneralUtilities.BindList(ddlTemplate, cmsHelper.Templates);

				chkDraft.Visible = false;
				divEditing.Visible = false;

				Dictionary<string, float> dictTemplates = pageHelper.GetPopularTemplateList(SiteID, ContentPageType.PageType.BlogEntry);
				if (dictTemplates.Any()) {
					try {
						GeneralUtilities.SelectListValue(ddlTemplate, dictTemplates.First().Key);
					} catch { }
				}

				if (pageContents == null) {
					btnDeleteButton.Visible = false;
				}

				if (pageContents != null) {
					bool bRet = pageHelper.RecordPageLock(pageContents.Root_ContentID, SiteData.CurrentSite.SiteID, SecurityData.CurrentUserGuid);

					if (pageContents.ContentType != ContentPageType.PageType.BlogEntry) {
						Response.Redirect(SiteFilename.PageAddEditURL + "?id=" + Request.QueryString.ToString());
					}

					cmsHelper.OverrideKey(pageContents.Root_ContentID);
					cmsHelper.cmsAdminContent = pageContents;
					cmsHelper.cmsAdminWidget = pageContents.GetWidgetList();

					BindTextDataGrid();

					guidRootContentID = pageContents.Root_ContentID;
					hdnRootID.Value = guidRootContentID.ToString();

					txtOldFile.Text = pageContents.FileName;

					if (guidImportContentID != Guid.Empty) {
						txtOldFile.Text = "";
					}

					Dictionary<string, string> dataVersions = (from v in pageHelper.GetVersionHistory(SiteID, pageContents.Root_ContentID)
															   join u in ExtendedUserData.GetUserList() on v.EditUserId equals u.UserId
															   orderby v.EditDate descending
															   select new KeyValuePair<string, string>(v.ContentID.ToString(), string.Format("{0} ({1}) {2}", v.EditDate, u.UserName, (v.IsLatestVersion ? " [**] " : " ")))
																).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					GeneralUtilities.BindListDefaultText(ddlVersions, dataVersions, null, "Page Versions", "00000");

					bLocked = pageHelper.IsPageLocked(pageContents);

					pnlHB.Visible = !bLocked;
					pnlButtons.Visible = !bLocked;
					divEditing.Visible = bLocked;
					chkDraft.Visible = !bLocked;
					pnlHBEmpty.Visible = bLocked;

					if (bLocked && pageContents.Heartbeat_UserId != null) {
						MembershipUser usr = SecurityData.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
						litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.";
					}

					txtTitle.Text = pageContents.TitleBar;
					txtNav.Text = pageContents.NavMenuText;
					txtHead.Text = pageContents.PageHead;
					txtPageSlug.Text = pageContents.PageSlug;
					txtThumb.Text = pageContents.Thumbnail;

					txtDescription.Text = pageContents.MetaDescription;
					txtKey.Text = pageContents.MetaKeyword;

					lblUpdated.Text = pageContents.EditDate.ToString();
					lblCreateDate.Text = pageContents.CreateDate.ToString();

					reBody.Text = pageContents.PageText;
					reLeftBody.Text = pageContents.LeftPageText;
					reRightBody.Text = pageContents.RightPageText;

					chkActive.Checked = pageContents.PageActive;
					chkHide.Checked = pageContents.BlockIndex;

					GeneralUtilities.BindDataBoundControl(gvTracks, pageContents.GetTrackbacks());

					ucReleaseDate.SetDate(pageContents.GoLiveDate);
					ucRetireDate.SetDate(pageContents.RetireDate);

					if (pageContents.CreditUserId.HasValue) {
						var usr = new ExtendedUserData(pageContents.CreditUserId.Value);
						hdnCreditUserID.Value = usr.UserName;
						txtSearchUser.Text = string.Format("{0} ({1})", usr.UserName, usr.EmailAddress);
					}

					pageContents.Parent_ContentID = null;

					GeneralUtilities.SelectListValue(ddlTemplate, pageContents.TemplateFile.ToLowerInvariant());

					GeneralUtilities.SelectListValues(listTags, pageContents.ContentTags.Cast<IContentMetaInfo>().Select(x => x.ContentMetaInfoID.ToString()).ToList());
					GeneralUtilities.SelectListValues(listCats, pageContents.ContentCategories.Cast<IContentMetaInfo>().Select(x => x.ContentMetaInfoID.ToString()).ToList());
				}
			}

			SetBlankText(reBody);
			SetBlankText(reLeftBody);
			SetBlankText(reRightBody);

			if (ddlVersions.Items.Count < 1) {
				pnlReview.Visible = false;
			}
		}

		private void BindTextDataGrid() {
			if (cmsHelper.cmsAdminWidget != null) {
				var ww1 = (from w in cmsHelper.cmsAdminWidget
						   where w.IsLatestVersion == true
						   && w.ControlPath.StartsWith("CLASS:Carrotware.CMS.UI.Controls.ContentRichText,")
						   select w);

				gvHtmControls.DataSource = ww1;
				gvHtmControls.DataBind();

				var ww2 = (from w in cmsHelper.cmsAdminWidget
						   where w.IsLatestVersion == true
						   && w.ControlPath.StartsWith("CLASS:Carrotware.CMS.UI.Controls.ContentPlainText,")
						   select w);

				gvTxtControls.DataSource = ww2;
				gvTxtControls.DataBind();
			}
		}

		protected void btnDelete_Click(object sender, EventArgs e) {
			guidRootContentID = new Guid(hdnRootID.Value);

			using (ContentPageHelper cph = new ContentPageHelper()) {
				cph.RemoveContent(SiteID, guidRootContentID);
			}

			Response.Redirect(SiteFilename.BlogPostIndexURL);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			SavePage(false);
		}

		protected void btnSaveVisit_Click(object sender, EventArgs e) {
			SavePage(true);
		}

		protected void SavePage(bool bRedirect) {
			ContentPage pageContents = null;
			if (guidVersionContentID != Guid.Empty) {
				pageContents = pageHelper.GetVersion(SiteID, guidVersionContentID);
			}
			if (guidContentID != Guid.Empty && pageContents == null) {
				pageContents = pageHelper.FindContentByID(SiteID, guidContentID);
			}
			if (guidImportContentID != Guid.Empty) {
				pageContents = ContentImportExportUtils.GetSerializedContentPageExport(guidImportContentID).ThePage;

				if (pageContents != null) {
					pageContents.SiteID = SiteID;
					pageContents.EditDate = SiteData.CurrentSite.Now;
					pageContents.CreateUserId = SecurityData.CurrentUserGuid;
					pageContents.CreateDate = SiteData.CurrentSite.Now;

					var rp = pageHelper.GetLatestContentByURL(SiteID, false, pageContents.FileName);
					if (rp != null) {
						pageContents.Root_ContentID = rp.Root_ContentID;
						pageContents.ContentID = rp.ContentID;
					} else {
						pageContents.Root_ContentID = Guid.Empty;
						pageContents.ContentID = Guid.Empty;
					}
					pageContents.Parent_ContentID = null;
					pageContents.NavOrder = SiteData.BlogSortOrderNumber;
				}
			}

			if (pageContents == null) {
				pageContents = new ContentPage(SiteData.CurrentSiteID, ContentPageType.PageType.BlogEntry);
			}

			DateTime dtSite = CMSConfigHelper.CalcNearestFiveMinTime(SiteData.CurrentSite.Now);
			pageContents.GoLiveDate = dtSite.AddMinutes(-5);
			pageContents.RetireDate = dtSite.AddYears(200);

			pageContents.IsLatestVersion = true;
			pageContents.Thumbnail = txtThumb.Text;

			pageContents.TemplateFile = ddlTemplate.SelectedValue;

			pageContents.TitleBar = txtTitle.Text;
			pageContents.NavMenuText = txtNav.Text;
			pageContents.PageHead = txtHead.Text;
			pageContents.PageSlug = txtPageSlug.Text;

			pageContents.MetaDescription = txtDescription.Text;
			pageContents.MetaKeyword = txtKey.Text;

			pageContents.EditDate = SiteData.CurrentSite.Now;
			pageContents.NavOrder = SiteData.BlogSortOrderNumber;

			pageContents.PageText = reBody.Text;
			pageContents.LeftPageText = reLeftBody.Text;
			pageContents.RightPageText = reRightBody.Text;

			pageContents.PageActive = chkActive.Checked;
			pageContents.ShowInSiteNav = false;
			pageContents.ShowInSiteMap = false;
			pageContents.BlockIndex = chkHide.Checked;

			pageContents.ContentType = ContentPageType.PageType.BlogEntry;

			pageContents.Parent_ContentID = null;

			if (String.IsNullOrEmpty(hdnCreditUserID.Value)) {
				pageContents.CreditUserId = null;
			} else {
				var usr = new ExtendedUserData(hdnCreditUserID.Value);
				pageContents.CreditUserId = usr.UserId;
			}

			pageContents.GoLiveDate = ucReleaseDate.GetDate();
			pageContents.RetireDate = ucRetireDate.GetDate();

			pageContents.EditUserId = SecurityData.CurrentUserGuid;

			pageContents.NewTrackBackURLs = txtTrackback.Text;

			List<ContentCategory> lstCat = new List<ContentCategory>();
			List<ContentTag> lstTag = new List<ContentTag>();

			lstCat = (from cr in GeneralUtilities.GetSelectedValues(listCats).Select(x => new Guid(x))
					  join l in SiteData.CurrentSite.GetCategoryList() on cr equals l.ContentCategoryID
					  select l).ToList();

			lstTag = (from cr in GeneralUtilities.GetSelectedValues(listTags).Select(x => new Guid(x))
					  join l in SiteData.CurrentSite.GetTagList() on cr equals l.ContentTagID
					  select l).ToList();

			pageContents.ContentCategories = lstCat;
			pageContents.ContentTags = lstTag;

			if (!chkDraft.Checked) {
				pageContents.SavePageEdit();
			} else {
				pageContents.SavePageAsDraft();
			}

			//if importing, copy in all meta data possible
			if (guidImportContentID != Guid.Empty) {
				List<Widget> widgets = ContentImportExportUtils.GetSerializedContentPageExport(guidImportContentID).ThePageWidgets;

				foreach (var wd in widgets) {
					wd.Root_ContentID = pageContents.Root_ContentID;
					wd.Root_WidgetID = Guid.NewGuid();
					wd.WidgetDataID = Guid.NewGuid();
					wd.IsPendingChange = true;
					wd.EditDate = SiteData.CurrentSite.Now;
					wd.Save();
				}

				ContentImportExportUtils.RemoveSerializedExportData(guidImportContentID);
			}

			cmsHelper.OverrideKey(pageContents.Root_ContentID);
			if (cmsHelper.cmsAdminWidget != null) {
				var ww = (from w in cmsHelper.cmsAdminWidget
						  where w.IsLatestVersion == true
						  && w.IsPendingChange == true
						  && (w.ControlPath.StartsWith("CLASS:Carrotware.CMS.UI.Controls.ContentRichText,")
						  || w.ControlPath.StartsWith("CLASS:Carrotware.CMS.UI.Controls.ContentPlainText,"))
						  select w);

				foreach (Widget w in ww) {
					w.Save();
				}
			}

			cmsHelper.cmsAdminContent = null;
			cmsHelper.cmsAdminWidget = null;

			if (pageContents.FileName.ToLowerInvariant().EndsWith(SiteData.DefaultDirectoryFilename)) {
				VirtualDirectory.RegisterRoutes(true);
			}

			if (!bRedirect) {
				if (sPageMode.Length < 1) {
					Response.Redirect(SiteData.CurrentScriptName + "?id=" + pageContents.Root_ContentID.ToString());
				} else {
					Response.Redirect(SiteData.CurrentScriptName + "?mode=" + SiteData.RawMode + "&id=" + pageContents.Root_ContentID.ToString());
				}
			} else {
				Response.Redirect(pageContents.FileName);
			}
		}
	}
}