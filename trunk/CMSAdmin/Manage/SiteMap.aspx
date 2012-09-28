<%@ Page ValidateRequest="false" Title="Site Map" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.SiteMap" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script src="Includes/jquery.ui.nestedSortable.js" type="text/javascript"></script>
	<style type="text/css">
		#cmsSiteMap ol {
			list-style: none;
			padding: 0;
			margin: 0;
			margin-left: 5px;
			margin-top: 2px;
			margin-bottom: 2px;
		}
		#cmsSiteMap a {
			text-decoration: none;
			color: #000000;
			border: 0;
		}
		#cmsSiteMap li {
			list-style: none;
			background: no-repeat url(images/page.png);
			padding: 0;
			margin: 0;
			padding-left: 15px;
			margin-left: 5px;
			margin-bottom: 8px;
		}
		#cmsSiteMap .sortable li span.handle-filename {
			padding: 3px;
			margin: 0;
			cursor: move;
			line-height: 20px;
		}
		
		#cmsSiteMap span.page-status, #cmsSiteMap span.handle-filename a {
			cursor: pointer;
		}
		
		.HighlightPH {
			height: 25px !important;
			margin: 5px;
			padding: 5px;
			border: 2px dashed #000000;
			background-color: #FFFFCC !important;
		}
	</style>
	<script type="text/javascript">

		$(document).ready(function () {

			$("ol.sortable").bind("sortupdate", function (event, ui) {
				//alert("sortupdate");
				var id = $(ui.item).attr('id');
				var p = $(ui.item).parent().parent().attr('id');
				//alert(p + '  ->  ' + id);

				setTimeout("itterateTree();", 250);
				setTimeout("BuildOrder();", 500);
				setTimeout("FindArrowAndOpen('" + p + "');", 750);

			});


			$("ol.sortable").nestedSortable({
				disableNesting: 'no-nest',
				forcePlaceholderSize: true,
				handle: 'span.map-handle',
				helper: 'clone',
				items: 'li',
				maxLevels: 5,
				opacity: .6,
				placeholder: 'HighlightPH',
				revert: 250,
				tabSize: 25,
				tolerance: 'pointer',
				toleranceElement: '> span'
			});

			itterateTree();
			itterateTreeSetToggle();

		});



		function itterateTree() {
			$("#cmsSiteMap li").each(function (i) {
				setListItem(this);
			});
		}


		function itterateTreeSetToggle() {
			$("#cmsSiteMap li span.page-status").each(function (i) {
				setDblClickSpan(this);
			});
		}


		function setDblClickSpan(elm) {
			$(elm).dblclick(function () {
				var target = $(elm).parent();
				var h = target.html();

				if (h.indexOf("ToggleTree") > 0) {
					var lnk = $(target).find("a");
					ToggleTree(lnk);
				}
			});
		}


		function setListItem(item) {

			var node = $(item).find("span.map-handle").first();
			var id = $(node).attr('id');
			var nodeNav = $(node).find("span.handle-filename");
			var nodeStat = $(node).find("span.page-status");

			var h = nodeNav.html();
			var t = nodeNav.text();
			var lst = $(item).find("ol").first();

			if (h.indexOf("ToggleTree") < 0) {
				if (lst.length > 0) {
					nodeNav.html('<a state="close" id="menu-lnk-' + id + '" onclick="ToggleTree(this);" href="javascript:void(0);">&#9658;</a>  ' + t); // &#9660;
					lst.attr('style', 'display:none;');
				} else {
					var lnk = nodeNav.find("a").first();
					lnk.remove();
				}
			} else {
				if (lst.length < 1) {
					var lnk = nodeNav.find("a").first();
					lnk.remove();
				}
			}
		}

		function FindArrowAndOpen(n) {
			if (n.length > 28) {
				var node = $('#' + n);
				//alert(n)
				var a = node.find("span.map-handle").find("a").first();
				if (a != null) {
					//alert(a.attr('state'));
					a.attr('state', 'close');
					ToggleTree(a);
				}
			}
		}


		function ToggleTree(arrow) {
			var a = $(arrow);
			//alert($(arrow).text() + '  ' + $(arrow).parent().parent().attr('id'));
			var lst = a.parent().parent().parent().find("ol").first();
			if (a.attr('state') != 'open') {
				a.html("&#9660;");
				lst.attr('style', 'display:block;');
				a.attr('state', 'open');
			} else {
				a.html("&#9658;");
				lst.attr('style', 'display:none;');
				a.attr('state', 'close');
			}
		}

		function BuildOrder() {
			var map = "#<%=txtMap.ClientID %>";
			$(map).val('');

			$("#cmsSiteMap li").each(function (i) {
				var itm = $(this);
				var id = itm.attr('id');
				var p = itm.parent().parent().attr('id');

				if (p.length < 25) {
					p = '<%=Guid.Empty %>';
				}

				var v = i + '\t' + p + '\t' + id;
				$(map).val($(map).val() + '\r\n ' + v);
			});
		}

		$(document).ready(function () {
			setTimeout("BuildOrder();", 250);
		});
		
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Site Map
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div style="display: none;">
		<asp:TextBox runat="server" ID="txtMap" TextMode="MultiLine" Columns="90" Rows="5"></asp:TextBox>
	</div>
	<p>
		<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
	</p>
	<div id="cmsSiteMap">
		<asp:Repeater ID="rpTop" runat="server" OnItemDataBound="rpMap_ItemDataBound">
			<HeaderTemplate>
				<ol class="sortable">
			</HeaderTemplate>
			<ItemTemplate>
				<li id="<%#Eval("Root_ContentID") %>"><span class="map-handle" id="handle-<%#Eval("Root_ContentID") %>"><span class="handle-filename" id="filename-<%#Eval("Root_ContentID") %>">
					<%#Eval("FileName")%>
				</span><span class="page-status">&nbsp;&nbsp;&nbsp; [<b><%#Eval("NavMenuText")%></b>] &nbsp;&nbsp;&nbsp;
					<img alt="status" class="image-status-icon img-status-<%#Eval("PageActive").ToString().ToLower() %>" src='<%#ReturnImage(Convert.ToBoolean(Eval("PageActive")))%>' />
				</span></span>
					<asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>
				</li>
			</ItemTemplate>
			<FooterTemplate>
				</ol></FooterTemplate>
		</asp:Repeater>
		<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/Manage/images/cancel.png" />
		<asp:HiddenField runat="server" ID="hdnActive" Visible="false" Value="/Manage/images/accept.png" />
	</div>
	<asp:Repeater ID="rpSub" runat="server" OnItemDataBound="rpMap_ItemDataBound">
		<HeaderTemplate>
			<ol>
		</HeaderTemplate>
		<%--<ItemTemplate>
			<li id="<%#Eval("Root_ContentID") %>"><span class="handle" id="handle-<%#Eval("Root_ContentID") %>">
				<%#Eval("FileName")%>
				<span class="page-status-<%#Eval("PageActive").ToString().ToLower() %>">- [<%#Eval("NavMenuText")%>]
					<img alt="status" src='<%#ReturnImage(Convert.ToBoolean(Eval("PageActive")))%>' />
				</span></span>
				<asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>
			</li>
		</ItemTemplate>--%>
		<FooterTemplate>
			</ol></FooterTemplate>
	</asp:Repeater>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
