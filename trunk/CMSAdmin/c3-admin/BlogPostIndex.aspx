<%@ Page Title="Post Index" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BlogPostIndex.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.BlogPostIndex" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucBlogMenuItems.ascx" TagName="ucBlogMenuItems" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		setTimeout("cmsSendTrackbackBatch();", 1500);
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Post Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<uc1:ucBlogMenuItems ID="ucBlogMenuItems1" runat="server" />
	<table>
		<tr>
			<td valign="top" class="tablecaption">
				show content:
			</td>
			<td valign="top" colspan="3">
				<asp:RadioButton ID="rdoFilterResults1" GroupName="rdoFilterResults" runat="server" Text="Show Filtered" Checked="true" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
				<asp:RadioButton ID="rdoFilterResults2" GroupName="rdoFilterResults" runat="server" Text="Show All" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
			</td>
		</tr>
		<tr runat="server" id="trFilter">
			<td valign="top" class="tablecaption">
				post filter:
				<br />
			</td>
			<td valign="top">
				<asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
			</td>
			<td valign="top">
				&nbsp;&nbsp;
			</td>
			<td valign="top">
				<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server"></asp:TextBox>
				<asp:DropDownList ID="ddlDateRange" runat="server">
					<asp:ListItem Text="30 Days +/-" Value="30" />
					<asp:ListItem Text="60 Days +/-" Value="60" />
					<asp:ListItem Text="90 Days +/-" Value="90" />
					<asp:ListItem Text="120 Days +/-" Value="120" />
				</asp:DropDownList>
				<div style="clear: both; height: 2px;">
				</div>
			</td>
		</tr>
	</table>
	<p>
		<br />
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="CreateDate desc" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit1" NavigateUrl='<%# String.Format("{0}?id={1}", SiteFilename.BlogPostAddEditURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit2" NavigateUrl='<%# String.Format("{0}?mode=raw&id={1}", SiteFilename.BlogPostAddEditURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/c3-admin/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit3" Target="_blank" NavigateUrl='<%# String.Format("{0}?id={1}", SiteFilename.DataExportURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/c3-admin/images/html_go.png" alt="Export latest version of this page" title="Export latest version of this page" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit4" NavigateUrl='<%# String.Format("{0}", Eval("FileName")) %>'><img class="imgNoBorder" src="/c3-admin/images/html.png" alt="Visit Page" title="Visit Page" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit5" NavigateUrl='<%# String.Format("{0}?carrotedit=true", Eval("FileName")) %>'><img class="imgNoBorder" src="/c3-admin/images/overlays.png" alt="Advanced Editor" title="Advanced Editor" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/c3-admin/CommentIndexPop.aspx?id=<%#Eval("Root_ContentID") %>');">
							<img class="imgNoBorder" src="/c3-admin/images/comments.png" alt="View Comments" title="View Comments" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/c3-admin/PageHistory.aspx?id=<%#Eval("Root_ContentID") %>');">
							<img class="imgNoBorder" src="/c3-admin/images/layout_content.png" alt="View Page History" title="View Page History" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="NavMenuText" HeaderText="Nav Menu Text" DataField="NavMenuText" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="Filename" HeaderText="Filename" DataField="Filename" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="EditDate" HeaderText="Last Edited" DataField="EditDate" DataFieldFormat="{0:MM/dd/yy h:mm tt}" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Created On" DataField="CreateDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsRetired" HeaderText="Retired" ShowBooleanImage="true" AlternateTextTrue="Retired"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/flag_yellow.png" ImagePathFalse="/c3-admin/images/page_world.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsUnReleased" HeaderText="Released" ShowBooleanImage="true" AlternateTextTrue="Unreleased"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/flag_yellow.png" ImagePathFalse="/c3-admin/images/page_world.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="PageActive" HeaderText="Published" AlternateTextFalse="Inactive"
					AlternateTextTrue="Active" ShowBooleanImage="true" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
