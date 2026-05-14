<%@ Application Language="C#" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Web.Configuration" %>

<script RunAt="server">

	protected void Application_Start(object sender, EventArgs e) {
		Carrotware.CMS.Core.VirtualDirectory.RegisterRoutes(true);
	}

</script>
