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
			RequireEmail="true" OnCreateUserError="createWizard_CreateUserError">
			<WizardSteps>
				<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
					<ContentTemplate>
						<div class="ui-widget" id="divMsg" runat="server">
							<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
								<p>
									<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
									<asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
								</p>
							</div>
						</div>
						<table style="width: 450px;">
							<tr>
								<td style="width: 180px;">
									<b class="caption">
										<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="User Name" />
									</b>
								</td>
								<td style="width: 300px;">
									<asp:TextBox Style="width: 140px;" ValidationGroup="createWizard" ID="UserName" runat="server" TabIndex="1" />
									<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="UserName" ErrorMessage="Username is required."
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
									<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="Password" ErrorMessage="Password is required."
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
									<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="ConfirmPassword" ErrorMessage="Confirm Password is required."
										ToolTip="Confirm Password is required." ValidationGroup="createWizard" Display="Dynamic" Text="**" />
									<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" CssClass="validationError" ForeColor="" ControlToValidate="ConfirmPassword"
										Display="Dynamic" ErrorMessage="Confirm Password does not match Password." ToolTip="Confirm Password does not match Password." ValidationGroup="createWizard" />
								</td>
							</tr>
							<tr>
								<td>
									<b class="caption">
										<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail " />
									</b>
								</td>
								<td>
									<asp:TextBox Style="width: 200px;" ValidationGroup="createWizard" ID="Email" runat="server" TabIndex="4" />
									<asp:RequiredFieldValidator ID="EmailRequired" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="Email" ErrorMessage="E-mail is required."
										ToolTip="E-mail is required." ValidationGroup="createWizard" Display="Dynamic" Text="**" />
								</td>
							</tr>
						</table>
					</ContentTemplate>
					<CustomNavigationTemplate>
						<table style="width: 300px;">
							<tr>
								<td>
									<div style="height: 10px; width: 125px; border: 1px solid #ffffff;">
									</div>
								</td>
								<td>
									<asp:Button ID="StepNextButton" runat="server" Text="Create User" CommandName="MoveNext" ValidationGroup="createWizard" TabIndex="5" OnClientClick="return ClickApplyBtn()" />
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
	<div style="display: none;">
		<asp:ValidationSummary ID="formValidationSummary" runat="server" ShowSummary="true" ValidationGroup="inputForm" />
	</div>
	<script type="text/javascript">
		function ClickApplyBtn() {
			if (cmsIsPageValid()) {
				return true;
			} else {
				cmsLoadPrettyValidationPopup('<%= formValidationSummary.ClientID %>');
				return false;
			}
		}
	</script>
</asp:Content>
