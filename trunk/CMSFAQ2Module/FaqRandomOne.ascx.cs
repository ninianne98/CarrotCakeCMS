using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using System.ComponentModel;


namespace Carrotware.CMS.UI.Plugins.FAQ2Module {
	public partial class FaqRandomOne : FaqPublicBase {

		[Description("Link Text")]
		public string LinkText { get; set; }

		[Description("Link URL")]
		public string LinkUrl { get; set; }

		protected void LoadValues() {
			if (PublicParmValues.Count > 0) {

				try {
					string sFoundVal = GetParmValue("LinkText", String.Empty);

					if (!string.IsNullOrEmpty(sFoundVal)) {
						LinkText = sFoundVal;
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("LinkUrl", String.Empty);

					if (!string.IsNullOrEmpty(sFoundVal)) {
						LinkUrl = sFoundVal;
					}
				} catch (Exception ex) { }
			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			LoadValues();

			if (string.IsNullOrEmpty(LinkUrl)) {
				pnlButton.Visible = false;
			}
			if (string.IsNullOrEmpty(LinkText)) {
				LinkText = "More";
			}

			if (FaqCategoryID != Guid.Empty) {

				var faqItem = GetRandomItem();

				if (faqItem == null) {
					pnlWrap.Visible = false;
				} else {
					litQuest.Text = faqItem.Question;
					litAns.Text = faqItem.Answer;
					pnlWrap.Visible = true;
				}

			} else {
				pnlWrap.Visible = false;
			}

		}


	}
}