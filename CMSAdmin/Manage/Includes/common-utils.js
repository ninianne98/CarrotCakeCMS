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

var htmlAjaxSpinnerTable = '<table style="TableSpinner"><tr><td><img style="RingSpinner" src="/Manage/images/Ring-64px-A7B2A0.gif"/></td></tr></table>';

function BlockUI(elementID) {
	if (typeof (Sys) != 'undefined') {
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_beginRequest(function () {
			$("#" + elementID).block({ message: htmlAjaxSpinnerTable,
				css: {
					padding: 0,
					margin: 0,
					width: '30%',
					top: '40%',
					left: '45%',
					textAlign: 'center'
				},
				fadeOut: 2000,
				timeout: 3000,
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

//===================

function MakeStringSafe(val) {
	val = Base64.encode(val);
	return val;
}

function cmsMakeStringSafe(val) {
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

//====================================

var TheURL = '';
var RefreshPage = 0;

function ShowWindowNoRefresh(theURL) {
	RefreshPage = 0;
	LaunchWindow(theURL);
}

function ShowWindow(theURL) {
	RefreshPage = 1;
	LaunchWindow(theURL);
}

function LaunchWindow(theURL) {
	TheURL = theURL;
	$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv2"> <iframe scrolling="auto" id="cmsFrameEditor" frameborder="0" name="cmsFrameEditor" width="910" height="540" src="' + TheURL + '" /> </div>');

	$("#cmsAjaxMainDiv2").block({ message: htmlAjaxSpinnerTable,
		css: { width: '900px', height: '540px' },
		fadeOut: 1000,
		timeout: 1200,
		overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
	});

	setTimeout("LoadWindow();", 800);
}

function LoadWindow() {
	$("#cms-basic-modal-content").modal({ onClose: function (dialog) {
		//$.modal.close(); // must call this!
		setTimeout("$.modal.close();", 800);
		$('#cmsModalFrame').html('<div id="cmsAjaxMainDiv"></div>');
		DirtyPageRefresh();
	}
	});
	IsDirty = 0;
	$('#cms-basic-modal-content').modal();
	return false;
}


var IsDirty = 0;

function SetDirtyPage() {
	IsDirty = 1;
}
function DirtyPageRefresh() {

	if (RefreshPage == 1) {
		$("#cmsAjaxMainDiv").block({ message: htmlAjaxSpinnerTable,
			css: {},
			fadeOut: 1000,
			timeout: 1200,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});

		setTimeout("__doPostBack('PageRefresh', 'JavaScript');", 800);
	} else {
		$("#cmsAjaxMainDiv").block({ message: htmlAjaxSpinnerTable,
			css: {},
			fadeOut: 250,
			timeout: 500,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}
}