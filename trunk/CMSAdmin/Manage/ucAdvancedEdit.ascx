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
	.HighlightPH {
		height: 25px !important;
		margin: 5px;
		padding: 5px;
		border: 2px dashed #676F6A;
	}
</style>

<script type="text/javascript">
	var webSvc = "/Manage/CMS.asmx";

	var thisPage = '<%=CurrentScriptName %>';
	
	var thisPageID = '<%=guidContentID.ToString() %>';

	function EscapeFile() {
		thisPage = MakeStringSafe(thisPage);
	}

	$(document).ready(function() {
		EscapeFile();
	});

	function MakeStringSafe(val) {
		val = Base64.encode(val);
		return val;
	}

	function EditHB() {
		setTimeout("EditHB();", 30 * 1000);

		var webMthd = webSvc + "/RecordHeartbeat";

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'PageID': '<%=guidContentID %>'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: updateHeartbeat,
			error: ajaxFailed
		});
	}


	function cmsSaveContent(val, zone) {

		var webMthd = webSvc + "/CacheContentZoneText";

		val = MakeStringSafe(val);
		zone = MakeStringSafe(zone);

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ZoneText': '" + val + "', 'Zone': '" + zone + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: saveContentCallback,
			error: ajaxFailed
		});
	}


	function cmsSaveGenericContent(val, key) {

		var webMthd = webSvc + "/CacheGenericContent";

		val = MakeStringSafe(val);

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'ZoneText': '" + val + "', 'DBKey': '" + key + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: saveContentCallback,
			error: ajaxFailed
		});
	}


	function cmsUpdateWidgets() {

		var webMthd = webSvc + "/CacheWidgetUpdate";

		var val = $("#fullorder").val();

		val = MakeStringSafe(val);

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'WidgetAddition': '" + val + "', 'ThisPage': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: saveWidgetsCallback,
			error: ajaxFailed
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
			success: saveWidgetsCallback,
			error: ajaxFailed
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
			success: savePageCallback,
			error: ajaxFailed
		});

	}

<% if (!bLocked) { %>
	setTimeout("EditHB();", 3000);
<%} else { %>
	$(document).ready(function() {
		alertModal("<%=litUser.Text %>");
	});
<%} %>

	function updateHeartbeat(data, status) {
		var hb = $('#heatbeat');
		hb.empty().append('HB:  ');
		hb.append(data.d);
		CMSBusyShort();
	}

	function saveContentCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
			window.setTimeout('location.reload()', 800);
		} else {
			alertModal(data.d);
		}
	}

	function saveWidgetsCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
			window.setTimeout('location.reload()', 800);
		} else {
			alertModal(data.d);
		}
	}

	function savePageCallback(data, status) {
		if (data.d == "OK") {
			CMSBusyShort();
			alertModal("Saved");
			window.setTimeout('location.href = \'<%=CurrentScriptName %>\'', 5000);
		} else {
			alertModal(data.d);
		}
	}


	function cancelEdit() {
		window.setTimeout('location.href = \'<%=CurrentScriptName %>\'', 1000);
	}


