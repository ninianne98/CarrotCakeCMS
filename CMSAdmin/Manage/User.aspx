<%@ Page Title="User" Language="C#" MasterPageFile="MasterPages/Main.Master" ValidateRequest="false" AutoEventWireup="true"
	CodeBehind="User.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.User" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	User Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<table cellspacing="2">
		<tr>
			<td>
				<b class="caption">
					<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name</asp:Label>&nbsp;&nbsp;</b>
			</td>
			<td>
				<asp:Label ID="lblUserName" runat="server" Columns="60" MaxLength="100"></asp:Label>
				<asp:TextBox Width="140px" ID="UserName" runat="server" MaxLength="50" />
				<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="!" ToolTip="Username is required."
					ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
			</td>
		</tr>
		<tr>
			<td>
				<b class="caption">
					<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail " />&nbsp;&nbsp;</b>
			</td>
			<td>
				<asp:TextBox Width="200px" ID="Email" runat="server" MaxLength="100" />
				<asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="!" ToolTip="E-mail is required."
					ValidationGroup="createWizard" Display="Dynamic" Text="*&nbsp;&nbsp;&nbsp;" />
			</td>
		</tr>
		<tr>
			<td>
				<b class="caption">Locked&nbsp;&nbsp;</b>
			</td>
			<td>
				<asp:CheckBox ID="chkLocked" runat="server" />
			</td>
		</tr>
	</table>
	<br />
	<table cellspacing="2">
		<tr>
			<td valign="top">
				<div id="SortableGrid" style="width: 300px;">
					<asp:GridView CssClass="datatable" ID="gvRoles" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
						AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
						<Columns>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:CheckBox ID="chkSelected" runat="server" />
									<asp:HiddenField Value='<%#Eval("RoleName") %>' ID="hdnRoleId" runat="server" Visible="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="RoleName" HeaderText="Role Name" />
						</Columns>
					</asp:GridView>
				</div>
			</td>
			<td valign="top">
				&nbsp;&nbsp;&nbsp;
			</td>
			<td valign="top">
				<div id="SortableGrid" style="width: 400px;">
					<asp:GridView CssClass="datatable" ID="gvSites" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
						AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
						<Columns>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:CheckBox ID="chkSelected" runat="server" />
									<asp:HiddenField Value='<%#Eval("SiteID") %>' ID="hdnSiteID" runat="server" Visible="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="SiteName" HeaderText="Site Name" />
						</Columns>
					</asp:GridView>
				</div>
			</td>
		</tr>
	</table>
	<br />
	<asp:Button ID="btnApply" runat="server" Text="Save" OnClick="btnApply_Click" />
	<input type="button" id="btnCancel" runat="server" value="Cancel" onclick="javascript:window.location='./UserMembership.aspx';" />
	<br />
</asp:Content>
