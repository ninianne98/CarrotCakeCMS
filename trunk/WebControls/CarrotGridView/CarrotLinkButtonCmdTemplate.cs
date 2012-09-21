using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
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

namespace Carrotware.Web.UI.Controls {

	class CarrotLinkButtonCmdTemplate : ITemplate {
		private string _cmd { get; set; }
		private string _text { get; set; }
		private string _lnkname { get; set; }
		private int _idx { get; set; }

		public CarrotLinkButtonCmdTemplate(string lnkname, int idx, string text, string cmd) {
			_text = text;
			_cmd = cmd;
			_idx = idx;
			_lnkname = lnkname;
		}

		public void InstantiateIn(Control container) {
			if (!string.IsNullOrEmpty(_cmd)) {
				LinkButton lb = new LinkButton();
				lb.CommandName = _cmd;
				lb.ID = container.ID + _lnkname + _idx.ToString();
				lb.Text = _text;

				container.Controls.Add(lb);
			} else {
				Literal lit = new Literal();
				lit.Text = _text;
				container.Controls.Add(lit);
			}
		}

	}

	//=========================================

	public class CarrotAutoItemTemplate : ITemplate {
		private string _format { get; set; }
		private string _field { get; set; }

		public CarrotAutoItemTemplate(string fieldParm, string formatPattern) {
			_format = formatPattern;
			_field = fieldParm;
		}

		public void InstantiateIn(Control container) {

			Literal litContent = new Literal();
			litContent.Text = _field;

			litContent.DataBinding += new EventHandler(litContent_DataBinding);

			container.Controls.Add(litContent);
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			Literal litContent = (Literal)sender;
			GridViewRow container = (GridViewRow)litContent.NamingContainer;

			object oValue = DataBinder.Eval(container, "DataItem." + _field);

			litContent.Text = String.Format(_format, oValue);
		}

	}


}
