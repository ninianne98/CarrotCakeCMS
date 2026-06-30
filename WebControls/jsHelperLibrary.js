function __carrotware_ValidateLongText(sender, args) {
	var txtValue = args.Value;
	var maxLen = 1000;

	args.IsValid = true;
	if (txtValue.indexOf('<') > -1 || txtValue.indexOf('>') > -1) {
		alert("Invalid characters encountered: cannot include < or > ");
		args.IsValid = false;
		return;
	}

	if (txtValue.length > maxLen) {
		alert("Comments are too long, limit is " + maxLen);
		args.IsValid = false;
		return;
	}

	args.IsValid = true;
}

//====================================================

function __carrotware_RedirectWithQuerystring(url, query) {
	location.href = url + '?' + query;
}

function __carrotware_RedirectWithQuerystringParm(url, parm, query) {
	if (query.length > 0) {
		var esc = encodeURIComponent(query);
		//alert(esc);
		location.href = url + '?' + parm + '=' + esc;
	}
}

//====================================================

var carrotAttemptCount = 0;
var carrot_JQ_URL = '/<%=WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-13-3.js")%>';

function __carrotware_SetJQueryURL(jqPath) {
	carrot_JQ_URL = jqPath;

	__carrotware_LoadJQuery();
}

function __carrotware_LoadJQuery() {
	setTimeout('__carrotware_LoadJS()', 1500);
}

function __carrotware_LoadJQScript() {
	if (typeof jQuery == 'undefined') {
		//alert('adding');
		var script = document.createElement('script');
		script.src = carrot_JQ_URL;
		document.getElementsByTagName('head')[0].appendChild(script);
		setTimeout('__carrotware_LoadJS()', 200);
	}
}

function __carrotware_LoadJS() {
	carrotAttemptCount++;
	if (carrotAttemptCount < 50) {
		if (typeof jQuery == 'undefined') {
			__carrotware_LoadJQScript();
		}
	}
}

function __carrotware_ResetValidation() {
	if (typeof (Page_ClientValidate) == 'function') {
		Page_BlockSubmit = false;
		Page_IsValid = true;
	}
}

function __carrotware_PageValidate(validGrp) {
	setTimeout("__carrotware_IsPageValid('" + validGrp + "');", 250);
}

function __carrotware_IsPageValid(validGrp) {
	if (typeof (Page_ClientValidate) == 'function') {
		Page_ClientValidate(validGrp);
	} else {
		return true;
	}

	if (Page_IsValid) {
		return true;
	} else {
		// because the sub form has validated, reset so other forms are not impacted
		setTimeout("__carrotware_ResetValidation()", 500);
		return false;
	}
}