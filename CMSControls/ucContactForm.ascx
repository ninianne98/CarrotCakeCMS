<%@ Control Language="C#" %>
<carrot:jsHelperLib runat="server" ID="jsHelperLib1" />
<div class="message-form" id="frmContactMessage" runat="server">
	<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
</div>
<div class="input-form" id="frmContactForm" runat="server">
	<div>
		<label>
			name:
			<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterName" ErrorMessage="*" />
		</label>
		<asp:TextBox runat="server" ID="CommenterName" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
	</div>
	<div>
		<label>
			email:
			<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterEmail" ErrorMessage="*" />
		</label>
		<asp:TextBox runat="server" ID="CommenterEmail" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
	</div>
	<div>
		<label>
			website:
		</label>
		<asp:TextBox runat="server" ID="CommenterURL" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
	</div>
	<div>
		<label>
			comment:
			<asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="VisitorComments" ClientValidationFunction="__carrotware_ValidateLongText"
				EnableClientScript="true" ErrorMessage="**" />
		</label>
		<asp:TextBox runat="server" ID="VisitorComments" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" />
	</div>
	<div>
		<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="ContentCommentCaptcha"
			ErrorMessage="**" />
		<carrot:Captcha runat="server" ID="ContentCommentCaptcha" ValidationGroup="ContentCommentForm" />
	</div>
	<div>
		<asp:Button ID="SubmitCommentButton" CssClass="button padding10" runat="server" Text="Submit Comment" ValidationGroup="ContentCommentForm" />
	</div>
</div>
