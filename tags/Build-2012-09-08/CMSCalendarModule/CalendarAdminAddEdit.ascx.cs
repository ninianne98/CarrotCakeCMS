using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
using Carrotware.CMS.Core;


namespace Carrotware.CMS.UI.Plugins.CalendarModule {
	public partial class CalendarAdminAddEdit : AdminModule {

		protected dbCalendarDataContext db = new dbCalendarDataContext();
		protected Guid ItemGuid = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			if (SiteID == Guid.Empty) {
				SiteID = SiteData.CurrentSiteID;
			}

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

			var sQueryStringFile = CreateLink("CalendarAdmin");

			Response.Redirect(sQueryStringFile);

		}

		protected void Save() {
			bool bAdd = false;

			var itm = (from c in db.tblCalendars
					   where c.CalendarID == ItemGuid
					   select c).FirstOrDefault();

			if (itm == null) {
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

			var sQueryStringFile = CreateLink(ModuleName, "id=" + ItemGuid);

			Response.Redirect(sQueryStringFile);

		}



	}
}