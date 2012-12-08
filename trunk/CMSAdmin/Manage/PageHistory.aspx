<%@ Page Title="Page History" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageHistory.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.PageHistory" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Page History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<h3>
		<asp:Label ID="lblFilename" runat="server" Text="lblFilename"></asp:Label>
		<asp:Image ID="imgStatus" runat="server" ImageUrl="/Manage/images/accept.png" AlternateText="Active" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Create Date:
		<asp:Label ID="lblCreated" runat="server" Text="1/1/1900"></asp:Label>
	</h3>
	<asp:Panel runat="server" ID="pnlDetail">
		<p>
			<asp:HyperLink ID="lnkReturn" runat="server" NavigateUrl="">
			<img class="imgNoBorder" src="/manage/images/table_go.png" alt="Return" title="Return" /> Return to list</asp:HyperLink>
		</p>
		<p>
			Edited On:
			<asp:Label ID="lblEditDate" runat="server" Text="1/1/1900"></asp:Label>
		</p>
		<div id="jqtabs" style="height: 350px; width: 625px;">
			<ul>
				<li><a href="#pagecontent-tabs-0">Left</a></li>
				<li><a href="#pagecontent-tabs-1">Center</a></li>
				<li><a href="#pagecontent-tabs-3">Right</a></li>
			</ul>
			<div id="pagecontent-tabs-0">
				<div style="clear: both; border: solid 0px #000000; height: 260px; width: 570px; overflow-x: auto; padding: 10px;">
					<asp:Literal ID="litLeft" runat="server"></asp:Literal>
					<div style="clear: both">
					</div>
				</div>
			</div>
			<div id="pagecontent-tabs-1">
				<div style="clear: both; border: solid 0px #000000; height: 260px; width: 570px; overflow-x: auto; padding: 10px;">
					<asp:Literal ID="litCenter" runat="server"></asp:Literal>
					<div style="clear: both">
					</div>
				</div>
			</div>
			<div id="pagecontent-tabs-3">
				<div style="clear: both; border: solid 0px #000000; height: 260px; width: 570px; overflow-x: auto; padding: 10px;">
					<asp:Literal ID="litRight" runat="server"></asp:Literal>
					<div style="clear: both">
					</div>
				</div>
			</div>
		</div>
		<script type="text/javascript">

			$(document).ready(function () {
				setTimeout("$('#jqtabs').tabs('select', 'pagecontent-tabs-1');", 500);
			});
		</script>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlHistory">
		<asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove Selected" /><br />
		<div id="SortableGrid">
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="EditDate DESC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular" OnDataBound="gvPages_DataBound">
				<Columns>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./PageHistory.aspx?version={0}", Eval("ContentID")) %>'><img class="imgNoBorder" src="/Manage/images/layout_content.png" alt="Inspect Version" title="Inspect Version" /></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:CheckBox runat="server" ID="chkContent" value='<%#  String.Format("{0}", Eval("ContentID")) %>' />
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
							<%# Eval("EditDate")%>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</carrot:CarrotGridView>
		</div>
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/Manage/images/cancel.png" />
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
