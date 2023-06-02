﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCommentIndex.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucCommentIndex" %>
<p>
	<br />
</p>
<table>
	<tr>
		<td class="tablecaption">public:
		</td>
		<td>
			<asp:DropDownList ID="ddlActive" runat="server" />
		</td>
		<td class="tablecaption">spam:
		</td>
		<td>
			<asp:DropDownList ID="ddlSpam" runat="server" />
		</td>
		<td class="tablecaption">page size:
		</td>
		<td>
			<asp:DropDownList ID="ddlSize" runat="server">
				<asp:ListItem>10</asp:ListItem>
				<asp:ListItem>25</asp:ListItem>
				<asp:ListItem>50</asp:ListItem>
				<asp:ListItem>100</asp:ListItem>
			</asp:DropDownList>
		</td>
		<td>
			<asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" />
		</td>
	</tr>
</table>
<p>
	<asp:Label ID="lblPages" runat="server" Text="Label" />
	total records<br />
</p>
<div class="SortableGrid">
	<carrot:CarrotGridPaged runat="server" ID="pagedDataGrid" PageSize="25">
		<TheGrid ID="TheGrid1" runat="server" CssClass="datatable" DefaultSort="CreateDate desc" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
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
				<carrot:CarrotHeaderSortTemplateField SortExpression="CreateDate" HeaderText="Date" DataField="CreateDate" DataFieldFormat="{0:d} {0:t}" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="NavMenuText" HeaderText="Page Title" DataField="NavMenuText" />
				<carrot:CarrotHeaderSortTemplateField SortExpression="FileName" HeaderText="Filename" DataField="FileName" />
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit4" NavigateUrl='<%# String.Format("{0}", Eval("FileName")) %>'><img class="imgNoBorder" src="/c3-admin/images/html.png" alt="Visit Page" title="Visit Page" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsSpam" HeaderText="Spam" AlternateTextFalse="Not Spam"
					AlternateTextTrue="Spam" ShowBooleanImage="true" ImagePathTrue="/c3-admin/images/error.png" ImagePathFalse="/c3-admin/images/comment.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" SortExpression="IsApproved" HeaderText="Approved" AlternateTextFalse="Inactive"
					AlternateTextTrue="Approved" ShowBooleanImage="true" />
			</Columns>
		</TheGrid>
	</carrot:CarrotGridPaged>
</div>