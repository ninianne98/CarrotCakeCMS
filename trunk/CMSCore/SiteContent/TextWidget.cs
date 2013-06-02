using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carrotware.CMS.Data;
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


namespace Carrotware.CMS.Core {
	public class TextWidget {

		public TextWidget() { }

		public Guid TextWidgetID { get; set; }
		public Guid SiteID { get; set; }
		public string TextWidgetAssembly { get; set; }
		public bool ProcessBody { get; set; }
		public bool ProcessPlainText { get; set; }
		public bool ProcessHTMLText { get; set; }
		public bool ProcessComment { get; set; }


		private ITextBodyUpdate _txt = null;
		public ITextBodyUpdate TextProcessor {
			get {
				if (_txt == null && !string.IsNullOrEmpty(this.TextWidgetAssembly)) {
					Type t = Type.GetType(this.TextWidgetAssembly);
					Object o = Activator.CreateInstance(t);

					if (o != null && o is ITextBodyUpdate) {
						_txt = o as ITextBodyUpdate;
					}
				}

				return _txt;
			}
		}

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is TextWidget) {
				TextWidget p = (TextWidget)obj;
				return (this.SiteID == p.SiteID
						&& this.TextWidgetAssembly.ToLower() == p.TextWidgetAssembly.ToLower());
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return this.TextWidgetAssembly.GetHashCode() ^ this.SiteID.GetHashCode();
		}

		internal TextWidget(carrot_TextWidget c) {
			if (c != null) {
				this.TextWidgetID = c.TextWidgetID;
				this.SiteID = c.SiteID;
				this.TextWidgetAssembly = c.TextWidgetAssembly;
				this.ProcessBody = c.ProcessBody;
				this.ProcessPlainText = c.ProcessPlainText;
				this.ProcessHTMLText = c.ProcessHTMLText;
				this.ProcessComment = c.ProcessComment;
			}
		}

		public void Save() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				bool bNew = false;
				carrot_TextWidget s = CompiledQueries.cqTextWidgetByID(_db, this.TextWidgetID);

				if (s == null) {
					bNew = true;
					s = new carrot_TextWidget();
					s.TextWidgetID = Guid.NewGuid();
					s.SiteID = this.SiteID;
					s.TextWidgetAssembly = this.TextWidgetAssembly;
				}

				s.ProcessBody = this.ProcessBody;
				s.ProcessPlainText = this.ProcessPlainText;
				s.ProcessHTMLText = this.ProcessHTMLText;
				s.ProcessComment = this.ProcessComment;

				if (bNew) {
					_db.carrot_TextWidgets.InsertOnSubmit(s);
				}

				_db.SubmitChanges();

				this.TextWidgetID = s.TextWidgetID;
			}
		}


		public void Delete() {
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_TextWidget s = CompiledQueries.cqTextWidgetByID(_db, this.TextWidgetID);

				if (s != null) {
					_db.carrot_TextWidgets.DeleteOnSubmit(s);
					_db.SubmitChanges();
				}
			}
		}


		public static TextWidget Get(Guid textWidgetID) {
			TextWidget _item = null;
			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				carrot_TextWidget query = CompiledQueries.cqTextWidgetByID(_db, textWidgetID);
				if (query != null) {
					_item = new TextWidget(query);
				}
			}

			return _item;
		}

		public static List<TextWidget> GetSiteTextWidgets(Guid siteID) {
			List<TextWidget> _lst = null;

			using (CarrotCMSDataContext _db = CarrotCMSDataContext.GetDataContext()) {
				IQueryable<carrot_TextWidget> query = CompiledQueries.cqTextWidgetBySiteID(_db, siteID);

				_lst = (from d in query.ToList()
						select new TextWidget(d)).ToList();
			}

			return _lst;
		}


	}
}
