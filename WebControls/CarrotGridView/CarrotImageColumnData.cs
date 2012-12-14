using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
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
	public class CarrotImageColumnData {
		private string _name;
		private string _imagePath;
		private string _caption;

		public CarrotImageColumnData()
			: this(String.Empty, String.Empty, String.Empty) {
		}

		public CarrotImageColumnData(string key, string image, string label) {
			_name = key;
			_imagePath = image;
			_caption = label;
		}


		[
		Category("Behavior"),
		DefaultValue(""),
		Description("KeyValue of CarrotImageColumnData"),
		NotifyParentProperty(true)
		]
		public String KeyValue {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}


		[
		Category("Behavior"),
		DefaultValue(""),
		Description("ImagePath of CarrotImageColumnData"),
		NotifyParentProperty(true)
		]
		public String ImagePath {
			get {
				return _imagePath;
			}
			set {
				_imagePath = value;
			}
		}


		[
		Category("Behavior"),
		DefaultValue(""),
		Description("ImageAltText of CarrotImageColumnData"),
		NotifyParentProperty(true)
		]
		public String ImageAltText {
			get {
				return _caption;
			}
			set {
				_caption = value;
			}
		}

	}
}


/*
<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="PageActive" HeaderText="Active" ShowEnumImage="true">
	<ImageSelectors>
		<carrot:CarrotImageColumnData KeyValue="true" ImageAltText="ACTIVE" ImagePath="images/shield.png" />
		<carrot:CarrotImageColumnData KeyValue="false" ImageAltText="INACTIVE" ImagePath="images/cross.png" />
	</ImageSelectors>
</carrot:CarrotHeaderSortTemplateField>
*/