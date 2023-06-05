﻿using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

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

	public partial class ucAdminModule : AdminBaseUserControl {
		public Guid ModuleID { get; set; }

		public string pf { get; set; }

		public string SelMenu = "0";

		public bool UseAjax { get; set; }

		public bool HideList { get; set; }

		public CMSAdminModule ModuleFamily { get; set; }

		public CMSAdminModuleMenu PluginItem { get; set; }

		protected bool bLoadModule = false;

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (!this.HideList && cmsHelper.AdminModules.Any()) {
					GeneralUtilities.BindRepeater(rpModuleList, cmsHelper.AdminModules);
				} else {
					rpModuleList.Visible = false;
				}
			}

			pnlSetter.Visible = true;
			pnlNav.Visible = !this.HideList;

			if (this.ModuleID != Guid.Empty) {
				int x = 0;
				foreach (var row in cmsHelper.AdminModules) {
					if (this.ModuleID.ToString().ToLowerInvariant() == row.PluginID.ToString().ToLowerInvariant()) {
						this.SelMenu = x.ToString();
						break;
					}
					x++;
				}

				this.ModuleFamily = (from m in cmsHelper.AdminModules
									 where m.PluginID == this.ModuleID
									 select m).FirstOrDefault();
				if (this.ModuleFamily != null) {
					litModuleTitle.Text = this.ModuleFamily.PluginName;

					this.PluginItem = (from m in this.ModuleFamily.PluginMenus
									   orderby m.Caption, m.SortOrder
									   where m.PluginParm == pf
									   select m).FirstOrDefault();

					if (this.PluginItem != null) {
						litModuleTitle.Text = string.Format("{0} : {1}", this.ModuleFamily.PluginName, this.PluginItem.Caption);
					}
				}
			}
		}

		protected override void OnInit(EventArgs e) {
			if (!bLoadModule) {
				LoadModule();
			}
			base.OnInit(e);
		}

		public void LoadModule() {
			ModuleID = AdminModuleQueryStringRoutines.GetModuleID();
			pf = AdminModuleQueryStringRoutines.GetPluginFile();

			if (!string.IsNullOrEmpty(pf)) {
				phNone.Visible = false;

				this.ModuleFamily = (from m in cmsHelper.AdminModules
									 where m.PluginID == this.ModuleID
									 select m).FirstOrDefault();

				this.PluginItem = (from m in this.ModuleFamily.PluginMenus
								   orderby m.Caption, m.SortOrder
								   where m.PluginParm == pf
								   select m).FirstOrDefault();

				this.UseAjax = this.PluginItem.UseAjax;

				Control c = Page.LoadControl(this.PluginItem.ControlFile);
				phAdminModule.Controls.Add(c);

				if (c is IAdminModule) {
					var w = (IAdminModule)c;
					w.SiteID = SiteData.CurrentSiteID;
					w.ModuleID = this.ModuleID;
					w.ModuleName = pf;
					w.QueryStringFragment = AdminModuleQueryStringRoutines.GenerateQueryStringFragment(pf, this.ModuleID);
					w.QueryStringPattern = AdminModuleQueryStringRoutines.GenerateQueryStringPattern(this.ModuleID);
				}
			} else {
				phNone.Visible = true;
			}

			bLoadModule = true;
		}

		protected string MarkSelected(string sID, string sParm) {
			if (sID == this.ModuleID.ToString().ToLowerInvariant() && sParm == pf) {
				return " class=\"selectedModule\" ";
			} else {
				return " ";
			}
		}

		protected string CreateLink(string sPop, string sID, string sParm) {
			if (Convert.ToBoolean(sPop)) {
				return string.Format("javascript:ShowWindowNoRefresh('{0}?pi={1}&pf={2}');", "./ModulePopup.aspx", Eval("PluginID"), Eval("PluginParm"));
			} else {
				return string.Format("{0}?pi={1}&pf={2}", SiteData.CurrentScriptName, Eval("PluginID"), Eval("PluginParm"));
			}
		}

		protected void rpModuleList_ItemDataBound(object sender, RepeaterItemEventArgs e) {
			Repeater rpModuleContents = (Repeater)e.Item.FindControl("rpModuleContents");
			HiddenField hdnID = (HiddenField)e.Item.FindControl("hdnID");

			if (rpModuleContents != null) {
				var d = (CMSAdminModule)e.Item.DataItem;

				GeneralUtilities.BindRepeater(rpModuleContents, d.PluginMenus.Where(z => z.IsVisible == true).OrderBy(x => x.Caption).OrderBy(x => x.SortOrder).ToList());
			}
		}
	}
}