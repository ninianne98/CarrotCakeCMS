using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System;

namespace Carrotware.CMS.UI.Plugins.PhotoGallery {

	public partial class PhotoGalleryAdminMetaData : AdminModule {
		private Guid gTheID = Guid.Empty;
		public string imageFile = string.Empty;

		protected FileDataHelper helpFile = new FileDataHelper();

		protected void Page_Load(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			}
			if (!string.IsNullOrEmpty(Request.QueryString["parm"])) {
				imageFile = CMSConfigHelper.DecodeBase64(Request.QueryString["parm"].ToString());
			}

			ValidateGalleryImage(imageFile);

			litImgName.Text = imageFile;
			ImageSizer1.ImageUrl = imageFile;
			ImageSizer1.ToolTip = imageFile;

			if (!IsPostBack) {
				LoadForm();
			}
		}

		private void LoadForm() {
			GalleryHelper gh = new GalleryHelper(SiteID);
			var meta = gh.GalleryMetaDataGetByFilename(imageFile);

			if (meta != null) {
				txtMetaInfo.Text = meta.ImageMetaData;
				txtTitle.Text = meta.ImageTitle;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			GalleryHelper gh = new GalleryHelper(SiteID);
			var meta = gh.GalleryMetaDataGetByFilename(imageFile);
			ValidateGalleryImage(imageFile);

			if (meta == null) {
				meta = new GalleryMetaData();
				meta.GalleryImageMetaID = Guid.Empty;
				meta.SiteID = SiteID;
				meta.GalleryImage = imageFile.ToLower();
			}

			meta.ImageMetaData = txtMetaInfo.Text;
			meta.ImageTitle = txtTitle.Text;

			meta.ValidateGalleryImage();

			meta.Save();

			Response.Redirect(SiteData.CurrentScriptName + "?" + Request.QueryString.ToString());
		}

		protected void ValidateGalleryImage(string imageFile) {
			if (imageFile.Contains("../") || imageFile.Contains(@"..\")) {
				throw new Exception("Cannot use relative paths.");
			}
			if (imageFile.Contains(":")) {
				throw new Exception("Cannot specify drive letters.");
			}
			if (imageFile.Contains("//") || imageFile.Contains(@"\\")) {
				throw new Exception("Cannot use UNC paths.");
			}
			if (imageFile.Contains("<") || imageFile.Contains(">")) {
				throw new Exception("Cannot include html tags.");
			}
		}

	}
}