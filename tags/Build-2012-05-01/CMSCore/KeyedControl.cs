using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class KeyedControl {

		public KeyedControl() { }


		public string KeyName { get; set; }
		public Control KeyControl { get; set; }


		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is KeyedControl) {
				KeyedControl p = (KeyedControl)obj;
				return (KeyName == p.KeyName);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return KeyName.GetHashCode() ^ KeyControl.GetHashCode();
		}

	}
}
