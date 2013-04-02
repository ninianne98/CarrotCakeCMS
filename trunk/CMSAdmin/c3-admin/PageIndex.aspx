<%@ Page Title="Page Index" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageIndex.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageIndex" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<%@ Register Src="ucPageMenuItems.ascx" TagName="ucPageMenuItems" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		setTimeout("cmsSendTrackbackBatch();", 1500);
	</script>
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
			<td class="tablecaption">
				show content:
			</td>
			<td colspan="3">
				<div class="jqradioset">
					<asp:RadioButton ID="rdoFilterResults1" GroupName="rdoFilterResults" runat="server" Text="Show Filtered" Checked="true" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
					<asp:RadioButton ID="rdoFilterResults2" GroupName="rdoFilterResults" runat="server" Text="Show All" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
				</div>
			</td>
		</tr>
	</table>
	<table>
		<tr runat="server" id="trFilter">
			<td class="tablecaption">
				page filter:
				<br />
			</td>
			<td>
				<div style="clear: both; height: 2px;">
				</div>
				<asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
			</td>
			<td>
				&nbsp;&nbsp;
			</td>
			<td>
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
	<p>
		<asp:Label ID="lblPages" runat="server" Text="Label" />
		total records<br />
		<asp:Panel ID="pnlPager" runat="server">
			<asp:Button ID="btnChangePage" runat="server" Text="Change Page Size" OnClick="btnChangePage_Click" />
			<asp:DropDownList ID="ddlSize" runat="server">
				<asp:ListItem>10</asp:ListItem>
				<asp:ListItem>25</asp:ListItem>
				<asp:ListItem>50</asp:ListItem>
				<asp:ListItem>100</asp:ListItem>
			</asp:DropDownList>
		</asp:Panel>
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridPaged runat="server" ID="pagedDataGrid" PageSize="25">
			<TheGrid ID="TheGrid1" runat="server" DefaultSort="NavMenuText desc" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead" AlternatingRowStyle-CssClass="rowalt"
				RowStyle-CssClass="rowregular" CssClass="datatable">
				<EmptyDataTemplate>
					<p>
						<b>No content pages found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:HyperLink runat="server" ID="lnkEdit1" NavigateUrl='<%# String.Format("{0}?id={1}", SiteFilename.PageAddEditURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:HyperLink runat="server" ID="lnkEdit2" NavigateUrl='<%# String.Format("{0}?mode=raw&id={1}", SiteFilename.PageAddEditURL, Eval("Root_ContentID")) %>'><img class="imgNoBorder" src="/c3-admin/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></asp:HyperLink>
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
							<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('<%#SiteFilename.PageHistoryURL %>?id=<%#Eval("Root_ContentID") %>');">
								<img class="imgNoBorder" src="/c3-admin/images/hourglass.png" alt="View Page History" title="View Page History" /></a>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/c3-admin/PageWidgets.aspx?id=<%#Eval("Root_ContentID") %>');">
								<img class="imgNoBorder" src="/c3-admin/images/shape_ungroup.png" alt="Page Widgets" title="Page Widgets" /></a>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/c3-admin/PageChildSort.aspx?pageid=<%#Eval("Root_ContentID") %>');">
								<img class="imgNoBorder" src="/c3-admin/images/chart_organisation.png" alt="Sort Sub Pages" title="Sort Sub Pages" /></a>
						</ItemTemplate>
					</asp:TemplateField>
					<carrot:CarrotHeaderSortTemplateField SortExpression="NavMenuText" HeaderText="Nav Menu Text" DataField="NavMenuText" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="Filename" HeaderText="Filename" DataField="Filename" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="EditDate" HeaderText="Last Edited" DataField="EditDate" DataFieldFormat="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Created On" DataField="CreateDate" DataFieldFormat="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="GoLiveDate" HeaderText="Go Live" DataField="GoLiveDate" DataFieldFormat="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsRetired" HeaderText="Retired" ShowBooleanImage="true" AlternateTextTrue="Retired"
						AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsUnReleased" HeaderText="Released" ShowBooleanImage="true" AlternateTextTrue="Unreleased"
						AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="ShowInSiteNav" HeaderText="Navigation" ShowBooleanImage="true"
						AlternateTextTrue="Yes" AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="ShowInSiteMap" HeaderText="In SiteMap" ShowBooleanImage="true"
						AlternateTextTrue="Yes" AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="BlockIndex" HeaderText="Block Index" ShowBooleanImage="true" AlternateTextTrue="Yes"
						AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/zoom_out.png" ImagePathFalse="/c3-admin/images/magnifier.png" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="PageActive" HeaderText="Public" AlternateTextFalse="Inactive"
						AlternateTextTrue="Active" ShowBooleanImage="true" />
				</Columns>
			</TheGrid>
		</carrot:CarrotGridPaged>
	</div>
</asp:Content>
