using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;

namespace Carrotware.CMS.UI.Controls {
	public class ContentContainerDesigner : ControlDesigner {

		public override string GetDesignTimeHtml() {
			ContentContainer myctrl = (ContentContainer)base.ViewControl;
			string sPageOutText = "";

			string sPageText = SiteNavHelper.GetSampleBody(myctrl, "SampleContent3");
			if (myctrl.TextZone == ContentContainer.TextFieldZone.Unknown) {
				myctrl.TextZone = ContentContainer.TextFieldZone.TextCenter;
			}
			sPageOutText = "<h2>Content D CENTER</h2>\r\n" + sPageText;
			if (myctrl.ClientID.ToLower().Contains("left") || myctrl.TextZone == ContentContainer.TextFieldZone.TextLeft) {
				sPageOutText = "<h2>Content D LEFT</h2>\r\n" + sPageText;
			}

			if (myctrl.ClientID.ToLower().Contains("right") || myctrl.TextZone == ContentContainer.TextFieldZone.TextRight) {
				sPageOutText = "<h2>Content D RIGHT</h2>\r\n" + sPageText;
			}

			return sPageOutText;
		}


	}
}