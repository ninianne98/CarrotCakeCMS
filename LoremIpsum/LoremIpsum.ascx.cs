using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.CMS.UI.Plugins.LoremIpsum.Code;
using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Plugins.LoremIpsum {

	public partial class LoremIpsum : AdminModule {
		protected string _pluginKey { get; set; }
		protected ContentPageType.PageType _pageType = ContentPageType.PageType.Unknown;
		protected ContentCreator _creator = new ContentCreator();

		protected void Page_Load(object sender, EventArgs e) {
			ModuleID = AdminModuleQueryStringRoutines.GetModuleID();
			_pluginKey = AdminModuleQueryStringRoutines.GetPluginFile();

			if (_pluginKey == "pages") {
				_pageType = ContentPageType.PageType.ContentEntry;
			}
			if (_pluginKey == "posts") {
				_pageType = ContentPageType.PageType.BlogEntry;
			}

			litContent.Text = _pageType.ToString();
			_creator = new ContentCreator(_pageType);

			phPages.Visible = _creator.ContentType == ContentPageType.PageType.ContentEntry;
			phPosts.Visible = _creator.ContentType == ContentPageType.PageType.BlogEntry;

			if (!IsPostBack) {
				lblSummary.Text = string.Empty;
				txtMany.Text = _creator.HowMany.ToString();
				chkTop.Checked = _creator.TopLevel;

				txtTo.Text = _creator.DateTo.ToString(WebHelper.ShortDatePattern);
				txtFrom.Text = _creator.DateFrom.ToString(WebHelper.ShortDatePattern);

				txtCat.Text = _creator.Categories.ToString();
				txtTag.Text = _creator.Tags.ToString();
				txtComment.Text = _creator.BlogComments.ToString();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			_creator.HowMany = int.Parse(txtMany.Text);
			_creator.TopLevel = chkTop.Checked;

			_creator.DateTo = DateTime.Parse(txtTo.Text);
			_creator.DateFrom = DateTime.Parse(txtFrom.Text);

			_creator.Categories = int.Parse(txtCat.Text);
			_creator.Tags = int.Parse(txtTag.Text);
			_creator.BlogComments = int.Parse(txtComment.Text);

			if (_creator.ContentType == ContentPageType.PageType.ContentEntry) {
				_creator.BuildPages();
			}

			if (_creator.ContentType == ContentPageType.PageType.BlogEntry) {
				_creator.BuildPosts();
			}

			rpUrls.DataSource = _creator.PageLinks;
			rpUrls.DataBind();

			lblSummary.Text = string.Format("Created {0} entries.", _creator.PageLinks.Count);
		}
	}
}