//	$(document).ready(function() {
//		alertModal("hello!");
//	});


	function ajaxFailed(request) {
		var s = "";
		s = s + "<b>status: </b>" + request.status + '<br />\r\n';
		s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
		s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
		alertModal(s);
	}

	function alertModal(request) {
		$("#modalalert").dialog("destroy");

		$("#modalalert").dialog({
			//autoOpen: false,
			height: 400,
			width: 600,
			modal: true
		});

		$("#modalalertmessage").html(request);
		
		var c = $("#modalalertmessage").parent().parent().attr('class');
		$("#modalalertmessage").parent().parent().attr('class', "GlossySeaGreen " + c);
		
		var txt = $("#modalalertmessage").attr('style', 'padding:8px;');
		
		var lnk = $("#modalalertmessage").parent().parent().find(".ui-dialog-titlebar-close").attr('style', 'float:right; padding:5px;');
		var cap = $("#modalalertmessage").parent().parent().find(".ui-dialog-title").attr('style', 'float:left; padding:5px;');
		//alert($(lnk).attr('class'));
		
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
	function ToggleMenu() {
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


	var trash_icon = "<div id='cmsDelIconDiv' style='text-align:right;padding:5px;margin:2px;'><a id='delico' class='ui-icon ui-icon-trash' href='javascript:void(0);' onclick='RemoveItem(this);' title='Delete'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></div>";


	function BuildOrder() {
		CMSBusyShort();

		$("#fullorder").val('');
		$(".cmsTargetArea").find('#ctrlOrder').each(function(i) {
			var txt = $(this);
			$(txt).val('');
			var p = txt.parent().parent().parent().attr('id');
			var key = txt.parent().find('#ctrlID').val();
			txt.val(i + '\t' + p + '\t' + key);
			//alert($('#'+p).find('#ctrlID').val());

			$("#fullorder").val($("#fullorder").val() + '\r\n ' + txt.val());

		});
	}

	setTimeout("BuildOrder();", 1800);



	function CarrotCMSRemoveWidget(v) {
		cmsRemoveWidget(v);
//		var p = $('#' + v);
//		var txt = p.find('#ctrlOrder');
//		txt.val('-1');
//		delItem(p);
		//alert(v);
	}


	function RemoveItem(a) {
		var tgt = $(a);
		//alert(tgt.val());
		if (tgt.is("a.ui-icon-trash")) {
			var p = $($(tgt).parent().parent().parent().parent());
			//alert(p.attr('id'));
			var txt = p.find('#ctrlOrder');
			txt.val('-1');
			delItem(p);
		}
		return false;
	}


	function setOrder(fld) {
		var id = $(fld).attr('id');
		$("#moveditem").val(id);
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
			placeholder: "HighlightPH GlossySeaGreen ui-state-highlight ui-corner-all",
			hoverClass: "HighlightPH GlossySeaGreen ui-state-highlight ui-corner-all"
		});

		$("#toolbox div.toolitem").draggable({
			connectToSortable: ".cmsTargetArea",
			helper: "clone",
			revert: "invalid",
			handle: "p.toolitem",
			placeholder: "HighlightPH GlossySeaGreen ui-state-highlight ui-corner-all"
		});


		$(".cmsTargetArea").bind("sortupdate", function(event, ui) {
			var id = $(ui.item).attr('id');
			var val = $(ui.item).find('#ctrlID').val();
			$("#moveditem").val(val);
			setTimeout("BuildOrder();", 500);
			setTimeout("cmsUpdateWidgets();", 1800);
		});

		$("div#toolbox").disableSelection();
		$("#cmsContentArea a").enableSelection();
		$("#cmsWidgetHead a").enableSelection();

	});

	function SetVal(u) {
		var id = $(u).attr('id');
		var val = $(u).find('#ctrlOrder').val();
		//alert(val);
		$("#moveditem").val(val);
		setTimeout("BuildOrder();", 500);
	}


	function delItem(item) {
		item.appendTo("#trashlist");
		setTimeout("BuildOrder();", 500);
		setTimeout("cmsUpdateWidgets();", 1800);
	}


	function ShiftPosition(p) {
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

	
	
</script>

<%--<div id="divCMSBusy" style="position: absolute; top: 20px; left: 20px; width: 90%; height: 90%; z-index: 500;">
</div>--%>
<div id="cmsAdminToolbox" class="GlossySeaGreen ui-widget-content ui-corner-all" style="padding: 5px; margin: 5px; border: solid 2px #000;
	color: #000; background: #fff; position: absolute; top: 20px; left: 20px; min-width: 275px; min-height: 200px; z-index: 1000;">
	<div id="divCMSActive" style="z-index: 500; border: 0px solid #ddd;">
		<div onclick="ToggleMenu();" id="cmsMnuToggle" class='ui-icon ui-icon-minusthick' title="toggle" style="margin: 2px; float: right;">
			T
		</div>
		<div onclick="ShiftPosition('R')" id="cmsMnuRight" class='ui-icon ui-icon-circle-triangle-e' title="R" style="margin: 2px;
			float: right;">
			R
		</div>
		<div onclick="ShiftPosition('L')" id="cmsMnuLeft" class='ui-icon ui-icon-circle-triangle-w' title="L" style="margin: 2px;
			float: right;">
			L
		</div>
		<p class="toolboxmenu ui-widget-header ui-corner-all" style="padding: 5px; margin: 0px;">
			Toolbox
		</p>
		<br />
		<div class="ui-widget" runat="server" id="divEditing">
			<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px;">
				<p>
					<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
					<asp:Literal ID="litUser" runat="server">&nbsp</asp:Literal></p>
			</div>
		</div>
		<br />
		<div style="display: none;" class="GlossySeaGreen">
			fullorder<br />
			<textarea rows="5" cols="30" id="fullorder" style="width: 310px; height: 50px;"></textarea><br />
			moveditem<br />
			<input type="text" id="moveditem" style="width: 310px;" /><br />
		</div>
		<div class="GlossySeaGreen" style="text-align: center; margin: 5px; padding: 5px;">
			<br />
			<%--<asp:Button ID="btnSave" class="GlossySeaGreen" runat="server" Text="Save" Style="margin: 2px; padding: 2px;" OnClick="btnSave_Click" />--%>
			<input runat="server" id="btnToolboxSave" type="button" value="Save" style="text-align: center; margin: 5px; padding: 5px;"
				onclick="cmsApplyChanges();" />
			&nbsp;&nbsp;&nbsp;
			<input type="button" value="Cancel" style="text-align: center; margin: 5px; padding: 5px;" onclick="cancelEdit();" />
		</div>
	</div>
	<div id="cmsMainToolbox">
		<br style="clear: none;" />
		<asp:Repeater ID="rpTools" runat="server">
			<HeaderTemplate>
				<div id="toolbox" class="GlossySeaGreen ui-widget-content ui-corner-all" style="overflow: auto; height: 290px; width: 250px;
					padding: 5px; margin: 5px; float: left; border: solid 1px #000;">
			</HeaderTemplate>
			<ItemTemplate>
				<div class="toolitem GlossySeaGreen ui-widget-content ui-corner-all" id="toolItem">
					<div id="cmsControl" class="GlossySeaGreen" style="min-height: 75px; min-width: 125px; padding: 2px; margin: 2px;">
						<p class="toolitem GlossySeaGreen ui-widget-header ui-corner-all" style="cursor: move; clear: both; padding: 2px; margin: 2px;">
							<%# Eval("Caption")%>
						</p>
						<%# String.Format("{0}", Eval("FilePath")).Replace(".",". ") %><br />
						<input type="hidden" id="ctrlID" size="35" value="<%# Eval("FilePath")%>" />
						<input type="hidden" id="ctrlOrder" value="0" />
						<div style="text-align: right;" id="ctrlBtn">
						</div>
					</div>
				</div>
			</ItemTemplate>
			<FooterTemplate>
				</div></FooterTemplate>
		</asp:Repeater>
		<div id="trashlist" style="clear: both; display: none; width: 300px; height: 100px; overflow: auto; float: left; border: solid 1px #ccc;
			background-color: #000;">
		</div>
	</div>
	<div id="heatbeat" style="clear: both; padding: 2px; margin: 2px;">
	</div>
</div>
<div style="display: none" class="GlossySeaGreen">
	<div id="modalalert" class="GlossySeaGreen" title="CMS Alert">
		<p id="modalalertmessage">
			&nbsp;</p>
	</div>
	<div style="display: none">
		<img src="/manage/images/x.png" />
	</div>
	<asp:TextBox Style="height: 280px; width: 730px;" ID="txtCenter" runat="server" TextMode="MultiLine" Rows="15" Columns="80"></asp:TextBox>
	<asp:TextBox Style="height: 280px; width: 730px;" ID="txtLeft" runat="server" TextMode="MultiLine" Rows="15" Columns="80"></asp:TextBox>
	<asp:TextBox Style="height: 280px; width: 730px;" ID="txtRight" runat="server" TextMode="MultiLine" Rows="15" Columns="80"></asp:TextBox>
	<div id="cmsAdminEditCenter" class="GlossySeaGreen" style="height: 320px; width: 730px; border: none 0px #000;">
		<div class="GlossySeaGreen ui-widget-header ui-corner-all" style="padding: 8px; margin: 0px;">
			Edit Center</div>
		<asp:TextBox Style="height: 280px; width: 730px;" CssClass="mceEditor2" ID="reBody" runat="server" TextMode="MultiLine" Rows="15"
			Columns="80"></asp:TextBox><br />
		<input class="GlossySeaGreen" onclick="clickCenterBtn()" type="button" style="margin: 2px; padding: 2px;" id="btnCenterDone"
			value="Done" />
	</div>
	<div id="cmsAdminEditLeft" class="GlossySeaGreen" style="height: 300px; width: 730px; border: none 0px #000;">
		<div class="GlossySeaGreen ui-widget-header ui-corner-all" style="padding: 8px; margin: 0px;">
			Edit Left</div>
		<asp:TextBox Style="height: 280px; width: 730px;" CssClass="mceEditor2" ID="reLeftBody" runat="server" TextMode="MultiLine"
			Rows="15" Columns="80"></asp:TextBox><br />
		<input class="GlossySeaGreen" onclick="clickLeftBtn()" type="button" style="margin: 2px; padding: 2px;" id="btnLeftDone"
			value="Done" />
	</div>
	<div id="cmsAdminEditRight" class="GlossySeaGreen" style="height: 300px; width: 730px; border: none 0px #000;">
		<div class="GlossySeaGreen ui-widget-header ui-corner-all" style="padding: 8px; margin: 0px;">
			Edit Right</div>
		<asp:TextBox Style="height: 280px; width: 730px;" CssClass="mceEditor2" ID="reRightBody" runat="server" TextMode="MultiLine"
			Rows="15" Columns="80"></asp:TextBox><br />
		<input class="GlossySeaGreen" onclick="clickRightBtn()" type="button" style="margin: 2px; padding: 2px;" id="btnRightDone"
			value="Done" />
	</div>
</div>

<script type="text/javascript">

	function cmsGenericEdit(PageId, WidgetId) {
		cmsLaunchWindow('/Manage/ControlPropertiesEdit.aspx?pageid=' + PageId + '&id=' + WidgetId);
	}


	function cmsLaunchWindow(theURL) {
		TheURL = theURL;
		$('#cmsModalFrame').html('<div id="divAjaxMain2"> <iframe scrolling="auto" id="frameEditor" frameborder="0" name="frameEditor" width="690" height="475" src="' + TheURL + '" /> </div>');

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
		window.setTimeout('location.href = \'<%=CurrentScriptName %>?carrotedit=true\'', 800);
	}


</script>

<div style="display: none">
	<div id="cms-basic-modal-content">
		<div id="cmsModalFrame">
			<%--<iframe scrolling="auto" id="frameEditor" frameborder="0" name="frameEditor" width="550" height="600" src="/Manage/blank.htm"></iframe>--%>
		</div>
	</div>
	<div style="display: none">
		<img src="/manage/images/x.png" />
	</div>
</div>

<script type="text/javascript">

	function clickCenterBtn() {
		tinyMCE.triggerSave();
		var ed = tinyMCE.get('<%=reBody.ClientID%>');
		var d = ed.getContent();

		//$('#cmsAdmin_BodyCenter').html(d);
		//$('#<%=txtCenter.ClientID%>').val(d);

		tinyMCE.execCommand("mceRemoveControl", false, '<%=reBody.ClientID%>');
		setTimeout("$.modal.close();", 500);
		CMSBusyShort();
		cmsSaveContent(d, 'c');
	}

	function clickLeftBtn() {
		tinyMCE.triggerSave();
		var ed = tinyMCE.get('<%=reLeftBody.ClientID%>');
		var d = ed.getContent();

		//$('#<%=txtLeft.ClientID%>').val(d);
		//$('#cmsAdmin_BodyLeft').html(d);

		tinyMCE.execCommand("mceRemoveControl", false, '<%=reLeftBody.ClientID%>');
		setTimeout("$.modal.close();", 800);
		CMSBusyShort();
		cmsSaveContent(d, 'l');
	}

	function clickRightBtn() {
		tinyMCE.triggerSave();
		var ed = tinyMCE.get('<%=reRightBody.ClientID%>');
		var d = ed.getContent();

		//$('#<%=txtRight.ClientID%>').val(d);
		//$('#cmsAdmin_BodyRight').html(d);

		tinyMCE.execCommand("mceRemoveControl", false, '<%=reRightBody.ClientID%>');
		setTimeout("$.modal.close();", 800);
		CMSBusyShort();
		cmsSaveContent(d, 'r');
	}


	function CarrotCMSEdit(f, v) {
		//alert(f + ' -- ' + v);
		if (f == 'c') {
			ClickCenterEdit();
		}
		if (f == 'l') {
			ClickLeftEdit();
		}
		if (f == 'r') {
			ClickRightEdit();
		}

	}



	function ClickCenterEdit() {
		$("#cmsAdminEditCenter").modal({
			onShow: function(dialog) {
				$(".GlossySeaGreen input:button, .GlossySeaGreen input:submit").button();

				var v = $('#<%=txtCenter.ClientID%>').val();
				$('#<%=reBody.ClientID%>').val(v);

				setTimeout("initTinyMCE();", 200);
				setTimeout("tinyMCE.execCommand('mceRemoveControl', false, '<%=reBody.ClientID%>');", 400);
				setTimeout("tinyMCE.execCommand('mceAddControl', true, '<%=reBody.ClientID%>');", 600);
				setTimeout("tinyMCE.triggerSave();", 800);
			},
			onClose: function(dialog) {
				tinyMCE.execCommand("mceRemoveControl", false, '<%=reBody.ClientID%>');
				$.modal.close();
			}
		});
		$('#cmsAdminEditCenter').modal();
		return false;
	}


	function ClickLeftEdit() {
		$("#cmsAdminEditLeft").modal({
			onShow: function(dialog) {
				$(".GlossySeaGreen input:button, .GlossySeaGreen input:submit").button();

				var v = $('#<%=txtLeft.ClientID%>').val();
				$('#<%=reLeftBody.ClientID%>').val(v);

				setTimeout("initTinyMCE();", 200);
				setTimeout("tinyMCE.execCommand('mceRemoveControl', false, '<%=reLeftBody.ClientID%>');", 400);
				setTimeout("tinyMCE.execCommand('mceAddControl', true, '<%=reLeftBody.ClientID%>');", 600);
				setTimeout("tinyMCE.triggerSave();", 800);
			},
			onClose: function(dialog) {
				tinyMCE.execCommand("mceRemoveControl", false, '<%=reLeftBody.ClientID%>');
				$.modal.close();
			}
		});
		$('#cmsAdminEditLeft').modal();
		return false;
	}


	function ClickRightEdit() {
		$("#cmsAdminEditRight").modal({
			onShow: function(dialog) {
				$(".GlossySeaGreen input:button, .GlossySeaGreen input:submit").button();

				var v = $('#<%=txtRight.ClientID%>').val();
				$('#<%=reRightBody.ClientID%>').val(v);

				setTimeout("initTinyMCE();", 200);
				setTimeout("tinyMCE.execCommand('mceRemoveControl', false, '<%=reRightBody.ClientID%>');", 400);
				setTimeout("tinyMCE.execCommand('mceAddControl', true, '<%=reRightBody.ClientID%>');", 600);
				setTimeout("tinyMCE.triggerSave();", 800);
			},
			onClose: function(dialog) {
				tinyMCE.execCommand("mceRemoveControl", false, '<%=reRightBody.ClientID%>');
				$.modal.close();
			}
		});
		$('#cmsAdminEditRight').modal();
		return false;
	}


</script>

<!-- TinyMCE -->

<script type="text/javascript" src="/Manage/tiny_mce/tiny_mce.js"></script>

<script type="text/javascript">
	//initTinyMCE();

	function initTinyMCE() {

		tinyMCE.init({

			//		mode : "textareas",
			//		theme : "advanced",
			//		theme_advanced_toolbar_location : "top",
			//		theme_advanced_toolbar_align : "left",
			//		document_base_url : "http://www.site.com/path1/"

			mode: "textareas",
			theme: "advanced",
			editor_selector: "mceEditor",
			theme_advanced_source_editor_width: "700",
			theme_advanced_source_editor_height: "500",
			skin: "o2k7",
			skin_variant: "silver",
			plugins: "advimage,advlink,advlist,media,iespell,inlinepopups",
			file_browser_callback: "fileBrowserCallback",
			theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,forecolor,|,outdent,indent,blockquote,|,bullist,numlist,|,link,unlink,anchor,image,media,|,fileupbtn,cleanup,code,help",
			theme_advanced_buttons2: "",
			theme_advanced_buttons3: "",
			theme_advanced_toolbar_location: "top",
			theme_advanced_toolbar_align: "left",
			theme_advanced_statusbar_location: "bottom",
			relative_urls: false,
			remove_script_host: true,
			content_css: "/Manage/Includes/richedit.css",

			// Add a custom button
			setup: function(ed) {
				ed.addButton('fileupbtn', {
					title: 'FileUpload',
					image: '/Manage/tiny_mce/insertfile.gif',
					onclick: function() {
						//debugger;
						ed.focus();
						//ed.selection.setContent('custom text');
						var x = fileBrowserCallback(ed, '', '', this);
						//ed.selection.setContent(x);
					}
				});
			}
		});
	}


	//http://wiki.moxiecode.com/index.php/TinyMCE:Custom_filebrowser
	function fileBrowserCallback(field_name, url, type, win) {
		var sURL = "/Manage/FileBrowser.aspx?useTiny=1&fldrpath=/Media/";

		// block multiple file browser windows
		if (!tinyMCE.selectedInstance.fileBrowserAlreadyOpen) {
			tinyMCE.selectedInstance.fileBrowserAlreadyOpen = true; // but now it will be

			tinyMCE.activeEditor.windowManager.open({
				file: sURL,
				title: 'File Browser',
				width: 700,
				height: 500,
				resizable: "no",
				scrollbars: "yes",
				status: "yes",
				inline: "yes",
				close_previous: "yes"
			}, {
				window: win,
				input: field_name
			});
		}

		return false;
	}


	function toggleEditor(id) {
		if (!tinyMCE.get(id))
			tinyMCE.execCommand('mceAddControl', false, id);
		else
			tinyMCE.execCommand('mceRemoveControl', false, id);
	}
	
</script>

<script type="text/javascript">
	var fldName = '';
	var winBrowse = null;
	function FileBrowserOpen(fldN) {
		fldN = '#' + fldN;
		var fld = $(fldN);
		fldName = fld.attr('id');

		if (winBrowse != null) {
			winBrowse.close();
		}
		winBrowse = window.open('/Manage/FileBrowser.aspx?useTiny=0&fldrpath=/Media/', '_winBrowse', 'resizable=yes,location=no,menubar=no,scrollbars=yes,status=yes,toolbar=no,fullscreen=no,dependent=yes,width=650,height=500,left=50,top=50');
		return false;
	}

	function fnSetFile(v) {
		var fldN = '#' + fldName;
		var fld = $(fldN);
		fld.val(v);

		winBrowse.close();
		winBrowse = null;
	}
	
</script>

<!-- /TinyMCE -->
