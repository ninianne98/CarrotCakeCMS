<%@ Page Title="Site Skin Edit" ValidateRequest="false" Language="C#" MasterPageFile="~/Manage/MasterPages/Main.Master" AutoEventWireup="true"
	CodeBehind="SiteSkinEdit.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.SiteSkinEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Skin Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<h2>
		<asp:Literal ID="litFileName" runat="server"></asp:Literal></h2>
	<p>
		<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtPageContents" ID="RequiredFieldValidator1"
			runat="server" ErrorMessage="Skin File Contents Required" Display="Dynamic"></asp:RequiredFieldValidator>
		<br />
	</p>
	<asp:TextBox Style="height: 450px; width: 790px;" ID="txtPageContents" runat="server" TextMode="MultiLine" Rows="15" Columns="100" />
	<p>
		<asp:Button ValidationGroup="inputForm" ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" id="btnCancel" value="Cancel" onclick="location.href='./SiteSkinIndex.aspx';" />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
