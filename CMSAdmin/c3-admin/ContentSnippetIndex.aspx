<%@ Page Title="ContentSnippetIndex" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ContentSnippetIndex.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ContentSnippetIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Content Snippet Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div>
		<table style="width: 625px;">
			<tr>
				<td style="width: 48%;">
					<a href="ContentSnippetAddEdit.aspx?id=">
						<img class="imgNoBorder" src="/c3-admin/images/add.png" alt="Add" title="Add as WYSIWYG" />
						Add Content Snippet (with HTML editor)</a>
				</td>
				<td>
					&nbsp;
				</td>
				<td style="width: 48%;">
					<a href="ContentSnippetAddEdit.aspx?mode=raw&id=">
						<img class="imgNoBorder" src="/c3-admin/images/script_add.png" alt="Add" title="Add as Plain Text" />
						Add Content Snippet (as plain text)</a>
				</td>
			</tr>
		</table>
	</div>
	<p>
		<br />
	</p>
	<div class="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="ContentSnippetName asc" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No snippets found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# String.Format("./ContentSnippetAddEdit.aspx?id={0}", Eval("Root_ContentSnippetID")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit2" NavigateUrl='<%# String.Format("./ContentSnippetAddEdit.aspx?mode=raw&id={0}", Eval("Root_ContentSnippetID")) %>'><img class="imgNoBorder" src="/c3-admin/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('./ContentSnippetHistory.aspx?id=<%#Eval("Root_ContentSnippetID") %>');">
							<img class="imgNoBorder" src="/c3-admin/images/hourglass.png" alt="View Page History" title="View Snippet History" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="ContentSnippetName" HeaderText="Name" DataField="ContentSnippetName" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="ContentSnippetSlug" HeaderText="Slug" DataField="ContentSnippetSlug" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="EditDate" HeaderText="Last Edited" DataField="EditDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="GoLiveDate" HeaderText="Go Live" DataField="GoLiveDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="RetireDate" HeaderText="Retire On" DataField="RetireDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="VersionCount" HeaderText="Versions" DataField="VersionCount" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsRetired" HeaderText="Retired" ShowBooleanImage="true" AlternateTextTrue="Retired"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsUnReleased" HeaderText="Released" ShowBooleanImage="true" AlternateTextTrue="Unreleased"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="ContentSnippetActive" HeaderText="Active" AlternateTextFalse="Not Active"
					AlternateTextTrue="Active" ShowBooleanImage="true" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
