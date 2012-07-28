<%@ Page Title="Widget List" Language="C#" MasterPageFile="~/Manage/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="WidgetList.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.WidgetList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<link href="/Manage/Includes/tooltiphelper.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var webSvc = "/Manage/CMS.asmx";
		var thisPageID = '<%=guidContentID.ToString() %>';

		function cmsGetWidgetText(val) {

			var webMthd = webSvc + "/GetWidgetText";

			$.ajax({
				type: "POST",
				url: webMthd,
				data: "{'DBKey': '" + val + "', 'ThisPage': '" + thisPageID + "'}",
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


		function cmsAjaxFailed(request) {
			var s = "";
			s = s + "<b>status: </b>" + request.status + '<br />\r\n';
			s = s + "<b>statusText: </b>" + request.statusText + '<br />\r\n';
			s = s + "<b>responseText: </b>" + request.responseText + '<br />\r\n';

			alert(s);
		}

	</script>
	<script src="/Manage/Includes/tooltiphelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Widget List
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div id="SortableGrid">
		<asp:GridView CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead" AlternatingRowStyle-CssClass="rowalt"
			RowStyle-CssClass="rowregular">
			<Columns>
				<asp:TemplateField>
					<HeaderTemplate>
						Control Path
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("ControlPath")%><br />
						<%-- onmouseover="javascript:cmsGetWidgetText('<%# Eval("Root_WidgetID").ToString() %>')" --%>
						<a class="dataPopupTrigger" rel="<%# Eval("Root_WidgetID") %>" href="javascript:void(0)">
							<img src="/manage/images/doc.png" alt="text" title="text" /></a>
						<%# GetCtrlName(Eval("ControlPath").ToString() )%>
						<asp:HiddenField ID="hdnActive" runat="server" Value='<%# Eval("IsWidgetActive")%>' Visible="false" />
						<asp:HiddenField ID="hdnDelete" runat="server" Value='<%# Eval("IsWidgetPendingDelete")%>' Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>
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
				<asp:TemplateField>
					<HeaderTemplate>
						Edit Date
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("EditDate")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<HeaderTemplate>
						PlaceholderName
					</HeaderTemplate>
					<ItemTemplate>
						<%# Eval("PlaceholderName")%>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxBodyContentPlaceHolder" runat="server">
</asp:Content>
