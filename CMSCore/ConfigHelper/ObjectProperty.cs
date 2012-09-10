using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

	public class ObjectProperty {
		public ObjectProperty() { }
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
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is ObjectProperty) {
				ObjectProperty p = (ObjectProperty)obj;
				return (Name == p.Name) && (PropertyType == p.PropertyType);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return Name.GetHashCode() ^ PropertyType.ToString().GetHashCode();
		}

	}
}
