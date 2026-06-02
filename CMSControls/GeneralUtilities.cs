using Carrotware.CMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	public static class GeneralUtilities {

		#region binding utilities

		public static string GetSelectedValue(ListControl ctrl) {
			string sVal = null;
			if (ctrl.SelectedItem != null) {
				sVal = ctrl.SelectedValue;
			}
			return sVal;
		}

		public static List<string> GetSelectedValues(ListControl ctrl) {
			List<string> sVal = new List<string>();
			if (ctrl.Items != null) {
				foreach (ListItem itm in ctrl.Items) {
					if (itm.Selected) {
						sVal.Add(itm.Value);
					}
				}
			}
			return sVal;
		}

		public static int? GetSelectedInt(ListControl ctrl) {
			int? iVal = null;
			if (ctrl.SelectedItem != null) {
				iVal = int.Parse(ctrl.SelectedValue);
			}
			return iVal;
		}

		public static Guid? GetSelectedGuid(ListControl ctrl) {
			Guid? gVal = null;
			if (ctrl.SelectedItem != null) {
				gVal = new Guid(ctrl.SelectedValue);
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

		public static void BindList(ListControl ctrl, object dataSource, string selectedValue) {
			BindList(ctrl, dataSource);
			SelectListValue(ctrl, selectedValue);
		}

		public static void BindListChooseOne(ListControl ctrl, object dataSource) {
			BindListChooseOne(ctrl, dataSource, null);
		}

		public static void BindListChooseOne(ListControl ctrl, object dataSource, string selectedValue) {
			BindListDefaultText(ctrl, dataSource, selectedValue, "Choose One", "");
		}

		public static void BindListDefaultText(ListControl ctrl, object dataSource, string selectedValue, string emptyChoiceText, string emptyChoiceValue) {
			BindList(ctrl, dataSource);

			emptyChoiceValue = string.IsNullOrEmpty(emptyChoiceValue) ? "" : emptyChoiceValue;
			ctrl.Items.Insert(0, new ListItem(string.Format("-{0}-", emptyChoiceText), emptyChoiceValue));

			SelectListValue(ctrl, selectedValue);
		}

		public static void SelectListValue(ListControl ctrl, string selectedValue) {
			if (ctrl.Items.Count > 0) {
				if (selectedValue != null) {
					try { ctrl.SelectedValue = selectedValue; } catch { }
				}
			}
		}

		public static void SelectListValues(ListControl ctrl, List<string> selectedValues) {
			if (ctrl != null && ctrl.Items.Count > 0 && selectedValues != null) {
				if (selectedValues.Any()) {
					try {
						foreach (ListItem itm in ctrl.Items) {
							if (selectedValues.Where(x => x.ToLowerInvariant() == itm.Value.ToLowerInvariant()).Any()) {
								itm.Selected = true;
							}
						}
					} catch { }
				}
			}
		}

		#endregion binding utilities

		#region table checkbox parsers

		public static List<Guid> GetCheckedItemGuidsByValue(GridView grid, bool collectState, string checkBoxName) {
			List<Guid> lstUpd = new List<Guid>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(checkBoxName);
				if (chk != null && chk.Checked == collectState) {
					Guid gRoot = new Guid(chk.Attributes["value"].ToString());
					lstUpd.Add(gRoot);
				}
			}
			return lstUpd;
		}

		public static List<Guid> GetCheckedItemGuids(GridView grid, bool collectState, string checkBoxName, string hiddenName) {
			List<Guid> lstUpd = new List<Guid>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(checkBoxName);
				if (chk != null && chk.Checked == collectState) {
					HiddenField hdn = (HiddenField)row.FindControl(hiddenName);
					Guid gRoot = new Guid(hdn.Value);
					lstUpd.Add(gRoot);
				}
			}
			return lstUpd;
		}

		public static List<string> GetCheckedItemString(GridView grid, bool collectState, string checkBoxName, string hiddenName) {
			List<string> lstUpd = new List<string>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(checkBoxName);
				if (chk != null && chk.Checked == collectState) {
					HiddenField hdn = (HiddenField)row.FindControl(hiddenName);
					lstUpd.Add(hdn.Value);
				}
			}
			return lstUpd;
		}

		#endregion table checkbox parsers

		#region boolean list stuff

		public static bool? GetNullableBoolValue(ListControl ctrl) {
			bool? bVal = null;

			if (ctrl.SelectedValue == "0") {
				bVal = false;
			}
			if (ctrl.SelectedValue == "1") {
				bVal = true;
			}

			return bVal;
		}

		public static void BindOptionalBooleanList(ListControl ctrl, string selectedValue, string emptyChoiceText, string emptyChoiceValue, string trueChoiceText, string falseChoiceText) {
			emptyChoiceValue = string.IsNullOrEmpty(emptyChoiceValue) ? "" : emptyChoiceValue;

			List<ListItem> lst = new List<ListItem>();
			lst.Add(new ListItem(string.Format("-{0}-", emptyChoiceText), emptyChoiceValue));
			lst.Add(new ListItem(string.Format("{0}", trueChoiceText), "1"));
			lst.Add(new ListItem(string.Format("{0}", falseChoiceText), "0"));

			ctrl.DataTextField = "Text";
			ctrl.DataValueField = "Value";

			BindList(ctrl, lst);

			SelectListValue(ctrl, selectedValue);
		}

		public static void BindOptionalYesNoList(ListControl ctrl) {
			BindOptionalYesNoList(ctrl, null);
		}

		public static void BindOptionalYesNoList(ListControl ctrl, string selectedValue) {
			BindOptionalBooleanList(ctrl, selectedValue, "Choose One", "-1", "Yes", "No");
		}

		public static void BindOptionalTrueFalseList(ListControl ctrl) {
			BindOptionalTrueFalseList(ctrl, null);
		}

		public static void BindOptionalTrueFalseList(ListControl ctrl, string selectedValue) {
			BindOptionalBooleanList(ctrl, selectedValue, "Choose One", "-1", "True", "False");
		}

		#endregion boolean list stuff

		#region QueryString Parsers

		public static Guid GetGuidPageIDFromQuery() {
			return GetGuidParameterFromQuery("pageid");
		}

		public static Guid GetGuidIDFromQuery() {
			return GetGuidParameterFromQuery("id");
		}

		public static Guid GetGuidVersionFromQuery() {
			return GetGuidParameterFromQuery("versionid");
		}

		public static Guid GetGuidImportFromQuery() {
			return GetGuidParameterFromQuery(ContentImportExportUtils.ImportQueryKey);
		}

		public static Guid GetGuidParameterFromQuery(string parmName) {
			Guid id = Guid.Empty;
			if (SiteData.IsWebView) {
				if (HttpContext.Current.Request.QueryString[parmName] != null
					&& !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[parmName].ToString())) {
					id = new Guid(HttpContext.Current.Request.QueryString[parmName].ToString());
				}
			}
			return id;
		}

		public static string GetStringParameterFromQuery(string parmName) {
			string id = string.Empty;
			if (SiteData.IsWebView) {
				if (HttpContext.Current.Request.QueryString[parmName] != null
					&& !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[parmName].ToString())) {
					id = HttpContext.Current.Request.QueryString[parmName].ToString();
				}
			}
			return id;
		}

		#endregion QueryString Parsers

		public static string ResolvePath(Control ctrl, string path) {
			string sPathOut = null;
			if (!string.IsNullOrEmpty(path)) {
				sPathOut = path.Replace(@"\", "/");
			} else {
				sPathOut = "";
			}

			if (!sPathOut.Contains("//")) {
				if ((!sPathOut.StartsWith("~") && !sPathOut.StartsWith("/"))) {
					sPathOut = ctrl.AppRelativeTemplateSourceDirectory + sPathOut;
				}
				if (sPathOut.StartsWith("~")) {
					sPathOut = VirtualPathUtility.ToAbsolute(sPathOut);
				}
			}

			return sPathOut;
		}
	}
}