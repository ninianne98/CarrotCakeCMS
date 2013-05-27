
var cmsIsPageLocked = true;

function cmsSetPageStatus(stat) {
	cmsIsPageLocked = stat;
}

var webSvc = "/c3-admin/CMS.asmx";
var thisPage = ""; // used in escaped fashion
var thisPageNav = "";  // used non-escaped (redirects)
var thisPageNavSaved = "";  // used non-escaped (redirects)
var thisPageID = "";
var timeTick = "";


function cmsSetServiceParms(serviceURL, pagePath, pageID, timeTck) {
	webSvc = serviceURL;
	thisPageNav = pagePath;
	thisPage = cmsMakeStringSafe(pagePath);
	thisPageID = pageID;
	timeTick = timeTck;
}

function cmsOverridePageName(pagePath) {
	thisPageNavSaved = pagePath;
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
	var toolbox = 'cmsToolBoxWrap';

	if (p == 'L') {
		cmsToolbarMargin = 'L';
		$("#" + toolbox).removeClass("cmsToolbarAlignmentR").addClass("cmsToolbarAlignmentL");
	} else {
		cmsToolbarMargin = 'R';
		$("#" + toolbox).removeClass("cmsToolbarAlignmentL").addClass("cmsToolbarAlignmentR");
	}
}

setTimeout("cmsResetToolbarScroll()", 500);

var cmsToolTabIdx = 0;
var cmsScrollPos = 0;
var cmsScrollWPos = 0;


function cmsSetPrefs(tab, margin, scrollPos, scrollWPos, opStat) {
	cmsToolTabIdx = tab;
	cmsToolbarMargin = margin;
	cmsScrollPos = scrollPos;
	cmsScrollWPos = scrollWPos;
	cmsMnuVis = opStat;
}

function cmsResetToolbarScroll() {
	setTimeout("cmsShiftPosition('" + cmsToolbarMargin + "')", 100);

	if (!cmsMnuVis) {
		cmsMnuVis = true;
		cmsToggleMenu();
	}

	$(document).scrollTop(cmsScrollPos);
	$('#cmsToolBox').scrollTop(cmsScrollWPos);
}


