using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Interface {
	public abstract class WidgetParmDataWebControl : WidgetWebControl, IWidgetParmData {

		#region IWidgetParmData Members

		private Dictionary<string, string> _parms = new Dictionary<string, string>();
		public virtual Dictionary<string, string> PublicParmValues {
			get { return _parms; }
			set { _parms = value; }
		}

		#endregion


		#region Common Parser Routines

		protected string GetParmValue(string sKey) {
			return ParmParser.GetParmValue(this.PublicParmValues, sKey);
		}

		protected string GetParmValue(string sKey, string sDefault) {
			return ParmParser.GetParmValue(this.PublicParmValues, sKey, sDefault);
		}

		protected string GetParmValueDefaultEmpty(string sKey, string sDefault) {
			return ParmParser.GetParmValueDefaultEmpty(this.PublicParmValues, sKey, sDefault);
		}

		protected List<string> GetParmValueList(string sKey) {
			return ParmParser.GetParmValueList(this.PublicParmValues, sKey);
		}

		#endregion

	}
}
