<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Configuration" %>
<script RunAt="server">
	
	protected void Application_Start(object sender, EventArgs e) {
		Carrotware.CMS.Core.VirtualDirectory.RegisterRoutes(true);
	}

		
</script>
