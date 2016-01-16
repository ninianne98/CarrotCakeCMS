using System;
using System.Collections.Generic;
using System.Linq;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Controls;

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

	public partial class BlogPostEdit : AdminBasePage {
		public Guid guidContentID = Guid.Empty;
		private ContentPage pageContents = null;

		protected void Page_Load(object sender, EventArgs e) {
			Master.UsesSaved = true;
			Master.HideSave();

			guidContentID = GetGuidPageIDFromQuery();

			cmsHelper.OverrideKey(guidContentID);

			if (cmsHelper.cmsAdminContent != null) {
				pageContents = cmsHelper.cmsAdminContent;
				litPageName.Text = pageContents.FileName;

				if (!IsPostBack) {
					GeneralUtilities.BindList(listCats, SiteData.CurrentSite.GetCategoryList().OrderBy(x => x.CategoryText));
					GeneralUtilities.BindList(listTags, SiteData.CurrentSite.GetTagList().OrderBy(x => x.TagText));

					txtTitle.Text = pageContents.TitleBar;
					txtNav.Text = pageContents.NavMenuText;
					txtHead.Text = pageContents.PageHead;
					txtThumb.Text = pageContents.Thumbnail;

					txtDescription.Text = pageContents.MetaDescription;
					txtKey.Text = pageContents.MetaKeyword;

					ucReleaseDate.SetDate(pageContents.GoLiveDate);
					ucRetireDate.SetDate(pageContents.RetireDate);

					lblUpdated.Text = pageContents.EditDate.ToString();

					chkActive.Checked = pageContents.PageActive;
					chkHide.Checked = pageContents.BlockIndex;

					if (pageContents.CreditUserId.HasValue) {
						var usr = new ExtendedUserData(pageContents.CreditUserId.Value);
						hdnCreditUserID.Value = usr.UserName;
						txtSearchUser.Text = string.Format("{0} ({1})", usr.UserName, usr.EmailAddress);
					}

					GeneralUtilities.SelectListValues(listTags, pageContents.ContentTags.Cast<IContentMetaInfo>().Select(x => x.ContentMetaInfoID.ToString()).ToList());
					GeneralUtilities.SelectListValues(listCats, pageContents.ContentCategories.Cast<IContentMetaInfo>().Select(x => x.ContentMetaInfoID.ToString()).ToList());
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

				pageContents.GoLiveDate = ucReleaseDate.GetDate();
				pageContents.RetireDate = ucRetireDate.GetDate();

				pageContents.PageActive = chkActive.Checked;
				pageContents.ShowInSiteNav = false;
				pageContents.ShowInSiteMap = false;
				pageContents.BlockIndex = chkHide.Checked;

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

				if (String.IsNullOrEmpty(hdnCreditUserID.Value)) {
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