using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQModule {
	public partial class FAQAdminAddEdit : BaseShellUserControl, IAdminModule {


		#region IAdminModule Members

		public Guid SiteID { get; set; }

		public Guid ModuleID { get; set; }

		public string QueryStringFragment { get; set; }

		public string QueryStringPattern { get; set; }

		public string ModuleName { get; set; }

		#endregion


		protected dbFAQDataContext db = new dbFAQDataContext();
		protected Guid ItemGuid = Guid.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			//if (SiteID == Guid.Empty) {
			//    SiteID = SiteData.CurrentSiteID;
			//}

			try {
				ItemGuid = new Guid(Request.QueryString["id"].ToString());
				cmdSave.Text = "Save";

			} catch {
				ItemGuid = Guid.NewGuid();
				txtID.Text = ItemGuid.ToString();

				cmdSave.Text = "Add";
				cmdClone.Visible = false;
				cmdDelete.Visible = false;
				btnDelete.Visible = false;
			}


			if (!IsPostBack) {
				var itm = (from c in db.tblFAQs
						   where c.FaqID == ItemGuid
						   select c).FirstOrDefault();

				if (itm != null) {
					reQuestion.Text = itm.Question;
					reAnswer.Text = itm.Answer;
					txtSort.Text = itm.SortOrder.ToString();
					chkActive.Checked = Convert.ToBoolean(itm.IsActive);
				}
			}

			txtID.Text = ItemGuid.ToString();

		}


		protected void cmdAdd_Click(object sender, System.EventArgs e) {
			ItemGuid = Guid.NewGuid();
			txtID.Text = ItemGuid.ToString();
			Save();
		}

		protected void cmdSave_Click(object sender, System.EventArgs e) {
			Save();
		}

		protected void cmdDelete_Click(object sender, System.EventArgs e) {

			var itm = (from c in db.tblFAQs
					   where c.FaqID == ItemGuid
					   select c).FirstOrDefault();

			db.tblFAQs.DeleteOnSubmit(itm);
			db.SubmitChanges();

			var QueryStringFile = CurrentScriptName + "?" + string.Format(QueryStringPattern, "FAQAdmin");

			Response.Redirect(QueryStringFile);

		}

		protected void Save() {
			bool bAdd = false;

			var itm = (from c in db.tblFAQs
					   where c.FaqID == ItemGuid
					   select c).FirstOrDefault();

			if (itm == null) {
				bAdd = true;
				itm = new tblFAQ();
				itm.FaqID = ItemGuid;
			}

			if (itm != null) {
				itm.Answer = reAnswer.Text;
				itm.Question = reQuestion.Text;
				itm.IsActive = chkActive.Checked;
				itm.dtStamp = DateTime.Now;
				itm.SortOrder = int.Parse(txtSort.Text);
				itm.SiteID = SiteID;
			}

			if (bAdd) {
				db.tblFAQs.InsertOnSubmit(itm);
			}
			db.SubmitChanges();

			var QueryStringFile = CurrentScriptName + "?" + string.Format(QueryStringPattern, "FAQAdminAddEdit");

			Response.Redirect(QueryStringFile + "&id=" + ItemGuid);

		}




	}
}