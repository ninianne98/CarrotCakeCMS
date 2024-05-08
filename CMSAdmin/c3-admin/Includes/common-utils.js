﻿if (typeof jQuery === 'undefined') {
	throw new Error('Common Utils JavaScript requires jQuery')
}

var cmsDatePattern = '';
var cmsTimePattern = '';

function AjaxBtnLoad() {
	$(function () {
		$('#jqtabs, .jqtabs').tabs();
	});

	$("#jqaccordion, .jqaccordion").accordion({
		//collapsible: true
		active: false,
		autoheight: false,
		collapsible: true,
		alwaysOpen: false
	});

	//	$(function () {
	//		$("#jqradioset, .jqradioset").buttonset();
	//	});

	$("input:button, input:submit, input:reset").button();

	initCheckboxStyle();

	if (!cmsDatePattern || cmsDatePattern.length < 1) {
		cmsGetShortDatePattern();
	} else {
		cmsSetDateRegion();
	}

	if (!cmsTimePattern || cmsTimePattern.length < 1) {
		cmsGetShortTimePattern();
	} else {
		cmsSetTimeRegion();
	}
}

var webSvc = cmsWebServiceApi;

function cmsScrubDate(val) {
	val = val.replace(/m/gi, 'mm');
	val = val.replace(/mmm/gi, 'mm');
	val = val.replace(/d/gi, 'dd');
	val = val.replace(/ddd/gi, 'dd');

	return val.replace(/yyyy/gi, 'yy');
}

function cmsScrubTime(val) {
	val = val.replace(/h/gi, 'hh');
	val = val.replace(/hhh/gi, 'hh');
	val = val.replace(/HH/gi, 'HH');
	val = val.replace(/HHH/gi, 'HH');
	val = val.replace(/t/gi, 'tt');
	val = val.replace(/ttt/gi, 'tt');

	return val.replace(/yyyy/gi, 'yy');
}

function cmsGetShortDatePattern() {
	var datePatrn = __carrotGetDateFormat();
	cmsDatePattern = cmsScrubDate(datePatrn);

	cmsSetDateRegion();
}

function cmsSetDateRegion() {
	$(".dateRegion").each(function () {
		$(this).datepicker({
			dateFormat: cmsDatePattern,
			buttonText: cmsDatePattern,
			changeMonth: true,
			changeYear: true,
			showOn: "both",
			buttonImage: '/c3-admin/images/calendar.png',
			buttonImageOnly: true,
			constrainInput: true,
			beforeShow: function () {
				setTimeout(function () {
					$('.ui-datepicker').css('z-index', 15);
				}, 0);
			}
		});
	});
}

function cmsGetShortTimePattern() {
	var datePatrn = __carrotGetTimeFormat();
	cmsTimePattern = cmsScrubTime(datePatrn);

	cmsSetTimeRegion();
}

function cmsSetTimeRegion() {
	var showAmPm = (cmsTimePattern.indexOf('tt') != -1) || !(cmsTimePattern.indexOf('HH') != -1);
	var stringAM = __carrotGetAMDateFormat();
	var stringPM = __carrotGetPMDateFormat();

	$(".timeRegion").each(function () {
		if (!$(this).hasClass("hasTimePicker")) {
			$(this).addClass("hasTimePicker");
			$(this).parent().css('z-index', 15);
			$(this).parent().css('position', 'relative');

			var id = $(this).attr('id');
			$('<img class="ui-timepicker-trigger" src="/c3-admin/images/clock.png" for="' + id + '" id="' + id + '_triggerbtn" alt="' + cmsTimePattern + '" title="' + cmsTimePattern + '">').insertAfter(this);

			$(this).timepicker({
				showOn: "both",
				button: '#' + id + '_triggerbtn',
				showPeriodLabels: showAmPm,
				showPeriod: showAmPm,
				amPmText: [stringAM, stringPM],
				showLeadingZero: true
			});
		};
	});
}

$(document).ready(function () {
	AjaxBtnLoad();
});

var htmlAjaxSpinnerTable = '<table class="cmsTableSpinner"><tr><td><img class="cmsRingSpinner" src="/c3-admin/images/Ring-64px-A7B2A0.gif"/></td></tr></table>';

function BlockUI(elementID) {
	if (typeof (Sys) != 'undefined') {
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_beginRequest(function () {
			$("#" + elementID).block({
				message: htmlAjaxSpinnerTable,
				css: {
					padding: 0,
					margin: 0,
					width: '30%',
					top: '40%',
					left: '45%',
					textAlign: 'center'
				},
				fadeOut: 80000,
				timeout: 90000,
				overlayCSS: {
					backgroundColor: '#FFFFFF',
					opacity: 0.6,
					border: '0px solid #000000'
				}
			});
			var spinner = ".blockMsg"; // fix for spinner location
			$(spinner).css('top', '33%');
			$(spinner).css('left', '45%');
			$(spinner).css('margin-left', 'auto');
			$(spinner).css('margin-right', 'auto');
			$(spinner).css('position', 'fixed');
		});
		prm.add_endRequest(function () {
			$("#" + elementID).unblock();
		});
	}
}

