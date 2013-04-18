<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileBrowser.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.FileBrowser" %>

<!DOCTYPE html>
<!--[if lt IE 7 ]><html class="ie ie6" lang="en"> <![endif]-->
<!--[if IE 7 ]><html class="ie ie7" lang="en"> <![endif]-->
<!--[if IE 8 ]><html class="ie ie8" lang="en"> <![endif]-->
<!--[if (gte IE 9)|!(IE)]><!-->
<html lang="en">
<!--<![endif]-->
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="LightGreen" />
	<link href="Includes/filebrowser.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		$(document).ready(function () {
			$("input:button, input:submit, input:reset").button();
		});

	</script>
	<script type="text/javascript">
		function SetFile(val) {
			var fldN = '#txtSelectedFile';
			var fld = $(fldN);
			fld.val(val);
		}

		function cmsSetFileName() {
			var fldN = '#txtSelectedFile';
			var fld = $(fldN);
			window.opener.cmsSetFileName(fld.val());
		}

		function cmsSetFileNameReturn() {
			var fldN = '#txtSelectedFile';
			var fld = $(fldN);
			window.parent.cmsSetFileNameReturn(fld.val());

			return false;
		}

	</script>
	<asp:Literal runat="server" ID="pnlTiny">
	
	<script type="text/javascript" src="/c3-admin/tiny_mce/tiny_mce.js"></script>

	<script type="text/javascript" src="/c3-admin/tiny_mce/tiny_mce_popup.js"></script>
	
	<script type="text/javascript">

		var FileBrowserDialogue = {
			init: function () {
				if (tinyMCE.selectedInstance != null) {
					tinyMCE.selectedInstance.fileBrowserAlreadyOpen = true;
				}
				// Here goes your code for setting your custom things onLoad.
			},
			mySubmit: function () {

				var fldN = '#txtSelectedFile';
				var fld = $(fldN);
				var URL = fld.val();
				var win = tinyMCEPopup.getWindowArg("window");
				// insert information now
				if (win.document != null) {
					win.document.getElementById(tinyMCEPopup.getWindowArg("input")).value = URL;
				}
				// are we an image browser
				if (typeof (win.ImageDialog) != "undefined") {
					// we are, so update image dimensions and preview if necessary
					if (win.ImageDialog.getImageData) win.ImageDialog.getImageData();
					if (win.ImageDialog.showPreviewImage) win.ImageDialog.showPreviewImage(URL);
				}

				// close popup window
				tinyMCEPopup.close();
			}
		}

		tinyMCEPopup.onInit.add(FileBrowserDialogue.init, FileBrowserDialogue);

	</script>

	<script type="text/javascript">
		myInitFunction = function () {

			// patch TinyMCEPopup.close
			tinyMCEPopup.close_original = tinyMCEPopup.close;
			tinyMCEPopup.close = function () {
				// remove blocking of opening another file browser window
				if (tinyMCE.selectedInstance != null) {
					tinyMCE.selectedInstance.fileBrowserAlreadyOpen = false;
				}

				// call original function to close the file browser window
				tinyMCEPopup.close_original();
			};
		}

		myExitFunction = function () {
			if (tinyMCE != null) {
				if (tinyMCE.selectedInstance != null) {
					tinyMCE.selectedInstance.fileBrowserAlreadyOpen = false;
				}
			}
		}

		window.onbeforeunload = myExitFunction;

		tinyMCEPopup.executeOnLoad('myInitFunction();');
		
	</script>
	
	</asp:Literal>
	<title>Browser</title>
