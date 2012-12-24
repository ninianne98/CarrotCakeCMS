<%@ Control Language="C#" %>
<div id="divComment" runat="server" class="comment-detail">
	<p>
		<b>
			<carrot:ListItemCommentText runat="server" ID="ListItemCommentText1" DataField="CommenterName" />
		</b>
		<carrot:ListItemCommentText runat="server" ID="ListItemCommentText2" DataField="CreateDate" FieldFormat="{0:d}" />
		<br />
		<carrot:ListItemCommentText runat="server" ID="ListItemCommentText3" DataField="PostCommentText" />
	</p>
</div>
