function AjaxBtnLoad() {
	$(function () {
		$('#jqtabs').tabs();
	});
	$("#jqaccordion").accordion({
		//collapsible: true
		active: false,
		autoheight: false,
		collapsible: true,
		alwaysOpen: false
	});

	$(function () {
		$("input:button, input:submit").button();
	});

	$(".dateRegion").each(function (i) {
		$(this).datepicker({
			changeMonth: true,
			changeYear: true,
			showOn: 'button',
			buttonImage: '/Manage/images/calendar.gif',
			buttonImageOnly: true,
			constrainInput: true
		});
	});
}

//$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
function BlockUI(elementID) {
	var prm = Sys.WebForms.PageRequestManager.getInstance();
	prm.add_beginRequest(function () {
		$("#" + elementID).block({ message: '<table><tr><td><img src="/Manage/images/Ring-64px-A7B2A0.gif"/></td></tr></table>',
			css: {},
			fadeOut: 1000,
			timeout: 1200,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	});
	prm.add_endRequest(function () {
		$("#" + elementID).unblock();
	});
}
$(document).ready(function () {
	BlockUI("cmsAjaxMainDiv");
	$.blockUI.defaults.css = {};
});

//===================

function MakeStringSafe(val) {
	val = Base64.encode(val);
	return val;
}

function cmsAjaxFailed(request) {
	var s = "";
	s = s + "<b>status: </b>" + request.status + '<br />\r\n';
	s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
	s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
	cmsAlertModal(s);
}

function cmsAjaxGeneralCallback(data, status) {
	if (data.d != "OK") {
		cmsAlertModal(data.d);
	}
}

function cmsAlertModal(request) {
	$("#divCMSModal").dialog("destroy");

	$("#divCMSModalMsg").html(request);

	$("#divCMSModal").dialog({
		height: 400,
		width: 550,
		modal: true,
		buttons: {
			"OK": function () {
				$(this).dialog("close");
			}
		}
	});
}


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