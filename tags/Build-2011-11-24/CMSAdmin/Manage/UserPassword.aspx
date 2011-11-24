<%@ Page Title="Manage - Change Password" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true"
	CodeBehind="UserPassword.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.UserPassword" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Profile
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<table id="Table2" width="350px">
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
				<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="180px" MaxLength="100" ID="txtEmail" runat="server" />
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="!" ToolTip="Email is required."
					ValidationGroup="changeEmail" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<p style="text-align: right; padding-right: 40px;">
					<asp:Button ID="btnSaveEmail" ValidationGroup="changeEmail" runat="server" Text="Save Email" OnClick="btnSaveEmail_Click" />
				</p>
			</td>
		</tr>
	</table>
	<p>
	</p>
	<asp:Panel ID="pnlChangeUserInfo" runat="server" Visible="true">
		<asp:ChangePassword runat="server" ID="changePassword" ChangePasswordButtonType="Button" DisplayUserName="false">
			<ChangePasswordTemplate>
				<div style="width: 380px">
					<p style="color: #FF0000;">
						<asp:Label ID="FailureText" runat="server" EnableViewState="False"></asp:Label>
					</p>
					<table id="Table2" width="350px">
						<tr>
							<td class="tablecontents">
								<b class="caption">
									<asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Current Password </asp:Label>
								</b>
							</td>
							<td class="tablecontents">
								<asp:TextBox onkeypress="return ProcessKeyPress(event)" ValidationGroup="changePassword" Width="140px" MaxLength="100" ID="CurrentPassword"
									TextMode="Password" runat="server" />
								<asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" ErrorMessage="!"
									ToolTip="CurrentPassword is required." ValidationGroup="changePassword" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
							</td>
						</tr>
						<tr>
							<td class="tablecontents">
								<b class="caption">
									<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="NewPassword" Text="New Password " />
								</b>
							</td>
							<td class="tablecontents">
								<asp:TextBox onkeypress="return ProcessKeyPress(event)" ValidationGroup="changePassword" Width="140px" ID="NewPassword" runat="server"
									TextMode="Password" />
								<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="NewPassword" ErrorMessage="!" ToolTip="Password is required."
									ValidationGroup="changePassword" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
							</td>
						</tr>
						<tr>
							<td class="tablecontents">
								<b class="caption">
									<asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword" Text="Confirm Password " />
								</b>
							</td>
							<td class="tablecontents">
								<asp:TextBox onkeypress="return ProcessKeyPress(event)" ValidationGroup="changePassword" Width="140px" ID="ConfirmNewPassword"
									runat="server" TextMode="Password" />
								<asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" ErrorMessage="!"
									ToolTip="Confirm Password is required." ValidationGroup="changePassword" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
									Display="Dynamic" ErrorMessage="!" ValidationGroup="createWizard" />
							</td>
						</tr>
					</table>
					<p style="text-align: right; padding-right: 40px;">
						<asp:Button ID="btnChange" ValidationGroup="changePassword" runat="server" CommandName="ChangePassword" Text="Change Password" />
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
</asp:Content>
