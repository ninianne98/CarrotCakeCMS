using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using Carrotware.CMS.Core;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Controls {


	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class PagedDataSummaryTitleOption {
		private PageViewType.ViewType _key;
		private string _label;

		public PagedDataSummaryTitleOption()
			: this(PageViewType.ViewType.SinglePage, String.Empty) {
		}

		public PagedDataSummaryTitleOption(PageViewType.ViewType key, string labelText) {
			_key = key;
			_label = labelText;
		}


		[
		Category("Behavior"),
		DefaultValue(PageViewType.ViewType.SinglePage),
		Description("KeyValue of PagedDataSummaryTitleOptions"),
		NotifyParentProperty(true)
		]
		public PageViewType.ViewType KeyValue {
			get {
				return _key;
			}
			set {
				_key = value;
			}
		}

		[
		Category("Behavior"),
		DefaultValue(""),
		Description("LabelText of PagedDataSummaryTitleOptions"),
		NotifyParentProperty(true)
		]
		public String LabelText {
			get {
				return _label;
			}
			set {
				_label = value;
			}
		}
	}

	//==================
	public class PagedDataSummaryTitleOptionEditor : CollectionEditor {
		public PagedDataSummaryTitleOptionEditor(Type type)
			: base(type) {
		}

		protected override bool CanSelectMultipleInstances() {
			return false;
		}

		protected override Type CreateCollectionItemType() {
			return typeof(PagedDataSummaryTitleOption);
		}
	}

}