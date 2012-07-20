<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdvancedEdit.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucAdvancedEdit" %>
<div style="clear: both;">
	&nbsp;</div>
<carrot:jquery runat="server" ID="jquery1" />
<script src="/Manage/glossyseagreen/js/jquery-ui-glossyseagreen.js" type="text/javascript"></script>
<link href="/Manage/glossyseagreen/css/jquery-ui-glossyseagreen-scoped.css" rel="stylesheet" type="text/css" />
<script src="/Manage/includes/base64.js" type="text/javascript"></script>
<script src="/Manage/includes/jquery.simplemodal.1.4.1.min.js" type="text/javascript"></script>
<script src="/Manage/Includes/jq-floating-1.6.js" type="text/javascript"></script>
<script src="/Manage/includes/jquery.blockUI.js" type="text/javascript"></script>
<link href="/Manage/includes/modal.css" rel="stylesheet" type="text/css" />
<link href="/Manage/includes/advanced-editor.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">


	var cmsMnuVis = true;
	function cmsToggleMenu() {
		var t = $('#cmsMainToolbox');
		var s = $('#cmsToolboxSpacer');
		var m = $('#cmsMnuToggle');
		//alert(m.attr('id'));
		if (cmsMnuVis) {
			m.attr('class', 'ui-icon ui-icon-plusthick cmsFloatRight');
			t.css('display', 'none');
			s.css('display', 'block');
			cmsMnuVis = false;
		} else {
			m.attr('class', 'ui-icon ui-icon-minusthick cmsFloatRight');
			t.css('display', 'block');
			s.css('display', 'none');
			cmsMnuVis = true;
		}

		//setTimeout("cmsSaveToolbarPosition();", 500);
	}

	var cmsToolbarPos = 'L';
	function cmsShiftPosition(p) {

		floatingArray[0].targetTop = 30;
		floatingArray[0].targetBottom = undefined;
		if (p == 'L') {
			cmsToolbarPos = 'L';
			floatingArray[0].targetLeft = 30;
			floatingArray[0].targetRight = undefined;
		} else {
			cmsToolbarPos = 'R';
			floatingArray[0].targetLeft = undefined;
			floatingArray[0].targetRight = 30;
		}

		floatingArray[0].centerX = undefined;
		floatingArray[0].centerY = undefined;

		//setTimeout("cmsSaveToolbarPosition();", 500);
	}

	$(document).ready(function() {
		cmsResetToolbarScroll();
	});

	setTimeout("cmsResetToolbarScroll()", 250);

	function cmsResetToolbarScroll() {
		setTimeout("cmsShiftPosition('<%=EditorPrefs.EditorMargin %>')", 100);

		if ('<%= EditorPrefs.EditorOpen %>' =='false') {
			cmsMnuVis = true;
			cmsToggleMenu();
		}

		$("html").scrollTop(<%= EditorPrefs.EditorScrollPosition %>);
	}


	function cmsMenuFixImages() {
		$(".cmsWidgetBarIconCog").each(function (i) {
			cmsFixGeneralImage(this, ' ', 'Manage', 'cog.png');
		});
		$(".cmsWidgetBarIconCross").each(function (i) {
			cmsFixGeneralImage(this, 'Remove', 'Remove', 'cross.png');
		});
		$(".cmsWidgetBarIconPencil").each(function (i) {
			cmsFixGeneralImage(this, 'Edit', 'Edit', 'pencil.png');
		});
		$(".cmsWidgetBarIconPencil2").each(function (i) {
			cmsFixGeneralImage(this, ' ', 'Edit', 'pencil.png');
		});
		$(".cmsWidgetBarIconWidget").each(function (i) {
			cmsFixGeneralImage(this, 'Widgets', 'Widgets', 'application_view_tile.png');
		});
		$(".cmsWidgetBarIconWidget2").each(function (i) {
			cmsFixGeneralImage(this, 'History', 'History', 'layout.png');
		});
		
	}


	function cmsFixGeneralImage(elm, txt1, txt2, img) {
		var id = $(elm).attr('id');
		$(elm).html(txt1 + " <img class='cmsWidgetBarImgReset' border='0' src='/manage/images/" + img + "' alt='" + txt2 + "' title='" + txt2 + "' />");
	}

	$(document).ready(function () {
		setTimeout("cmsMenuFixImages();", 250);
		setTimeout("cmsMenuFixImages();", 2500);
		setTimeout("cmsMenuFixImages();", 5000);
	});