$(document).ready(function () {
	BlockUI("cmsAjaxMainDiv");
	$.blockUI.defaults.css = {};
});

function checkGridBoxes(gridID) {
	$('#' + gridID + ' input[type=checkbox]').each(function () {
		$(this).prop('checked', true).trigger("change");  // because of iCheck, must do .trigger("change")
	});
}

function uncheckGridBoxes(gridID) {
	$('#' + gridID + ' input[type=checkbox]').each(function () {
		$(this).prop('checked', false).trigger("change");  // because of iCheck, must do .trigger("change")
	});
}

//===================

var webSvc = cmsWebServiceApi;

function cmsGetServiceAddress() {
	return webSvc;
}

function MakeStringSafe(val) {
	val = Base64.encode(val);
	return val;
}

function cmsMakeStringSafe(val) {
	val = Base64.encode(val);
	return val;
}

function cmsAjaxFailedSwallow(request) {
	var s = "";
	if (request.status > 0) {
		s = s + "cmsAjaxFailedSwallow(request) \r\n";
		s = s + "status: " + request.status + ' \r\n';
		s = s + "statusText:  " + request.statusText + ' \r\n';
		s = s + "responseText:  " + request.responseText + ' \r\n';
		if (typeof (console) !== "undefined" && console.log) {
			console.log(s);
		}
	}
}

function cmsAjaxFailed(request) {
	var s = "";
	if (request.status > 0) {
		s = s + "cmsAjaxFailed(request) \r\n";
		s = s + "<b>status: </b>" + request.status + '<br />\r\n';
		s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
		s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
		cmsAlertModal(s);
	}
}

function cmsAjaxGeneralCallback(data, status) {
	if (data.d != "OK") {
		cmsAlertModal(data.d);
	}
}

function cmsAlertModalClose() {
	$("#divCMSModal").dialog("close");
}

function cmsAlertModalHeightWidth(request, h, w) {
	$("#divCMSModalMsg").html('');

	$("#divCMSModalMsg").html(request);

	$("#divCMSModal").dialog({
		height: h,
		width: w,
		modal: true,
		buttons: {
			"OK": function () {
				cmsAlertModalClose();
			}
		}
	});
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

function cmsAlertModalHeightWidthBtns(request, h, w, buttonsOpts) {
	$("#divCMSModalMsg").html('');

	$("#divCMSModalMsg").html(request);

	$("#divCMSModal").dialog({
		open: function () {
			$(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
		},
		height: h,
		width: w,
		modal: true,
		buttons: buttonsOpts
	});
}

function cmsAlertModalBtns(request, buttonsOpts) {
	cmsAlertModalHeightWidthBtns(request, 400, 550, buttonsOpts);
}
function cmsAlertModalSmallBtns(request, buttonsOpts) {
	cmsAlertModalHeightWidthBtns(request, 250, 400, buttonsOpts);
}
function cmsAlertModalLargeBtns(request, buttonsOpts) {
	cmsAlertModalHeightWidthBtns(request, 550, 700, buttonsOpts);
}

function cmsOpenPage(theURL) {
	$("#divCMSCancelWinMsg").html('');

	if (theURL.length > 3) {
		$("#divCMSCancelWinMsg").html('Are you sure you want to open the webpage and leave this editor? <br /> All unsaved changes will be lost!');

		$("#divCMSCancelWin").dialog({
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
					cmsRecordCancellation();
					window.setTimeout("location.href = '" + theURL + "';", 800);
					$(this).dialog("close");
				}
			}
		});
	} else {
		cmsAlertModalSmall("No saved content to show.");
	}
}

// begin fancy validator stylings

$(document).ready(function () {
	loadValidationStyles();
	ajaxLoadValidationStyles();
});

function ajaxLoadValidationStyles() {
	if (typeof (Sys) != 'undefined') {
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(function () {
			loadValidationStyles();
		});
	}
}

function loadValidationStyles() {
	if (typeof (Page_ClientValidate) == 'function') {
		cmsLoadOverrideValidation();
	}
}

var invalidCSSClass = "validationErrorBox";

function cmsLoadOverrideValidation() {
	for (i = 0; i < Page_Validators.length; i++) {
		cmsOverrideValidation(Page_Validators[i]);
	}
}

