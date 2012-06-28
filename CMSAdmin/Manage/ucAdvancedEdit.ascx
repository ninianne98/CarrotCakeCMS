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
<style type="text/css">
	#simplemodal-container, .simplemodal-container {
		height: 375px;
		width: 780px;
		z-index: 5000;
	}
	#cmsDelIconDiv {
		text-align: right;
	}
	.cmsHighlightPH {
		height: 25px !important;
		margin: 5px;
		padding: 5px;
		background: #FFFFAA !important;
		border: 2px dashed #676F6A !important;
	}
	#cmsWidgetHead img, .GlossySeaGreen a img {
		border: 0px solid #CDE3D6 !important;
	}
	#cmsMainToolbox {
		text-align: left;
	}
</style>

<script type="text/javascript">
	var webSvc = "/Manage/CMS.asmx";

	var thisPage = '<%=CurrentScriptName %>';
	
	var thisPageID = '<%=guidContentID.ToString() %>';

	function cmsEscapeFile() {
		thisPage = cmsMakeStringSafe(thisPage);
	}

	$(document).ready(function() {
		cmsEscapeFile();
	});

	function cmsMakeStringSafe(val) {
		val = Base64.encode(val);
		return val;
	}

	function cmsEditHB() {
		setTimeout("cmsEditHB();", 30 * 1000);

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


	function cmsSaveContent(val, zone) {

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
		var tmpl = $("#<%=ddlTemplate.ClientID%>").val();

		var webMthd = webSvc + "/UpdatePageTemplate";

		tmpl = cmsMakeStringSafe(tmpl);
		
		//alert(tmpl);

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

<% if (!bLocked) { %>
	setTimeout("cmsEditHB();", 3000);
<%} else { %>
	$(document).ready(function() {
		cmsAlertModal("<%=litUser.Text %>");
	});
<%} %>

	function cmsUpdateHeartbeat(data, status) {
		var hb = $('#cmsHeartBeat');
		hb.empty().append('HB:  ');
		hb.append(data.d);
		CMSBusyShort();
	}

	function cmsSaveContentCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
			//window.setTimeout('location.reload()', 800);
			cmsDirtyPageRefresh();
		} else {
			cmsAlertModal(data.d);
		}
	}

	function cmsSaveWidgetsCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
			//window.setTimeout('location.reload()', 800);
			cmsDirtyPageRefresh();
		} else {
			cmsAlertModal(data.d);
		}
	}

	function cmsSavePageCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
			cmsAlertModal("Saved");
			window.setTimeout('location.href = \'<%=CurrentScriptName %>\'', 5000);
		} else {
			cmsAlertModal(data.d);
		}
	}


	function cmsCancelEdit() {
		window.setTimeout('location.href = \'<%=CurrentScriptName %>\'', 1000);
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

		$("#divCMSActive").block({ message: '<table width="100%" border="0"><tr><td align="center"><img border="0" src="/Manage/images/ani-smallbar.gif"/></td></tr></table>',
			css: { border: 'none', backgroundColor: 'transparent' },
			fadeOut: 900,
			timeout: 1200,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}

	function CMSBusyLong() {

		$("#divCMSActive").block({ message: '<table width="100%" border="0"><tr><td align="center"><img border="0" src="/Manage/images/ani-smallbar.gif"/></td></tr></table>',
			css: { border: 'none', backgroundColor: 'transparent' },
			fadeOut: 4000,
			timeout: 5000,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});
	}


	var mnuVis = true;
	function cmsToggleMenu() {
		var t = $('#cmsMainToolbox');
		var m = $('#cmsMnuToggle');
		//alert(m.attr('id'));
		if (mnuVis) {
			m.attr('class', 'ui-icon ui-icon-plusthick');
			t.css('display', 'none')
			mnuVis = false;
		} else {
			m.attr('class', 'ui-icon ui-icon-minusthick');
			t.css('display', 'block')
			mnuVis = true;
		}
	}


	$(function() {
		$(".GlossySeaGreen input:button, .GlossySeaGreen input:submit").button();
	});

	function cmsBuildOrder() {
		CMSBusyShort();

		$("#cmsFullOrder").val('');
		$(".cmsTargetArea").find('#cmsCtrlOrder').each(function(i) {
			var txt = $(this);
			$(txt).val('');
			var p = txt.parent().parent().parent().attr('id');
			var key = txt.parent().find('#cmsCtrlID').val();
			txt.val(i + '\t' + p + '\t' + key);
			//alert($('#'+p).find('#cmsCtrlID').val());

			$("#cmsFullOrder").val($("#cmsFullOrder").val() + '\r\n ' + txt.val());

		});
	}

	setTimeout("cmsBuildOrder();", 1800);



	function CarrotCMSRemoveWidget(v) {
		$("#CMSremoveconfirm").dialog("destroy");
		
		$("#CMSremoveconfirm" ).dialog({
			open: function() {
				$(this).parents('.ui-dialog-buttonpane button:eq(0)').focus(); 
			},

			resizable: false,
			height: 250,
			width: 400,
			modal: true,
			buttons: {
					"No": function() {
						$( this ).dialog( "close" );
					},
					"Yes": function() {
						cmsRemoveWidget(v);
						$( this ).dialog( "close" );
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


	$(document).ready(function() {

		$('#cmsAdminToolbox').addFloating({
			targetLeft: 25,
			targetTop: 25,
			snap: true
		});

		$(".GlossySeaGreen input:button, .GlossySeaGreen input:submit").button();


		$(".cmsTargetArea").sortable({
			revert: true,
			dropOnEmpty: true,
			placeholder: "cmsHighlightPH GlossySeaGreen ui-state-highlight ui-corner-all",
			hoverClass: "cmsHighlightPH GlossySeaGreen ui-state-highlight ui-corner-all"
		});

		$("#cmsToolBox div.cmsToolItem").draggable({
			connectToSortable: ".cmsTargetArea",
			helper: "clone",
			revert: "invalid",
			handle: "p.cmsToolItem",
			placeholder: "cmsHighlightPH GlossySeaGreen ui-state-highlight ui-corner-all"
		});


		$(".cmsTargetArea").bind("sortupdate", function(event, ui) {
			var id = $(ui.item).attr('id');
			var val = $(ui.item).find('#cmsCtrlID').val();
			$("#cmsMovedItem").val(val);
			setTimeout("cmsBuildOrder();", 500);
			setTimeout("cmsUpdateWidgets();", 1800);
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
		setTimeout("cmsBuildOrder();", 500);
		setTimeout("cmsUpdateWidgets();", 1800);
	}


	function cmsShiftPosition(p) {
		floatingArray[0].targetTop = 30;
		floatingArray[0].targetBottom = undefined;
		if (p == 'L') {
			floatingArray[0].targetLeft = 30;
			floatingArray[0].targetRight = undefined;
		} else {
			floatingArray[0].targetLeft = undefined;
			floatingArray[0].targetRight = 30;
		}

		floatingArray[0].centerX = undefined;
		floatingArray[0].centerY = undefined;

	}	

	function cmsFixDialog(dialogname) {	
	
		var dilg = $("#"+dialogname).parent().parent();
		var txt = $("#"+dialogname).attr('style', 'margin: 2px; padding: 5px; text-align: left;');
			
		var c = $(dilg).attr('class');
		$(dilg).attr('class', "GlossySeaGreen ui-widget-content ui-corner-all " + c);

		var pstyle = $(dilg).attr('style');
		$(dilg).attr('style', "font-size: 12px; padding:2px; margin:2px; background: #ffffff; border: 1px solid #c0c0c0;" + pstyle);
		
		var cap = $(dilg).find(".ui-dialog-titlebar-close").attr('style', 'float:right; padding:4px; background-color:transparent;');
		var cap = $(dilg).find(".ui-dialog-titlebar").attr('style', 'margin: 2px; padding: 2px; height: 32px;');
		var cap = $(dilg).find(".ui-button").attr('style', 'float:right;');
		var cap = $(dilg).find(".ui-dialog-title").attr('style', 'float:left; margin:2px; padding:2px;');
		var cap = $(dilg).find(".ui-dialog-buttonpane").attr('style', 'margin:2px; padding:3px;');
		
		//var h = $(dilg).html();
		//$(dilg).html('');
		//$(dilg).html('<div class="GlossySeaGreen ui-widget-content ui-corner-all">' + h + '</div>');
	}
	
</script>

<%--<div id="divCMSBusy" style="position: absolute; top: 20px; left: 20px; width: 90%; height: 90%; z-index: 500;">
</div>--%>
<div id="cmsAdminToolbox" class="GlossySeaGreen ui-widget-content ui-corner-all" style="padding: 5px; margin: 5px; border: solid 2px #000;
	color: #000; background: #fff; position: absolute; top: 20px; left: 20px; min-width: 275px; min-height: 200px; z-index: 1000;">
	<div id="divCMSActive" style="z-index: 500; border: 0px solid #ddd;">
		<div onclick="cmsToggleMenu();" id="cmsMnuToggle" class='ui-icon ui-icon-minusthick' title="toggle" style="margin: 2px; float: right;">
			T
		</div>
		<div onclick="cmsShiftPosition('R')" id="cmsMnuRight" class='ui-icon ui-icon-circle-triangle-e' title="R" style="margin: 2px;
			float: right;">
			R
		</div>
		<div onclick="cmsShiftPosition('L')" id="cmsMnuLeft" class='ui-icon ui-icon-circle-triangle-w' title="L" style="margin: 2px;
			float: right;">
			L
		</div>
		<p class="ui-widget-header ui-corner-all" style="padding: 5px; margin: 0px; text-align: left;">
			Toolbox
		</p>
		<div class="ui-widget" runat="server" id="divEditing">
			<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px;">
				<p>
					<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
					<asp:Literal ID="litUser" runat="server">&nbsp</asp:Literal></p>
			</div>
		</div>
		<div style="display: none;" class="GlossySeaGreen">
			cmsFullOrder<br />
			<textarea rows="5" cols="30" id="cmsFullOrder" style="width: 310px; height: 50px;"></textarea><br />
			cmsMovedItem<br />
			<input type="text" id="cmsMovedItem" style="width: 310px;" /><br />
		</div>
		<div class="GlossySeaGreen" style="text-align: center; margin: 5px; padding: 5px;">
			<input runat="server" id="btnEditCoreInfo" type="button" value="Edit Core Page Info" style="text-align: center; margin: 5px;
				padding: 5px;" onclick="cmsShowEditPageInfo();" />
			<br />
			<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
			</asp:DropDownList>
			<br />
			<input runat="server" id="btnTemplate" type="button" value="Apply" style="text-align: center; margin: 5px; padding: 5px;"
				onclick="cmsUpdateTemplate();" />
		</div>
		<div class="GlossySeaGreen" style="text-align: center; margin: 5px; padding: 5px;">
			<input runat="server" id="btnToolboxSave" type="button" value="Save" style="text-align: center; margin: 5px; padding: 5px;"
				onclick="cmsApplyChanges();" />
			&nbsp;&nbsp;&nbsp;
			<input type="button" value="Cancel" style="text-align: center; margin: 5px; padding: 5px;" onclick="cmsCancelEdit();" />
		</div>
	</div>
	<div id="cmsMainToolbox">
		<br style="clear: none;" />
		<asp:Repeater ID="rpTools" runat="server">
			<HeaderTemplate>
				<div id="cmsToolBox" class="GlossySeaGreen ui-widget-content ui-corner-all" style="overflow: auto; height: 290px; width: 250px;
					padding: 5px; margin: 5px; float: left; border: solid 1px #000;">
			</HeaderTemplate>
			<ItemTemplate>
				<div class="cmsToolItem GlossySeaGreen ui-widget-content ui-corner-all" id="cmsToolItemDiv">
					<div id="cmsControl" class="GlossySeaGreen" style="min-height: 75px; min-width: 125px; padding: 2px; margin: 2px;">
						<p class="cmsToolItem GlossySeaGreen ui-widget-header ui-corner-all" style="cursor: move; clear: both; padding: 2px; margin: 2px;">
							<%# Eval("Caption")%>
						</p>
						<%# String.Format("{0}", Eval("FilePath")).Replace(".",". ") %><br />
						<input type="hidden" id="cmsCtrlID" size="35" value="<%# Eval("FilePath")%>" />
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
	<div id="cmsHeartBeat" style="clear: both; padding: 2px; margin: 2px;">
	</div>
</div>
<div style="display: none" class="GlossySeaGreen">
	<div id="CMSmodalalert" class="GlossySeaGreen" title="CMS Alert">
		<p id="CMSmodalalertmessage">
			&nbsp;</p>
	</div>
	<div id="CMSremoveconfirm" class="GlossySeaGreen" title="Remove Widget?">
		<p id="CMSremoveconfirmmsg">
			Are you sure you want to remove this widget?</p>
	</div>
	<div style="display: none">
		<img src="/manage/images/x.png" alt="x" />
	</div>
</div>

<script type="text/javascript">

	function cmsGenericEdit(PageId, WidgetId) {
		cmsLaunchWindow('/Manage/ControlPropertiesEdit.aspx?pageid=' + PageId + '&id=' + WidgetId);
	}

	function cmsLaunchWindow(theURL) {
		TheURL = theURL;
		$('#cmsModalFrame').html('<div id="divAjaxMain2"> <iframe scrolling="auto" id="frameEditor" frameborder="0" name="frameEditor" width="750" height="350" src="' + TheURL + '" /> </div>');

		$("#divAjaxMain2").block({ message: '<table><tr><td><img src="/Manage/images/Ring-64px-A7B2A0.gif"/></td></tr></table>',
			css: { width: '650px', height: '450px' },
			fadeOut: 1000,
			timeout: 1200,
			overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '0px solid #000000' }
		});

		setTimeout("cmsLoadWindow();", 800);
	}

	function cmsLoadWindow() {
		$("#cms-basic-modal-content").modal({ onClose: function(dialog) {
			//$.modal.close(); // must call this!
			setTimeout("$.modal.close();", 800);
			$('#cmsModalFrame').html('<div id="divAjaxMain"></div>');
			cmsDirtyPageRefresh();
		}
		});

		$('#cms-basic-modal-content').modal();
		return false;
	}

	function cmsDirtyPageRefresh() {
		window.setTimeout('location.href = \'<%=CurrentScriptName %>?carrotedit=true&carrottick=<%=DateTime.Now.Ticks.ToString() %>\'', 800);
	}

</script>

<div style="display: none">
	<div id="cms-basic-modal-content">
		<div id="cmsModalFrame">
			<%--<iframe scrolling="auto" id="frameEditor" frameborder="0" name="frameEditor" width="550" height="600" src="/Manage/blank.htm"></iframe>--%>
		</div>
	</div>
	<div style="display: none">
		<img src="/manage/images/x.png" alt="x" />
	</div>
</div>
