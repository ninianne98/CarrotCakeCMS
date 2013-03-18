<%@ Page Title="CategoryIndex" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CategoryIndex.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.CategoryIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Category Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<a href="./CategoryAddEdit.aspx">
			<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add" />
			Add Category</a>
	</p>
	<p>
		<br />
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="CategoryText asc" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# String.Format("./CategoryAddEdit.aspx?id={0}", Eval("ContentCategoryID")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="CategoryText" HeaderText="Text" DataField="CategoryText" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="CategorySlug" HeaderText="Slug" DataField="CategorySlug" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsPublic" HeaderText="Public" AlternateTextFalse="Not Public"
					AlternateTextTrue="Public" ShowBooleanImage="true" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="UseCount" HeaderText="Count" DataField="UseCount" ItemStyle-HorizontalAlign="Center" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
