<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdmin.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdmin" %>
<h2>
	Photo Gallery :
	<asp:Literal ID="litGalleryName" runat="server"></asp:Literal></h2>
<br />
<style type="text/css">
	#galleryTarget, #gallerySource {
		list-style-type: none;
		margin: 0;
		padding: 0;
		float: left;
		margin-right: 10px;
		padding: 5px;
		min-height: 40px;
		min-width: 300px;
	}
	#galleryTarget li, #gallerySource li {
		margin: 5px;
		padding: 5px;
		font-size: 11px;
		width: 350px;
		height: 50px;
	}
	#galleryTarget li img, #gallerySource li img {
		margin: 3px;
	}
	.fileInfo {
		margin: 8px;
	}
	div.galleryScroll {
		height: 300px;
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
	
	
	
	.inputFields {
		display: none;
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


	function galleryOrder() {
		var OrderField = "<%=txtGalleryOrder.ClientID %>";
		$("#" + OrderField).val('');

		$("#galleryTarget").find('li').each(function (i) {

			var liImg = $(this);
			var id = liImg.attr('id');
			var key = liImg.find('#imgName').text();

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

	function galleryRemoveItem(a) {
		var tgt = $(a);
		if (tgt.is("a")) {
			var p = $($(tgt).parent().parent());
			//alert(p.attr('id'));
			var txt = p.find('#imgName');
			p.remove();
		}

		//alert("clicked");
		setTimeout("galleryOrder();", 500);
		return false;
	}
	
</script>
<br />
<div style="float: left; width: 275px">
	<asp:CheckBox ID="chkFilter" runat="server" />
	Restrict Site Images to +/- 14 days
</div>
<div style="float: left; width: 175px">
	<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" />
</div>
<div style="float: left; width: 125px">
	<asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" />
</div>
<div style="clear: both">
</div>
<div style="width: 960px">
	<div class="galleryScrollHead">
		Site Images</div>
	<div class="galleryScrollHead">
		Gallery Images</div>
	<div style="clear: both">
	</div>
	<div class="galleryScroll">
		<ul id="gallerySource" class='ui-state-default photoSource'>
			<asp:Repeater ID="rpFiles" runat="server">
				<ItemTemplate>
					<li class="ui-widget ui-widget-content" id="ID_0000000000">
						<img height="40" width="40" style="float: left" src="/carrotwarethumb.axd?square=50&thumb=<%# HttpUtility.UrlEncode( String.Format("{0}{1}", Eval("FolderPath"), Eval("FileName")) )%>"
							alt="<%# HttpUtility.UrlEncode( String.Format("{0}", Eval("FileName"))) %>" />
						<div style="float: left; max-width: 250px" class="fileInfo">
							<%# String.Format("<span id=\"imgName\">{0}{1}</span>", Eval("FolderPath"), Eval("FileName"))%>
							<br />
							<%# String.Format("{0:d}", Eval("FileDate"))%>
							<%# String.Format("{0}", Eval("FileSizeFriendly"))%>
						</div>
						<span class="icoDel ui-state-default ui-corner-all"><a href='javascript:void(0);' onclick='galleryRemoveItem(this);' title='Delete'><span class="ui-icon ui-icon-closethick">
						</span></a></span><a class="editMetaData" href="<%#CreatePopupLink("MetaDataEdit", "parm="+ Carrotware.CMS.Core.CMSConfigHelper.EncodeBase64(Eval("FolderPath").ToString() + Eval("FileName").ToString()))%>">
							<img class="imgNoBorder" src="/Manage/images/pencil.png" alt="Edit" title="Edit" />
						</a></li>
				</ItemTemplate>
			</asp:Repeater>
		</ul>
	</div>
	<div class="galleryScroll">
		<ul id="galleryTarget" class='ui-state-default photoTarget'>
			<asp:Repeater ID="rpGallery" runat="server">
				<ItemTemplate>
					<li class="ui-widget ui-widget-content" id="ID_0000000000">
						<img height="40" width="40" style="float: left" src="/carrotwarethumb.axd?square=50&thumb=<%# HttpUtility.UrlEncode( String.Format("{0}{1}", Eval("FolderPath"), Eval("FileName")) )%>"
							alt="<%# HttpUtility.UrlEncode( String.Format("{0}", Eval("FileName"))) %>" />
						<div style="float: left; max-width: 250px" class="fileInfo">
							<%# String.Format("<span id=\"imgName\">{0}{1}</span>", Eval("FolderPath"), Eval("FileName"))%>
							<br />
							<%# String.Format("{0:d}", Eval("FileDate"))%>
							<%# String.Format("{0}", Eval("FileSizeFriendly"))%>
						</div>
						<span class="icoDel ui-state-default ui-corner-all"><a href='javascript:void(0);' onclick='galleryRemoveItem(this);' title='Delete'><span class="ui-icon ui-icon-closethick">
						</span></a></span><a class="editMetaData" href="<%#CreatePopupLink("MetaDataEdit", "parm="+ Carrotware.CMS.Core.CMSConfigHelper.EncodeBase64(Eval("FolderPath").ToString() + Eval("FileName").ToString()))%>">
							<img class="imgNoBorder" src="/Manage/images/pencil.png" alt="Edit" title="Edit" />
						</a></li>
				</ItemTemplate>
			</asp:Repeater>
		</ul>
	</div>
	<div style="clear: both">
	</div>
</div>
<asp:TextBox ID="txtGalleryOrder" CssClass="inputFields" TextMode="MultiLine" Rows="8" Columns="60" runat="server"></asp:TextBox>
<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />