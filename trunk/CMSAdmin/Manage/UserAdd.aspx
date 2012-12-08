<%@ Page Title="Create User" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.UserAdd" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Create An Account
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:CreateUserWizard ID="createWizard" runat="server" OnCreatingUser="createWizard_CreatingUser" OnCreatedUser="createWizard_CreatedUser"
		CreateUserButtonType="Button" CancelButtonText="Cancel" CancelButtonType="Button" CancelDestinationPageUrl="./UserMembership.aspx"
		DisplayCancelButton="true" LoginCreatedUser="false" RequireEmail="true">
		<WizardSteps>
			<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
				<ContentTemplate>
					<div>
						<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
					</div>
					<table cellpadding="2px" cellspacing="2px" border="0" width="380px">
						<tr>
							<td class="tableback">
								<b class="caption">
									<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name </asp:Label></b>
							</td>
							<td class="tableback">
								<asp:TextBox Width="140px" ID="UserName" runat="server" />
								<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="!" ToolTip="Username is required."
									ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
							</td>
						</tr>
						<tr>
							<td class="tableback">
								<b class="caption">
									<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password " /></b>
							</td>
							<td class="tableback">
								<asp:TextBox Width="140px" ID="Password" runat="server" TextMode="Password" />
								<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="!" ToolTip="Password is required."
									ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
							</td>
						</tr>
						<tr>
							<td class="tableback">
								<b class="caption">
									<asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword" Text="Confirm Password " />
							</td>
							<td class="tableback">
								<asp:TextBox Width="140px" ID="ConfirmPassword" runat="server" TextMode="Password" />
								<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="!"
									ToolTip="Confirm Password is required." ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
									Display="Dynamic" ErrorMessage="!" ValidationGroup="createWizard" />
							</td>
						</tr>
						<tr>
							<td class="tableback">
								<b class="caption">
									<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail " /></b>
							</td>
							<td class="tableback">
								<asp:TextBox Width="200px" ID="Email" runat="server" />
								<asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="!" ToolTip="E-mail is required."
									ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
							</td>
						</tr>
					</table>
				</ContentTemplate>
			</asp:CreateUserWizardStep>
			<asp:CompleteWizardStep ID="CreateUserWizardStep2" runat="server">
				<ContentTemplate>
					<h3>
						Account Created</h3>
				</ContentTemplate>
			</asp:CompleteWizardStep>
		</WizardSteps>
	</asp:CreateUserWizard>
</asp:Content>
