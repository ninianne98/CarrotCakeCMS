<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoremIpsum.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.LoremIpsum.LoremIpsum" %>
<h3>
	<asp:Literal ID="litContent" runat="server" Text="" />
</h3>
<div class="ui-widget">
	<div class="ui-state-highlight ui-corner-all" style="padding: 0.2em; width: 680px; height: auto;">
		<div style="font-size: 1.3em; margin: 0.25em;">
			<div class="ui-icon ui-icon-info" style="float: left; margin: 5px; height: 16px; width: 16px"></div>
			<div style="float: left; margin: 0.2em; width: 600px; min-height: 2em;">
				This will create however many entries specified by "How Many" of the specified content type.
				Use with caution in a production site, this is intended for a dev or demo site where
				content needs to be created to experience the site design properly.
			</div>
			<div style="clear: both"></div>
		</div>
	</div>
</div>
<br />
<table>
	<tr>
		<td style="min-width: 90px; width: 9em;" class="tablecaption">How many
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtMany" runat="server" Columns="12" MaxLength="6" type="numeric" />
			<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtMany" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
				Display="Dynamic" />
		</td>
	</tr>

	<asp:PlaceHolder runat="server" ID="phPages">
		<tr>
			<td class="tablecaption">Top Level
			</td>
			<td>
				<asp:CheckBox ValidationGroup="inputForm" ID="chkTop" runat="server" />
			</td>
		</tr>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="phPosts">
		<tr>
			<td class="tablecaption">From
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtFrom" runat="server" CssClass="dateRegion" Columns="12" MaxLength="14" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtFrom" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
					Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">To
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTo" runat="server" CssClass="dateRegion" Columns="12" MaxLength="14" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtTo" ID="RequiredFieldValidator6" runat="server" ErrorMessage="Required"
					Display="Dynamic" />
			</td>
		</tr>

		<tr>
			<td class="tablecaption">Categories
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtCat" runat="server" Columns="12" MaxLength="6" type="numeric" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtCat" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
					Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td align="top">Tags
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtTag" runat="server" Columns="12" MaxLength="6" type="numeric" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtTag" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
					Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td class="tablecaption">Comments
			</td>
			<td>
				<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtComment" runat="server" Columns="12" MaxLength="6" type="numeric" />
				<asp:RequiredFieldValidator ValidationGroup="inputForm" ControlToValidate="txtComment" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required"
					Display="Dynamic" />
			</td>
		</tr>
	</asp:PlaceHolder>
</table>
<p>
	<asp:Button ValidationGroup="inputForm" ID="btnSave" class="myButton" runat="server" Text="Save" OnClick="btnSave_Click" />
</p>
<p>
	<asp:Label runat="server" ID="lblSummary" />
</p>
<asp:Repeater runat="server" ID="rpUrls">
	<HeaderTemplate>
		<ul>
	</HeaderTemplate>
	<ItemTemplate>
		<li><a target="_blank" href='<%# String.Format( "{0}", Container.DataItem ) %>'>
			<%# String.Format( "{0}", Container.DataItem ) %></a></li>
	</ItemTemplate>
	<FooterTemplate>
		</ul>
	</FooterTemplate>
</asp:Repeater>
