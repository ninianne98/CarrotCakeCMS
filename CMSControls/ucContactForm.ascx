<%@ Control Language="C#" %>
<carrot:jsHelperLib runat="server" ID="jsHelperLib1" />
<div class="message-form contact-frm-msg" id="frmContactMessage" runat="server">
	<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
</div>
<div class="input-form contact-frm" id="frmContactForm" runat="server">
	<div>
		<label>
			name:
			<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CommenterName" ErrorMessage="*" />
		</label>
		<asp:TextBox runat="server" ID="CommenterName" CssClass="contact-fld contact-name" Columns="30" MaxLength="100" />
	</div>
	<div>
		<label>
			email:
			<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="CommenterEmail" ErrorMessage="*" />
		</label>
		<asp:TextBox runat="server" ID="CommenterEmail" CssClass="contact-fld contact-email" Columns="30" MaxLength="100" />
	</div>
	<div>
		<label>
			website:
		</label>
		<asp:TextBox runat="server" ID="CommenterURL" CssClass="contact-fld contact-url" Columns="30" MaxLength="100" />
	</div>
	<div>
		<label>
			comment:
			<asp:CustomValidator ID="CustomValidator1" runat="server"
				ControlToValidate="VisitorComments" ClientValidationFunction="__carrotware_ValidateLongText"
				EnableClientScript="true" ErrorMessage="**" />
		</label>
		<asp:TextBox runat="server" ID="VisitorComments" CssClass="contact-fld contact-comment" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" />
	</div>
	<div>
		<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
			ControlToValidate="ContentCommentCaptcha" ErrorMessage="**" />
		<carrot:Captcha runat="server" ID="ContentCommentCaptcha" CssClass="contact-fld contact-captcha"
			CaptchaIsValidStyle-CssClass="contact-valid-frm" CaptchaIsValidStyle-Style="clear: both; color: green;"
			CaptchaIsNotValidStyle-CssClass="contact-not-valid-frm" CaptchaIsNotValidStyle-Style="clear: both; color: red;"
			CaptchaImageBoxStyle-Style="clear: both;" CaptchaInstructionStyle-Style="clear: both;"
			CaptchaTextStyle-Style="clear: both;" IsNotValidMessage="Code is not correct!" />
	</div>
	<div>
		<asp:Button ID="SubmitCommentButton" CssClass="button contact-btn" runat="server" Text="Submit Comment" />
	</div>
</div>
