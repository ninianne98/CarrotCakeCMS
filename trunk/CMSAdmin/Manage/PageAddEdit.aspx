<%@ Page ValidateRequest="false" Title="PageAddEdit" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true"
	CodeBehind="PageAddEdit.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.PageAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Page Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<br />
	<div class="ui-widget" runat="server" id="divEditing">
		<div class="ui-state-highlight ui-corner-all" style="padding: 5px; margin-top: 5px; margin-bottom: 5px; width: 500px;">
			<p>
				<span class="ui-icon ui-icon-info" style="float: left; margin: 3px;"></span>
				<asp:Literal ID="litUser" runat="server">&nbsp</asp:Literal></p>
		</div>
	</div>
	<table width="700">
		<tr>
			<td width="125" class="tablecaption" valign="top">
				last updated:
			</td>
			<td width="575" valign="top">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td width="175" valign="top">
							<asp:Label ID="lblUpdated" runat="server"></asp:Label>
						</td>
						<td width="175" valign="top">
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</td>
						<td width="100" valign="top">
							<asp:CheckBox ID="chkActive" runat="server" Text="published" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				title bar:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTitle" runat="server" Columns="45"
					MaxLength="200"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtTitle" ID="RequiredFieldValidator1" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				filename:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtFileName"
					runat="server" Columns="45" MaxLength="200"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileName" ID="RequiredFieldValidator2" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFileValid" ID="RequiredFieldValidator6" runat="server"
					ErrorMessage="Not Valid/Unique" Display="Dynamic"></asp:RequiredFieldValidator>
				<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" Style="display: none;"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				page head:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtHead" runat="server" Columns="45"
					MaxLength="200"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtHead" ID="RequiredFieldValidator3" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				navigation:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtNav" runat="server" Columns="45"
					MaxLength="200"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtNav" ID="RequiredFieldValidator4" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="tablecaption" valign="top">
				sort:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onblur="checkIntNumber(this);" Text="10" ID="txtSort" runat="server" Columns="15"
					MaxLength="5" onkeypress="return ProcessKeyPress(event)"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtSort" ID="RequiredFieldValidator5" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				parent page:
			</td>
			<td valign="top">
				<asp:DropDownList DataTextField="NavMenuText" DataValueField="Root_ContentID" ID="ddlParent" runat="server">
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				template:
			</td>
			<td valign="top">
				<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
			</td>
			<td valign="top" align="right">
				<input type="button" id="btnBrowseSvr" value="Browse Server Files" onclick="FileBrowserOpen('not-a-real-file');" />
			</td>
		</tr>
	</table>
	<br />
	<div id="jqtabs" style="height: 400px; width: 780px;">
		<ul>
			<li><a href="#pagecontent-tabs-0">Left</a></li>
			<li><a href="#pagecontent-tabs-1">Center</a></li>
			<li><a href="#pagecontent-tabs-3">Right</a></li>
		</ul>
		<div id="pagecontent-tabs-0">
			<div style="height: 325px; margin-bottom: 10px;">
				<div runat="server" id="divLeft">
					body (left)<br />
					<a href="javascript:toggleEditor('<%= reLeftBody.ClientID %>');">Show/Hide Editor</a></div>
				<asp:TextBox Style="height: 280px; width: 730px;" CssClass="mceEditor" ID="reLeftBody" runat="server" TextMode="MultiLine"
					Rows="15" Columns="80"></asp:TextBox>
				<%--<asp:Button ID="Button1" runat="server" OnClientClick="SubmitPage()" Text="Save" /><br />--%>
				<br />
			</div>
		</div>
		<div id="pagecontent-tabs-1">
			<div style="height: 310px; margin-bottom: 10px;">
				<div runat="server" id="divCenter">
					body (main/center)<br />
					<a href="javascript:toggleEditor('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
				<asp:TextBox Style="height: 280px; width: 730px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="15"
					Columns="80"></asp:TextBox>
				<%--<asp:Button ID="Button2" runat="server" OnClientClick="SubmitPage()" Text="Save" /><br />--%>
				<br />
			</div>
		</div>
		<div id="pagecontent-tabs-3">
			<div style="height: 310px; margin-bottom: 10px;">
				<div runat="server" id="divRight">
					body (right)<br />
					<a href="javascript:toggleEditor('<%= reRightBody.ClientID %>');">Show/Hide Editor</a></div>
				<asp:TextBox Style="height: 280px; width: 730px;" CssClass="mceEditor" ID="reRightBody" runat="server" TextMode="MultiLine"
					Rows="15" Columns="80"></asp:TextBox>
				<%--<asp:Button ID="Button3" runat="server" OnClientClick="SubmitPage()" Text="Save" /><br />--%>
				<br />
			</div>
		</div>
	</div>
	<div id="heatbeat" style="clear: both; padding: 2px; margin: 2px;">
		&nbsp;</div>
	<br />
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Save" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<input type="button" id="btnCancel" value="Cancel" onclick="location.href='./PageIndex.aspx';" />
	<br />
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
	</div>

	<script type="text/javascript">
		var webSvc = "/Manage/CMS.asmx";

		var thisPage = '';

		var thisPageID = '<%=guidContentID.ToString() %>';

		function MakeStringSafe(val) {
			val = Base64.encode(val);
			return val;
		}

		function CheckFileName() {
			thisPage = $('#<%= txtFileName.ClientID %>').val();

			$('#<%= txtFileValid.ClientID %>').val('');

			var webMthd = webSvc + "/ValidateUniqueFilename";
			var myPage = MakeStringSafe(thisPage);

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'TheFileName': '" + myPage + "', 'PageID': '" + thisPageID + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: editFilenameCallback,
				error: ajaxFailed
			});
		}

		$(document).ready(function() {
			CheckFileName();
		});

		function editFilenameCallback(data, status) {
			if (data.d == "PASS") {
				$('#<%= txtFileValid.ClientID %>').val('VALID');
			} else {
				$('#<%= txtFileValid.ClientID %>').val('');
			}
			Page_ClientValidate();
		}
	</script>

	<script type="text/javascript">

		function AutoSynchMCE() {
			if (saving != 1) {
				tinyMCE.triggerSave();
				setTimeout("AutoSynchMCE();", 2500);
				//alert("AutoSynchMCE");
			}
		}

		AutoSynchMCE();

		var saving = 0;

		function SubmitPage() {
			saving = 1;
			//tinyMCE.triggerSave();
			CheckFileName();
			setTimeout("ClickBtn();", 1000);
		}
		function ClickBtn() {
			$('#<%=btnSave.ClientID %>').click();
		}
	</script>

	<script type="text/javascript">

		$(document).ready(function() {
			setTimeout("$('#jqtabs').tabs('select', 'pagecontent-tabs-1');", 500);
		});
	</script>

	<asp:Panel ID="pnlHB" runat="server">

		<script type="text/javascript">
			var webSvc = "/Manage/CMS.asmx";

			var thisPage = '<%=CurrentScriptName %>';

			function EscapeFile() {
				thisPage = MakeStringSafe(thisPage);
			}
			EscapeFile();

			function MakeStringSafe(val) {
				val = val.replace(/(')/g, "{&&#0x0027-#0039&&}");
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

			function updateHeartbeat(data, status) {
				var hb = $('#heatbeat');
				hb.empty().append('HB:  ');
				hb.append(data.d);
			}

			setTimeout("EditHB();", 1500);


			function ajaxFailed(request) {
				var s = "";
				s = s + "<b>status: </b>" + request.status + '<br />\r\n';
				s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
				s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';
				alertModal(s);
			}

	
		</script>

	</asp:Panel>
</asp:Content>
