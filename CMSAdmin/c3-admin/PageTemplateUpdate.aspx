<%@ Page Title="Bulk Apply Templates/Skins - Pages" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageTemplateUpdate.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageTemplateUpdate" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<%@ Register Src="ucPageMenuItems.ascx" TagName="ucPageMenuItems" TagPrefix="uc1" %>
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
	Bulk Apply Templates/Skins - Pages
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
	<br />
	<table>
		<tr>
			<td class="tablecaption">
				template to apply to selected pages:
			</td>
			<td>
				<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
				</asp:DropDownList>
			</td>
		</tr>
	</table>
	<br />
	<div style="height: 50px; margin-top: 10px; margin-bottom: 10px;">
		<asp:Button ID="btnSaveMapping" runat="server" Text="Save" OnClick="btnSaveMapping_Click" />
	</div>
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
						&nbsp;
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkSelect" runat="server" value='<%# Eval("Root_ContentID") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Template File" DataField="TemplateFile" />
				<asp:BoundField HeaderText="Nav Menu Text" DataField="NavMenuText" />
				<asp:BoundField HeaderText="Filename" DataField="Filename" />
				<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0:d}" />
				<asp:BoundField HeaderText="Created On" DataField="CreateDate" DataFormatString="{0:d}" />
				<asp:BoundField HeaderText="Go Live" DataField="GoLiveDate" DataFormatString="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ShowInSiteNav" HeaderText="Navigation" ShowBooleanImage="true" AlternateTextTrue="Yes"
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Active" ShowBooleanImage="true" AlternateTextFalse="Inactive"
					AlternateTextTrue="Active" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
