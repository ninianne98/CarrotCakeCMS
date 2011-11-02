using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQModule {
	public partial class FAQDisplay : BaseShellUserControl, IWidget {

		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		public Guid SiteID { get; set; }

		public string JSEditFunction {
			get { return ""; }
		}
		public bool EnableEdit {
			get { return false; }
		}
		#endregion

		protected int iFaq = 0;

		protected string FaqCounter() {
			iFaq++;
			return iFaq.ToString();
		}

		protected void Page_Load(object sender, EventArgs e) {
			//if (SiteID == Guid.Empty) {
			//    SiteID = SiteData.CurrentSiteID;
			//}

			dbFAQDataContext db = new dbFAQDataContext();

			var dtFAQ = (from f in db.tblFAQs
						 where f.SiteID == SiteID
						 orderby f.SortOrder
						 select f).ToList();

			if (dtFAQ == null) {
				dgFAQ.Visible = false;
			} else {
				dgFAQ.DataSource = dtFAQ;
				dgFAQ.DataBind();
				dgFAQ.Visible = true;
			}

		}
	}
}