<%@ Page Title="" Language="C#" MasterPageFile="oilpainting.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageMainContentPlaceHolder" runat="server">
	<asp:PlaceHolder ID="PlaceHolder2" runat="server">
		<p>
			Posted By
			<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="Author_FullName_FirstLast" />
			on
			<carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="GoLiveDate" FieldFormat="{0:MMMM d, yyyy}" />
		</p>
	</asp:PlaceHolder>
	<carrot:WidgetContainer ID="phCenterTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
	<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
	<div style="clear: both;">
		<carrot:PostMetaWordList CssClass="meta" HtmlTagNameInner="li" HtmlTagNameOuter="ul" ID="wpl1" runat="server" ContentType="Category" MetaDataTitle="Categories:" />
		<carrot:PostMetaWordList CssClass="meta" HtmlTagNameInner="li" HtmlTagNameOuter="ul" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle="Tags:" />
	</div>
	<carrot:WidgetContainer ID="phRightTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyRight" runat="server" />
	<carrot:WidgetContainer ID="phRightBottom" runat="server" />

	<div id="contactForm">
		<carrot:ContentCommentForm runat="server" ID="commentFrm" ValidationGroup="CommentFrmValidGrp">
			<CommentEntryTemplate>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageSideContentPlaceHolder" runat="server">
	<carrot:WidgetContainer ID="phLeftTop" runat="server" />
	<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server" />
	<carrot:WidgetContainer ID="phLeftBottom" runat="server" />
	<ul>
		<li>
			<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" ContentType="DateMonth" MetaDataTitle="Dates" TakeTop="14" />
		</li>
		<li>
			<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" />
		</li>
		<li>
			<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" />
		</li>
	</ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageFootContentPlaceHolder" runat="server">
</asp:Content>
