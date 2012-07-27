<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdvancedEdit.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucAdvancedEdit" %>
<div style="width: 1280px; height: 2px; clear: both; left: 0; border: 0px dotted #000000; z-index: 5000;">
</div>
<div style="width: 2px; height: 1024px; clear: both; left: 0; border: 0px dotted #000000; z-index: 5000;">
</div>
<div style="clear: both;">
	&nbsp;</div>
<carrot:jquery runat="server" ID="jquery1" />
<carrot:jqueryui runat="server" ID="jqueryui1" />
<link href="/Manage/glossyseagreen/css/jquery-ui-glossyseagreen-scoped2.css" rel="stylesheet" type="text/css" />
<link href="/Manage/glossyseagreen/css/jquery-ui-glossyseagreen-scoped.css" rel="stylesheet" type="text/css" />
<script src="/Manage/includes/base64.js" type="text/javascript"></script>
<script src="/Manage/includes/jquery.simplemodal.js" type="text/javascript"></script>
<script src="/Manage/Includes/floating-menu.js" type="text/javascript"></script>
<script src="/Manage/includes/jquery.blockUI.js" type="text/javascript"></script>
<link href="/Manage/includes/modal.css" rel="stylesheet" type="text/css" />
<link href="/Manage/includes/advanced-editor.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">

	$(document).ready(function() {
		<% if (!bLocked) { %>
		setTimeout('cmsEditHB();', 1000);
		<%} else { %>
		cmsAlertModal('<%=litUser.Text %>');
		<%} %>
	});

	$(document).ready(function () {
		$(".cmsGlossySeaGreen input:button, .cmsGlossySeaGreen input:submit").button();
		$("#cmsGlossySeaGreenID input:button, #cmsGlossySeaGreenID input:submit").button();
	});

