<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCommentIndex.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucCommentIndex" %>
<p>
	<br />
</p>
<div class="SortableGrid">
	<carrot:CarrotGridPaged runat="server" ID="pagedDataGrid" PageSize="20">
		<TheGrid ID="TheGrid1" runat="server" CssClass="datatable" DefaultSort="CreateDate desc" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead" AlternatingRowStyle-CssClass="rowalt"
			RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No comments found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# String.Format("./{0}?id={1}", LinkingPage, Eval("ContentCommentID")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit" title="Edit" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField SortExpression="CommenterName" HeaderText="Commenter Name" DataField="CommenterName" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="CommenterIP" HeaderText="IP Addy" DataField="CommenterIP" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Date" DataField="CreateDate" DataFieldFormat="{0:MM/dd/yy h:mm tt}" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="NavMenuText" HeaderText="Page Title" DataField="NavMenuText" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="FileName" HeaderText="Filename" DataField="FileName" />
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit4" NavigateUrl='<%# String.Format("{0}", Eval("FileName")) %>'><img class="imgNoBorder" src="/c3-admin/images/html.png" alt="Visit Page" title="Visit Page" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsSpam" HeaderText="Spam" AlternateTextFalse="Not Spam" AlternateTextTrue="Spam"
					ShowBooleanImage="true" ImagePathTrue="/c3-admin/images/error.png" ImagePathFalse="/c3-admin/images/comment.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsApproved" HeaderText="Active" AlternateTextFalse="Inactive"
					AlternateTextTrue="Active" ShowBooleanImage="true" />
			</Columns>
		</TheGrid>
		<%--
		<ThePager ID="ThePager1" runat="server">
			<HeaderTemplate>
				<br />
				<div class="datafooter ui-helper-clearfix ui-widget-content ui-corner-all ui-corner-all">
			</HeaderTemplate>
			<ItemTemplate>
				<carrot:ListItemWrapperForPager HtmlTagName="div" ID="wrap" runat="server" CSSSelected="sel-pagerlink ui-state-active" CssClassNormal="pagerlink ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
					<carrot:NavLinkForPagerTemplate ID="lnkBtn" runat="server" CSSSelected="selected" />
				</carrot:ListItemWrapperForPager>
			</ItemTemplate>
			<FooterTemplate>
				</div>
				<br />
			</FooterTemplate>
		</ThePager>
		--%>
	</carrot:CarrotGridPaged>
</div>
