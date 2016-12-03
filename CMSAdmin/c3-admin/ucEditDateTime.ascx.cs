using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin.c3_admin {

	public partial class ucEditDateTime : System.Web.UI.UserControl {

		public ucEditDateTime() {
			this.ValidationGroup = String.Empty;
			this.OnBlur = String.Empty;
			this.OnChange = String.Empty;
		}

		public DateTime TheDate { get; set; }

		public TextBox DateField { get { return txtDate; } }

		public TextBox TimeField { get { return txtTime; } }

		public string ValidationGroup { get; set; }

		public string OnBlur { get; set; }

		public string OnChange { get; set; }

		public void SetDate(DateTime date) {
			this.TheDate = date;

			txtDate.Text = this.TheDate.ToString(Helper.ShortDatePattern);
			txtTime.Text = this.TheDate.ToString(Helper.ShortTimePattern);
		}

		public DateTime GetDate() {
			this.TheDate = Convert.ToDateTime(txtDate.Text + " " + txtTime.Text);

			return this.TheDate;
		}

		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			if (!IsPostBack) {
				SetDate(SiteData.CurrentSite.Now);
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			if (!String.IsNullOrEmpty(this.ValidationGroup)) {
				txtDate.ValidationGroup = this.ValidationGroup;
				txtTime.ValidationGroup = this.ValidationGroup;
			}

			if (!String.IsNullOrEmpty(this.OnChange)) {
				txtDate.Attributes["onchange"] = this.OnChange;
				txtTime.Attributes["onchange"] = this.OnChange;
			}

			if (!String.IsNullOrEmpty(this.OnBlur)) {
				txtDate.Attributes["onblur"] = this.OnBlur;
				txtTime.Attributes["onblur"] = this.OnBlur;
			}
		}
	}
}