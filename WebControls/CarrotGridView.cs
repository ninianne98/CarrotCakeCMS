using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
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

namespace Carrotware.Web.UI.Controls {

	[DefaultProperty("Text"), ToolboxData("<{0}:CarrotGridView runat=server></{0}:CarrotGridView>")]

	public class CarrotGridView : GridView {

		public CarrotGridView()
			: base() {

			SortDownIndicator = "&nbsp;&#x25BC;";
			SortUpIndicator = "&nbsp;&#x25B2;";
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string DefaultSort {
			get {
				String s = ViewState["DefaultSort"] as String;
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["DefaultSort"] = value;
			}
		}


		private string SortField {
			get;
			set;
		}

		private string SortDir {
			get;
			set;
		}

		private string SortUpIndicator {
			get;
			set;
		}

		private string SortDownIndicator {
			get;
			set;
		}


		public void lblSort_Command(object sender, EventArgs e) {
			SortParm = DefaultSort;
			LinkButton lb = (LinkButton)sender;
			string sSortField = "";
			try { sSortField = lb.CommandName.ToString(); } catch { }
			sSortField = ResetSortToColumn(sSortField);
			DefaultSort = sSortField;

			//Type theType = DataSource.GetType();
			//if (theType.IsGenericType) {
			//    Type X = theType.GetGenericArguments()[0];
			//    //List<X> lst = DataSource as List<X>;
			//    //SetListDataSource<object>(lst);
			//    // load data!
			//}

			//base.DataBind();
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



		public void SetListDataSource<T>(List<T> lstData) {
			List<T> lst = null;

			SortParm = DefaultSort;

			if (lstData != null) {
				if (!string.IsNullOrEmpty(DefaultSort)) {
					lst = SortDataList<T>(lstData);
				}
			}

			DataSource = lst;
		}


		protected override void Render(HtmlTextWriter writer) {
			SortParm = DefaultSort;

			WalkGridForHeadings(this);

			base.Render(writer);
		}


		protected override void PerformDataBinding(IEnumerable data) {

			base.PerformDataBinding(data);

			WalkGridSetClick(this);
		}



		private string SortParm {
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
				//sSort = sSort.Replace(" ", "    ");

				int pos = sSort.LastIndexOf(" ");

				sSortFld = sSort.Substring(0, pos).Trim();
				sSortDir = sSort.Substring(pos).Trim();

				SortField = sSortFld.Trim();
				SortDir = sSortDir.Trim();
			}
		}



		private void WalkGridSetClick(Control X) {
			foreach (Control c in X.Controls) {
				if (c is LinkButton) {
					LinkButton lb = (LinkButton)c;
					lb.Click += new EventHandler(this.lblSort_Command);
				} else {
					WalkGridSetClick(c);
				}
			}
		}


		private string ResetSortToColumn(string sSortField) {

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


		private List<T> SortDataList<T>(List<T> d) {

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

		private List<T> SortDataList<T>(List<T> d, string sSort) {
			ResetSortToColumn(sSort);
			return SortDataList<T>(d);
		}

		private object GetPropertyValue(object obj, string property) {
			System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo.GetValue(obj, null);
		}


		private void WalkGridForHeadings(Control X) {
			WalkGridForHeadings(X, SortField, SortDir);
		}

		private void WalkGridForHeadings(Control X, string strSortFld, string strSortDir) {

			strSortFld = strSortFld.ToLower();
			strSortDir = strSortDir.ToLower();

			foreach (Control c in X.Controls) {
				if (c is LinkButton) {
					LinkButton lb = (LinkButton)c;
					lb.Click += new EventHandler(lblSort_Command);
					if (strSortFld == lb.CommandName.ToLower()) {
						//don't add the arrows if alread sorted!
						if (lb.Text.IndexOf("&#x25B") < 0) {
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
