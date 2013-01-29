<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdvancedEdit.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucAdvancedEdit" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<!-- CarrotCake CMS Editor Control BEGIN -->
<%--<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="NotUsed"   />--%>
<asp:PlaceHolder ID="plcIncludes" runat="server">
	<link href="/c3-admin/includes/advanced-editor-reset.css" rel="stylesheet" type="text/css" />
	<link href="/c3-admin/glossyseagreen/jquery-ui-glossyseagreen-scoped2.css" rel="stylesheet" type="text/css" />
	<link href="/c3-admin/glossyseagreen/jquery-ui-glossyseagreen-scoped.css" rel="stylesheet" type="text/css" />
	<link href="/c3-admin/includes/advanced-editor.css" rel="stylesheet" type="text/css" />
	<link href="/c3-admin/includes/modal.css" rel="stylesheet" type="text/css" />
</asp:PlaceHolder>
<script src="/c3-admin/includes/jquery.simplemodal.js" type="text/javascript"></script>
<script src="/c3-admin/includes/jquery.blockUI.js" type="text/javascript"></script>
<script src="/c3-admin/includes/base64.js" type="text/javascript"></script>
<script src="/c3-admin/includes/advanced-editor.js" type="text/javascript"></script>
<script type="text/javascript">
	var jqLoadCount = 1;
	function cmsLoadJQDyn() {
		if (jqLoadCount <= 20) {
			setTimeout("cmsLoadJQDyn();", 2000);

			var jq2URL = '<%=GetWebControlUrl("Carrotware.Web.UI.Controls.jquery183.js")%>';
			var jq1URL = '<%=GetWebControlUrl("Carrotware.Web.UI.Controls.jqueryui-1-10-0.js")%>';

			cmsSetJQueryURL(jq2URL, jq1URL);

			jqLoadCount++;
		}
	}

	cmsLoadJQDyn();
</script>
<script type="text/javascript">
	var cmsPageInit2 = false;

	if (typeof jQuery == 'undefined') {
		setTimeout("cmsToolbarPageInit2();", 1000);
		setTimeout("cmsToolbarPageInit2();", 1500);
		setTimeout("cmsToolbarPageInit2();", 2500);
	}
	
	$(document).ready(function () {
		cmsToolbarPageInit2();
	});

	function cmsToolbarPageInit2() {
		if (!cmsPageInit2) {
			<% if (!bLocked) { %>
				setTimeout('cmsEditHB();', 500);
			<%} else { %>
				cmsAlertModal('<%=litUser.Text %>');
			<%} %>
			cmsPageInit2 = true;
		}

		$(".cmsGlossySeaGreen input:button, .cmsGlossySeaGreen input:submit, .cmsGlossySeaGreen input:reset").button();
		$("#cmsGlossySeaGreenID input:button, #cmsGlossySeaGreenID input:submit, #cmsGlossySeaGreenID input:reset").button();
	}

</script>
<script type="text/javascript">

	var cmsPageInit1 = false;

	if (typeof jQuery == 'undefined') {
		setTimeout("cmsToolbarPageInit1();", 1000);
		setTimeout("cmsToolbarPageInit1();", 1500);
		setTimeout("cmsToolbarPageInit1();", 2500);
	}

	$(document).ready(function () {
		cmsToolbarPageInit1();
	});

	function cmsToolbarPageInit1() {

		var cmsWebSvc = "/c3-admin/CMS.asmx";
		var cmsThisPage = "<%=SiteData.AlternateCurrentScriptName %>";
		var cmsThisPageID = "<%=guidContentID.ToString() %>";

		var cmsTabIdx = parseInt('<%=EditorPrefs.EditorSelectedTabIdx %>');
		var cmsMargin = '<%=EditorPrefs.EditorMargin %>';
		var cmsScrollPos = parseInt('<%= EditorPrefs.EditorScrollPosition %>');

		var cmsTimeTick = "<%=DateTime.Now.Ticks.ToString() %>";
		var cmsOpenStat = true;

		if ('<%=bLocked.ToString().ToLower() %>' != 'true') {
			cmsSetPageStatus(false);
		} else {
			cmsSetPageStatus(true);
		}

		if ('<%= EditorPrefs.EditorOpen %>' == 'false') {
			cmsOpenStat = false;
		}

		cmsSetPrefs(cmsTabIdx, cmsMargin, cmsScrollPos, cmsOpenStat);

		cmsSetServiceParms(cmsWebSvc, cmsThisPage, cmsThisPageID, cmsTimeTick);

		cmsSetTemplateDDL('#<%=ddlTemplate.ClientID%>');

		cmsSetPreviewFileName('<%=SiteData.PreviewTemplateFilePage %>');

		cmsOverridePageName('<%=EditedPageFileName %>');

		if (!cmsPageInit1) {

			cmsInitWidgets();

			cmsPageLockCheck();

			cmsResetToolbarScroll();

			setTimeout("cmsBuildOrder();", 250);

			cmsPageInit1 = true;
		}
	}

