<%@ Page Title="Site Skin Index" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteSkinIndex.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteSkinIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Skin Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="SortableGrid">
		<carrot:CarrotGridView DefaultSort="Caption ASC" CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No skins found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./SiteSkinEdit.aspx?path={0}", Eval("EncodedPath")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="Caption" HeaderText="Caption" DataField="Caption" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="TemplatePath" HeaderText="Template Path" DataField="TemplatePath" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
