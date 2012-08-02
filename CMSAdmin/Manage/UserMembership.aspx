<%@ Page Title="Membership" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="UserMembership.aspx.cs"
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
			<img border="0" src="/manage/images/add.png" alt="Add" title="Add" />
			Add User</a>
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="username ASC" ID="dvMembers" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular" OnDataBound="dvMembers_DataBound">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./User.aspx?id={0}", Eval("ProviderUserKey")) %>'><img border="0" src="/Manage/images/application_edit.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead1" runat="server" CommandName="username">Username</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("UserName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead2" runat="server" CommandName="Email">Email</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("Email")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead3" runat="server" CommandName="CreationDate">Created On</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# string.Format("{0:d}", Eval("CreationDate") )%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead4" runat="server" CommandName="IsLockedOut">Status</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<asp:Image ID="imgLocked" runat="server" ImageUrl="/Manage/images/user_green.png" AlternateText="Unlocked" />
						<asp:HiddenField ID="hdnIsLockedOut" Visible="false" runat="server" Value='<%#Eval("IsLockedOut") %>' />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</carrot:CarrotGridView>
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/Manage/images/lock.png" />
	</div>
</asp:Content>
