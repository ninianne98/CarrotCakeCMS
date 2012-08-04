<%@ Page Title="User Group Add/Edit" Language="C#" MasterPageFile="~/Manage/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserGroupAddEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.UserGroupAddEdit" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	User Group Add/Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:TextBox ID="txtRoleName" Width="300px" MaxLength="100" runat="server"></asp:TextBox>
		<asp:RequiredFieldValidator ID="RoleNameRequired" runat="server" ControlToValidate="txtRoleName" ErrorMessage="!" ToolTip="Role is required."
			ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
	</p>
	<p>
		<br />
		<asp:Button ValidationGroup="createWizard" ID="btnApply" runat="server" Text="Save" OnClick="btnApply_Click" />
		<input type="button" id="btnCancel" runat="server" value="Cancel" onclick="javascript:window.location='./UserGroups.aspx';" />
		<br />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
