<%@ Page Title="Manage - Change Password" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="UserPassword.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.UserPassword" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Change Password
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
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
								<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="140px" ID="CurrentPassword" TextMode="Password" runat="server" />
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
								<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="140px" ID="NewPassword" runat="server" TextMode="Password" />
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
								<asp:TextBox onkeypress="return ProcessKeyPress(event)" Width="140px" ID="ConfirmNewPassword" runat="server" TextMode="Password" />
								<asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" ErrorMessage="!"
									ToolTip="Confirm Password is required." ValidationGroup="changePassword" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
									Display="Dynamic" ErrorMessage="!" ValidationGroup="createWizard" />
							</td>
						</tr>
					</table>
					<p style="text-align: right; padding-right: 40px;">
						<asp:Button ID="btnChange" runat="server" CommandName="ChangePassword" Text="Change Password" />
						<%--<input id="btnCancel" type="button" runat="server" value="Cancel" onclick="javascript:window.location='/default.aspx';" />--%>
					</p>
				</div>
				<%--<p>
					Click <a href="default.aspx">here</a> to return.</p>--%>
			</ChangePasswordTemplate>
			<SuccessTemplate>
				<p>
					Your Password has been changed.
				</p>
				<%--<p>
					Click <a href="default.aspx">here</a> to return.</p>--%>
			</SuccessTemplate>
		</asp:ChangePassword>
	</asp:Panel>
</asp:Content>
