using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Carrotware.CMS.Data;
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

	public class Widget : IDisposable {

		protected CarrotCMSDataContext db = new CarrotCMSDataContext();

		public Widget() { }

		public Widget(Guid rootWidgetID) {

			carrot_WidgetData w = (from r in db.carrot_WidgetDatas
								   where r.Root_WidgetID == rootWidgetID
									  && r.IsLatestVersion == true
								   select r).FirstOrDefault();

			SetVals(w);
		}

		public void LoadPageWidgetVersion(Guid widgetDataID) {

			carrot_WidgetData w = (from r in db.carrot_WidgetDatas
								   where r.WidgetDataID == widgetDataID
								   select r).FirstOrDefault();

			SetVals(w);
		}

		public Widget(carrot_WidgetData w) {
			SetVals(w);
		}

		private void SetVals(carrot_WidgetData w) {

			vw_carrot_Widget ww = db.vw_carrot_Widgets.Where(x => x.Root_WidgetID == w.Root_WidgetID && x.IsLatestVersion == true).FirstOrDefault();

			this.IsWidgetPendingDelete = false;

			this.WidgetDataID = w.WidgetDataID;
			this.EditDate = w.EditDate;
			this.IsLatestVersion = w.IsLatestVersion;
			this.ControlProperties = w.ControlProperties;

			if (ww != null) {
				this.Root_WidgetID = ww.Root_WidgetID;
				this.Root_ContentID = ww.Root_ContentID;
				this.WidgetOrder = ww.WidgetOrder;
				this.ControlPath = ww.ControlPath;
				this.PlaceholderName = ww.PlaceholderName;
				this.IsWidgetActive = ww.WidgetActive;
			}
		}

		public string ControlPath { get; set; }
		public string ControlProperties { get; set; }
		public Guid WidgetDataID { get; set; }
		public Guid Root_WidgetID { get; set; }
		public string PlaceholderName { get; set; }
		public Guid Root_ContentID { get; set; }
		public int WidgetOrder { get; set; }
		public bool? IsLatestVersion { get; set; }
		public bool IsWidgetActive { get; set; }
		public bool IsWidgetPendingDelete { get; set; }
		public DateTime EditDate { get; set; }


		public void Save() {

			if (!this.IsWidgetPendingDelete) {
				carrot_Widget w = (from r in db.carrot_Widgets
								   orderby r.WidgetOrder
								   where r.Root_WidgetID == this.Root_WidgetID
								   select r).FirstOrDefault();

				bool bAdd = false;
				if (w == null) {
					bAdd = true;
					w = new carrot_Widget();
				}

				if (this.Root_WidgetID == Guid.Empty) {
					this.Root_WidgetID = Guid.NewGuid();
				}

				w.Root_WidgetID = this.Root_WidgetID;

				w.WidgetOrder = this.WidgetOrder;
				w.Root_ContentID = this.Root_ContentID;
				w.PlaceholderName = this.PlaceholderName;
				w.ControlPath = this.ControlPath;
				w.WidgetActive = this.IsWidgetActive;

				carrot_WidgetData wd = new carrot_WidgetData();
				wd.Root_WidgetID = w.Root_WidgetID;
				wd.WidgetDataID = Guid.NewGuid();
				wd.IsLatestVersion = true;
				wd.ControlProperties = this.ControlProperties;
				wd.EditDate = DateTime.Now;

				carrot_WidgetData oldWD = (from ww in db.carrot_WidgetDatas
										   where ww.Root_WidgetID == w.Root_WidgetID
											  && ww.IsLatestVersion == true
										   select ww).FirstOrDefault();

				//only add a new entry if the widget has some sort of change in the data stored.
				if (oldWD != null) {
					if (oldWD.ControlProperties != wd.ControlProperties) {
						oldWD.IsLatestVersion = false;
						db.carrot_WidgetDatas.InsertOnSubmit(wd);
					}
				} else {
					db.carrot_WidgetDatas.InsertOnSubmit(wd);
				}

				if (bAdd) {
					db.carrot_Widgets.InsertOnSubmit(w);
				}

				db.SubmitChanges();

			} else {

				DeleteAll();

			}
		}


		public void Delete() {
			carrot_WidgetData w = (from r in db.carrot_WidgetDatas
								   where r.WidgetDataID == this.WidgetDataID
								   select r).FirstOrDefault();

			if (w != null) {
				db.carrot_WidgetDatas.DeleteOnSubmit(w);
				db.SubmitChanges();
			}
		}

		public void DeleteAll() {

			List<carrot_WidgetData> w1 = (from r in db.carrot_WidgetDatas
										  where r.Root_WidgetID == this.Root_WidgetID
										  select r).ToList();

			List<carrot_Widget> w2 = (from r in db.carrot_Widgets
									  where r.Root_WidgetID == this.Root_WidgetID
									  select r).ToList();

			bool bPendingDel = false;

			if (w1 != null) {
				foreach (carrot_WidgetData w in w1) {
					db.carrot_WidgetDatas.DeleteOnSubmit(w);
					bPendingDel = true;
				}
			}

			if (w2 != null) {
				foreach (carrot_Widget w in w2) {
					db.carrot_Widgets.DeleteOnSubmit(w);
					bPendingDel = true;
				}
			}

			if (bPendingDel) {
				db.SubmitChanges();
			}
		}


		public void Disable() {
			carrot_Widget w = (from r in db.carrot_Widgets
							   where r.Root_WidgetID == this.Root_WidgetID
							   select r).FirstOrDefault();

			if (w != null) {
				w.WidgetActive = false;
				db.SubmitChanges();
			}
		}

		public List<WidgetProps> ParseDefaultControlProperties() {
			List<WidgetProps> props = new List<WidgetProps>();
			string sProps = this.ControlProperties;

			if (!string.IsNullOrEmpty(sProps) && sProps.StartsWith("<?xml version=\"1.0\"")
					&& sProps.Contains("<KeyName") && sProps.Contains("<KeyValue")) {
				if (sProps.Contains("<ArrayOfWidgetProps")) {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WidgetProps>));
					Object genpref = null;
					using (StringReader stringReader = new StringReader(sProps)) {
						genpref = xmlSerializer.Deserialize(stringReader);
					}
					props = genpref as List<WidgetProps>;
				}
				if (sProps.Contains("<DefaultControlProperties")) {
					props = ParseDefaultControlPropertiesOld(sProps);
				}
			}

			return props;
		}


		public void SaveDefaultControlProperties(List<WidgetProps> props) {

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WidgetProps>));
			string sXML = "";
			using (StringWriter stringWriter = new StringWriter()) {
				xmlSerializer.Serialize(stringWriter, props);
				sXML = stringWriter.ToString();
			}

			this.ControlProperties = sXML;
		}


		private List<WidgetProps> ParseDefaultControlPropertiesOld(string sProps) {
			List<WidgetProps> props = new List<WidgetProps>();

			if (!string.IsNullOrEmpty(sProps) && sProps.StartsWith("<?xml")) {
				DataSet ds = new DataSet();
				using (StringReader stream = new StringReader(sProps)) {
					ds.ReadXml(stream);
				}

				props = (from d in ds.Tables[0].AsEnumerable()
						 select new WidgetProps {
							 KeyName = d.Field<string>("KeyName"),
							 KeyValue = d.Field<string>("KeyValue")
						 }).ToList();
			}

			return props;
		}


		private void SaveDefaultControlPropertiesOld(List<WidgetProps> props) {

			DataSet ds = new DataSet("DefaultControlProperties");
			DataTable dt = new DataTable("ControlProperties");
			DataColumn dc1 = new DataColumn("KeyName", typeof(System.String));
			DataColumn dc2 = new DataColumn("KeyValue", typeof(System.String));
			dt.Columns.Add(dc1);
			dt.Columns.Add(dc2);
			ds.Tables.Add(dt);

			foreach (WidgetProps p in props) {
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
