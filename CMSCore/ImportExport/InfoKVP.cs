using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
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
			return this.InfoKey + " : " + this.InfoLabel;
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
			return (this.InfoLabel ?? string.Empty).GetHashCode() ^ (this.InfoKey ?? string.Empty).ToLowerInvariant().GetHashCode();
		}
	}
}