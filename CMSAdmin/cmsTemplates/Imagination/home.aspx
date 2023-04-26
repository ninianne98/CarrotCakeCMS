<%@ Page Title="" Language="C#" MasterPageFile="imagination.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			//setBodyCSS('homepage');
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BannerBodyContentPlaceHolder" runat="server">
	<section>
		<span class="fa fa-cubes"></span>
		<header>
			<h2>
				<carrot:ContentPageProperty runat="server" ID="ContentPageProperty1" DataField="PageHead" /></h2>
		</header>
		<carrot:WidgetContainer ID="phLeftTop" runat="server" />
		<carrot:ContentContainer EnableViewState="false" ID="BodyLeft" runat="server" />
		<carrot:WidgetContainer ID="phLeftBottom" runat="server" />
	</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraBodyContentPlaceHolder" runat="server">
	<!-- Extra -->
	<div id="extra">
		<div class="container">
			<div class="row">
				<carrot:PagedDataSummary ID="PagedDataSummary1" runat="server" ContentType="Blog" PageSize="4" ShowPager="false" IgnoreSitePath="true" HideSpanWrapper="true">
					<ContentHeaderTemplate>
					</ContentHeaderTemplate>
					<ContentTemplate>
						<section class="3u">
							<header>
								<h2>
									<carrot:NavLinkForTemplate ID="NavLinkForTemplate2" runat="server" />
									<br />
									<carrot:ListItemNavText runat="server" ID="ListItemNavText3" DataField="GoLiveDate" FieldFormat="{0:M.d.yyyy}" />
								</h2>
							</header>
							<%--<span class="fa fa-magic"></span>--%>
							<p>
								<carrot:ListItemNavText runat="server" ID="ListItemNavText4" DataField="PageTextPlainSummary" />
							</p>
						</section>
					</ContentTemplate>
					<ContentFooterTemplate>
					</ContentFooterTemplate>
				</carrot:PagedDataSummary>
			</div>
		</div>
	</div>
	<!-- /Extra -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<section>
		<header>
			<h2>
				<carrot:ContentPageProperty runat="server" ID="ContentPageProperty3" DataField="PageHead" /></h2>
		</header>
	</section>
	<div class="row">
		<section class="12u">
			<carrot:WidgetContainer ID="phCenterTop" runat="server" />
			<carrot:ContentContainer EnableViewState="false" ID="BodyCenter" runat="server" />
			<carrot:WidgetContainer ID="phCenterBottom" runat="server" />
		</section>
	</div>
	<div class="divider">
	</div>
	<%--<a href="#" class="button medium">View Full Details</a>--%>
</asp:Content>
