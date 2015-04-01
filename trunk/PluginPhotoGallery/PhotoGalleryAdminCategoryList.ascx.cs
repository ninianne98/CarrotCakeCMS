using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdminGalleryList : AdminModule {


		protected void Page_Load(object sender, EventArgs e) {

			LoadData();
		}

		private void LoadData() {

			GalleryHelper gh = new GalleryHelper(SiteID);

			var lstCont = gh.GalleryGroupListGetBySiteID();

			gvPages.DataSource = lstCont;
			gvPages.DataBind();

		}


	}
}