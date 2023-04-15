using System;
using System.Collections.Generic;
using Carrotware.CMS.Core;

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

	public class BaseContentPage : System.Web.UI.Page {
		protected PageProcessingHelper pph = new PageProcessingHelper();

		public ContentPage ThePage { get { return _pageContents; } }
		public SiteData TheSite { get { return _theSite; } }
		public List<Widget> ThePageWidgets { get { return _pageWidgets; } }

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

		protected ContentPage _pageContents = null;
		protected SiteData _theSite = null;
		protected List<Widget> _pageWidgets = null;
	}
}