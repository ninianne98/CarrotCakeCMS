using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Base {
	public partial class GenericPage : BaseContentPage {

		protected override void OnInit(EventArgs e) {

			LoadPageControls(this.Page);
			base.OnInit(e);
		}


		protected void Page_Load(object sender, EventArgs e) {

			if (pageContents != null) {
				litPageHeading.Text = pageContents.PageHead;

				AssignContentZones(BodyCenter, contCenter);
				AssignContentZones(BodyLeft, contLeft);
				AssignContentZones(BodyRight, contRight);

			}
		}

	}
}
