using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using System.ComponentModel;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.FAQ2Module {
	public partial class FaqList : FaqPublicBase {


		protected int iFaq = 0;

		public string FaqCounter() {
			iFaq++;
			return iFaq.ToString();
		}


		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			var lstFAQ = GetList();

			if (lstFAQ == null || lstFAQ.Count < 1) {
				rpFAQ.Visible = false;
			} else {
				rpFAQ.DataSource = lstFAQ;
				rpFAQ.DataBind();
				rpFAQ.Visible = true;
			}
		}


	}
}