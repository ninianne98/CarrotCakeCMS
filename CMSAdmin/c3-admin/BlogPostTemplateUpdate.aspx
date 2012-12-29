<%@ Page Title="Blog Bulk Apply Templates/Skins" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BlogPostTemplateUpdate.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.BlogPostTemplateUpdate" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucBlogMenuItems.ascx" TagName="ucBlogMenuItems" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		function CheckTheBoxes() {
			$('#<%=gvPages.ClientID %> input[type=checkbox]').each(function () {
				$(this).prop('checked', true);
			});
		}

		function UncheckTheBoxes() {
			$('#<%=gvPages.ClientID %> input[type=checkbox]').each(function () {
				$(this).prop('checked', false);
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Bulk Apply Templates/Skins
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<uc1:ucBlogMenuItems ID="ucBlogMenuItems1" runat="server" />
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
	</table>
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
	<p>
		<input type="button" value="Check All" onclick="CheckTheBoxes()" />&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" value="Uncheck All" onclick="UncheckTheBoxes()" />
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
				<asp:BoundField HeaderText="Template File" DataField="TemplateFile" />
				<asp:BoundField HeaderText="Titlebar" DataField="Titlebar" />
				<asp:BoundField HeaderText="Page Header" DataField="PageHead" />
				<asp:BoundField HeaderText="Filename" DataField="Filename" />
				<asp:BoundField HeaderText="Nav Menu Text" DataField="NavMenuText" />
				<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0:d}" />
				<asp:BoundField HeaderText="Created On" DataField="CreateDate" DataFormatString="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Active" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
					ShowBooleanImage="true" />
			</Columns>
		</carrot:CarrotGridView>
		<div style="height: 50px; margin-top: 10px; margin-bottom: 10px;">
			<asp:Button ID="btnSaveMapping" runat="server" Text="Save" OnClick="btnSaveMapping_Click" />
		</div>
	</div>
</asp:Content>
