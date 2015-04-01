<%@ Page Title="Widget List" Language="C#" MasterPageFile="MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="WidgetList.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.WidgetList" %>

<%@ MasterType VirtualPath="MasterPages/MainPopup.Master" %>
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
	Widget List
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="SortableGrid">
		<carrot:CarrotGridView CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead" AlternatingRowStyle-CssClass="rowalt"
			RowStyle-CssClass="rowregular">
			<EmptyDataTemplate>
				<p>
					<b>No widgets found.</b>
				</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField>
					<HeaderTemplate>
						Control Path
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("ControlPath")%><br />
						<a class="dataPopupTrigger" rel="<%# Eval("Root_WidgetID") %>" href="javascript:void(0)">
							<img src="/c3-admin/images/doc.png" alt="text" title="text" /></a>
						<%# GetCtrlName(Eval("ControlPath").ToString() )%>
						<asp:HiddenField ID="hdnActive" runat="server" Value='<%# Eval("IsWidgetActive")%>' Visible="false" />
						<asp:HiddenField ID="hdnDelete" runat="server" Value='<%# Eval("IsWidgetPendingDelete")%>' Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsWidgetActive" HeaderText="Active" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
					ShowBooleanImage="true" />
				<asp:TemplateField>
					<HeaderTemplate>
						Active
					</HeaderTemplate>
					<ItemTemplate>
						<asp:Button CommandName='<%#String.Format("restore_{0}", Eval("Root_WidgetID")) %>' ID="btnRestore" runat="server" Text="Show" OnCommand="ClickAction"
							Visible="false" />
						<asp:Button CommandName='<%#String.Format("remove_{0}", Eval("Root_WidgetID")) %>' ID="btnRemove" runat="server" Text="Hide" OnCommand="ClickAction" Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						Pending Delete
					</HeaderTemplate>
					<ItemTemplate>
						<asp:Button CommandName='<%#String.Format("cancel_{0}", Eval("Root_WidgetID")) %>' ID="btnCancel" runat="server" Text="Cancel Deletion" OnCommand="ClickAction"
							Visible="false" />
						<asp:Button CommandName='<%#String.Format("delete_{0}", Eval("Root_WidgetID")) %>' ID="btnDelete" runat="server" Text="Flag For Deletion" OnCommand="ClickAction"
							Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="Edit Date" DataField="EditDate" />
				<asp:BoundField HeaderText="PlaceholderName" DataField="PlaceholderName" />
				<carrot:CarrotHeaderSortTemplateField HeaderText="Go Live" DataField="GoLiveDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField HeaderText="Retire On" DataField="RetireDate" DataFieldFormat="{0:d}" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsRetired" HeaderText="Retired" ShowBooleanImage="true" AlternateTextTrue="Retired"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
				<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsUnReleased" HeaderText="Released" ShowBooleanImage="true" AlternateTextTrue="Unreleased"
					AlternateTextFalse="Active" ImagePathTrue="/c3-admin/images/clock_red.png" ImagePathFalse="/c3-admin/images/clock.png" />
			</Columns>
		</carrot:CarrotGridView>
	</div>
	<p>
		&nbsp;
	</p>
	<p>
		&nbsp;
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
