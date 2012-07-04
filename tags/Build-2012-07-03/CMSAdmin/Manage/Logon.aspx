<%@ Page Title="Logon" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="Logon.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Logon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Panel ID="panelLogin" runat="server" DefaultButton="loginTemplate$cmdLogon">
		<asp:Login ID="loginTemplate" runat="server" OnLoggingIn="loginTemplate_LoggingIn" OnLoggedIn="loginTemplate_LoggedIn">
			<LayoutTemplate>
				<div style="text-align: left;">
					<table width="320px">
						<tr>
							<td class="tableback">
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								&nbsp;
							</td>
						</tr>
						<tr>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								&nbsp;<%--<b class="caption">username</b>&nbsp;--%>
							</td>
							<td class="tableback">
								&nbsp;<b class="caption">username</b>&nbsp;<br />
								<asp:TextBox ID="UserName" runat="server" Width="160px" MaxLength="60" ValidationGroup="loginTemplate"></asp:TextBox>
							</td>
							<td class="tableback">
								&nbsp;
							</td>
						</tr>
						<tr>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								&nbsp;<%--<b class="caption">password</b>&nbsp;--%>
							</td>
							<td class="tableback">
								<br />
								&nbsp;<b class="caption">password</b>&nbsp;<br />
								<asp:TextBox ID="Password" runat="server" TextMode="Password" Width="160px" MaxLength="60" ValidationGroup="loginTemplate"></asp:TextBox>
							</td>
							<td class="tableback">
								&nbsp;
							</td>
						</tr>
						<tr>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback" align="right">
								<br />
								<asp:Button ID="cmdLogon" runat="server" Text="Logon" CommandName="Login" OnClick="cmdLogon_Click" ValidationGroup="loginTemplate">
								</asp:Button>
							</td>
							<td class="tableback" align="right">
								&nbsp;
							</td>
						</tr>
					</table>
					<div style="width: 310px; text-align: left">
						<asp:Label ID="FailureText" runat="server" EnableViewState="False"></asp:Label>
						<br />
						<a href="ForgotPassword.aspx">Forgot Password?</a>
					</div>
				</div>
			</LayoutTemplate>
		</asp:Login>
	</asp:Panel>
</asp:Content>
