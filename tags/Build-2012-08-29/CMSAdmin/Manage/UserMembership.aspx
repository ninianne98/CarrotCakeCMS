<%@ Page Title="Manage Users" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserMembership.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.UserMembership" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Manage Users
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<a href="./UserAdd.aspx">
			<img class="imgNoBorder" src="/manage/images/add.png" alt="Add" title="Add" />
			Add User</a>
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="username ASC" ID="dvMembers" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular" OnDataBound="dvMembers_DataBound">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./User.aspx?id={0}", Eval("ProviderUserKey")) %>'><img class="imgNoBorder" src="/Manage/images/application_edit.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="username" HeaderText="Username">
					<ItemTemplate>
						<%# Eval("UserName")%>
					</ItemTemplate>
				</carrot:CarrotHeaderSortTemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="Email" HeaderText="Email">
					<ItemTemplate>
						<%# Eval("Email")%>
					</ItemTemplate>
				</carrot:CarrotHeaderSortTemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="CreationDate" HeaderText="Created On">
					<ItemTemplate>
						<%# string.Format("{0:d}", Eval("CreationDate") )%>
					</ItemTemplate>
				</carrot:CarrotHeaderSortTemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="IsLockedOut" HeaderText="Status" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<asp:Image ID="imgLocked" runat="server" ImageUrl="/Manage/images/user.png" AlternateText="Unlocked" />
						<asp:HiddenField ID="hdnIsLockedOut" Visible="false" runat="server" Value='<%#Eval("IsLockedOut") %>' />
					</ItemTemplate>
				</carrot:CarrotHeaderSortTemplateField>
			</Columns>
		</carrot:CarrotGridView>
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/Manage/images/lock.png" />
	</div>
</asp:Content>
