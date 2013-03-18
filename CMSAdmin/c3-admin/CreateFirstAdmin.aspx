<%@ Page Title="Create First Admin" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="CreateFirstAdmin.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.CreateFirstAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<style type="text/css">
		#CreateUserForm table {
			width: 360px !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="height: 20px; width: 18px; float: right; border: 1px solid #ffffff; clear: both;">
		<a href="/">
			<img class="imgNoBorder" src="/c3-admin/images/house_go.png" alt="Homepage" title="Homepage" /></a>
	</div>
	<div id="CreateUserForm" style="width: 360px;">
		<asp:CreateUserWizard ID="createWizard" runat="server" Visible="false" OnCreatingUser="createWizard_CreatingUser" OnCreatedUser="createWizard_CreatedUser"
			CreateUserButtonType="Button" CancelButtonText="Cancel" CancelButtonType="Button" CancelDestinationPageUrl="./Logon.aspx" DisplayCancelButton="true"
			LoginCreatedUser="false" RequireEmail="true">
			<WizardSteps>
				<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
					<ContentTemplate>
						<div>
							<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
						</div>
						<table cellpadding="2px" cellspacing="2px" border="0" style="width: 380px;">
							<tr>
								<td class="tableback">
									<b class="caption">
										<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name </asp:Label></b>
								</td>
								<td class="tableback">
									<asp:TextBox Width="140px" ID="UserName" runat="server" />
									<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="!" ToolTip="Username is required." ValidationGroup="createWizard"
										Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								</td>
							</tr>
							<tr>
								<td class="tableback">
									<b class="caption">
										<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password " /></b>
								</td>
								<td class="tableback">
									<asp:TextBox Width="140px" ID="Password" runat="server" TextMode="Password" />
									<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="!" ToolTip="Password is required." ValidationGroup="createWizard"
										Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								</td>
							</tr>
							<tr>
								<td class="tableback">
									<b class="caption">
										<asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword" Text="Confirm Password " />
								</td>
								<td class="tableback">
									<asp:TextBox Width="140px" ID="ConfirmPassword" runat="server" TextMode="Password" />
									<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="!" ToolTip="Confirm Password is required."
										ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
									<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="!"
										ValidationGroup="createWizard" />
								</td>
							</tr>
							<tr>
								<td class="tableback">
									<b class="caption">
										<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail " /></b>
								</td>
								<td class="tableback">
									<asp:TextBox Width="200px" ID="Email" runat="server" />
									<asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="!" ToolTip="E-mail is required." ValidationGroup="createWizard"
										Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
								</td>
							</tr>
						</table>
					</ContentTemplate>
				</asp:CreateUserWizardStep>
				<asp:CompleteWizardStep ID="CreateUserWizardStep2" runat="server">
					<ContentTemplate>
						<h3>
							Account Created</h3>
						<p>
							&nbsp;
						</p>
						<p>
							Click <a href="./logon.aspx">here </a>to logon.
						</p>
					</ContentTemplate>
				</asp:CompleteWizardStep>
			</WizardSteps>
		</asp:CreateUserWizard>
		<asp:Label ID="lblLogon" runat="server" Visible="false">
			<p> 
				Click <a href="./logon.aspx">here </a>to logon.
			</p>
		</asp:Label>
	</div>
</asp:Content>
