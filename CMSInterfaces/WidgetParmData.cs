using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carrotware.CMS.Interface {
	public class WidgetParmData : BaseShellUserControl, IWidgetParmData {

		#region IWidgetParmData Members

		private Dictionary<string, string> _parms = new Dictionary<string, string>();
		public Dictionary<string, string> PublicParmValues {
			get { return _parms; }
			set { _parms = value; }
		}

		#endregion

		protected string GetParmValue(string sKey) {
			string ret = null;

			if (PublicParmValues.Count > 0) {
				ret = (from c in PublicParmValues
					   where c.Key.ToLower() == sKey.ToLower()
					   select c.Value).FirstOrDefault();
			}

			return ret;
		}

		protected string GetParmValue(string sKey, string sDefault) {
			string ret = null;

			if (PublicParmValues.Count > 0) {
				ret = (from c in PublicParmValues
					   where c.Key.ToLower() == sKey.ToLower()
					   select c.Value).FirstOrDefault();
			}

			ret = ret == null ? sDefault : ret;

			return ret;
		}


		protected List<string> GetParmValueList(string sKey) {

			sKey = sKey.EndsWith("|") ? sKey : sKey + "|";
			sKey = sKey.ToLower();

			List<string> ret = new List<string>();

			if (PublicParmValues.Count > 0) {
				ret = (from c in PublicParmValues
					   where c.Key.ToLower().StartsWith(sKey)
					   select c.Value).ToList();
			}

			return ret;
		}



	}
}
