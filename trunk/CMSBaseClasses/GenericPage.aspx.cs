using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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


namespace Carrotware.CMS.UI.Base {
	public partial class GenericPage : BaseContentPage {

		protected override void OnInit(EventArgs e) {

			LoadPageControls(this.Page);
			base.OnInit(e);
		}

		private ControlUtilities cu = new ControlUtilities();

		protected void Page_Load(object sender, EventArgs e) {

			if (pageContents != null) {

				cu.ResetFind();
				Control ctrlHead = cu.FindControl("litPageHeading", this);
				if (ctrlHead != null && ctrlHead is ITextControl) {
					((ITextControl)ctrlHead).Text = pageContents.PageHead;
				}

				cu.ResetFind();
				Control ctrlCenter = cu.FindControl("BodyCenter", this);
				if (ctrlCenter != null && ctrlCenter is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlCenter, contCenter);
				}

				cu.ResetFind();
				Control ctrlLeft = cu.FindControl("BodyLeft", this);
				if (ctrlLeft != null && ctrlLeft is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlLeft, contLeft);
				}

				cu.ResetFind();
				Control ctrlRight = cu.FindControl("BodyRight", this);
				if (ctrlRight != null && ctrlRight is ContentContainer) {
					AssignContentZones((ContentContainer)ctrlRight, contRight);
				}

				//litPageHeading.Text = pageContents.PageHead;
				//AssignContentZones(BodyCenter, contCenter);
				//AssignContentZones(BodyLeft, contLeft);
				//AssignContentZones(BodyRight, contRight);

			}
		}

	}
}
