using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carrotware.CMS.Interface {
	public class WidgetAttribute : Attribute {

		public enum FieldMode {
			DropDownList,
			CheckBoxList,
			TextBox,
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
