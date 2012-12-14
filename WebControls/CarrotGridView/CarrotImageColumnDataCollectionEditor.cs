using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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
	public class CarrotImageColumnDataCollectionEditor : CollectionEditor {
		public CarrotImageColumnDataCollectionEditor(Type type)
			: base(type) {
		}

		protected override bool CanSelectMultipleInstances() {
			return false;
		}

		protected override Type CreateCollectionItemType() {
			return typeof(CarrotImageColumnData);
		}
	}
}
