<%@ Page Title="DuplicateWidgetFrom" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="DuplicateWidgetFrom.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.DuplicateWidgetFrom" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		function ClickRdo(item) {
			$('#<%=hdnSelectedItem.ClientID %>').val(item.value);
			$('#<%=btnLoadWidgets.ClientID %>').click();
		}
	</script>
	<link href="/c3-admin/Includes/tooltiphelper.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var webSvc = cmsGetServiceAddress();
		var thisPageID = '';

		function cmsGetWidgetText(val) {

			thisPageID = $('#<%=hdnSelectedItem.ClientID %>').val();

			var webMthd = webSvc + "/GetWidgetLatestText";

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

	</script>
	<script src="/c3-admin/Includes/tooltiphelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Duplicate Widget From?
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<fieldset style="width: 680px;">
		<legend>
			<label>
				Search
			</label>
		</legend>
		<div style="display: none">
			<asp:HiddenField runat="server" ID="hdnSelectedItem" />
			<asp:Button ID="btnLoadWidgets" runat="server" Text="LoadWidgets" OnClick="btnLoadWidgets_Click" />
		</div>
		<div>
			<b class="caption">search for: </b>
			<asp:TextBox ID="txtSearchTerm" runat="server" MaxLength="100" Columns="75" onkeypress="return ProcessKeyPress(event)" />
			<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
		</div>
		<div>
			<b class="caption">hide inactive results: </b>
			<asp:CheckBox ID="chkActive" runat="server" />
		</div>
		<br />
		<asp:PlaceHolder ID="phResults" runat="server">
			<div>
				<asp:Literal ID="litResults" runat="server" />
			</div>
			<div class="SortableGrid">
				<carrot:CarrotGridView CssClass="datatable" DefaultSort="EditDate DESC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
					AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
					<EmptyDataTemplate>
						<p>
							<b>No matches found.</b>
						</p>
					</EmptyDataTemplate>
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<input type="radio" value="<%# Eval("Root_ContentID") %>" id="rdoContent" name="rdoContent" onclick="ClickRdo(this)" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:Image ID="imgStatus" runat="server" ImageUrl="images/flag_blue.png" ToolTip="Current Page" Visible='<%#  Eval("Root_ContentID").ToString() == guidContentID.ToString() %>' />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField HeaderText="Nav Menu Text" DataField="NavMenuText" />
						<asp:BoundField HeaderText="Filename" DataField="Filename" />
						<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0:d}" />
						<asp:BoundField HeaderText="Created On" DataField="CreateDate" DataFormatString="{0:d}" />
						<asp:BoundField HeaderText="Go Live" DataField="GoLiveDate" DataFormatString="{0:d}" />
						<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ContentType" HeaderText="Content Type" ShowEnumImage="true">
							<ImageSelectors>
								<carrot:CarrotImageColumnData ImageAltText="Post" ImagePath="/c3-admin/images/blogger.png" KeyValue="BlogEntry" />
								<carrot:CarrotImageColumnData ImageAltText="Page" ImagePath="/c3-admin/images/page_world.png" KeyValue="ContentEntry" />
							</ImageSelectors>
						</carrot:CarrotHeaderSortTemplateField>
						<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ShowInSiteNav" HeaderText="Navigation" ShowBooleanImage="true" AlternateTextTrue="Yes"
							AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
						<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="ShowInSiteMap" HeaderText="In SiteMap" ShowBooleanImage="true" AlternateTextTrue="Yes"
							AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/lightbulb.png" ImagePathFalse="/c3-admin/images/lightbulb_off.png" />
						<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="BlockIndex" HeaderText="Block Index" ShowBooleanImage="true" AlternateTextTrue="Yes"
							AlternateTextFalse="No" ImagePathTrue="/c3-admin/images/zoom_out.png" ImagePathFalse="/c3-admin/images/magnifier.png" />
						<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="PageActive" HeaderText="Public" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
							ShowBooleanImage="true" />
					</Columns>
				</carrot:CarrotGridView>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="phWidgets" runat="server">
			<h3>
				<asp:Literal ID="litSrc" runat="server" />
			</h3>
			<div>
				<asp:Button ID="btnDuplicate" runat="server" OnClick="btnDuplicate_Click" Text="Duplicate" />
			</div>
			<div class="SortableGrid">
				<carrot:CarrotGridView CssClass="datatable" DefaultSort="WidgetOrder ASC" ID="gvWidgets" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
					AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
					<EmptyDataTemplate>
						<p>
							<b>No widgets found.</b>
						</p>
					</EmptyDataTemplate>
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:CheckBox runat="server" ID="chkContent" value='<%#  String.Format("{0}", Eval("Root_WidgetID")) %>' />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
							</HeaderTemplate>
							<ItemTemplate>
								<a class="dataPopupTrigger" rel="<%# Eval("Root_WidgetID") %>" href="javascript:void(0)">
									<img src="/c3-admin/images/doc.png" alt="text" title="text" /></a>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField HeaderText="Last Edited" DataField="EditDate" DataFormatString="{0}" />
						<asp:BoundField HeaderText="Placeholder Name" DataField="PlaceholderName" DataFormatString="{0}" />
						<asp:BoundField HeaderText="Control Path" DataField="ControlPath" DataFormatString="{0}" />
						<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsWidgetActive" HeaderText="Active" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
							ShowBooleanImage="true" />
					</Columns>
				</carrot:CarrotGridView>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="phDone" runat="server">
			<div>
				Copied
				<asp:Literal ID="litCount" runat="server" />
				widget(s) into the current page and placeholder.
			</div>
		</asp:PlaceHolder>
	</fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
