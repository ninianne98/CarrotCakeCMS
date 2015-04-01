using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;

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