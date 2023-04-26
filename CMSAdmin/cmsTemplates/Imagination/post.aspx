<%@ Page Title="" Language="C#" MasterPageFile="imagination.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {

		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="row">
		<div class="3u">
			<section class="sidebar">
				<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" HeadWrapTag="H2" ContentType="DateMonth" MetaDataTitle="Dates" ShowUseCount="true" TakeTop="12"
					CssClass="default" />
			</section>
			<section class="sidebar">
				<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" HeadWrapTag="H2" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true" CssClass="default" />
			</section>
			<section class="sidebar">
				<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" HeadWrapTag="H2" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" CssClass="default" />
			</section>
		</div>
		<div class="9u skel-cell-important">
			<section>
				<header>
					<h2>
						<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="PageHead" /></h2>
				</header>
				<article>
					<carrot:WidgetContainer ID="phCenterTop" runat="server" />
					<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
					<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
				</article>
			</section>
			<div>
				<carrot:ContentCommentForm runat="server" ID="commentFrm">
					<CommentEntryTemplate>
						<carrot:jsHelperLib runat="server" ID="jsHelperLib1" />
						<div>
							<asp:Label ID="ContentCommentFormMsg" runat="server" Text="" />
						</div>
						<div class="input-form">
							<p class="padding10">
								<label class="comment-label">
									name:
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterName" ErrorMessage="*" />
								</label>
								<asp:TextBox runat="server" ID="CommenterName" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
								<label class="comment-label">
									email:
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="CommenterEmail" ErrorMessage="*" />
								</label>
								<asp:TextBox runat="server" ID="CommenterEmail" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
								<label class="comment-label">
									website:
								</label>
								<asp:TextBox runat="server" ID="CommenterURL" Columns="30" MaxLength="100" ValidationGroup="ContentCommentForm" />
								<label class="comment-label">
									comment:
									<asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="VisitorComments" ClientValidationFunction="__carrotware_ValidateLongText"
										EnableClientScript="true" ErrorMessage="**" />
								</label>
								<asp:TextBox runat="server" ID="VisitorComments" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="1024" />
								<div class="padding10">
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="ContentCommentForm" ControlToValidate="ContentCommentCaptcha"
										ErrorMessage="**" />
									<carrot:Captcha runat="server" ID="ContentCommentCaptcha" ValidationGroup="ContentCommentForm" CaptchaIsValidStyle-Style="clear: both; color: green;" CaptchaIsNotValidStyle-Style="clear: both; color: red;"
										CaptchaImageBoxStyle-Style="clear: both;" CaptchaInstructionStyle-Style="clear: both;" CaptchaTextStyle-Style="clear: both;" IsNotValidMessage="Code is not correct!" />
								</div>
								<div class="padding10">
									<asp:Button ID="SubmitCommentButton" CssClass="button padding10" runat="server" Text="Submit Comment" ValidationGroup="ContentCommentForm" />
								</div>
								<script type="text/javascript">
									__carrotware_PageValidate();
								</script>
							</p>
						</div>
					</CommentEntryTemplate>
				</carrot:ContentCommentForm>
			</div>
		</div>
	</div>
</asp:Content>
