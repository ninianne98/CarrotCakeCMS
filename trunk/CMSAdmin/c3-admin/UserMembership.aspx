<%@ Page Title="Manage Users" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserMembership.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.UserMembership" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Manage Users
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<a href="./UserAdd.aspx">
			<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
			Add User</a>
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="username ASC" ID="dvMembers" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./User.aspx?id={0}", Eval("UserId")) %>'><img class="imgNoBorder" src="/c3-admin/images/application_edit.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="Username" HeaderText="Username" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="FullName_LastFirst" HeaderText="Name" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="EmailAddress" HeaderText="Email" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Created On" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsLockedOut" HeaderText="Status" ShowBooleanImage="true" AlternateTextTrue="Locked Out"
					AlternateTextFalse="Unlocked" ImagePathTrue="/c3-admin/images/lock.png" ImagePathFalse="/c3-admin/images/user.png" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
