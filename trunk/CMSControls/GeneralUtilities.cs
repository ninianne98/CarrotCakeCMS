using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using System.Web.UI;
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
	public class GeneralUtilities {

		#region binding utilitites

		public static string GetSelectedValue(ListControl ddl) {
			string sVal = null;
			if (ddl.SelectedItem != null) {
				sVal = ddl.SelectedValue;
			}
			return sVal;
		}
		public static int? GetSelectedInt(ListControl ddl) {
			int? iVal = null;
			if (ddl.SelectedItem != null) {
				iVal = int.Parse(ddl.SelectedValue);
			}
			return iVal;
		}
		public static Guid? GetSelectedGuid(ListControl ddl) {
			Guid? gVal = null;
			if (ddl.SelectedItem != null) {
				gVal = new Guid(ddl.SelectedValue);
			}
			return gVal;
		}

		public static void BindRepeater(Repeater ctrl, object dataSource) {
			ctrl.DataSource = dataSource;
			ctrl.DataBind();
		}

		public static void BindDataBoundControl(DataBoundControl ctrl, object dataSource) {
			ctrl.DataSource = dataSource;
			ctrl.DataBind();
		}

		public static void BindList(ListControl ctrl, object dataSource) {
			ctrl.DataSource = dataSource;
			ctrl.DataBind();
		}
		public static void BindList(ListControl ctrl, object dataSource, string SelectedValue) {
			BindList(ctrl, dataSource);
			SelectListValue(ctrl, SelectedValue);
		}

		public static void BindListChooseOne(ListControl ctrl, object dataSource) {
			BindListChooseOne(ctrl, dataSource, null);
		}
		public static void BindListChooseOne(ListControl ctrl, object dataSource, string SelectedValue) {
			BindListDefaultText(ctrl, dataSource, SelectedValue, "Choose One", "");
		}

		public static void BindListDefaultText(ListControl ctrl, object dataSource, string SelectedValue, string EmptyChoiceText, string EmptyChoiceValue) {
			BindList(ctrl, dataSource);

			EmptyChoiceValue = string.IsNullOrEmpty(EmptyChoiceValue) ? "" : EmptyChoiceValue;
			ctrl.Items.Insert(0, new ListItem(String.Format("-{0}-", EmptyChoiceText), EmptyChoiceValue));

			SelectListValue(ctrl, SelectedValue);
		}

		public static void SelectListValue(ListControl ctrl, string SelectedValue) {
			if (ctrl.Items.Count > 0) {
				if (SelectedValue != null) {
					try { ctrl.SelectedValue = SelectedValue; } catch { }
				}
			}
		}

		#endregion

		#region table checkbox parsers

		public static List<Guid> GetCheckedItemGuidsByValue(GridView grid, bool CollectState, string CheckBoxName) {
			List<Guid> lstUpd = new List<Guid>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(CheckBoxName);
				if (chk != null && chk.Checked == CollectState) {
					Guid gRoot = new Guid(chk.Attributes["value"].ToString());
					lstUpd.Add(gRoot);
				}
			}
			return lstUpd;
		}

		public static List<Guid> GetCheckedItemGuids(GridView grid, bool CollectState, string CheckBoxName, string HiddenName) {
			List<Guid> lstUpd = new List<Guid>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(CheckBoxName);
				if (chk != null && chk.Checked == CollectState) {
					HiddenField hdn = (HiddenField)row.FindControl(HiddenName);
					Guid gRoot = new Guid(hdn.Value);
					lstUpd.Add(gRoot);
				}
			}
			return lstUpd;
		}


		public static List<string> GetCheckedItemString(GridView grid, bool CollectState, string CheckBoxName, string HiddenName) {
			List<string> lstUpd = new List<string>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(CheckBoxName);
				if (chk != null && chk.Checked == CollectState) {
					HiddenField hdn = (HiddenField)row.FindControl(HiddenName);
					lstUpd.Add(hdn.Value);
				}
			}
			return lstUpd;
		}

		#endregion

		#region boolean list stuff

		public static bool? GetNullableBoolValue(ListControl ddl) {
			bool? bVal = null;

			if (ddl.SelectedValue == "0") {
				bVal = false;
			}
			if (ddl.SelectedValue == "1") {
				bVal = true;
			}

			return bVal;
		}


		public static void BindOptionalBooleanList(ListControl ctrl, string SelectedValue, string EmptyChoiceText, string EmptyChoiceValue, string TrueChoiceText, string FalseChoiceText) {

			EmptyChoiceValue = string.IsNullOrEmpty(EmptyChoiceValue) ? "" : EmptyChoiceValue;

			List<ListItem> lst = new List<ListItem>();
			lst.Add(new ListItem(String.Format("-{0}-", EmptyChoiceText), EmptyChoiceValue));
			lst.Add(new ListItem(String.Format("{0}", TrueChoiceText), "1"));
			lst.Add(new ListItem(String.Format("{0}", FalseChoiceText), "0"));

			ctrl.DataTextField = "Text";
			ctrl.DataValueField = "Value";

			BindList(ctrl, lst);

			SelectListValue(ctrl, SelectedValue);
		}

		public static void BindOptionalYesNoList(ListControl ctrl) {
			BindOptionalYesNoList(ctrl, null);
		}
		public static void BindOptionalYesNoList(ListControl ctrl, string SelectedValue) {

			BindOptionalBooleanList(ctrl, SelectedValue, "Choose One", "-1", "Yes", "No");

		}

		public static void BindOptionalTrueFalseList(ListControl ctrl) {
			BindOptionalTrueFalseList(ctrl, null);
		}
		public static void BindOptionalTrueFalseList(ListControl ctrl, string SelectedValue) {

			BindOptionalBooleanList(ctrl, SelectedValue, "Choose One", "-1", "True", "False");

		}

		#endregion

		#region QueryString Parsers

		public static Guid GetGuidPageIDFromQuery() {
			return GetGuidParameterFromQuery("pageid");
		}

		public static Guid GetGuidIDFromQuery() {
			return GetGuidParameterFromQuery("id");
		}

		public static Guid GetGuidParameterFromQuery(string ParmName) {
			Guid id = Guid.Empty;
			if (SiteData.IsWebView) {
				if (HttpContext.Current.Request.QueryString[ParmName] != null
					&& !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[ParmName].ToString())) {
					id = new Guid(HttpContext.Current.Request.QueryString[ParmName].ToString());
				}
			}
			return id;
		}

		#endregion


		public static string ResolvePath(Control srcControl, string sPath) {
			string sPathOut = null;
			if (!string.IsNullOrEmpty(sPath)) {
				sPathOut = sPath.Replace(@"\", "/");
			} else {
				sPathOut = "";
			}

			if (!sPathOut.Contains("//")) {
				if ((!sPathOut.StartsWith("~") && !sPathOut.StartsWith("/"))) {
					sPathOut = srcControl.AppRelativeTemplateSourceDirectory + sPathOut;
				}
				if (sPathOut.StartsWith("~")) {
					sPathOut = VirtualPathUtility.ToAbsolute(sPathOut);
				}
			}

			return sPathOut;
		}

	}
}
