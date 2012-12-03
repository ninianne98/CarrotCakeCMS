<%@ Page Title="Bulk Apply Templates/Skins" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageTemplateUpdate.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.PageTemplateUpdate" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<%@ Register Src="ucPageMenuItems.ascx" TagName="ucPageMenuItems" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Bulk Apply Templates/Skins
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<uc1:ucPageMenuItems ID="ucPageMenuItems1" runat="server" />
	<table>
		<tr>
			<td valign="top" class="tablecaption">
				template:
			</td>
			<td valign="top">
				<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
				</asp:DropDownList>
			</td>
			<td valign="top">
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				show content:
			</td>
			<td valign="top">
				<asp:RadioButton ID="rdoFilterResults1" GroupName="rdoFilterResults" runat="server" Text="Show Filtered" Checked="true" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
				<asp:RadioButton ID="rdoFilterResults2" GroupName="rdoFilterResults" runat="server" Text="Show All" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
			</td>
			<td valign="top">
			</td>
		</tr>
		<tr runat="server" id="trFilter">
			<td valign="top" class="tablecaption">
				filter:
			</td>
			<td valign="top">
				<!-- parent page plugin-->
				<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
				<div style="clear: both; height: 2px;">
				</div>
			</td>
			<td valign="top">
				<asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
			</td>
		</tr>
	</table>
	<p>
		<br />
	</p>
	<div id="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="TemplateFile ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						Remap
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkReMap" runat="server" />
						<asp:HiddenField ID="hdnContentID" runat="server" Value='<%# Eval("Root_ContentID") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Template File
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("TemplateFile")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Titlebar
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("TitleBar")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Page Header
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("PageHead")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Filename
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("FileName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Nav Menu Text
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("NavMenuText")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Last Edited
					</HeaderTemplate>
					<ItemTemplate>
						<%# String.Format("{0:d}", Eval("EditDate"))%>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Active" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
					ShowBooleanImage="true" />
			</Columns>
		</carrot:CarrotGridView>
		<div style="height: 50px; margin-top: 10px; margin-bottom: 10px;">
			<asp:Button ID="btnSaveMapping" runat="server" Text="Save" OnClick="btnSaveMapping_Click" />
		</div>
	</div>
</asp:Content>
