<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSiteMap.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucSiteMap" %>
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
		padding: 0;
		margin: 0;
		padding-left: 15px;
		margin-left: 5px;
		margin-bottom: 8px;
	}
	#cmsSiteMap .image-handle {
		padding: 3px;
		margin: 0;
		cursor: move;
		line-height: 25px;
	}
	
	#cmsSiteMap span.page-status, #cmsSiteMap span.handle-expand a {
		cursor: pointer;
	}
	#cmsSiteMap span.handle-expand {
		width: 50px !important;
		height: 25px !important;
		border: 1px dashed #ffffff;
	}
	
	#cmsSiteMap img {
		vertical-align: text-top;
	}
	.HighlightPH {
		height: 25px !important;
		margin: 5px;
		padding: 5px;
		border: 2px dashed #000000;
		background-color: #FFFFCC !important;
		width: 400px !important;
		display: block !important;
	}
</style>
<script type="text/javascript">
	var handleCssClass = 'img.image-handle';

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
			handle: handleCssClass,
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

		var imgNode = $(item).find(handleCssClass).first();
		var id = $(imgNode).attr('id');
		//alert(id);
		var node = $(imgNode).parent();
		var nodeNav = $(node).find("span.handle-expand");
		var nodeStat = $(node).find("span.page-status");

		var h = nodeNav.html();
		//var t = nodeNav.text();
		var lst = $(item).find("ol").first();
		var blankField = '&nbsp;&#9671;&nbsp; ';

		if (h.indexOf("ToggleTree") < 0) {
			if (lst.length > 0) {
				//nodeNav.html('<a state="close" id="menu-lnk-' + id + '" onclick="ToggleTree(this);" href="javascript:void(0);">&#9658;</a>  ' + t); // &#9660;
				nodeNav.html('&nbsp;<a state="close" id="menu-lnk-' + id + '" onclick="ToggleTree(this);" href="javascript:void(0);">&#9658;</a>&nbsp; '); // &#9660;
				lst.attr('style', 'display:none;');
			} else {
				var lnk = nodeNav.find("a").first();
				lnk.remove();
				nodeNav.html(blankField);
			}
		} else {
			if (lst.length < 1) {
				var lnk = nodeNav.find("a").first();
				lnk.remove();
				nodeNav.html(blankField);
			}
		}
	}

	function FindArrowAndOpen(n) {
		if (n.length > 28) {
			var node = $('#' + n);
			//alert(n)
			var a = node.find(handleCssClass).parent().find("a").first();
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
<div style="display: none;">
	<asp:TextBox runat="server" ID="txtMap" TextMode="MultiLine" Columns="90" Rows="5" />
</div>
<p>
	<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
	&nbsp;&nbsp;&nbsp;
	<asp:Button ID="btnFixOrphan" runat="server" Text="Fix Orphaned Pages " OnClick="btnFixOrphan_Click" />
	&nbsp;&nbsp;&nbsp;
	<asp:Button ID="btnFixBlog" runat="server" Text="Fix Blog Navs " OnClick="btnFixBlog_Click" />
</p>
<div id="cmsSiteMap">
	<asp:Repeater ID="rpTop" runat="server" OnItemDataBound="rpMap_ItemDataBound">
		<HeaderTemplate>
			<ol class="sortable">
		</HeaderTemplate>
		<ItemTemplate>
			<li id="<%#Eval("Root_ContentID") %>"><span class="page-info" id="handle-<%#Eval("Root_ContentID") %>"><span class="handle-expand" id="filename-<%#Eval("Root_ContentID") %>">
				&nbsp; </span>
				<img src="/c3-admin/images/webpage.png" class="imgNoBorder image-handle" title="webpage" alt="webpage" id="img-<%#Eval("Root_ContentID") %>" />
				<span class="page-status"><a href="<%#Eval("FileName")%>" target="_blank">
					<%#Eval("FileName")%>
					&nbsp;&nbsp;&nbsp;&nbsp; [<b><%#Eval("NavMenuText")%></b>] </a>&nbsp;&nbsp;&nbsp;&nbsp;
					<img alt="status" title="status <%#Eval("PageActive").ToString().ToLower() %>" class="image-status-icon img-status-<%#Eval("PageActive").ToString().ToLower() %>"
						src='<%#ReturnImage(Convert.ToBoolean(Eval("PageActive")))%>' />
					<img alt="navstatus" title="navstatus <%#Eval("ShowInSiteNav").ToString().ToLower() %>" class="image-navstatus-icon img-navstatus-<%#Eval("ShowInSiteNav").ToString().ToLower() %>"
						src='<%#ReturnNavImage(Convert.ToBoolean(Eval("ShowInSiteNav")))%>' />
				</span></span>
				<asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol></FooterTemplate>
	</asp:Repeater>
	<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/c3-admin/images/cancel.png" />
	<asp:HiddenField runat="server" ID="hdnActive" Visible="false" Value="/c3-admin/images/accept.png" />
	<asp:HiddenField runat="server" ID="hdnNavShow" Visible="false" Value="/c3-admin/images/lightbulb.png" />
	<asp:HiddenField runat="server" ID="hdnNavHide" Visible="false" Value="/c3-admin/images/lightbulb_off.png" />
</div>
<asp:Repeater ID="rpSub" runat="server" OnItemDataBound="rpMap_ItemDataBound">
	<HeaderTemplate>
		<ol>
	</HeaderTemplate>
	<%--use ItemTemplate from top level--%>
	<FooterTemplate>
		</ol></FooterTemplate>
</asp:Repeater>
