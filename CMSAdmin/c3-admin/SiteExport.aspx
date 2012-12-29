<%@ Page Title="Site Export" Language="C#" MasterPageFile="~/c3-admin/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteExport.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.SiteExportPage" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>
<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">


		function ExportContent() {
			var dateBegin = $('#<%= txtBegin.ClientID %>').val();
			var dateEnd = $('#<%= txtEnd.ClientID %>').val();

			var exportAll = $('#<%= rdoAll.ClientID %>').prop('checked');
			var exportRange = $('#<%= rdoRange.ClientID %>').prop('checked');

			var exportBlog = $('#<%= chkBlog.ClientID %>').prop('checked');
			var exportPage = $('#<%= chkPage.ClientID %>').prop('checked');

			//alert("dateBegin : " + dateBegin + " exportBlog : " + exportBlog + " exportPage : " + exportPage + " exportAll : " + exportAll);

			var exportQS = "export=site";

			if (exportRange) {
				exportQS = exportQS + "&datebegin=" + encodeURIComponent(dateBegin) + "&dateend=" + encodeURIComponent(dateEnd);
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
			<asp:RadioButton ID="rdoAll" runat="server" GroupName="rdoDateRange" Checked="true" value='all' />
			All date ranges
			<br />
			<asp:RadioButton ID="rdoRange" runat="server" GroupName="rdoDateRange" value='range' />
			Selected date range&nbsp;&nbsp;&nbsp; From :
			<asp:TextBox ID="txtBegin" runat="server" CssClass="dateRegion" Columns="12" />
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Through :
			<asp:TextBox ID="txtEnd" runat="server" CssClass="dateRegion" Columns="12" />
			<br />
		</p>
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
		</p>
	</fieldset>
	<input type="button" runat="server" id="btnExport" value="Export Site" onclick="ExportContent();" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