</script>
<script type="text/javascript">
	$(document).ready(function() {
		<% if (!bLocked) { %>
		setTimeout("cmsEditHB();", 1000);
		<%} else { %>
		cmsAlertModal("<%=litUser.Text %>");
		<%} %>
	});
</script>
<script type="text/javascript">
	var webSvc = "/Manage/CMS.asmx";

	var thisPage = '<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>';

	var thisPageID = '<%=guidContentID.ToString() %>';

	function cmsEscapeFile() {
		thisPage = cmsMakeStringSafe(thisPage);
	}

	$(document).ready(function () {
		cmsEscapeFile();
	});

	function cmsMakeStringSafe(val) {
		val = Base64.encode(val);
		return val;
	}

	var cmsConfirmLeavingPage = true;

	$(window).bind('beforeunload', function () {
		if (cmsConfirmLeavingPage) {
			return '>>>>>Are you sure you want to navigate away<<<<<<<<';
		}
	});

	function cmsMakeOKToLeave() {
		cmsConfirmLeavingPage = false;
	}


	function cmsEditHB() {
		//cmsSaveToolbarPosition();

		setTimeout("cmsEditHB();", 45 * 1000);

		var webMthd = webSvc + "/RecordHeartbeat";

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'PageID': '<%=guidContentID %>'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsUpdateHeartbeat,
			error: cmsAjaxFailed
		});
	}

	function cmsSaveToolbarPosition() {

		var scrollTop = $("html").scrollTop();

		var webMthd = webSvc + "/RecordEditorPosition";

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ToolbarState': '" + cmsMnuVis + "', 'ToolbarMargin': '" + cmsToolbarPos + "', 'ToolbarScroll': '" + scrollTop + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsAjaxGeneralCallback,
			error: cmsAjaxFailed
		});
	}

	function cmsSaveContent(val, zone) {
		cmsSaveToolbarPosition();

		var webMthd = webSvc + "/CacheContentZoneText";

		val = cmsMakeStringSafe(val);
		zone = cmsMakeStringSafe(zone);

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ZoneText': '" + val + "', 'Zone': '" + zone + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsSaveContentCallback,
			error: cmsAjaxFailed
		});
	}


	function cmsSaveGenericContent(val, key) {
		cmsSaveToolbarPosition();

		var webMthd = webSvc + "/CacheGenericContent";

		val = cmsMakeStringSafe(val);

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ZoneText': '" + val + "', 'DBKey': '" + key + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsSaveContentCallback,
			error: cmsAjaxFailed
		});
	}


	function cmsUpdateTemplate() {
		cmsSaveToolbarPosition();

		var tmpl = $("#<%=ddlTemplate.ClientID%>").val();

		var webMthd = webSvc + "/UpdatePageTemplate";

		tmpl = cmsMakeStringSafe(tmpl);

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'TheTemplate': '" + tmpl + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsSaveContentCallback,
			error: cmsAjaxFailed
		});
	}


	function cmsUpdateWidgets() {
		cmsSaveToolbarPosition();

		var webMthd = webSvc + "/CacheWidgetUpdate";

		var val = $("#cmsFullOrder").val();

		val = cmsMakeStringSafe(val);

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'WidgetAddition': '" + val + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsSaveWidgetsCallback,
			error: cmsAjaxFailed
		});
	}

	function cmsRemoveWidget(key) {
		cmsSaveToolbarPosition();

		var webMthd = webSvc + "/RemoveWidget";

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'DBKey': '" + key + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsSaveWidgetsCallback,
			error: cmsAjaxFailed
		});
	}

	function cmsMoveWidgetZone(zone, val) {
		cmsSaveToolbarPosition();

		var webMthd = webSvc + "/MoveWidgetToNewZone";

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'WidgetTarget': '" + zone + "', 'WidgetDropped': '" + val + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsSaveWidgetsCallback,
			error: cmsAjaxFailed
		});
	}

	function cmsApplyChanges() {

		var webMthd = webSvc + "/PublishChanges";

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsSavePageCallback,
			error: cmsAjaxFailed
		});

	}

	function cmsUpdateHeartbeat(data, status) {
		var hb = $('#cmsHeartBeat');
		hb.empty().append('HB:  ');
		hb.append(data.d);
		//CMSBusyShort();
	}

	function cmsSaveContentCallback(data, status) {

		if (data.d == "OK") {
			CMSBusyShort();
			cmsDirtyPageRefresh();
		} else {
			cmsAlertModal(data.d);
		}
	}

	function cmsAjaxGeneralCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
		} else {
			cmsAlertModal(data.d);
		}
	}

	function cmsSaveWidgetsCallback(data, status) {

		if (data.d == "OK") {
			CMSBusyShort();
			cmsDirtyPageRefresh();
		} else {
			cmsAlertModal(data.d);
		}
	}

	function cmsSavePageCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
			cmsAlertModal("Saved");
			cmsMakeOKToLeave();
			window.setTimeout('location.href = \'<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>\'', 5000);
		} else {
			cmsAlertModal(data.d);
		}
	}


	function cmsCancelEdit0() {
		cmsMakeOKToLeave();
		window.setTimeout('location.href = \'<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>\'', 1000);
	}

	function cmsCancelEdit() {
		$("#CMScancelconfirm").dialog("destroy");

		$("#CMScancelconfirm").dialog({
			open: function () {
				$(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
			},

			resizable: false,
			height: 250,
			width: 400,
			modal: true,
			buttons: {
				"No": function () {
					$(this).dialog("close");
				},
				"Yes": function () {
					cmsMakeOKToLeave();
					window.setTimeout('location.href = \'<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>\'', 500);
					$(this).dialog("close");
				}
			}
		});

		cmsFixDialog('CMScancelconfirmmsg');
	}


	function cmsAjaxFailed(request) {
		var s = "";
		s = s + "<b>status: </b>" + request.status + '<br />\r\n';
		s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
		s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
		cmsAlertModal(s);
	}

	function cmsAlertModal(request) {
		$("#CMSmodalalert").dialog("destroy");

		$("#CMSmodalalert").dialog({
			//autoOpen: false,
			height: 400,
			width: 600,
			modal: true
		});

		$("#CMSmodalalertmessage").html(request);


		cmsFixDialog('CMSmodalalertmessage');
	}

	function cmsManageWidgetList(zoneName) {
		//alert(zoneName);
		cmsLaunchWindow('/Manage/WidgetList.aspx?pageid=' + thisPageID + "&zone=" + zoneName);
	}
	function cmsManageWidgetHistory(widgetID) {
		//alert(widgetID);
		cmsLaunchWindow('/Manage/WidgetHistory.aspx?pageid=' + thisPageID + "&widgetid=" + widgetID);
	}

	function cmsShowEditPageInfo() {
		//cmsAlertModal("cmsShowEditPageInfo");
		cmsLaunchWindow('/Manage/PageEdit.aspx?pageid=' + thisPageID);
	}

	function cmsShowEditWidgetForm(w, m) {
		//cmsAlertModal("cmsShowEditWidgetForm");
		cmsLaunchWindow('/Manage/ContentEdit.aspx?pageid=' + thisPageID + "&widgetid=" + w + "&mode=" + m);
	}

	function cmsShowEditContentForm(f, m) {
		//cmsAlertModal("cmsShowEditContentForm");
		cmsLaunchWindow('/Manage/ContentEdit.aspx?pageid=' + thisPageID + "&field=" + f + "&mode=" + m);
	}

	function CMSBusyShort() {

		$("#divCMSActive").block({ message: '<table width="100%" border="0"><tr><td align="center"><img class="cmsImageSpinner" border="0" src="/Manage/images/ani-smallbar.gif"/></td></tr></table>',
			css: { border: 'none', backgroundColor: 'transparent' },
			fadeOut: 750,
			timeout: 1000,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}

	function CMSBusyLong() {

		$("#divCMSActive").block({ message: '<table width="100%" border="0"><tr><td align="center"><img class="cmsImageSpinner" border="0" src="/Manage/images/ani-smallbar.gif"/></td></tr></table>',
			css: { border: 'none', backgroundColor: 'transparent' },
			fadeOut: 4000,
			timeout: 5000,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}

	$(document).ready(function () {
		$(".cmsGlossySeaGreen input:button, .cmsGlossySeaGreen input:submit").button();
	});

	function cmsBuildOrderAndUpdateWidgets() {
		var ret = cmsBuildOrder();
		if (ret) {
			cmsUpdateWidgets();
		}
	}

	function cmsBuildOrder() {
		CMSBusyShort();

		$("#cmsFullOrder").val('');
		$(".cmsTargetArea").find('#cmsCtrlOrder').each(function (i) {
			var txt = $(this);
			$(txt).val('');
			var p = txt.parent().parent().parent().attr('id');
			var key = txt.parent().find('#cmsCtrlID').val();
			txt.val(i + '\t' + p + '\t' + key);
			//alert($('#'+p).find('#cmsCtrlID').val());

			$("#cmsFullOrder").val($("#cmsFullOrder").val() + '\r\n ' + txt.val());

		});

		return true;
	}

	$(document).ready(function () {
		setTimeout("cmsBuildOrder();", 250);
	});

	function cmsRemoveWidgetLink(v) {
		$("#CMSremoveconfirm").dialog("destroy");

		$("#CMSremoveconfirm").dialog({
			open: function () {
				$(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
			},

			resizable: false,
			height: 250,
			width: 400,
			modal: true,
			buttons: {
				"No": function () {
					$(this).dialog("close");
				},
				"Yes": function () {
					cmsRemoveWidget(v);
					$(this).dialog("close");
				}
			}
		});

		cmsFixDialog('CMSremoveconfirmmsg');

	}


	function cmsRemoveItem(a) {
		var tgt = $(a);
		//alert(tgt.val());
		if (tgt.is("a.ui-icon-trash")) {
			var p = $($(tgt).parent().parent().parent().parent());
			//alert(p.attr('id'));
			var txt = p.find('#cmsCtrlOrder');
			txt.val('-1');
			cmsDelItem(p);
		}
		return false;
	}


	function cmsSetOrder(fld) {
		var id = $(fld).attr('id');
		$("#cmsMovedItem").val(id);
	}


	$(document).ready(function () {

		$('#cmsAdminToolbox').addFloating({
			targetLeft: 25,
			targetTop: 25,
			snap: true
		});

		$(".cmsGlossySeaGreen input:button, .cmsGlossySeaGreen input:submit").button();

		/*
		$(".cmsTargetArea").sortable({
		revert: true,
		dropOnEmpty: true,
		placeholder: "cmsHighlightPH cmsGlossySeaGreen ui-state-highlight ui-corner-all",
		hoverClass: "cmsHighlightPH cmsGlossySeaGreen ui-state-highlight ui-corner-all"
		});
		*/

		$("#cmsToolBox div.cmsToolItem").draggable({
			connectToSortable: ".cmsTargetArea",
			helper: "clone",
			revert: "invalid",
			handle: "p.cmsToolItem",
			placeholder: "cmsHighlightPH cmsGlossySeaGreen ui-state-highlight ui-corner-all"
		});

		$(".cmsTargetMove").sortable({
			connectWith: ".cmsTargetMove",
			revert: true,
			dropOnEmpty: true,
			placeholder: "cmsHighlightPH cmsGlossySeaGreen ui-state-highlight ui-corner-all",
			hoverClass: "cmsHighlightPH cmsGlossySeaGreen ui-state-highlight ui-corner-all"
		}).disableSelection();


		// make sure sort only fires once
		var cmsSortWidgets = false;

		$(".cmsTargetArea").bind("sortupdate", function (event, ui) {
			if (!cmsSortWidgets) {
				var id = $(ui.item).attr('id');
				var val = $(ui.item).find('#cmsCtrlID').val();
				$("#cmsMovedItem").val(val);
				setTimeout("cmsBuildOrderAndUpdateWidgets();", 500);
				cmsSortWidgets = true;
			}
		});

		$("div#cmsToolBox").disableSelection();
		$("#cmsContentArea a").enableSelection();
		$("#cmsWidgetHead a").enableSelection();

	});

	function cmsSetValue(u) {
		var id = $(u).attr('id');
		var val = $(u).find('#cmsCtrlOrder').val();
		//alert(val);
		$("#cmsMovedItem").val(val);
		setTimeout("cmsBuildOrder();", 500);
	}


	function cmsDelItem(item) {
		item.appendTo("#cmsTrashList");
		setTimeout("cmsBuildOrderAndUpdateWidgets();", 500);
	}


	function cmsFixDialog(dialogname) {

		var fontfacereset = ' font-family: Lucida Grande, Lucida Sans, Arial, sans-serif !important; font-size: 12px !important; line-height: 16px !important; '

		var dilg = $("#" + dialogname).parent().parent();
		var txt = $("#" + dialogname).attr('style', 'margin: 2px !important; padding: 5px !important; text-align: left !important;' + fontfacereset);

		var c = $(dilg).attr('class');
		$(dilg).attr('class', "cmsGlossySeaGreen ui-widget-content ui-corner-all " + c);

		var pstyle = $(dilg).attr('style');
		$(dilg).attr('style', "padding:2px !important; margin:2px !important; background: #ffffff !important; border: 1px solid #c0c0c0 !important;" + fontfacereset + pstyle);

		var cap = $(dilg).find(".ui-dialog-titlebar-close").attr('style', 'float:right !important; padding:4px !important; background-color:transparent !important;');
		var cap = $(dilg).find(".ui-dialog-titlebar").attr('style', 'margin: 2px !important; padding: 2px !important; height: 32px !important;');
		var cap = $(dilg).find(".ui-button").attr('style', 'float:right !important;');
		var cap = $(dilg).find(".ui-dialog-title").attr('style', 'float:left !important; margin:2px !important; padding:2px !important;');
		var cap = $(dilg).find(".ui-dialog-buttonpane").attr('style', 'margin:2px !important; padding:3px !important;');

		//var h = $(dilg).html();
		//$(dilg).html('');
		//$(dilg).html('<div class="cmsGlossySeaGreen ui-widget-content ui-corner-all">' + h + '</div>');
	}
	
</script>
<script type="text/javascript">

	function cmsGenericEdit(PageId, WidgetId) {
		cmsLaunchWindow('/Manage/ControlPropertiesEdit.aspx?pageid=' + PageId + '&id=' + WidgetId);
	}

	function cmsLaunchWindow(theURL) {
		TheURL = theURL;
		$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv2"> <iframe scrolling="auto" id="cmsFrameEditor" frameborder="0" name="cmsFrameEditor" width="750" height="475" src="' + TheURL + '" /> </div>');

		$("#cmsAjaxMainDiv2").block({ message: '<table><tr><td><img class="cmsAjaxModalSpinner" src="/Manage/images/Ring-64px-A7B2A0.gif"/></td></tr></table>',
			css: { width: '750px', height: '475px' },
			fadeOut: 1000,
			timeout: 1200,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});

		cmsSaveToolbarPosition();
		setTimeout("cmsLoadWindow();", 800);
	}

	function cmsLoadWindow() {
		cmsSaveToolbarPosition();

		$("#cms-basic-modal-content").modal({ onClose: function (dialog) {
			//$.modal.close(); // must call this!
			setTimeout("$.modal.close();", 800);
			$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv"></div>');
			cmsDirtyPageRefresh();
		}
		});

		$('#cms-basic-modal-content').modal();
		return false;
	}

	function cmsDirtyPageRefresh() {
		cmsSaveToolbarPosition();
		cmsMakeOKToLeave();
		window.setTimeout('location.href = \'<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>?carrotedit=true&carrottick=<%=DateTime.Now.Ticks.ToString() %>\'', 800);
	}

</script>
<div style="display: none;">
	<img src="/manage/images/cog.png" alt="" />
	<img src="/manage/images/cross.png" alt="" />
	<img src="/manage/images/pencil.png" alt="" />
	<img src="/manage/images/application_view_tile.png" alt="" />
	<img src="/manage/images/layout.png" alt="" />
</div>
<div id="cmsAdminToolbox" class="cmsGlossySeaGreen ui-widget-content ui-corner-all cmsToolbox2">
	<div class="cmsInsideArea">
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
		<div id="divCMSActive">
			<div class="ui-widget" runat="server" id="divEditing">
				<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px;">
					<p>
						<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
						<asp:Literal ID="litUser" runat="server">&nbsp</asp:Literal></p>
				</div>
			</div>
			<div id="cmsMainToolbox">
				<asp:Panel ID="pnlBUttonGroup" runat="server">
					<div style="display: none;" class="cmsGlossySeaGreen">
						cmsFullOrder<br />
						<textarea rows="5" cols="30" id="cmsFullOrder" style="width: 310px; height: 50px;"></textarea><br />
						cmsMovedItem<br />
						<input type="text" id="cmsMovedItem" style="width: 310px;" /><br />
					</div>
					<div class="cmsGlossySeaGreen cmsCenter5px">
						<input runat="server" id="btnEditCoreInfo" type="button" value="Edit Core Page Info" class="cmsCenter5px" onclick="cmsShowEditPageInfo();" />
						<br />
						<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
						</asp:DropDownList>
						<br />
						<input runat="server" id="btnTemplate" type="button" value="Apply" class="cmsCenter5px" onclick="cmsUpdateTemplate();" />
					</div>
					<div class="cmsGlossySeaGreen cmsCenter5px">
						<input runat="server" id="btnToolboxSave" type="button" value="Save" class="cmsCenter5px" onclick="cmsApplyChanges();" />
						&nbsp;&nbsp;&nbsp;
						<input type="button" value="Cancel" class="cmsCenter5px" onclick="cmsCancelEdit();" />
					</div>
				</asp:Panel>
				<br style="clear: none;" />
				<asp:Repeater ID="rpTools" runat="server">
					<HeaderTemplate>
						<div id="cmsToolBox" class="cmsGlossySeaGreen ui-widget-content ui-corner-all" style="overflow: auto; height: 290px; width: 250px;
							padding: 5px; margin: 5px; float: left; border: solid 1px #000;">
					</HeaderTemplate>
					<ItemTemplate>
						<div class="cmsToolItem cmsGlossySeaGreen ui-widget-content ui-corner-all" id="cmsToolItemDiv">
							<div id="cmsControl" class="cmsGlossySeaGreen" style="min-height: 75px; min-width: 125px; padding: 2px; margin: 2px;">
								<p class="cmsToolItem cmsGlossySeaGreen ui-widget-header ui-corner-all" style="cursor: move; clear: both; padding: 2px; margin: 2px;">
									<%# Eval("Caption")%>
								</p>
								<%# String.Format("{0}", Eval("FilePath")).Replace(".",". ") %><br />
								<input type="hidden" id="cmsCtrlID" value="<%# Eval("FilePath")%>" />
								<input type="hidden" id="cmsCtrlOrder" value="0" />
								<div style="text-align: right;" id="cmsCtrlButton">
								</div>
							</div>
						</div>
					</ItemTemplate>
					<FooterTemplate>
						</div></FooterTemplate>
				</asp:Repeater>
				<div id="cmsTrashList" style="clear: both; display: none; width: 300px; height: 100px; overflow: auto; float: left; border: solid 1px #ccc;
					background-color: #000;">
				</div>
			</div>
			<div id="cmsToolboxSpacer" style="display: none;">
			</div>
			<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px; height: 20px;">
			</div>
		</div>
	</div>
</div>
<div style="display: none" class="cmsGlossySeaGreen">
	<div id="CMSmodalalert" class="cmsGlossySeaGreen" title="CMS Alert">
		<p id="CMSmodalalertmessage">
			&nbsp;</p>
	</div>
	<div id="CMSremoveconfirm" class="cmsGlossySeaGreen" title="Remove Widget?">
		<p id="CMSremoveconfirmmsg">
			Are you sure you want to remove this widget?</p>
	</div>
	<div id="CMScancelconfirm" class="cmsGlossySeaGreen" title="Quit Editor?">
		<p id="CMScancelconfirmmsg">
			Are you sure you want to leave the editor? All changes will be lost!</p>
	</div>
</div>
<div style="display: none">
	<div id="cms-basic-modal-content">
		<div id="cmsModalFrame">
		</div>
	</div>
	<div style="display: none">
		<img src="/manage/images/x.png" alt="x" />
	</div>
</div>
