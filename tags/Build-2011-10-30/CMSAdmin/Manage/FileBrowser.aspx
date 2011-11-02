<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileBrowser.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.FileBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
	</style>
	<carrot:jquery runat="server" ID="jquery1" JQVersion="1.6" />

	<script src="/Manage/glossyseagreen/js/jquery-ui-glossyseagreen.js" type="text/javascript"></script>

	<link href="/Manage/glossyseagreen/css/jquery-ui-glossyseagreen.css" rel="stylesheet" type="text/css" />

	<script>
		$(document).ready(function() {
			$(function() {
				$("input:button, input:submit, input:file").button();
			});
		});
	</script>

	<script type="text/javascript">
		function SetFile(val) {
			var fldN = '#txtSelectedFile';
			var fld = $(fldN);
			fld.val(val);
			var img = $('#imgThumb');
			img.attr('width', 180);
			img.attr('height', 180);
			img.removeAttr("width").attr("width");
			img.removeAttr("height").attr("height");
			img.attr('src', '/nothing.png');

			img.attr('alt', val);
			img.attr('title', val);
			img.attr('src', val);

			if (img.removeAttr("width").attr("width") > 180) {
				img.attr('width', 200);
			}
			setTimeout("resizeImg();", 1500);

		}

		function resizeImg() {
			//			var img = $('#imgThumb');
			//			img.removeAttr("width").attr("width");
			//			img.removeAttr("height").attr("height");
			//			if (img.removeAttr("width").attr("width") > 180) {
			//				img.attr('width', 180);
			//				setTimeout("resizeImg();", 1000);
			//			}
		}

		function fnSetFile() {
			var fldN = '#txtSelectedFile';
			var fld = $(fldN);
			window.opener.fnSetFile(fld.val());
		}

	</script>

	<asp:Literal runat="server" ID="pnlTiny">
	
	<script type="text/javascript" src="/manage/tiny_mce/tiny_mce.js"></script>

	<script type="text/javascript" src="/manage/tiny_mce/tiny_mce_popup.js"></script>
	
	<script language="javascript" type="text/javascript">

		var FileBrowserDialogue = {
			init: function() {
				if (tinyMCE.selectedInstance != null) {
					tinyMCE.selectedInstance.fileBrowserAlreadyOpen = true;
				}
				// Here goes your code for setting your custom things onLoad.
			},
			mySubmit: function() {

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
		myInitFunction = function() {

			// patch TinyMCEPopup.close
			tinyMCEPopup.close_original = tinyMCEPopup.close;
			tinyMCEPopup.close = function() {
				// remove blocking of opening another file browser window
				if (tinyMCE.selectedInstance != null) {
					tinyMCE.selectedInstance.fileBrowserAlreadyOpen = false;
				}

				// call original function to close the file browser window
				tinyMCEPopup.close_original();
			};
		}

		myExitFunction = function() {
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
					<asp:HyperLink runat="server" ID="lnkUp"> <img src="/manage/tiny_mce/FolderUp.gif" border=0 />...</asp:HyperLink>
					<br />
				</td>
			</tr>
		</table>
		<div class="scroll">
			<asp:Repeater ID="rpFolders" runat="server">
				<HeaderTemplate>
					<table cellpadding="2" cellspacing="0">
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<img src="/manage/tiny_mce/Folder.gif" />
						</td>
						<td>
							<a runat="server" id="lnkContent" href='<%# String.Format( "./FileBrowser.aspx?fldrpath={0}&useTiny={1}", Eval("FolderPath"), sQueryMode ) %>'>
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
		<%--<div style="float: right; text-align: center;">
			<img src='' id="imgThumb" name="imgThumb" width="200" alt="Preview (if image)" />
		</div>--%>
		<div class="scroll">
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
							<img src="/manage/tiny_mce/File.gif" />
						</td>
						<td>
							<a runat="server" id="lnkContent" href='<%# CreateFileLink(String.Format( "{0}{1}", Eval("FolderPath"), Eval("FileName") )) %>'>
								<%# String.Format( "{0}", Eval("FileName") ).ToLower() %></a>
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
		<p>
			<br />
			<asp:Button ID="btnRemove" runat="server" Text="Delete Checked" OnClick="btnRemove_Click" />
		</p>
		<p>
			<br />
			Selected File:
			<asp:TextBox ID="txtSelectedFile" Columns="50" runat="server"></asp:TextBox>
			<asp:Button Visible="false" ID="btnSelectedFile" runat="server" Text="Return Selection" OnClientClick="FileBrowserDialogue.mySubmit();" />
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
		<input type="submit" id="Submit1" name="insert" value="Select" onclick="fnSetFile();return false;" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" id="Button1" name="cancel" value="Cancel" onclick="window.close();" />
		</asp:Literal>
	</div>
	</form>
</body>
</html>