function cmsOverrideValidation(validator) {
	var origValidator = validator.evaluationfunction;

	validator.evaluationfunction = function (val) {
		var id = val.controltovalidate;

		var valid1 = origValidator(val);
		var valid2 = cmsChkCtrlValid(val);

		if (valid2) {
			$('#' + id).removeClass(invalidCSSClass);
		}

		if (!valid1) {
			$('#' + id).addClass(invalidCSSClass);
		}

		return valid1;
	};
}

function cmsChkCtrlValid(val) {
	if (typeof val.controltocompare != 'undefined') {
		var id2 = val.controltocompare;
		var ctrl2 = document.getElementById(id2);
		var v2 = true;

		for (var i = 0; i < ctrl2.Validators.length; i++) {
			if (!ctrl2.Validators[i].isvalid) {
				v2 = false;
				break;
			}
		}

		if (v2) {
			$('#' + id2).removeClass(invalidCSSClass);
		}
	}

	var id = val.controltovalidate;
	var ctrl = document.getElementById(id);

	for (var i = 0; i < ctrl.Validators.length; i++) {
		if (!ctrl.Validators[i].isvalid) {
			return false;
		}
	}

	return true;
}

function cmsLoadPrettyValidationPopup(validSummaryFld) {
	if (!Page_IsValid) {
		var txt = $('#' + validSummaryFld).html();
		cmsAlertModalSmall(txt);
	}
}

function cmsLoadPrettyValidation(validSummary) {
	$(".cmsPrettyValidationButton").each(function (i) {
		$(this).click(function () {
			cmsLoadPrettyValidationPopup(validSummary);
		});
	});
}

// end fancy validator stylings

function ProcessKeyPress(e) {
	var obj = window.event ? event : e;
	var key = (window.event) ? event.keyCode : e.which;

	if ((key == 13) || (key == 10)) {
		obj.returnValue = false;
		obj.cancelBubble = true;
		return false;
	}
	return true;
}

function checkIntNumber(obj) {
	var n = obj.value;
	var intN = parseInt(n);
	if (isNaN(intN) || intN < 0 || n != intN) {
		alert("Value must be non-negative integers.");
		obj.focus();
	} else {
		obj.value = intN;
	}
}

function checkFloatNumber(obj) {
	var n = obj.value;
	var intN = parseFloat(n);
	if (isNaN(intN) || intN < 0 || n != intN) {
		alert("Value must be non-negative decimals.");
		obj.value = 0;
		obj.focus();
	} else {
		obj.value = intN;
	}
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

	setTimeout("cmsSendTrackbackBatch();", 10000);
}

//setTimeout("cmsSendTrackbackBatch();", 5000);

function cmsSendTrackbackPageBatch(thePageID) {
	var webMthd = webSvc + "/SendTrackbackPageBatch";

	if (!cmsGetOKToLeaveStatus()) {
		$.ajax({
			type: "POST",
			url: webMthd,
			data: JSON.stringify({ ThisPage: thePageID }),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: cmsAjaxGeneralCallback,
			error: cmsAjaxFailedSwallow
		});
	}

	setTimeout("cmsSendTrackbackPageBatch('" + thePageID + "');", 12000);
}

function cmsSaveMakeOKAndCancelLeave() {
	cmsMakeOKToLeave();
	if (cmsIsPageValid()) {
		setTimeout("cmsMakeNotOKToLeave();", 30000);
	} else {
		setTimeout("cmsMakeNotOKToLeave();", 500);
	}
}

function cmsIsPageValid() {
	if (typeof (Page_ClientValidate) == 'function') {
		Page_ClientValidate();
	} else {
		return true;
	}

	if (Page_IsValid) {
		return true;
	} else {
		return false;
	}
}

function cmsForceInputValidation(inputId) {
	var targetedControl = document.getElementById(inputId);

	if (typeof (targetedControl.Validators) != "undefined") {
		var i;
		for (i = 0; i < targetedControl.Validators.length; i++)
			ValidatorValidate(targetedControl.Validators[i]);
	}

	ValidatorUpdateIsValid();
}

//====================================

var TheURL = '';
var RefreshPage = 0;

//============ full page
function ShowWindowNoRefresh(theURL) {
	RefreshPage = 0;
	LaunchWindow(theURL);
}

function ShowWindow(theURL) {
	RefreshPage = 1;
	LaunchWindow(theURL);
}

function SetIframeRealSrc(theFrameID) {
	var theSRC = $('#' + theFrameID).attr('realsrc');
	$('#' + theFrameID).attr('src', theSRC);
}

