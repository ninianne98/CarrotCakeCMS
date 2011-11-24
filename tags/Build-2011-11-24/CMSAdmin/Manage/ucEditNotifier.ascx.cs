using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Admin.Manage {
	public partial class ucEditNotifier : BaseUserControl {

		public Guid CurrentPageID {
			get;
			set;
		}

		protected void Page_Load(object sender, EventArgs e) {
			var p = pageHelper.GetLatestContent(SiteID, null, CurrentScriptName);
			CurrentPageID = p.Root_ContentID;
		}
	}
}