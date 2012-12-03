using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdminCategoryList : AdminModule {


		protected void Page_Load(object sender, EventArgs e) {

			LoadData();
		}

		private void LoadData() {
			PhotoGalleryDataContext db = new PhotoGalleryDataContext();

			var lstCont = (from c in db.tblGalleries
						   where c.SiteID == SiteID
						   select c).ToList();


			gvPages.DataSource = lstCont;
			gvPages.DataBind();

		}


	}
}