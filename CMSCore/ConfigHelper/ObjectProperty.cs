using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public partial class ObjectProperty {

		public ObjectProperty() { }

		public ObjectProperty(PropertyInfo prop) {
			this.DefValue = null;
			this.Name = prop.Name;
			this.PropertyType = prop.PropertyType;
			this.CanRead = prop.CanRead;
			this.CanWrite = prop.CanWrite;
			this.Props = prop;
			this.CompanionSourceFieldName = String.Empty;
			this.FieldMode = (prop.PropertyType == typeof(bool)) ?
					WidgetAttribute.FieldMode.CheckBox : WidgetAttribute.FieldMode.TextBox;
		}

		public ObjectProperty(Object obj, PropertyInfo prop)
			: this(prop) {
			this.DefValue = obj.GetType().GetProperty(prop.Name).GetValue(obj, null);
		}

		public string Name { get; set; }
		public bool CanWrite { get; set; }
		public bool CanRead { get; set; }
		public Type PropertyType { get; set; }

		public Object DefValue { get; set; }

		public PropertyInfo Props { get; set; }

		public string CompanionSourceFieldName { get; set; }

		public string FieldDescription { get; set; }

		public WidgetAttribute.FieldMode FieldMode { get; set; }

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || this.GetType() != obj.GetType()) return false;
			if (obj is ObjectProperty) {
				ObjectProperty p = (ObjectProperty)obj;
				return (this.Name == p.Name) && (this.PropertyType == p.PropertyType);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return this.Name.GetHashCode() ^ this.PropertyType.ToString().GetHashCode();
		}

		//==========================

		public static List<ObjectProperty> GetObjectProperties(Object obj) {
			List<ObjectProperty> props = (from i in ReflectionUtilities.GetProperties(obj)
										  select GetCustProps(obj, i)).ToList();
			return props;
		}

		public static List<ObjectProperty> GetTypeProperties(Type theType) {
			List<ObjectProperty> props = (from i in ReflectionUtilities.GetProperties(theType)
										  select new ObjectProperty {
											  Name = i.Name,
											  PropertyType = i.PropertyType,
											  CanRead = i.CanRead,
											  CanWrite = i.CanWrite
										  }).ToList();
			return props;
		}

		public static ObjectProperty GetCustProps(Object obj, PropertyInfo prop) {
			ObjectProperty objprop = new ObjectProperty(obj, prop);

			try {
				foreach (Attribute attr in objprop.Props.GetCustomAttributes(true)) {
					if (attr is WidgetAttribute) {
						var widgetAttrib = attr as WidgetAttribute;
						if (null != widgetAttrib) {
							try { objprop.CompanionSourceFieldName = widgetAttrib.SelectFieldSource; } catch { objprop.CompanionSourceFieldName = ""; }
							try { objprop.FieldMode = widgetAttrib.Mode; } catch { objprop.FieldMode = WidgetAttribute.FieldMode.Unknown; }
						}
					}
				}
			} catch (Exception ex) { }

			objprop.FieldDescription = ReflectionUtilities.GetDescriptionAttribute(obj.GetType(), objprop.Name);

			return objprop;
		}
	}
}