</script>
<div style="display: none;">
	<img src="/c3-admin/images/cog.png" alt="" />
	<img src="/c3-admin/images/cross.png" alt="" />
	<img src="/c3-admin/images/pencil.png" alt="" />
	<img src="/c3-admin/images/application_view_tile.png" alt="" />
	<img src="/c3-admin/images/layout.png" alt="" />
	<img src="/c3-admin/images/arrow_in.png" alt="" />
	<img src="/c3-admin/images/dragbg.png" alt="" />
	<img src="/c3-admin/images/x.png" alt="x" />
</div>
<div style="display: none" id="cmsGlossySeaGreenID">
	<div id="cms-basic-modal-content" class="cmsGlossySeaGreen">
		<div id="cmsModalFrame">
		</div>
	</div>
</div>
<div id="cmsToolBoxWrap" class="<%=String.Format("cmsToolbarAlignment{0}", EditorPrefs.EditorMargin) %>">
	<div id="cmsAdminToolbox" class="cmsGlossySeaGreen cmsToolbox2" style="padding: 2px; margin: 0px; min-height: 300px; width: 290px;">
		<div id="cmsGlossySeaGreenID" class="cmsInsideArea">
			<div onclick="cmsToggleMenu();" id="cmsMnuToggle" class='ui-icon ui-icon-minusthick cmsFloatRight' title="toggle">
				T
			</div>
			<div onclick="cmsShiftPosition('R')" id="cmsMnuRight" class='ui-icon ui-icon-circle-triangle-e cmsFloatRight' title="R">
				R
			</div>
			<div onclick="cmsShiftPosition('L')" id="cmsMnuLeft" class='ui-icon ui-icon-circle-triangle-w cmsFloatRight' title="L">
				L
			</div>
			<p class="ui-widget-header ui-corner-all" style="padding: 5px; margin: 0px; text-align: left;">
				Toolbox
			</p>
			<div id="cmsDivActive">
				<div class="ui-widget" runat="server" id="cmsDivEditing">
					<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px;">
						<p>
							<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
							<asp:Literal ID="litUser" runat="server">&nbsp;</asp:Literal></p>
					</div>
				</div>
				<div id="cmsMainToolbox">
					<asp:Panel ID="pnlCMSEditZone" runat="server">
						<div id="cmsJQTabedToolbox" style="min-height: 50px; width: 275px;">
							<ul>
								<li><a href="#cmsTabIdx-tabs-1">Widgets</a></li>
								<li><a href="#cmsTabIdx-tabs-2">Page Info</a></li>
							</ul>
							<div id="cmsTabIdx-tabs-1">
								<div style="display: none;">
									cmsFullOrder<br />
									<textarea rows="5" cols="30" id="cmsFullOrder" style="width: 310px; height: 50px;"></textarea><br />
									cmsMovedItem<br />
									<input type="text" id="cmsMovedItem" style="width: 310px;" /><br />
								</div>
								<div class="cmsCenter5px">
									<input type="button" runat="server" id="btnToolboxSave1" value="Save" onclick="cmsApplyChanges();" />
									&nbsp;&nbsp;&nbsp;
									<input type="button" id="btnToolboxCancel1" value="Cancel" onclick="cmsCancelEdit();" />
								</div>
								<asp:Repeater ID="rpTools" runat="server">
									<HeaderTemplate>
										<div id="cmsToolBox" class="ui-widget-content ui-corner-all" style="overflow: auto; height: 290px; width: 250px; padding: 5px; margin: 5px; float: left;
											border: solid 1px #000;">
									</HeaderTemplate>
									<ItemTemplate>
										<div id="cmsToolItemDiv" class="cmsToolItem cmsToolItemWrapper">
											<div class="cmsWidgetControlItem cmsWidgetToolboxItem cmsWidgetCtrlPath" id="cmsControl">
												<p class="cmsToolItem ui-widget-header">
													<%# Eval("Caption")%>
												</p>
												<p class="cmsWidgetToolboxPath">
													<%# String.Format("{0}", Eval("FilePath")).Replace(".", ". ").Replace("/", "/ ")%><br />
												</p>
												<input type="hidden" id="cmsCtrlID" value="<%# Eval("FilePath")%>" />
												<input type="hidden" id="cmsCtrlOrder" value="0" />
											</div>
										</div>
									</ItemTemplate>
									<FooterTemplate>
										</div></FooterTemplate>
								</asp:Repeater>
								<br style="clear: both;" />
							</div>
							<div id="cmsTabIdx-tabs-2">
								<div class="cmsLeft5px">
									<p>
										Templates / Skins<br />
										<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
										</asp:DropDownList>
									</p>
									<p class="cmsCenter5px">
										<input runat="server" id="btnTemplate" type="button" value="Apply" onclick="cmsUpdateTemplate();" />
										&nbsp;&nbsp;&nbsp;
										<input runat="server" id="btnPreview" type="button" value="Preview" onclick="cmsPreviewTemplate();" />
										<br />
									</p>
									<p>
										<input runat="server" id="btnEditCoreInfo" type="button" value="Edit Core Page Info" />
									</p>
									<p>
										<input runat="server" id="btnAddTop" type="button" value="Create Top Level Page" onclick="cmsShowAddTopPage();" />
									</p>
									<p>
										<input runat="server" id="btnAddChild" type="button" value="Create Sub Page" onclick="cmsShowAddChildPage();" />
									</p>
									<p>
										<input runat="server" id="btnSortChildPages" type="button" value="Sort Child/Sub Pages" onclick="cmsSortChildren();" />
									</p>
									<p>
										<input runat="server" id="btnSiteMap" type="button" value="Site Map" onclick="cmsEditSiteMap();" />
									</p>
									<p>
										<input runat="server" id="btnAllWidgets" type="button" value="View Full Widget List" onclick="cmsShowWidgetList();" />
									</p>
								</div>
								<div class="cmsCenter5px">
									<input type="button" runat="server" id="btnToolboxSave2" value="Save" onclick="cmsApplyChanges();" />
									&nbsp;&nbsp;&nbsp;
									<input type="button" id="btnToolboxCancel2" value="Cancel" onclick="cmsCancelEdit();" />
								</div>
								<br style="clear: both;" />
							</div>
						</div>
					</asp:Panel>
				</div>
			</div>
			<div id="cmsToolboxSpacer" style="display: none;">
			</div>
			<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px; height: 20px;">
			</div>
		</div>
	</div>
