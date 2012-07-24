<%@ Page Title="Page Child Sort" Language="C#" MasterPageFile="~/Manage/MasterPages/MainPopup.Master" AutoEventWireup="true"
	CodeBehind="PageChildSort.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.Manage.PageChildSort" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			$("#sortable").sortable({
				placeholder: "ui-state-highlight",
				update: function (event, ui) {
					setTimeout("BuildOrder();", 500);
				}
			});
			$("#sortable").disableSelection();
		});

		function BuildOrder() {
			var map = "#<%=txtSort.ClientID %>";
			$(map).val('');

			$("#sortable li").each(function (i) {
				var itm = $(this);
				var id = itm.attr('id');

				var v = i + '\t' + id;
				$(map).val($(map).val() + '\r\n ' + v);
			});
		}

		$(document).ready(function () {
			setTimeout("BuildOrder();", 200);
		});


	</script>
	<style type="text/css">
		#sortable {
			list-style-type: none;
			margin: 0;
			padding: 0;
			width: 650px;
		}
		#sortable li {
			margin: 5px 5px 5px 5px;
			padding: 5px;
			height: 1.5em;
		}
		html > body #sortable li {
			line-height: 1.2em;
		}
		.ui-state-highlight {
			line-height: 1.2em;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Sort Child / Sub Pages
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Repeater ID="rpPages" runat="server">
		<HeaderTemplate>
			<ul id="sortable">
		</HeaderTemplate>
		<ItemTemplate>
			<li class="ui-state-default" id='<%# Eval("Root_ContentID")%>'>
				<%#MakeStar(Convert.ToBoolean(Eval("PageActive")))%>
				Head:
				<%# Eval("PageHead")%>
				&nbsp;&nbsp;&nbsp; Nav:
				<%# Eval("NavMenuText")%>
				&nbsp;&nbsp;&nbsp; File:
				<%# Eval("FileName")%></li></ItemTemplate>
		<FooterTemplate>
			</ul></FooterTemplate>
	</asp:Repeater>
	<br style="clear: both;" />
	<div style="display: none;">
		<asp:TextBox ID="txtSort" runat="server" TextMode="MultiLine" Rows="5" Columns="80"></asp:TextBox>
	</div>
	<p>
		<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxBodyContentPlaceHolder" runat="server">
</asp:Content>
