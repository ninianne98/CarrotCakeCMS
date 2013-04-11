using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
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

	public abstract class BaseNavHeaded : BaseNav, IHeadedList {

		public int ItemCount { get; set; }

		[Category("Appearance")]
		[DefaultValue("")]
		public string MetaDataTitle {
			get {
				string s = (string)ViewState["MetaDataTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["MetaDataTitle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("H2")]
		public TagType HeadWrapTag {
			get {
				String s = (String)ViewState["HeadWrapTag"];
				TagType c = TagType.H2;
				if (!string.IsNullOrEmpty(s)) {
					c = (TagType)Enum.Parse(typeof(TagType), s, true);
				}
				return c;
			}

			set {
				ViewState["HeadWrapTag"] = value.ToString();
			}
		}

		protected override void WriteListPrefix(HtmlTextWriter output) {

			if (this.NavigationData != null) {
				this.ItemCount = this.NavigationData.Count;
			}

			if (this.NavigationData != null && this.NavigationData.Count > 0 && !string.IsNullOrEmpty(this.MetaDataTitle)) {
				output.WriteLine("<" + this.HeadWrapTag.ToString().ToLower() + ">" + this.MetaDataTitle + "</" + this.HeadWrapTag.ToString().ToLower() + ">\r\n");
			}

			base.WriteListPrefix(output);
		}

		protected override void OnPreRender(EventArgs e) {

			base.OnPreRender(e);

			try {

				if (PublicParmValues.Count > 0) {
					this.MetaDataTitle = GetParmValue("MetaDataTitle", "");
				}
			} catch (Exception ex) {
			}
		}

	}
}
