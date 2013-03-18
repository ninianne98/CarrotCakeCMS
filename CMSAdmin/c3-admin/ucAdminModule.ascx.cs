using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
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
				if (!HideList && cmsHelper.AdminModules.Count > 0) {
					GeneralUtilities.BindRepeater(rpModuleList, cmsHelper.AdminModules);
				} else {
					rpModuleList.Visible = false;
				}
			}

			pnlNav.Visible = !HideList;

			if (ModuleID != Guid.Empty) {
				pnlSetter.Visible = true;
				int x = 0;
				foreach (var row in cmsHelper.AdminModules) {
					if (ModuleID.ToString().ToLower() == row.PluginID.ToString().ToLower()) {
						SelMenu = x.ToString();
						break;
					}
					x++;
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
			ModuleID = Guid.Empty;
			pf = String.Empty;

			if (Request.QueryString["pi"] != null) {
				try { ModuleID = new Guid(Request.QueryString["pi"].ToString()); } catch { }
			}

			if (Request.QueryString["pf"] != null) {
				pf = Request.QueryString["pf"].ToString();

				ModuleFamily = (from m in cmsHelper.AdminModules
								where m.PluginID == ModuleID
								select m).FirstOrDefault();

				PluginItem = (from m in ModuleFamily.PluginMenus
							  where m.PluginParm == pf
							  select m).FirstOrDefault();

				UseAjax = PluginItem.UseAjax;

				Control c = Page.LoadControl(PluginItem.ControlFile);
				phAdminModule.Controls.Add(c);

				if (c is IAdminModule) {
					var w = (IAdminModule)c;
					w.SiteID = SiteData.CurrentSiteID;
					w.ModuleID = ModuleID;
					w.ModuleName = pf;
					w.QueryStringFragment = "pf=" + pf + "&pi=" + ModuleID.ToString();
					w.QueryStringPattern = "pf={0}&pi=" + ModuleID.ToString();
				}
			}

			bLoadModule = true;
		}

		protected string MarkSelected(string sID, string sParm) {

			if (sID == ModuleID.ToString().ToLower() && sParm == pf) {
				return " class=\"selectedModule\" ";
			} else {
				return " ";
			}

		}

		protected string CreateLink(string sPop, string sID, string sParm) {
			if (Convert.ToBoolean(sPop)) {
				return String.Format("javascript:ShowWindowNoRefresh('{0}?pi={1}&pf={2}');", "./ModulePopup.aspx", Eval("PluginID"), Eval("PluginParm"));
			} else {
				return String.Format("{0}?pi={1}&pf={2}", SiteData.CurrentScriptName, Eval("PluginID"), Eval("PluginParm"));
			}

		}

		protected void rpModuleList_ItemDataBound(object sender, RepeaterItemEventArgs e) {

			Repeater rpModuleContents = (Repeater)e.Item.FindControl("rpModuleContents");
			HiddenField hdnID = (HiddenField)e.Item.FindControl("hdnID");

			if (rpModuleContents != null) {
				var d = (CMSAdminModule)e.Item.DataItem;

				GeneralUtilities.BindRepeater(rpModuleContents, d.PluginMenus.Where(z => z.IsVisible == true).OrderBy(x => x.SortOrder).ToList());
			}
		}

	}
}