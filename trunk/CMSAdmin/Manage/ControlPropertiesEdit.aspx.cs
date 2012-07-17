using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Base;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class ControlPropertiesEdit : AdminBasePage {

		public Guid guidWidget = Guid.Empty;
		public Guid guidPage = Guid.Empty;
		public List<WidgetProps> lstProps = null;
		public List<ObjectProperty> lstDefProps = null;


		protected void Page_Load(object sender, EventArgs e) {

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				guidWidget = new Guid(Request.QueryString["id"].ToString());
			}

			if (!string.IsNullOrEmpty(Request.QueryString["pageid"])) {
				guidPage = new Guid(Request.QueryString["pageid"].ToString());
			}

			var pageContents = pageHelper.GetLatestContent(SiteID, guidPage);
			cmsHelper.OverrideKey(pageContents.FileName);


			PageWidget w = (from aw in cmsHelper.cmsAdminWidget
							where aw.Root_WidgetID == guidWidget
							orderby aw.WidgetOrder
							select aw).FirstOrDefault();


			if (!IsPostBack) {

				lstProps = w.ParseDefaultControlProperties();

				Control widget = new Control();

				if (w.ControlPath.EndsWith(".ascx")) {
					try {
						widget = Page.LoadControl(w.ControlPath);
					} catch (Exception ex) {
					}
				}

				if (w.ControlPath.ToLower().StartsWith("class:")) {
					try {
						Assembly a = Assembly.GetExecutingAssembly();
						var className = w.ControlPath.Replace("CLASS:", "");
						Type t = Type.GetType(className);
						Object o = Activator.CreateInstance(t);
						if (o != null) {
							widget = o as Control;
						}
					} catch (Exception ex) {
					}
				}

				lstDefProps = cmsHelper.GetProperties(widget);
				List<ObjectProperty> props1 = new List<ObjectProperty>();
				List<ObjectProperty> props2 = new List<ObjectProperty>();
				List<ObjectProperty> props3 = new List<ObjectProperty>();
				List<ObjectProperty> props4 = new List<ObjectProperty>();
				List<ObjectProperty> props5 = new List<ObjectProperty>();
				List<ObjectProperty> props6 = new List<ObjectProperty>();

				if (widget is Carrotware.CMS.UI.Base.BaseUserControl) {
					props1 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.UI.Base.BaseUserControl));
				}
				if (widget is Carrotware.CMS.Interface.IWidget) {
					props2 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.Interface.IWidget));
				}
				if (widget is Carrotware.CMS.Interface.BaseShellUserControl) {
					props3 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.Interface.BaseShellUserControl));
				}
				if (widget is System.Web.UI.UserControl) {
					props4 = cmsHelper.GetTypeProperties(typeof(System.Web.UI.UserControl));
				}
				if (widget is Carrotware.CMS.Interface.IWidgetParmData) {
					props5 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.Interface.IWidgetParmData));
				}
				if (widget is Carrotware.CMS.Interface.IWidgetEditStatus) {
					props6 = cmsHelper.GetTypeProperties(typeof(Carrotware.CMS.Interface.IWidgetEditStatus));
				}

				rpProps.DataSource = (from p in lstDefProps
									  where p.CanRead == true
									  && p.CanWrite == true
									  && !props1.Contains(p)
									  && !props2.Contains(p)
									  && !props3.Contains(p)
									  && !props4.Contains(p)
									  && !props5.Contains(p)
									  && !props6.Contains(p)
									  select p).ToList();

				rpProps.DataBind();
			}
		}

		public string GetSavedValue(string sDefVal, string sName) {

			var pp = (from p in lstProps
					  where p.KeyName.ToLower() == sName.ToLower()
					  select p).FirstOrDefault();

			if (pp == null) {

				var dp = (from p in lstDefProps
						  where p.Name.ToLower() == sName.ToLower()
						  select p).FirstOrDefault();

				string sType = dp.PropertyType.ToString().ToLower();
				if (dp.DefValue != null) {
					sDefVal = dp.DefValue.ToString();
					switch (sType) {
						case "system.boolean":
							bool vB = Convert.ToBoolean(dp.DefValue.ToString());
							sDefVal = vB.ToString();
							break;
						case "system.drawing.color":
							System.Drawing.Color vC = (System.Drawing.Color)dp.DefValue;
							sDefVal = System.Drawing.ColorTranslator.ToHtml(vC);
							break;
						default:
							sDefVal = dp.DefValue.ToString();
							break;
					}
				} else {
					sDefVal = "";
				}

				return sDefVal;
			} else {
				return pp.KeyValue;
			}
		}

		protected void rpProps_Bind(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item) {

				HiddenField hdnName = (HiddenField)e.Item.FindControl("hdnName");
				TextBox txtValue = (TextBox)e.Item.FindControl("txtValue");
				DropDownList ddlValue = (DropDownList)e.Item.FindControl("ddlValue");
				CheckBox chkValue = (CheckBox)e.Item.FindControl("chkValue");
				CheckBoxList chkValues = (CheckBoxList)e.Item.FindControl("chkValues");

				txtValue.Visible = true;
				string sName = hdnName.Value;

				ObjectProperty ListSourceProperty = new ObjectProperty();

				string sListSourcePropertyName = (from p in lstDefProps
												  where p.CanRead == true
													 && p.CanWrite == false
													 && (p.Name.ToLower() == ("dict" + sName).ToLower()
														 || p.Name.ToLower() == ("chk" + sName).ToLower())
												  select p.Name).FirstOrDefault();

				if (string.IsNullOrEmpty(sListSourcePropertyName)) {
					sListSourcePropertyName = (from p in lstDefProps
											   where p.Name.ToLower() == sName.ToLower()
													 && !string.IsNullOrEmpty(p.CompanionSourceFieldName)
											   select p.CompanionSourceFieldName).FirstOrDefault();

					if (string.IsNullOrEmpty(sListSourcePropertyName)) {
						sListSourcePropertyName = "";
					}

					ListSourceProperty = (from p in lstDefProps
										  where p.CanRead == true
										  && p.CanWrite == false
										  && p.Name.ToLower() == sListSourcePropertyName.ToLower()
										  select p).FirstOrDefault();

				} else {
					if (string.IsNullOrEmpty(sListSourcePropertyName)) {
						sListSourcePropertyName = "";
					}

					ListSourceProperty = (from p in lstDefProps
										  where p.CanRead == true
										  && p.CanWrite == false
										  && (p.Name.ToLower() == ("dict" + sName).ToLower()
											  || p.Name.ToLower() == ("chk" + sName).ToLower())
										  select p).FirstOrDefault();
				}

				var dp = (from p in lstDefProps
						  where p.Name.ToLower() == sName.ToLower()
						  select p).FirstOrDefault();


				if (ListSourceProperty != null) {
					if (ListSourceProperty.DefValue is Dictionary<string, string>) {
						txtValue.Visible = false;

						//work with a drop down list, only allow one item in the drop down.
						if (sListSourcePropertyName.StartsWith("dict") || dp.FieldMode == WidgetAttribute.FieldMode.DropDownList) {
							ddlValue.Visible = true;

							ddlValue.DataTextField = "Value";
							ddlValue.DataValueField = "Key";

							ddlValue.DataSource = ListSourceProperty.DefValue;
							ddlValue.DataBind();

							ddlValue.Items.Insert(0, new ListItem("-Select Value-", ""));

							if (!string.IsNullOrEmpty(txtValue.Text)) {
								try {
									ddlValue.SelectedValue = txtValue.Text;
								} catch { }
							}
						}

						// work with a checkbox list, allow more than one value
						if (sListSourcePropertyName.StartsWith("chk") || dp.FieldMode == WidgetAttribute.FieldMode.CheckBoxList) {
							chkValues.Visible = true;

							chkValues.DataTextField = "Value";
							chkValues.DataValueField = "Key";

							chkValues.DataSource = ListSourceProperty.DefValue;
							chkValues.DataBind();

							// since this is a multi selected capable field, look for anything that starts with the 
							// field name and has the delimeter trailing
							var pp = (from p in lstProps
									  where p.KeyName.ToLower().StartsWith(sName.ToLower() + "|")
									  select p).ToList();

							if (pp.Count > 0) {
								foreach (ListItem v in chkValues.Items) {
									v.Selected = (from p in pp
												  where p.KeyValue == v.Value
												  select p.KeyValue).Count() < 1 ? false : true;
								}
							}
						}
					}
				}

				string sType = dp.PropertyType.ToString().ToLower();
				if (sType == "system.boolean" || dp.FieldMode == WidgetAttribute.FieldMode.CheckBox) {
					txtValue.Visible = false;
					chkValue.Visible = true;

					chkValue.Checked = Convert.ToBoolean(txtValue.Text);
				}

			}
		}


		protected void btnSave_Click(object sender, EventArgs e) {

			PageWidget w = (from aw in cmsHelper.cmsAdminWidget
							where aw.Root_WidgetID == guidWidget
							orderby aw.WidgetOrder
							select aw).FirstOrDefault();

			var props = new List<WidgetProps>();

			foreach (RepeaterItem r in rpProps.Items) {

				HiddenField hdnName = (HiddenField)r.FindControl("hdnName");
				TextBox txtValue = (TextBox)r.FindControl("txtValue");
				DropDownList ddlValue = (DropDownList)r.FindControl("ddlValue");
				CheckBox chkValue = (CheckBox)r.FindControl("chkValue");
				CheckBoxList chkValues = (CheckBoxList)r.FindControl("chkValues");

				var p = new WidgetProps();
				p.KeyName = hdnName.Value;

				if (ddlValue.Visible) {
					// drop down list detected, save the selected item
					p.KeyValue = ddlValue.SelectedValue;
				} else if (chkValue.Visible) {
					//boolean detected
					p.KeyValue = chkValue.Checked.ToString();
				} else if (chkValues.Visible) {
					//multiple selections are possible, since dictionary is used, insure key is unique by appending the ordinal with a | delimeter.
					p = null;
					int CheckedPosition = 0;
					foreach (ListItem v in chkValues.Items) {
						if (v.Selected) {
							var pp = new WidgetProps();
							pp.KeyName = hdnName.Value + "|" + CheckedPosition.ToString();
							pp.KeyValue = v.Value.ToString();
							props.Add(pp);
							CheckedPosition++;
						}
					}
				} else {
					//default, free text field
					p.KeyValue = txtValue.Text;
				}

				if (p != null) {
					props.Add(p);
				}
			}

			w.SaveDefaultControlProperties(props);

			List<PageWidget> lstPageWidgets = cmsHelper.cmsAdminWidget;
			lstPageWidgets.RemoveAll(x => x.Root_WidgetID == guidWidget);
			lstPageWidgets.Add(w);

			cmsHelper.cmsAdminWidget = lstPageWidgets;

		}


	}
}
