using System;

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

	public enum TagType {
		H1,
		H2,
		H3,
		H4,
		H5,
		H6,
		DIV,
		SPAN,
		B,
		I,
		STRONG,
		P,
		LI
	}

	//========================================
	public interface IHeadedList {
		string MetaDataTitle { get; set; }

		TagType HeadWrapTag { get; set; }

		int ItemCount { get; set; }
	}

	//========================================
	public interface ICMSCoreControl {
		bool IsAdminMode { get; set; }

		Guid DatabaseKey { get; set; }
	}

	//========================================
	public interface IActivateNavItem {
		string CSSSelected { get; set; }

		string CssClassNormal { get; set; }

		string CssClassHasChild { get; set; }

		bool IsSelected { get; set; }

		string NavigateUrl { get; set; }

		Guid ContentID { get; set; }
	}

	//========================================
	public interface IActivatePageNavItem {
		string CSSSelected { get; set; }

		string CssClassNormal { get; set; }

		bool IsSelected { get; set; }

		int PageNumber { get; set; }
	}
}