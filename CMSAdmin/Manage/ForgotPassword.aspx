<%@ Page Title="Forgot Password?" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
	</p>
	<table width="380px">
		<tr>
			<td class="tableback">
				&nbsp;
			</td>
			<td class="tableback">
				&nbsp;<%--<b class="caption">email address</b>&nbsp;--%>
			</td>
			<td class="tableback">
				<b class="caption">email address</b>&nbsp;<br />
				<asp:TextBox ID="txtEmail" runat="server" Width="300px" MaxLength="80" ValidationGroup="loginTemplate"></asp:TextBox>
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
				<asp:Button ID="cmdCancel" runat="server" OnClick="cmdCancel_Click" Text="Cancel" />
				&nbsp;
				<asp:Button ID="cmdReset" runat="server" Text="Reset" CommandName="Login" OnClick="cmdReset_Click" ValidationGroup="loginTemplate">
				</asp:Button>
			</td>
			<td class="tableback" align="right">
				&nbsp;
			</td>
		</tr>
	</table>
	<div style="width: 310px; text-align: left">
		<asp:Label ID="FailureText" runat="server" EnableViewState="False"></asp:Label>
		<div runat="server" id="divLogonLink">
			<p>
				Click <a href="./logon.aspx">here </a>to logon.
			</p>
		</div>
	</div>
</asp:Content>
