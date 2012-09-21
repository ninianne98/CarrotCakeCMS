<%@ Page Title="Page Index" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageIndex.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.PageIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucPageMenuItems.ascx" TagName="ucPageMenuItems" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Page Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<uc1:ucPageMenuItems ID="ucPageMenuItems1" runat="server" />
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="TitleBar ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular" OnDataBound="gvPages_DataBound">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./PageAddEdit.aspx?id={0}", Eval("Root_ContentID")) %>'><img  class="imgNoBorder"  src="/Manage/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit2" NavigateUrl='<%#  String.Format("./PageAddEdit.aspx?mode=raw&id={0}", Eval("Root_ContentID")) %>'><img  class="imgNoBorder"  src="/Manage/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit4" Target="_blank" NavigateUrl='<%#  String.Format("./PageExport.aspx?id={0}", Eval("Root_ContentID")) %>'><img  class="imgNoBorder"  src="/Manage/images/html_go.png" alt="Export latest version of this page" title="Export latest version of this page" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit3" NavigateUrl='<%#  String.Format("{0}?carrotedit=true", Eval("FileName")) %>'><img  class="imgNoBorder"  src="/Manage/images/overlays.png" alt="Advanced Editor" title="Advanced Editor" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/Manage/PageHistory.aspx?id=<%#Eval("Root_ContentID") %>');">
							<img class="imgNoBorder" src="/Manage/images/layout_content.png" alt="View Page History" title="View Page History" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/Manage/PageChildSort.aspx?pageid=<%#Eval("Root_ContentID") %>');">
							<img class="imgNoBorder" src="/Manage/images/chart_organisation.png" alt="Sort Sub Pages" title="Sort Sub Pages" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="titlebar" HeaderText="Titlebar" DataField="Titlebar" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="pagehead" HeaderText="Page Header" DataField="pagehead" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="Filename" HeaderText="Filename" DataField="Filename" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="NavMenuText" HeaderText="Nav Menu Text" DataField="NavMenuText" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="EditDate" HeaderText="Last Edited" DataField="EditDate" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Created On" DataField="CreateDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="PageActive" HeaderText="Active" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<asp:Image ID="imgActive" runat="server" ImageUrl="/Manage/images/application_lightning.png" AlternateText="Active" />
						<asp:HiddenField ID="hdnIsActive" Visible="false" runat="server" Value='<%#Eval("PageActive") %>' />
					</ItemTemplate>
				</carrot:CarrotHeaderSortTemplateField>
			</Columns>
		</carrot:CarrotGridView>
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/Manage/images/cancel.png" />
		<asp:HiddenField runat="server" ID="hdnActive" Visible="false" Value="/Manage/images/accept.png" />
	</div>
</asp:Content>
