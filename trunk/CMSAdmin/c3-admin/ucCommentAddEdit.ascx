<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCommentAddEdit.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucCommentAddEdit" %>
<fieldset style="width: 360px;">
	<legend>
		<label>
			Page Info
		</label>
	</legend>
	<table style="width: 700px;">
		<tr>
			<td style="width: 125px;" class="tablecaption">
				title:
			</td>
			<td>
				<asp:Literal ID="lblTitle" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				filename:
			</td>
			<td>
				<asp:Literal ID="lblFile" runat="server" />
				&nbsp;&nbsp;<asp:HyperLink ID="lnkPage" runat="server" Target="_blank"><img class="imgNoBorder" src="images/html2.png" 
				title="Visit page" alt="Visit page" /></asp:HyperLink>
			</td>
		</tr>
	</table>
</fieldset>
<fieldset style="width: 360px;">
	<legend>
		<label>
			Comment
		</label>
	</legend>
	<table style="width: 700px;">
		<tr>
			<td style="width: 150px;" class="tablecaption">
				IP address:
			</td>
			<td>
				<asp:Literal ID="lblIP" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				date:
			</td>
			<td>
				<asp:Literal ID="lblDate" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				email:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" onblur="CheckFileName()" ID="txtEmail" runat="server" Columns="60"
					MaxLength="200" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtEmail" ID="RequiredFieldValidator1"
					runat="server" ErrorMessage="Required" Display="Dynamic" />
				<asp:TextBox runat="server" ValidationGroup="inputForm" ID="txtFileValid" MaxLength="25" Columns="25" Style="display: none;" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				name:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtName" runat="server" Columns="60" MaxLength="200" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtName" ID="RequiredFieldValidator3"
					runat="server" ErrorMessage="Required" Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				url:
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtURL" runat="server" Columns="60" MaxLength="200" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				&nbsp;
			</td>
			<td>
				<div class="table-subblock">
					<b class="caption">approved:</b>&nbsp;&nbsp;
					<asp:CheckBox ID="chkApproved" runat="server" />
				</div>
				<div class="table-subblock">
					<b class="caption">spam:</b>&nbsp;&nbsp;
					<asp:CheckBox ID="chkSpam" runat="server" />
				</div>
			</td>
		</tr>
		<tr>
			<td class="tablecaption">
				comment:
			</td>
			<td>
				<asp:TextBox Style="height: 250px; width: 600px;" ID="txtComment" runat="server" TextMode="MultiLine" Rows="15" Columns="80" />
			</td>
		</tr>
	</table>
</fieldset>
<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Save" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<input type="button" id="btnCancel" value="Cancel" onclick="cancelEditing()" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button ValidationGroup="deleteForm" ID="btnDeleteButton" runat="server" OnClientClick="return DeleteItem()" Text="Delete" />
<br />
<div style="display: none;">
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
	<asp:Button ValidationGroup="deleteForm" ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
</div>
<script type="text/javascript">

	function DeleteItem() {
		var opts = {
			"No": function () { cmsAlertModalClose(); },
			"Yes": function () { ClickDeleteItem(); }
		};

		cmsAlertModalSmallBtns('Are you sure you want to delete this comment?', opts);

		return false;
	}

	function ClickDeleteItem() {
		$('#<%=btnDelete.ClientID %>').click();
	}

	function SubmitPage() {
		setTimeout("ClickBtn();", 250);
	}
	function ClickBtn() {
		$('#<%=btnSave.ClientID %>').click();
	}

	function cancelEditing() {
		window.setTimeout("location.href = './<%=ReturnPageURL %>';", 250);
	}

</script>
