using Carrotware.CMS.Core;
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

	public partial class GenericPageFromMaster : BaseContentPage {

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			LoadMasterPageInfo();
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			LoadMasterPageInfo();
		}

		protected void LoadMasterPageInfo() {
			if (SiteData.IsWebView) {
				if (Master.ThePage != null) {
					_theSite = Master.TheSite;
					_pageContents = Master.ThePage;
					_pageWidgets = Master.ThePageWidgets;
				} else {
					if (_pageContents == null) {
						_theSite = SiteData.CurrentSite;
						_pageContents = SiteData.GetCurrentPage();
						if (_pageContents != null) {
							_pageWidgets = SiteData.GetCurrentPageWidgets(_pageContents.Root_ContentID);
						}
					}
				}
			} else {
				_theSite = SiteData.CurrentSite;
				_pageContents = SiteData.GetCurrentPage();
				_pageWidgets = new List<Widget>();
			}
		}
	}
}