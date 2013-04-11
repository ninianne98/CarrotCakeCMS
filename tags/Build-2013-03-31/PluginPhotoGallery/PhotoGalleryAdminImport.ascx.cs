using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdminImport : AdminModule {
		Guid gTheID = Guid.Empty;

		private GalleryExportList lstGalleries = null;

		protected void Page_Load(object sender, EventArgs e) {
			pnlReview.Visible = false;
			pnlUpload.Visible = false;

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			} else {
				pnlUpload.Visible = true;
			}

			lblWarning.Text = "";
			lblWarning.Attributes["style"] = "color: #000000;";

			if (gTheID != Guid.Empty) {
				lstGalleries = GalleryExportList.GetGalleryExport(gTheID);
			}

			if (!IsPostBack && gTheID != Guid.Empty) {
				pnlReview.Visible = true;
				LoadLists();
			}
		}


		private void LoadLists() {

			List<GalleryGroup> lst = (from g in lstGalleries.TheGalleries
									  select g.TheGallery).ToList();

			gvPages.DataSource = lst;
			gvPages.DataBind();

			lblPages.Text = lst.Count.ToString();
		}


		protected void btnUpload_Click(object sender, EventArgs e) {
			string sXML = "";
			if (upFile.HasFile) {
				using (StreamReader sr = new StreamReader(upFile.FileContent)) {
					sXML = sr.ReadToEnd();
				}
			}
			string sTest = "";
			if (!string.IsNullOrEmpty(sXML) && sXML.Length > 500) {

				sTest = sXML.Substring(0, 250).ToLower();

				try {
					if (sTest.Contains("<galleryexportlist xmlns:xsi=\"http://www.w3.org/2001/xmlschema-instance\" xmlns:xsd=\"http://www.w3.org/2001/xmlschema\">")) {

						GalleryExportList galExp = GalleryExportList.DeserializeGalleryExport(sXML);
						Guid gKey = GalleryExportList.SaveSerializedDataExport(galExp);

						Response.Redirect(CreateLink("GalleryImport", String.Format("id={0}", gKey.ToString())));
					}

					lblWarning.Text = "File did not appear to match an expected format.";
					lblWarning.Attributes["style"] = "color: #990000;";

				} catch (Exception ex) {
					lblWarning.Text = ex.ToString();
					lblWarning.Attributes["style"] = "color: #990000;";
				}

			} else {
				lblWarning.Text = "No file appeared in the upload queue.";
				lblWarning.Attributes["style"] = "color: #990000;";
			}

		}

		protected void btnCreate_Click(object sender, EventArgs e) {

			BuildInstallList();

			Response.Redirect(CreateLink("GalleryList"));
		}


		protected void BuildInstallList() {

			pnlReview.Visible = true;
			SiteData site = SiteData.CurrentSite;

			using (GalleryHelper gh = new GalleryHelper(site.SiteID)) {

				foreach (GridViewRow row in gvPages.Rows) {
					Guid gGallery = Guid.Empty;

					CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

					if (chkSelect.Checked) {
						HiddenField hdnGalleryID = (HiddenField)row.FindControl("hdnGalleryID");

						gGallery = new Guid(hdnGalleryID.Value);

						GalleryGroup grpImp = (from g in lstGalleries.TheGalleries
											   where g.TheGallery.GalleryID == gGallery
											   select g.TheGallery).FirstOrDefault();

						GalleryGroup grpSite = gh.GalleryGroupGetByName(grpImp.GalleryTitle);

						if (grpSite == null) {
							grpSite = grpImp;
							grpSite.SiteID = site.SiteID;
						}
						grpSite.Save();

						grpImp.GalleryImages.ForEach(q => q.GalleryImageID = Guid.NewGuid());
						grpImp.GalleryImages.ForEach(q => q.GalleryID = grpSite.GalleryID);

						foreach (GalleryImageEntry imgImp in grpImp.GalleryImages) {
							GalleryImageEntry imgSite = gh.GalleryImageEntryGetByFilename(grpSite.GalleryID, imgImp.GalleryImage);

							if (imgSite == null) {
								imgSite = imgImp;
								imgSite.GalleryID = grpSite.GalleryID;
							}
							imgSite.Save();
						}
					}
				}
			}
		}
	}
}