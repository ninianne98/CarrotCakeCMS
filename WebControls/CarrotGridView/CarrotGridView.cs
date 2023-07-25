using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
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

namespace Carrotware.Web.UI.Controls {

	[Designer(typeof(CarrotGridViewDesigner))]
	[ToolboxData("<{0}:CarrotGridView runat=server></{0}:CarrotGridView>")]
	public class CarrotGridView : GridView {

		public CarrotGridView()
			: base() {
			//SortDownIndicator = "&nbsp;&#x25BC;";
			//SortUpIndicator = "&nbsp;&#x25B2;";
			SortDownIndicator = "&nbsp;&#9660;";
			SortUpIndicator = "&nbsp;&#9650;";
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string DefaultSort {
			get {
				String s = ViewState["DefaultSort"] as String;
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["DefaultSort"] = value;
			}
		}

		public string CurrentSort {
			get {
				return this.SortParm;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool PrettifyHeadings {
			get {
				String s = (String)ViewState["PrettifyHeadings"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["PrettifyHeadings"] = value.ToString();
			}
		}

		public string LinkButtonCommands {
			get {
				String s = ViewState["LinkButtonCommands"] as String;
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["LinkButtonCommands"] = value;
			}
		}

		public string GetClickedCommand() {
			HttpRequest request = HttpContext.Current.Request;
			KeyValuePair<string, string> pair = new KeyValuePair<string, string>(" -- ", "");

			if (request.ServerVariables["REQUEST_METHOD"] != null &&
				request.ServerVariables["REQUEST_METHOD"].ToString().ToUpperInvariant() == "POST"
				&& request.Form["__EVENTARGUMENT"] != null) {
				string arg = request.Form["__EVENTARGUMENT"].ToString();
				string tgt = request.Form["__EVENTTARGET"].ToString();

				string sParm = this.ClientID.Replace(this.ID, "").Replace("_", "$");
				if (string.IsNullOrEmpty(sParm)) {
					sParm = this.ID + "$";
				} else {
					sParm = (sParm + "$" + this.ID).Replace("$$", "$");
				}

				if (tgt.StartsWith(sParm)
					&& tgt.Contains("$lnkHead")
					&& tgt.Contains(this.ID + "$")) {
					string[] btn = this.LinkButtonCommands.Split(';');
					string[] parms = tgt.Split('$');
					string sKey = parms[parms.Length - 1];

					Dictionary<string, string> lst = new Dictionary<string, string>();
					foreach (string b1 in btn) {
						if (!string.IsNullOrEmpty(b1)) {
							string[] b2 = b1.Split('|');
							lst.Add(b2[0], b2[1]);
							if (b2[0].EndsWith(sKey)) {
								break;
							}
						}
					}

					pair = (from d in lst
							where d.Key.EndsWith(sKey)
							select d).FirstOrDefault();
				}
			}

			return pair.Value;
		}

		public string PredictNewSort {
			get {
				string sSort = this.DefaultSort;
				string sNewSortField = GetClickedCommand();
				bool bColChanged = false;

				if (!string.IsNullOrEmpty(sNewSortField)) {
					string sCurrentSortField = string.IsNullOrEmpty(this.SortField) ? string.Empty : this.SortField;
					string sSortDir = string.IsNullOrEmpty(this.SortDir) ? string.Empty : this.SortDir;

					if (sCurrentSortField.ToLowerInvariant().Trim() != sNewSortField.ToLowerInvariant().Trim()) {
						bColChanged = true;
						this.SortDir = string.Empty;  //previous sort not the same field, force ASC
					}

					sCurrentSortField = sNewSortField;

					if (bColChanged || sSortDir.Trim().ToUpperInvariant().EndsWith("ASC") == false) {
						sSortDir = "ASC";
					} else {
						sSortDir = "DESC";
					}

					sSort = string.Format("{0}   {1}", sCurrentSortField, sSortDir);
				}

				return sSort;
			}
		}

		private string SortField {
			get {
				String s = ViewState["SortField"] as String;
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["SortField"] = value;
			}
		}

		private string SortDir {
			get {
				String s = ViewState["SortDir"] as String;
				return ((s == null) ? string.Empty : s);
			}
			set {
				ViewState["SortDir"] = value;
			}
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
			this.SortParm = this.DefaultSort;
			LinkButton lb = (LinkButton)sender;
			string sSortField = "";
			try { sSortField = lb.CommandName.ToString(); } catch { }
			sSortField = ResetSortToColumn(sSortField);
			this.DefaultSort = sSortField;

			base.DataBind();
		}

		private void SetTemplates() {
			foreach (DataControlField col in this.Columns) {
				if (col is CarrotHeaderSortTemplateField) {
					var ctf = (CarrotHeaderSortTemplateField)col;

					if (this.PrettifyHeadings || ctf.PrettifyHeading) {
						ctf.HeaderText = ctf.HeaderText.ToSpacedPascal();
					}

					ctf.HeaderTemplate = new CarrotSortButtonHeaderTemplate(ctf.HeaderText, ctf.SortExpression);

					if (string.IsNullOrEmpty(ctf.DataField) && !string.IsNullOrEmpty(ctf.SortExpression)) {
						ctf.DataField = ctf.SortExpression;
					}

					if (ctf.ItemTemplate == null) {
						if (!string.IsNullOrEmpty(ctf.DataField) && !ctf.ShowBooleanImage && !ctf.ShowEnumImage) {
							ctf.ItemTemplate = new CarrotAutoItemTemplate(ctf.DataField, ctf.DataFieldFormat);
						}

						if (ctf.ShowBooleanImage && !ctf.ShowEnumImage) {
							var itemTemplate = new CarrotBooleanImageItemTemplate(ctf);
							if (!string.IsNullOrEmpty(ctf.AlternateTextTrue) || !string.IsNullOrEmpty(ctf.AlternateTextFalse)) {
								itemTemplate.SetVerbiage(ctf.AlternateTextTrue, ctf.AlternateTextFalse);
							}
							if (!string.IsNullOrEmpty(ctf.ImagePathTrue) || !string.IsNullOrEmpty(ctf.ImagePathFalse)) {
								itemTemplate.SetImage(ctf.ImagePathTrue, ctf.ImagePathFalse);
							}
							ctf.ItemTemplate = itemTemplate;
						}

						if (ctf.ShowEnumImage) {
							ctf.ItemTemplate = new CarrotImageItemTemplate(ctf);
						}
					}
				}
			}
		}

		private void SetData() {
			SetTemplates();

			if (!string.IsNullOrEmpty(this.DefaultSort) && this.DataSource != null) {
				Type theType = this.DataSource.GetType();

				if (theType.IsGenericType && theType.GetGenericTypeDefinition() == typeof(List<>)) {
					IList lst = (IList)this.DataSource;
					this.SortParm = this.DefaultSort;
					var lstVals = SortDataListType(lst);

					this.DataSource = lstVals;
				}

				if (this.DataSource is DataSet || theType == typeof(DataSet)) {
					DataSet ds = (DataSet)this.DataSource;
					if (ds.Tables.Count > 0) {
						this.SortParm = this.DefaultSort;
						DataTable dt = ds.Tables[0];
						var dsVals = SortDataTable(dt);

						this.DataSource = dsVals;
					}
				}

				if (this.DataSource is DataTable || theType == typeof(DataTable)) {
					DataTable dt = (DataTable)this.DataSource;
					this.SortParm = this.DefaultSort;
					var dsVals = SortDataTable(dt);

					this.DataSource = dsVals;
				}
			}
		}

		public DataTable SortDataTable(DataTable dt) {
			DataTable dtNew = dt.Clone();

			if (!string.IsNullOrEmpty(this.SortField)) {
				dtNew.DefaultView.RowFilter = dt.DefaultView.RowFilter;
				DataRow[] copyRows = dt.DefaultView.Table.Select(dt.DefaultView.RowFilter, string.Format("{0}   {1}", this.SortField, this.SortDir));

				int iTotal = dt.Rows.Count;

				for (int t = 0; t < iTotal; t++) {
					DataRow copyRow = copyRows[t];
					dtNew.ImportRow(copyRow);
				}

				return dtNew;
			} else {
				return dt;
			}
		}

		public IList SortDataListType(IList lst) {
			IList query = null;
			List<object> d = lst.Cast<object>().ToList();
			IEnumerable<object> enuQueryable = d.AsQueryable();

			if (lst != null && lst.Count > 0) {
				this.SortField = GetProperties(d[0]).Where(x => x.ToLowerInvariant() == this.SortField.ToLowerInvariant()).FirstOrDefault();
			} else {
				this.SortField = string.Empty;
			}

			if (!string.IsNullOrEmpty(this.SortField)) {
				if (this.SortDir.ToUpperInvariant().Trim().IndexOf("ASC") < 0) {
					query = (from enu in enuQueryable
							 orderby GetPropertyValue(enu, this.SortField) descending
							 select enu).ToList();
				} else {
					query = (from enu in enuQueryable
							 orderby GetPropertyValue(enu, this.SortField) ascending
							 select enu).ToList();
				}
			} else {
				query = (from enu in enuQueryable
						 select enu).ToList();
			}

			return query;
		}

		private IList SortDataListType(IList lst, string sSort) {
			ResetSortToColumn(sSort);
			return SortDataListType(lst);
		}

		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();

			if (this.Rows.Count > 0) {
				WalkGridForHeadings(this.HeaderRow);
			}

			base.Render(writer);
		}

		protected override void PerformDataBinding(IEnumerable data) {
			SetData();

			if (this.DataSource != null) {
				Type theType = this.DataSource.GetType();
				if (theType.IsGenericType && theType.GetGenericTypeDefinition() == typeof(List<>)) {
					data = (IList)this.DataSource;
				}

				if (this.DataSource is DataSet || theType == typeof(DataSet)) {
					if (((DataSet)this.DataSource).Tables.Count > 0) {
						data = ((DataSet)this.DataSource).Tables[0].AsDataView();
					}
				}
				if (this.DataSource is DataTable || theType == typeof(DataTable)) {
					data = ((DataTable)this.DataSource).AsDataView();
				}
			}

			base.PerformDataBinding(data);

			if (string.IsNullOrEmpty(this.SortField) || string.IsNullOrEmpty(this.SortDir)) {
				this.SortParm = string.Empty;
			}

			this.LinkButtonCommands = "";
			if (this.Rows.Count > 0) {
				WalkGridSetClick(this.HeaderRow);
			}
		}

		private string SortParm {
			get {
				string sSort = "";
				try {
					sSort = string.Format("{0}   {1}", this.SortField, this.SortDir);
				} catch {
					sSort = this.DefaultSort;
				}
				return sSort.Trim();
			}
			set {
				string sSort = this.DefaultSort;
				if (!string.IsNullOrEmpty(value)) {
					sSort = value;
				}
				string sSortFld = string.Empty;
				string sSortDir = string.Empty;

				if (!string.IsNullOrEmpty(sSort)) {
					int pos = sSort.LastIndexOf(" ");

					sSortFld = sSort.Substring(0, pos).Trim();
					sSortDir = sSort.Substring(pos).Trim();
				}

				this.SortField = sSortFld.Trim();
				this.SortDir = sSortDir.Trim().ToUpperInvariant();
			}
		}

		private void WalkGridSetClick(Control ctrl) {
			foreach (Control c in ctrl.Controls) {
				if (c is LinkButton) {
					LinkButton lb = (LinkButton)c;
					lb.Click += new EventHandler(this.lblSort_Command);
					this.LinkButtonCommands += lb.ClientID + "|" + lb.CommandName + ";";
				} else {
					WalkGridSetClick(c);
				}
			}
		}

		private string ResetSortToColumn(string sSortField) {
			if (this.SortField.Length < 1) {
				this.SortField = sSortField;
				this.SortDir = string.Empty;
			} else {
				if (this.SortField.ToLowerInvariant() != sSortField.ToLowerInvariant()) {
					this.SortDir = string.Empty;  //previous sort not the same field, force ASC
				}
				this.SortField = sSortField;
			}

			if (this.SortDir.Trim().ToUpperInvariant().IndexOf("ASC") < 0) {
				this.SortDir = "ASC";
			} else {
				this.SortDir = "DESC";
			}

			sSortField = string.Format("{0}   {1}", this.SortField, this.SortDir);
			return sSortField;
		}

		private object GetPropertyValue(object obj, string property) {
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo.GetValue(obj, null);
		}

		private bool TestIfPropExists(object obj, string property) {
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo == null ? false : true;
		}

		public List<string> GetProperties(object obj) {
			PropertyInfo[] info = obj.GetType().GetProperties();

			List<string> props = (from i in info.AsEnumerable()
								  select i.Name).ToList();
			return props;
		}

		private void WalkGridForHeadings(Control ctrl) {
			if (string.IsNullOrEmpty(this.SortField) || string.IsNullOrEmpty(this.SortDir)) {
				this.SortParm = string.Empty;
			}

			WalkGridForHeadings(ctrl, this.SortField, this.SortDir);
		}

		private void WalkGridForHeadings(Control ctrl, string sSortFld, string sSortDir) {
			sSortFld = sSortFld.ToLowerInvariant();
			sSortDir = sSortDir.ToLowerInvariant();

			foreach (Control c in ctrl.Controls) {
				if (c is LinkButton) {
					LinkButton lb = (LinkButton)c;
					if (sSortFld == lb.CommandName.ToLowerInvariant()) {
						//don't add the arrows if alread sorted!
						if (lb.Text.IndexOf("#x25B") < 0) {
							if (sSortDir != "asc") {
								lb.Text += SortDownIndicator;
							} else {
								lb.Text += SortUpIndicator;
							}
						}
						break;
					}
				} else {
					WalkGridForHeadings(c, sSortFld, sSortDir);
				}
			}
		}
	}

	//======================================

	public class CarrotGridViewDesigner : DataBoundControlDesigner {

		public override string GetDesignTimeHtml() {
			Control ctrl = base.ViewControl;
			var myctrl = (CarrotGridView)ctrl;

			string sType = myctrl.GetType().ToString().Replace(myctrl.GetType().Namespace + ".", "Carrot, ");
			string sID = myctrl.ID;
			string sTextOut = "<span>[" + sType + " - " + sID + "]</span> <br />";

			StringBuilder sb = new StringBuilder();
			sb.AppendLine(sTextOut);

			sb.AppendLine(DoGrid(ctrl));

			return sb.ToString();
		}

		public string DoGrid(Control ctrl) {
			CarrotGridView theGrid = (CarrotGridView)ctrl;

			Table table = new Table();
			table.CssClass = theGrid.CssClass;
			ctrl.Page.Controls.Add(table);

			TableHeaderRow trh = new TableHeaderRow();
			trh.CssClass = theGrid.HeaderStyle.CssClass;
			table.Rows.Add(trh);

			foreach (DataControlField col in theGrid.Columns) {
				TableHeaderCell thc = new TableHeaderCell();
				trh.Controls.Add(thc);
				thc.Text = col.HeaderText;
			}

			int ps = 5;
			if (theGrid.PageSize > 0 && theGrid.PageSize < 30) {
				ps = theGrid.PageSize;
			}

			for (int r = 0; r < ps; r++) {
				TableRow tr = new TableRow();
				var altRow = (r % 2 == 0);
				if (altRow) {
					tr.CssClass = theGrid.RowStyle.CssClass;
				} else {
					tr.CssClass = theGrid.AlternatingRowStyle.CssClass;
				}
				table.Rows.Add(tr);

				foreach (DataControlField col in theGrid.Columns) {
					TableCell tc = new TableCell();
					tc.Text = " &nbsp; ";
					tr.Controls.Add(tc);

					if (col is BoundField) {
						var bf = (BoundField)col;
						tc.Text = bf.DataField;
					}

					if (col is TemplateField) {
						var tf = (TemplateField)col;
						try {
							var ph = new PlaceHolder();
							tc.Controls.Add(ph);
							tf.ItemTemplate.InstantiateIn(ph);
						} catch { }
					}

					if (col is CarrotHeaderSortTemplateField) {
						var ctf = (CarrotHeaderSortTemplateField)col;
						tc.Text = ctf.DataField;
						try {
							if (ctf.ShowBooleanImage || ctf.ShowEnumImage) {
								ITemplate imgTemplate = null;
								var ph = new PlaceHolder();
								tc.Controls.Add(ph);

								if (ctf.ShowBooleanImage || !ctf.ShowEnumImage) {
									imgTemplate = new CarrotBooleanImageItemTemplate(ctf, r, altRow);
								}

								if (ctf.ShowEnumImage) {
									imgTemplate = new CarrotImageItemTemplate(ctf, r, altRow);
								}

								if (imgTemplate != null) {
									ctf.ItemTemplate = imgTemplate;
									ctf.ItemTemplate.InstantiateIn(ph);
								}
							}
						} catch (Exception ex) { }
					}
				}
			}

			return WebControlHelper.RenderCtrl(table);
		}
	}
}