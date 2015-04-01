<%@ Page Title="Widget History" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="WidgetHistory.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.WidgetHistory" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<link href="/c3-admin/Includes/tooltiphelper.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();
		var thisPageID = '<%=guidContentID.ToString() %>';

		function cmsGetWidgetText(val) {

			var webMthd = webSvc + "/GetWidgetVersionText";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: JSON.stringify({ DBKey: val, ThisPage: thisPageID }),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: cmsReqContentCallback,
				error: cmsAjaxFailed
			});
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
	<script src="/c3-admin/Includes/tooltiphelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Widget History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<asp:Literal ID="litControlPath" runat="server" /><br />
		<asp:Literal ID="litControlPathName" runat="server" /><br />
	</p>
	<asp:PlaceHolder ID="phNavIndex" runat="server">
		<p>
			<asp:HyperLink ID="lnkIndex" runat="server" NavigateUrl="./PageWidgets.aspx">
		<img src="/c3-admin/images/back.png" alt="Return" title="Return" />
		Return to widget list</asp:HyperLink>
		</p>
	</asp:PlaceHolder>
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
						<asp:CheckBox runat="server" ID="chkContent" value='<%#  String.Format("{0}", Eval("WidgetDataID")) %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
					</HeaderTemplate>
					<ItemTemplate>
						<a class="dataPopupTrigger" rel="<%# Eval("WidgetDataID") %>" href="javascript:void(0)">
							<img src="/c3-admin/images/doc.png" alt="text" title="text" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0}" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
