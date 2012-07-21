using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
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
				return ((s == null) ? String.Empty : s);
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
				try { c = Convert.ToDateTime(ViewState["CalendarDate"].ToString()); } catch { ViewState["CalendarDate"] = DateTime.Today.ToString(); }
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
			int d = 1;
			d -= iFirstDay;



			List<DateTime> dates = new List<DateTime>();

			if (HilightDateList != null) {
				if (HilightDateList.Count > 0) {
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

			while ((d <= iDaysInMonth) && (d <= 31) && (d >= -7)) {
				sb.Append("<tr>");

				for (int cal = 0; cal <= iDayOfWeek; cal++) {
					string strCaption = "&nbsp;";
					string sClass = "normal";
					DateTime cellDate = DateTime.MinValue;
					if ((d >= 1) && (d <= iDaysInMonth)) {
						cellDate = new DateTime(YearNumber, MonthNumber, d);
						if (!string.IsNullOrEmpty(JavascriptForDate)) {
							strCaption = "&nbsp;<a href=\"javascript:" + JavascriptForDate + "('" + cellDate.ToShortDateString() + "')\">" + d.ToString() + "&nbsp;";
						} else {
							strCaption = "&nbsp;" + d.ToString() + "&nbsp;";
						}
					}

					if (strCaption != "&nbsp;") {
						cellDate = new DateTime(YearNumber, MonthNumber, d);
						if (cellDate == Today) {
							sClass = "today";
						}

						var copyRows = (from c in dates
										where c == cellDate.Date
										select c);
						if (copyRows.Count() > 0) {
							sClass = sClass + "sel";
						}
					}
					d++;
					string cell = "<td id=cellDay" + d.ToString() + " class=\"" + sClass + "\">" + strCaption + "</td>\r\n";
					sb.Append(cell);

					if (cal == iDayOfWeek) {
						sb.Append("</tr>\r\n <tr>");

					}
				}

				sb.Append("</tr>");
			}


			output.Write("\r\n<table class=\"calendarGrid\" cellspacing=\"0\" cellpadding=\"3\" align=\"center\" border=\"1\">");
			output.Write("\r\n	<tr>");
			output.Write("\r\n		<td class=\"head\" colspan=\"7\">");
			output.Write("\r\n			<table class=\"innerhead\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">");
			output.Write("\r\n				<tr> <td class=\"head normaltext\"> &nbsp; </td> </tr>");
			output.Write("\r\n				<tr> <td class=\"head headtext\"> " + MonthName + " </td> </tr>");
			output.Write("\r\n				<tr> <td class=\"head normaltext\"> &nbsp; </td> </tr>");
			output.Write("\r\n			</table>");
			output.Write("\r\n		</td>");
			output.Write("\r\n	</tr>");
			output.Write("\r\n	<tr class=\"weekday\">");
			output.Write("\r\n		<td class=\"weekday\" width=\"38\"> SU </td>");
			output.Write("\r\n		<td class=\"weekday\" width=\"38\"> M </td>");
			output.Write("\r\n		<td class=\"weekday\" width=\"38\"> TU </td>");
			output.Write("\r\n		<td class=\"weekday\" width=\"38\"> W </td>");
			output.Write("\r\n		<td class=\"weekday\" width=\"38\"> TR </td>");
			output.Write("\r\n		<td class=\"weekday\" width=\"38\"> F </td>");
			output.Write("\r\n		<td class=\"weekday\" width=\"38\"> SA </td>");
			output.Write("\r\n	</tr>");
			output.Write("\r\n	");
			output.Write(sb.ToString());
			output.Write("\r\n	");
			output.Write("\r\n</table> ");

		}

		protected override void OnPreRender(EventArgs e) {

			if (string.IsNullOrEmpty(OverrideCSS)) {
				//string sCSSFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.calendar.css");

				//var link = new HtmlLink();
				//link.Href = sCSSFile;
				//link.Attributes.Add("rel", "stylesheet");
				//link.Attributes.Add("type", "text/css");
				//Page.Header.Controls.Add(link);

				var _assembly = Assembly.GetExecutingAssembly();
				var _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.Web.UI.Controls.calendar.txt"));
				string sCSS = _textStreamReader.ReadToEnd();

				sCSS = sCSS.Replace("{WEEKDAY_CHEX}", ColorTranslator.ToHtml(WeekdayColor));
				sCSS = sCSS.Replace("{WEEKDAY_BGHEX}", ColorTranslator.ToHtml(WeekdayBackground));
				sCSS = sCSS.Replace("{CELL_CHEX}", ColorTranslator.ToHtml(CellColor));
				sCSS = sCSS.Replace("{CELL_BGHEX}", ColorTranslator.ToHtml(CellBackground));

				sCSS = sCSS.Replace("{TODAY_CHEX}", ColorTranslator.ToHtml(TodayColor));
				sCSS = sCSS.Replace("{TODAY_BGHEX}", ColorTranslator.ToHtml(TodayBackground));
				sCSS = sCSS.Replace("{TODAYSEL_BDR}", ColorTranslator.ToHtml(TodaySelectBorder));
				sCSS = sCSS.Replace("{TODAY_LNK}", ColorTranslator.ToHtml(TodayLink));

				sCSS = sCSS.Replace("{NORMAL_CHEX}", ColorTranslator.ToHtml(NormalColor));
				sCSS = sCSS.Replace("{NORMAL_BGHEX}", ColorTranslator.ToHtml(NormalBackground));
				sCSS = sCSS.Replace("{NORMALSEL_BDR}", ColorTranslator.ToHtml(NormalSelectBorder));
				sCSS = sCSS.Replace("{NORMAL_LNK}", ColorTranslator.ToHtml(NormalLink));

				sCSS = sCSS.Replace("{CALENDAR_ID}", "#" + this.ClientID);
				sCSS = "\r\n<style type=\"text/css\">\r\n" + sCSS + "\r\n</style>\r\n";

				var link = new Literal();
				link.Text = sCSS;
				Page.Header.Controls.Add(link);

			} else {
				var link = new HtmlLink();
				link.Href = OverrideCSS;
				link.Attributes.Add("rel", "stylesheet");
				link.Attributes.Add("type", "text/css");
				Page.Header.Controls.Add(link);
			}

			base.OnPreRender(e);
		}

	}
}
