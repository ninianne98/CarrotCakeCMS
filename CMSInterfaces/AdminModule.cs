using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Interface {
	public abstract class AdminModule : BaseShellUserControl, IAdminModule {

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			if (this.ModuleID == Guid.Empty || string.IsNullOrEmpty(this.ModuleName)) {
				this.ModuleID = AdminModuleQueryStringRoutines.GetModuleID();
				this.ModuleName = AdminModuleQueryStringRoutines.GetPluginFile();

				this.QueryStringFragment = AdminModuleQueryStringRoutines.GenerateQueryStringFragment(this.ModuleName, this.ModuleID);
				this.QueryStringPattern = AdminModuleQueryStringRoutines.GenerateQueryStringPattern(this.ModuleID);
			}
		}

		public string CreateLink(string sModuleName, string sIDParm) {

			return CreateLink(false, sModuleName, sIDParm);
		}

		public string CreateLink(string sScriptName, string sModuleName, string sIDParm) {

			return CreateLink(false, sScriptName, sModuleName, sIDParm);
		}

		public string CreateLink(string sModuleName) {

			return CreateLink(false, sModuleName);
		}


		public string CreatePopupLink(string sModuleName, string sIDParm) {

			return CreateLink(true, sModuleName, sIDParm);
		}

		public string CreatePopupLink(string sScriptName, string sModuleName, string sIDParm) {

			return CreateLink(true, sScriptName, sModuleName, sIDParm);
		}

		public string CreatePopupLink(string sModuleName) {

			return CreateLink(true, sModuleName);
		}

		private string sPopupFileName = "./ModulePopup.aspx?";

		private string CreateLink(bool bPop, string sModuleName, string sIDParm) {
			string sQueryStringFile = "";
			string sSuffix = string.Format(this.QueryStringPattern, sModuleName) + "&" + sIDParm;
			if (bPop) {
				sQueryStringFile = String.Format("javascript:ShowWindowNoRefresh('{0}');", sPopupFileName + sSuffix);
			} else {
				sQueryStringFile = CurrentScriptName + "?" + sSuffix;
			}
			return sQueryStringFile;
		}

		private string CreateLink(bool bPop, string sScriptName, string sModuleName, string sIDParm) {
			string sQueryStringFile = "";
			string sSuffix = string.Format(this.QueryStringPattern, sModuleName) + "&" + sIDParm;
			if (bPop) {
				sQueryStringFile = String.Format("javascript:ShowWindowNoRefresh('{0}');", sScriptName + "?" + sSuffix);
			} else {
				sQueryStringFile = sScriptName + "?" + sSuffix;
			}
			return sQueryStringFile;
		}

		private string CreateLink(bool bPop, string sModuleName) {
			string sQueryStringFile = "";
			string sSuffix = string.Format(this.QueryStringPattern, sModuleName);
			if (bPop) {
				sQueryStringFile = String.Format("javascript:ShowWindowNoRefresh('{0}');", sPopupFileName + sSuffix);
			} else {
				sQueryStringFile = CurrentScriptName + "?" + sSuffix;
			}
			return sQueryStringFile;
		}

		#region IAdminModule Members

		public Guid SiteID { get; set; }

		public Guid ModuleID { get; set; }

		public string QueryStringFragment { get; set; }

		public string QueryStringPattern { get; set; }

		public string ModuleName { get; set; }

		#endregion

		public string ResolveResourceFilePath(string sPath) {
			string sPathOut = null;
			if (!string.IsNullOrEmpty(sPath)) {
				sPathOut = sPath.Replace(@"\", "/");
			} else {
				sPathOut = "";
			}

			if (!sPathOut.Contains("//")) {
				if ((!sPathOut.StartsWith("~") && !sPathOut.StartsWith("/"))) {
					sPathOut = this.AppRelativeTemplateSourceDirectory + sPathOut;
				}
				if (sPathOut.StartsWith("~")) {
					sPathOut = VirtualPathUtility.ToAbsolute(sPathOut);
				}
			}

			return sPathOut;
		}

	}

	//===========================

	public class AdminModuleQueryStringRoutines {

		#region Common Querystring Parsing Routines

		public static Guid GetModuleID() {
			Guid moduleID = Guid.Empty;
			if (HttpContext.Current.Request.QueryString["pi"] != null) {
				try { moduleID = new Guid(HttpContext.Current.Request.QueryString["pi"].ToString()); } catch { }
			}
			return moduleID;
		}
		public static string GetPluginFile() {
			string plug = "";
			if (HttpContext.Current.Request.QueryString["pf"] != null) {
				plug = HttpContext.Current.Request.QueryString["pf"].ToString();
			}
			return plug;
		}

		public static string GenerateQueryStringFragment(string plug, Guid moduleID) {
			return "pf=" + plug + "&pi=" + moduleID.ToString();
		}

		public static string GenerateQueryStringPattern(Guid moduleID) {
			return "pf={0}&pi=" + moduleID.ToString();
		}

		#endregion
	}

}
