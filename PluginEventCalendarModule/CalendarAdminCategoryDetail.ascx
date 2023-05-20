<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarAdminCategoryDetail.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.EventCalendarModule.CalendarAdminCategoryDetail" %>
<h2>Add/Edit Category</h2>
<table style="width: 700px;">
	<tr>
		<td class="tablecaption">name:<br />
			<div style="border: 2px dotted #aaaaaa; background-color: #ffffff; margin: 2px; padding: 2px; min-width: 64px;">
				<div id="ColorTextSample" style="margin: 2px; padding: 4px; font-size: 14px;">
					Sample Text
				</div>
			</div>
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" ID="txtCategoryName" runat="server" Columns="60" MaxLength="100" />
			<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtCategoryName" ID="RequiredFieldValidator1"
				runat="server" Text="**" ToolTip="Required" ErrorMessage="Required" Display="Dynamic" />
		</td>
	</tr>
	<tr>
		<td class="tablecaption">foreground color:<br />
			<div style="border: 2px dotted #aaaaaa; background-color: #ffffff; margin: 2px; padding: 2px; width: 48px;">
				<div id="ColorSampleFG" style="margin: 2px; padding: 2px; font-size: 14px;">
					&nbsp;
				</div>
			</div>
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" runat="server" ID="txtFgColor" Columns="20" MaxLength="7" CssClass="color-field" />
			<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtFgColor" ID="RequiredFieldValidator2"
				runat="server" Text="**" ToolTip="Required" ErrorMessage="Required" Display="Dynamic" />
		</td>
	</tr>
	<tr>
		<td class="tablecaption">background color:<br />
			<div style="border: 2px dotted #aaaaaa; background-color: #ffffff; margin: 2px; padding: 2px; width: 48px;">
				<div id="ColorSampleBG" style="margin: 2px; padding: 2px; font-size: 14px;">
					&nbsp;
				</div>
			</div>
		</td>
		<td>
			<asp:TextBox ValidationGroup="inputForm" onkeypress="return ProcessKeyPress(event)" runat="server" ID="txtBgColor" Columns="20" MaxLength="7" CssClass="color-field" />
			<asp:RequiredFieldValidator ValidationGroup="inputForm" CssClass="validationError" ForeColor="" ControlToValidate="txtBgColor" ID="RequiredFieldValidator3"
				runat="server" Text="**" ToolTip="Required" ErrorMessage="Required" Display="Dynamic" />
		</td>
	</tr>
</table>
<script type="text/javascript">

	function initColors() {
		var val1 = $("#<%=txtFgColor.ClientID %>").val();
		setColor(val1, '#ColorSampleFG');
		var val2 = $("#<%=txtBgColor.ClientID %>").val();
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
		$("#<%=txtFgColor.ClientID %>").bind("change keyup input blur", function () {
			var val = $(this).val();
			setColor(val, '#ColorSampleFG');
		});

		$("#<%=txtBgColor.ClientID %>").bind("change keyup input blur", function () {
			var val = $(this).val();
			setColor(val, '#ColorSampleBG');
		});

		initColors();
	});
</script>
<div>
	<asp:Button ValidationGroup="inputForm" ID="btnSaveButton" runat="server" Text="Save" OnClick="btnSaveButton_Click" />
	&nbsp;&nbsp;&nbsp;
	<input type="button" id="btnCancel" value="Cancel" onclick="cancelEditing()" />
</div>
<script type="text/javascript">

	function cancelEditing() {
		window.setTimeout("location.href = '<%= CancelURL%>';", 250);
	}
</script>
