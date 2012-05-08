<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTextContent.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.ucTextContent" %>
<div id="cms_EditZone<%=this.ClientID %>">
	<carrot:ContentContainer ID="GenericBody" runat="server"></carrot:ContentContainer>
</div>
<asp:Panel runat="server" ID="pnlJS">
	<div style="display: none">
		<div id="cms_Edit<%=this.ClientID %>" class="GlossySeaGreen" style="height: 300px; width: 730px; border: none 0px #000;">
			<div class="GlossySeaGreen ui-widget-header ui-corner-all" style="padding: 8px; margin: 0px;">
				Edit
				<%=GenericBody.ClientID%>
			</div>
			<asp:TextBox Style="height: 280px; width: 730px;" ID="txtGeneric" runat="server" TextMode="MultiLine" Rows="15" Columns="80"></asp:TextBox>
			<input class="GlossySeaGreen" onclick="clickGeneric<%=this.ClientID %>()" type="button" style="margin: 2px; padding: 2px;"
				id="btnDone" value="Done" />
		</div>
	</div>

	<script type="text/javascript">
	function click<%=this.ClientID %>(val) {
		alert("<%=this.ClientID %> - " + val);
	}
	</script>

	<script type="text/javascript">

	function clickGeneric<%=this.ClientID %>() {
		var v = $('#<%=txtGeneric.ClientID%>').val();
		setTimeout("$.modal.close();", 500);
		CMSBusyShort();
		cmsSaveGenericContent(v, '<%=PageWidgetID%>');
	}
	
	$(document).ready(function() {
		$('#cmsContentAreaLink_<%=GenericBody.ClientID %>').click(function() {
			Click<%=this.ClientID %>Edit();
		});
	});

	
	function Click<%=this.ClientID %>Edit() {
		$("#cms_Edit<%=this.ClientID %>").modal({
			onShow: function(dialog) {
				$(".GlossySeaGreen input:button, .GlossySeaGreen input:submit").button();
			},
			onClose: function(dialog) {
				$.modal.close();
			}
		});
		$('#cms_Edit<%=this.ClientID %>').modal();
		return false;
	}

	
	</script>

</asp:Panel>
