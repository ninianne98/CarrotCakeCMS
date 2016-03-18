<%@ Page Title="" Language="C#" MasterPageFile="aqueous.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<%@ Register Src="navBlog.ascx" TagPrefix="uc" TagName="navBlog" %>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<!-- Sidebar -->
	<div id="sidebar" class="4u">
		<uc:navBlog runat="server" ID="navBlog1" />
	</div>
	<!-- Sidebar -->
	<!-- Content -->
	<div id="content" class="8u skel-cell-important">
		<section>
			<header>
				<h2>
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h2>
				<span class="byline">By
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty2" DataField="Credit_FullName_FirstLast" />
					on
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="GoLiveDate" FieldFormat="{0:d}" />
				</span>
			</header>
			<carrot:WidgetContainer ID="phCenterTop" runat="server" />
			<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
			<div id="meta-zone">
				<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl1" runat="server" ContentType="Category" MetaDataTitle=" " />
				<br />
				<carrot:PostMetaWordList HtmlTagNameInner="span" HtmlTagNameOuter="span" ID="wpl2" runat="server" ContentType="Tag" MetaDataTitle=" " />
			</div>
			<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
		</section>
		<div>
			<carrot:ContentCommentForm runat="server" ID="commentFrm">
				<CommentEntryTemplate>
					<carrot:jsHelperLib runat="server" ID="jsHelperLib2" />
					<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
					<div class="row 50%">
						<div class="12u">
							<asp:TextBox runat="server" ID="CommenterName" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" placeholder="Name" />
							<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterName"
								ErrorMessage="Required" />
						</div>
					</div>
					<div class="row 50%">
						<div class="12u">
							<asp:TextBox runat="server" ID="CommenterEmail" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" placeholder="Email" />
							<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterEmail"
								ErrorMessage="Required" />
						</div>
					</div>
					<div class="row 50%">
						<div class="12u">
							<asp:TextBox runat="server" ID="CommenterURL" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" placeholder="Website" />
						</div>
					</div>
					<div class="row 50%">
						<div class="12u">
							<carrot:Captcha runat="server" ID="ContentCommentCaptcha" ValidationGroup="ContentCommentForm" CaptchaIsValidStyle-Style="clear: both; color: green;"
								CaptchaIsNotValidStyle-Style="clear: both; color: red;" CaptchaImageBoxStyle-Style="clear: both;" CaptchaInstructionStyle-Style="clear: both;"
								CaptchaTextStyle-Style="clear: both;" IsNotValidMessage="Code is not correct!" />
						</div>
					</div>
					<div class="row 50%">
						<div class="12u">
							<asp:TextBox runat="server" ID="VisitorComments" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" placeholder="Message" />
						</div>
					</div>
					<div class="row 50%">
						<div class="12u">
							<asp:Button ID="SubmitCommentButton" runat="server" Text="Send Message" ValidationGroup="ContentCommentForm" />
							<input type="reset" value="Clear form" />
						</div>
					</div>
					<script type="text/javascript">
						__carrotware_PageValidate();
					</script>
				</CommentEntryTemplate>
			</carrot:ContentCommentForm>
			<%--<carrot:ContentCommentForm runat="server" ID="commentFrm">
				<CommentEntryTemplate>
					<carrot:jsHelperLib runat="server" ID="jsHelperLib1" />
					<div>
						<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
					</div>
					<div class="input-form">
						<p>
							<asp:Label ID="Label1" runat="server" CssClass="comment-label" AssociatedControlID="CommenterName">name: &nbsp;
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterName"
									ErrorMessage="*" />
							</asp:Label>
							<asp:TextBox runat="server" ID="CommenterName" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
							<br />
							<asp:Label ID="Label2" runat="server" CssClass="comment-label" AssociatedControlID="CommenterEmail">email: &nbsp;
								<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterEmail"
									ErrorMessage="*" />
							</asp:Label>
							<asp:TextBox runat="server" ID="CommenterEmail" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
							<br />
							<asp:Label ID="Label3" runat="server" CssClass="comment-label">
								website: &nbsp;&nbsp;
							</asp:Label>
							<asp:TextBox runat="server" ID="CommenterURL" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
							<br />
							<asp:Label ID="Label4" runat="server" CssClass="comment-label" AssociatedControlID="VisitorComments">comment:
								<asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="VisitorComments" ClientValidationFunction="__carrotware_ValidateLongText"
									EnableClientScript="true" ErrorMessage="**" />
							</asp:Label>
							<br />
							<asp:TextBox runat="server" ID="VisitorComments" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" />
							<div>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="ContentCommentCaptcha"
									ErrorMessage="**" />
								<carrot:Captcha runat="server" ID="ContentCommentCaptcha" ValidationGroup="ContentCommentForm" CaptchaIsValidStyle-Style="clear: both; color: green;"
									CaptchaIsNotValidStyle-Style="clear: both; color: red;" CaptchaImageBoxStyle-Style="clear: both;" CaptchaInstructionStyle-Style="clear: both;"
									CaptchaTextStyle-Style="clear: both;" IsNotValidMessage="Code is not correct!" />
							</div>
							<div>
								<asp:Button ID="SubmitCommentButton" CssClass="button" runat="server" Text="Submit Comment" ValidationGroup="ContentCommentForm" />
							</div>
							<script type="text/javascript">
								__carrotware_PageValidate();
							</script>
						</p>
					</div>
				</CommentEntryTemplate>
			</carrot:ContentCommentForm>--%>
		</div>
	</div>
	<!-- Content -->
</asp:Content>