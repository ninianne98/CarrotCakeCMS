using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQ2Module {
	public class FaqPublicBase : WidgetParmDataUserControl {

		[Description("FAQ to display")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstFAQID")]
		public Guid FaqCategoryID { get; set; }


		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstFAQID {
			get {
				if (SiteID == Guid.Empty) {
					SiteID = SiteData.CurrentSiteID;
				}
				Dictionary<string, string> _dict = null;

				using (FaqHelper fh = new FaqHelper(SiteID)) {

					_dict = (from c in fh.CategoryListGetBySiteID(SiteID)
							 orderby c.FAQTitle
							 select c).ToList().ToDictionary(k => k.FaqCategoryID.ToString(), v => v.FAQTitle);
				}

				return _dict;
			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			if (PublicParmValues.Count > 0) {

				try {
					string sFoundVal = GetParmValue("FaqCategoryID", Guid.Empty.ToString());

					if (!string.IsNullOrEmpty(sFoundVal)) {
						FaqCategoryID = new Guid(sFoundVal);
					}
				} catch (Exception ex) { }

			}

		}


		public List<carrot_FaqItem> GetList() {

			using (FaqHelper fh = new FaqHelper(SiteID)) {

				return fh.FaqItemListPublicGetByFaqCategoryID(FaqCategoryID, SiteID);
			}
		}


		public List<carrot_FaqItem> GetListTop(int takeTop) {

			using (FaqHelper fh = new FaqHelper(SiteID)) {

				return fh.FaqItemListPublicTopGetByFaqCategoryID(FaqCategoryID, SiteID, takeTop);
			}
		}

		public carrot_FaqItem GetRandomItem() {

			using (FaqHelper fh = new FaqHelper(SiteID)) {

				return fh.FaqItemListPublicRandGetByFaqCategoryID(FaqCategoryID, SiteID);
			}
		}

	}
}