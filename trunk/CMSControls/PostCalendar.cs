using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/



namespace Carrotware.CMS.UI.Controls {

	[DefaultProperty("Text")]
	[ToolboxData("<{0}:PostCalendar runat=server></{0}:PostCalendar>")]
	public class PostCalendar : BaseServerControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CalendarHead {
			get {
				string s = (string)ViewState["CalendarHead"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CalendarHead"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSClassCaption {
			get {
				string s = (string)ViewState["CSSClassCaption"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassCaption"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSClassTable {
			get {
				string s = (string)ViewState["CSSClassTable"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassTable"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSClassDayHead {
			get {
				string s = (string)ViewState["CSSClassDayHead"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassDayHead"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSClassTableBody {
			get {
				string s = (string)ViewState["CSSClassTableBody"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassTableBody"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSClassDateLink {
			get {
				string s = (string)ViewState["CSSClassDateLink"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassDateLink"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSClassTableFoot {
			get {
				string s = (string)ViewState["CSSClassTableFoot"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassTableFoot"] = value;
			}
		}


		DateTime _date = DateTime.MinValue;
		private DateTime ThisMonth {
			get {
				if (_date.Year < 1900) {
					_date = DateTime.Now.Date;
					string sFilterPath = SiteData.CurrentScriptName;
					if (sFilterPath.ToLower().StartsWith(SiteData.CurrentSite.BlogDateFolderPath.ToLower())) {
						BlogDatePathParser p = new BlogDatePathParser(SiteData.CurrentSite, sFilterPath);
						if (p.dateBegin.Year > 1900) {
							_date = p.dateBegin;
						}
					}
				}
				return _date;
			}

		}

		protected List<ContentDateLinks> GetMetaInfo() {
			return navHelper.GetSingleMonthBlogUpdateList(SiteData.CurrentSite, ThisMonth, !SecurityData.IsAuthEditor);
		}

		protected override void RenderContents(HtmlTextWriter output) {
			int indent = output.Indent;

			List<ContentDateLinks> lstCalendar = GetMetaInfo();

			output.Indent = indent + 3;
			output.WriteLine();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = " class=\"" + CssClass + "\" ";
			}

			string sCSSClassTable = "";
			if (!string.IsNullOrEmpty(CSSClassTable)) {
				sCSSClassTable = " class=\"" + CSSClassTable + "\" ";
			}
			string sCSSClassCaption = "";
			if (!string.IsNullOrEmpty(CSSClassCaption)) {
				sCSSClassCaption = " class=\"" + CSSClassCaption + "\" ";
			}
			string sCSSClassDayHead = "";
			if (!string.IsNullOrEmpty(CSSClassDayHead)) {
				sCSSClassDayHead = " class=\"" + CSSClassDayHead + "\" ";
			}
			string sCSSClassTableBody = "";
			if (!string.IsNullOrEmpty(CSSClassTableBody)) {
				sCSSClassTableBody = " class=\"" + CSSClassTableBody + "\" ";
			}
			string sCSSClassDateLink = "";
			if (!string.IsNullOrEmpty(CSSClassDateLink)) {
				sCSSClassDateLink = " class=\"" + CSSClassDateLink + "\" ";
			}
			string sCSSClassTableFoot = "";
			if (!string.IsNullOrEmpty(CSSClassTableFoot)) {
				sCSSClassTableFoot = " class=\"" + CSSClassTableFoot + "\" ";
			}

			ContentDateTally lastMonth = new ContentDateTally { CreateDate = ThisMonth.AddMonths(-1), TheSite = SiteData.CurrentSite };
			ContentDateTally nextMonth = new ContentDateTally { CreateDate = ThisMonth.AddMonths(1), TheSite = SiteData.CurrentSite };

			output.WriteLine("<div" + sCSS + " id=\"" + this.ClientID + "\"> ");
			output.Indent++;

			if (!string.IsNullOrEmpty(CalendarHead)) {
				output.WriteLine("<h2 class=\"calendar-caption\">" + CalendarHead + "  </h2> ");
			}

			DateTime FirstOfMonth = ThisMonth.AddDays(1 - ThisMonth.Day);
			int iFirstDay = (int)FirstOfMonth.DayOfWeek;
			TimeSpan ts = FirstOfMonth.AddMonths(1) - FirstOfMonth;
			int iDaysInMonth = ts.Days;

			int YearNumber = FirstOfMonth.Date.Year;
			int MonthNumber = FirstOfMonth.Date.Month;

			int iDayOfWeek = 6;
			int DayOfMonth = 1;
			DayOfMonth -= iFirstDay;
			int WeekNumber = 1;


			output.WriteLine("	<table " + sCSSClassTable + "> ");
			output.WriteLine("		<caption id=\"" + this.ClientID + "-caption\"  " + sCSSClassCaption + "> " + ThisMonth.Date.ToString("MMMM yyyy") + " </caption>");

			output.WriteLine("	<thead id=\"" + this.ClientID + "-head\" " + sCSSClassDayHead + ">");
			output.WriteLine("		<tr>");
			output.WriteLine("			<th scope=\"col\">SU</th>");
			output.WriteLine("			<th scope=\"col\">M</th>");
			output.WriteLine("			<th scope=\"col\">TU</th>");
			output.WriteLine("			<th scope=\"col\">W</th>");
			output.WriteLine("			<th scope=\"col\">TR</th>");
			output.WriteLine("			<th scope=\"col\">F</th>");
			output.WriteLine("			<th scope=\"col\">SA</th>");
			output.WriteLine("		</tr>");
			output.WriteLine("	</thead>");

			output.WriteLine("		<tbody id=\"" + this.ClientID + "-body\"  " + sCSSClassTableBody + ">");
			while ((DayOfMonth <= iDaysInMonth) && (DayOfMonth <= 31) && (DayOfMonth >= -7)) {

				for (int DayIndex = 0; DayIndex <= iDayOfWeek; DayIndex++) {
					if (DayIndex == 0) {
						output.WriteLine("			<tr id=\"" + this.ClientID + "-week" + WeekNumber.ToString() + "\"> ");
						WeekNumber++;
					}

					DateTime cellDate = DateTime.MinValue;

					if ((DayOfMonth >= 1) && (DayOfMonth <= iDaysInMonth)) {
						cellDate = new DateTime(YearNumber, MonthNumber, DayOfMonth);

						string sTD = "<td";
						if (cellDate.Date == DateTime.Now.Date) {
							sTD = "<td id=\"today\"";
						}

						ContentDateLinks cal = (from n in lstCalendar
												where n.PostDate.Date == cellDate.Date
												select n).FirstOrDefault();
						if (cal != null) {
							output.WriteLine("			" + sTD + " " + sCSSClassDateLink + ">");
							output.WriteLine("				<a href=\"" + cal.MetaInfoURL + "\"> " + cellDate.Day.ToString() + " </a>");
						} else {
							output.WriteLine("			" + sTD + ">");
							output.WriteLine("				" + cellDate.Day.ToString() + " ");
						}
						output.WriteLine("			</td>");
					} else {
						output.WriteLine("			<td class=\"pad\"> </td>");
					}

					DayOfMonth++;

					if (DayIndex == iDayOfWeek) {
						output.WriteLine("		</tr>");
					}
				}
			}
			output.WriteLine("		</tbody>");

			// as a bot crawler abuse stopper


			output.WriteLine("		<tfoot id=\"" + this.ClientID + "-foot\" " + sCSSClassTableFoot + ">");
			output.WriteLine("		<tr>");
			output.WriteLine("			<td colspan=\"3\" id=\"prev\" class=\"cal-prev\">");
			if (lastMonth.CreateDate >= DateTime.Now.AddYears(-5)) {
				output.WriteLine("				<a href=\"" + lastMonth.MetaInfoURL + "\">&laquo; " + lastMonth.CreateDate.ToString("MMM") + "</a>");
			}
			output.WriteLine("			</td>");
			output.WriteLine("			<td class=\"pad\"> &nbsp; </td>");
			output.WriteLine("			<td colspan=\"3\" id=\"next\" class=\"cal-prev\">");
			if (nextMonth.CreateDate <= DateTime.Now.AddYears(5)) {
				output.WriteLine("				<a href=\"" + nextMonth.MetaInfoURL + "\">" + nextMonth.CreateDate.ToString("MMM") + " &raquo;</a>");
			}
			output.WriteLine("			</td>");
			output.WriteLine("		</tr>");
			output.WriteLine("		</tfoot>");



			output.WriteLine("	</table>");


			output.Indent--;
			output.WriteLine("</div> ");
			output.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {

			try {

				if (PublicParmValues.Count > 0) {

					CssClass = GetParmValue("CssClass", "");

					CalendarHead = GetParmValue("CalendarHead", "");

				}
			} catch (Exception ex) {
			}


			base.OnPreRender(e);
		}


	}
}
