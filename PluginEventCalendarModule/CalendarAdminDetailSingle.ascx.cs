﻿using Carrotware.CMS.Interface;
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

	public partial class CalendarAdminDetailSingle : AdminModule {

		public string CancelURL {
			get {
				return CreateLink(CalendarHelper.PluginKeys.EventAdminList.ToString());
			}
		}

		protected Guid ItemGuid = Guid.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			ItemGuid = ParmParser.GetGuidIDFromQuery();

			if (!IsPostBack) {
				var itm = CalendarHelper.GetEvent(ItemGuid);
				var itmPro = CalendarHelper.GetProfile(itm.CalendarEventProfileID);

				litTitle.Text = itmPro.EventTitle;
				litTime.Visible = !itmPro.IsAllDayEvent;

				litTime.Text = String.Format(" {0:h:mm tt} ", CalendarHelper.GetFullDateTime(itmPro.EventStartTime));

				litDate.Text = itm.EventDate.ToShortDateString();

				CalendarHelper.SetTextboxToTimeSpan(txtEventStartTime, itm.EventStartTime);
				CalendarHelper.SetTextboxToTimeSpan(txtEventEndTime, itm.EventEndTime);

				chkIsCancelled.Checked = itm.IsCancelled;
				reContent.Text = itm.EventDetail;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			bool bAdd = false;

			using (CalendarDataContext db = CalendarDataContext.GetDataContext()) {
				var currItem = (from c in db.carrot_CalendarEvents
								where c.CalendarEventID == ItemGuid
								select c).FirstOrDefault();

				if (currItem == null) {
					bAdd = true;
					ItemGuid = Guid.NewGuid();
					currItem = new carrot_CalendarEvent();
					currItem.CalendarEventID = ItemGuid;
				}

				currItem.EventDetail = reContent.Text;
				currItem.IsCancelled = chkIsCancelled.Checked;

				currItem.EventStartTime = CalendarHelper.GetTimeSpanFromTextbox(txtEventStartTime);
				currItem.EventEndTime = CalendarHelper.GetTimeSpanFromTextbox(txtEventEndTime);

				if (bAdd) {
					db.carrot_CalendarEvents.InsertOnSubmit(currItem);
				}

				db.SubmitChanges();
			}

			Response.Redirect(CreateLink(ModuleName, String.Format("id={0}", ItemGuid)));
		}
	}
}