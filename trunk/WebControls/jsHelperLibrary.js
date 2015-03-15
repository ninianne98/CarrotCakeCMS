
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


var jqAttemptCount = 0;
//var jqURL = 'http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js';
var jqURL = '/<%=WebResource("Carrotware.Web.UI.Controls.jquery-1-8-3.js")%>';

function __carrotware_SetJQueryURL(jqPath) {
	jqURL = jqPath;

	__carrotware_LoadJQuery();
}

function __carrotware_LoadJQuery() {
	setTimeout('__carrotware_LoadJS()', 150);
}

function __carrotware_LoadJQScript() {
	if (typeof jQuery == 'undefined') {
		//alert('adding');
		var script = document.createElement('script');
		script.src = jqURL;
		document.getElementsByTagName('head')[0].appendChild(script);
		setTimeout('__carrotware_LoadJS()', 150);
	}
}


function __carrotware_LoadJS() {
	jqAttemptCount++;
	if (jqAttemptCount < 50) {
		if (typeof jQuery == 'undefined') {
			__carrotware_LoadJQScript();
		}
	}
}

function __carrotware_PageValidate() {
	setTimeout("__carrotware_IsPageValid();", 250);
}

function __carrotware_IsPageValid() {

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


//setTimeout('__carrotware_LoadJS()', 150);

//====================================================

/*
<script>
//alert("alert 1");
setTimeout('delayed()', 1500);
function delayed() {
//alert("alert 3.a");
$(document).ready(function () {
alert("alert 2");
});
//alert("alert 3.b");
}
</script>
*/