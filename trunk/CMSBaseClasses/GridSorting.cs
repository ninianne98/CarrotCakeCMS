using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Base {
	public class GridSorting {

		public GridSorting() {

			//SortUpIndicator = "&nbsp;&uarr;";
			//SortDownIndicator = "&nbsp;&darr;";

			SortDownIndicator = "&nbsp;&#x25BC;";
			SortUpIndicator = "&nbsp;&#x25B2;";
		}

		public string DefaultSort {
			get;
			set;
		}
		public string SortField {
			get;
			set;
		}
		public string SortDir {
			get;
			set;
		}
		public string SortUpIndicator {
			get;
			set;
		}
		public string SortDownIndicator {
			get;
			set;
		}


		public string Sort {

			get {
				string sSort = "";
				try {
					sSort = SortField + "   " + SortDir;
				} catch {
					sSort = DefaultSort;
				}
				return sSort.Trim();
			}
			set {
				string sSort = DefaultSort;
				if (!string.IsNullOrEmpty(value)) {
					sSort = value;
				}
				string sSortFld = string.Empty;
				string sSortDir = string.Empty;
				sSort = sSort.Replace(" ", "    ");

				sSortFld = sSort.Substring(0, sSort.Length - 5).Trim();
				sSortDir = sSort.Substring(sSort.Length - 5).Trim();

				SortField = sSortFld.Trim();
				SortDir = sSortDir.Trim();
			}
		}


		public void SetHeaderClick(Control TheControl, EventHandler CmdFunc) {
			//add the command click event to the link buttons on the datagrid heading

			foreach (Control c in TheControl.Controls) {
				if (c is LinkButton) {
					LinkButton lb = (LinkButton)c;
					lb.Click += new EventHandler(CmdFunc);
				} else {
					SetHeaderClick(c, CmdFunc);
				}
			}
		}


		public string ResetSortToColumn(string sSortField) {

			if (SortField.Length < 1) {
				SortField = sSortField;
				SortDir = string.Empty;
			} else {
				if (SortField.ToLower() != sSortField.ToLower()) {
					SortDir = string.Empty;  //previous sort not the same field, force ASC
				}
				SortField = sSortField;
			}

			if (SortDir.Trim().IndexOf("ASC") < 0) {
				SortDir = "ASC";
			} else {
				SortDir = "DESC";
			}
			sSortField = SortField + "   " + SortDir;
			return sSortField;
		}


		public List<T> SortDataList<T>(List<T> d) {

			List<T> query = null;
			IEnumerable<T> myEnumerables = d.AsEnumerable();

			if (SortDir.Trim().IndexOf("ASC") < 0) {
				query = (from enu in myEnumerables
						 orderby GetPropertyValue(enu, SortField) descending
						 select enu).ToList<T>();
			} else {
				query = (from enu in myEnumerables
						 orderby GetPropertyValue(enu, SortField) ascending
						 select enu).ToList<T>();
			}

			return query.ToList<T>();

		}

		protected List<T> SortDataList<T>(List<T> d, string sSort) {
			ResetSortToColumn(sSort);
			return SortDataList<T>(d);
		}

		private object GetPropertyValue(object obj, string property) {
			System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo.GetValue(obj, null);
		}

		public void WalkGridForHeadings(Control X) {
			WalkGridForHeadings(X, SortField, SortDir);
		}


		private void WalkGridForHeadings(Control X, string strSortFld, string strSortDir) {

			strSortFld = strSortFld.ToLower();
			strSortDir = strSortDir.ToLower();

			foreach (Control c in X.Controls) {
				if (c is LinkButton) {
					LinkButton lb = (LinkButton)c;
					if (strSortFld == lb.CommandName.ToLower()) {
						//don't add the arrows if alread sorted!
						if (lb.Text.IndexOf("arr;") < 0) {
							if (strSortDir != "asc") {
								//Response.Write(strSortFld + " -D- ");
								lb.Text += SortDownIndicator;
							} else {
								//Response.Write(strSortFld + " -U- ");
								lb.Text += SortUpIndicator;
							}
						}
						break;
					}
				} else {
					WalkGridForHeadings(c, strSortFld, strSortDir);
				}
			}
		}






	}
}
