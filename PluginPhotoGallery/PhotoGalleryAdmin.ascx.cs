using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdmin : AdminModule {

		Guid gTheID = Guid.Empty;
		protected PhotoGalleryDataContext db = new PhotoGalleryDataContext();
		protected FileDataHelper helpFile = new FileDataHelper();


		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			}

			if (!IsPostBack) {
				txtDate.Text = DateTime.Now.ToShortDateString();
				LoadLists();
			}
		}

		protected string SetSitePath(string sPath) {
			return FileDataHelper.MakeFileFolderPath(sPath);
		}

		protected void LoadLists() {

			var gal = (from g in db.tblGalleries
					   where g.GalleryID == gTheID
					   select g).FirstOrDefault();

			if (gal != null) {
				litGalleryName.Text = gal.GalleryTitle;

				rpGallery.DataSource = (from g in db.tblGalleryImages
										where g.GalleryID == gTheID
										orderby g.ImageOrder ascending
										select helpFile.GetFileInfo(g.GalleryImage, g.GalleryImage)).ToList();

				rpGallery.DataBind();
			}

			SetSourceFiles(null);

		}

		protected Dictionary<int, string> ParseGalleryImages() {

			var sImageList = txtGalleryOrder.Text;
			Dictionary<int, string> lstImages = new Dictionary<int, string>();

			sImageList = sImageList.Replace("\r\n", "\n");
			sImageList = sImageList.Replace("\r", "\n");
			var arrImageRows = sImageList.Split('\n');

			int iRow = 0;
			foreach (string arrImgCell in arrImageRows) {
				if (!string.IsNullOrEmpty(arrImgCell)) {
					var w = arrImgCell.Split('\t');
					var img = w[1];
					if (!string.IsNullOrEmpty(img)) {
						lstImages.Add(iRow, img);
					}
				}
				iRow++;
			}

			return lstImages;
		}


		protected void btnApply_Click(object sender, EventArgs e) {
			SetSrcFiles();
		}


		protected void chkFilter_CheckedChanged(object sender, EventArgs e) {
			SetSrcFiles();
		}

		protected void SetSrcFiles() {

			Dictionary<int, string> lstImages = ParseGalleryImages();

			DateTime? dtFilter = null;

			if (chkFilter.Checked) {
				dtFilter = DateTime.Now.Date;
				try {
					dtFilter = Convert.ToDateTime(txtDate.Text);
				} catch {
				}
			}

			rpGallery.DataSource = (from g in lstImages
									orderby g.Key ascending
									select helpFile.GetFileInfo(g.Value, g.Value)).ToList();
			rpGallery.DataBind();

			SetSourceFiles(dtFilter);
		}


		protected void SetSourceFiles(DateTime? dtFilter) {

			List<FileData> flsWorking = new List<FileData>();
			List<FileData> fldrWorking = new List<FileData>();

			fldrWorking = helpFile.SpiderDeepFoldersFD("/");

			foreach (var f in fldrWorking) {
				var fls = helpFile.GetFiles(f.FolderPath);

				flsWorking = (from m in flsWorking.Union(fls).ToList()
							  where m.MimeType.StartsWith("image")
							  select m).ToList();
			}

			flsWorking = flsWorking.Where(x => x.MimeType.StartsWith("image") && (x.FolderPath.ToLower().StartsWith("/manage/") == false)).ToList();

			if (dtFilter != null) {
				DateTime dtFlt = Convert.ToDateTime(dtFilter);
				flsWorking = flsWorking.Where(x => x.FileDate >= dtFlt.AddDays(-14) && x.FileDate <= dtFlt.AddDays(14)).ToList();
			}

			rpFiles.DataSource = flsWorking.OrderBy(x => x.FileName).OrderBy(x => x.FolderPath).ToList();
			rpFiles.DataBind();
		}

		protected void btnSave_Click(object sender, EventArgs e) {

			Dictionary<int, string> lstImages = ParseGalleryImages();
			int iPos = 0;

			foreach (var img in lstImages) {
				var bAdding = false;

				if (!string.IsNullOrEmpty(img.Value)) {

					var theImg = (from g in db.tblGalleryImages
								  where g.GalleryID == gTheID
								  && g.GalleryImage.ToLower() == img.Value
								  orderby g.ImageOrder ascending
								  select g).FirstOrDefault();

					if (theImg == null) {
						bAdding = true;
						theImg = new tblGalleryImage();
						theImg.GalleryImage = img.Value;
						theImg.GalleryImageID = Guid.NewGuid();
						theImg.GalleryID = gTheID;
					}
					theImg.ImageOrder = iPos;

					if (bAdding) {
						db.tblGalleryImages.InsertOnSubmit(theImg);
					}
				}
				iPos++;
			}


			if (lstImages.Count > 0) {
				List<string> lst = (from l in lstImages
									select l.Value.ToLower()).ToList();

				var lstDel = (from g in db.tblGalleryImages
							  where g.GalleryID == gTheID
							  && !lst.Contains(g.GalleryImage.ToLower())
							  select g).ToList();

				foreach (var itm in lstDel) {
					db.tblGalleryImages.DeleteOnSubmit(itm);
				}
			}

			db.SubmitChanges();

			var QueryStringFile = CreateLink(ModuleName, "id=" + gTheID.ToString());


			Response.Redirect(QueryStringFile);
		}


	}
}