</div>
<div style="display: none" id="cmsGlossySeaGreenID">
	<div id="CMSmodalalert" title="CMS Alert" class="cmsGlossySeaGreen">
		<p id="CMSmodalalertmessage">
			&nbsp;</p>
	</div>
	<div id="CMSremoveconfirm" title="Remove Widget?" class="cmsGlossySeaGreen">
		<p id="CMSremoveconfirmmsg">
			Are you sure you want to remove this widget?</p>
	</div>
	<div id="CMSsavedconfirm" title="Page Saved!" class="cmsGlossySeaGreen">
		<p id="CMSsavedconfirmmsg">
			The page has been saved. Click OK to return to browse mode. Redirecting in <span id="cmsSaveCountdown">10</span> seconds...</p>
	</div>
	<div id="CMScancelconfirm" title="Quit Editor?" class="cmsGlossySeaGreen">
		<p id="CMScancelconfirmmsg">
			Are you sure you want to leave the editor? All changes will be lost!</p>
	</div>
</div>
<div style="width: 1024px; height: 2px; clear: both; left: 0; border: 0px dotted #c0c0c0; z-index: 5000;">
</div>
<div style="width: 2px; height: 1024px; clear: both; left: 0; border: 0px dotted #c0c0c0; z-index: 5000;">
</div>
<div style="clear: both;">
	&nbsp;</div>
<!-- CarrotCake CMS Editor Control END -->
