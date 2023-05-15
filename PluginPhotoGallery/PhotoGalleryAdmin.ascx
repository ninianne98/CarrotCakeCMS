﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdmin.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdmin" %>
<h2>Photo Gallery :
	<asp:Literal ID="litGalleryName" runat="server" /></h2>
<div id="imgWrapperMain" style="display: none;">
	<div style="padding: 5px; min-height: 20px; min-width: 20px;">
		<img alt="" id="imgThmbnail" src="/c3-admin/images/document.png" />
	</div>
</div>
<div style="clear: both">
</div>
<br />
<style type="text/css">
	#galleryTarget, #gallerySource {
		float: left;
		font-size: 11pt;
		list-style-type: none;
		margin: 0;
		margin-right: 10px;
		padding: 5px;
		min-height: 40px;
		min-width: 300px;
	}

		#galleryTarget li, #gallerySource li {
			font-size: 11pt;
			margin: 5px;
			padding: 5px;
			width: 350px;
			height: 76px;
		}

			#galleryTarget li img, #gallerySource li img {
				margin: 3px;
			}

	.fileInfo {
		display: block;
		float: left;
		padding: 0;
		margin: 5px;
		max-height: 72px;
		max-width: 240px;
		min-height: 10px;
		overflow: hidden;
	}

	#imgName {
		padding: 0;
		margin: 0;
		font-size: 0.85em;
		display: block;
		max-height: 45px;
		max-width: 225px;
		min-height: 10px;
		overflow: hidden;
		padding-bottom: 2px;
	}

	#imgThumb {
		margin: 0;
		padding: 2px 0;
		max-height: 64px;
		max-width: 64px;
	}

	div.galleryScroll {
		height: 400px;
		width: 420px;
		overflow: auto;
		border: 1px solid #666;
		padding: 2px;
		float: left;
		margin-right: 25px;
		margin-top: 10px;
		margin-bottom: 25px;
	}

	div.galleryScrollHead {
		padding: 2px;
		margin-right: 25px;
		margin-top: 5px;
		margin-bottom: 5px;
		width: 420px;
		float: left;
	}

		div.galleryScrollHead strong, div.galleryScrollHead b {
			font-size: 16px;
			font-weight: bold;
		}

	.HighlightPH {
		height: 25px !important;
		margin: 5px;
		padding: 5px;
		background: #FFFFAA !important;
		border: 2px dashed #676F6A !important;
	}

	#galleryTarget .icoDel {
		display: block;
		float: right;
		padding: 4px;
	}

	#gallerySource .icoDel {
		display: none;
	}

	#galleryTarget .editMetaData {
		display: block;
		float: right;
		padding: 4px;
	}

	#gallerySource .editMetaData {
		display: none;
	}

	#imgWrapperMain {
		display: block;
		min-height: 2px;
		min-width: 2px;
		width: 850px;
		padding: 8px;
		margin: 0px;
		position: absolute;
		z-index: 2000;
		text-align: center;
		margin: 10px auto;
	}

		#imgWrapperMain #imgThmbnail {
			min-height: 2px;
			min-width: 2px;
			max-height: 105px;
			width: auto;
		}

	.thumbpreview {
		display: block;
		min-height: 2px;
		min-width: 2px;
		padding: 8px;
		color: #000000;
		background-color: #B7D7C4;
		margin: 10px auto;
		text-align: center;
	}
</style>
<script type="text/javascript">
	function galleryAjaxJQuery() {
		if (typeof (Sys) != 'undefined') {
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			prm.add_endRequest(function () {
				updateGallery();
			});
		}
	}

	$(document).ready(function () {
		galleryAjaxJQuery();
		updateGallery();
	});

	var imgPreview = 'imgWrapperMain';

	function hideImg(obj) {
		var theNode = $(obj).parent();

		var theImgLayer = $('#' + imgPreview);
		$(theImgLayer).attr('style', 'display:none;');
		$(theImgLayer).attr('class', '');

		var img = $(theImgLayer).find('img');
		img.attr('src', '/c3-admin/images/document.png');
	}

	function showImg(obj) {
		var theNode = $(obj).parent();

		var key = $(theNode).find('#imgName').text();

		var newImgSrc = '/carrotwarethumb.axd?square=100&scale=true&thumb=' + encodeURIComponent(key);

		var theImgLayer = $('#' + imgPreview);
		$(theImgLayer).attr('style', '');
		$(theImgLayer).attr('class', 'thumbpreview ui-corner-all');

		var img = $(theImgLayer).find('img');
		img.attr('src', '/c3-admin/images/document.png');
		img.attr('src', newImgSrc);
	}

	function galleryOrder() {

		$('#srcGalleryCount').text($("#gallerySource").find('li').length);
		$('#tgtGalleryCount').text($("#galleryTarget").find('li').length);

		var OrderField = "<%=txtGalleryOrder.ClientID %>";
		$("#" + OrderField).val('');

		$("#galleryTarget").find('li').each(function (i) {

			var liImg = $(this);
			var id = liImg.attr('id');
			var key = liImg.find('#imgName').text();
			var img = liImg.find('#imgThumb').first();

			var newImgSrc = '/carrotwarethumb.axd?square=72&thumb=' + encodeURIComponent(key);
			$(img).attr('src', newImgSrc);

			var keys = (i + '\t' + key);

			$("#" + OrderField).val($("#" + OrderField).val() + '\r\n ' + keys);
		});
	}

	function updateGallery() {

		$(document).ready(function () {
			setTimeout("galleryOrder();", 800);

			$(function () {

				$("#galleryTarget").sortable({
					revert: true,
					dropOnEmpty: true,
					handle: "img",
					placeholder: "HighlightPH ui-state-highlight ui-corner-all",
					hoverClass: "HighlightPH ui-state-highlight ui-corner-all"
				});

				$("#gallerySource li").draggable({
					connectToSortable: "#galleryTarget",
					helper: "clone",
					revert: "invalid",
					handle: "img",
					placeholder: "HighlightPH ui-state-highlight ui-corner-all"
				});

				$("#galleryTarget").bind("sortupdate", function (event, ui) {
					setTimeout("galleryOrder();", 500);
				});

				$("#galleryTarget, #gallerySource").disableSelection();

				$("#galleryTarget a").enableSelection();
			});
		});
	}

	function clearFolderContents() {
		$("#galleryTarget").find('li').each(function (i) {
			$(this).remove();
		});

		galleryOrder();
	}

	function copyFolderContents() {
		var ulTgt = $("#galleryTarget");

		$("#gallerySource").find('li').each(function (i) {
			$(this).clone().appendTo(ulTgt);
		});

		galleryOrder();
	}

	function galleryRemoveItem(a) {
		var tgt = $(a);
		if (tgt.is("a")) {
			var p = $($(tgt).parent().parent().parent());
			var txt = p.find('#imgName');
			p.remove();
		}

		setTimeout("galleryOrder();", 500);
		return false;
	}
