using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Base {
	public partial class GenericPage : BaseContentPage {

		protected override void OnInit(EventArgs e) {
			//contCenter = BodyCenter;
			//contLeft = BodyLeft;
			//contRight = BodyRight;

			LoadPageControls(this.Page);
			base.OnInit(e);
		}


		protected void Page_Load(object sender, EventArgs e) {

			if (pageContents != null) {
				litPageHeading.Text = pageContents.PageHead;

				BodyCenter.JQueryUIScope = contCenter.JQueryUIScope;
				BodyLeft.JQueryUIScope = contLeft.JQueryUIScope;
				BodyRight.JQueryUIScope = contRight.JQueryUIScope;

				BodyCenter.IsAdminMode = contCenter.IsAdminMode;
				BodyLeft.IsAdminMode = contLeft.IsAdminMode;
				BodyRight.IsAdminMode = contRight.IsAdminMode;

				BodyCenter.Text = contCenter.Text;
				BodyLeft.Text = contLeft.Text;
				BodyRight.Text = contRight.Text;

				BodyCenter.ZoneChar = contCenter.ZoneChar;
				BodyLeft.ZoneChar = contLeft.ZoneChar;
				BodyRight.ZoneChar = contRight.ZoneChar;

				BodyCenter.DatabaseKey = contCenter.DatabaseKey;
				BodyLeft.DatabaseKey = contLeft.DatabaseKey;
				BodyRight.DatabaseKey = contRight.DatabaseKey;

				//BodyCenter = contCenter;
				//BodyLeft = contLeft;
				//BodyRight = contRight;
				//Response.StatusCode = 200;

			}
		}



	}
}
