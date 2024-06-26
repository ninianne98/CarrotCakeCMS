﻿<%@ Page Title="Content Snippet History" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="ContentSnippetHistory.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.ContentSnippetHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();

		function cmsGetWidgetText(val) {

			var webMthd = webSvc + "/GetSnippetVersionText";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: JSON.stringify({ DBKey: val }),
				contentType: "application/json; charset=utf-8",
				dataType: "json"
			}).done(cmsReqContentCallback)
				.fail(cmsAjaxFailed);
		}

		function cmsDoToolTipDataRequest(val) {
			cmsGetWidgetText(val);
		}

		function cmsReqContentCallback(data, status) {
			if (data.d == "FAIL") {
				cmsSetHTMLMessage('<i>An error occurred. Please try again.</i>');
			} else {
				cmsSetTextMessage(data.d);
			}
		}
	</script>
	<link href="/c3-admin/Includes/tooltipster.css" rel="stylesheet" type="text/css" />
	<script src="/c3-admin/Includes/jquery.tooltipster.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Content Snippet History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<b>slug:</b>
		<asp:Literal ID="litSlug" runat="server" />
		<asp:Image ID="imgStatus" runat="server" ImageUrl="images/accept.png" AlternateText="Active" />
		<br />
		<b>name:</b>
		<asp:Literal ID="litSnippetName" runat="server" /><br />
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="images/cancel.png" />
	</p>
	<asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove Selected" /><br />
	<div class="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="EditDate DESC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No history found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:CheckBox runat="server" ID="chkContent" value='<%#  String.Format("{0}", Eval("ContentSnippetID")) %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
					</HeaderTemplate>
					<ItemTemplate>
						<a class="dataPopupTrigger" rel="<%# Eval("ContentSnippetID") %>" href="javascript:void(0)">
							<img src="/c3-admin/images/doc.png" alt="text" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0}" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>