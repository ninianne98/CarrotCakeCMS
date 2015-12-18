using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
			if (PublicParmValues.Any()) {
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