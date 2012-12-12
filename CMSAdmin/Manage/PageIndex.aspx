<%@ Page Title="Page Index" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageIndex.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.PageIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
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
		<br />
		<tr runat="server" id="trFilter">
			<td valign="top" class="tablecaption">
				page filter:
				<br />
			</td>
			<td valign="top">
				<div style="clear: both; height: 2px;">
				</div>
				<asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
			</td>
			<td valign="top">
				&nbsp;&nbsp;
			</td>
			<td valign="top">
				<!-- parent page plugin-->
				<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
				<div style="clear: both; height: 2px;">
				</div>
			</td>
		</tr>
	</table>
	<p>
		<br />
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="TitleBar ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit1" NavigateUrl='<%# String.Format("{0}?id={1}", Carrotware.CMS.UI.Admin.SiteFilename.PageAddEditURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/Manage/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit2" NavigateUrl='<%# String.Format("{0}?mode=raw&id={1}", Carrotware.CMS.UI.Admin.SiteFilename.PageAddEditURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/Manage/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit3" Target="_blank" NavigateUrl='<%# String.Format("{0}?id={1}", Carrotware.CMS.UI.Admin.SiteFilename.DataExportURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/Manage/images/html_go.png" alt="Export latest version of this page" title="Export latest version of this page" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit4" NavigateUrl='<%# String.Format("{0}", Eval("FileName")) %>'><img class="imgNoBorder" src="/Manage/images/html.png" alt="Visit Page" title="Visit Page" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit5" NavigateUrl='<%# String.Format("{0}?carrotedit=true", Eval("FileName")) %>'><img class="imgNoBorder" src="/Manage/images/overlays.png" alt="Advanced Editor" title="Advanced Editor" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/Manage/CommentIndexPop.aspx?id=<%#Eval("Root_ContentID") %>');">
							<img class="imgNoBorder" src="/Manage/images/comments.png" alt="View Comments" title="View Comments" /></a>
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
				<carrot:CarrotHeaderSortTemplateField SortExpression="titlebar" HeaderText="Titlebar" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="pagehead" HeaderText="Page Header" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="Filename" HeaderText="Filename" DataField="Filename" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="NavMenuText" HeaderText="Nav Menu Text" DataField="NavMenuText" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="EditDate" HeaderText="Last Edited" DataField="EditDate" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Created On" DataField="CreateDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="PageActive" HeaderText="Active" AlternateTextFalse="Inactive"
					AlternateTextTrue="Active" ShowBooleanImage="true" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
