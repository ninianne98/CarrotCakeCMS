using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
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
	public class CalendarHelper {

		public enum PluginKeys {
			EventAdminDatabase,
			AdminProfileList,
			EventAdminList,
			EventAdminDetail,
			EventAdminDetailSingle,
			EventAdminCategoryList,
			EventAdminCategoryDetail,
		}

		public static string HEX_White = "#FFFFFF";
		public static string HEX_Black = "#000000";

		public static List<carrot_CalendarEventCategory> GetCalendarCategories(Guid siteID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				if (db.carrot_CalendarEventCategories.Count() < 1) {

					carrot_CalendarEventCategory itm = new carrot_CalendarEventCategory();

					itm = new carrot_CalendarEventCategory();
					itm.CalendarEventCategoryID = Guid.NewGuid();
					itm.SiteID = siteID;

					itm.CategoryName = "Default";
					itm.CategoryFGColor = HEX_Black;
					itm.CategoryBGColor = HEX_White;

					db.carrot_CalendarEventCategories.InsertOnSubmit(itm);

					db.SubmitChanges();
				}

				return (from c in db.carrot_CalendarEventCategories
						orderby c.CategoryName
						where c.SiteID == siteID
						select c).ToList();
			}
		}

		public static carrot_CalendarEventCategory GetCalendarCategory(Guid calendarEventCategoryID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				return (from c in db.carrot_CalendarEventCategories
						where c.CalendarEventCategoryID == calendarEventCategoryID
						select c).FirstOrDefault();
			}
		}


		public static List<carrot_CalendarEventProfile> GetProfileList(Guid siteID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				return (from c in db.carrot_CalendarEventProfiles
						orderby c.EventStartDate
						where c.SiteID == siteID
						select c).ToList();
			}
		}

		public static List<vw_carrot_CalendarEventProfile> GetProfileView(Guid siteID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				return (from c in db.vw_carrot_CalendarEventProfiles
						orderby c.EventStartDate
						where c.SiteID == siteID
						select c).ToList();
			}
		}

		public static carrot_CalendarEventProfile GetProfile(Guid calendarEventProfileID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				return (from c in db.carrot_CalendarEventProfiles
						where c.CalendarEventProfileID == calendarEventProfileID
						select c).FirstOrDefault();
			}
		}

		public static void RemoveEvent(Guid calendarEventProfileID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				var profile = (from c in db.carrot_CalendarEventProfiles
							   where c.CalendarEventProfileID == calendarEventProfileID
							   select c).FirstOrDefault();

				if (profile != null) {
					var lst = (from m in db.carrot_CalendarEvents
							   where m.CalendarEventProfileID == profile.CalendarEventProfileID
							   select m);

					db.carrot_CalendarEvents.DeleteAllOnSubmit(lst);
					db.carrot_CalendarEventProfiles.DeleteOnSubmit(profile);
					db.SubmitChanges();
				}
			}
		}

		public static carrot_CalendarEventProfile CopyEvent(Guid calendarEventProfileID) {

			var srcProfile = GetProfile(calendarEventProfileID);

			using (CalendarDataContext db = new CalendarDataContext()) {

				var item = new carrot_CalendarEventProfile();
				item.CalendarEventProfileID = Guid.NewGuid();
				item.SiteID = srcProfile.SiteID;
				item.EventDetail = srcProfile.EventDetail;

				item.EventRepeatPattern = srcProfile.EventRepeatPattern;
				item.CalendarFrequencyID = srcProfile.CalendarFrequencyID;
				item.CalendarEventCategoryID = srcProfile.CalendarEventCategoryID;

				item.EventStartDate = srcProfile.EventStartDate;
				item.EventEndDate = srcProfile.EventEndDate;
				item.EventStartTime = srcProfile.EventStartTime;
				item.EventEndTime = srcProfile.EventEndTime;

				item.IsPublic = srcProfile.IsPublic;
				item.IsAllDayEvent = srcProfile.IsAllDayEvent;
				item.IsCancelled = srcProfile.IsCancelled;
				item.IsCancelledPublic = srcProfile.IsCancelledPublic;

				db.carrot_CalendarEventProfiles.InsertOnSubmit(item);

				if (srcProfile != null) {
					var lst = (from m in db.carrot_CalendarEvents
							   where m.CalendarEventProfileID == calendarEventProfileID
							   select m);

					foreach (carrot_CalendarEvent date in lst) {
						carrot_CalendarEvent evt = new carrot_CalendarEvent {
							CalendarEventID = Guid.NewGuid(),
							CalendarEventProfileID = item.CalendarEventProfileID,
							EventDate = date.EventDate,
							EventDetail = date.EventDetail,
							IsCancelled = date.IsCancelled
						};

						db.carrot_CalendarEvents.InsertOnSubmit(evt);
					}

					db.SubmitChanges();
				}

				return item;
			}
		}


		public static List<carrot_CalendarEvent> GetEventList(Guid calendarEventProfileID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				return (from c in db.carrot_CalendarEvents
						orderby c.EventDate
						where c.CalendarEventProfileID == calendarEventProfileID
						select c).ToList();
			}
		}

		public static List<vw_carrot_CalendarEvent> GetEventView(Guid calendarEventProfileID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				return (from c in db.vw_carrot_CalendarEvents
						orderby c.EventDate
						where c.CalendarEventProfileID == calendarEventProfileID
						select c).ToList();
			}
		}

		public static carrot_CalendarEvent GetEvent(Guid calendarEventID) {

			using (CalendarDataContext db = new CalendarDataContext()) {

				return (from c in db.carrot_CalendarEvents
						where c.CalendarEventID == calendarEventID
						select c).FirstOrDefault();
			}
		}


		public static Dictionary<int, string> DaysOfTheWeek {
			get {

				Dictionary<int, string> daysOfWeek = new Dictionary<int, string>();

				daysOfWeek.Add(64, DayOfWeek.Sunday.ToString());
				daysOfWeek.Add(1, DayOfWeek.Monday.ToString());
				daysOfWeek.Add(2, DayOfWeek.Tuesday.ToString());
				daysOfWeek.Add(4, DayOfWeek.Wednesday.ToString());
				daysOfWeek.Add(8, DayOfWeek.Thursday.ToString());
				daysOfWeek.Add(16, DayOfWeek.Friday.ToString());
				daysOfWeek.Add(32, DayOfWeek.Saturday.ToString());

				return daysOfWeek;
			}
		}

		public static Dictionary<string, string> ColorCodes {
			get {
				var _dict = new Dictionary<string, string>();
				_dict.Add("#F0F8FF", "AliceBlue");
				_dict.Add("#FAEBD7", "AntiqueWhite");
				_dict.Add("#7FFFD4", "Aquamarine");
				_dict.Add("#F0FFFF", "Azure");
				_dict.Add("#F5F5DC", "Beige");
				_dict.Add("#FFE4C4", "Bisque");
				_dict.Add("#000000", "Black");
				_dict.Add("#FFEBCD", "BlanchedAlmond");
				_dict.Add("#0000FF", "Blue");
				_dict.Add("#8A2BE2", "BlueViolet");
				_dict.Add("#A52A2A", "Brown");
				_dict.Add("#DEB887", "BurlyWood");
				_dict.Add("#5F9EA0", "CadetBlue");
				_dict.Add("#7FFF00", "Chartreuse");
				_dict.Add("#D2691E", "Chocolate");
				_dict.Add("#FF7F50", "Coral");
				_dict.Add("#6495ED", "CornflowerBlue");
				_dict.Add("#FFF8DC", "Cornsilk");
				_dict.Add("#DC143C", "Crimson");
				_dict.Add("#00FFFF", "Cyan");
				_dict.Add("#00008B", "DarkBlue");
				_dict.Add("#008B8B", "DarkCyan");
				_dict.Add("#B8860B", "DarkGoldenRod");
				_dict.Add("#A9A9A9", "DarkGray");
				_dict.Add("#006400", "DarkGreen");
				_dict.Add("#BDB76B", "DarkKhaki");
				_dict.Add("#8B008B", "DarkMagenta");
				_dict.Add("#556B2F", "DarkOliveGreen");
				_dict.Add("#FF8C00", "Darkorange");
				_dict.Add("#9932CC", "DarkOrchid");
				_dict.Add("#8B0000", "DarkRed");
				_dict.Add("#E9967A", "DarkSalmon");
				_dict.Add("#8FBC8F", "DarkSeaGreen");
				_dict.Add("#483D8B", "DarkSlateBlue");
				_dict.Add("#2F4F4F", "DarkSlateGray");
				_dict.Add("#00CED1", "DarkTurquoise");
				_dict.Add("#9400D3", "DarkViolet");
				_dict.Add("#FF1493", "DeepPink");
				_dict.Add("#00BFFF", "DeepSkyBlue");
				_dict.Add("#696969", "DimGray");
				_dict.Add("#1E90FF", "DodgerBlue");
				_dict.Add("#B22222", "FireBrick");
				_dict.Add("#FFFAF0", "FloralWhite");
				_dict.Add("#228B22", "ForestGreen");
				_dict.Add("#DCDCDC", "Gainsboro");
				_dict.Add("#F8F8FF", "GhostWhite");
				_dict.Add("#FFD700", "Gold");
				_dict.Add("#DAA520", "GoldenRod");
				_dict.Add("#808080", "Gray");
				_dict.Add("#008000", "Green");
				_dict.Add("#ADFF2F", "GreenYellow");
				_dict.Add("#F0FFF0", "HoneyDew");
				_dict.Add("#FF69B4", "HotPink");
				_dict.Add("#CD5C5C", "IndianRed");
				_dict.Add("#4B0082", "Indigo");
				_dict.Add("#FFFFF0", "Ivory");
				_dict.Add("#F0E68C", "Khaki");
				_dict.Add("#E6E6FA", "Lavender");
				_dict.Add("#FFF0F5", "LavenderBlush");
				_dict.Add("#7CFC00", "LawnGreen");
				_dict.Add("#FFFACD", "LemonChiffon");
				_dict.Add("#ADD8E6", "LightBlue");
				_dict.Add("#F08080", "LightCoral");
				_dict.Add("#E0FFFF", "LightCyan");
				_dict.Add("#FAFAD2", "LightGoldenRodYellow");
				_dict.Add("#D3D3D3", "LightGray");
				_dict.Add("#90EE90", "LightGreen");
				_dict.Add("#FFB6C1", "LightPink");
				_dict.Add("#FFA07A", "LightSalmon");
				_dict.Add("#20B2AA", "LightSeaGreen");
				_dict.Add("#87CEFA", "LightSkyBlue");
				_dict.Add("#778899", "LightSlateGray");
				_dict.Add("#B0C4DE", "LightSteelBlue");
				_dict.Add("#FFFFE0", "LightYellow");
				_dict.Add("#00FF00", "Lime");
				_dict.Add("#32CD32", "LimeGreen");
				_dict.Add("#FAF0E6", "Linen");
				_dict.Add("#FF00FF", "Magenta");
				_dict.Add("#800000", "Maroon");
				_dict.Add("#66CDAA", "MediumAquaMarine");
				_dict.Add("#0000CD", "MediumBlue");
				_dict.Add("#BA55D3", "MediumOrchid");
				_dict.Add("#9370DB", "MediumPurple");
				_dict.Add("#3CB371", "MediumSeaGreen");
				_dict.Add("#7B68EE", "MediumSlateBlue");
				_dict.Add("#00FA9A", "MediumSpringGreen");
				_dict.Add("#48D1CC", "MediumTurquoise");
				_dict.Add("#C71585", "MediumVioletRed");
				_dict.Add("#191970", "MidnightBlue");
				_dict.Add("#F5FFFA", "MintCream");
				_dict.Add("#FFE4E1", "MistyRose");
				_dict.Add("#FFE4B5", "Moccasin");
				_dict.Add("#FFDEAD", "NavajoWhite");
				_dict.Add("#000080", "Navy");
				_dict.Add("#FDF5E6", "OldLace");
				_dict.Add("#808000", "Olive");
				_dict.Add("#6B8E23", "OliveDrab");
				_dict.Add("#FFA500", "Orange");
				_dict.Add("#FF4500", "OrangeRed");
				_dict.Add("#DA70D6", "Orchid");
				_dict.Add("#EEE8AA", "PaleGoldenRod");
				_dict.Add("#98FB98", "PaleGreen");
				_dict.Add("#AFEEEE", "PaleTurquoise");
				_dict.Add("#DB7093", "PaleVioletRed");
				_dict.Add("#FFEFD5", "PapayaWhip");
				_dict.Add("#FFDAB9", "PeachPuff");
				_dict.Add("#CD853F", "Peru");
				_dict.Add("#FFC0CB", "Pink");
				_dict.Add("#DDA0DD", "Plum");
				_dict.Add("#B0E0E6", "PowderBlue");
				_dict.Add("#800080", "Purple");
				_dict.Add("#FF0000", "Red");
				_dict.Add("#BC8F8F", "RosyBrown");
				_dict.Add("#4169E1", "RoyalBlue");
				_dict.Add("#8B4513", "SaddleBrown");
				_dict.Add("#FA8072", "Salmon");
				_dict.Add("#F4A460", "SandyBrown");
				_dict.Add("#2E8B57", "SeaGreen");
				_dict.Add("#FFF5EE", "SeaShell");
				_dict.Add("#A0522D", "Sienna");
				_dict.Add("#C0C0C0", "Silver");
				_dict.Add("#87CEEB", "SkyBlue");
				_dict.Add("#6A5ACD", "SlateBlue");
				_dict.Add("#708090", "SlateGray");
				_dict.Add("#FFFAFA", "Snow");
				_dict.Add("#00FF7F", "SpringGreen");
				_dict.Add("#4682B4", "SteelBlue");
				_dict.Add("#D2B48C", "Tan");
				_dict.Add("#008080", "Teal");
				_dict.Add("#D8BFD8", "Thistle");
				_dict.Add("#FF6347", "Tomato");
				_dict.Add("#40E0D0", "Turquoise");
				_dict.Add("#EE82EE", "Violet");
				_dict.Add("#F5DEB3", "Wheat");
				_dict.Add("#FFFFFF", "White");
				_dict.Add("#F5F5F5", "WhiteSmoke");
				_dict.Add("#FFFF00", "Yellow");
				_dict.Add("#9ACD32", "YellowGreen");
				return _dict;
			}
		}

		public static void BindDataBoundControl(DataBoundControl ctrl, object dataSource) {
			ctrl.DataSource = dataSource;
			ctrl.DataBind();
		}

		public static void BindRepeater(Repeater ctrl, object dataSource) {
			ctrl.DataSource = dataSource;
			ctrl.DataBind();
		}

		public static void BindDropDownList(DropDownList ctrl, object dataSource) {
			BindDropDownList(ctrl, dataSource, null);
		}

		public static void BindDropDownList(DropDownList ctrl, object dataSource, string SelectedValue) {
			ctrl.DataSource = dataSource;
			ctrl.DataBind();

			if (SelectedValue != null) {
				try { ctrl.SelectedValue = SelectedValue; } catch { }
			}
		}

		public static List<string> GetCheckedItemString(GridView grid, bool CollectState, string CheckBoxName, string HiddenName) {
			List<string> lstUpd = new List<string>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(CheckBoxName);
				if (chk != null && chk.Checked == CollectState) {
					HiddenField hdn = (HiddenField)row.FindControl(HiddenName);
					lstUpd.Add(hdn.Value);
				}
			}
			return lstUpd;
		}

		public static List<string> GetCheckedItemString(Repeater repeat, bool CollectState, string CheckBoxName, string HiddenName) {
			List<string> lstUpd = new List<string>();

			foreach (RepeaterItem row in repeat.Items) {
				CheckBox chk = (CheckBox)row.FindControl(CheckBoxName);
				if (chk != null && chk.Checked == CollectState) {
					HiddenField hdn = (HiddenField)row.FindControl(HiddenName);
					lstUpd.Add(hdn.Value);
				}
			}
			return lstUpd;
		}

		public static List<string> GetCheckedItemStringByValue(GridView grid, bool CollectState, string CheckBoxName) {
			List<string> lstUpd = new List<string>();

			foreach (GridViewRow row in grid.Rows) {
				CheckBox chk = (CheckBox)row.FindControl(CheckBoxName);
				if (chk != null && chk.Checked == CollectState) {
					lstUpd.Add(chk.Attributes["value"].ToString());
				}
			}
			return lstUpd;
		}

		public static List<string> GetCheckedItemStringByValue(Repeater repeat, bool CollectState, string CheckBoxName) {
			List<string> lstUpd = new List<string>();

			foreach (RepeaterItem row in repeat.Items) {
				CheckBox chk = (CheckBox)row.FindControl(CheckBoxName);
				if (chk != null && chk.Checked == CollectState) {
					lstUpd.Add(chk.Attributes["value"].ToString());
				}
			}
			return lstUpd;
		}


		private static DateTime fakeDate {
			get {
				return Convert.ToDateTime("1899-12-31").Date;
			}
		}

		public static void SetTextboxToTimeSpan(TextBox txt, TimeSpan? timeSpan) {

			if (timeSpan.HasValue) {
				txt.Text = fakeDate.Add(timeSpan.Value).ToShortTimeString();
			} else {
				txt.Text = "";
			}
		}

		public static string GetTextFromTimeSpan(TimeSpan? timeSpan) {

			if (timeSpan.HasValue) {
				return GetFullDateTime(fakeDate, timeSpan).ToShortTimeString();
			} else {
				return null;
			}
		}

		public static DateTime GetFullDateTime(TimeSpan? timeSpan) {
			return GetFullDateTime(fakeDate, timeSpan);
		}

		public static DateTime GetFullDateTime(DateTime theDate, TimeSpan? timeSpan) {

			if (timeSpan.HasValue) {
				return theDate.Date.Add(timeSpan.Value);
			} else {
				return theDate;
			}
		}

		public static TimeSpan? GetTimeSpanFromTextbox(TextBox txt) {
			TimeSpan? ts = null;

			if (!string.IsNullOrEmpty(txt.Text)) {
				ts = DateTime.Parse(txt.Text).TimeOfDay;
			}

			return ts;
		}


	}
}