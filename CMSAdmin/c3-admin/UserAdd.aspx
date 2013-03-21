<%@ Page Title="Create User" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.UserAdd" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Create An Account
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div id="CreateUserForm">
		<asp:CreateUserWizard ID="createWizard" runat="server" OnCreatingUser="createWizard_CreatingUser" OnCreatedUser="createWizard_CreatedUser" CreateUserButtonType="Button"
			CancelButtonText="Cancel" CancelButtonType="Button" CancelDestinationPageUrl="./UserMembership.aspx" DisplayCancelButton="true" LoginCreatedUser="false"
			RequireEmail="true">
			<WizardSteps>
				<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
					<ContentTemplate>
						<div>
							<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False" />
						</div>
						<table>
							<tr>
								<td style="width: 165px;">
									<b class="caption">
										<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="User Name" />
										<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="!" ToolTip="Username is required." ValidationGroup="createWizard"
											Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" /></b>
								</td>
								<td style="width: 210px;">
									<asp:TextBox Style="width: 140px;" ValidationGroup="createWizard" ID="UserName" runat="server" TabIndex="1" />
								</td>
							</tr>
							<tr>
								<td>
									<b class="caption">
										<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password " />
										<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="!" ToolTip="Password is required." ValidationGroup="createWizard"
											Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" /></b>
								</td>
								<td>
									<asp:TextBox Style="width: 140px;" ValidationGroup="createWizard" ID="Password" runat="server" TextMode="Password" TabIndex="2" />
								</td>
							</tr>
							<tr>
								<td>
									<b class="caption">
										<asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword" Text="Password (confirm) " />
										<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="!" ToolTip="Confirm Password is required."
											ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
										<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="!"
											ToolTip="Confirm Password does not match Password." ValidationGroup="createWizard" />
									</b>
								</td>
								<td>
									<asp:TextBox Style="width: 140px;" ValidationGroup="createWizard" ID="ConfirmPassword" runat="server" TextMode="Password" TabIndex="3" />
								</td>
							</tr>
							<tr>
								<td>
									<b class="caption">
										<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail " />
										<asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="!" ToolTip="E-mail is required." ValidationGroup="createWizard"
											Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
									</b>
								</td>
								<td>
									<asp:TextBox Style="width: 200px;" ValidationGroup="createWizard" ID="Email" runat="server" TabIndex="4" />
								</td>
							</tr>
						</table>
					</ContentTemplate>
					<CustomNavigationTemplate>
						<table>
							<tr>
								<td>
									<div style="height: 10px; width: 125px; border: 1px solid #ffffff;">
									</div>
								</td>
								<td>
									<asp:Button ID="StepNextButton" runat="server" Text="Create User" CommandName="MoveNext" ValidationGroup="createWizard" TabIndex="5" />
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									<asp:Button ID="CancelButtonButton" runat="server" Text="Cancel" CommandName="Cancel" TabIndex="6" />
								</td>
							</tr>
						</table>
					</CustomNavigationTemplate>
				</asp:CreateUserWizardStep>
				<asp:CompleteWizardStep ID="CreateUserWizardStep2" runat="server">
					<ContentTemplate>
						<h3>
							Account Created</h3>
						<p>
							&nbsp;
						</p>
					</ContentTemplate>
				</asp:CompleteWizardStep>
			</WizardSteps>
		</asp:CreateUserWizard>
	</div>
</asp:Content>
