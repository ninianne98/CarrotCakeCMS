using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
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
	public partial class PageAddEdit : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		public Guid guidRootContentID = Guid.Empty;
		public Guid guidVersionContentID = Guid.Empty;

		public Guid guidImportContentID = Guid.Empty;

		public bool bLocked = false;
		string sPageMode = String.Empty;

		private int iPageCount = 0;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.ContentAdd);

			RedirectIfNoSite();

			lblUpdated.Text = SiteData.CurrentSite.Now.ToString();
			lblCreateDate.Text = SiteData.CurrentSite.Now.ToString();

			iPageCount = pageHelper.GetSitePageCount(SiteID, ContentPageType.PageType.ContentEntry);

			guidContentID = GetGuidIDFromQuery();
			guidVersionContentID = GetGuidParameterFromQuery("versionid");
			guidImportContentID = GetGuidParameterFromQuery("importid");

			if (iPageCount < 1) {
				txtSort.Text = "0";
			}

			if (!string.IsNullOrEmpty(Request.QueryString["mode"])) {
				sPageMode = Request.QueryString["mode"].ToString();
				if (sPageMode.ToLower() == "raw") {
					reBody.CssClass = "rawEditor";
					reLeftBody.CssClass = "rawEditor";
					reRightBody.CssClass = "rawEditor";
					divCenter.Visible = false;
					divRight.Visible = false;
					divLeft.Visible = false;
				}
			}

			if (!IsPostBack) {
				DateTime dtSite = CalcNearestFiveMinTime(SiteData.CurrentSite.Now);
				txtReleaseDate.Text = dtSite.ToShortDateString();
				txtReleaseTime.Text = dtSite.ToShortTimeString();
				txtRetireDate.Text = dtSite.AddYears(200).ToShortDateString();
				txtRetireTime.Text = dtSite.AddYears(200).ToShortTimeString();

				ParentPagePicker.RootContentID = Guid.Empty;

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

						ContentPage par = pageHelper.FindByFilename(SiteID, cpe.ParentFileName);
						if (par != null) {
							pageContents.Parent_ContentID = par.Root_ContentID;
						} else {
							pageContents.Parent_ContentID = null;
						}
					}
				}

				//if (pageContents == null) {
				//    pageContents = new ContentPage(SiteData.CurrentSiteID, ContentPageType.PageType.ContentEntry);
				//}

				List<ContentPage> lstContent = pageHelper.GetAllLatestContentList(SiteID);

				GeneralUtilities.BindList(ddlTemplate, cmsHelper.Templates);

				chkDraft.Visible = false;
				divEditing.Visible = false;

				float iThird = (float)(iPageCount - 1) / (float)3;

				Dictionary<string, float> dictTemplates = pageHelper.GetPopularTemplateList(SiteID, ContentPageType.PageType.ContentEntry);
				if (dictTemplates.Count > 0 && dictTemplates.First().Value >= iThird) {
					try {
						GeneralUtilities.SelectListValue(ddlTemplate, dictTemplates.First().Key);
					} catch { }
				}

				if (pageContents != null) {
					if (pageContents.ContentType != ContentPageType.PageType.ContentEntry) {
						Response.Redirect(SiteFilename.BlogPostAddEditURL + "?id=" + Request.QueryString.ToString());
					}

					guidRootContentID = pageContents.Root_ContentID;

					txtOldFile.Text = pageContents.FileName;

					if (guidImportContentID != Guid.Empty) {
						txtOldFile.Text = "";
					}

					List<ContentPage> lstVer = pageHelper.GetVersionHistory(SiteID, pageContents.Root_ContentID);

					var dataVersions = (from v in lstVer
										orderby v.EditDate descending
										select new {
											EditDate = v.EditDate.ToString() + (v.IsLatestVersion ? " [**]" : " "),
											ContentID = v.ContentID
										}).ToList();

					GeneralUtilities.BindListDefaultText(ddlVersions, dataVersions, null, "Page Versions", "00000");

					bLocked = pageHelper.IsPageLocked(pageContents);

					pnlHB.Visible = !bLocked;
					pnlButtons.Visible = !bLocked;
					divEditing.Visible = bLocked;
					chkDraft.Visible = !bLocked;
					pnlHBEmpty.Visible = bLocked;

					if (bLocked && pageContents.Heartbeat_UserId != null) {
						var usr = SecurityData.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
						litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.";
					}

					txtTitle.Text = pageContents.TitleBar;
					txtNav.Text = pageContents.NavMenuText;
					txtHead.Text = pageContents.PageHead;
					txtFileName.Text = pageContents.FileName;
					txtThumb.Text = pageContents.Thumbnail;

					txtDescription.Text = pageContents.MetaDescription;
					txtKey.Text = pageContents.MetaKeyword;

					txtSort.Text = pageContents.NavOrder.ToString();

					lblUpdated.Text = pageContents.EditDate.ToString();
					lblCreateDate.Text = pageContents.CreateDate.ToString();

					reBody.Text = pageContents.PageText;
					reLeftBody.Text = pageContents.LeftPageText;
					reRightBody.Text = pageContents.RightPageText;

					chkActive.Checked = pageContents.PageActive;
					chkNavigation.Checked = pageContents.ShowInSiteNav;
					chkSiteMap.Checked = pageContents.ShowInSiteMap;
					chkHide.Checked = pageContents.BlockIndex;

					GeneralUtilities.BindDataBoundControl(gvTracks, pageContents.GetTrackbacks());

					txtReleaseDate.Text = pageContents.GoLiveDate.ToShortDateString();
					txtReleaseTime.Text = pageContents.GoLiveDate.ToShortTimeString();
					txtRetireDate.Text = pageContents.RetireDate.ToShortDateString();
					txtRetireTime.Text = pageContents.RetireDate.ToShortTimeString();

					if (pageContents.Parent_ContentID.HasValue) {
						ParentPagePicker.SelectedPage = pageContents.Parent_ContentID.Value;
					}

					GeneralUtilities.SelectListValue(ddlTemplate, pageContents.TemplateFile.ToLower());

				}
			}

			ParentPagePicker.RootContentID = guidRootContentID;

			SetBlankText(reBody);
			SetBlankText(reLeftBody);
			SetBlankText(reRightBody);

			if (ddlVersions.Items.Count < 1) {
				pnlReview.Visible = false;
			}
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
				}
			}

			if (pageContents == null) {
				pageContents = new ContentPage(SiteData.CurrentSiteID, ContentPageType.PageType.ContentEntry);
			}

			DateTime dtSite = CalcNearestFiveMinTime(SiteData.CurrentSite.Now);
			pageContents.GoLiveDate = dtSite.AddMinutes(-5);
			pageContents.RetireDate = dtSite.AddYears(200);

			pageContents.IsLatestVersion = true;
			pageContents.Thumbnail = txtThumb.Text;

			pageContents.TemplateFile = ddlTemplate.SelectedValue;

			pageContents.TitleBar = txtTitle.Text;
			pageContents.NavMenuText = txtNav.Text;
			pageContents.PageHead = txtHead.Text;
			pageContents.FileName = txtFileName.Text;

			pageContents.MetaDescription = txtDescription.Text;
			pageContents.MetaKeyword = txtKey.Text;

			pageContents.EditDate = SiteData.CurrentSite.Now;
			pageContents.NavOrder = int.Parse(txtSort.Text);

			pageContents.PageText = reBody.Text;
			pageContents.LeftPageText = reLeftBody.Text;
			pageContents.RightPageText = reRightBody.Text;

			pageContents.PageActive = chkActive.Checked;
			pageContents.ShowInSiteNav = chkNavigation.Checked;
			pageContents.ShowInSiteMap = chkSiteMap.Checked;
			pageContents.BlockIndex = chkHide.Checked;

			pageContents.ContentType = ContentPageType.PageType.ContentEntry;

			if (ParentPagePicker.SelectedPage.HasValue) {
				pageContents.Parent_ContentID = ParentPagePicker.SelectedPage.Value;
			} else {
				pageContents.Parent_ContentID = null;
			}

			pageContents.GoLiveDate = Convert.ToDateTime(txtReleaseDate.Text + " " + txtReleaseTime.Text);
			pageContents.RetireDate = Convert.ToDateTime(txtRetireDate.Text + " " + txtRetireTime.Text);
			pageContents.EditUserId = SecurityData.CurrentUserGuid;

			pageContents.NewTrackBackURLs = txtTrackback.Text;


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
					wd.EditDate = SiteData.CurrentSite.Now;
					wd.Save();
				}

				ContentImportExportUtils.RemoveSerializedExportData(guidImportContentID);
			}

			if (pageContents.FileName.ToLower().EndsWith(SiteData.DefaultDirectoryFilename)) {
				VirtualDirectory.RegisterRoutes(true);
			}

			if (!bRedirect) {
				if (sPageMode.Length < 1) {
					Response.Redirect(SiteData.CurrentScriptName + "?id=" + pageContents.Root_ContentID.ToString());
				} else {
					Response.Redirect(SiteData.CurrentScriptName + "?mode=raw&id=" + pageContents.Root_ContentID.ToString());
				}
			} else {
				Response.Redirect(pageContents.FileName);
			}
		}



	}
}
