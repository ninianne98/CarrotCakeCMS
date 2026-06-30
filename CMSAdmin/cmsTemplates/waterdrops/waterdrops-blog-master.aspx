<%@ Page Title="" Language="C#" MasterPageFile="waterdrops.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<%@ MasterType VirtualPath="waterdrops.master" %>

<script runat="server">
	protected override void OnInit(EventArgs e) {
		base.OnInit(e);

		Master.IsHome = false;
	}
</script>

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
			<carrot:ContentCommentForm runat="server" ID="commentFrm" ValidationGroup="CommentFrmValidGrp">
				<CommentEntryTemplate>
					<carrot:jsHelperLib runat="server" ID="jsHelperLib1" />
					<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
					<div>
						<asp:TextBox runat="server" ID="CommenterName" Columns="30" MaxLength="100" placeholder="Name" />
						<asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="CommenterName"
							ErrorMessage="Required" />
					</div>
					<div>
						<asp:TextBox runat="server" ID="CommenterEmail" Columns="30" MaxLength="100" placeholder="Email" />
						<asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="CommenterEmail"
							ErrorMessage="Required" />
					</div>
					<div>
						<asp:TextBox runat="server" ID="CommenterURL" Columns="30" MaxLength="100" placeholder="Website" />
					</div>
					<div>
						<br />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ContentCommentCaptcha"
							ErrorMessage="**" />
						<carrot:Captcha runat="server" ID="ContentCommentCaptcha" CaptchaIsValidStyle-Style="clear: both; color: green;"
							CaptchaIsNotValidStyle-Style="clear: both; color: red;" CaptchaImageBoxStyle-Style="clear: both;" CaptchaInstructionStyle-Style="clear: both;"
							CaptchaTextStyle-Style="clear: both;" IsNotValidMessage="Code is not correct!" />
					</div>
					<div>
						<br />
						<asp:TextBox runat="server" ID="VisitorComments" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" placeholder="Message" />
					</div>
					<div>
						<br />
						<asp:Button ID="SubmitCommentButton" runat="server" Text="Send Message" />
					</div>
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
