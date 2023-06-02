<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSitePageDrillDown.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucSitePageDrillDown" %>
<script src="Includes/page-drill-down.js"></script>
<script type="text/javascript">
	InitDrillParms('<%=RootContentID.ToString() %>', '<%=txtParent.ClientID %>');
</script>
<asp:TextBox Style="display: none" runat="server" ID="txtParent" />
<div id="nodedrilldown">
	<div id="menupath" class="pageNodeDrillDown1 ui-widget">
	</div>
	<div class="pageNodeDrillDown5 ui-widget">
		<div id="menuhead" onmouseout="hideMnu()" onmouseover="mouseNode()" class="menuitems pageNodeDrillDown4 ui-button ui-corner-all ui-widget">
			<div class="pageNodeDrillDown6 ui-widget">
				&nbsp;Pages <a title="Reset Path" href='javascript:void(0);' onclick='selectDrillItem(this);' data-rootcontent=''>
					<div class="ui-icon ui-icon-power"></div>
				</a>
			</div>
		</div>
		<div id="menuitemsouter">
			<div id="menuitemsinner" class="scroll">
			</div>
		</div>
	</div>
</div>
