using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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
	public partial class CalendarAdminDetail : AdminModule {

		public string CancelURL {
			get {
				return CreateLink(CalendarHelper.PluginKeys.AdminProfileList.ToString());
			}
		}

		protected Guid ItemGuid = Guid.Empty;
		protected int? selectedDatePattern = null;

		protected void Page_Load(object sender, EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();

			btnCopyButton.Visible = !(ItemGuid == Guid.Empty);
			btnDeleteButton.Visible = !(ItemGuid == Guid.Empty);
			btnCopy.Visible = !(ItemGuid == Guid.Empty);
			btnDelete.Visible = !(ItemGuid == Guid.Empty);

			if (!IsPostBack) {

				CalendarHelper.BindRepeater(rpDays, CalendarHelper.DaysOfTheWeek);

				Dictionary<Guid, string> colors = (from c in CalendarHelper.GetCalendarCategories(SiteID)
												   select new KeyValuePair<Guid, string>(c.CalendarEventCategoryID, string.Format("{0}|{1}", c.CategoryBGColor, c.CategoryFGColor)))
												   .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				CalendarHelper.BindDropDownList(ddlColors, colors);

				var freq = CalendarFrequencyHelper.GetCalendarFrequencies();
				var cat = CalendarHelper.GetCalendarCategories(SiteID);

				txtEventStartDate.Text = SiteData.CurrentSite.Now.ToShortDateString();
				txtEventEndDate.Text = SiteData.CurrentSite.Now.ToShortDateString();

				CalendarHelper.BindDropDownList(ddlRecurr, freq, CalendarFrequencyHelper.GetIDByFrequencyType(CalendarFrequencyHelper.FrequencyType.Once).ToString());
				CalendarHelper.BindDropDownList(ddlCategory, cat);

				var itm = CalendarHelper.GetProfile(ItemGuid);

				if (itm != null) {
					selectedDatePattern = itm.EventRepeatPattern;
					CalendarHelper.BindRepeater(rpDays, CalendarHelper.DaysOfTheWeek);

					txtEventTitle.Text = itm.EventTitle;
					reContent.Text = itm.EventDetail;

					chkIsPublic.Checked = itm.IsPublic;
					chkIsAllDayEvent.Checked = itm.IsAllDayEvent;
					chkIsCancelled.Checked = itm.IsCancelled;
					chkIsCancelledPublic.Checked = itm.IsCancelledPublic;

					txtEventStartDate.Text = itm.EventStartDate.ToShortDateString();
					txtEventEndDate.Text = itm.EventEndDate.ToShortDateString();

					txtRecursEvery.Text = itm.RecursEvery.ToString();

					CalendarHelper.SetTextboxToTimeSpan(txtEventStartTime, itm.EventStartTime);
					CalendarHelper.SetTextboxToTimeSpan(txtEventEndTime, itm.EventEndTime);

					ddlRecurr.SelectedValue = itm.CalendarFrequencyID.ToString();
					ddlCategory.SelectedValue = itm.CalendarEventCategoryID.ToString();
				}
			}
		}


		protected void btnCopy_Click(object sender, EventArgs e) {
			carrot_CalendarEventProfile p = CalendarHelper.CopyEvent(ItemGuid);
			ItemGuid = p.CalendarEventProfileID;


			btnSave_Click(sender, e);
		}


		protected void btnSave_Click(object sender, EventArgs e) {
			bool bAdd = false;

			using (CalendarDataContext db = new CalendarDataContext()) {

				var currItem = (from c in db.carrot_CalendarEventProfiles
								where c.CalendarEventProfileID == ItemGuid
								select c).FirstOrDefault();

				var origItem = new CalendarEvent(currItem);

				if (currItem == null) {
					bAdd = true;
					ItemGuid = Guid.NewGuid();
					currItem = new carrot_CalendarEventProfile();
					currItem.CalendarEventProfileID = ItemGuid;
					currItem.SiteID = SiteID;
					currItem.IsHoliday = false;
					currItem.IsAnnualHoliday = false;
					currItem.RecursEvery = 1;
				}

				currItem.CalendarFrequencyID = new Guid(ddlRecurr.SelectedValue);
				currItem.CalendarEventCategoryID = new Guid(ddlCategory.SelectedValue);

				currItem.EventRepeatPattern = null;

				List<string> days = CalendarHelper.GetCheckedItemStringByValue(rpDays, true, "chkDay");

				if (CalendarFrequencyHelper.GetFrequencyTypeByID(currItem.CalendarFrequencyID) == CalendarFrequencyHelper.FrequencyType.Weekly
					&& days.Count > 0) {

					int dayMask = (from d in days select int.Parse(d)).Sum();

					if (dayMask > 0) {
						currItem.EventRepeatPattern = dayMask;
					}
				}

				currItem.EventTitle = txtEventTitle.Text;
				currItem.EventDetail = reContent.Text;
				currItem.RecursEvery = int.Parse(txtRecursEvery.Text);

				currItem.IsPublic = chkIsPublic.Checked;
				currItem.IsAllDayEvent = chkIsAllDayEvent.Checked;
				currItem.IsCancelled = chkIsCancelled.Checked;
				currItem.IsCancelledPublic = chkIsCancelledPublic.Checked;

				currItem.EventStartDate = Convert.ToDateTime(txtEventStartDate.Text);
				currItem.EventStartTime = CalendarHelper.GetTimeSpanFromTextbox(txtEventStartTime);

				currItem.EventEndDate = Convert.ToDateTime(txtEventEndDate.Text);
				currItem.EventEndTime = CalendarHelper.GetTimeSpanFromTextbox(txtEventEndTime);

				if (bAdd) {
					db.carrot_CalendarEventProfiles.InsertOnSubmit(currItem);
				}

				CalendarFrequencyHelper.SaveFrequencies(db, new CalendarEvent(currItem), origItem);

				db.SubmitChanges();
			}

			Response.Redirect(CreateLink(ModuleName, String.Format("id={0}", ItemGuid)));
		}

		protected void btnDelete_Click(object sender, EventArgs e) {

			CalendarHelper.RemoveEvent(ItemGuid);

			Response.Redirect(CreateLink(CalendarHelper.PluginKeys.AdminProfileList.ToString()));
		}


		public bool GetSelectedDays(int dayInt) {

			if (selectedDatePattern.HasValue) {
				if ((selectedDatePattern.Value & dayInt) != 0) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}

		}


	}
}