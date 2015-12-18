using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarAdminAddEdit : AdminModule {

		protected dbCalendarDataContext db = dbCalendarDataContext.GetDataContext();
		protected Guid ItemGuid = Guid.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();


			if (ItemGuid != Guid.Empty) {

				cmdSave.Text = "Save";

			} else {

				ItemGuid = Guid.NewGuid();
				txtID.Text = ItemGuid.ToString();

				cmdSave.Text = "Add";
				cmdClone.Visible = false;
				cmdDelete.Visible = false;
				btnDelete.Visible = false;
			}


			if (!IsPostBack) {

				txtDate.Text = DateTime.Now.Date.ToShortDateString();

				var itm = (from c in db.tblCalendars
						   where c.CalendarID == ItemGuid
						   select c).FirstOrDefault();

				if (itm != null) {
					txtDate.Text = Convert.ToDateTime(itm.EventDate).ToShortDateString();
					reContent.Text = itm.EventDetail;
					txtEvent.Text = itm.EventTitle;
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

			var itm = (from c in db.tblCalendars
					   where c.CalendarID == ItemGuid
					   select c).FirstOrDefault();

			db.tblCalendars.DeleteOnSubmit(itm);
			db.SubmitChanges();

			string filePath = CreateLink("CalendarAdmin");

			Response.Redirect(filePath);

		}

		protected void Save() {
			bool bAdd = false;

			var itm = (from c in db.tblCalendars
					   where c.CalendarID == ItemGuid
					   select c).FirstOrDefault();

			if (itm == null || ItemGuid == Guid.Empty) {
				ItemGuid = Guid.NewGuid();
				bAdd = true;
				itm = new tblCalendar();
				itm.CalendarID = ItemGuid;
			}

			if (itm != null) {
				itm.EventDate = Convert.ToDateTime(txtDate.Text);
				itm.EventDetail = reContent.Text;
				itm.EventTitle = txtEvent.Text;
				itm.IsActive = chkActive.Checked;
				itm.SiteID = SiteID;
			}


			if (bAdd) {
				db.tblCalendars.InsertOnSubmit(itm);
			}
			db.SubmitChanges();

			string filePath = CreateLink(ModuleName, string.Format("id={0}", ItemGuid));

			Response.Redirect(filePath);
		}


	}
}