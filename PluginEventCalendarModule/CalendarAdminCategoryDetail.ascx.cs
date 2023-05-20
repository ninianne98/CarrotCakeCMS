using Carrotware.CMS.Interface;
using System;
using System.Linq;

/*
* CarrotCake CMS - Event Calendar
* http://www.carrotware.com/
*
* Copyright 2013, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: June 2013
*/

namespace Carrotware.CMS.UI.Plugins.EventCalendarModule {

	public partial class CalendarAdminCategoryDetail : AdminModule {
		protected Guid ItemGuid = Guid.Empty;

		public string CancelURL {
			get {
				return CreateLink(CalendarHelper.PluginKeys.EventAdminCategoryList.ToString());
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();

			if (!IsPostBack) {
				txtFgColor.Text = CalendarHelper.HEX_Black;
				txtBgColor.Text = CalendarHelper.HEX_White;

				var itm = CalendarHelper.GetCalendarCategory(ItemGuid);

				if (itm != null) {
					txtCategoryName.Text = itm.CategoryName;
					txtFgColor.Text = itm.CategoryFGColor;
					txtBgColor.Text = itm.CategoryBGColor;
				}
			}
		}

		protected void btnSaveButton_Click(object sender, EventArgs e) {
			bool bAdd = false;

			using (CalendarDataContext db = CalendarDataContext.GetDataContext()) {
				var itm = (from c in db.carrot_CalendarEventCategories
						   where c.CalendarEventCategoryID == ItemGuid
						   select c).FirstOrDefault();

				if (itm == null) {
					bAdd = true;
					ItemGuid = Guid.NewGuid();
					itm = new carrot_CalendarEventCategory();
					itm.CalendarEventCategoryID = ItemGuid;
					itm.SiteID = SiteID;
				}

				itm.CategoryName = txtCategoryName.Text;
				itm.CategoryFGColor = txtFgColor.Text;
				itm.CategoryBGColor = txtBgColor.Text;

				if (bAdd) {
					db.carrot_CalendarEventCategories.InsertOnSubmit(itm);
				}

				db.SubmitChanges();
			}

			Response.Redirect(CreateLink(ModuleName, String.Format("id={0}", ItemGuid)));
		}
	}
}