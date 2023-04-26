<%@ Page Title="" Language="C#" MasterPageFile="waterdrops.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageMainContentPlaceHolder" runat="server">
	<h2 class="title">
		<carrot:ContentPageProperty runat="server" ID="ContentPageProperty10" DataField="PageHead" />
	</h2>
	<div class="entry">
		<p>
			Posted By
			<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="Author_FullName_FirstLast" />
			on
			<carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="GoLiveDate" FieldFormat="{0:MMMM d, yyyy}" />
		</p>
		<carrot:WidgetContainer ID="phCenterTop" runat="server">
		</carrot:WidgetContainer>
		<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" TextZone="TextCenter" runat="server" />
		<p style="clear: both;">
			<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl1" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
			<br />
			<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
		</p>
		<carrot:WidgetContainer ID="phCenterBottom" runat="server">
		</carrot:WidgetContainer>
		<div id="contactForm">
			<carrot:ContentCommentForm runat="server" ID="commentFrm">
				<CommentEntryTemplate>
					<carrot:jsHelperLib runat="server" ID="jsHelperLib2" />
					<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
					<div>
						<asp:TextBox runat="server" ID="CommenterName" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" placeholder="Name" />
						<asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterName"
							ErrorMessage="Required" />
					</div>
					<div>
						<asp:TextBox runat="server" ID="CommenterEmail" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" placeholder="Email" />
						<asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterEmail"
							ErrorMessage="Required" />
					</div>
					<div>
						<asp:TextBox runat="server" ID="CommenterURL" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" placeholder="Website" />
					</div>
					<div>
						<br />
						<carrot:Captcha runat="server" ID="ContentCommentCaptcha" ValidationGroup="ContentCommentForm" CaptchaIsValidStyle-Style="clear: both; color: green;"
							CaptchaIsNotValidStyle-Style="clear: both; color: red;" CaptchaImageBoxStyle-Style="clear: both;" CaptchaInstructionStyle-Style="clear: both;"
							CaptchaTextStyle-Style="clear: both;" IsNotValidMessage="Code is not correct!" />
					</div>
					<div>
						<br />
						<asp:TextBox runat="server" ID="VisitorComments" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" placeholder="Message" />
					</div>
					<div>
						<br />
						<asp:Button ID="SubmitCommentButton" runat="server" Text="Send Message" ValidationGroup="ContentCommentForm" />
					</div>
					<script type="text/javascript">
						__carrotware_PageValidate();
					</script>
					<br />
				</CommentEntryTemplate>
			</carrot:ContentCommentForm>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageSideContentPlaceHolder" runat="server">
	<div>
		<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" CssClass="list-style1" ContentType="DateMonth" MetaDataTitle="Archive" TakeTop="6" />
	</div>
	<div>
		<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" CssClass="list-style1" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" />
	</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageFootContentPlaceHolder" runat="server">
	<p>&nbsp;</p>
</asp:Content>
