﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:Calendar runat=server></{0}:Calendar>")]
	public class Calendar : WebControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color CellColor {
			get {
				string s = (string)ViewState["CellColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#ffffff") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["CellColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color CellBackground {
			get {
				string s = (string)ViewState["CellBackground"];
				return ((s == null) ? ColorTranslator.FromHtml("#00509F") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["CellBackground"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color WeekdayColor {
			get {
				string s = (string)ViewState["WeekdayColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#000000") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["WeekdayColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color WeekdayBackground {
			get {
				string s = (string)ViewState["WeekdayBackground"];
				return ((s == null) ? ColorTranslator.FromHtml("#00C87F") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["WeekdayBackground"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color TodayColor {
			get {
				string s = (string)ViewState["TodayColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#FFFF80") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["TodayColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color TodayBackground {
			get {
				string s = (string)ViewState["TodayBackground"];
				return ((s == null) ? ColorTranslator.FromHtml("#800080") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["TodayBackground"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color TodaySelectBorder {
			get {
				string s = (string)ViewState["TodaySelectBorder"];
				return ((s == null) ? ColorTranslator.FromHtml("#FFFF80") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["TodaySelectBorder"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color NormalColor {
			get {
				string s = (string)ViewState["NormalColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#004040") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["NormalColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color NormalBackground {
			get {
				string s = (string)ViewState["NormalBackground"];
				return ((s == null) ? ColorTranslator.FromHtml("#D8D8EB") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["NormalBackground"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color NormalSelectBorder {
			get {
				string s = (string)ViewState["NormalSelectBorder"];
				return ((s == null) ? ColorTranslator.FromHtml("#FF0080") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["NormalSelectBorder"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color TodayLink {
			get {
				string s = (string)ViewState["TodayLink"];
				return ((s == null) ? ColorTranslator.FromHtml("#FFFF80") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["TodayLink"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Color NormalLink {
			get {
				string s = (string)ViewState["NormalLink"];
				return ((s == null) ? ColorTranslator.FromHtml("#004040") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["NormalLink"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string JavascriptForDate {
			get {
				String s = (String)ViewState["JavascriptForDate"];
				return ((s == null) ? string.Empty : s);
			}

			set {
				ViewState["JavascriptForDate"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int MonthNumber {
			get {
				int s = DateTime.Now.Date.Month;
				try { s = (int)ViewState["MonthNumber"]; } catch { ViewState["MonthNumber"] = DateTime.Now.Date.Month; }
				return s;
			}
			set {
				ViewState["MonthNumber"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string OverrideCSS {
			get {
				string s = "";
				try { s = Convert.ToString(ViewState["OverrideCSS"]); } catch { ViewState["OverrideCSS"] = ""; }
				return s;
			}
			set {
				ViewState["OverrideCSS"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public int YearNumber {
			get {
				int s = DateTime.Now.Date.Year;
				try { s = (int)ViewState["YearNumber"]; } catch { ViewState["YearNumber"] = DateTime.Now.Date.Year; }
				return s;
			}
			set {
				ViewState["YearNumber"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public DateTime CalendarDate {
			get {
				DateTime c = DateTime.Today;
				if (ViewState["CalendarDate"] != null) {
					try { c = Convert.ToDateTime(ViewState["CalendarDate"].ToString()); } catch { }
				} else {
					ViewState["CalendarDate"] = DateTime.Today.ToString();
				}
				YearNumber = c.Year;
				MonthNumber = c.Month;
				return c;
			}
			set {
				YearNumber = value.Year;
				MonthNumber = value.Month;
				ViewState["CalendarDate"] = value;
			}
		}

		public string HilightDates { get; set; }

		public List<DateTime> HilightDateList { get; set; }

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			string CtrlID = this.ClientID;

			output.Indent = indent + 3;
			output.WriteLine();

			DateTime Today = DateTime.Today.Date;
			DateTime ThisMonth = DateTime.Today.Date;

			try {
				ThisMonth = new DateTime(YearNumber, MonthNumber, 15);
			} catch { }

			DateTime FirstOfMonth = ThisMonth.AddDays(1 - ThisMonth.Day);

			DateTime nextMonth = ThisMonth.AddMonths(1);
			DateTime prevMonth = ThisMonth.AddMonths(-1);

			int iFirstDay = (int)FirstOfMonth.DayOfWeek;
			TimeSpan ts = FirstOfMonth.AddMonths(1) - FirstOfMonth;
			int iDaysInMonth = ts.Days;

			string MonthName = ThisMonth.ToString("MMMM") + "  " + ThisMonth.Year.ToString();

			int iDayOfWeek = 6;
			int DayOfMonth = 1;
			DayOfMonth -= iFirstDay;

			List<DateTime> dates = new List<DateTime>();

			if (HilightDateList != null) {
				if (HilightDateList.Any()) {
					dates = HilightDateList;
				}
			} else {
				List<string> lstDates = new List<string>();

				if (!string.IsNullOrEmpty(HilightDates)) {
					lstDates = HilightDates.Split(';').AsEnumerable().ToList();
				}
				dates = (from dd in lstDates
						 select Convert.ToDateTime(dd)).ToList();
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("");

			int WeekNumber = 1;

			while ((DayOfMonth <= iDaysInMonth) && (DayOfMonth <= 31) && (DayOfMonth >= -7)) {
				for (int DayIndex = 0; DayIndex <= iDayOfWeek; DayIndex++) {
					if (DayIndex == 0) {
						sb.Append("<tr class=\"weekdayrow\" id=\"" + CtrlID + "-weekRow" + WeekNumber.ToString() + "\">\n");
						WeekNumber++;
					}

					string strCaption = "&nbsp;";
					string sClass = "normal";
					DateTime cellDate = DateTime.MinValue;

					if ((DayOfMonth >= 1) && (DayOfMonth <= iDaysInMonth)) {
						cellDate = new DateTime(YearNumber, MonthNumber, DayOfMonth);
						if (!string.IsNullOrEmpty(JavascriptForDate)) {
							strCaption = "&nbsp;<a href=\"javascript:" + JavascriptForDate + "('" + cellDate.ToString("yyyy-MM-dd") + "')\">" + DayOfMonth.ToString() + "&nbsp;";
						} else {
							strCaption = "&nbsp;" + DayOfMonth.ToString() + "&nbsp;";
						}
					}

					if (strCaption != "&nbsp;") {
						cellDate = new DateTime(YearNumber, MonthNumber, DayOfMonth);
						if (cellDate == Today) {
							sClass = "today";
						}

						IEnumerable<DateTime> copyRows = (from c in dates
														  where c == cellDate.Date
														  select c);
						if (copyRows.Count() > 0) {
							sClass = sClass + "sel";
						}
					}

					DayOfMonth++;

					string cell = "\t<td id=\"" + CtrlID + "-cellDay" + DayOfMonth.ToString() + "\" class=\"" + sClass + "\">" + strCaption + "</td>\n";
					sb.Append(cell);

					if (DayIndex == iDayOfWeek) {
						sb.Append("</tr>\n");
					}
				}
			}

			output.WriteLine("<table  id=\"" + CtrlID + "-CalTable\" class=\"calendarGrid\" cellspacing=\"0\" cellpadding=\"3\" align=\"center\" border=\"1\">");
			output.WriteLine("	<tr class=\"calendarheadrow\">");
			output.WriteLine("		<td class=\"head\" colspan=\"7\">");
			output.WriteLine("			<table class=\"innerhead\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">");
			output.WriteLine("				<tr> <td class=\"head normaltext\"> &nbsp; </td> </tr>");
			output.WriteLine("				<tr> <td class=\"head headtext\"> " + MonthName + " </td> </tr>");
			output.WriteLine("				<tr> <td class=\"head normaltext\"> &nbsp; </td> </tr>");
			output.WriteLine("			</table>");
			output.WriteLine("		</td>");
			output.WriteLine("	</tr>");
			output.WriteLine();
			output.WriteLine("	<tr class=\"weekday\">");
			output.WriteLine("		<td class=\"weekday\" width=\"38\"> SU </td>");
			output.WriteLine("		<td class=\"weekday\" width=\"38\"> M </td>");
			output.WriteLine("		<td class=\"weekday\" width=\"38\"> TU </td>");
			output.WriteLine("		<td class=\"weekday\" width=\"38\"> W </td>");
			output.WriteLine("		<td class=\"weekday\" width=\"38\"> TR </td>");
			output.WriteLine("		<td class=\"weekday\" width=\"38\"> F </td>");
			output.WriteLine("		<td class=\"weekday\" width=\"38\"> SA </td>");
			output.WriteLine("	</tr>");
			output.WriteLine();

			output.Indent = indent + 4;

			string[] rows = sb.ToString().Split('\n');
			int iRows = rows.Count();

			for (int i = 0; i < iRows; i++) {
				output.WriteLine(rows[i]);
			}

			output.Indent = indent + 3;

			output.WriteLine("</table>");

			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {
			if (string.IsNullOrEmpty(OverrideCSS)) {
				string sCSS = string.Empty;
				var sbCSS = new StringBuilder();

				Assembly _assembly = Assembly.GetExecutingAssembly();
				using (var stream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.Web.UI.Controls.calendar.txt"))) {
					sCSS = stream.ReadToEnd();
				}

				sbCSS.Append(sCSS);

				sbCSS.Replace("{WEEKDAY_CHEX}", ColorTranslator.ToHtml(WeekdayColor));
				sbCSS.Replace("{WEEKDAY_BGHEX}", ColorTranslator.ToHtml(WeekdayBackground));
				sbCSS.Replace("{CELL_CHEX}", ColorTranslator.ToHtml(CellColor));
				sbCSS.Replace("{CELL_BGHEX}", ColorTranslator.ToHtml(CellBackground));

				sbCSS.Replace("{TODAY_CHEX}", ColorTranslator.ToHtml(TodayColor));
				sbCSS.Replace("{TODAY_BGHEX}", ColorTranslator.ToHtml(TodayBackground));
				sbCSS.Replace("{TODAYSEL_BDR}", ColorTranslator.ToHtml(TodaySelectBorder));
				sbCSS.Replace("{TODAY_LNK}", ColorTranslator.ToHtml(TodayLink));

				sbCSS.Replace("{NORMAL_CHEX}", ColorTranslator.ToHtml(NormalColor));
				sbCSS.Replace("{NORMAL_BGHEX}", ColorTranslator.ToHtml(NormalBackground));
				sbCSS.Replace("{NORMALSEL_BDR}", ColorTranslator.ToHtml(NormalSelectBorder));
				sbCSS.Replace("{NORMAL_LNK}", ColorTranslator.ToHtml(NormalLink));

				sbCSS.Replace("{CALENDAR_ID}", "#" + this.ClientID);

				sCSS = "\r\n<style type=\"text/css\">\r\n" + sbCSS.ToString() + "\r\n</style>\r\n";

				Literal link = new Literal();
				link.Text = sCSS;
				Page.Header.Controls.Add(link);
			} else {
				HtmlLink link = new HtmlLink();
				link.Href = OverrideCSS;
				link.Attributes.Add("rel", "stylesheet");
				link.Attributes.Add("type", "text/css");
				Page.Header.Controls.Add(link);
			}

			base.OnPreRender(e);
		}
	}
}