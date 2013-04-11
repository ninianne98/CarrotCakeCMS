using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Carrotware.CMS.Core;
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

	[ToolboxData("<{0}:MultiLevelNavigation runat=server></{0}:MultiLevelNavigation>")]
	public class MultiLevelNavigation : BaseNavSel {

		[Category("Appearance")]
		[DefaultValue(true)]
		public override bool MultiLevel {
			get {
				return true;
			}
		}

		[Category("Appearance")]
		[DefaultValue(3)]
		public int LevelDepth {
			get {
				int s = 3;
				try { s = int.Parse(ViewState["LevelDepth"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["LevelDepth"] = value.ToString();
			}
		}

		protected override void LoadData() {
			base.LoadData();

			this.NavigationData = navHelper.GetLevelDepthNavigation(SiteData.CurrentSiteID, LevelDepth, !SecurityData.IsAuthEditor);
		}

	}
}
