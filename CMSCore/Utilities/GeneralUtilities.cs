using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {
	public class GeneralUtilities {

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

		public static void BindRepeater(Repeater ctrl, object DataSource) {
			ctrl.DataSource = DataSource;
			ctrl.DataBind();
		}

		public static void BindList(ListControl ctrl, object DataSource) {
			ctrl.DataSource = DataSource;
			ctrl.DataBind();
		}
		public static void BindList(ListControl ctrl, object DataSource, string SelectedValue) {
			BindList(ctrl, DataSource);
			SelectListValue(ctrl, SelectedValue);
		}

		public static void BindListChooseOne(ListControl ctrl, object DataSource) {
			BindListChooseOne(ctrl, DataSource, null);
		}
		public static void BindListChooseOne(ListControl ctrl, object DataSource, string SelectedValue) {
			BindListDefaultText(ctrl, DataSource, SelectedValue, "Choose One", "");
		}

		public static void BindListDefaultText(ListControl ctrl, object DataSource, string SelectedValue, string EmptyChoiceText, string EmptyChoiceValue) {
			BindList(ctrl, DataSource);

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

	}
}
