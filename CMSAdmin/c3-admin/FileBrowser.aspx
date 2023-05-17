<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileBrowser.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.FileBrowser" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="LightGreen" />
	<link href="iCheck/iCheck.css" rel="stylesheet" type="text/css" />
	<script src="iCheck/icheck.min.js" type="text/javascript"></script>
	<script src="Includes/icheck.init.js" type="text/javascript"></script>
	<script src="Includes/jquery.form.min.js" type="text/javascript"></script>
	<link href="Includes/uploadfile.css" rel="stylesheet" type="text/css" />
	<script src="Includes/jquery.uploadfile.min.js" type="text/javascript"></script>
	<link href="Includes/filebrowser.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		$(document).ready(function () {
			$("input:button, input:submit, input:reset").button();
		});
	</script>
	<script type="text/javascript">
		var imgFileExt = ['jpeg', 'jpg', 'png', 'gif', 'bmp', 'webp'];
		var defaultImage = '/c3-admin/images/LargeDoc.png';

		var imgPreviewId = '#imgWrapperMain';
		var imgRealId = '#imgRealPreview';
		var imgThumbId = '#imgThmbnailPreview';
		var imgSizeId = '#imgPreviewCaption';
		var selFile = '#filePickerFileName';
		var selFileImgPrev = '#imgThmbnailSelected';
		var fileZone = '#fileZone';

		$(document).ready(function () {
			$("input:button, input:submit, input:reset, button").button();

			$(selFile).val('');
		});

		//=========================

		function SetFile(uri) {
			var fld = $(selFile);

			fld.val(uri);
			fld.blur();
		}

		function cmsUpdateFileName() {
			$(selFileImgPrev).attr('src', defaultImage);

			var uri = $(selFile).val();
			var uriExt = uri.substr(uri.lastIndexOf('.') + 1);
			if ($.inArray(uriExt.toLowerCase(), imgFileExt) > -1) {
				$(selFileImgPrev).attr('src', uri);
			}
		}

		function cmsSetFileName() {
			var fld = $(selFile);

			window.opener.cmsSetFileName(fld.val());
		}

		function cmsSetFileNameReturn() {
			var fld = $(selFile);

			window.parent.cmsSetFileNameReturn(fld.val());

			return false;
		}
	</script>

	<script type="text/javascript">
		var dropResults = '.ajax-file-upload-container';
		var clrBtnFiles = '#btnClearUploads';

		function clearUploads() {
			if ($(dropResults).html().length > 10) {
				$(clrBtnFiles).hide();
				$(dropResults).html('');
			}
			return false;
		}

		var upLoad = null

		$(document).ready(function () {
			$(clrBtnFiles).hide();

			$("#fileuploader").on('drop', function (e) {
				setTimeout(function () {
					if ($(dropResults).html().length > 10) {
						$(clrBtnFiles).show();
					}
				}, 900);

				return true;
			});

			upLoad = $("#fileuploader").uploadFile({
				url: "./FileUp.ashx",
				fileName: '<%=PostedFiles.ClientID %>',

				dragDrop: true,
				multiple: true,
				maxFileCount: -1,
				maxFileSize: 8 * 1024 * 1024,

				uploadButtonClass: "ajax-file-upload-none",
				uploadStr: "  ",
				dragDropStr: "<span>Drag files here to upload <button style='display:none;' id='btnClearUploads' onclick='clearUploads();return false;'>clear uploads</button> </span>",
				statusBarWidth: 525,
				dragdropWidth: 525,
				showPreview: true,
				previewHeight: "auto",
				previewWidth: "100px",

				dynamicFormData: function () {
					var data = {
						'FileDirectory': $('#<%=FileDirectory.ClientID %>').val(),
						'EscapeSpaces': $('#<%=chkSpaceEscape.ClientID %>').prop("checked")
					};
					return data;
				},

				returnType: "json",
				showDone: true
			});
		});

		function resetUp() {
			upLoad.reset();
		}
	</script>
	<asp:PlaceHolder runat="server" ID="pnlTiny">
		<script type="text/javascript">
			function tinySubmit() {
				var fld = $(selFile);
				var uri = fld.val();

				var imgReal = $(imgRealId);
				$(imgReal).attr('src', '');
				var h = -1;
				var w = -1;
				$(imgReal).attr('src', uri);

				var uriExt = uri.substr(uri.lastIndexOf('.') + 1);

				// wait a tiny bit so the image can load
				setTimeout(function () {
					if ($.inArray(uriExt.toLowerCase(), imgFileExt) > -1) {
						h = $(imgReal).height();
						w = $(imgReal).width();
					}

					window.parent.cmsFileBrowseSetUri(uri, h, w);
				}, 500);
			}
		</script>
	</asp:PlaceHolder>
	<title>Browser</title>
