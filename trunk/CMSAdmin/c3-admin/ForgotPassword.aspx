<%@ Page Title="Forgot Password?" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="height: 25px; width: 200px; border: 1px solid #ffffff; clear: both;">
		&nbsp;
	</div>
	<table width="380px">
		<tr>
			<td class="tableback">
				<div style="height: 10px; width: 20px; border: 1px solid #ffffff;">
					&nbsp;
				</div>
			</td>
			<td class="tableback">
				<b class="caption">email address</b>&nbsp;<br />
				<asp:TextBox ID="txtEmail" runat="server" Width="310px" MaxLength="90" ValidationGroup="loginTemplate"></asp:TextBox>
			</td>
			<td class="tableback">
				<div style="height: 10px; width: 20px; border: 1px solid #ffffff;">
					&nbsp;
				</div>
			</td>
		</tr>
		<tr>
			<td class="tableback">
				&nbsp;
			</td>
			<td class="tableback" align="right">
				<br />
				<asp:Button ID="cmdCancel" runat="server" OnClick="cmdCancel_Click" Text="Cancel" />
				&nbsp;
				<asp:Button ID="cmdReset" runat="server" Text="Reset" CommandName="Login" OnClick="cmdReset_Click" ValidationGroup="loginTemplate"></asp:Button>
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
	<div style="display: none;">
		<asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
	</div>
</asp:Content>
