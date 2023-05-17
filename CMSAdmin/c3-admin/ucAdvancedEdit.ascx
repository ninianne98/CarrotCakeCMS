<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdvancedEdit.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucAdvancedEdit" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<!-- CarrotCake CMS Editor Control BEGIN -->
<%--<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="NotUsed"   />--%>
<asp:PlaceHolder ID="plcIncludes" runat="server">
	<link href="/c3-admin/includes/advanced-editor-reset.css<%=AntiCache%>" rel="stylesheet" type="text/css" />
	<link href="/c3-admin/includes/modal.css<%=AntiCache%>" rel="stylesheet" type="text/css" />
	<script src="/c3-admin/includes/jquery.simplemodal.js<%=AntiCache%>" type="text/javascript"></script>
	<script src="/c3-admin/includes/jquery.blockUI.js<%=AntiCache%>" type="text/javascript"></script>
	<script src="/c3-admin/includes/base64.js<%=AntiCache%>" type="text/javascript"></script>
	<script src="/c3-admin/includes/advanced-editor.js<%=AntiCache%>" type="text/javascript"></script>
</asp:PlaceHolder>

<%--
<!-- jQuery CDN -->
<script>		setTimeout(function () { window.jQuery || document.write('<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"><\/script>'); }, 100);  </script>
<script>		setTimeout(function () { window.jQuery.ui || document.write('<script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js" type="text/javascript"><\/script>'); }, 200);  </script>
<script>	(!window.jQuery || (typeof jQuery == 'undefined')) || document.write('<script src="<% = Carrotware.Web.UI.Controls.jquery.GeneralUri %>" type="text/javascript"><\/script>'); </script>
<script>	window.jQuery.ui || document.write('<script src="<% = Carrotware.Web.UI.Controls.jqueryui.GeneralUri %>" type="text/javascript"><\/script>'); </script>
--%>

<script type="text/javascript">
	var cmsJQLoadCtr = 1;
	function cmsLoadJQDyn() {
		var jq2URL = '<%= Carrotware.Web.UI.Controls.jquery.GeneralUri %>';
		var jq1URL = '<%= Carrotware.Web.UI.Controls.jqueryui.GeneralUri %>';

		if (cmsJQLoadCtr <= 30) {
			cmsJQLoadCtr++;
			cmsSetJQueryURL(jq2URL, jq1URL);

			if ((typeof jQuery.ui == 'undefined') || (typeof jQuery == 'undefined')) {
				setTimeout("cmsLoadJQDyn();", 500);
			}
		}
	}

	cmsLoadJQDyn();