</head>
<body>
	<form id="form1" runat="server">
	<div class="panel_wrapper">
		<table>
			<tr>
				<td>
					<h2 class="head2">
						Files On Server</h2>
					Contents of:
					<asp:Label ID="lblPath" runat="server" /><br />
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
							<a runat="server" id="lnkContent" href='<%# String.Format( "./FileBrowser.aspx?fldrpath={0}&useTiny={1}&returnvalue={2}", Eval("FolderPath"),  sQueryMode, sReturnMode ) %>'>
								<%# String.Format( "{0}", Eval("FileName") ).ToUpper() %></a>
						</td>
						<td>
							&nbsp;&nbsp;
						</td>
						<td style="width: 150px;">
							<asp:Label ID="lblFileDate" runat="server" Text='<%# String.Format( "{0}", Eval("FileDate") ) %>'></asp:Label>
						</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table></FooterTemplate>
			</asp:Repeater>
		</div>
		<p>
			<br />
			Select a file to upload to the current folder:<br />
			<asp:FileUpload ID="upFile" runat="server" Width="400" />
			<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
			<asp:Label ID="lblWarning" runat="server"></asp:Label>
		</p>
		<div class="scroll" id="fileZone">
			<asp:Repeater ID="rpFiles" runat="server">
				<HeaderTemplate>
					<table style="width: 98%;">
						<tr class="headerRow">
							<th style="width: 20px;">
							</th>
							<th style="width: 20px;">
							</th>
							<th class="headerRowText">
								Filename
							</th>
							<th style="width: 150px;" class="headerRowText">
								Date
							</th>
							<th>
							</th>
							<th style="width: 80px;" class="headerRowText">
								Size
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
								<div runat="server" id="imgContainer" onmouseout="hideImg(this)" onmouseover="showImg(this)">
									<a runat="server" id="lnkContent" href='<%# CreateFileLink(String.Format( "{0}{1}", Eval("FolderPath"), Eval("FileName") )) %>'>
										<%# String.Format( "{0}", Eval("FileName") ).ToLower() %></a>
								</div>
								<div id="imgWrapper" style="display: none;">
									<img id="imgThmbnail" filetype="<%# FileImageLink(Eval("MimeType").ToString()) %>" alt="" src="<%# CreateFileSrc(Eval("FolderPath").ToString(), Eval("FileName").ToString(), Eval("MimeType").ToString())  %>" />
								</div>
							</div>
						</td>
						<td>
							<asp:Label ID="lblFileDate" runat="server" Text='<%# String.Format( "{0}", Eval("FileDate") ) %>'></asp:Label>
						</td>
						<td>
							&nbsp;
						</td>
						<td>
							<asp:Label ID="lblFileSize" runat="server" Text='<%# String.Format( "{0}", Eval("FileSizeFriendly") ) %>'></asp:Label>
						</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table></FooterTemplate>
			</asp:Repeater>
		</div>
		<div id="imgWrapperMain" style="display: none;">
			<div style="padding: 5px; min-height: 10px; min-width: 10px;">
				<div id="imgPreviewCaption">
					0x0
				</div>
				<img alt="document" id="imgThmbnailPreview" src="/c3-admin/images/document.png" />
			</div>
		</div>
		<div style="display: block; margin-left: -9999px; float: left; max-height: 9000px; max-width: 9000px;">
			<img alt="document" id="imgRealPreview" src="/c3-admin/images/document.png" />
		</div>
		<script type="text/javascript">

			var imgSrc = '/c3-admin/images/document.png';

			var imgPreviewId = 'imgWrapperMain';
			var imgRealId = 'imgRealPreview';
			var imgThumbId = 'imgThmbnailPreview';
			var imgSizeId = 'imgPreviewCaption';

			var divImgLayer = $('#' + imgPreviewId);
			var imgDim = $('#' + imgSizeId);
			var imgThumb = $('#' + imgThumbId);
			var imgReal = $('#' + imgRealId);

			function hideImg(obj) {
				var theNode = $(obj).parent();
				var grp = $(theNode).attr('id');

				$(divImgLayer).attr('style', 'display:none;');
				$(divImgLayer).attr('class', '');

				$(imgThumb).attr('src', imgSrc);
				$(imgThumb).attr('width', 64);
				$(imgThumb).attr('height', 64);
				$(imgThumb).removeAttr("width").attr("width");
				$(imgThumb).removeAttr("height").attr("height");

				$(imgReal).attr('src', imgSrc);

				$(imgDim).html('<br />');
			}

			function showImg(obj) {
				var theNode = $(obj).parent();

				var imgSrc = $(theNode).find('img');
				var imgtype = $(imgSrc).attr('filetype');

				if (imgtype.indexOf('image') >= 0) {

					var val = $(imgSrc).attr('src');
					var grp = $('#fileZone').attr('id');
					var pos = $('#fileZone').offset();

					$(divImgLayer).attr('style', '');
					$(divImgLayer).css({ "left": (pos.left + 200) + "px", "top": (pos.top - 25) + "px" }).show();
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
		<p>
			<br />
			<asp:Button ID="btnRemove" runat="server" Text="Delete Checked" OnClick="btnRemove_Click" />
		</p>
		<p>
			<br />
			Selected File:
			<asp:TextBox ID="txtSelectedFile" Columns="50" runat="server" />
			<asp:Button Visible="false" ID="btnSelectedFile" runat="server" Text="Return Selection" OnClientClick="FileBrowserDialogue.mySubmit();" />
			<asp:Button Visible="false" ID="btnReturnFile" runat="server" Text="Select File" OnClientClick="return cmsSetFileNameReturn();" />
		</p>
	</div>
	<asp:Panel runat="server" ID="pnlTiny2">
		<div class="mceActionPanel">
			<input type="submit" id="insert" name="insert" value="Select" onclick="FileBrowserDialogue.mySubmit();return false;" />
			<input type="button" id="cancel" name="cancel" value="Cancel" onclick="tinyMCEPopup.close();" />
		</div>
	</asp:Panel>
	<div style="display: none">
		<asp:Panel runat="server" ID="pnlFileMgr">
			<input type="submit" id="Submit1" name="insert" value="Select" onclick="cmsSetFileName();return false;" />
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<input type="button" id="Button1" name="cancel" value="Cancel" onclick="window.close();" />
		</asp:Panel>
	</div>
	</form>
</body>
</html>
