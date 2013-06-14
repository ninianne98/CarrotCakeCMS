<%@ Page Title="Text Widget Index" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="TextWidgetIndex.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.TextWidgetIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Text Widget Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		These assemblies and classes listed below will be used to pre-process the text content types selected when rendered in the CMS via the associated page.
		If you don't want a particular class to be used for processing, simply uncheck all boxes on that entry's row and it will no be used for processing.
	</p>
	<div style="height: 50px; margin-top: 10px; margin-bottom: 10px;">
		<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
	</div>
	<div class="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="DisplayName ASC" ID="gvContent" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No content found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:BoundField HeaderText="Display Name" DataField="DisplayName" />
				<asp:BoundField HeaderText="Assembly String" DataField="AssemblyString" />
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						Body
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkSelect1" runat="server" value='<%# Eval("TextWidgetPickerID") %>' Checked='<%# Eval("ProcessBody") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						Plain Text
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkSelect2" runat="server" value='<%# Eval("TextWidgetPickerID") %>' Checked='<%# Eval("ProcessPlainText") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						HTML Text
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkSelect3" runat="server" value='<%# Eval("TextWidgetPickerID") %>' Checked='<%# Eval("ProcessHTMLText") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						Comment
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkSelect4" runat="server" value='<%# Eval("TextWidgetPickerID") %>' Checked='<%# Eval("ProcessComment") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						Snippet
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkSelect5" runat="server" value='<%# Eval("TextWidgetPickerID") %>' Checked='<%# Eval("ProcessSnippet") %>' />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
