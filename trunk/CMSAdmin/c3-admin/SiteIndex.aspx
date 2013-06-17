<%@ Page Title="SiteIndex" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteIndex.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteIndex" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Manage Site Users
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="SiteName ASC" ID="gvSites" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No sites found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./SiteDetail.aspx?id={0}", Eval("SiteID")) %>'><img class="imgNoBorder" src="/c3-admin/images/application_edit.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="SiteName" HeaderText="Site Name" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="MainURL" HeaderText="Main URL" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="SiteID" HeaderText="Site ID" />
				<asp:TemplateField>
					<ItemTemplate>
						<asp:Image ID="imgStatus" runat="server" ImageUrl="images/house.png" ToolTip="Current Site" Visible='<%#  Eval("SiteID").ToString() == SiteData.CurrentSiteID.ToString() %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="BlockIndex" HeaderText="Block Index" ShowBooleanImage="true" AlternateTextTrue="Yes"
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/zoom_out.png" ImagePathFalse="/c3-admin/images/magnifier.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="SendTrackbacks" HeaderText="Send Trackbacks" AlternateTextFalse="N"
					AlternateTextTrue="Y" ShowBooleanImage="true" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="AcceptTrackbacks" HeaderText="Accept Trackbacks" AlternateTextFalse="N"
					AlternateTextTrue="Y" ShowBooleanImage="true" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
