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
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:SiblingNavigation runat=server></{0}:SiblingNavigation>")]
	public class SiblingNavigation : BaseNavSelHeaded {

		public SiblingNavigation()
			: base() {
			this.IncludeParent = false;
		}

		[Category("Appearance")]
		[DefaultValue("false")]
		public bool IncludeParent { get; set; }

		public override List<string> LimitedPropertyList {
			get {
				List<string> lst = base.LimitedPropertyList;
				lst.Add("IncludeParent");

				return lst.Distinct().ToList();
			}
		}

		protected override void OnPreRender(System.EventArgs e) {
			if (this.PublicParmValues.Any()) {
				string sTmp = "";
				try {
					sTmp = GetParmValue("IncludeParent", "");
					if (!String.IsNullOrEmpty(sTmp)) {
						this.IncludeParent = Convert.ToBoolean(sTmp);
					}
				} catch (Exception ex) {
				}
			}

			base.OnPreRender(e);
		}

		protected override void LoadData() {
			base.LoadData();

			List<SiteNav> lstNav = navHelper.GetSiblingNavigation(SiteData.CurrentSiteID, SiteData.AlternateCurrentScriptName, !SecurityData.IsAuthEditor);

			if (this.IncludeParent && lstNav != null && lstNav.Where(x => x.ShowInSiteNav == true).Count() > 0) {
				SiteNav p = GetParent(lstNav.OrderByDescending(x => x.Parent_ContentID).FirstOrDefault().Parent_ContentID);
				if (p != null) {
					p.NavOrder = -100;
					lstNav.Add(p);
				}
			}

			this.NavigationData = lstNav.OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected SiteNav GetParent(Guid? rootContentID) {
			SiteNav pageNav = null;
			if (rootContentID.HasValue) {
				pageNav = navHelper.GetPageNavigation(SiteData.CurrentSiteID, rootContentID.Value);
			}
			return pageNav;
		}
	}
}