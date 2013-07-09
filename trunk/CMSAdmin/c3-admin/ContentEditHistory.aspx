<%@ Page Title="Content Edit History" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ContentEditHistory.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ContentEditHistory" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Content Edit History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:TextBox Style="display: none;" ID="lblDate" runat="server" ReadOnly="true" Columns="40" />
		<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" />
	</p>
	<p>
		<asp:CheckBox ID="chkLatest" runat="server" Checked="true" />
		Show Latest Only
	</p>
	<p>
		<asp:DropDownList ID="ddlUsers" runat="server" DataTextField="Username" DataValueField="UserId" />
	</p>
	<p>
		<asp:Button ID="btnFilter" runat="server" Text="Apply" OnClick="btnFilter_Click" />
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
	<div class="SortableGrid">
		<carrot:CarrotGridPaged runat="server" ID="pagedDataGrid" PageSize="25">
			<TheGrid ID="TheGrid1" runat="server" DefaultSort="EditDate desc" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead" AlternatingRowStyle-CssClass="rowalt"
				RowStyle-CssClass="rowregular" CssClass="datatable">
				<EmptyDataTemplate>
					<p>
						<b>No history found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit4" NavigateUrl='<%# String.Format("{0}", Eval("FileName")) %>'><img class="imgNoBorder" src="/c3-admin/images/html.png" alt="Visit Page" title="Visit Page" /></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('<%#SiteFilename.PageHistoryURL %>?id=<%#Eval("Root_ContentID") %>');">
								<img class="imgNoBorder" src="/c3-admin/images/hourglass.png" alt="View Page History" title="View Page History" /></a>
						</ItemTemplate>
					</asp:TemplateField>
					<carrot:CarrotHeaderSortTemplateField SortExpression="NavMenuText" HeaderText="Nav Menu Text" DataField="NavMenuText" />
					<%--<carrot:CarrotHeaderSortTemplateField SortExpression="Filename" HeaderText="Filename" DataField="Filename" />--%>
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ContentTypeValue" HeaderText="Content Type" ShowEnumImage="true">
						<ImageSelectors>
							<carrot:CarrotImageColumnData ImageAltText="Post" ImagePath="/c3-admin/images/blogger.png" KeyValue="BlogEntry" />
							<carrot:CarrotImageColumnData ImageAltText="Page" ImagePath="/c3-admin/images/page_world.png" KeyValue="ContentEntry" />
						</ImageSelectors>
					</carrot:CarrotHeaderSortTemplateField>
					<carrot:CarrotHeaderSortTemplateField SortExpression="CreateUserName" HeaderText="Create User Name" DataField="CreateUserName" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Created On" DataField="CreateDate" DataFieldFormat="{0:d} {0:h:mm tt}" ItemStyle-Wrap="false" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="EditUserName" HeaderText="Edit User Name" DataField="EditUserName" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="EditDate" HeaderText="Last Edited" DataField="EditDate" DataFieldFormat="{0:d} {0:h:mm tt}" ItemStyle-Wrap="false" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="GoLiveDate" HeaderText="Go Live" DataField="GoLiveDate" DataFieldFormat="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField SortExpression="RetireDate" HeaderText="Retire On" DataField="RetireDate" DataFieldFormat="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsLatestVersion" HeaderText="Is Latest Version" AlternateTextFalse="No"
						AlternateTextTrue="Yes" ShowBooleanImage="true" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="PageActive" HeaderText="Public" AlternateTextFalse="Inactive"
						AlternateTextTrue="Active" ShowBooleanImage="true" />
				</Columns>
			</TheGrid>
		</carrot:CarrotGridPaged>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
