using System;

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

	public partial class ucSitePageDrillDown : AdminBaseUserControl {
		public Guid RootContentID { get; set; }

		private Guid? _selectedPage = Guid.Empty;

		public Guid? SelectedPage {
			get {
				if (!String.IsNullOrEmpty(txtParent.Text)) {
					_selectedPage = new Guid(txtParent.Text);
				} else {
					_selectedPage = null;
				}
				return _selectedPage;
			}

			set {
				if (value.HasValue) {
					_selectedPage = value;
					txtParent.Text = value.ToString();
				} else {
					_selectedPage = null;
					txtParent.Text = "";
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
		}

	}
}