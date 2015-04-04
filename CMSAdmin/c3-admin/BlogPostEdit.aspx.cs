using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
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
	public partial class BlogPostEdit : AdminBasePage {

		public Guid guidContentID = Guid.Empty;
		ContentPage pageContents = null;

		protected void Page_Load(object sender, EventArgs e) {
			Master.UsesSaved = true;
			Master.HideSave();

			guidContentID = GetGuidPageIDFromQuery();

			cmsHelper.OverrideKey(guidContentID);

			if (cmsHelper.cmsAdminContent != null) {
				pageContents = cmsHelper.cmsAdminContent;
				litPageName.Text = pageContents.FileName;

				if (!IsPostBack) {

					GeneralUtilities.BindRepeater(rpCat, SiteData.CurrentSite.GetCategoryList().OrderBy(x => x.CategoryText));

					GeneralUtilities.BindRepeater(rpTag, SiteData.CurrentSite.GetTagList().OrderBy(x => x.TagText));

					txtTitle.Text = pageContents.TitleBar;
					txtNav.Text = pageContents.NavMenuText;
					txtHead.Text = pageContents.PageHead;
					txtThumb.Text = pageContents.Thumbnail;

					txtDescription.Text = pageContents.MetaDescription;
					txtKey.Text = pageContents.MetaKeyword;

					txtReleaseDate.Text = pageContents.GoLiveDate.ToShortDateString();
					txtReleaseTime.Text = pageContents.GoLiveDate.ToShortTimeString();
					txtRetireDate.Text = pageContents.RetireDate.ToShortDateString();
					txtRetireTime.Text = pageContents.RetireDate.ToShortTimeString();

					lblUpdated.Text = pageContents.EditDate.ToString();

					chkActive.Checked = pageContents.PageActive;
					chkHide.Checked = pageContents.BlockIndex;

					if (pageContents.CreditUserId.HasValue) {
						var usr = new ExtendedUserData(pageContents.CreditUserId.Value);
						hdnCreditUserID.Value = usr.UserName;
						txtSearchUser.Text = string.Format("{0} ({1})", usr.UserName, usr.EmailAddress);
					}

					PreselectCheckboxRepeater(rpCat, pageContents.ContentCategories.Cast<IContentMetaInfo>().ToList());

					PreselectCheckboxRepeater(rpTag, pageContents.ContentTags.Cast<IContentMetaInfo>().ToList());
				}
			}
		}


		protected void btnSave_Click(object sender, EventArgs e) {

			if (pageContents != null) {
				pageContents.TitleBar = txtTitle.Text;
				pageContents.NavMenuText = txtNav.Text;
				pageContents.PageHead = txtHead.Text;

				pageContents.MetaDescription = txtDescription.Text;
				pageContents.MetaKeyword = txtKey.Text;
				pageContents.Thumbnail = txtThumb.Text;

				pageContents.EditDate = SiteData.CurrentSite.Now;

				pageContents.GoLiveDate = Convert.ToDateTime(txtReleaseDate.Text + " " + txtReleaseTime.Text);
				pageContents.RetireDate = Convert.ToDateTime(txtRetireDate.Text + " " + txtRetireTime.Text);

				pageContents.PageActive = chkActive.Checked;
				pageContents.ShowInSiteNav = false;
				pageContents.ShowInSiteMap = false;
				pageContents.BlockIndex = chkHide.Checked;

				List<ContentCategory> lstCat = new List<ContentCategory>();
				List<ContentTag> lstTag = new List<ContentTag>();

				lstCat = (from cr in CollectCheckboxRepeater(rpCat)
						  join l in SiteData.CurrentSite.GetCategoryList() on cr equals l.ContentCategoryID
						  select l).ToList();

				lstTag = (from cr in CollectCheckboxRepeater(rpTag)
						  join l in SiteData.CurrentSite.GetTagList() on cr equals l.ContentTagID
						  select l).ToList();

				pageContents.ContentCategories = lstCat;
				pageContents.ContentTags = lstTag;

				if (string.IsNullOrEmpty(hdnCreditUserID.Value)) {
					pageContents.CreditUserId = null;
				} else {
					var usr = new ExtendedUserData(hdnCreditUserID.Value);
					pageContents.CreditUserId = usr.UserId;
				}

				pageContents.FileName = ContentPageHelper.CreateFileNameFromSlug(pageContents.SiteID, pageContents.GoLiveDate, pageContents.PageSlug);

				cmsHelper.cmsAdminContent = pageContents;

				Master.ShowSave();

				Response.Redirect(SiteData.CurrentScriptName + "?pageid=" + pageContents.Root_ContentID.ToString() + Master.SavedSuffix);
			}

		}
	}
}
