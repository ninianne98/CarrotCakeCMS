<%@ Page Title="Page History" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageHistory.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageHistory" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<style type="text/css">
		.scrollingArea {
			clear: both;
			border: solid 0px #000000;
			height: 300px;
			width: 700px;
			overflow-x: auto;
			padding: 5px;
		}
	</style>
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
	Page History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<h3>
		<asp:Literal ID="lblFilename" runat="server" Text="lblFilename" />
		<asp:Image ID="imgStatus" runat="server" ImageUrl="images/accept.png" AlternateText="Active" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Create Date:
		<asp:Literal ID="lblCreated" runat="server" Text="1/1/1900" />
	</h3>
	<asp:Panel runat="server" ID="pnlDetail">
		<p>
			<asp:HyperLink ID="lnkReturn" runat="server" NavigateUrl="">
			<img class="imgNoBorder" src="images/table_go.png" alt="Return" title="Return" /> Return to list</asp:HyperLink>
		</p>
		<p>
			Edited On:
			<asp:Literal ID="lblEditDate" runat="server" Text="1/1/1900" />
		</p>
		<div id="jqtabs" style="height: 380px; width: 750px;">
			<ul>
				<li><a href="#pagecontent-tabs-0">Left</a></li>
				<li><a href="#pagecontent-tabs-1">Center</a></li>
				<li><a href="#pagecontent-tabs-3">Right</a></li>
			</ul>
			<div id="pagecontent-tabs-0">
				<div class="scrollingArea">
					<asp:Literal ID="litLeft" runat="server" />
					<div style="clear: both">
					</div>
				</div>
			</div>
			<div id="pagecontent-tabs-1">
				<div class="scrollingArea">
					<asp:Literal ID="litCenter" runat="server" />
					<div style="clear: both">
					</div>
				</div>
			</div>
			<div id="pagecontent-tabs-3">
				<div class="scrollingArea">
					<asp:Literal ID="litRight" runat="server" />
					<div style="clear: both">
					</div>
				</div>
			</div>
		</div>
		<script type="text/javascript">

			$(document).ready(function () {
				setTimeout("$('#jqtabs').tabs('option', 'active', 1);", 500);
			});
		</script>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlHistory">
		<p>
			<input type="button" value="Check All" onclick="CheckTheBoxes()" />&nbsp;&nbsp;&nbsp;&nbsp;
			<input type="button" value="Uncheck All" onclick="UncheckTheBoxes()" />
		</p>
		<p>
			<asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove Selected" /><br />
		</p>
		<div class="SortableGrid">
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="EditDate DESC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular" OnDataBound="gvPages_DataBound">
				<EmptyDataTemplate>
					<p>
						<b>No content history found.</b>
					</p>
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# String.Format("{0}?versionid={1}", SiteFilename.PageHistoryURL, Eval("ContentID")) %>'><img class="imgNoBorder" src="images/layout_content.png" alt="Inspect Version" title="Inspect Version" /></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:CheckBox runat="server" ID="chkContent" value='<%# Eval("ContentID") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<HeaderTemplate>
							user
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Literal ID="litUser" runat="server" Text='<%# GetUserName( new Guid( Eval("EditUserId").ToString()) ) %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField HeaderText="TitleBar" DataField="TitleBar" />
					<asp:BoundField HeaderText="Page Header" DataField="PageHead" />
					<asp:BoundField HeaderText="Nav Menu Text" DataField="NavMenuText" />
					<asp:BoundField HeaderText="Last Edited" DataField="EditDate" />
				</Columns>
			</carrot:CarrotGridView>
		</div>
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="images/cancel.png" />
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