function cmsMenuFixImages() {
	$(".cmsWidgetBarIconCog").each(function (i) {
		cmsFixGeneralImage(this, 'cog.png');
	});
	$(".cmsWidgetBarIconCross").each(function (i) {
		cmsFixGeneralImage(this, 'cross.png');
	});
	$(".cmsWidgetBarIconPencil").each(function (i) {
		cmsFixGeneralImage(this, 'pencil.png');
	});
	$(".cmsWidgetBarIconPencil2").each(function (i) {
		cmsFixGeneralImage(this, 'pencil.png');
	});
	$(".cmsWidgetBarIconWidget").each(function (i) {
		cmsFixGeneralImage(this, 'application_view_tile.png');
	});
	$(".cmsWidgetBarIconWidget2").each(function (i) {
		cmsFixGeneralImage(this, 'hourglass.png');
	});
	//$(".cmsWidgetBarIconShrink").each(function (i) {
	//cmsFixGeneralImage(this, 'Shrink', 'Shrink', 'arrow_in.png');
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

function cmsFixGeneralImage(elm, img) {
	var id = $(elm).attr('id');
	var title = $(elm).attr('title');
	var alt = $(elm).attr('alt');

	$(elm).html(" <img class='cmsWidgetBarImgReset' border='0' src='/c3-admin/images/" + img + "' alt='" + alt + "' title='" + alt + "' />" + title);
}


function cmsPageLockCheck() {
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
}

function cmsMakeOKToLeave() {
	cmsConfirmLeavingPage = false;
}

function cmsMakeNotOKToLeave() {
	cmsConfirmLeavingPage = true;
}

function cmsRequireConfirmToLeave(confirmLeave) {
	cmsConfirmLeavingPage = confirmLeave;
}

function cmsGetOKToLeaveStatus() {
	return cmsConfirmLeavingPage;
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

	var scrollTopPos = $(document).scrollTop();
	var scrollWTopPos = $('#cmsToolBox').scrollTop();
	var tabID = $('#cmsJQTabedToolbox').tabs("option", "active");

	var webMthd = webSvc + "/RecordEditorPosition";

	$.ajax({
		type: "POST",
		url: webMthd,
		data: "{'ToolbarState': '" + cmsMnuVis + "', 'ToolbarMargin': '" + cmsToolbarMargin + "', 'ToolbarScroll': '" + scrollTopPos + "', 'WidgetScroll': '" + scrollWTopPos + "', 'SelTabID': '" + tabID + "'}",
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

function cmsPreviewTemplate2() {
	var tmpl = $(cmsTemplateListPreviewer).val();
	tmpl = cmsMakeStringSafe(tmpl);

	var srcURL = cmsTemplatePreview + "?carrot_templatepreview=" + tmpl;

	var editIFrame = $('#cmsFrameEditorPreview');
	$(editIFrame).attr('src', srcURL);
	window.frames["cmsFrameEditorPreview"].location.reload();

}


var cmsTemplateListPreviewer = "#cmsTemplateList"


function cmsPreviewTemplate() {
	var tmplReal = $(cmsTemplateDDL).val();
	tmpl = cmsMakeStringSafe(tmplReal);

	cmsLaunchWindowOnly(cmsTemplatePreview + "?carrot_templatepreview=" + tmpl);

	var editFrame = $('#cmsModalFrame');
	var editIFrame = $('#cmsFrameEditor');

	var frmHgt = 450;
	var hgt = ($(window).height() * 0.75) - 90;

	if (hgt > frmHgt) {
		frmHgt = hgt;
	}

	$(editIFrame).attr('width', '100%');
	$(editIFrame).attr('height', frmHgt);

	var ddlPreview = ' <select id="cmsTemplateList"></select>  <input type="button" value="Preview" id="btnPreviewCMS" onclick="cmsPreviewTemplate2();" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ';

	var btnClose = ' <input type="button" id="btnCloseTemplateCMS" value="Close" onclick="cmsCloseModalWin();" /> &nbsp;&nbsp;&nbsp; ';
	var btnApply = ' <input type="button" id="btnApplyTemplateCMS" value="Apply Template" onclick="cmsUpdateTemplate();" /> &nbsp;&nbsp;&nbsp; ';

	$(editFrame).append('<div id="cmsGlossySeaGreenID"><div id="cmsPreviewControls" class="cmsGlossySeaGreen cmsPreviewButtons"> ' + ddlPreview + btnClose + btnApply + ' </div></div>');
	window.setTimeout("cmsLateBtnStyle();", 500);

	var list = $(cmsTemplateListPreviewer);
	$(cmsTemplateDDL + " > option").each(function () {
		list.append(new Option(this.text, this.value));
	});

	$(cmsTemplateListPreviewer).val(tmplReal);

	$('#cmsFrameEditor').attr('id', 'cmsFrameEditorPreview');
	$('#cmsAjaxMainDiv2').attr('id', 'cmsAjaxMainDiv3');

}

function cmsLateBtnStyle() {
	$(".cmsGlossySeaGreen input:button, .cmsGlossySeaGreen input:submit, .cmsGlossySeaGreen input:reset").button();
	$("#cmsGlossySeaGreenID input:button, #cmsGlossySeaGreenID input:submit, #cmsGlossySeaGreenID input:reset").button();
}

var cmsTemplateDDL = "";

function cmsSetTemplateDDL(ddlName) {
	cmsTemplateDDL = ddlName;
}

function cmsUpdateTemplate() {
	cmsSaveToolbarPosition();

	var tmpl = '';

	if ($(cmsTemplateListPreviewer).length) {
		tmpl = $(cmsTemplateListPreviewer).val();
	} else {
		tmpl = $(cmsTemplateDDL).val();
	}

	tmpl = cmsMakeStringSafe(tmpl);

	var webMthd = webSvc + "/UpdatePageTemplate";

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
	cmsSpinnerLong();

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
	cmsSpinnerLong();

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
	cmsSpinnerLong();

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

var IsPublishing = false;

function cmsApplyChanges() {

	var webMthd = webSvc + "/PublishChanges";

	// prevent multiple submissions
	if (!IsPublishing) {

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

	IsPublishing = true;
}


function cmsUpdateHeartbeat(data, status) {
	var hb = $('#cmsHeartBeat');
	hb.empty().append('HB:  ');
	hb.append(data.d);
	//cmsSpinnerShort();
}

function cmsSaveContentCallback(data, status) {

	if (data.d == "OK") {
		cmsSpinnerShort();
		cmsDirtyPageRefresh();
	} else {
		cmsAlertModal(data.d);
	}
}

function cmsAjaxGeneralCallback(data, status) {
	//if (data.d == "OK") {
	//	cmsSpinnerShort();
	//} else {
	//	cmsAlertModal(data.d);
	//}

	if (data.d != "OK") {
		cmsAlertModal(data.d);
	}

}

function cmsSaveWidgetsCallback(data, status) {

	if (data.d == "OK") {
		cmsSpinnerShort();
		cmsDirtyPageRefresh();
	} else {
		cmsAlertModal(data.d);
	}
}

function cmsSavePageCallback(data, status) {
	if (data.d == "OK") {
		cmsSpinnerShort();
		cmsMakeOKToLeave();
		cmsNotifySaved();
		//window.setTimeout("location.href = \'" + thisPageNav + "\'", 10000);
		iCount = 10;
		cmsCountdownWindow();
	} else {
		cmsAlertModal(data.d);
	}
}

var iCount = 0;
function cmsCountdownWindow() {
	if (iCount > 0) {
		iCount--;
		$('#cmsSaveCountdown').html(iCount);

		setTimeout("cmsCountdownWindow();", 1025);
	} else {
		window.setTimeout("location.href = \'" + thisPageNavSaved + "\'", 250);
	}
}


function cmsNotifySaved() {

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
				window.setTimeout("location.href = \'" + thisPageNavSaved + "\'", 250);
				$(this).dialog("close");
			}
		}
	});

	cmsFixDialog('CMSsavedconfirmmsg');
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

function cmsSendTrackbackBatch() {

	var webMthd = webSvc + "/SendTrackbackBatch";

	if (!cmsGetOKToLeaveStatus()) {

		$.ajax({
			type: "POST",
			url: webMthd,
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsAjaxGeneralCallback,
			error: cmsAjaxFailedSwallow
		});

	}

	setTimeout("cmsSendTrackbackBatch();", 15000);

}

setTimeout("cmsSendTrackbackBatch();", 5000);

function cmsSendTrackbackPageBatch() {

	var webMthd = webSvc + "/SendTrackbackPageBatch";

	if (!cmsGetOKToLeaveStatus()) {

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsAjaxGeneralCallback,
			error: cmsAjaxFailedSwallow
		});

	}

	setTimeout("cmsSendTrackbackPageBatch();", 3000);
}

setTimeout("cmsSendTrackbackPageBatch();", 1500);

function cmsAjaxFailedSwallow(request) {
	var s = "";
	s = s + "<b>status: </b>" + request.status + '<br />\r\n';
	s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
	s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
	// cmsAlertModal(s);
}

function cmsAjaxFailed(request) {
	var s = "";
	if (request.status > 0) {
		s = s + "<b>status: </b>" + request.status + '<br />\r\n';
		s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
		s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
		cmsAlertModal(s);
	}
}

function cmsAlertModalHeightWidth(request, h, w) {

	$("#CMSmodalalertmessage").html('');
	$("#CMSmodalalertmessage").html(request);

	$("#CMSmodalalert").dialog({
		height: h,
		width: w,
		modal: true,
		buttons: {
			"OK": function () {
				$(this).dialog("close");
			}
		}
	});

	cmsFixDialog('CMSmodalalertmessage');
}

function cmsAlertModal(request) {
	cmsAlertModalHeightWidth(request, 400, 550);
}
function cmsAlertModalSmall(request) {
	cmsAlertModalHeightWidth(request, 250, 400);
}
function cmsAlertModalLarge(request) {
	cmsAlertModalHeightWidth(request, 550, 700);
}


//===========================
// do a lot of iFrame magic

function cmsShowWidgetList() {
	cmsLaunchWindow('/c3-admin/WidgetList.aspx?pageid=' + thisPageID + "&zone=cms-all-placeholder-zones");
}
function cmsManageWidgetList(zoneName) {
	//alert(zoneName);
	cmsLaunchWindow('/c3-admin/WidgetList.aspx?pageid=' + thisPageID + "&zone=" + zoneName);
}
function cmsManageWidgetHistory(widgetID) {
	//alert(widgetID);
	cmsLaunchWindow('/c3-admin/WidgetHistory.aspx?pageid=' + thisPageID + "&widgetid=" + widgetID);
}

function cmsShowEditPageInfo() {

	cmsLaunchWindow('/c3-admin/PageEdit.aspx?pageid=' + thisPageID);
}

function cmsShowEditPostInfo() {

	cmsLaunchWindow('/c3-admin/BlogPostEdit.aspx?pageid=' + thisPageID);
}

function cmsShowAddChildPage() {

	cmsLaunchWindow('/c3-admin/PageAddChild.aspx?pageid=' + thisPageID);
}
function cmsShowAddTopPage() {

	cmsLaunchWindow('/c3-admin/PageAddChild.aspx?addtoplevel=true&pageid=' + thisPageID);
}
function cmsEditSiteMap() {

	cmsLaunchWindow('/c3-admin/SiteMapPop.aspx');
}

function cmsSortChildren() {
	//cmsAlertModal("cmsSortChildren");
	cmsLaunchWindowOnly('/c3-admin/PageChildSort.aspx?pageid=' + thisPageID);
}

function cmsShowEditWidgetForm(w, m) {
	//cmsAlertModal("cmsShowEditWidgetForm");
	cmsLaunchWindow('/c3-admin/ContentEdit.aspx?pageid=' + thisPageID + "&widgetid=" + w + "&mode=" + m);
}

function cmsShowEditContentForm(f, m) {
	//cmsAlertModal("cmsShowEditContentForm");
	cmsLaunchWindow('/c3-admin/ContentEdit.aspx?pageid=' + thisPageID + "&field=" + f + "&mode=" + m);
}

function cmsFixSpinner() {
	$(".blockMsg img").addClass('cmsImageSpinner');
	$(".blockMsg table").addClass('cmsImageSpinnerTbl');
}

var cmsHtmlSpinner = '<table width="100%" class="cmsImageSpinnerTbl" border="0"><tr><td align="center" id="cmsSpinnerZone"><img id="cmsImageSpinnerImage" class="cmsImageSpinner" border="0" src="/c3-admin/images/ani-smallbar.gif"/></td></tr></table>';

function cmsSpinnerShort() {

	$("#cmsDivActive").block({ message: cmsHtmlSpinner,
		css: { border: 'none', backgroundColor: 'transparent' },
		fadeOut: 500,
		timeout: 750,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});
	cmsFixSpinner();
}

function cmsSpinnerLong() {

	$("#cmsDivActive").block({ message: cmsHtmlSpinner,
		css: { border: 'none', backgroundColor: 'transparent' },
		fadeOut: 10000,
		timeout: 12000,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});
	cmsFixSpinner();
}

function cmsBuildOrderAndUpdateWidgets() {
	var ret = cmsBuildOrder();

	if (ret) {
		cmsUpdateWidgets();
	}

	return true;
}

function cmsBuildOrder() {
	cmsSpinnerShort();

	$("#cmsFullOrder").val('');

	$(".cmsTargetArea").find('#cmsControl.cmsWidgetControlItem').each(function (i) {
		var txt = $(this).find('#cmsCtrlOrder');
		$(txt).val('');

		var p = $(this).parent().parent().attr('id');
		var key = $(this).find('#cmsCtrlID').val();
		txt.val(i + '\t' + p + '\t' + key);
		//alert(txt.val());

		$("#cmsFullOrder").val($("#cmsFullOrder").val() + '\r\n ' + txt.val());
	});

	return true;
}

function cmsRemoveWidgetLink(v) {

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

/*
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
*/

function cmsSetOrder(fld) {
	var id = $(fld).attr('id');
	$("#cmsMovedItem").val(id);
}

function cmsInitWidgets() {
	// make sure sort only fires once
	var cmsSortWidgets = false;

	if (!cmsIsPageLocked) {

		$('#cmsJQTabedToolbox').tabs();

		$('#cmsJQTabedToolbox').tabs("option", "active", cmsToolTabIdx);

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
			handle: ".cmsWidgetTitleBar",
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
}


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
		$(zone).attr('style', 'border: solid 0px #000000; padding: 1px; height: 100px; max-width: 650px; overflow: auto;');
	} else {
		$(zone).attr('style', '');
	}
}


function cmsMoveWidgetResizer(key, state) {
	var item = $("#" + key);
	var id = $(item).attr('id');
	var zone = $(item).find('#cmsControl');

	if (state != 0) {
		$(zone).attr('style', 'border: solid 0px #000000; padding: 1px; height: 75px; max-width: 650px; overflow: auto;');
	} else {
		$(zone).attr('style', '');
	}
}


function cmsFixDialog(dialogname) {
	var dilg = $("#" + dialogname).parent().parent();

	cmsOverrideCSSScope(dilg, "");

	$(".ui-widget-overlay").each(function (i) {
		$(this).wrap("<div class=\"cmsGlossySeaGreen\" />");
		$(this).css('zIndex', 9950001);
	});

	var d = $(dilg);
	$(d).wrap("<div class=\"cmsGlossySeaGreen\" />");
	$(d).css('zIndex', 9950005);

	//$(dilg).find('.ui-dialog-titlebar').addClass("cmsGlossySeaGreen");
	//$(dilg).find('ui-dialog-title').addClass("cmsGlossySeaGreen");
}

function cmsOverrideCSSScope(elm, xtra) {
	var c = $(elm).attr('class');
	if (c.indexOf("cmsGlossySeaGreen") < 0 || c.indexOf(xtra) < 0) {
		$(elm).attr('class', "cmsGlossySeaGreen " + xtra + " " + c);
	}
	$(elm).addClass("cmsGlossySeaGreen");
}

function cmsGenericEdit(PageId, WidgetId) {
	cmsLaunchWindow('/c3-admin/ControlPropertiesEdit.aspx?pageid=' + PageId + '&id=' + WidgetId);
}

function cmsLaunchWindow(theURL) {
	var TheURL = theURL;

	cmsSetiFrameSource(theURL);

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

function cmsSetiFrameSource(theURL) {
	var TheURL = theURL;
	$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv2"> <iframe scrolling="auto" id="cmsFrameEditor" frameborder="0" name="cmsFrameEditor" width="90%" height="500" src="' + TheURL + '" /> </div>');

	$("#cmsAjaxMainDiv2").block({ message: '<table><tr><td><img class="cmsAjaxModalSpinner" src="/c3-admin/images/Ring-64px-A7B2A0.gif"/></td></tr></table>',
		css: { width: '98%', height: '98%' },
		fadeOut: 1000,
		timeout: 1200,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});
}

function cmsLaunchWindowOnly(theURL) {
	var TheURL = theURL;

	cmsSetiFrameSource(theURL);

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

//===================

var jqAttemptCount = 0;
var jq1URL = 'http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js';
var jq2URL = 'http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.0/jquery-ui.min.js';

function cmsSetJQueryURL(jqPath, jqUIPath) {
	jq1URL = jqPath;
	jq2URL = jqUIPath;

	cmsLoadJQuery();
}

function cmsLoadJQuery() {
	setTimeout('cmsAttemptAttachJQ1()', 200);
	setTimeout('cmsAttemptAttachJQ2()', 800);
}

function cmsAttachJQScript1() {
	if (typeof jQuery == 'undefined') {
		var first = document.getElementsByTagName('head')[0].children[0];
		var script1 = document.createElement('script');
		script1.src = jq1URL;

		document.getElementsByTagName('head')[0].insertBefore(script1, first);
		setTimeout('cmsAttemptAttachJQ1()', 125);
	}
}

function cmsAttemptAttachJQ1() {
	if (jqAttemptCount < 50) {
		jqAttemptCount++;
		if (typeof jQuery == 'undefined') {
			cmsAttachJQScript1();
		} else {
			cmsAttemptAttachJQ2();
			cmsAttemptExtra();
		}
	}
}

function cmsAttachJQScript2() {
	if (typeof jQuery.ui == 'undefined') {
		cmsAttachSecondScript(jq2URL);

		setTimeout('cmsAttemptAttachJQ2()', 125);
	}
}

function cmsAttemptAttachJQ2() {
	if (jqAttemptCount < 150) {
		jqAttemptCount++;
		if (typeof jQuery != 'undefined' && typeof jQuery.ui == 'undefined') {
			cmsAttachJQScript2();
		}
	}
}

function cmsAttachSecondScript(scriptURL) {
	var second = document.getElementsByTagName('head')[0].children[1];
	var script2 = document.createElement('script');
	script2.src = scriptURL;

	document.getElementsByTagName('head')[0].insertBefore(script2, second);
}

var cmsExtraAttached = false;

function cmsAttachExtraScripts() {
	if (!cmsExtraAttached && (typeof jQuery != 'undefined')) {
		cmsAttachSecondScript("/c3-admin/includes/jquery.simplemodal.js");
		cmsAttachSecondScript("/c3-admin/includes/jquery.blockUI.js");
		setTimeout('cmsAttemptExtra()', 125);
		cmsExtraAttached = true;
	}
}

function cmsAttemptExtra() {
	if (jqAttemptCount < 250) {
		jqAttemptCount++;
		if (typeof jQuery != 'undefined') {
			cmsAttachExtraScripts();
		}
	}
}
