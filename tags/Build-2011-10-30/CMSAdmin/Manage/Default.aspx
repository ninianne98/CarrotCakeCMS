<%@ Page Title="Dashboard" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Default" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Dashboard!
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<br />
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
				<asp:TextBox ValidationGroup="inputForm" ID="txtSiteName" MaxLength="200" Columns="80" Style="width: 425px;" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtSiteName" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
					Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Site URL
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" ID="txtURL" MaxLength="100" Columns="80" Style="width: 425px;" runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtURL" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
					Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Keywords
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" ID="txtKey" MaxLength="200" Columns="80" Style="width: 425px;" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				Description
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox>
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
<%--	<asp:Panel ID="pnlFolder" runat="server">
		<table width="600">
			<tr>
				<td valign="top" class="tablecaption">
					Virtual Site Folder
				</td>
				<td valign="top">
					<asp:TextBox ValidationGroup="inputForm" ID="txtFolder" MaxLength="200" Columns="80" Style="width: 425px;" runat="server"></asp:TextBox>
				</td>
			</tr>
		</table>
		<p>
			Virtual Site Folder is required to distinguish between multiple sites hosted in the same web root. Examples might be /site1/ and /site2/ when two sites
			are sharing the same web root. If a single site resides in the web root, no value is required.
		</p>
	</asp:Panel>--%>
	<br />
	<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" Text="Apply Changes" OnClick="btnSave_Click" />
	<br />
</asp:Content>
