<%@ Page Title="Forgot Password?" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<table style="width: 380px;">
		<tr>
			<td class="tableback">
				<div style="height: 10px; width: 20px; border: 1px solid #ffffff;">
					&nbsp;
				</div>
			</td>
			<td class="tableback">
				<b class="caption">email address</b>&nbsp;<br />
				<asp:TextBox ID="txtEmail" runat="server" Width="310px" MaxLength="90" ValidationGroup="loginTemplate" />
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
				<asp:Button ID="cmdReset" runat="server" Text="Reset" CommandName="Login" OnClick="cmdReset_Click" ValidationGroup="loginTemplate" />
			</td>
			<td class="tableback" align="right">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td class="tableback">
				&nbsp;
			</td>
			<td class="tableback" align="right">
				<div class="ui-widget" id="divMsg" runat="server">
					<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
						<p>
							<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
							<asp:Label ID="FailureText" runat="server" EnableViewState="False" />
						</p>
					</div>
				</div>
			</td>
			<td class="tableback" align="right">
				&nbsp;
			</td>
		</tr>
	</table>
	<div style="width: 300px; text-align: left;">
		<div runat="server" id="divLogonLink">
			<p>
				Click <a href="./logon.aspx">here </a>to logon.
			</p>
		</div>
	</div>
	<div style="display: none;">
		<asp:Label ID="lblErr" runat="server" Text="" />
	</div>
</asp:Content>
