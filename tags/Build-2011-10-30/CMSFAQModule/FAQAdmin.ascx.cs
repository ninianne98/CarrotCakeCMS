using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.FAQModule {
	public partial class FAQAdmin : BaseShellUserControl, IAdminModule {

		#region IAdminModule Members

		public Guid SiteID { get; set; }

		public Guid ModuleID { get; set; }

		public string QueryStringFragment { get; set; }

		public string QueryStringPattern { get; set; }

		#endregion

        protected dbFAQDataContext db = new dbFAQDataContext();

        public string QueryStringFile { get; set; }


        protected void Page_Load(object sender, EventArgs e) {
			//if (SiteID == Guid.Empty) {
			//    SiteID = SiteData.CurrentSiteID;
			//}

            QueryStringFile = CurrentScriptName + "?" + string.Format(QueryStringPattern, "FAQAdminAddEdit");

            if (!IsPostBack) {
                LoadData();
            }

        }



        protected void LoadData() {

            var lst = (from c in db.tblFAQs
                       where c.SiteID == SiteID
                       orderby c.SortOrder
                       select c).ToList();


            dgMenu.DataSource = lst;
            dgMenu.DataBind();

        }



    }
}