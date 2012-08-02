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
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./PageAddEdit.aspx?id={0}", Eval("Root_ContentID")) %>'><img border="0" src="/Manage/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit2" NavigateUrl='<%#  String.Format("./PageAddEdit.aspx?mode=raw&id={0}", Eval("Root_ContentID")) %>'><img border="0" src="/Manage/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit3" NavigateUrl='<%#  String.Format("{0}?carrotedit=true", Eval("FileName")) %>'><img border="0" src="/Manage/images/overlays.png" alt="Advanced Editor" title="Advanced Editor" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/Manage/PageHistory.aspx?id=<%#Eval("Root_ContentID") %>');">
							<img border="0" src="/Manage/images/layout_content.png" alt="View Page History" title="View Page History" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead1" runat="server" CommandName="titlebar">Titlebar</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("TitleBar")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead2" runat="server" CommandName="pagehead">Page Header</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("PageHead")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead5" runat="server" CommandName="filename">Filename</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("FileName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead6" runat="server" CommandName="NavMenuText">Nav Menu Text</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("NavMenuText")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead3" runat="server" CommandName="EditDate">Last Edited</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("EditDate")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead7" runat="server" CommandName="CreateDate">Created On</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<%# String.Format("{0:d}", Eval("CreateDate"))%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						<asp:LinkButton ID="lnkHead4" runat="server" CommandName="PageActive">Active</asp:LinkButton>
					</HeaderTemplate>
					<ItemTemplate>
						<asp:Image ID="imgActive" runat="server" ImageUrl="/Manage/images/application_lightning.png" AlternateText="Active" />
						<asp:HiddenField ID="hdnIsActive" Visible="false" runat="server" Value='<%#Eval("PageActive") %>' />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</carrot:CarrotGridView>
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/Manage/images/cancel.png" />
		<asp:HiddenField runat="server" ID="hdnActive" Visible="false" Value="/Manage/images/accept.png" />
	</div>
</asp:Content>
