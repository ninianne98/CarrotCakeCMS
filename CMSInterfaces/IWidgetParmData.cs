using System;
using System.Collections.Generic;
using System.Linq;
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

	public interface IWidgetParmData {


		Dictionary<string, string> PublicParmValues { get; set; }

	}
}
