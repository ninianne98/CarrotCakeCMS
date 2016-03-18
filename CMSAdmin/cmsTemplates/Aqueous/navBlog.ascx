<%@ Control Language="C#" %>
<section class="sidebar-cal">
	<carrot:PostCalendar runat="server" ID="calendar" CssClass="calendar" />
</section>
<section class="sidebar">
	<carrot:SiteMetaWordList ID="SiteMetaWordList1" runat="server" HeadWrapTag="H2" ContentType="DateMonth" MetaDataTitle="Dates" ShowUseCount="true"
		CssClass="style1" TakeTop="6" />
</section>
<section class="sidebar">
	<carrot:SiteMetaWordList ID="SiteMetaWordList2" runat="server" HeadWrapTag="H2" ContentType="Category" MetaDataTitle="Categories" ShowUseCount="true"
		CssClass="style1" TakeTop="10" />
</section>
<section class="sidebar">
	<carrot:SiteMetaWordList ID="SiteMetaWordList3" runat="server" HeadWrapTag="H2" ContentType="Tag" MetaDataTitle="Tags" ShowUseCount="true" CssClass="style1"
		TakeTop="10" />
</section>