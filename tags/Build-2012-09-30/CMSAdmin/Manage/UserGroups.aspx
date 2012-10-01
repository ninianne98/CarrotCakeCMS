<%@ Page Title="User Groups" Language="C#" MasterPageFile="~/Manage/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserGroups.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.UserGroups" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Manage User Groups
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<a href="./UserGroupAddEdit.aspx">
			<img class="imgNoBorder" src="/manage/images/add.png" alt="Add" title="Add" />
			Add Group</a>
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="RoleName ASC" ID="gvRoles" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./UserGroupAddEdit.aspx?id={0}", Eval("RoleId")) %>'><img class="imgNoBorder" src="/Manage/images/application_edit.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="RoleName" HeaderText="Role Name" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
