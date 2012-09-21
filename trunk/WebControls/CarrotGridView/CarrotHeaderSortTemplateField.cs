using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
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

	[DefaultProperty("Text"), ToolboxData("<{0}:CarrotHeaderSortTemplateField runat=server></{0}:CarrotHeaderSortTemplateField>")]

	public class CarrotHeaderSortTemplateField : TemplateField {

		public CarrotHeaderSortTemplateField()
			: base() {

		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string DataField {
			get {
				String s = ViewState["DataField"] as String;
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["DataField"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string DataFieldFormat {
			get {
				String s = ViewState["DataFieldFormat"] as String;
				return ((s == null) ? "{0}" : s);
			}
			set {
				ViewState["DataFieldFormat"] = value;
			}
		}


		//[Bindable(true)]
		//[Category("Appearance")]
		//[DefaultValue("")]
		//[Localizable(true)]
		//public string SortBy {
		//    get {
		//        String s = ViewState["SortBy"] as String;
		//        return ((s == null) ? String.Empty : s);
		//    }
		//    set {
		//        ViewState["SortBy"] = value;
		//    }
		//}



	}
}
