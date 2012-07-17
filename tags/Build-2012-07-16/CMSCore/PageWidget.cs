using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Carrotware.CMS.Data;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class PageWidget : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public PageWidget() { }

		public PageWidget(Guid pageWidgetID) {

			var w = (from r in db.tblPageWidgets
					 orderby r.WidgetOrder
					 where r.PageWidgetID == pageWidgetID
					 select r).FirstOrDefault();

			SetVals(w);
		}

		public PageWidget(tblPageWidget w) {
			SetVals(w);
		}

		private void SetVals(tblPageWidget w) {
			this.PageWidgetID = w.PageWidgetID;
			this.WidgetOrder = w.WidgetOrder;
			this.Root_ContentID = w.Root_ContentID;
			this.PlaceholderName = w.PlaceholderName;
			this.ControlPath = w.ControlPath;
			this.ControlProperties = w.ControlProperties;
		}


		public string ControlPath { get; set; }
		public string ControlProperties { get; set; }
		public Guid PageWidgetID { get; set; }
		public string PlaceholderName { get; set; }
		public Guid Root_ContentID { get; set; }
		public int WidgetOrder { get; set; }




		public void Save() {
			var w = (from r in db.tblPageWidgets
					 orderby r.WidgetOrder
					 where r.PageWidgetID == this.PageWidgetID
					 select r).FirstOrDefault();

			bool bAdd = false;
			if (w == null) {
				bAdd = true;
				w = new tblPageWidget();
			}

			if (this.PageWidgetID == Guid.Empty) {
				this.PageWidgetID = Guid.NewGuid();
			} else {
				w.PageWidgetID = this.PageWidgetID;
			}

			w.WidgetOrder = this.WidgetOrder;
			w.Root_ContentID = this.Root_ContentID;
			w.PlaceholderName = this.PlaceholderName;
			w.ControlPath = this.ControlPath;
			w.ControlProperties = this.ControlProperties;

			if (bAdd) {
				db.tblPageWidgets.InsertOnSubmit(w);
			}
			db.SubmitChanges();
		}


		public void Delete() {
			var w = (from r in db.tblPageWidgets
					 where r.PageWidgetID == this.PageWidgetID
					 select r).FirstOrDefault();

			if (w != null) {
				db.tblPageWidgets.DeleteOnSubmit(w);
				db.SubmitChanges();
			}

		}


		public List<WidgetProps> ParseDefaultControlProperties() {
			List<WidgetProps> props = new List<WidgetProps>();
			var sProps = this.ControlProperties;

			if (!string.IsNullOrEmpty(sProps)) {
				if (sProps.StartsWith("<?xml")) {
					System.IO.StringReader stream = new System.IO.StringReader(sProps);

					DataSet ds = new DataSet();
					ds.ReadXml(stream);

					props = (from d in ds.Tables[0].AsEnumerable()
							 select new WidgetProps {
								 KeyName = d.Field<string>("KeyName"),
								 KeyValue = d.Field<string>("KeyValue")
							 }).ToList();
				}
			}

			return props;
		}


		public void SaveDefaultControlProperties(List<WidgetProps> props) {

			DataSet ds = new DataSet("DefaultControlProperties");
			DataTable dt = new DataTable("ControlProperties");
			DataColumn dc1 = new DataColumn("KeyName", typeof(System.String));
			DataColumn dc2 = new DataColumn("KeyValue", typeof(System.String));
			dt.Columns.Add(dc1);
			dt.Columns.Add(dc2);
			ds.Tables.Add(dt);

			foreach (var p in props) {
				DataRow newRow = ds.Tables["ControlProperties"].NewRow();
				newRow["KeyName"] = p.KeyName;
				newRow["KeyValue"] = p.KeyValue;
				ds.Tables["ControlProperties"].Rows.Add(newRow);
			}

			ds.AcceptChanges();

			this.ControlProperties = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>     " + ds.GetXml();

		}

		#region IDisposable Members

		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion
	}

	public class WidgetProps {

		public WidgetProps() { }

		public string KeyName { get; set; }
		public string KeyValue { get; set; }

	}


}
