using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQ2Module {

	public partial class AdminList : AdminModule {
		protected Guid ItemGuid = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();

			using (FaqHelper fh = new FaqHelper(SiteID)) {
				var itm = fh.CategoryGetByID(ItemGuid);

				litFaqName.Text = itm.FAQTitle;
			}

			lnkAdd.NavigateUrl = CreateLink("FAQDetail", String.Format("cat={0}", ItemGuid));

			LoadData();
		}

		private void LoadData() {

			using (FaqHelper fh = new FaqHelper(SiteID)) {
				var lstCont = fh.FaqItemListGetByFaqCategoryID(ItemGuid);

				gvPages.DataSource = lstCont;
				gvPages.DataBind();
			}
		}


	}
}