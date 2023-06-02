using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

	[Designer(typeof(CarrotGridPagedDesigner))]
	[ParseChildren(true, "TheGrid"), PersistChildren(true)]
	[ToolboxData("<{0}:CarrotGridPaged runat=server></{0}:CarrotGridPaged>")]
	public class CarrotGridPaged : DataBoundControl, INamingContainer {

		[Category("Appearance")]
		[DefaultValue(10)]
		public int PageSize {
			get {
				String s = (String)ViewState["PageSize"];
				return ((s == null) ? 10 : int.Parse(s));
			}
			set {
				ViewState["PageSize"] = value.ToString();
			}
		}

		[Category("Appearance")]
		[DefaultValue(1)]
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

		[Category("Appearance")]
		[DefaultValue(1)]
		public int TotalRecords {
			get {
				String s = (String)ViewState["TotalRecords"];
				return ((s == null) ? 1 : int.Parse(s));
			}
			set {
				ViewState["TotalRecords"] = value.ToString();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPostBack {
			get {
				string sReq = "GET";
				try { sReq = HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"].ToString().ToUpperInvariant(); } catch { }
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

			this.TheGrid.ID = "gridData";
			this.Controls.Add(TheGrid);

			if (this.ThePager == null) {
				this.ThePager = new Repeater();
			}

			this.ThePager.ID = "repeaterPager";
			this.Controls.Add(ThePager);

			base.OnInit(e);
		}

		private Repeater GetCtrl() {
			Repeater r = new Repeater();

			try {
				BasicControlUtils cu = new BasicControlUtils(this);
				Control userControl = cu.CreateControlFromResource("Carrotware.Web.UI.Controls.ucFancyPager.ascx");
				r = (Repeater)cu.FindControl("rpPager", userControl);
			} catch { }

			return r;
		}

		public void BuildSorting() {
			HttpContext context = HttpContext.Current;

			if (context != null) {
				HttpRequest request = context.Request;

				this.SortingBy = this.TheGrid.DefaultSort;

				if (!IsPostBack) {
					bHeadClicked = false;
					hdnPageNbr.Value = "1";
					SetSort();
				} else {
					if (request.Form["__EVENTARGUMENT"] != null) {
						string arg = request.Form["__EVENTARGUMENT"].ToString();
						string tgt = request.Form["__EVENTTARGET"].ToString();

						if (tgt.Contains("$lnkHead") && tgt.Contains("$" + this.TheGrid.ID + "$")) {
							bHeadClicked = true;
						}

						if (tgt.Contains("$" + sBtnName) && tgt.Contains("$" + this.ThePager.ID + "$")) {
							string[] parms = tgt.Split('$');
							int pg = int.Parse(parms[parms.Length - 1].Replace(sBtnName, ""));
							this.PageNumber = pg;
							hdnPageNbr.Value = pg.ToString();
							bHeadClicked = false;
						}
					}
				}
			}

			if (this.PageNumber <= 1 && !string.IsNullOrEmpty(hdnPageNbr.Value)) {
				this.PageNumber = int.Parse(hdnPageNbr.Value);
			}

			if (IsPostBack) {
				SetSort();
			}
		}

		private void SetSort() {
			string sSort = string.Empty;
			// if current isn't blank use that, otherwise, default kicks in
			if (!string.IsNullOrEmpty(this.TheGrid.CurrentSort)) {
				sSort = this.TheGrid.CurrentSort;
			} else {
				sSort = this.TheGrid.DefaultSort;
			}
			if (bHeadClicked) {
				sSort = this.TheGrid.PredictNewSort;
			}

			this.SortingBy = sSort;
		}

		[Browsable(false)]
		public override object DataSource {
			get { return this.TheGrid.DataSource; }
			set { this.TheGrid.DataSource = value; }
		}

		public override void DataBind() {
			base.DataBind();
			this.TheGrid.DataBind();

			int iTotalPages = 0;

			int iPageNbr = this.PageNumber - 1;

			iTotalPages = this.TotalRecords / this.PageSize;

			if ((this.TotalRecords % this.PageSize) > 0) {
				iTotalPages++;
			}

			this.ThePager.Visible = true;

			if (this.ThePager.ItemTemplate == null) {
				Repeater rp = GetCtrl();
				this.ThePager.HeaderTemplate = rp.HeaderTemplate;
				this.ThePager.ItemTemplate = rp.ItemTemplate;
				this.ThePager.FooterTemplate = rp.FooterTemplate;
			}

			if (iTotalPages > 1) {
				List<int> pagelist = new List<int>();
				pagelist = Enumerable.Range(1, iTotalPages).ToList();

				this.ThePager.DataSource = pagelist;
				this.ThePager.DataBind();
			}

			if (iTotalPages <= 1) {
				this.ThePager.Visible = false;
			}

			WalkCtrlsForAssignment(this.ThePager);
		}

		private void WalkCtrlsForAssignment(Control ctrl) {
			foreach (Control c in ctrl.Controls) {
				if (c is IActivatePageNavItem) {
					IActivatePageNavItem btn = (IActivatePageNavItem)c;
					if (btn.PageNumber == this.PageNumber) {
						btn.IsSelected = true;
					}
					WalkCtrlsForAssignment(c);
				} else {
					WalkCtrlsForAssignment(c);
				}
			}
		}
	}

	//======================================

	public class CarrotGridPagedDesigner : DataBoundControlDesigner {

		public override string GetDesignTimeHtml() {
			Control ctrl = base.ViewControl;
			var myctrl = (CarrotGridPaged)ctrl;

			string sType = myctrl.GetType().ToString().Replace(myctrl.GetType().Namespace + ".", "Carrot, ");
			string sID = myctrl.ID;
			string sTextOut = "<span>[" + sType + " - " + sID + "]</span> <br />";

			StringBuilder sb = new StringBuilder();
			sb.AppendLine(sTextOut);

			sb.AppendLine(DoGrid(ctrl));
			sb.AppendLine(DoRptr(ctrl));

			return sb.ToString();
		}

		protected string DoGrid(Control ctrl) {
			CarrotGridPaged myctrl = (CarrotGridPaged)ctrl;
			CarrotGridView theGrid = myctrl.TheGrid;

			var gvdesign = new CarrotGridViewDesigner();
			return gvdesign.DoGrid(theGrid);
		}

		protected string DoRptr(Control ctrl) {
			CarrotGridPaged myctrl = (CarrotGridPaged)ctrl;
			Repeater thePager = myctrl.ThePager;

			if (thePager.ItemTemplate == null) {
				Repeater rp = GetCtrl(ctrl);
				thePager.HeaderTemplate = rp.HeaderTemplate;
				thePager.ItemTemplate = rp.ItemTemplate;
				thePager.FooterTemplate = rp.FooterTemplate;
			}
			thePager.Visible = true;

			List<int> pagelist = new List<int>();
			pagelist = Enumerable.Range(1, 7).ToList();

			thePager.DataSource = pagelist;
			thePager.DataBind();

			//since the pretty pager won't show up, append some placeholder text to simulate a pager
			return WebControlHelper.RenderCtrl(thePager)
				+ " <br />\r\n <span>[" + string.Join("],  [", pagelist.Select(x => x.ToString()).ToArray()) + "]</span> <br />"; ;
		}

		private Repeater GetCtrl(Control ctrl) {
			Repeater r = new Repeater();

			try {
				var cu = new BasicControlUtils(ctrl);
				Control userControl = cu.CreateControlFromResource("Carrotware.Web.UI.Controls.ucFancyPager.ascx");
				r = (Repeater)cu.FindControl("rpPager", userControl);
			} catch { }

			return r;
		}
	}
}