</head>
<body>
	<form id="form1" runat="server">
		<div class="panel_wrapper">
			<table>
				<tr>
					<td>
						<h2 class="head2">Files On Server</h2>
						Contents of:
						<asp:Literal ID="litPath" runat="server" /><br />
						<asp:HyperLink runat="server" ID="lnkUp"><img src="/c3-admin/images/back.png" border="0" alt="back" /><img src="/c3-admin/images/folder.png" border="0" alt="folder" /> </asp:HyperLink>
						<br />
					</td>
				</tr>
			</table>
			<div class="scroll" id="folderZone">
				<asp:Repeater ID="rpFolders" runat="server">
					<HeaderTemplate>
						<table style="width: 98%">
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td style="width: 32px">
								<img src="/c3-admin/images/folder.png" alt="folder" />
							</td>
							<td>
								<a runat="server" id="lnkContent" href='<%# String.Format( "./FileBrowser.aspx?fldrpath={0}&useTiny={1}&returnvalue={2}&viewmode={3}", Eval("FolderPath"),  useTinyMode, sReturnMode, sViewMode ) %>'>
									<%# String.Format( "{0}", Eval("FileName") ) %></a>
							</td>
							<td>&nbsp;&nbsp;
							</td>
							<td style="width: 150px;">
								<asp:Literal ID="litFileDate" runat="server" Text='<%# String.Format( "{0}", Eval("FileDate") ) %>' />
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
			</div>
			<%--<p>
			<br />
			Select a file to upload to the current folder:<br />
			<asp:FileUpload ID="upFile" runat="server" Width="400" />
			<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
			<asp:CheckBox runat="server" ID="chkSpaceEscape" Checked="true" />
			Change spaces to dashes
			<br />
			<asp:Label ID="lblWarning" runat="server" />
		</p>--%>
			<div style="margin-top: 10px; margin-bottom: 10px;">
				<div>
					<asp:CheckBox runat="server" ID="chkSpaceEscape" Checked="true" />
					Change spaces to dashes &nbsp;&nbsp;&nbsp;&nbsp; [<asp:HyperLink runat="server" ID="lnkRefresh" Text="Refresh" />]
				<br />
				</div>
				<div>
					<div id="fileuploader">
						Upload
					</div>
				</div>
				<div style="display: none;">
					<input type="file" id="PostedFiles" name="PostedFiles" runat="server" />
					<asp:HiddenField ID="FileDirectory" runat="server" />
				</div>
				<asp:Label ID="lblWarning" runat="server" />
			</div>
			<div class="scroll" id="fileZone">
				<asp:Repeater ID="rpThumbs" runat="server">
					<ItemTemplate>
						<div class="ui-widget-header ui-corner-all thumbCell" runat="server" id="imgContainerGroup">
							<div runat="server" id="imgContainer" onmouseout="hideImg(this)" onmouseover="showImg(this, 'thumb')">
								<div id="imgWrapper" style="display: none;">
									<img id="imgThmbnail" filetype="<%# FileImageLink(Eval("MimeType").ToString()) %>" alt="" src="<%# CreateFileSrc(Eval("FolderPath").ToString(), Eval("FileName").ToString(), Eval("MimeType").ToString())  %>" />
								</div>
								<div style="margin: 3px;" id="imgSubContainer">
									<a runat="server" id="lnkContent" href='<%# CreateFileLink(String.Format( "{0}{1}", Eval("FolderPath"), Eval("FileName") )) %>'>
										<carrot:ImageSizer runat="server" ID="ImageSizer1" ImageUrl='<%# String.Format( "{0}{1}", Eval("FolderPath"), Eval("FileName") )  %>' ThumbSize="50"
											ScaleImage="true" ToolTip="" />
									</a>
								</div>
								<div style="margin: 3px; text-align: center;">
									<%# String.Format( "{0}", Eval("FileName") ) %><br />
									<%# String.Format( "{0:d}", Eval("FileDate") ) %>
								</div>
							</div>
						</div>
					</ItemTemplate>
				</asp:Repeater>
				<asp:Repeater ID="rpFiles" runat="server">
					<HeaderTemplate>
						<table style="width: 98%;">
							<tr class="headerRow">
								<th style="width: 20px;"></th>
								<th style="width: 20px;"></th>
								<th class="headerRowText">Filename
								</th>
								<th style="width: 150px;" class="headerRowText">Date
								</th>
								<th></th>
								<th style="width: 80px;" class="headerRowText">Size
								</th>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td>
								<asp:CheckBox ID="chkRemove" runat="server" value='<%# Eval("FileName") %>' />
							</td>
							<td>
								<img src="/c3-admin/images/<%# FileImageLink(String.Format("{0}", Eval("MimeType")))  %>.png" alt="filetype" />
							</td>
							<td>
								<div class="ImgGroup" runat="server" id="imgContainerGroup">
									<div runat="server" id="imgContainer" onmouseout="hideImg(this)" onmouseover="showImg(this, 'file')">
										<a runat="server" id="lnkContent" href='<%# CreateFileLink(String.Format( "{0}{1}", Eval("FolderPath"), Eval("FileName") )) %>'>
											<%# String.Format( "{0}", Eval("FileName") ) %></a>
									</div>
									<div id="imgWrapper" style="display: none;">
										<img id="imgThmbnail" filetype="<%# FileImageLink(Eval("MimeType").ToString()) %>" alt="" src="<%# CreateFileSrc(Eval("FolderPath").ToString(), Eval("FileName").ToString(), Eval("MimeType").ToString())  %>" />
									</div>
								</div>
							</td>
							<td>
								<asp:Literal ID="litFileDate" runat="server" Text='<%# String.Format( "{0}", Eval("FileDate") ) %>' />
							</td>
							<td>&nbsp;
							</td>
							<td>
								<asp:Literal ID="litFileSize" runat="server" Text='<%# String.Format( "{0}", Eval("FileSizeFriendly") ) %>' />
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
			</div>
			<div>
				<asp:HyperLink runat="server" ID="lnkThumbView" Text="View Image Thumbnails" />
				<asp:HyperLink runat="server" ID="lnkFileView" Text="View All Files" />
			</div>
			<div id="imgWrapperMain" style="display: none;">
				<div style="padding: 5px; min-height: 10px; min-width: 10px;">
					<div id="imgPreviewCaption">
						0x0
					</div>
					<img alt="document" id="imgThmbnailPreview" src="/c3-admin/images/LargeDoc.png" class="thumbPreview" />
				</div>
			</div>

			<script type="text/javascript">
				var divImgLayer = $(imgPreviewId);
				var imgDim = $(imgSizeId);
				var imgThumb = $(imgThumbId);
				var imgReal = $(imgRealId);

				function hideImg(obj) {
					var theNode = $(obj).parent();
					var grp = $(theNode).attr('id');

					$(divImgLayer).attr('style', 'display:none;');
					$(divImgLayer).attr('class', '');

					$(imgThumb).attr('src', defaultImage);
					$(imgThumb).attr('width', 64);
					$(imgThumb).attr('height', 64);
					$(imgThumb).removeAttr("width").attr("width");
					$(imgThumb).removeAttr("height").attr("height");

					$(imgReal).attr('src', defaultImage);

					$(imgDim).html('<br />');
				}

				function showImg(obj, mode) {
					var theNode = $(obj).parent();
					var defaultImage = $(theNode).find('img');
					var imgtype = $(defaultImage).attr('filetype');

					if (imgtype.indexOf('image') >= 0) {

						var val = $(defaultImage).attr('src');
						var grp = $(fileZone).attr('id');
						var pos = $(fileZone).offset();

						$(divImgLayer).attr('style', '');

						if (mode == 'file') {
							$(divImgLayer).css({ "left": (pos.left + 150) + "px", "top": (pos.top - 25) + "px" }).show();
						} else {
							$(divImgLayer).css({ "left": (pos.left + 20) + "px", "top": (pos.top - 200) + "px" }).show();
						}

						$(divImgLayer).attr('class', 'thumbpreview ui-corner-all');

						$(imgThumb).attr('alt', val);
						$(imgThumb).attr('title', val);
						$(imgThumb).attr('src', val);

						$(imgReal).attr('src', val);

						resizeImg();

						setTimeout("resizeImg();", 500);
						setTimeout("resizeImg();", 1500);
						setTimeout("resizeImg();", 5000);
					}
				}

				function resizeImg() {

					$(imgDim).html($(imgReal).width() + ' x ' + $(imgReal).height());

					if ($(imgThumb).height() > 175) {
						$(imgThumb).attr('height', 165);
						setTimeout("resizeImg();", 1500);
					}
				}
			</script>
			<asp:PlaceHolder ID="phRemove" runat="server">
				<p>
					<br />
					<asp:Button ID="btnRemove" runat="server" Text="Delete Checked" OnClick="btnRemove_Click" />
				</p>
			</asp:PlaceHolder>
			<div>
				<div style="float: left">
					<p>
						<br />
						Selected File:
						<input name="filePickerFileName" type="text" size="55" id="filePickerFileName" onchange="cmsUpdateFileName();return false;" onblur="cmsUpdateFileName();return false;" />
						<asp:Button Visible="false" ID="btnSelectedFile" runat="server" Text="Return Selection" OnClientClick="FileBrowserDialogue.mySubmit();" />
						<asp:Button Visible="false" ID="btnReturnFile" runat="server" Text="Select File" OnClientClick="return cmsSetFileNameReturn();" />
					</p>
				</div>
				<div style="float: left">
					<div class="ui-widget-header ui-corner-all" style="margin: auto; margin: 5px 0 0 25px; height: 160px; width: 325px; float: left; display: block; border: 1px solid #666;">
						<img style="max-height: 99%; max-width: 99%; display: block; margin: 2px; margin-left: auto; margin-right: auto;" id="imgThmbnailSelected" src="/c3-admin/images/LargeDoc.png" />
					</div>
				</div>
			</div>
		</div>
		<div style="clear: both; height: 110px">
			<asp:Panel runat="server" ID="pnlTiny2">
				<div class="mceActionPanel fileBrowserButtons">
					<input type="submit" runat="server" id="insert" name="insert" value="Select" onclick="tinySubmit(); return false;" />
					<input type="button" runat="server" id="cancel" name="cancel" value="Cancel" onclick="window.parent.cmsFileBrowseClose();" />
				</div>
			</asp:Panel>
		</div>
		<div style="display: none">
			<asp:Panel runat="server" ID="pnlFileMgr">
				<input type="submit" id="Submit1" name="insert" value="Select" onclick="cmsSetFileName(); return false;" />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<input type="button" id="Button1" name="cancel" value="Cancel" onclick="window.close();" />
			</asp:Panel>
		</div>
		<div style="display: block; margin-left: -9999px; margin-top: -9999px; float: left; max-height: 9000px; max-width: 9000px;">
			<img alt="document" id="imgRealPreview" src="/c3-admin/images/LargeDoc.png" />
		</div>
		<p>
			<br class="clear" />
		</p>
	</form>
</body>
</html>
