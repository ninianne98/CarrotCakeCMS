using Carrotware.CMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:MultiLevelNavigation runat=server></{0}:MultiLevelNavigation>")]
	public class MultiLevelNavigation : BaseNavSel {

		public MultiLevelNavigation() {
			this.LevelDepth = 3;
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public override bool MultiLevel {
			get {
				return true;
			}
		}

		[Category("Appearance")]
		[DefaultValue(3)]
		public int LevelDepth { get; set; }

		protected override void LoadData() {
			base.LoadData();

			this.NavigationData = navHelper.GetLevelDepthNavigation(SiteData.CurrentSiteID, LevelDepth, !SecurityData.IsAuthEditor);
		}

		public override List<string> LimitedPropertyList {
			get {
				List<string> lst = base.LimitedPropertyList;
				lst.Add("LevelDepth");

				return lst.Distinct().ToList();
			}
		}

		protected override void OnPreRender(System.EventArgs e) {
			if (this.PublicParmValues.Any()) {
				string sTmp = "";
				try {
					sTmp = GetParmValue("LevelDepth", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.LevelDepth = Convert.ToInt32(sTmp);
					}
				} catch (Exception ex) {
				}
			}

			base.OnPreRender(e);
		}
	}
}