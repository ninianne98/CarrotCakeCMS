using Carrotware.CMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.UI.Controls {

	[ToolboxData("<{0}:PostCalendar runat=server></{0}:PostCalendar>")]
	public class PostCalendar : BaseServerControl {

		[Category("Appearance")]
		[DefaultValue("")]
		public string CalendarHead {
			get {
				string s = (string)ViewState["CalendarHead"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CalendarHead"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSClassCaption {
			get {
				string s = (string)ViewState["CSSClassCaption"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassCaption"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSClassTable {
			get {
				string s = (string)ViewState["CSSClassTable"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassTable"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSClassDayHead {
			get {
				string s = (string)ViewState["CSSClassDayHead"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassDayHead"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSClassTableBody {
			get {
				string s = (string)ViewState["CSSClassTableBody"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassTableBody"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSClassDateLink {
			get {
				string s = (string)ViewState["CSSClassDateLink"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassDateLink"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("")]
		public string CSSClassTableFoot {
			get {
				string s = (string)ViewState["CSSClassTableFoot"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["CSSClassTableFoot"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool RenderHTMLWithID {
			get {
				String s = (String)ViewState["RenderHTMLWithID"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["RenderHTMLWithID"] = value.ToString();
			}
		}

		public string HtmlClientID {
			get {
				if (this.RenderHTMLWithID) {
					return this.ID;
				} else {
					return this.ClientID;
				}
			}
		}

		private DateTime _date = DateTime.MinValue;

		private DateTime ThisMonth {
			get {
				if (_date.Year < 1900) {
					if (SiteData.IsWebView) {
						_date = SiteData.CurrentSite.Now.Date;
						string sFilterPath = SiteData.CurrentScriptName;
						if (SiteData.CurrentSite.CheckIsBlogDateFolderPath(sFilterPath)) {
							BlogDatePathParser p = new BlogDatePathParser(SiteData.CurrentSite, sFilterPath);
							if (p.DateBegin.Year > 1900) {
								_date = p.DateBegin;
							}
						}
					} else {
						_date = DateTime.Now;
					}
				}
				return _date;
			}
		}

		protected List<ContentDateLinks> GetMetaInfo() {
			if (SiteData.IsWebView) {
				return _navHelper.GetSingleMonthBlogUpdateList(SiteData.CurrentSite, ThisMonth, !SecurityData.IsAuthEditor);
			} else {
				return new List<ContentDateLinks>();
			}
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			int indent = writer.Indent;

			List<ContentDateLinks> lstCalendar = GetMetaInfo();

			writer.Indent = indent + 3;
			writer.WriteLine();

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

			ContentDateTally thisMonth = new ContentDateTally { GoLiveDate = this.ThisMonth, TheSite = SiteData.CurrentSite };
			ContentDateTally lastMonth = new ContentDateTally { GoLiveDate = this.ThisMonth.AddMonths(-1), TheSite = SiteData.CurrentSite };
			ContentDateTally nextMonth = new ContentDateTally { GoLiveDate = this.ThisMonth.AddMonths(1), TheSite = SiteData.CurrentSite };

			writer.WriteLine("<div" + sCSS + " id=\"" + this.HtmlClientID + "\"> ");
			writer.Indent++;

			if (!string.IsNullOrEmpty(this.CalendarHead)) {
				writer.WriteLine("<h2 class=\"calendar-caption\">" + this.CalendarHead + "  </h2> ");
			}

			DateTime firstOfMonth = new DateTime(this.ThisMonth.Year, this.ThisMonth.Month, 1);
			int firstDay = (int)firstOfMonth.DayOfWeek;
			TimeSpan ts = firstOfMonth.AddMonths(1) - firstOfMonth;
			int daysInMonth = ts.Days;

			int yearNumber = firstOfMonth.Date.Year;
			int monthNumber = firstOfMonth.Date.Month;

			int weekNumber = 1;
			int dayOfWeek = 6;
			int dayOfMonth = 1;
			dayOfMonth -= firstDay;

			writer.WriteLine("	<table " + sCSSClassTable + "> ");
			writer.WriteLine("		<caption id=\"" + this.HtmlClientID + "-caption\"  " + sCSSClassCaption + "> "
							+ "<a href=\"" + thisMonth.MetaInfoURL + "\">" + this.ThisMonth.Date.ToString("MMMM yyyy") + "</a> </caption>");

			writer.WriteLine("	<thead id=\"" + this.HtmlClientID + "-head\" " + sCSSClassDayHead + ">");
			writer.WriteLine("		<tr>");
			writer.WriteLine("			<th scope=\"col\">SU</th>");
			writer.WriteLine("			<th scope=\"col\">M</th>");
			writer.WriteLine("			<th scope=\"col\">TU</th>");
			writer.WriteLine("			<th scope=\"col\">W</th>");
			writer.WriteLine("			<th scope=\"col\">TR</th>");
			writer.WriteLine("			<th scope=\"col\">F</th>");
			writer.WriteLine("			<th scope=\"col\">SA</th>");
			writer.WriteLine("		</tr>");
			writer.WriteLine("	</thead>");

			writer.WriteLine("		<tbody id=\"" + this.HtmlClientID + "-body\"  " + sCSSClassTableBody + ">");
			while ((dayOfMonth <= daysInMonth) && (dayOfMonth <= 31) && (dayOfMonth >= -7)) {
				for (int DayIndex = 0; DayIndex <= dayOfWeek; DayIndex++) {
					if (DayIndex == 0) {
						writer.WriteLine("			<tr id=\"" + this.HtmlClientID + "-week" + weekNumber.ToString() + "\"> ");
						weekNumber++;
					}

					DateTime cellDate = DateTime.MinValue;

					if ((dayOfMonth >= 1) && (dayOfMonth <= daysInMonth)) {
						cellDate = new DateTime(yearNumber, monthNumber, dayOfMonth);

						string sTD = "<td";
						if (cellDate.Date == SiteData.CurrentSite.Now.Date) {
							sTD = "<td id=\"today\"";
						}

						ContentDateLinks cal = (from n in lstCalendar
												where n.PostDate.Date == cellDate.Date
												select n).FirstOrDefault();
						if (cal != null) {
							writer.WriteLine("			" + sTD + " " + sCSSClassDateLink + ">");
							writer.WriteLine("				<a href=\"" + cal.MetaInfoURL + "\"> " + cellDate.Day.ToString() + " </a>");
						} else {
							writer.WriteLine("			" + sTD + ">");
							writer.WriteLine("				" + cellDate.Day.ToString() + " ");
						}
						writer.WriteLine("			</td>");
					} else {
						writer.WriteLine("			<td class=\"pad\"> </td>");
					}

					dayOfMonth++;

					if (DayIndex == dayOfWeek) {
						writer.WriteLine("		</tr>");
					}
				}
			}
			writer.WriteLine("		</tbody>");

			// as a bot crawler abuse stopper

			writer.WriteLine("		<tfoot id=\"" + this.HtmlClientID + "-foot\" " + sCSSClassTableFoot + ">");
			writer.WriteLine("		<tr>");
			writer.WriteLine("			<td colspan=\"3\" id=\"prev\" class=\"cal-prev\">");
			if (lastMonth.GoLiveDate >= SiteData.CurrentSite.Now.AddYears(-5)) {
				writer.WriteLine("				<a href=\"" + lastMonth.MetaInfoURL + "\">&laquo; " + lastMonth.GoLiveDate.ToString("MMM") + "</a>");
			}
			writer.WriteLine("			</td>");
			writer.WriteLine("			<td class=\"pad\"> &nbsp; </td>");
			writer.WriteLine("			<td colspan=\"3\" id=\"next\" class=\"cal-prev\">");
			if (nextMonth.GoLiveDate <= SiteData.CurrentSite.Now.AddYears(5)) {
				writer.WriteLine("				<a href=\"" + nextMonth.MetaInfoURL + "\">" + nextMonth.GoLiveDate.ToString("MMM") + " &raquo;</a>");
			}
			writer.WriteLine("			</td>");
			writer.WriteLine("		</tr>");
			writer.WriteLine("		</tfoot>");

			writer.WriteLine("	</table>");

			writer.Indent--;
			writer.WriteLine("</div> ");
			writer.Indent = indent;
		}

		protected override void OnPreRender(EventArgs e) {
			try {
				if (this.PublicParmValues.Any()) {
					this.CssClass = GetParmValue("CssClass", "");

					this.CalendarHead = GetParmValue("CalendarHead", "");
				}
			} catch (Exception ex) {
			}

			base.OnPreRender(e);
		}
	}
}