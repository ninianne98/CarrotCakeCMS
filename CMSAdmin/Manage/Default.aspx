<%@ Page Title="Dashboard" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Default" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Dashboard!
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:Button ValidationGroup="inputForm2" ID="btnResetVars" runat="server" Text="Refresh Configs" OnClick="btnResetVars_Click" />
		<br />
	</p>
	<table width="600">
		<tr>
			<td valign="top" class="tablecaption">
				Site ID
			</td>
			<td valign="top">
				<asp:Literal ID="litID" runat="server"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Site Name
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtSiteName" MaxLength="200" Columns="80"
					Style="width: 425px;" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtSiteName" ID="RequiredFieldValidator1" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Site URL
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtURL" MaxLength="100" Columns="80"
					Style="width: 425px;" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtURL" ID="RequiredFieldValidator2" runat="server"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Meta Keywords
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtKey" MaxLength="200" Columns="80"
					Style="width: 425px;" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Meta Description
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="5"
					TextMode="MultiLine" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Hide from Search Engines
			</td>
			<td valign="top">
				<asp:CheckBox ID="chkHide" runat="server" />
			</td>
		</tr>
	</table>
	<br />
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" Text="Apply Changes" OnClick="btnSave_Click" />
	<br />
</asp:Content>
