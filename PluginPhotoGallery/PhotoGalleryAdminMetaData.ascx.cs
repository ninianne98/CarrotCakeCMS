using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdminMetaData : AdminModule {

		Guid gTheID = Guid.Empty;
		string sImageFile = "";
		protected PhotoGalleryDataContext db = new PhotoGalleryDataContext();
		protected FileDataHelper helpFile = new FileDataHelper();


		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["parm"])) {
				sImageFile = CMSConfigHelper.DecodeBase64(Request.QueryString["parm"].ToString());
			}

			litImgName.Text = sImageFile;

			if (!IsPostBack) {

				var meta = (from m in db.tblGalleryImageMetas
							where m.SiteID == SiteData.CurrentSiteID
							&& m.GalleryImage.ToLower() == sImageFile.ToLower()
							select m).FirstOrDefault();


				if (meta != null) {
					txtMetaInfo.Text = meta.ImageMetaData;
				}
			}


		}

		protected void btnSave_Click(object sender, EventArgs e) {
			bool bAdd = false;

			var meta = (from m in db.tblGalleryImageMetas
						where m.SiteID == SiteData.CurrentSiteID
						&& m.GalleryImage.ToLower() == sImageFile.ToLower()
						select m).FirstOrDefault();

			if (meta == null) {
				bAdd = true;
				meta = new tblGalleryImageMeta();
				meta.SiteID = SiteID;
				meta.GalleryImage = sImageFile;
			}

			meta.ImageMetaData = txtMetaInfo.Text;

			if (bAdd) {
				db.tblGalleryImageMetas.InsertOnSubmit(meta);
			}

			db.SubmitChanges();

			Response.Redirect(SiteData.CurrentScriptName + "?" + Request.QueryString.ToString());
		}

	}
}