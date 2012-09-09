

var cmsIsPageLocked = true;

function cmsSetPageStatus(stat) {
	cmsIsPageLocked = stat;
}

var webSvc = "/Manage/CMS.asmx";
var thisPage = ""; // used in escaped fashion
var thisPageNav = "";  // used non-escaped (redirects)
var thisPageID = "";
var timeTick = "";


function cmsSetServiceParms(serviceURL, pagePath, pageID, timeTck) {
	webSvc = serviceURL;
	thisPageNav = pagePath;
	thisPage = cmsMakeStringSafe(pagePath);
	thisPageID = pageID;
	timeTick = timeTck;
}


var cmsConfirmLeavingPage = true;

$(window).bind('beforeunload', function () {
	//cmsConfirmLeavingPage = false;
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

}

var cmsToolbarMargin = 'L';
function cmsShiftPosition(p) {

	floatingArray[0].targetTop = 30;
	floatingArray[0].targetBottom = undefined;
	if (p == 'L') {
		cmsToolbarMargin = 'L';
		floatingArray[0].targetLeft = 30;
		floatingArray[0].targetRight = undefined;
	} else {
		cmsToolbarMargin = 'R';
		floatingArray[0].targetLeft = undefined;
		floatingArray[0].targetRight = 30;
	}

	floatingArray[0].centerX = undefined;
	floatingArray[0].centerY = undefined;

}

$(document).ready(function () {
	cmsResetToolbarScroll();
});

setTimeout("cmsResetToolbarScroll()", 500);

var cmsToolTabIdx = 0;
var cmsScrollPos = 0;

function cmsSetPrefs(tab, margin, scrollPos, opStat) {
	cmsToolTabIdx = tab;
	cmsToolbarMargin = margin;
	cmsScrollPos = scrollPos;
	cmsMnuVis = opStat;
}

function cmsResetToolbarScroll() {
	setTimeout("cmsShiftPosition('" + cmsToolbarMargin + "')", 100);

	if (!cmsMnuVis) {
		cmsMnuVis = true;
		cmsToggleMenu();
	}

	$(document).scrollTop(cmsScrollPos);
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


//=============================================


function cmsMakeStringSafe(val) {
	val = Base64.encode(val);
	return val;
}

function cmsEditHB() {

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

	var scrollTop = $(document).scrollTop();
	var tabID = $('#cmsJQTabedToolbox').tabs("option", "selected");

	var webMthd = webSvc + "/RecordEditorPosition";

	$.ajax({
		type: "POST",
		url: webMthd,
		data: "{'ToolbarState': '" + cmsMnuVis + "', 'ToolbarMargin': '" + cmsToolbarMargin + "', 'ToolbarScroll': '" + scrollTop + "', 'SelTabID': '" + tabID + "'}",
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

var cmsTemplatePreview = "";

function cmsSetPreviewFileName(tmplName) {
	cmsTemplatePreview = tmplName;
}

function cmsPreviewTemplate() {
	var tmpl = $(cmsTemplateDDL).val();

	tmpl = cmsMakeStringSafe(tmpl);

	cmsLaunchWindowOnly(cmsTemplatePreview + "?carrot_templatepreview=" + tmpl);

	var editFrame = $('#cmsModalFrame');
	var editIFrame = $('#cmsFrameEditor');

	$(editIFrame).attr('width', '910');
	$(editIFrame).attr('height', '480');

	var btnApply = ' <input type="button" id="btnApplyTemplate" value="Apply Template" onclick="cmsUpdateTemplate();" /> &nbsp;&nbsp;&nbsp; ';
	var btnClose = ' <input type="button" id="btnCloseTemplate" value="Close" onclick="cmsCloseModalWin();" /> &nbsp;&nbsp;&nbsp; ';

	$(editFrame).append('<div id="cmsGlossySeaGreenID" class="cmsRight5px"> ' + btnClose + btnApply + ' </div>');
	window.setTimeout("cmsLateBtnStyle();", 500);
}

function cmsLateBtnStyle() {
	$(".cmsGlossySeaGreen input:button, .cmsGlossySeaGreen input:submit").button();
	$("#cmsGlossySeaGreenID input:button, #cmsGlossySeaGreenID input:submit").button();
}

var cmsTemplateDDL = "";

function cmsSetTemplateDDL(ddlName) {
	cmsTemplateDDL = ddlName;
}

function cmsUpdateTemplate() {
	cmsSaveToolbarPosition();

	var tmpl = $(cmsTemplateDDL).val();

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
		//cmsAlertModal("Saved");
		cmsMakeOKToLeave();
		cmsNotifySaved()
		window.setTimeout("location.href = \'" + thisPageNav + "\'", 10000);
	} else {
		cmsAlertModal(data.d);
	}
}

function cmsNotifySaved() {
	$("#CMSsavedconfirm").dialog("destroy");

	$("#CMSsavedconfirm").dialog({
		open: function () {
			$(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
		},

		resizable: false,
		height: 250,
		width: 400,
		modal: true,
		buttons: {
			"OK": function () {
				cmsMakeOKToLeave();
				window.setTimeout("location.href = \'" + thisPageNav + "\'", 500);
				$(this).dialog("close");
			}
		}
	});

	cmsFixDialog('CMSsavedconfirmmsg');
}


function cmsCancelEdit0() {
	cmsMakeOKToLeave();
	window.setTimeout("location.href = \'" + thisPageNav + "\'", 1000);
}

function cmsRecordCancellation() {

	var webMthd = webSvc + "/CancelEditing";

	$.ajax({
		type: "POST",
		url: webMthd,
		data: "{'ThisPage': '" + thisPageID + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: cmsAjaxGeneralCallback,
		error: cmsAjaxFailed
	});
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
				cmsRecordCancellation();
				cmsMakeOKToLeave();
				window.setTimeout("location.href = \'" + thisPageNav + "\'", 800);
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
		width: 550,
		modal: true,
		buttons: {
			"OK": function () {
				$(this).dialog("close");
			}
		}
	});

	$("#CMSmodalalertmessage").html(request);

	cmsFixDialog('CMSmodalalertmessage');
}

//===========================
// do a lot of iFrame magic

function cmsShowWidgetList() {
	cmsLaunchWindow('/Manage/WidgetList.aspx?pageid=' + thisPageID + "&zone=cms-all-placeholder-zones");
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

function cmsShowAddChildPage() {
	//cmsAlertModal("cmsShowEditPageInfo");
	cmsLaunchWindow('/Manage/PageAddChild.aspx?pageid=' + thisPageID);
}

function cmsSortChildren() {
	//cmsAlertModal("cmsSortChildren");
	cmsLaunchWindowOnly('/Manage/PageChildSort.aspx?pageid=' + thisPageID);
}

function cmsShowEditWidgetForm(w, m) {
	//cmsAlertModal("cmsShowEditWidgetForm");
	cmsLaunchWindow('/Manage/ContentEdit.aspx?pageid=' + thisPageID + "&widgetid=" + w + "&mode=" + m);
}

function cmsShowEditContentForm(f, m) {
	//cmsAlertModal("cmsShowEditContentForm");
	cmsLaunchWindow('/Manage/ContentEdit.aspx?pageid=' + thisPageID + "&field=" + f + "&mode=" + m);
}

function cmsFixSpinner() {
	$(".blockMsg img").addClass('cmsImageSpinner');
	$(".blockMsg table").addClass('cmsImageSpinnerTbl');
}

var cmsHtmlSpinner = '<table width="100%" class="cmsImageSpinnerTbl" border="0"><tr><td align="center" id="cmsSpinnerZone"><img id="cmsImageSpinnerImage" class="cmsImageSpinner" border="0" src="/Manage/images/ani-smallbar.gif"/></td></tr></table>';

function CMSBusyShort() {

	$("#cmsDivActive").block({ message: cmsHtmlSpinner,
		css: { border: 'none', backgroundColor: 'transparent' },
		fadeOut: 750,
		timeout: 1000,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});
	cmsFixSpinner();
}

function CMSBusyLong() {

	$("#cmsDivActive").block({ message: cmsHtmlSpinner,
		css: { border: 'none', backgroundColor: 'transparent' },
		fadeOut: 4000,
		timeout: 6000,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});
	cmsFixSpinner();
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

		$('#cmsJQTabedToolbox').tabs('select', cmsToolTabIdx);

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
	$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv2"> <iframe scrolling="auto" id="cmsFrameEditor" frameborder="0" name="cmsFrameEditor" width="910" height="540" src="' + TheURL + '" /> </div>');

	$("#cmsAjaxMainDiv2").block({ message: '<table><tr><td><img class="cmsAjaxModalSpinner" src="/Manage/images/Ring-64px-A7B2A0.gif"/></td></tr></table>',
		css: { width: '900px', height: '540px' },
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

function cmsLaunchWindowOnly(theURL) {
	var TheURL = theURL;
	$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv2"> <iframe scrolling="auto" id="cmsFrameEditor" frameborder="0" name="cmsFrameEditor" width="910" height="540" src="' + TheURL + '" /> </div>');

	$("#cmsAjaxMainDiv2").block({ message: '<table><tr><td><img class="cmsAjaxModalSpinner" src="/Manage/images/Ring-64px-A7B2A0.gif"/></td></tr></table>',
		css: { width: '900px', height: '540px' },
		fadeOut: 1000,
		timeout: 1200,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});

	cmsSaveToolbarPosition();
	setTimeout("cmsLoadWindowOnly();", 800);
}

function cmsLoadWindowOnly() {
	cmsSaveToolbarPosition();

	$("#cms-basic-modal-content").modal({ onClose: function (dialog) {
		//$.modal.close(); // must call this!
		setTimeout("$.modal.close();", 800);
		$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv"></div>');
	}
	});

	$('#cms-basic-modal-content').modal();
	return false;
}


function cmsCloseModalWin() {
	cmsSaveToolbarPosition();
	setTimeout("$.modal.close();", 250);
	$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv"></div>');
}

function cmsDirtyPageRefresh() {
	cmsSaveToolbarPosition();
	cmsMakeOKToLeave();
	window.setTimeout('cmsMakeOKToLeave();', 500);
	window.setTimeout('cmsMakeOKToLeave();', 700);
	window.setTimeout("location.href = \'" + thisPageNav + "?carrotedit=true&carrottick=" + timeTick + "\'", 800);
}