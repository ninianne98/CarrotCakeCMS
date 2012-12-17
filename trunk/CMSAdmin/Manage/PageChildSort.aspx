<%@ Page Title="Page Child Sort" Language="C#" MasterPageFile="~/Manage/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="PageChildSort.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.Manage.PageChildSort" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">

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

		function WireUpSorting() {
			$("#sortable").sortable({
				placeholder: "ui-state-highlight",
				update: function (event, ui) {
					setTimeout("BuildOrder();", 500);
				}
			});
			$("#sortable").disableSelection();

			setTimeout("BuildOrder();", 200);
		}

		function UpdateAjaxSortChild() {
			if (typeof (Sys) != 'undefined') {
				var prm = Sys.WebForms.PageRequestManager.getInstance();
				prm.add_endRequest(function () {
					WireUpSorting();
				});
			}
		}

		$(document).ready(function () {
			UpdateAjaxSortChild();
			WireUpSorting();
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
	<div>
		<asp:DropDownList ID="ddlAlternateSort" runat="server">
			<asp:ListItem Value="000000">--Sort Preset--</asp:ListItem>
			<asp:ListItem Value="alpha">Alphabetical (asc)</asp:ListItem>
			<asp:ListItem Value="datecreated">Date Created (asc)</asp:ListItem>
			<asp:ListItem Value="dateupdated">Date Last Updated (asc)</asp:ListItem>
			<asp:ListItem Value="alpha2">Alphabetical (desc)</asp:ListItem>
			<asp:ListItem Value="datecreated2">Date Created (desc)</asp:ListItem>
			<asp:ListItem Value="dateupdated2">Date Last Updated (desc)</asp:ListItem>
		</asp:DropDownList>
		<asp:Button ID="btnSort" runat="server" Text="Apply Sort Preset" OnClick="btnSort_Click" />
	</div>
	<br />
	<div>
		<asp:Repeater ID="rpPages" runat="server">
			<HeaderTemplate>
				<ul id="sortable">
			</HeaderTemplate>
			<ItemTemplate>
				<li class="ui-state-default" id='<%# Eval("Root_ContentID")%>'>
					Head:
					<%# Eval("PageHead")%>
					&nbsp;&nbsp;&nbsp; Nav:
					<%# Eval("NavMenuText")%>
					&nbsp;&nbsp;&nbsp; File:
					<%# Eval("FileName")%></li></ItemTemplate>
			<FooterTemplate>
				</ul></FooterTemplate>
		</asp:Repeater>
	</div>
	<br style="clear: both;" />
	<div style="display: none;">
		<asp:TextBox ID="txtSort" runat="server" TextMode="MultiLine" Rows="5" Columns="80"></asp:TextBox>
	</div>
	<p>
		<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
	</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
