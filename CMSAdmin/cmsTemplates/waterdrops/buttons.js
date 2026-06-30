$(document).ready(function () {
	buttonStyleWater()
});

$(document).ajaxComplete(function (event, xhr, settings) {
	buttonStyleWater()
});

function buttonStyleWater() {
	$("input:button, input:submit, input:reset, button").button();

	$("#search input").removeClass("ui-button ui-widget ui-state-default ui-state-hover");
}