<%@ Page Title="Create First Admin" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="CreateFirstAdmin.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.CreateFirstAdmin" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<style type="text/css">
		#CreateUserForm table {
			width: 400px !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="height: 20px; width: 18px; float: right; border: 1px solid #ffffff; clear: both;">
		<a href="/">
			<img class="imgNoBorder" src="/c3-admin/images/house_go.png" alt="Homepage" title="Homepage" /></a>
	</div>
	<div id="CreateUserForm">
		<asp:CreateUserWizard ID="createWizard" runat="server" Visible="false" OnCreatingUser="createWizard_CreatingUser" OnCreatedUser="createWizard_CreatedUser"
			CreateUserButtonType="Button" CancelButtonText="Cancel" CancelButtonType="Button" CancelDestinationPageUrl="./Logon.aspx" DisplayCancelButton="true"
			LoginCreatedUser="false" RequireEmail="true">
			<WizardSteps>
				<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
					<ContentTemplate>
						<div>
							<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False" />
						</div>
						<table>
							<tr>
								<td style="width: 125px;">
									<b class="caption">
										<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="User Name" />
									</b>
								</td>
								<td style="width: 210px;">
									<asp:TextBox Style="width: 140px;" ValidationGroup="createWizard" ID="UserName" runat="server" TabIndex="1" />
									<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="UserName" ErrorMessage="!"
										ToolTip="Username is required." ValidationGroup="createWizard" Display="Dynamic" Text="**" />
								</td>
							</tr>
							<tr>
								<td>
									<b class="caption">
										<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password " />
									</b>
								</td>
								<td>
									<asp:TextBox Style="width: 140px;" ValidationGroup="createWizard" ID="Password" runat="server" TextMode="Password" TabIndex="2" />
									<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="Password" ErrorMessage="!"
										ToolTip="Password is required." ValidationGroup="createWizard" Display="Dynamic" Text="**" />
								</td>
							</tr>
							<tr>
								<td>
									<b class="caption">
										<asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword" Text="Password (confirm) " />
									</b>
								</td>
								<td>
									<asp:TextBox Style="width: 140px;" ValidationGroup="createWizard" ID="ConfirmPassword" runat="server" TextMode="Password" TabIndex="3" />
									<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="ConfirmPassword" ErrorMessage="!"
										ToolTip="Confirm Password is required." ValidationGroup="createWizard" Display="Dynamic" Text="**" />
									<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" CssClass="validationError" ForeColor="" ControlToValidate="ConfirmPassword"
										Display="Dynamic" ErrorMessage="!!" ToolTip="Confirm Password does not match Password." ValidationGroup="createWizard" />
								</td>
							</tr>
							<tr>
								<td>
									<b class="caption">
										<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail " />
									</b>
								</td>
								<td>
									<asp:TextBox Style="width: 175px;" ValidationGroup="createWizard" ID="Email" runat="server" TabIndex="4" />
									<asp:RequiredFieldValidator ID="EmailRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="Email" ErrorMessage="!" ToolTip="E-mail is required."
										ValidationGroup="createWizard" Display="Dynamic" Text="**" />
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
						<p>
							Click <a href="<%=SiteFilename.LogonURL %>">here </a>to logon.
						</p>
					</ContentTemplate>
				</asp:CompleteWizardStep>
			</WizardSteps>
		</asp:CreateUserWizard>
		<asp:Label ID="lblLogon" runat="server" Visible="false">
			<p> 
				Click <a href="<%=SiteFilename.LogonURL %>">here </a>to logon.
			</p>
		</asp:Label>
	</div>
</asp:Content>
