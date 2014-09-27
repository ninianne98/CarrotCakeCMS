using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS - Event Calendar
* http://www.carrotware.com/
*
* Copyright 2013, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
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

				CalendarHelper.BindDropDownList(ddlFGColor, CalendarHelper.ColorCodes, CalendarHelper.HEX_Black);
				CalendarHelper.BindDropDownList(ddlBGColor, CalendarHelper.ColorCodes, CalendarHelper.HEX_White);

				var itm = CalendarHelper.GetCalendarCategory(ItemGuid);

				if (itm != null) {
					txtCategoryName.Text = itm.CategoryName;
					ddlFGColor.SelectedValue = itm.CategoryFGColor;
					ddlBGColor.SelectedValue = itm.CategoryBGColor;
				}

			}
		}

		protected void btnSaveButton_Click(object sender, EventArgs e) {
			bool bAdd = false;

			using (CalendarDataContext db = new CalendarDataContext()) {

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
				itm.CategoryFGColor = ddlFGColor.SelectedValue;
				itm.CategoryBGColor = ddlBGColor.SelectedValue;

				if (bAdd) {
					db.carrot_CalendarEventCategories.InsertOnSubmit(itm);
				}

				db.SubmitChanges();
			}

			Response.Redirect(CreateLink(ModuleName, String.Format("id={0}", ItemGuid)));
		}



	}
}