using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
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
	public class BaseShellUserControl : System.Web.UI.UserControl {

		protected string CurrentDLLVersion {
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}


		public string CurrentScriptName {
			get { return Request.ServerVariables["script_name"].ToString(); }
		}


	}
}