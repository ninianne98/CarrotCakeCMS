<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCommentIndex.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.ucCommentIndex" %>
<style type="text/css">
	.pagerlink, .sel-pagerlink {
		float: left;
		margin: 2px 2px 2px 2px;
		padding: 5px 5px 5px 5px;
	}
	.datafooter {
		margin: 2px 2px 2px 2px;
		padding: 2px 2px 2px 2px;
		clear: both;
		width: 50%;
	}
</style>
<p>
	<br />
</p>
<div id="SortableGrid">
	<carrot:CarrotGridView CssClass="datatable" DefaultSort="CreateDate desc" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
		AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# String.Format("./{0}?id={1}", LinkingPage, Eval("ContentCommentID")) %>'><img class="imgNoBorder" src="/Manage/images/pencil.png" alt="Edit" title="Edit" /></asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<carrot:CarrotHeaderSortTemplateField SortExpression="CommenterName" HeaderText="Commenter Name" DataField="CommenterName" />
			<carrot:CarrotHeaderSortTemplateField SortExpression="CommenterIP" HeaderText="IP Addy" DataField="CommenterIP" />
			<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Date" DataField="CreateDate" />
			<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsSpam" HeaderText="Spam" AlternateTextFalse="Not Spam" AlternateTextTrue="Spam"
				ShowBooleanImage="true" ImagePathTrue="/Manage/images/error.png" ImagePathFalse="/Manage/images/comment.png" />
			<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsApproved" HeaderText="Active" AlternateTextFalse="Inactive"
				AlternateTextTrue="Active" ShowBooleanImage="true" />
		</Columns>
	</carrot:CarrotGridView>
</div>
<br />
<asp:HiddenField ID="hdnPageSize" Value="20" runat="server" />
<asp:HiddenField ID="hdnPageNbr" Value="1" runat="server" />
<asp:Repeater runat="server" ID="rpDataPager">
	<HeaderTemplate>
		<div class="datafooter ui-helper-clearfix ui-widget-content ui-corner-all ui-corner-all">
	</HeaderTemplate>
	<ItemTemplate>
		<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="sel-pagerlink ui-state-active" CssClassNormal="pagerlink ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
			<carrot:NavLinkForPagerTemplate ID="lnkBtn" runat="server" CSSSelected="selected" />
		</carrot:ListItemWrapperForPager>
	</ItemTemplate>
	<FooterTemplate>
		</div></FooterTemplate>
</asp:Repeater>
<br />
