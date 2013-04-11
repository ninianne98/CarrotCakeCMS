using System;
using System.ComponentModel.Design;
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
	public class GuidItemCollectionEditor : CollectionEditor {
		public GuidItemCollectionEditor(Type type)
			: base(type) {
		}

		protected override bool CanSelectMultipleInstances() {
			return false;
		}

		protected override Type CreateCollectionItemType() {
			return typeof(GuidItem);
		}
	}
}
