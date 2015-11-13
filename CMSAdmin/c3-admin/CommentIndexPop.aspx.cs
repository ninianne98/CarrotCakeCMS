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

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class CommentIndexPop : AdminBasePage {

		protected void Page_Load(object sender, EventArgs e) {
			Master.UsesSaved = true;

			if (IsPostBack) {
				Master.ShowSave();
			} else {
				if (!SiteData.RefererScriptName.ToLower().EndsWith(SiteData.CurrentScriptName.ToLower())) {
					Master.HideSave();
				} else {
					Master.ShowSave();
				}
			}
		}
	}
}