<%@ Page Title="PageWidgets" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageWidgets.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.PageWidgets" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
<%@ Import Namespace="Carrotware.CMS.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<link href="/c3-admin/Includes/tooltiphelper.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();
		var thisPageID = '<%=guidContentID.ToString() %>';

		function cmsGetWidgetText(val) {

			var webMthd = webSvc + "/GetWidgetText";

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
	Page Widgets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		Leave checked the widgets you want to have enabled, and uncheck the ones that do not want to load in the page, click the button to apply changes. You
		can also edit the Go Live and Retire date/time by selecting the edit icon.
	</p>
	<p>
		<asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" /><br />
	</p>
	<div class="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" DefaultSort="WidgetOrder ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
			AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No widgets found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:CheckBox runat="server" ID="chkContent" value='<%# Eval("Root_WidgetID") %>' Checked='<%# Eval("IsWidgetActive") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Status" DataField="IsWidgetActive" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
					ShowBooleanImage="true" />
				<asp:TemplateField>
					<HeaderTemplate>
					</HeaderTemplate>
					<ItemTemplate>
						<a class="dataPopupTrigger" rel="<%# Eval("Root_WidgetID") %>" href="javascript:void(0)">
							<img src="/c3-admin/images/doc.png" alt="text" title="text" /></a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Control Path" DataField="ControlPath" />
				<asp:BoundField HeaderText="PlaceholderName" DataField="PlaceholderName" />
				<asp:BoundField HeaderText="Edit Date" DataField="EditDate" DataFormatString="{0:d} {0:t}" />
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit1" NavigateUrl='<%# String.Format("{0}?widgetid={1}", SiteFilename.WidgetHistoryURL, Eval("Root_WidgetID")) %>'><img class="imgNoBorder" src="/c3-admin/images/hourglass.png" alt="History" title="History" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField HeaderText="Go Live" DataField="GoLiveDate" DataFieldFormat="{0:d} {0:t}" />
				<carrot:CarrotHeaderSortTemplateField HeaderText="Retire On" DataField="RetireDate" DataFieldFormat="{0:d} {0:t}" />
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink runat="server" ID="lnkEdit1" NavigateUrl='<%# String.Format("{0}?widgetid={1}", SiteFilename.WidgetTimeURL, Eval("Root_WidgetID")) %>'><img class="imgNoBorder" src="/c3-admin/images/pencil.png" alt="Edit Time" title="Edit Time" /></asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsRetired" HeaderText="Retired" ShowBooleanImage="true" AlternateTextTrue="Retired"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsUnReleased" HeaderText="Released" ShowBooleanImage="true" AlternateTextTrue="Unreleased"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
