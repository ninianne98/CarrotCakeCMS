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
	public partial class FaqListTop : FaqPublicBase {

		[Description("Rows to return")]
		public int TakeTop { get; set; }

		protected int iFaq = 0;

		public string FaqCounter() {
			iFaq++;
			return iFaq.ToString();
		}


		protected void LoadValues() {
			if (PublicParmValues.Count > 0) {
				TakeTop = 3;

				try {
					string sFoundVal = GetParmValue("TakeTop", String.Empty);

					if (!string.IsNullOrEmpty(sFoundVal)) {
						TakeTop = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			LoadValues();

			var lstFAQ = GetListTop(TakeTop);

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