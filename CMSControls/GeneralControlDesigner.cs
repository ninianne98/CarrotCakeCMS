using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
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
	public class GeneralControlDesigner : ControlDesigner {

		public override string GetDesignTimeHtml() {
			Control myctrl = (Control)base.ViewControl;

			string sType = myctrl.GetType().ToString().Replace(myctrl.GetType().Namespace + ".", "CMS, ");
			string sID = myctrl.ID;

			string sTextOut = "[" + sType + " - " + sID + "]";

			if (myctrl is ITextControl) {
				if (!string.IsNullOrEmpty(((ITextControl)myctrl).Text)) {
					sTextOut = "[" + sType + " - " + sID + " : " + ((ITextControl)myctrl).Text + "]";
				}
			}

			/*
			if (myctrl is SearchBox) {
				SearchBox s = (SearchBox)myctrl;
				PlaceHolder p = new PlaceHolder();
				//int child = s.TemplateControl.Controls.Count;
				int child = s.Controls.Count;
				if (s.TemplateControl != null) {
					sTextOut = "";
					for (int i = 0; i < child; i++) {
						//p.Controls.Add(s.TemplateControl.Controls[0]);
						p.Controls.Add(s.Controls[0]);
					}
					sTextOut += RenderCtrl(p);
				}
			}
			*/

			if (myctrl is ContentPageProperty) {
				sTextOut = "[" + sType + " : " + ((ContentPageProperty)myctrl).DataField.ToString() + "]";
			}

			if (myctrl is SiteDataProperty) {
				sTextOut = "[" + sType + " : " + ((SiteDataProperty)myctrl).DataField.ToString() + "]";
			}

			return sTextOut;
		}


		private string RenderCtrl(Control ctrl) {
			StringWriter sw = new StringWriter();
			HtmlTextWriter tw = new HtmlTextWriter(sw);

			ctrl.RenderControl(tw);

			return sw.ToString();
		}


	}
}