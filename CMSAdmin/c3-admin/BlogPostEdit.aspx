<%@ Page Title="Edit Post Info" Language="C#" MasterPageFile="~/c3-admin/MasterPages/MainPopup.Master" AutoEventWireup="true" CodeBehind="BlogPostEdit.aspx.cs"
	Inherits="Carrotware.CMS.UI.Admin.c3_admin.BlogPostEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="H1ContentPlaceHolder" runat="server">
	Edit Post Info
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<h2>
		<asp:Literal ID="litPageName" runat="server"></asp:Literal></h2>
	<table width="700">
		<tr>
			<td width="125" class="tablecaption" valign="top">
				last updated:
			</td>
			<td width="575" valign="top">
				<asp:Label ID="lblUpdated" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				release date:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseDate" runat="server" CssClass="dateRegion" Columns="16" />
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtReleaseTime" runat="server" CssClass="timeRegion" Columns="10" />
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				retire date:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireDate" runat="server" CssClass="dateRegion" Columns="16" />
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtRetireTime" runat="server" CssClass="timeRegion" Columns="10" />
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				title bar:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTitle" runat="server" Columns="45" MaxLength="200" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtTitle" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
					Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				navigation:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtNav" runat="server" Columns="45" MaxLength="200" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtNav" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
					Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				page head:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtHead" runat="server" Columns="45" MaxLength="200" />
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				thumbnail:
				<br />
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtThumb" runat="server" Columns="45" MaxLength="200" />
				<input type="button" id="btnThumb" value="Browse" onclick="cmsFileBrowserOpenReturnPop('<%=txtThumb.ClientID %>');return false;" />
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				&nbsp;
			</td>
			<td valign="top">
				<asp:CheckBox ID="chkActive" runat="server" Text="Show publicly" />
				<%--&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkNavigation" runat="server" Text="Include in site navigation" Checked="true" />--%>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				meta keywords:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" ID="txtKey" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td valign="top" class="tablecaption">
				meta description:
			</td>
			<td valign="top">
				<asp:TextBox ValidationGroup="inputForm" ID="txtDescription" MaxLength="1000" Columns="60" Style="width: 425px;" Rows="4" TextMode="MultiLine" runat="server"></asp:TextBox>
			</td>
		</tr>
	</table>
	<table width="700">
		<tr>
			<td width="350" valign="top">
				<div style="width: 340px; clear: both;">
					<div class="picker-area ui-widget-header ui-state-default ui-corner-top" style="width: 340px">
						categories</div>
					<div class="picker-area ui-widget-content ui-corner-bottom scroll-container" style="width: 340px">
						<div class="scroll-area">
							<asp:Repeater ID="rpCat" runat="server">
								<ItemTemplate>
									<asp:CheckBox runat="server" ID="chk" Text='<%#Eval("CategoryText") %>' value='<%#Eval("ContentCategoryID") %>' /><br />
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
				</div>
				<div style="clear: both;">
				</div>
			</td>
			<td>
				&nbsp;&nbsp;
			</td>
			<td width="350" valign="top">
				<div style="width: 340px; clear: both;">
					<div class="picker-area ui-widget-header ui-state-default ui-corner-top" style="width: 340px">
						tags</div>
					<div class="picker-area ui-widget-content ui-corner-bottom scroll-container" style="width: 340px">
						<div class="scroll-area">
							<asp:Repeater ID="rpTag" runat="server">
								<ItemTemplate>
									<asp:CheckBox runat="server" ID="chk" Text='<%#Eval("TagText") %>' value='<%#Eval("ContentTagID") %>' /><br />
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
				</div>
				<div style="clear: both;">
				</div>
			</td>
		</tr>
	</table>
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" OnClientClick="SubmitPage()" Text="Apply" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<%-- <input type="button" id="btnCancel" value="Cancel" onclick="location.href='./PageIndex.aspx';" />--%>
	<br />
	<div style="display: none;">
		<asp:Button ValidationGroup="inputForm" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Apply" />
	</div>
	<script type="text/javascript">
		var saving = 0;

		function SubmitPage() {
			saving = 1;
			setTimeout("ClickBtn();", 500);
		}
		function ClickBtn() {
			$('#<%=btnSave.ClientID %>').click();
		}
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NoAjaxContentPlaceHolder" runat="server">
</asp:Content>
