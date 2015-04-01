using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQ2Module {

	public partial class AdminDetail : AdminModule {
		protected Guid ItemGuid = Guid.Empty;
		protected Guid CatGuid = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();
			CatGuid = ParmParser.GetGuidParameterFromQuery("cat");

			if (ItemGuid != Guid.Empty) {

				cmdSave.Text = "Save";

			} else {
				ItemGuid = Guid.NewGuid();
				txtID.Text = ItemGuid.ToString();
				txtCatID.Text = CatGuid.ToString();

				cmdSave.Text = "Add";
				cmdClone.Visible = false;
				cmdDelete.Visible = false;
				btnDelete.Visible = false;
			}


			if (!IsPostBack) {
				using (FaqHelper fh = new FaqHelper(SiteID)) {
					var itm = fh.FaqItemGetByID(ItemGuid);

					if (itm != null) {
						txtCaption.Text = itm.Caption;
						reQuestion.Text = itm.Question;
						reAnswer.Text = itm.Answer;

						txtSort.Text = itm.ItemOrder.ToString();
						chkActive.Checked = itm.IsActive;

						CatGuid = itm.FaqCategoryID;

						txtID.Text = itm.FaqItemID.ToString();
						txtCatID.Text = CatGuid.ToString();
					}
				}
			}

			txtID.Text = ItemGuid.ToString();

			lnkBack.NavigateUrl = CreateLink("FAQList", String.Format("id={0}", CatGuid));
		}


		protected void cmdAdd_Click(object sender, System.EventArgs e) {
			CatGuid = new Guid(txtCatID.Text);
			ItemGuid = Guid.NewGuid();
			txtID.Text = ItemGuid.ToString();

			Save();
		}

		protected void cmdSave_Click(object sender, System.EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();

			Save();
		}

		protected void cmdDelete_Click(object sender, System.EventArgs e) {
			CatGuid = new Guid(txtCatID.Text);

			using (FaqHelper fh = new FaqHelper(SiteID)) {

				fh.DeleteItem(ItemGuid);

			}

			string filePath = CreateLink("FAQList", String.Format("id={0}", CatGuid));

			Response.Redirect(filePath);

		}

		protected void Save() {

			using (FaqHelper fh = new FaqHelper(SiteID)) {

				var itm = fh.FaqItemGetByID(ItemGuid);

				if (itm == null || ItemGuid == Guid.Empty) {
					ItemGuid = Guid.NewGuid();
					itm = new carrot_FaqItem();
					itm.FaqItemID = ItemGuid;
					itm.FaqCategoryID = CatGuid;
				}

				if (itm != null) {
					itm.Caption = txtCaption.Text;
					itm.Answer = reAnswer.Text;
					itm.Question = reQuestion.Text;

					itm.IsActive = chkActive.Checked;
					itm.ItemOrder = int.Parse(txtSort.Text);

					fh.Save(itm);

					ItemGuid = itm.FaqItemID;
				}

			}

			string filePath = CreateLink(ModuleName, string.Format("id={0}", ItemGuid));

			Response.Redirect(filePath);
		}

		protected void btnCanel_Click(object sender, EventArgs e) {
			CatGuid = new Guid(txtCatID.Text);

			string filePath = CreateLink("FAQList", String.Format("id={0}", CatGuid));

			Response.Redirect(filePath);
		}


	}
}