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
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Bulk Change Status
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<table>
		<tr>
			<td valign="top">
				<table>
					<tr>
						<td valign="top" class="tablecaption">
							show publicly:
						</td>
						<td valign="top">
							<asp:DropDownList ID="ddlActive" runat="server">
								<asp:ListItem Text="[choose]" Value="-1" />
								<asp:ListItem Text="Yes" Value="1" />
								<asp:ListItem Text="No" Value="0" Selected="True" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td valign="top" class="tablecaption">
							include in site navigation:
						</td>
						<td valign="top">
							<asp:DropDownList ID="ddlNavigation" runat="server">
								<asp:ListItem Text="[choose]" Value="-1" />
								<asp:ListItem Text="Yes" Value="1" />
								<asp:ListItem Text="No" Value="0" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td valign="top" class="tablecaption">
							include in sitemap:
						</td>
						<td valign="top">
							<asp:DropDownList ID="ddlSiteMap" runat="server">
								<asp:ListItem Text="[choose]" Value="-1" />
								<asp:ListItem Text="Yes" Value="1" />
								<asp:ListItem Text="No" Value="0" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td valign="top" class="tablecaption">
							hide from search engines:
						</td>
						<td valign="top">
							<asp:DropDownList ID="ddlHide" runat="server">
								<asp:ListItem Text="[choose]" Value="-1" />
								<asp:ListItem Text="Yes" Value="1" />
								<asp:ListItem Text="No" Value="0" />
							</asp:DropDownList>
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td valign="top" class="tablecaption">
							content type:
						</td>
						<td valign="top" colspan="3">
							<div class="jqradioset">
								<asp:RadioButton ID="rdoAll" GroupName="rdoFilterType" runat="server" Text="All" Checked="true" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
								<asp:RadioButton ID="rdoPage" GroupName="rdoFilterType" runat="server" Text="Pages" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
								<asp:RadioButton ID="rdoPost" GroupName="rdoFilterType" runat="server" Text="Posts" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
							</div>
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td valign="top" class="tablecaption">
							use date range:
						</td>
						<td valign="top" colspan="3">
							<div class="jqradioset">
								<asp:RadioButton ID="rdoFilterResults1" GroupName="rdoFilterResults" runat="server" Text="Yes" Checked="true" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
								<asp:RadioButton ID="rdoFilterResults2" GroupName="rdoFilterResults" runat="server" Text="No" AutoPostBack="True" OnCheckedChanged="rdoFilterResults_CheckedChanged" />
							</div>
						</td>
					</tr>
				</table>
				<table>
					<tr runat="server" id="trFilter">
						<td valign="top" class="tablecaption">
							go live date filter:
							<br />
						</td>
						<td valign="top">
							&nbsp;&nbsp;
						</td>
						<td valign="top">
							<asp:TextBox CssClass="dateRegion" ID="txtDate" Columns="12" runat="server" />
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
			</td>
			<td valign="top">
				<asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" /><br />
			</td>
		</tr>
	</table>
	<table>
		<tr>
			<td valign="top" class="tablecaption">
				mark selected items as:
			</td>
			<td valign="top">
				<asp:DropDownList ID="ddlAction" runat="server">
					<asp:ListItem Text="-Select Value-" Value="none" />
					<asp:ListItem Text="show publicly" Value="active" />
					<asp:ListItem Text="include in site navigation" Value="navigation" />
					<asp:ListItem Text="include in sitemap" Value="sitemap" />
					<asp:ListItem Text="allow search engine indexing" Value="searchengine" />
				</asp:DropDownList>
			</td>
		</tr>
	</table>
	<p>
		<asp:Button ID="btnSaveMapping" runat="server" Text="Save" OnClick="btnSaveMapping_Click" />
	</p>
	<p>
		<input type="button" value="Check All" onclick="CheckTheBoxes()" />&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" value="Uncheck All" onclick="UncheckTheBoxes()" />
	</p>
	<p>
		<asp:Label ID="lblPages" runat="server" Text="Label" />
		total records<br />
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
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/application_lightning.png" ImagePathFalse="/c3-admin/images/flag_blue.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ShowInSiteMap" HeaderText="In SiteMap" ShowBooleanImage="true" AlternateTextTrue="Yes"
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/application_lightning.png" ImagePathFalse="/c3-admin/images/flag_blue.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="BlockIndex" HeaderText="Block Index" ShowBooleanImage="true" AlternateTextTrue="Yes"
					AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/flag_blue.png" ImagePathFalse="/c3-admin/images/application_lightning.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Public" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
					ShowBooleanImage="true" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
