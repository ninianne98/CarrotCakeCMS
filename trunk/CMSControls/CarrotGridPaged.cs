using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.Web.UI.Controls;
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
	public class CarrotGridPaged : WebControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int PageSize {
			get {
				String s = (String)ViewState["PageSize"];
				return ((s == null) ? 10 : int.Parse(s));
			}
			set {
				ViewState["PageSize"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int PageNumber {
			get {
				String s = (String)ViewState["PageNumber"];
				return ((s == null) ? 1 : int.Parse(s));
			}
			set {
				hdnPageNbr.Value = value.ToString();
				ViewState["PageNumber"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int TotalRecords {
			get {
				String s = (String)ViewState["TotalRecords"];
				return ((s == null) ? 1 : int.Parse(s));
			}
			set {
				ViewState["TotalRecords"] = value.ToString();
			}
		}


		public bool IsPostBack {
			get {
				string sReq = "GET";
				try { sReq = HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"].ToString().ToUpper(); } catch { }
				return sReq != "GET" ? true : false;
			}
		}

		private HiddenField hdnPageNbr = new HiddenField();
		private bool bHeadClicked = true;
		private string sBtnName = "lnkPagerBtn";


		public string SortingBy { get; set; }

		[
		Category("Behavior"),
		Description("The CarrotGridView "),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		Browsable(true),
		TemplateContainer(typeof(CarrotGridView)),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public CarrotGridView TheGrid { get; set; }


		[
		Category("Behavior"),
		Description("The Repeater "),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		Browsable(true),
		TemplateContainer(typeof(Repeater)),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public Repeater ThePager { get; set; }

		protected override void OnInit(EventArgs e) {
			if (this.DataSource == null) {
				this.DataSource = new List<object>();
			}

			hdnPageNbr.ID = "hdnPageNbr";
			this.Controls.Add(hdnPageNbr);

			TheGrid.ID = "gridData";
			this.Controls.Add(TheGrid);

			if (ThePager == null) {
				ThePager = new Repeater();
			}

			ThePager.ID = "repeaterPager";
			this.Controls.Add(ThePager);

			base.OnInit(e);

		}

		ControlUtilities cu = new ControlUtilities();

		private Repeater GetCtrl() {
			cu = new ControlUtilities(this);
			Control userControl = cu.CreateControlFromResource(this.GetType(), "Carrotware.CMS.UI.Controls.ucFancyPager.ascx");
			Repeater r = (Repeater)cu.FindControl("rpPager", userControl);

			return r;
		}

		public void BuildSorting() {
			HttpContext context = HttpContext.Current;
			HttpRequest request = context.Request;

			this.SortingBy = TheGrid.DefaultSort;

			if (!IsPostBack) {
				bHeadClicked = false;
				hdnPageNbr.Value = "1";
				SetSort();
			} else {
				if (request.Form["__EVENTARGUMENT"] != null) {
					string arg = request.Form["__EVENTARGUMENT"].ToString();
					string tgt = request.Form["__EVENTTARGET"].ToString();

					if (tgt.Contains("$lnkHead") && tgt.Contains("$" + TheGrid.ID + "$")) {
						bHeadClicked = true;
					}

					if (tgt.Contains("$" + sBtnName) && tgt.Contains("$" + ThePager.ID + "$")) {
						string[] parms = tgt.Split('$');
						int pg = int.Parse(parms[parms.Length - 1].Replace(sBtnName, ""));
						PageNumber = pg;
						hdnPageNbr.Value = PageNumber.ToString();
						bHeadClicked = false;
					}
				}
			}

			if (PageNumber <= 1 && !string.IsNullOrEmpty(hdnPageNbr.Value)) {
				PageNumber = int.Parse(hdnPageNbr.Value);
			}

			if (IsPostBack) {
				SetSort();
			}
		}



		private void SetSort() {

			string sSort = TheGrid.CurrentSort;
			if (bHeadClicked) {
				sSort = TheGrid.PredictNewSort;
			}

			this.SortingBy = sSort;
		}


		public object DataSource { get; set; }

		public override void DataBind() {

			int TotalPages = 0;

			int iPageNbr = PageNumber - 1;

			TotalPages = TotalRecords / PageSize;

			if ((TotalRecords % PageSize) > 0) {
				TotalPages++;
			}

			ThePager.Visible = true;

			TheGrid.DataSource = this.DataSource;
			TheGrid.DataBind();

			if (ThePager.ItemTemplate == null) {
				Repeater rp = GetCtrl();
				ThePager.HeaderTemplate = rp.HeaderTemplate;
				ThePager.ItemTemplate = rp.ItemTemplate;
				ThePager.FooterTemplate = rp.FooterTemplate;
			}

			if (TotalPages > 1) {
				List<int> pagelist = new List<int>();
				pagelist = Enumerable.Range(1, TotalPages).ToList();

				ThePager.DataSource = pagelist;
				ThePager.DataBind();
			}

			if (TotalPages <= 1) {
				ThePager.Visible = false;
			}

			WalkCtrlsForAssignment(ThePager);

		}

		private void WalkCtrlsForAssignment(Control X) {
			foreach (Control c in X.Controls) {
				if (c is IActivatePageNavItem) {
					IActivatePageNavItem btn = (IActivatePageNavItem)c;
					if (btn.PageNumber == PageNumber) {
						btn.IsSelected = true;
					}
					WalkCtrlsForAssignment(c);
				} else {
					WalkCtrlsForAssignment(c);
				}
			}
		}

	}
}
