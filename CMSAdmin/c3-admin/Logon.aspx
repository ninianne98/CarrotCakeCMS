<%@ Page Title="Logon" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.Logon" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Panel ID="panelLogin" runat="server" DefaultButton="loginTemplate$cmdLogon">
		<asp:Login ID="loginTemplate" runat="server" OnLoggingIn="loginTemplate_LoggingIn" OnLoggedIn="loginTemplate_LoggedIn">
			<LayoutTemplate>
				<div style="text-align: left;">
					<table style="width: 300px;">
						<tr>
							<td>
								<div style="height: 35px; width: 50px; border: 1px solid #ffffff;">
								</div>
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								&nbsp;<b class="caption">username</b>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="UserName" ErrorMessage="!"
									ToolTip="Username is required" ValidationGroup="loginTemplate" Display="Dynamic" Text="**" />
								<br />
								<asp:TextBox ID="UserName" runat="server" Width="180px" MaxLength="60" ValidationGroup="loginTemplate" TabIndex="1" />
							</td>
							<td rowspan="2">
								<div style="height: 50px; width: 75px; text-align: right; border: 1px solid #ffffff;">
									<a href="/">
										<img class="imgNoBorder" src="/c3-admin/images/house_go.png" alt="Homepage" title="Homepage" /></a>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div style="height: 35px; width: 10px; border: 1px solid #ffffff;">
								</div>
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								<br />
								&nbsp;<b class="caption">password</b>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validationError" ForeColor="" ControlToValidate="Password" ErrorMessage="!"
									ToolTip="Password is required" ValidationGroup="loginTemplate" Display="Dynamic" Text="**" />
								<br />
								<asp:TextBox ID="Password" runat="server" TextMode="Password" Width="180px" MaxLength="60" ValidationGroup="loginTemplate" TabIndex="2" />
							</td>
						</tr>
						<tr>
							<td>
								<div style="height: 25px; width: 10px; border: 1px solid #ffffff;">
								</div>
							</td>
							<td>
								&nbsp;
							</td>
							<td>
								<div style="float: right; clear: both; margin-right: 10px;">
									<asp:Button ID="cmdLogon" runat="server" Text="Logon" CommandName="Login" OnClick="cmdLogon_Click" ValidationGroup="loginTemplate" TabIndex="3" />
								</div>
							</td>
							<td>
								&nbsp;
							</td>
						</tr>
					</table>
					<div style="width: 400px; text-align: left; clear: both;">
						<div class="ui-widget" id="divMsg" runat="server">
							<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
								<p>
									<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
									<asp:Label ID="FailureText" runat="server" EnableViewState="False" />
								</p>
							</div>
						</div>
						<div>
							<p>
								<a runat="server" id="lnkForgot" href="#">Forgot Password?</a>
							</p>
						</div>
					</div>
				</div>
			</LayoutTemplate>
		</asp:Login>
	</asp:Panel>
</asp:Content>