function LaunchWindow(theURL) {
	TheURL = theURL;

	$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv2"> <iframe scrolling="auto" id="cmsFrameEditor" frameborder="0" name="cmsFrameEditor" width="90%" height="500" realsrc="' + TheURL + '" src="/c3-admin/includes/Blank.htm" /> </div>');

	setTimeout("SetIframeRealSrc('cmsFrameEditor');", 1500);

	$("#cmsAjaxMainDiv2").block({
		message: htmlAjaxSpinnerTable,
		css: { width: '98%', height: '98%' },
		fadeOut: 1000,
		timeout: 1200,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});

	setTimeout("LoadWindow();", 800);
}

//============ popup page
function ShowWindowNoRefreshPop(theURL) {
	RefreshPage = 0;
	LaunchWindowPop(theURL);
}

function ShowWindowPop(theURL) {
	RefreshPage = 1;
	LaunchWindowPop(theURL);
}

function LaunchWindowPop(theURL) {
	TheURL = theURL;

	$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv2"> <iframe scrolling="auto" id="cmsFrameEditor" frameborder="0" name="cmsFrameEditor" width="640" height="390" realsrc="' + TheURL + '" src="/c3-admin/includes/Blank.htm" /> </div>');

	setTimeout("SetIframeRealSrc('cmsFrameEditor');", 1500);

	$("#cmsAjaxMainDiv2").block({
		message: htmlAjaxSpinnerTable,
		css: { width: '98%', height: '98%' },
		fadeOut: 1000,
		timeout: 1200,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});

	setTimeout("LoadWindow();", 800);
}

function LoadWindow() {
	$("#cms-basic-modal-content").simplemodal({
		onClose: function (dialog) {
			//$.simplemodal.close(); // must call this!
			setTimeout("$.simplemodal.close();", 800);
			$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv"></div>');
			DirtyPageRefresh();
		}
	});
	IsDirty = 0;
	$('#cms-basic-modal-content').simplemodal();
	return false;
}

//======================================

var IsDirty = 0;

function SetDirtyPage() {
	IsDirty = 1;
}
function DirtyPageRefresh() {
	if (RefreshPage == 1) {
		$("#cmsAjaxMainDiv").block({
			message: htmlAjaxSpinnerTable,
			css: {},
			fadeOut: 1000,
			timeout: 1200,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});

		setTimeout("__doPostBack('PageRefresh', 'JavaScript');", 800);
	} else {
		$("#cmsAjaxMainDiv").block({
			message: htmlAjaxSpinnerTable,
			css: {},
			fadeOut: 250,
			timeout: 500,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}
}

function cmsSpinnerUnblock() {
	$("#cmsAjaxMainDiv").unblock();
}

//===================

function AjaxShowErrorMsg(sender, args) {
	if (args.get_error() != undefined) {
		//debugger;
		var errorMessage;
		var errorCode = args.get_response().get_statusCode();
		if (errorCode == '200' || errorCode == '500') {
			errorMessage = args.get_error().message;
		} else {
			// Error occurred somewhere other than the server page.
			errorMessage = 'An error occurred. ' + errorCode;
		}
		args.set_errorHandled(true);

		cmsSpinnerUnblock();
		cmsAlertModal(errorMessage);
	}
}

function UpdateAjaxErrorMsg() {
	if (typeof (Sys) != 'undefined') {
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(AjaxShowErrorMsg);
	}
}

$(document).ready(function () {
	UpdateAjaxErrorMsg();
});

//=======================

var fldNameRet = '';

function cmsFileBrowserOpenReturn(fldN) {
	fldN = '#' + fldN;
	var fld = $(fldN);
	fldNameRet = fld.attr('id');

	ShowWindowNoRefresh('/c3-admin/FileBrowser.aspx?returnvalue=1&viewmode=file&fldrpath=/');

	return false;
}

function cmsFileBrowserOpenReturnPop(fldN) {
	fldN = '#' + fldN;
	var fld = $(fldN);
	fldNameRet = fld.attr('id');

	ShowWindowNoRefreshPop('/c3-admin/FileBrowser.aspx?returnvalue=1&viewmode=file&fldrpath=/');

	return false;
}

function cmsSetFileNameReturn(v) {
	var fldN = '#' + fldNameRet;
	var fld = $(fldN);
	fld.val(v);

	setTimeout("$.simplemodal.close();", 200);

	return false;
}

//===========================

var cmsConfirmLeavingPage = true;

function cmsGetPageStatus() {
	return cmsConfirmLeavingPage;
}

function cmsMakeOKToLeave() {
	cmsConfirmLeavingPage = false;
}

function cmsMakeNotOKToLeave() {
	cmsConfirmLeavingPage = true;
}

function cmsGetOKToLeaveStatus() {
	return cmsConfirmLeavingPage;
}

function cmsRequireConfirmToLeave(confirmLeave) {
	cmsConfirmLeavingPage = confirmLeave;
}