<%@ Page Title="" Language="C#" MasterPageFile="imagination.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {

		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div class="row">
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
		</div>
		<div class="3u">
			<section class="sidebar">
				<carrot:ChildNavigation MetaDataTitle="Child Pages" HeadWrapTag="H2" runat="server" ID="ChildNavigation1" CssClass="default" />
			</section>
			<section class="sidebar">
				<carrot:SecondLevelNavigation MetaDataTitle="Section Pages" HeadWrapTag="H2" runat="server" ID="SecondLevelNavigation1" CssClass="default" />
			</section>
		</div>
	</div>
</asp:Content>
