﻿using Carrotware.CMS.Core;
using System;
using System.Collections.Generic;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {

	public partial class GenericMasterPage : System.Web.UI.MasterPage {
		protected PageProcessingHelper pph = new PageProcessingHelper();

		public ContentPage ThePage { get { return pageContents; } }
		public SiteData TheSite { get { return theSite; } }
		public List<Widget> ThePageWidgets { get { return pageWidgets; } }

		protected ContentPage pageContents = null;
		protected SiteData theSite = null;
		protected List<Widget> pageWidgets = null;

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			pph = new PageProcessingHelper(this.Page);

			pph.LoadData();
			if (pph.ThePage != null) {
				theSite = pph.TheSite;
				pageContents = pph.ThePage;
				pageWidgets = pph.ThePageWidgets;
			}

			if (SiteData.IsWebView) {
				pph.LoadPageControls();
			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			SecurityData.AuthCookieTime();

			pph.AssignControls();
		}

		public bool IsSiteIndex {
			get {
				return this.TheSite != null && this.ThePage != null
						&& this.TheSite.Blog_Root_ContentID.HasValue
						&& this.ThePage.Root_ContentID == this.TheSite.Blog_Root_ContentID.Value;
			}
		}

		public bool IsBlogPost {
			get {
				return this.ThePage != null
						&& this.ThePage.ContentType == ContentPageType.PageType.BlogEntry;
			}
		}

		public bool IsPageContent {
			get {
				return this.ThePage != null
						&& this.ThePage.ContentType == ContentPageType.PageType.ContentEntry;
			}
		}
	}
}