using Carrotware.CMS.Interface;
using System;

namespace Carrotware.CMS.UI.Plugins.FAQ2Module {

	public partial class AdminCategoryList : AdminModule {

		protected void Page_Load(object sender, EventArgs e) {
			lnkAdd.NavigateUrl = CreateLink("CategoryDetail");

			LoadData();
		}

		private void LoadData() {
			using (FaqHelper fh = new FaqHelper(SiteID)) {
				var lstCont = fh.CategoryListGetBySiteID();

				gvPages.DataSource = lstCont;
				gvPages.DataBind();
			}
		}
	}
}