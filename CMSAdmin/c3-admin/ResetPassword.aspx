<%@ Page Title="Reset Password" Language="C#" MasterPageFile="MasterPages/Public.Master"
	AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ResetPassword" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="ui-widget" id="divMsg" runat="server">
		<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
			<p>
				<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
				<asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
			</p>
		</div>
	</div>
	<asp:PlaceHolder ID="phReset" runat="server">
		<table style="width: 400px;">
			<tr>
				<td>
					<div style="height: 35px; width: 50px; border: 1px solid #ffffff;">
					</div>
				</td>
				<td>&nbsp;
				</td>
				<td>&nbsp;<b class="caption">email</b>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" CssClass="validationError" ForeColor=""
						ControlToValidate="txtEmail" ErrorMessage="!" ToolTip="email is required" ValidationGroup="inputForm"
						Display="Dynamic" Text="**" />
					<br />
					<asp:TextBox ID="txtEmail" Style="width: 200px;" ValidationGroup="inputForm" runat="server" CssClass="form-control" TabIndex="0" />
				</td>
				<td rowspan="3">
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
				<td>&nbsp;
				</td>
				<td>&nbsp;<b class="caption">new password</b>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validationError" ForeColor=""
						ControlToValidate="txtNewPassword" ErrorMessage="!" ToolTip="password is required" ValidationGroup="inputForm"
						Display="Dynamic" Text="**" />
					<br />
					<asp:TextBox ID="txtNewPassword" Style="width: 200px;" ValidationGroup="inputForm" runat="server" CssClass="form-control" TextMode="Password" TabIndex="1" />
				</td>
			</tr>
			<tr>
				<td>
					<div style="height: 35px; width: 10px; border: 1px solid #ffffff;">
					</div>
				</td>
				<td>&nbsp;
				</td>
				<td>
					<br />
					&nbsp;<b class="caption">confirm password</b>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validationError" ForeColor=""
						ControlToValidate="txtConfirmPassword" ErrorMessage="!" ToolTip="confirm password is required" ValidationGroup="inputForm"
						Display="Dynamic" Text="**" />
					<asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="validationError" ForeColor=""
						ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="!!"
						ToolTip="Confirm Password does not match Password." ValidationGroup="inputForm" />
					<br />
					<asp:TextBox ID="txtConfirmPassword" Style="width: 200px;" ValidationGroup="inputForm" runat="server" CssClass="form-control" TextMode="Password" TabIndex="2" />
				</td>
			</tr>
			<tr>
				<td>
					<div style="height: 25px; width: 10px; border: 1px solid #ffffff;">
					</div>
				</td>
				<td>&nbsp;
				</td>
				<td>
					<div style="float: right; clear: both; margin-right: 10px;">
						<asp:Button ID="btnReset" runat="server" ValidationGroup="inputForm" Text="Set Password" OnClick="btnReset_Click" TabIndex="3" />
						<asp:HiddenField ID="hdnToken" runat="server" />
					</div>
				</td>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</asp:PlaceHolder>
	<div style="width: 350px; text-align: left;">
		<asp:PlaceHolder runat="server" ID="phLogonLink">
			<p>
				Click <a href="<%=SiteFilename.LogonURL %>">here</a> to logon.
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="phExpired">
			<p>
				Reset link is not valid.  Please make a new password reset request.
			</p>
			<p>
				<a runat="server" id="lnkForgot" href="#">Forgot Password?</a>
			</p>
		</asp:PlaceHolder>
	</div>

</asp:Content>