</script>
<script type="text/javascript">

	var cmsIsPageLocked = true;
	if ('<%=bLocked.ToString().ToLower() %>' != 'true') {
		cmsIsPageLocked = false;
	}

	var cmsConfirmLeavingPage = true;

	$(window).bind('beforeunload', function () {
		cmsConfirmLeavingPage = false;
		if (!cmsIsPageLocked) {
			if (cmsConfirmLeavingPage) {
				return '>>Are you sure you want to navigate away<<';
			}
		}
	});

	function cmsGetPageStatus() {
		return cmsConfirmLeavingPage;
	}

	function cmsGetPageLock() {
		return cmsIsPageLocked;
	}

	// ===============================================
	// do menu stuff / icons etc

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

	$(document).ready(function () {
		cmsResetToolbarScroll();
	});

	setTimeout("cmsResetToolbarScroll()", 250);

	function cmsResetToolbarScroll() {
		setTimeout("cmsShiftPosition('<%=EditorPrefs.EditorMargin %>')", 100);

		if ('<%= EditorPrefs.EditorOpen %>' == 'false') {
			cmsMnuVis = true;
			cmsToggleMenu();
		}

		//$("html").scrollTop(parseInt('<%= EditorPrefs.EditorScrollPosition %>'));
		$(document).scrollTop(parseInt('<%= EditorPrefs.EditorScrollPosition %>'));

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
		//$(".cmsWidgetBarIconShrink").each(function (i) {
		//	cmsFixGeneralImage(this, 'Shrink', 'Shrink', 'arrow_in.png');
		//});
	}

	function cmsBlockImageEdits(elm) {
		$(elm).attr('style', 'display: none;');
	}

	function cmsBlockContentEdits(elm) {
		var txt = $(elm).text();
		$(elm).attr('style', 'display: none;');
		$(elm).parent().text(txt);
	}

	function cmsFixGeneralImage(elm, txt1, txt2, img) {
		var id = $(elm).attr('id');
		$(elm).html(txt1 + " <img class='cmsWidgetBarImgReset' border='0' src='/manage/images/" + img + "' alt='" + txt2 + "' title='" + txt2 + "' />");
	}


	$(document).ready(function () {
		if (!cmsIsPageLocked) {
			setTimeout("cmsMenuFixImages();", 250);
			setTimeout("cmsMenuFixImages();", 2500);
			setTimeout("cmsMenuFixImages();", 5000);

			// these click events because of stoopid IE9 navigate away behavior
			$('#cmsEditMenuList a').each(function (i) {
				$(this).click(function () {
					cmsMakeOKToLeave();
					setTimeout("cmsMakeNotOKToLeave();", 250);
				});
			});

			$('#cmsEditMenuList').each(function (i) {
				$(this).click(function () {
					cmsMakeOKToLeave();
					setTimeout("cmsMakeNotOKToLeave();", 250);
				});
			});

			$('.cmsContentContainerInnerTitle a').each(function (i) {
				$(this).click(function () {
					cmsMakeOKToLeave();
					setTimeout("cmsMakeNotOKToLeave();", 250);
				});
			});

			$('.cmsContentContainerInnerTitle').each(function (i) {
				$(this).click(function () {
					cmsMakeOKToLeave();
					setTimeout("cmsMakeNotOKToLeave();", 250);
				});
			});

		} else {
			$("#cmsEditMenuList ul").each(function (i) {
				cmsBlockImageEdits(this);
			});

			$(".cmsContentContainerTitle a").each(function (i) {
				cmsBlockContentEdits(this);
			});
		}
	});

	function cmsMakeOKToLeave() {
		cmsConfirmLeavingPage = false;
	}

	function cmsMakeNotOKToLeave() {
		cmsConfirmLeavingPage = true;
	}

	function cmsRequireConfirmToLeave(confirmLeave) {
		cmsConfirmLeavingPage = confirmLeave;
	}

	// ===============================================
	// do loads of web service stuff

	var webSvc = "/Manage/CMS.asmx";

	var thisPage = "<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>"; // used in escaped fashion
	var thisPageNav = "<%=Carrotware.CMS.Core.SiteData.CurrentScriptName %>";  // used non-escaped (redirects)

	var thisPageID = "<%=guidContentID.ToString() %>";

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

	function cmsEditHB() {
		//cmsSaveToolbarPosition();
		if (!cmsIsPageLocked) {
			setTimeout("cmsEditHB();", 45 * 1000);

			var webMthd = webSvc + "/RecordHeartbeat";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'PageID': '" + thisPageID + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: cmsUpdateHeartbeat,
				error: cmsAjaxFailed
			});
		}
	}

	function cmsSaveToolbarPosition() {

		var scrollTop = $(document).scrollTop();  //$("html").scrollTop();
		var tabID = $('#cmsJQTabedToolbox').tabs("option", "selected");

		var webMthd = webSvc + "/RecordEditorPosition";

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ToolbarState': '" + cmsMnuVis + "', 'ToolbarMargin': '" + cmsToolbarPos + "', 'ToolbarScroll': '" + scrollTop + "', 'SelTabID': '" + tabID + "'}",
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
			window.setTimeout("location.href = \'" + thisPageNav + "\'", 5000);
		} else {
			cmsAlertModal(data.d);
		}
	}


	function cmsCancelEdit0() {
		cmsMakeOKToLeave();
		window.setTimeout("location.href = \'" + thisPageNav + "\'", 1000);
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
					window.setTimeout("location.href = \'" + thisPageNav + "\'", 500);
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

	//===========================
	// do a lot of iFrame magic

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

	function cmsSortChildren() {
		//cmsAlertModal("cmsSortChildren");
		cmsLaunchWindow('/Manage/PageChildSort.aspx?pageid=' + thisPageID);
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

		$("#cmsDivActive").block({ message: '<table width="100%" border="0"><tr><td align="center"><img class="cmsImageSpinner" border="0" src="/Manage/images/ani-smallbar.gif"/></td></tr></table>',
			css: { border: 'none', backgroundColor: 'transparent' },
			fadeOut: 750,
			timeout: 1000,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}

	function CMSBusyLong() {

		$("#cmsDivActive").block({ message: '<table width="100%" border="0"><tr><td align="center"><img class="cmsImageSpinner" border="0" src="/Manage/images/ani-smallbar.gif"/></td></tr></table>',
			css: { border: 'none', backgroundColor: 'transparent' },
			fadeOut: 4000,
			timeout: 5000,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}

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

		// make sure sort only fires once
		var cmsSortWidgets = false;

		if (!cmsIsPageLocked) {

			$('#cmsJQTabedToolbox').tabs();

			$('#cmsJQTabedToolbox').tabs('select', parseInt('<%=EditorPrefs.EditorSelectedTabIdx %>'));

			$("#cmsToolBox div.cmsToolItem").draggable({
				connectToSortable: ".cmsTargetArea",
				helper: "clone",
				revert: "invalid",
				handle: "p.cmsToolItem",
				placeholder: "cmsHighlightPH"
			});


			$(".cmsWidgetTitleBar").mousedown(function () {
				var target = $(this).parent().parent().attr("id");
				cmsMoveWidgetResizer(target, 1);
			});

			$(".cmsWidgetTitleBar").mouseup(function () {
				var target = $(this).parent().parent().attr("id");
				cmsMoveWidgetResizer(target, 0);
			});

			$(".cmsTargetMove").sortable({
				stop: function (event, ui) {
					var id = $(ui.item).attr('id');
					cmsMoveWidgetResizer(id, 0);
				},
				connectWith: ".cmsTargetMove",
				revert: true,
				dropOnEmpty: true,
				distance: 25,
				placeholder: "cmsHighlightPH",
				hoverClass: "cmsHighlightPH"
			}).disableSelection();


			$(".cmsTargetArea").bind("sortupdate", function (event, ui) {
				if (!cmsSortWidgets) {
					var id = $(ui.item).attr('id');
					var val = $(ui.item).find('#cmsCtrlID').val();
					$("#cmsMovedItem").val(val);
					setTimeout("cmsBuildOrderAndUpdateWidgets();", 500);
					cmsSortWidgets = true;
				}
			});

		}

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

	function cmsShrinkWidgetHeight(key) {
		var item = $("#" + key);
		var id = $(item).attr('id');
		//alert(id);

		var zone = $(item).find('#cmsControl');
		var st = $(zone).attr('style');

		if (st == undefined || st.length < 10) {
			$(zone).attr('style', 'border: solid 0px #000000; padding: 1px; height: 100px; max-width: 800px; overflow: auto;');
		} else {
			$(zone).attr('style', '');
		}
	}


	function cmsMoveWidgetResizer(key, state) {
		var item = $("#" + key);
		var id = $(item).attr('id');
		var zone = $(item).find('#cmsControl');

		if (state != 0) {
			$(zone).attr('style', 'border: solid 0px #000000; padding: 1px; height: 75px; max-width: 800px; overflow: auto;');
		} else {
			$(zone).attr('style', '');
		}
	}


	function cmsFixDialog(dialogname) {
		var dilg = $("#" + dialogname).parent().parent();

		cmsOverrideCSSScope(dilg, "");

		var over = $(dilg).next(".ui-widget-overlay");
		$(over).wrap("<div class=\"cmsGlossySeaGreen\" />");

		var d = $(dilg);
		$(d).wrap("<div class=\"cmsGlossySeaGreen\" />");

	}

	function cmsOverrideCSSScope(elm, xtra) {
		var c = $(elm).attr('class');
		if (c.indexOf("cmsGlossySeaGreen") < 0 || c.indexOf(xtra) < 0) {
			$(elm).attr('class', "cmsGlossySeaGreen " + xtra + " " + c);
		}
	}

	function cmsGenericEdit(PageId, WidgetId) {
		cmsLaunchWindow('/Manage/ControlPropertiesEdit.aspx?pageid=' + PageId + '&id=' + WidgetId);
	}

	function cmsLaunchWindow(theURL) {
		var TheURL = theURL;
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
		window.setTimeout('cmsMakeOKToLeave();', 500);
		window.setTimeout('cmsMakeOKToLeave();', 700);
		window.setTimeout("location.href = \'" + thisPageNav + "?carrotedit=true&carrottick=<%=DateTime.Now.Ticks.ToString() %>\'", 800);
	}

</script>
<div style="display: none;">
	<img src="/manage/images/cog.png" alt="" />
	<img src="/manage/images/cross.png" alt="" />
	<img src="/manage/images/pencil.png" alt="" />
	<img src="/manage/images/application_view_tile.png" alt="" />
	<img src="/manage/images/layout.png" alt="" />
	<img src="/manage/images/arrow_in.png" alt="" />
</div>
<div id="cmsAdminToolbox" class="cmsToolbox2" style="padding: 2px; margin: 0px; min-height: 300px; width: 290px;">
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
						<asp:Literal ID="litUser" runat="server">&nbsp</asp:Literal></p>
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
							<div class=" cmsCenter5px">
								<input runat="server" id="btnToolboxSave" type="button" value="Save" class="cmsPlain5px" onclick="cmsApplyChanges();" />
								&nbsp;&nbsp;&nbsp;
								<input type="button" value="Cancel" class="cmsPlain5px" onclick="cmsCancelEdit();" />
							</div>
							<asp:Repeater ID="rpTools" runat="server">
								<HeaderTemplate>
									<div id="cmsToolBox" class="ui-widget-content ui-corner-all" style="overflow: auto; height: 290px; width: 250px; padding: 5px;
										margin: 5px; float: left; border: solid 1px #000;">
								</HeaderTemplate>
								<ItemTemplate>
									<div id="cmsToolItemDiv" class="cmsToolItem cmsToolItemWrapper">
										<div id="cmsControl" style="min-height: 75px; min-width: 125px; padding: 2px; margin: 2px;">
											<p class="cmsToolItem ui-widget-header">
												<%# Eval("Caption")%>
											</p>
											<%# String.Format("{0}", Eval("FilePath")).Replace(".",". ") %><br />
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
							<div class="cmsCenter5px">
								<p class="cmsLeft5px">
									<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
									</asp:DropDownList>
									<br />
									<input runat="server" id="btnTemplate" type="button" value="Apply Template" class="cmsPlain5px" onclick="cmsUpdateTemplate();" />
								</p>
								<p class="cmsLeft5px">
									<input runat="server" id="btnEditCoreInfo" type="button" value="Edit Core Page Info" class="cmsPlain5px" onclick="cmsShowEditPageInfo();" />
								</p>
								<p class="cmsLeft5px">
									<input runat="server" id="btnSortChildPages" type="button" value="Sort Child/Sub Pages" class="cmsPlain5px" onclick="cmsSortChildren();" />
								</p>
							</div>
							<br style="clear: both;" />
						</div>
					</div>
				</asp:Panel>
			</div>
		</div>
		<div id="cmsToolboxSpacer" style="display: none;">
		</div>
		<div id="cmsTrashList" style="clear: both; display: none; width: 300px; height: 100px; overflow: auto; float: left; border: solid 1px #ccc;
			background-color: #000;">
		</div>
		<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px; height: 20px;">
		</div>
	</div>
</div>
</div>
<div style="display: none">
	<div id="CMSmodalalert" title="CMS Alert">
		<p id="CMSmodalalertmessage">
			&nbsp;</p>
	</div>
	<div id="CMSremoveconfirm" title="Remove Widget?">
		<p id="CMSremoveconfirmmsg">
			Are you sure you want to remove this widget?</p>
	</div>
	<div id="CMScancelconfirm" title="Quit Editor?">
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
