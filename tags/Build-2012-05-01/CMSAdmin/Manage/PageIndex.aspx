<%@ Page Title="PageIndex" Language="C#" MasterPageFile="MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PageIndex.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.PageIndex" %>

<%@ MasterType VirtualPath="MasterPages/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Page Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<p>
		<a href="PageAddEdit.aspx?id=">
			<img border="0" src="/manage/images/add.png" alt="Add" title="Add as WYSIWYG" />
			Add Page (with editor)</a><br />
		<a href="PageAddEdit.aspx?mode=raw&id=">
			<img border="0" src="/manage/images/script_add.png" alt="Add" title="Add as Plain Text" />
			Add Page (as plain text)</a>
	</p>
	<p>
		<a href="SiteMap.aspx">
			<img border="0" src="/manage/images/chart_organisation.png" alt="Map" title="Edit Site Map" />
			Edit Site Map</a>
	</p>
	<br />
	<div id="jqtabs" style="min-height: 350px; width: 800px;">
		<ul>
			<li><a href="#pageidx-tabs-1">Edit Pages</a></li>
			<li><a href="#pageidx-tabs-2">Mass Update Templates</a></li>
		</ul>
		<div id="pageidx-tabs-1">
			<div id="SortableGrid">
				<asp:GridView CssClass="datatable" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
					AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#  String.Format("./PageAddEdit.aspx?id={0}", Eval("Root_ContentID")) %>'><img border="0" src="/Manage/images/pencil.png" alt="Edit with WYSIWYG" title="Edit with WYSIWYG" /></asp:HyperLink>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:HyperLink runat="server" ID="lnkEdit2" NavigateUrl='<%#  String.Format("./PageAddEdit.aspx?mode=raw&id={0}", Eval("Root_ContentID")) %>'><img border="0" src="/Manage/images/script.png" alt="Edit with Plain Text" title="Edit with Plain Text" /></asp:HyperLink>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:HyperLink runat="server" Target="_blank" ID="lnkEdit3" NavigateUrl='<%#  String.Format("{0}?carrotedit=true", Eval("FileName")) %>'><img border="0" src="/Manage/images/overlays.png" alt="Advanced Editor" title="Advanced Editor" /></asp:HyperLink>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<ItemTemplate>
								<a href="javascript:void(0)" onclick="ShowWindowNoRefresh('/Manage/PageHistory.aspx?id=<%#Eval("Root_ContentID") %>');">
									<img border="0" src="/Manage/images/layout_content.png" alt="View Page History" title="View Page History" /></a>
							</ItemTemplate>
						</asp:TemplateField>
						<%--<asp:TemplateField>
							<HeaderTemplate>
								<asp:LinkButton OnClick="lblSort_Command" ID="lnkHead0" runat="server" CommandName="NavOrder">Order</asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("NavOrder")%>
							</ItemTemplate>
						</asp:TemplateField>--%>
						<asp:TemplateField>
							<HeaderTemplate>
								<asp:LinkButton OnClick="lblSort_Command" ID="lnkHead1" runat="server" CommandName="TitleBar">Page Title</asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("TitleBar")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								<asp:LinkButton OnClick="lblSort_Command" ID="lnkHead2" runat="server" CommandName="PageHead">Page Heading</asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("PageHead")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								<asp:LinkButton OnClick="lblSort_Command" ID="lnkHead5" runat="server" CommandName="FileName">Filename</asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("FileName")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								<asp:LinkButton OnClick="lblSort_Command" ID="lnkHead6" runat="server" CommandName="NavMenuText">Nav Menu Text</asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("NavMenuText")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								<asp:LinkButton OnClick="lblSort_Command" ID="lnkHead3" runat="server" CommandName="EditDate">Last Edited</asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("EditDate")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField ItemStyle-HorizontalAlign="Center">
							<HeaderTemplate>
								<asp:LinkButton OnClick="lblSort_Command" ID="lnkHead4" runat="server" CommandName="PageActive">Active</asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Image ID="imgActive" runat="server" ImageUrl="/Manage/images/accept.png" AlternateText="Active" />
								<asp:HiddenField ID="hdnIsActive" Visible="false" runat="server" Value='<%#Eval("PageActive") %>' />
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>
			</div>
		</div>
		<div id="pageidx-tabs-2">
			<table>
				<tr>
					<td valign="top" class="tablecaption">
						template:
					</td>
					<td valign="top">
						<asp:DropDownList DataTextField="Caption" DataValueField="TemplatePath" ID="ddlTemplate" runat="server">
						</asp:DropDownList>
					</td>
				</tr>
			</table>
			<div id="SortableGrid">
				<asp:GridView CssClass="datatable" ID="gvApply" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
					AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
					<Columns>
						<asp:TemplateField ItemStyle-HorizontalAlign="Center">
							<HeaderTemplate>
								Remap
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox ID="chkReMap" runat="server" />
								<asp:HiddenField ID="hdnContentID" runat="server" Value='<%# Eval("Root_ContentID") %>' />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Template File
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("TemplateFile")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Page Title
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("TitleBar")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Page Heading
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("PageHead")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Filename
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("FileName")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Nav Menu Text
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("NavMenuText")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Last Edited
							</HeaderTemplate>
							<ItemTemplate>
								<%# Eval("EditDate")%>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField ItemStyle-HorizontalAlign="Center">
							<HeaderTemplate>
								Active
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Image ID="imgActive" runat="server" ImageUrl="/Manage/images/accept.png" AlternateText="Active" />
								<asp:HiddenField ID="hdnIsActive" Visible="false" runat="server" Value='<%#Eval("PageActive") %>' />
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>
				<div style="height: 50px; margin-top: 10px; margin-bottom: 10px;">
					<asp:Button ID="btnSaveMapping" runat="server" Text="Save" OnClick="btnSaveMapping_Click" />
				</div>
			</div>
		</div>
	</div>
	<br />
	<asp:HiddenField runat="server" ID="hdnInactive" Visible="false" Value="/Manage/images/cancel.png" />
	<asp:HiddenField runat="server" ID="hdnSort" Visible="false" Value="TitleBar ASC" />

	<script type="text/javascript">

		function ajaxIndex() {
			if (typeof (Sys) != 'undefined') {
				var prm = Sys.WebForms.PageRequestManager.getInstance();
				prm.add_endRequest(function() {

					$(function() {
						$('#jqtabs').tabs();
					});

					$('#jqtabs').tabs('select', '<%=sTab %>');
				});
			}
		}

		ajaxIndex();

		$(document).ready(function() {
			$('#jqtabs').tabs('select', '<%=sTab %>');
		});

		function SetClientTab(tabID) {
			$('#jqtabs').tabs('select', tabID);
			//alert(tabID);
		}
		
	</script>

</asp:Content>
