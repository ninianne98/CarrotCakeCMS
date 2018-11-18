using Carrotware.CMS.Interface;
using System;

namespace Carrotware.CMS.UI.Plugins.FAQ2Module {

	public partial class AdminCategoryDetail : AdminModule {
		private Guid ItemGuid = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();

			if (!IsPostBack) {
				using (FaqHelper fh = new FaqHelper(SiteID)) {
					var gal = fh.CategoryGetByID(ItemGuid);

					if (gal != null) {
						txtFAQ.Text = gal.FAQTitle;
					}
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			using (FaqHelper fh = new FaqHelper(SiteID)) {
				var fc = fh.CategoryGetByID(ItemGuid);

				if (fc == null || ItemGuid == Guid.Empty) {
					ItemGuid = Guid.NewGuid();
					fc = new carrot_FaqCategory();
					fc.SiteID = SiteID;
					fc.FaqCategoryID = ItemGuid;
				}

				fc.FAQTitle = txtFAQ.Text;

				fh.Save(fc);
			}

			string stringFile = CreateLink("CategoryList");

			Response.Redirect(stringFile);
		}
	}
}