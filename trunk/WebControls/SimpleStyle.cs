using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Carrotware.Web.UI.Controls {
	public class SimpleStyle {

		public SimpleStyle() { }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		public string CssClass { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		public string Style { get; set; }


		public string StyleToString() {
			if (!string.IsNullOrEmpty(this.Style)) {
				return string.Format(" style=\"{0}\" ", this.Style);
			} else {
				return string.Empty;
			}
		}

		public string CssClassToString() {
			if (!string.IsNullOrEmpty(this.CssClass)) {
				return string.Format(" class=\"{0}\" ", this.CssClass);
			} else {
				return string.Empty;
			}
		}

		public override string ToString() {
			return this.StyleToString() + this.CssClassToString();
		}

	}
}
