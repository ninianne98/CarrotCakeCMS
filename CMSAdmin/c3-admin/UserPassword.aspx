<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="UserPassword.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.UserPassword" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Profile
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<fieldset style="width: 360px;">
		<legend>
			<label>
				Update Profile
			</label>
		</legend>
		<table style="width: 380px;">
			<tr>
				<td colspan="2">
					<p>
						<asp:Label ID="lblEmail" runat="server"></asp:Label>
						&nbsp;</p>
				</td>
			</tr>
			<tr>
				<td class="tablecontents">
					<b class="caption">Email</b>
				</td>
				<td class="tablecontents">
					<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtEmail" runat="server" ValidationGroup="changeEmail" />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="!" ToolTip="Email is required." ValidationGroup="changeEmail"
						Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
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
			<tr>
				<td colspan="2">
					<p style="text-align: right; padding-right: 40px;">
						<asp:Button ID="btnSaveEmail" ValidationGroup="changeEmail" runat="server" Text="Submit" OnClick="btnSaveEmail_Click" />
					</p>
				</td>
			</tr>
		</table>
	</fieldset>
	<fieldset style="width: 390px;">
		<legend>
			<label>
				Change Password
			</label>
		</legend>
		<asp:Panel ID="pnlChangeUserInfo" runat="server" Visible="true">
			<asp:ChangePassword runat="server" ID="changePassword" ChangePasswordButtonType="Button" DisplayUserName="false">
				<ChangePasswordTemplate>
					<div style="width: 380px">
						<p style="color: #FF0000;">
							<asp:Label ID="FailureText" runat="server" EnableViewState="False"></asp:Label>
						</p>
						<table style="width: 350px;">
							<tr>
								<td class="tablecontents">
									<b class="caption">Current Password </b>
								</td>
								<td class="tablecontents">
									<asp:TextBox onkeypress="return ProcessKeyPress(event)" ValidationGroup="changePassword" Width="140px" MaxLength="100" ID="CurrentPassword" TextMode="Password"
										runat="server" />
									<asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" ErrorMessage="!" ToolTip="CurrentPassword is required."
										ValidationGroup="changePassword" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								</td>
							</tr>
							<tr>
								<td class="tablecontents">
									<b class="caption">New Password </b>
								</td>
								<td class="tablecontents">
									<asp:TextBox onkeypress="return ProcessKeyPress(event)" ValidationGroup="changePassword" Width="140px" ID="NewPassword" runat="server" TextMode="Password" />
									<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="NewPassword" ErrorMessage="!" ToolTip="Password is required." ValidationGroup="changePassword"
										Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								</td>
							</tr>
							<tr>
								<td class="tablecontents">
									<b class="caption">Confirm Password </b>
								</td>
								<td class="tablecontents">
									<asp:TextBox onkeypress="return ProcessKeyPress(event)" ValidationGroup="changePassword" Width="140px" ID="ConfirmNewPassword" runat="server" TextMode="Password" />
									<asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" ErrorMessage="!" ToolTip="Confirm Password is required."
										ValidationGroup="changePassword" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
									<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="!!"
										ToolTip="Both new passwords must match." ValidationGroup="createWizard" />
								</td>
							</tr>
						</table>
						<p style="text-align: right; padding-right: 40px;">
							<asp:Button ID="btnChange" ValidationGroup="createWizard" runat="server" CommandName="ChangePassword" Text="Submit" />
						</p>
					</div>
				</ChangePasswordTemplate>
				<SuccessTemplate>
					<p>
						Your Password has been changed.
					</p>
				</SuccessTemplate>
			</asp:ChangePassword>
		</asp:Panel>
	</fieldset>
</asp:Content>