</script>
<br />
<table>
	<tr>
		<td style="width: 225px">
			<asp:CheckBox ID="chkFilter" runat="server" />
			Restrict images to +/- 14 days
		</td>
		<td style="width: 225px">
			<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" />
		</td>
		<td style="width: 100px">
			<asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:CheckBox ID="chkPath" runat="server" />
			Restrict images to selected folder
		</td>
		<td colspan="2">
			<asp:DropDownList ID="ddlFolders" runat="server" DataTextField="FileName" DataValueField="FolderPath" />
		</td>
	</tr>
</table>
<div style="clear: both">
</div>
<div style="width: 960px">
	<div class="galleryScrollHead">
		<b>Site Images (<span id="srcGalleryCount">0</span> items)</b> &nbsp;&nbsp;&nbsp;
		<input type="button" value="copy all" onclick="javascript: copyFolderContents()" />
	</div>
	<div class="galleryScrollHead">
		<b>Gallery Images (<span id="tgtGalleryCount">0</span> items)</b> &nbsp;&nbsp;&nbsp;
		<input type="button" value="clear all" onclick="javascript: clearFolderContents()" />
	</div>
	<div style="clear: both">
	</div>
	<div class="galleryScroll">
		<ul id="gallerySource" class='ui-state-default photoSource'>
			<asp:Repeater ID="rpFiles" runat="server">
				<ItemTemplate>
					<li class="ui-widget ui-widget-content" id="ID_0000000000">
						<img id="imgThumb" height="64" width="64" onmouseout="hideImg(this)" onmouseover="showImg(this)" style="float: left" src="<%# ResolveResourceFilePath( "PhotoIcon.png" ) %>"
							title="<%# Eval("FileName").ToString() %>" alt="<%# HttpUtility.UrlEncode( String.Format("{0}", Eval("FileName"))) %>" />
						<div class="fileInfo">
							<%# String.Format("<div id=\"imgName\">{0}</div>", Eval("FullFileName"))%>
							<%# String.Format("{0:d}", Eval("FileDate"))%>
							<%# String.Format("{0}", Eval("FileSizeFriendly"))%>
						</div>
						<div style="float: right; max-width: 32px; min-width: 16px;">
							<span class="icoDel ui-state-default ui-corner-all"><a href='javascript:void(0);' onclick='galleryRemoveItem(this);' title='Delete'><span class="ui-icon ui-icon-closethick"></span></a></span><a class="editMetaData" href="<%#CreatePopupLink("MetaDataEdit", "parm="+ EncodeBase64(Eval("FullFileName").ToString() ))%>">
								<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" />
							</a>
						</div>
					</li>
				</ItemTemplate>
			</asp:Repeater>
		</ul>
	</div>
	<div class="galleryScroll">
		<ul id="galleryTarget" class='ui-state-default photoTarget'>
			<asp:Repeater ID="rpGallery" runat="server">
				<ItemTemplate>
					<li class="ui-widget ui-widget-content" id="ID_0000000000">
						<img id="imgThumb" height="64" width="64" onmouseout="hideImg(this)" onmouseover="showImg(this)" style="float: left" src="/carrotwarethumb.axd?square=72&thumb=<%# HttpUtility.UrlEncode( String.Format("{0}", Eval("FullFileName")) )%>"
							title="<%# Eval("FileName").ToString() %>" alt="<%# HttpUtility.UrlEncode( String.Format("{0}", Eval("FileName"))) %>" />
						<div class="fileInfo">
							<%# String.Format("<div id=\"imgName\">{0}</div>", Eval("FullFileName"))%>
							<%# String.Format("{0:d}", Eval("FileDate"))%>
							<%# String.Format("{0}", Eval("FileSizeFriendly"))%>
						</div>
						<div style="float: right; max-width: 32px; min-width: 16px;">
							<span class="icoDel ui-state-default ui-corner-all"><a href='javascript:void(0);' onclick='galleryRemoveItem(this);' title='Delete'><span class="ui-icon ui-icon-closethick"></span></a></span><a class="editMetaData" href="<%#CreatePopupLink("MetaDataEdit", "parm="+ EncodeBase64(Eval("FullFileName").ToString() ))%>">
								<img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" />
							</a>
						</div>
					</li>
				</ItemTemplate>
			</asp:Repeater>
		</ul>
	</div>
	<div style="clear: both">
	</div>
</div>
<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
<div style="display: none">
	<asp:TextBox ID="txtGalleryOrder" TextMode="MultiLine" Rows="8" Columns="60" runat="server" />
</div>