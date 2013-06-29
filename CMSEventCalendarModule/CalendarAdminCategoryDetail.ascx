<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminCategoryDetail.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.CalendarAdminCategoryDetail" %>
<h2>
	Add/Edit Category
</h2>
<table style="width: 700px;">
	<tr>
		<td class="tablecaption">
			name:<br />
			<div style="border: 1px dotted #c0c0c0; background-color: #ffffff; margin: 2px; padding: 2px; min-width: 64px;">
				<div id="ColorTextSample" style="margin: 2px; padding: 2px;">
					Sample Text</div>
			</div>
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtCategoryName" runat="server" Columns="60" MaxLength="100" />
			<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtCategoryName" ID="RequiredFieldValidator1"
				runat="server" Text="**" ToolTip="Required" ErrorMessage="Required" Display="Dynamic" />
		</td>
	</tr>
	<tr>
		<td class="tablecaption">
			foreground color:<br />
			<div style="border: 1px dotted #c0c0c0; background-color: #ffffff; margin: 2px; padding: 2px; width: 48px;">
				<div id="ColorSampleFG" style="margin: 2px; padding: 2px;">
					&nbsp;</div>
			</div>
		</td>
		<td>
			<asp:DropDownList ID="ddlFGColor" runat="server" DataTextField="Value" DataValueField="Key" />
		</td>
	</tr>
	<tr>
		<td class="tablecaption">
			background color:<br />
			<div style="border: 1px dotted #c0c0c0; background-color: #ffffff; margin: 2px; padding: 2px; width: 48px;">
				<div id="ColorSampleBG" style="margin: 2px; padding: 2px;">
					&nbsp;</div>
			</div>
		</td>
		<td>
			<asp:DropDownList ID="ddlBGColor" runat="server" DataTextField="Value" DataValueField="Key" />
		</td>
	</tr>
</table>
<script type="text/javascript">

	function initColors() {
		var val1 = $("#<%=ddlFGColor.ClientID %>").val();
		setColor(val1, '#ColorSampleFG');
		var val2 = $("#<%=ddlBGColor.ClientID %>").val();
		setColor(val2, '#ColorSampleBG');
	}

	function setColor(color, cell) {
		$(cell).css('background-color', color);

		var fg = $('#ColorSampleFG').css('background-color');
		var bg = $('#ColorSampleBG').css('background-color');

		$('#ColorTextSample').css('color', fg);
		$('#ColorTextSample').css('background-color', bg);

	}

	$(document).ready(function () {

		$("#<%=ddlFGColor.ClientID %>").bind("change keyup input", function () {
			var val = $(this).val();
			setColor(val, '#ColorSampleFG');
		});

		$("#<%=ddlBGColor.ClientID %>").bind("change keyup input", function () {
			var val = $(this).val();
			setColor(val, '#ColorSampleBG');
		});

		initColors();
	});


</script>
<div>
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" Text="Save" OnClick="btnSaveButton_Click" />
</div>
