﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class InfoKVP {

		public InfoKVP() { }

		public InfoKVP(string k, string t) {
			this.InfoKey = k;
			this.InfoLabel = t;
		}

		public string InfoLabel { get; set; }
		public string InfoKey { get; set; }

		public override string ToString() {
			return InfoKey + " : " + InfoLabel;
		}

		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is InfoKVP) {
				InfoKVP p = (InfoKVP)obj;
				return (this.InfoKey == p.InfoKey);
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return InfoLabel.GetHashCode() ^ InfoKey.GetHashCode();
		}
	}
}
