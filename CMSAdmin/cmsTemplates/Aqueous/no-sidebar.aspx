<%@ Page Title="" Language="C#" MasterPageFile="aqueous.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<div id="content" class="12u skel-cell-important">
		<section>
			<header>
				<h2>
					<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="PageHead" /></h2>
			</header>
			<carrot:WidgetContainer ID="phCenterTop" runat="server" />
			<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
			<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
		</section>
	</div>
</asp:Content>