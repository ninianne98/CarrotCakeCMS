using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:PagedComments runat=server></{0}:PagedComments>")]
	public class PagedComments : BasePagedDataTemplate, IWidgetLimitedProperties {

		[Category("Appearance")]
		[DefaultValue("CreateDate  desc")]
		public override string OrderBy {
			get {
				String s = (String)ViewState["OrderBy"];
				return ((s == null) ? "CreateDate  desc" : s);
			}
			set {
				ViewState["OrderBy"] = value;
			}
		}

		protected override void OnInit(EventArgs e) {

			if (ContentTemplate == null) {
				ContentTemplate = new DefaultCommentTemplate();
			}

			base.OnInit(e);
		}

		public override void FetchData() {

			HttpContext context = HttpContext.Current;

			if (string.IsNullOrEmpty(OrderBy)) {
				this.OrderBy = "CreateDate  desc";
			}

			List<PostComment> lstContents = new List<PostComment>();

			int iPageNbr = this.PageNumberZeroIndex;

			if (context != null) {
				SiteNav sn = navHelper.FindByFilename(SiteData.CurrentSiteID, SiteData.CurrentScriptName);

				if (sn != null) {
					TotalRecords = PostComment.GetCommentCountByContent(sn.Root_ContentID, !SecurityData.IsAuthEditor);
					lstContents = PostComment.GetCommentsByContentPageNumber(sn.Root_ContentID, iPageNbr, this.PageSize, this.OrderBy, !SecurityData.IsAuthEditor);
				}
			} else {
				TotalRecords = 0;
				lstContents = new List<PostComment>();
			}

			this.DataSource = lstContents;
		}

		public List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("PageSize");
				lst.Add("PagerBelowContent");
				lst.Add("ShowPager");
				lst.Add("EnableViewState");
				lst.Add("OrderBy");
				lst.Add("CSSSelectedPage");
				lst.Add("CSSPageListing");
				lst.Add("CSSPageFooter");

				return lst;
			}
		}

	}
}