using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
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
	public partial class CommentAddEdit : AdminBasePage {
		public Guid guidItemID = Guid.Empty;
		public Guid guidRootContentID = Guid.Empty;
		public ContentPageType.PageType pageType = ContentPageType.PageType.BlogEntry;

		protected void Page_Load(object sender, EventArgs e) {
			Master.ActivateTab(AdminBaseMasterPage.SectionID.BlogComment);
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidItemID = new Guid(Request.QueryString["id"].ToString());
			}

			if (!IsPostBack) {
				PostComment item = PostComment.GetContentCommentByID(guidItemID);
				if (item != null) {
					txtEmail.Text = item.CommenterEmail;
					txtName.Text = item.CommenterName;
					txtComment.Text = item.PostCommentText;
					chkApproved.Checked = item.IsApproved;
					chkSpam.Checked = item.IsSpam;
					guidRootContentID = item.Root_ContentID;
				}
			}

			if (guidRootContentID != Guid.Empty) {
				ContentPage pageContents = pageHelper.FindContentByID(SiteID, guidRootContentID);
				pageType = pageContents.ContentType;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			PostComment item = PostComment.GetContentCommentByID(guidItemID);
			if (item == null) {
				item = new PostComment();
				item.ContentCommentID = Guid.NewGuid();
			}

			guidRootContentID = item.Root_ContentID;

			item.CommenterEmail = txtEmail.Text;
			item.CommenterName = txtName.Text;
			item.PostCommentText = txtComment.Text;
			item.IsApproved = chkApproved.Checked;
			item.IsSpam = chkSpam.Checked;

			item.Save();

			Response.Redirect(SiteData.CurrentScriptName + "?id=" + item.ContentCommentID.ToString());
		}

	}
}