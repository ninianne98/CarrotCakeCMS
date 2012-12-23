<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileBrowser.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.FileBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<carrot:jquerybasic runat="server" ID="jquerybasic1" SelectedSkin="LightGreen" />
	<style type="text/css">
		BODY {
			background-color: #FFFFFF;
			text-align: left;
			font-size: 11px;
			font-family: Arial, Helvetica, sans-serif;
		}
		a {
			font-weight: bold;
			text-decoration: none;
		}
		a:link {
			color: #000099;
		}
		a:active {
			color: #FFDB0D;
		}
		a:visited {
			color: #BF9232;
		}
		a:hover {
			text-decoration: underline;
		}
		div.scroll {
			height: 175px;
			width: 600px;
			overflow: auto;
			border: 1px solid #666;
			padding: 2px;
		}
		td {
			font-size: 11px;
			color: #000000;
			background-color: #FFFFFF;
		}
		th {
			font-size: 11px;
			color: #FFFFFF;
			background-color: #97AC88;
		}
		.head2 {
			color: #97AC88;
		}
		.ImgGroup {
			position: relative;
			z-index: 1200;
		}
		#imgWrapperMain {
			min-height: 25px;
			min-width: 25px;
			padding: 8px;
			position: absolute;
			z-index: 2000;
		}
		.thumbpreview {
			font-size: 12px;
			color: #000000;
			background-color: #B7D7C4;
		}
		#imgDimension {
			font-size: 12px;
			font-weight: bold;
			text-align: left;
		}
	</style>
	<script type="text/javascript">
		$(document).ready(function () {
			$(function () {
				$("input:button, input:submit, input:file").button();
			});
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
		<table cellpadding="2" cellspacing="0">
			<tr>
				<td>
					<h2 class="head2">
						Files On Server</h2>
					Contents of:
					<asp:Label ID="lblPath" runat="server"></asp:Label><br />
					<asp:HyperLink runat="server" ID="lnkUp"><img src="/c3-admin/images/back.png" border="0" alt="back" /><img src="/c3-admin/images/folder.png" border="0" alt="folder" /> </asp:HyperLink>
					<br />
				</td>
			</tr>
		</table>
		<div class="scroll" id="folderZone">
			<asp:Repeater ID="rpFolders" runat="server">
				<HeaderTemplate>
					<table cellpadding="2" cellspacing="0">
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<img src="/c3-admin/images/folder.png" alt="folder" />
						</td>
						<td>
							<a runat="server" id="lnkContent" href='<%# String.Format( "./FileBrowser.aspx?fldrpath={0}&useTiny={1}&returnvalue={2}", Eval("FolderPath"),  sQueryMode, sReturnMode ) %>'>
								<%# String.Format( "{0}", Eval("FileName") ).ToUpper() %></a>
						</td>
						<td>
							&nbsp;&nbsp;
						</td>
						<td>
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
			<asp:FileUpload ID="upFile" runat="server" />
			<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
			<asp:Label ID="lblWarning" runat="server"></asp:Label>
		</p>
		<div class="scroll" id="fileZone">
			<asp:Repeater ID="rpFiles" runat="server">
				<HeaderTemplate>
					<table cellpadding="2" cellspacing="0">
						<tr bgcolor="#F74902">
							<th>
							</th>
							<th>
							</th>
							<th>
								<font color="#FFFFFF">Filename</font>
							</th>
							<th>
								<font color="#FFFFFF">Date</font>
							</th>
							<th>
							</th>
							<th>
								<font color="#FFFFFF">Size</font>
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
				<div id="imgDimension">
				</div>
				<img alt="" id="imgThmbnail" src="/c3-admin/images/document.png" />
			</div>
		</div>
		<script type="text/javascript">

			var imgSrcLayer = 'imgWrapper';
			var imgPreview = 'imgWrapperMain';

			function hideImg(obj) {
				var theNode = $(obj).parent();
				var grp = $(theNode).attr('id');

				hideImg2(grp);
			}

			function hideImg2(grp) {
				var theImgLayer = $('#' + imgPreview);

				$(theImgLayer).attr('style', 'display:none;');
				$(theImgLayer).attr('class', '');
			}

			function showImg(obj) {
				var theNode = $(obj).parent();

				var imgSrc = $(theNode).find('img');

				var imgtype = imgSrc.attr('filetype');

				//alert(imgtype);

				if (imgtype.indexOf('image') >= 0) {

					var val = imgSrc.attr('src');
					var grp = $('#fileZone').attr('id');
					var pos = $('#fileZone').offset();
					var theImgLayer = $('#' + imgPreview);

					var imgDim = $(theImgLayer).find('#imgDimension');

					$(theImgLayer).attr('style', '');
					$(theImgLayer).css({ "left": (pos.left + 200) + "px", "top": (pos.top - 25) + "px" }).show();
					$(theImgLayer).attr('class', 'thumbpreview ui-corner-all');

					var img = $(theImgLayer).find('img');

					img.attr('width', 200);
					img.attr('height', 200);
					img.removeAttr("width").attr("width");
					img.removeAttr("height").attr("height");
					img.attr('src', '/c3-admin/images/document.png');

					img.attr('alt', val);
					img.attr('title', val);
					img.attr('src', val);

					imgDim.html('<br />');
					//imgDim.html(val +'<br /> '+ img.width() + 'x' + img.height());
					imgDim.html(img.width() + ' x ' + img.height());

					if (img.height() > 175) {
						img.attr('height', 180);
					}

					setTimeout("resizeImg('" + grp + "');", 1000);
					setTimeout("resizeImg('" + grp + "');", 2500);
				}
			}

			function resizeImg(grp) {
				var theImgLayer = $('#' + imgSrcLayer);
				var img = $(theImgLayer).find('img');

				img.removeAttr("width").attr("width");
				img.removeAttr("height").attr("height");

				if (img.height() > 175) {
					img.attr('height', 165);
					setTimeout("resizeImg('" + grp + "');", 1000);
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
			<asp:TextBox ID="txtSelectedFile" Columns="50" runat="server"></asp:TextBox>
			<asp:Button Visible="false" ID="btnSelectedFile" runat="server" Text="Return Selection" OnClientClick="FileBrowserDialogue.mySubmit();" />
			<asp:Button Visible="false" ID="btnReturnFile" runat="server" Text="Select File" OnClientClick="return cmsSetFileNameReturn();" />
		</p>
	</div>
	<asp:Literal runat="server" ID="pnlTiny2">
	<div class="mceActionPanel">
		<input type="submit" id="insert" name="insert" value="Select" onclick="FileBrowserDialogue.mySubmit();return false;" />
		<input type="button" id="cancel" name="cancel" value="Cancel" onclick="tinyMCEPopup.close();" />
	</div>
	</asp:Literal>
	<div style="display: none">
		<asp:Literal runat="server" ID="litFileMgr">
		<input type="submit" id="Submit1" name="insert" value="Select" onclick="cmsSetFileName();return false;" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" id="Button1" name="cancel" value="Cancel" onclick="window.close();" />
		</asp:Literal>
	</div>
	</form>
</body>
</html>
