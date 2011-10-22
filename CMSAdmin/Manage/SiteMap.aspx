<%@ Page ValidateRequest="false" Title="SiteMap" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true"
	CodeBehind="SiteMap.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.SiteMap" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

	<script src="Includes/jquery.ui.nestedSortable.js" type="text/javascript"></script>

	<style>
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
		#cmsSiteMap .sortable li span {
			padding: 3px;
			margin: 0;
			cursor: move;
			line-height: 20px;
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

		$(document).ready(function() {

			$("ol.sortable").bind("sortupdate", function(event, ui) {
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
				handle: 'span',
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

		});



		function itterateTree() {
			$("#cmsSiteMap li").each(function(i) {
				setListItem(this);
			});
		}

		function setListItem(item) {
			var node = $(item).find("span").first();
			var h = node.html();
			var t = node.text();
			var lst = $(item).find("ol").first();
			if (h.indexOf("ToggleTree") < 0) {
				if (lst.length > 0) {
					node.html('<a state="close" onclick="ToggleTree(this);" href="javascript:void(0);">&#9658;</a>  ' + t); // &#9660;
					lst.attr('style', 'display:none;');
				} else {
					var lnk = node.find("a").first();
					lnk.remove();
				}
			} else {
				if (lst.length < 1) {
					var lnk = node.find("a").first();
					lnk.remove();
				}
			}
		}

		function FindArrowAndOpen(n) {
			if (n.length > 28) {
				var node = $('#' + n);
				//alert(n)
				var a = node.find("span").find("a").first();
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
			var lst = a.parent().parent().find("ol").first();
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

			$("#cmsSiteMap li").each(function(i) {
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

		$(document).ready(function() {
			setTimeout("BuildOrder();", 250);
		});
		
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	SiteMap
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
				<li id="<%#Eval("Root_ContentID") %>"><span>
					<%#Eval("FileName")%>
					- [[<%#Eval("NavMenuText")%>]]</span>
					<asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>
				</li>
			</ItemTemplate>
			<FooterTemplate>
				</ol></FooterTemplate>
		</asp:Repeater>
	</div>
	<asp:Repeater ID="rpSub" runat="server" OnItemDataBound="rpMap_ItemDataBound">
		<HeaderTemplate>
			<ol>
		</HeaderTemplate>
		<ItemTemplate>
			<li id="<%#Eval("Root_ContentID") %>"><span>
				<%#Eval("FileName")%>
				- [[<%#Eval("NavMenuText")%>]]</span>
				<asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol></FooterTemplate>
	</asp:Repeater>
	<%--	 
	<div id="cmsSiteMap">
		<ol class="sortable">
			<li id="itm1"><span>Item 1</span></li>
			<li id="itm2"><span>Item 2</span>
				<ol>
					<li id="itm2.1"><span>Sub Item 2.1</span></li>
					<li id="itm2.2"><span>Sub Item 2.2</span></li>
					<li id="itm2.3"><span>Sub Item 2.3</span></li>
					<li id="itm2.4"><span>Sub Item 2.4</span></li>
				</ol></li>
			<li id="itm3"><span>Item 3 </span></li>
			<li id="itm4"><span>Item 4 </span></li>
			<li id="itm5"><span>Item 5</span></li>
			<li id="itm6"><span>Item 6</span>
				<ol>
					<li id="itm6.1"><span>Sub Item 6.1</span></li>
					<li id="itm6.2"><span>Sub Item 6.2</span>
						<ol>
						<li id="itm6.2a"><span>Sub Item 6.2 a</span></li>
						<li id="itm6.2b"><span>Sub Item 6.2 b</span></li>
						</ol> </li>
					<li id="itm6.3"><span>Sub Item 6.3</span></li>
					<li id="itm6.4"><span>Sub Item 6.4</span></li>
				</ol></li>
			<li id="itm7"><span>Item 7</span></li>
			<li id="itm8"><span>Item 8</span></li>
		</ol>
		
	</div>
 --%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
