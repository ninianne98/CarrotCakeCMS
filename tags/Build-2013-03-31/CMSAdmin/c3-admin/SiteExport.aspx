<%@ Page Title="Site Export" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteExport.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteExportPage" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<%@ Register Src="ucSitePageDrillDown.ascx" TagName="ucSitePageDrillDown" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">

		function ExportContent() {
			var dateBegin = $('#<%= txtBegin.ClientID %>').val();
			var dateEnd = $('#<%= txtEnd.ClientID %>').val();

			var exportAll = $('#<%= rdoAll.ClientID %>').prop('checked');
			var exportRange = $('#<%= rdoRange.ClientID %>').prop('checked');
			var exportScope = $('#<%= rdoScope.ClientID %>').prop('checked');

			var exportBlog = $('#<%= chkBlog.ClientID %>').prop('checked');
			var exportPage = $('#<%= chkPage.ClientID %>').prop('checked');
			var exportComment = $('#<%= chkComment.ClientID %>').prop('checked');

			var selectedNode = getSelectedNodeValue();

			//alert("dateBegin : " + dateBegin + " exportBlog : " + exportBlog + " exportPage : " + exportPage + " exportAll : " + exportAll);

			var exportQS = "export=site";

			if (exportRange) {
				exportQS = exportQS + "&datebegin=" + encodeURIComponent(dateBegin) + "&dateend=" + encodeURIComponent(dateEnd);
			}

			if (exportScope) {
				exportQS = exportQS + "&node=" + selectedNode;
			}

			if (exportComment) {
				exportQS = exportQS + "&comment=include";
			}

			if (exportBlog && exportPage) {
				exportQS = exportQS + "&exportwhat=AllData";
			} else {
				if (exportPage) {
					exportQS = exportQS + "&exportwhat=ContentData";
				}
				if (exportBlog) {
					exportQS = exportQS + "&exportwhat=BlogData";
				}
			}

			if (exportBlog || exportPage) {
				var exportURL = '<%=SiteFilename.DataExportURL %>?' + exportQS;
				//alert(exportURL);
				window.open(exportURL);
			} else {
				cmsAlertModal("You must select at least one export data type.");
			}

		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Export
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<fieldset style="width: 650px;">
		<legend>
			<label>
				Export Date Range
			</label>
		</legend>
		<p>
			Will not restrct selection by any critera other than page/post content type.<br />
			<br />
			<asp:RadioButton ID="rdoAll" runat="server" GroupName="rdoExportGroup" Checked="true" value='all' />
			<b>All date ranges</b>
			<br />
		</p>
		<p>
			<br />
			Restrict pages/posts to those with a go live data that falls in the selectedrange
			<br />
			<br />
			<asp:RadioButton ID="rdoRange" runat="server" GroupName="rdoExportGroup" value='range' />
			<b>Selected date range</b>&nbsp;&nbsp;&nbsp; From :
			<asp:TextBox ID="txtBegin" runat="server" CssClass="dateRegion" Columns="12" />
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Through :
			<asp:TextBox ID="txtEnd" runat="server" CssClass="dateRegion" Columns="12" />
			<br />
		</p>
		<p>
			<br />
			Only export pages that fall under the hierarchy of the selected page. Does not affect blog post selection.
		</p>
		<div style="clear: both">
			<div style="float: left; margin-top: 8px;">
				<asp:RadioButton ID="rdoScope" runat="server" GroupName="rdoExportGroup" value='scope' />
			</div>
			<div style="float: left;">
				<!-- parent page plugin-->
				<uc1:ucSitePageDrillDown ID="ParentPagePicker" runat="server" />
			</div>
		</div>
	</fieldset>
	<fieldset style="width: 650px;">
		<legend>
			<label>
				Export Data Types
			</label>
		</legend>
		<p>
			<asp:CheckBox ID="chkBlog" runat="server" Checked="true" />
			Blog Posts
			<br />
			<asp:CheckBox ID="chkPage" runat="server" Checked="true" />
			Content Pages
			<br />
			<asp:CheckBox ID="chkComment" runat="server" />
			Comments for Selected Pages / Posts
			<br />
		</p>
	</fieldset>
	<input type="button" runat="server" id="btnExport" value="Export Site" onclick="ExportContent();" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
