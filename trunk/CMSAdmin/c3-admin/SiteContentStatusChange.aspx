<%@ Page Title="Bulk Change Status" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteContentStatusChange.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteContentStatusChange" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
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


		function doDateClick(obj) {
			doDateTable();
		}

		function doDateTable() {
			var rdo1 = $('#<%=rdoFilterResults1.ClientID %>');

			if (rdo1.prop('checked')) {
				doDateShowHide('<%=rdoFilterResults1.ClientID %>');
			} else {
				doDateShowHide('<%=rdoFilterResults2.ClientID %>');
			}
		}

		function doDateShowHide(rdoID) {
			var tbl = $('#dateFilterTable');
			var rdo = $('#' + rdoID);

			if (rdo.val() != '1') {
				tbl.css('display', 'none');
			} else {
				tbl.css('display', 'block');
			}
		}

		function UpdateDateTable() {
			if (typeof (Sys) != 'undefined') {
				var prm = Sys.WebForms.PageRequestManager.getInstance();
				prm.add_endRequest(function () {
					doDateTable();
				});
			}
		}

		$(document).ready(function () {
			UpdateDateTable();
			doDateTable();
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Bulk Change Status
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<fieldset style="width: 500px;">
		<legend>
			<label>
				Filter Content
			</label>
		</legend>
		<div style="clear: both;">
			<table>
				<tr>
					<td class="tablecaption">
						show publicly:
					</td>
					<td>
						<asp:DropDownList ID="ddlActive" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="tablecaption">
						include in site navigation:
					</td>
					<td>
						<asp:DropDownList ID="ddlNavigation" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="tablecaption">
						include in sitemap:
					</td>
					<td>
						<asp:DropDownList ID="ddlSiteMap" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="tablecaption">
						hide from search engines:
					</td>
					<td>
						<asp:DropDownList ID="ddlHide" runat="server" />
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<td class="tablecaption">
						content type:
					</td>
					<td colspan="3">
						<div class="jqradioset">
							<asp:RadioButton ID="rdoAll" GroupName="rdoFilterType" runat="server" Text="All" Checked="true" />
							<asp:RadioButton ID="rdoPage" GroupName="rdoFilterType" runat="server" Text="Pages" />
							<asp:RadioButton ID="rdoPost" GroupName="rdoFilterType" runat="server" Text="Posts" />
							<%--  AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged"  --%>
						</div>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<td class="tablecaption">
						use date range:
					</td>
					<td colspan="3">
						<div class="jqradioset">
							<asp:RadioButton ID="rdoFilterResults1" GroupName="rdoFilterResults" runat="server" Text="Yes" value="1" Checked="true" onclick="doDateClick(this)" />
							<asp:RadioButton ID="rdoFilterResults2" GroupName="rdoFilterResults" runat="server" Text="No" value="0" onclick="doDateClick(this)" />
							<%--  AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged"  --%>
						</div>
					</td>
				</tr>
			</table>
			<table id="dateFilterTable">
				<tr>
					<td class="tablecaption">
						go live date filter:
						<br />
					</td>
					<td>
						&nbsp;&nbsp;
					</td>
					<td>
						<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" />
						<asp:DropDownList ID="ddlDateRange" runat="server">
							<asp:ListItem Text="2 Days +/-" Value="2" />
							<asp:ListItem Text="7 Days +/-" Value="7" />
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
		</div>
		<div style="float: right; clear: both;">
			<asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
		</div>
	</fieldset>
	<fieldset style="width: 500px;">
		<legend>
			<label>
				Update to Selected Status
			</label>
		</legend>
		<div style="clear: both;">
			<table>
				<tr>
					<td class="tablecaption">
						mark selected items as:
					</td>
					<td>
						<asp:DropDownList ID="ddlAction" runat="server" ValidationGroup="updateStatus">
							<asp:ListItem Text="-Select Value-" Value="none" />
							<asp:ListItem Text="show publicly" Value="active" />
							<asp:ListItem Text="do NOT show publicly **" Value="inactive" />
							<asp:ListItem Text="include in site navigation" Value="navigation" />
							<asp:ListItem Text="EXCLUDE from site navigation **" Value="navigation-no" />
							<asp:ListItem Text="include in sitemap" Value="sitemap" />
							<asp:ListItem Text="EXCLUDE from sitemap **" Value="sitemap-no" />
							<asp:ListItem Text="allow search engine indexing" Value="searchengine" />
							<asp:ListItem Text="BLOCK search engine indexing **" Value="searchengine-no" />
						</asp:DropDownList>
						<asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="updateStatus" Text="Invalid" Operator="NotEqual" CssClass="validationError"
							ForeColor="" ControlToValidate="ddlAction" ValueToCompare="none" />
					</td>
				</tr>
			</table>
		</div>
		<div style="float: right; clear: both;">
			<asp:Button ID="btnSaveMapping" runat="server" Text="Save" OnClick="btnSaveMapping_Click" ValidationGroup="updateStatus" />
		</div>
	</fieldset>
	<p>
		<input type="button" value="Check All" onclick="CheckTheBoxes()" />&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" value="Uncheck All" onclick="UncheckTheBoxes()" />
	</p>
	<p>
		<asp:Label ID="lblPages" runat="server" Text="Label" />
		total records<br />
	</p>
	<div class="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="ContentType desc" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No content found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField ItemStyle-HorizontalAlign="Center">
					<HeaderTemplate>
						&nbsp;
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox ID="chkSelect" runat="server" />
						<asp:HiddenField ID="hdnContentID" runat="server" Value='<%# Eval("Root_ContentID") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Nav Menu Text" DataField="NavMenuText" />
				<asp:BoundField HeaderText="Filename" DataField="Filename" />
				<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0:d}" />
				<asp:BoundField HeaderText="Created On" DataField="CreateDate" DataFormatString="{0:d}" />
				<asp:BoundField HeaderText="Go Live" DataField="GoLiveDate" DataFormatString="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ContentType" HeaderText="Content Type" ShowEnumImage="true">
					<ImageSelectors>
						<carrot:CarrotImageColumnData ImageAltText="Post" ImagePath="/c3-admin/images/blogger.png" KeyValue="BlogEntry" />
						<carrot:CarrotImageColumnData ImageAltText="Page" ImagePath="/c3-admin/images/page_world.png" KeyValue="ContentEntry" />
					</ImageSelectors>
				</carrot:CarrotHeaderSortTemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ShowInSiteNav" HeaderText="Navigation" ShowBooleanImage="true" AlternateTextTrue="Yes"
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ShowInSiteMap" HeaderText="In SiteMap" ShowBooleanImage="true" AlternateTextTrue="Yes"
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="BlockIndex" HeaderText="Block Index" ShowBooleanImage="true" AlternateTextTrue="Yes"
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/zoom_out.png" ImagePathFalse="/c3-admin/images/magnifier.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Public" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
					ShowBooleanImage="true" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
