<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdminModule.ascx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.ucAdminModule" %>
<table style="width: 96%">
	<tr>
		<asp:PlaceHolder ID="pnlNav" runat="server">
			<td style="width: 275px;">
				<div style="width: 250px; padding-right: 25px;">
					<div id="module-accordion">
						<asp:Repeater ID="rpModuleList" runat="server" OnItemDataBound="rpModuleList_ItemDataBound">
							<ItemTemplate>
								<h3>
									<a href="#">
										<%# Eval("PluginName")%></a></h3>
								<div>
									<asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("PluginID")%>' />
									<asp:Repeater ID="rpModuleContents" runat="server">
										<ItemTemplate>
											<div class="moduleLink">
												<a <%# MarkSelected(Eval("PluginID").ToString(), Eval("PluginParm").ToString()) %> href="<%# CreateLink( Eval("UsePopup").ToString(), Eval("PluginID").ToString(), Eval("PluginParm").ToString() ) %>">
													<%# Eval("Caption")%>
												</a>
											</div>
										</ItemTemplate>
									</asp:Repeater>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>
				</div>
			</td>
		</asp:PlaceHolder>
		<td>
			<h2 class="widgetTitle">
				<asp:Literal runat="server" ID="litModuleTitle" /></h2>
			<asp:PlaceHolder ID="phAdminModule" runat="server" />
			<asp:PlaceHolder ID="phNone" runat="server" Visible="false">
				<div>
					<h2>No widget module selected.</h2>
				</div>
			</asp:PlaceHolder>
		</td>
	</tr>
</table>
<asp:PlaceHolder ID="pnlSetter" runat="server" Visible="false">
	<script type="text/javascript">
		function moduleUpdateAjaxJQuery() {
			if (typeof (Sys) != 'undefined') {
				var prm = Sys.WebForms.PageRequestManager.getInstance();
				prm.add_endRequest(function () {
					modulePageLoad();
				});
			}
		}

		$(document).ready(function () {
			moduleLoader();
		});

		setTimeout("moduleLoader();", 250);

		function moduleLoader() {
			moduleUpdateAjaxJQuery();
			modulePageLoad();
		}

		function modulePageLoad() {
			$("#module-accordion").accordion({
				heightStyle: "content",
				collapsible: true,
				active: parseInt('<%=SelMenu %>')
			});
		}
	</script>
</asp:PlaceHolder>
