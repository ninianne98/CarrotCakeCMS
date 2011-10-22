<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGenericContent.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.ucGenericContent" %>
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
			<asp:TextBox Style="height: 280px; width: 730px;" CssClass="mceEditor2" ID="reGeneric" runat="server" TextMode="MultiLine" Rows="15" Columns="80"></asp:TextBox><br />
			<input class="GlossySeaGreen" onclick="clickGeneric<%=this.ClientID %>()" type="button" style="margin: 2px; padding: 2px;" id="btnDone" value="Done" />
		</div>
		<asp:TextBox Style="height: 280px; width: 730px;" ID="txtGeneric" runat="server" TextMode="MultiLine" Rows="15" Columns="80"></asp:TextBox>
	</div>

	<script type="text/javascript">
	function click<%=this.ClientID %>(val) {
		alert("<%=this.ClientID %> - " + val);
	}
	</script>

	<script type="text/javascript">

	function clickGeneric<%=this.ClientID %>() {
		tinyMCE.triggerSave();
		var ed = tinyMCE.get('<%=reGeneric.ClientID%>');
		var d = ed.getContent();

		tinyMCE.execCommand("mceRemoveControl", false, '<%=reGeneric.ClientID%>');
		setTimeout("$.modal.close();", 500);
		CMSBusyShort();
		cmsSaveGenericContent(d, '<%=PageWidgetID%>');
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

				var v = $('#<%=txtGeneric.ClientID%>').val();
				$('#<%=reGeneric.ClientID%>').val(v);

				setTimeout("initTinyMCE();", 200);
				setTimeout("tinyMCE.execCommand('mceRemoveControl', false, '<%=reGeneric.ClientID%>');", 400);
				setTimeout("tinyMCE.execCommand('mceAddControl', true, '<%=reGeneric.ClientID%>');", 600);
				setTimeout("tinyMCE.triggerSave();", 800);
			},
			onClose: function(dialog) {
				tinyMCE.execCommand("mceRemoveControl", false, '<%=reGeneric.ClientID%>');
				$.modal.close();
			}
		});
		$('#cms_Edit<%=this.ClientID %>').modal();
		return false;
	}

	
	</script>

</asp:Panel>
