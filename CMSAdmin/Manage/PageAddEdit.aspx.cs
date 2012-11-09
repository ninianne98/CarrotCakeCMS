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

namespace Carrotware.CMS.UI.Admin {
	public partial class PageAddEdit : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		public Guid guidRootContentID = Guid.Empty;
		public Guid guidVersionContentID = Guid.Empty;

		public Guid guidImportContentID = Guid.Empty;

		public bool bLocked = false;
		string sPageMode = String.Empty;
		SiteMapOrder orderHelper = new SiteMapOrder();

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.Content);
			lblUpdated.Text = DateTime.Now.ToString();

			SiteData site = siteHelper.GetCurrentSite();
			if (site == null) {
				Response.Redirect("./Default.aspx");
			}

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidContentID = new Guid(Request.QueryString["id"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["versionid"])) {
				guidVersionContentID = new Guid(Request.QueryString["versionid"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["importid"])) {
				guidImportContentID = new Guid(Request.QueryString["importid"].ToString());
			}

			if (pageHelper.GetSitePageCount(SiteID) < 1) {
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
				ContentPage pageContents = null;
				if (guidVersionContentID != Guid.Empty) {
					pageContents = pageHelper.GetVersion(SiteID, guidVersionContentID);
				}
				if (guidContentID != Guid.Empty && pageContents == null) {
					pageContents = pageHelper.FindContentByID(SiteID, guidContentID);
				}

				if (guidImportContentID != Guid.Empty) {
					ContentPageExport cpe = ContentPageExport.GetSerializedContentPageExport(guidImportContentID);
					if (cpe != null) {
						pageContents = cpe.ThePage;
						pageContents.CreateDate = DateTime.Now;
						pageContents.EditDate = DateTime.Now;

						ContentPage par = pageHelper.FindByFilename(SiteID, cpe.ParentFileName);
						if (par != null) {
							pageContents.Parent_ContentID = par.Root_ContentID;
						} else {
							pageContents.Parent_ContentID = null;
						}
					}
				}

				List<ContentPage> lstContent = pageHelper.GetAllLatestContentList(SiteID);

				ddlTemplate.DataSource = cmsHelper.Templates;
				ddlTemplate.DataBind();

				chkDraft.Visible = false;
				divEditing.Visible = false;

				if (pageContents != null) {

					guidRootContentID = pageContents.Root_ContentID;

					txtOldFile.Text = pageContents.FileName;

					if (guidImportContentID != Guid.Empty) {
						txtOldFile.Text = "";
					}

					var lstVer = pageHelper.GetVersionHistory(SiteID, pageContents.Root_ContentID);

					ddlVersions.DataSource = (from v in lstVer
											  orderby v.EditDate descending
											  select new {
												  EditDate = v.EditDate.ToString() + (v.IsLatestVersion ? " [**]" : " "),
												  ContentID = v.ContentID
											  }).ToList();

					ddlVersions.DataBind();
					ddlVersions.Items.Insert(0, new ListItem("-Page Versions-", "00000"));

					bLocked = pageHelper.IsPageLocked(pageContents);

					pnlHB.Visible = !bLocked;
					pnlButtons.Visible = !bLocked;
					divEditing.Visible = bLocked;
					chkDraft.Visible = !bLocked;

					if (bLocked && pageContents.Heartbeat_UserId != null) {
						var usr = SecurityData.GetUserByGuid(pageContents.Heartbeat_UserId.Value);
						litUser.Text = "Read only mode. User '" + usr.UserName + "' is currently editing the page.";
					}

					txtTitle.Text = pageContents.TitleBar;
					txtNav.Text = pageContents.NavMenuText;
					txtHead.Text = pageContents.PageHead;
					txtFileName.Text = pageContents.FileName;

					txtDescription.Text = pageContents.MetaDescription;
					txtKey.Text = pageContents.MetaKeyword;

					txtSort.Text = pageContents.NavOrder.ToString();

					lblUpdated.Text = pageContents.EditDate.ToString();
					lblCreatDate.Text = pageContents.CreateDate.ToString();

					reBody.Text = pageContents.PageText;
					reLeftBody.Text = pageContents.LeftPageText;
					reRightBody.Text = pageContents.RightPageText;

					chkActive.Checked = Convert.ToBoolean(pageContents.PageActive);

					if (pageContents.Parent_ContentID != null) {
						txtParent.Text = pageContents.Parent_ContentID.ToString();
					}
					if (pageContents.TemplateFile != null) {
						try { ddlTemplate.SelectedValue = pageContents.TemplateFile.ToLower(); } catch { }
					}

				}
			}


			if (string.IsNullOrEmpty(reBody.Text)) {
				reBody.Text = "<p>&nbsp;</p>";
			}
			if (string.IsNullOrEmpty(reLeftBody.Text)) {
				reLeftBody.Text = "<p>&nbsp;</p>";
			}
			if (string.IsNullOrEmpty(reRightBody.Text)) {
				reRightBody.Text = "<p>&nbsp;</p>";
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
				pageContents = ContentPageExport.GetSerializedContentPageExport(guidImportContentID).ThePage;
				if (pageContents != null) {
					pageContents.SiteID = SiteID;
					pageContents.CreateDate = DateTime.Now;
					pageContents.EditDate = DateTime.Now;
				}
			}

			if (pageContents == null) {
				pageContents = new ContentPage();
				pageContents.Root_ContentID = Guid.NewGuid();
				pageContents.ContentID = pageContents.ContentID;
				pageContents.SiteID = SiteID;
			}

			pageContents.IsLatestVersion = true;

			pageContents.TemplateFile = ddlTemplate.SelectedValue;

			pageContents.TitleBar = txtTitle.Text;
			pageContents.NavMenuText = txtNav.Text;
			pageContents.PageHead = txtHead.Text;
			pageContents.FileName = txtFileName.Text;

			pageContents.MetaDescription = txtDescription.Text;
			pageContents.MetaKeyword = txtKey.Text;

			pageContents.EditDate = DateTime.Now;
			pageContents.NavOrder = int.Parse(txtSort.Text);

			pageContents.PageText = reBody.Text;
			pageContents.LeftPageText = reLeftBody.Text;
			pageContents.RightPageText = reRightBody.Text;

			pageContents.PageActive = chkActive.Checked;

			if (txtParent.Text.Length > 5) {
				pageContents.Parent_ContentID = new Guid(txtParent.Text);
			} else {
				pageContents.Parent_ContentID = null;
			}

			pageContents.EditUserId = SecurityData.CurrentUserGuid;

			if (!chkDraft.Checked) {
				pageContents.SavePageEdit();
			} else {
				pageContents.SavePageAsDraft();
			}

			//if importing, copy in all meta data possible
			if (guidImportContentID != Guid.Empty) {
				List<Widget> widgets = ContentPageExport.GetSerializedContentPageExport(guidImportContentID).ThePageWidgets;

				foreach (var wd in widgets) {
					wd.Root_ContentID = pageContents.Root_ContentID;
					wd.Root_WidgetID = Guid.NewGuid();
					wd.WidgetDataID = Guid.NewGuid();
					wd.EditDate = DateTime.Now;
					wd.Save();
				}

				ContentPageExport.RemoveSerializedContentPageExport(guidImportContentID);
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