</script>
<script type="text/javascript">
	var cmsPageInit2 = false;
	var cmsPageLocked = <%=bLocked.ToString().ToLowerInvariant() %>;

	if (!window.jQuery || (typeof jQuery == 'undefined')) {
		setTimeout("cmsToolbarPageInit2();", 1000);
		setTimeout("cmsToolbarPageInit2();", 3000);
		setTimeout("cmsToolbarPageInit2();", 5000);
	}

	$(document).ready(function () {
		cmsToolbarPageInit2();
	});

	function cmsToolbarPageInit2() {
		if (!cmsPageInit2) {
			cmsSetPageStatus(cmsPageLocked);

			if(!cmsPageLocked){
				setTimeout('cmsEditHB();', 1000);
			} else{
				setTimeout(function () { cmsAlertModal("The content is already being edited by '<%= EditUserName %>' "); }, 250);
			}

			cmsPageInit2 = true;
		}

		$(".cms-seagreen input:button, .cms-seagreen input:submit, .cms-seagreen input:reset").button();
		$("#cms-seagreen-id input:button, #cms-seagreen-id input:submit, #cms-seagreen-id input:reset").button();
	}

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
		var cmsScrollWPos = parseInt('<%= EditorPrefs.EditorWidgetScrollPosition %>');

		var cmsTimeTick = "<%=DateTime.Now.Ticks.ToString() %>";
		var cmsOpenStat = true;

		cmsSetPageStatus(cmsPageLocked);

		if ('<%= EditorPrefs.EditorOpen %>' == 'false') {
			cmsOpenStat = false;
		}

		cmsSetPrefs(cmsTabIdx, cmsMargin, cmsScrollPos, cmsScrollWPos, cmsOpenStat);

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
<div class="cmsMainAdvControls">
	<div id="cms-seagreen-id" class="cms-seagreen">
		<div style="display: none;">
			<img src="/c3-admin/images/cog.png" alt="" />
			<img src="/c3-admin/images/cross.png" alt="" />
			<img src="/c3-admin/images/pencil.png" alt="" />
			<img src="/c3-admin/images/application_view_tile.png" alt="" />
			<img src="/c3-admin/images/layout.png" alt="" />
			<img src="/c3-admin/images/arrow_in.png" alt="" />
			<img src="/c3-admin/images/dragbg.png" alt="" />
			<img src="/c3-admin/images/x.png" alt="x" />
			<img src="/c3-admin/images/mini-spinner3-6F997D.gif" alt="spinner" />
			<img src="/c3-admin/images/Ring-64px-A7B2A0.gif" alt="ring" />
			<img src="/c3-admin/images/ani-smallbar.gif" alt="bar" />
		</div>
		<div style="display: none">
			<div id="cms-basic-modal-content" class="cms-seagreen">
				<div id="cmsModalFrame">
				</div>
			</div>
		</div>
		<div id="cmsToolBoxWrap" class="<%=string.Format("cmsToolbarAlignment{0}", EditorPrefs.EditorMargin) %>">
			<div id="cmsAdminToolbox" class="cms-seagreen cmsToolbox2 cmsToolbox3">
				<div class="cmsInsideArea">
					<div class="ui-widget-header ui-corner-all cmsToolboxHead">
						<div class="cmsFloatLeft">
							<p class="cmsToolboxHeadCaption">
								CarrotCake CMS
							</p>
						</div>
						<div class='cmsFloatRight'>
							<div onclick="cmsShiftPosition('L')" id="cmsMnuLeft" class="ui-icon ui-icon-circle-triangle-w cmsFloatLeft" title="L">
								L
							</div>
							<div onclick="cmsShiftPosition('R')" id="cmsMnuRight" class="ui-icon ui-icon-circle-triangle-e cmsFloatLeft" title="R">
								R
							</div>
							<div onclick="cmsToggleMenu();" id="cmsMnuToggle" class="ui-icon ui-icon-minusthick cmsFloatLeft" title="toggle">
								T
							</div>
						</div>
						<div style="clear: both;">
						</div>
					</div>
					<div id="cmsDivActive" class="cms-seagreen">
						<div class="ui-widget" runat="server" id="cmsDivEditing">
							<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px;">
								<p>
									<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
									<asp:Literal ID="litUser" runat="server">&nbsp;</asp:Literal>
								</p>
							</div>
						</div>
						<div id="cmsMainToolbox" class="cms-seagreen">
							<asp:Panel ID="pnlCMSEditZone" runat="server">
								<div id="cmsTabbedToolbox">
									<ul>
										<li><a href="#cmsTabIdx-tabs-1">Widgets</a></li>
										<li><a href="#cmsTabIdx-tabs-2">Templates</a></li>
										<li><a href="#cmsTabIdx-tabs-3">Page Info</a></li>
									</ul>
									<div id="cmsTabIdx-tabs-1" class="cmsToolboxTab">
										<% if (!bLocked) { %>
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
												<div id="cmsToolBox" class="ui-widget-content ui-corner-all">
											</HeaderTemplate>
											<ItemTemplate>
												<div id="cmsToolItemDiv" class="cmsToolItem cmsToolItemWrapper">
													<div class="cmsWidgetControlItem cmsWidgetToolboxItem cmsWidgetCtrlPath" id="cmsControl">
														<p class="cmsToolItem ui-widget-header">
															<%# Eval("Caption")%>
														</p>
														<p class="cmsWidgetToolboxPath">
															<%# string.Format("{0}", Eval("FilePath")).Replace(".", ". ").Replace("/", "/ ")%><br />
														</p>
														<input type="hidden" id="cmsCtrlID" value="<%# Eval("FilePath")%>" />
														<input type="hidden" id="cmsCtrlOrder" value="0" />
													</div>
												</div>
											</ItemTemplate>
											<FooterTemplate>
												</div>
											</FooterTemplate>
										</asp:Repeater>
										<%} else { %>
										<div class="cmsCenter5px">
											<input type="button" id="Button2" value="Cancel" onclick="cmsCancelEdit();" />
										</div>
										<%} %>
										<div style="clear: both;">
										</div>
									</div>
									<div id="cmsTabIdx-tabs-2" class="cmsToolboxTab">
										<% if (!bLocked) { %>
										<div class="cmsLeft5px">
											<p>
												Templates / Skins<br />
												<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server" CssClass="cmsAdvEditTemplatePicker">
												</asp:DropDownList>
											</p>
											<p class="cmsCenter5px">
											</p>
											<p class="cmsCenter5px">
												<input runat="server" id="btnTemplate" type="button" value="Apply" onclick="cmsUpdateTemplate();" />
												&nbsp;&nbsp;&nbsp;
												<input runat="server" id="btnPreview" type="button" value="Preview" onclick="cmsPreviewTemplate();" />
												<br />
											</p>
											<p class="cmsCenter5px">
											</p>
											<div class="cmsCenter5px">
												<input type="button" runat="server" id="btnToolboxSave2" value="Save" onclick="cmsApplyChanges();" />
												&nbsp;&nbsp;&nbsp;
												<input type="button" id="btnToolboxCancel2" value="Cancel" onclick="cmsCancelEdit();" />
											</div>
										</div>
										<%} %>
										<div style="clear: both;">
										</div>
									</div>
									<div id="cmsTabIdx-tabs-3" class="cmsToolboxTab">
										<% if (!bLocked) { %>
										<div class="cmsLeft5px">
											<p>
												<input runat="server" id="btnEditCoreInfo" type="button" value="Edit Core Page Info" />
											</p>
											<p>
												<input runat="server" id="btnAddPage" type="button" value="Add New Page" onclick="cmsShowAddPage();" />
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
											<input type="button" runat="server" id="btnToolboxSave3" value="Save" onclick="cmsApplyChanges();" />
											&nbsp;&nbsp;&nbsp;
											<input type="button" id="btnToolboxCancel3" value="Cancel" onclick="cmsCancelEdit();" />
										</div>
										<%} %>
										<div style="clear: both;">
										</div>
									</div>
								</div>
							</asp:Panel>
						</div>
					</div>
					<div style="height: 2px; width: 100px; clear: both;">
					</div>
					<div class="cmsVersion">
						<%=  SiteData.CarrotCakeCMSVersion%>
					</div>
					<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px; height: 20px;">
					</div>
				</div>
			</div>
		</div>
	</div>
	<div style="display: none" class="cms-seagreen">
		<div id="CMSmodalalert" title="CarrotCake CMS" class="cms-seagreen">
			<p id="CMSmodalalertmessage">
				&nbsp;
			</p>
		</div>
		<div id="CMSremoveconfirm" title="Remove Widget?" class="cms-seagreen">
			<p id="CMSremoveconfirmmsg">
				Are you sure you want to remove this widget?
			</p>
		</div>
		<div id="CMSaddconfirm" title="Add Widget?" class="cms-seagreen">
			<p id="CMSaddconfirmmsg">
				Would you like to add "<b class="cms-widget-name">W</b>" to placeholder "<b class="cms-widget-target">T</b>"?
			</p>
		</div>
		<div id="CMSsavedconfirm" title="Page Saved!" class="cms-seagreen">
			<p id="CMSsavedconfirmmsg">
				The page has been saved. Click OK to return to browse mode. Redirecting in <span id="cmsSaveCountdown">10</span> seconds...
			</p>
		</div>
		<div id="CMScancelconfirm" title="Quit Editor?" class="cms-seagreen">
			<p id="CMScancelconfirmmsg">
				Are you sure you want to leave the editor? All changes will be lost!
			</p>
		</div>
	</div>
	<div style="width: 1024px; height: 2px; clear: both; left: 0; border: 0px dotted #c0c0c0; z-index: 5000;">
	</div>
	<div style="width: 2px; height: 1024px; clear: both; left: 0; border: 0px dotted #c0c0c0; z-index: 5000;">
	</div>
	<div style="clear: both;">
		&nbsp;
	</div>
</div>
<!-- CarrotCake CMS Editor Control END -->
