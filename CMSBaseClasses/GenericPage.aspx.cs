using System;
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

	public partial class GenericPage : BaseContentPage {

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

			pph.AssignControls();
		}
	}
}