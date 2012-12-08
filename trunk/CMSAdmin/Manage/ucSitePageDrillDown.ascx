<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSitePageDrillDown.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucSitePageDrillDown" %>
<script src="/Manage/Includes/common-utils.js" type="text/javascript"></script>
<style type="text/css">
	div.scroll {
		height: 90px;
		width: 340px;
		overflow: auto;
		padding: 2px;
		position: absolute;
		z-index: 999;
	}
	div.scrollcontainer {
		height: 100px;
		width: 350px;
		padding: 4px;
		position: absolute;
		z-index: 999;
	}
	div.scrollcontainerhide {
		display: none;
	}
	div.scrollcontainerheight {
		height: 5px;
		width: 75px;
	}
	div.menuitems {
		margin-top: 2px;
		margin-left: 2px;
		padding: 6px;
	}
	div.menuitems {
		padding: 4px;
	}
</style>
<script type="text/javascript">

	var thisPageID = '<%=RootContentID.ToString() %>';

	var webSvc = "/Manage/CMS.asmx";

	var thePage = '';

	var menuOuter = 'menuitemsouter';
	var menuInner = 'menuitemsinner';
	var menuPath = 'menupath';
	var menuValue = '<%=txtParent.ClientID %>';

	var bMoused = false;


	function UpdateAjaxLoadDrillMenu() {
		if (typeof (Sys) != 'undefined') {
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			prm.add_endRequest(function () {
				setTimeout("resetDrill();", 200);
				setTimeout("AjaxLoadDrillMenu();", 600);
			});
		}
	}

	$(document).ready(function () {
		UpdateAjaxLoadDrillMenu();
		setTimeout("AjaxLoadDrillMenu();", 500);
	});


	function AjaxLoadDrillMenu() {
		mouseNode();
		getCrumbs();

		setTimeout("hideMnu();", 250);
		setTimeout("hideMnu();", 500);

		$('#' + menuOuter).bind("mouseenter", function (e) {
			showMnu();
		});
		$('#' + menuOuter).bind("mouseleave", function (e) {
			hideMnu();
		});

		//alert("AjaxLoadDrillMenu");
	}



	function getCrumbs() {
		var webMthd = webSvc + "/GetPageCrumbs";
		var myVal = $('#' + menuValue).val();

		$.ajax({
			type: "POST",
			url: webMthd,
			data: "{'PageID': '" + myVal + "', 'CurrPageID': '" + thisPageID + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: ajaxReturnCrumb,
			error: cmsAjaxFailed
		});

	}

	function ajaxReturnCrumb(data, status) {

		var lstData = data.d;
		var val = '';
		var mnuName = '#' + menuPath;

		$(mnuName).text('');

		if (lstData.length > 0) {
			$.each(lstData, function (i, v) {
				var del = "<a href='javascript:void(0);' title='Remove' thevalue='" + val + "' onclick='selectDrillItem(this);'><div class='ui-icon ui-icon-closethick' style='float:left'></div></a>";
				if (i != (lstData.length - 1)) {
					del = '';
				}
				val = v.Root_ContentID;
				var bc = "<div class='pageNodeDrillDown2' thevalue='" + v.Root_ContentID + "' id='node' >" + v.NavMenuText + " </div>";
				$(mnuName).append("<div class='ui-widget-header ui-corner-all pageNodeDrillDown3' >" + bc + del + "<div  style='clear: both;'></div></div>");
			});
		}

		makeMenuClickable();
	}


	var bMoused = false;

	function makeMenuClickable() {
		$('#PageContents a').each(function (i) {
			$(this).click(function () {
				cmsMakeOKToLeave();
				setTimeout("cmsMakeNotOKToLeave();", 500);
			});
		});
	}

	function hideMnu() {
		bHidden = true;
		$('#' + menuInner).attr('class', 'scroll scrollcontainerhide');
		$('#' + menuOuter).attr('class', 'scrollcontainer scrollcontainerheight');
	}

	var bHidden = true;
	function showMnu() {
		if (bHidden == true) {
			$('#' + menuInner).attr('class', 'scroll');
			$('#' + menuOuter).attr('class', 'scrollcontainer ui-widget ui-widget-content ui-corner-all');
			bHidden = false;
		}
	}

	function mouseNode() {
		var webMthd = webSvc + "/GetChildPages";
		var myVal = $('#' + menuValue).val();

		showMnu();

		if (bMoused != true) {

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'PageID': '" + myVal + "', 'CurrPageID': '" + thisPageID + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: ajaxReturnNode,
				error: cmsAjaxFailed
			});
		}
	}




	function ajaxReturnNode(data, status) {

		var lstData = data.d;

		var mnuName = '#' + menuInner;

		$(mnuName).html('');
		hideMnu();

		$.each(lstData, function (i, v) {
			//$('#returneditems').append(new Option(v.PageFile, v.Root_ContentID));
			$(mnuName).append("<div><a href='javascript:void(0);' onclick='selectDrillItem(this);' thevalue='" + v.Root_ContentID + "' id='node' >" + v.NavMenuText + "</a></div>");
		});

		showMnu();
		if ($(mnuName).text().length < 2) {
			$(mnuName).append("<div><b>No Data</b></div>");
		}

		makeMenuClickable();

		bMoused = true;
	}

	function selectDrillItem(a) {
		bMoused = false;

		var tgt = $(a);

		$('#' + menuValue).val(tgt.attr('thevalue'));

		mouseNode();
		getCrumbs();
	}

	function resetDrill() {
		bMoused = false;

		//$('#' + menuValue).val('');
		//$('#' + menuValue).val('<%=SelectedPage.ToString() %>');

		mouseNode();
		getCrumbs();

		//alert("resetDrill");
	}


</script>
<asp:TextBox Style="display: none" runat="server" ID="txtParent" />
<div style="float: left;">
	<div id="menupath" class="pageNodeDrillDown1">
	</div>
	<div class="pageNodeDrillDown5">
		<div id="menuhead" onmouseout="hideMnu()" onmouseover="mouseNode()" class="menuitems pageNodeDrillDown4 ui-widget-header ui-corner-all">
			<div class="pageNodeDrillDown6">
				Pages <a title="Reset Path" href='javascript:void(0);' onclick='selectDrillItem(this);' thevalue=''><span style="float: right;" class="ui-icon ui-icon-power"></span>
				</a>
			</div>
		</div>
		<div id="menuitemsouter">
			<div id="menuitemsinner" class="scroll">
			</div>
		</div>
	</div>
</div>
