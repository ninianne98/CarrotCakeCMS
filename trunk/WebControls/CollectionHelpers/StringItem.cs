using System;
using System.ComponentModel;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class StringItem {
		private String _value;

		public StringItem()
			: this(String.Empty) {
		}

		public StringItem(String stringValue) {
			_value = stringValue;
		}

		[
		Category("Behavior"),
		DefaultValue(""),
		Description("Name of StringItem"),
		NotifyParentProperty(true),
		]
		public String StringValue {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}


	}
}
