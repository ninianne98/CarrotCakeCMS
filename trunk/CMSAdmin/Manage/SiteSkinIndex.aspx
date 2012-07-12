<%@ Page Title="Site Skin Index" Language="C#" MasterPageFile="~/Manage/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteSkinIndex.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.SiteSkinIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Skin Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div id="SortableGrid">
		<carrot:CarrotGridView DefaultSort="Caption desc" CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false"
			HeaderStyle-CssClass="tablehead" AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./SiteSkinEdit.aspx?path={0}", Eval("EncodedPath")) %>'><img border="0" src="/Manage/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead0" runat="server" CommandName="Caption">Caption</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("Caption")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead1" runat="server" CommandName="TemplatePath">Template Path</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("TemplatePath")%>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
