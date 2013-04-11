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
	public class GuidItem {
		private Guid _value;

		public GuidItem()
			: this(Guid.Empty) {
		}

		public GuidItem(Guid guidValue) {
			_value = guidValue;
		}

		[
		Category("Behavior"),
		DefaultValue(""),
		Description("Name of GuidItem"),
		NotifyParentProperty(true),
		]
		public Guid GuidValue {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}


	}
}
