<%@ Page Title="Edit Profile" Language="C#" ValidateRequest="false" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.UserProfile" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	User Preferences
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		&nbsp;</p>
	<div>
		<div class="ui-widget" id="divInfoMsg" runat="server" style="width: 400px;">
			<div class="ui-state-highlight ui-corner-all" style="padding: 0 .7em;">
				<p>
					<span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
					<asp:Literal ID="InfoMessage" runat="server" EnableViewState="False" />&nbsp;
				</p>
			</div>
		</div>
	</div>
	<div style="width: 700px;">
		<fieldset style="width: 400px;">
			<legend>
				<label>
					Edit Profile
				</label>
			</legend>
			<table style="width: 360px;">
				<tr>
					<td class="tablecontents" style="width: 100px;">
						<b class="caption">Email</b>
					</td>
					<td class="tablecontents">
						<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtEmail" runat="server" ValidationGroup="inputForm" />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="txtEmail" ErrorMessage="!"
							ToolTip="Email is required." ValidationGroup="inputForm" Display="Dynamic" Text="**" />
					</td>
				</tr>
				<tr>
					<td class="tablecontents">
						<b class="caption">Nickname</b>
					</td>
					<td class="tablecontents">
						<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtNickName" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="tablecontents">
						<b class="caption">First Name</b>
					</td>
					<td class="tablecontents">
						<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtFirstName" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="tablecontents">
						<b class="caption">Last Name</b>
					</td>
					<td class="tablecontents">
						<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtLastName" runat="server" />
					</td>
				</tr>
			</table>
		</fieldset>
		<fieldset style="width: 650px;">
			<legend>
				<label>
					Bio Data</label>
			</legend>
			<div runat="server" id="divCenter">
				<a href="javascript:cmsToggleTinyMCE('<%= reBody.ClientID %>');">Show/Hide Editor</a></div>
			<asp:TextBox ValidationGroup="inputForm" Style="height: 250px; width: 550px;" CssClass="mceEditor" ID="reBody" runat="server" TextMode="MultiLine" Rows="20"
				Columns="60" />
			<br />
		</fieldset>
		<p>
			<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="return SubmitPage()" Text="Save" />
		</p>
		<div style="display: none">
			<asp:Button ID="btnSaveEmail" ValidationGroup="inputForm" runat="server" Text="Submit" OnClick="btnSaveEmail_Click" />
		</div>
	</div>
	<script type="text/javascript">

		function SubmitPage() {
			var sc = SaveCommon();
			var ret = cmsIsPageValid();
			setTimeout("ClickSaveBtn();", 800);
			return ret;
		}

		function ClickSaveBtn() {
			if (cmsIsPageValid()) {
				$('#<%=btnSaveEmail.ClientID %>').click();
			}
		}

		function SaveCommon() {
			var ret = tinyMCE.triggerSave();

			return true;
		}
	</script>
</asp:Content>
