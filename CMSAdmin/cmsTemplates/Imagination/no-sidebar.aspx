<%@ Page Title="" Language="C#" MasterPageFile="imagination.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {

		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="row">
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
	</div>
</asp:Content>