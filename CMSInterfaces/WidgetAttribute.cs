using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Interface {

	public class WidgetAttribute : Attribute {

		public enum FieldMode {
			Unknown,
			DictionaryList,
			DropDownList,
			CheckBoxList,
			TextBox,
			ColorBox,
			MultiLineTextBox,
			RichHTMLTextBox,
			CheckBox
		}

		public WidgetAttribute(FieldMode mode) {
			this._mode = mode;
		}

		public WidgetAttribute(FieldMode mode, string field) {
			this._mode = mode;
			this._field = field;
		}

		private FieldMode _mode;

		public FieldMode Mode {
			get {
				return this._mode;
			}
		}

		private string _field;

		public string SelectFieldSource {
			get {
				return this._field;
			}
		}
	}
}