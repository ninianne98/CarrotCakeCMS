<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdminModule.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.ucAdminModule" %>
<style type="text/css">
	.selectedModule {
		font-weight: bolder;
	}
</style>
<table width="95%">
	<tr>
		<asp:Panel ID="pnlNav" runat="server">
			<td valign="top" style="width: 275px;">
				<div style="width: 250px; padding-right: 25px;">
					<div id="jqaccordion">
						<asp:Repeater ID="rpModuleList" runat="server" OnItemDataBound="rpModuleList_ItemDataBound">
							<ItemTemplate>
								<h3>
									<a href="#">
										<%# Eval("PluginName")%></a></h3>
								<div>
									<asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("PluginID")%>' />
									<asp:Repeater ID="rpModuleContents" runat="server">
										<ItemTemplate>
											<a <%# MarkSelected(Eval("PluginID").ToString(), Eval("PluginParm").ToString()) %> href="<%# CreateLink( Eval("UsePopup").ToString(), Eval("PluginID").ToString(), Eval("PluginParm").ToString() ) %>">
												<%# Eval("Caption")%>
											</a>
											<br />
										</ItemTemplate>
									</asp:Repeater>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>
				</div>
			</td>
		</asp:Panel>
		<td valign="top">
			<asp:PlaceHolder ID="phAdminModule" runat="server"></asp:PlaceHolder>
		</td>
	</tr>
</table>
<asp:Panel ID="pnlSetter" runat="server" Visible="false">
	<script type="text/javascript">
		
			function moduleUpdateAjaxJQuery() {
				if (typeof (Sys) != 'undefined') {
					var prm = Sys.WebForms.PageRequestManager.getInstance();
						prm.add_endRequest(function() {
						modulePageLoad();
					});
				}
			}

			$(document).ready(function() {
				moduleUpdateAjaxJQuery();
				modulePageLoad();
			});

			function modulePageLoad() {
				$("#jqaccordion").accordion("option", "active", <%=SelMenu %>);
			}
			
	</script>
</asp:Panel>
