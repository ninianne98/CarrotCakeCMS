<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Carrotware.CMS.Core" %>

<script runat="server">
	protected CmsSkin.SkinOption _theme = CmsSkin.SkinOption.Classic;

	protected override void OnInit(EventArgs e) {
		base.OnInit(e);

		try {
			var config = CarrotCakeConfig.GetConfig();
			string skin = config.MainConfig.SiteSkin;

			var actualSkin = CmsSkin.SkinOption.Classic;
			actualSkin = (CmsSkin.SkinOption)Enum.Parse(typeof(CmsSkin.SkinOption), skin, true);

			_theme = actualSkin;
		} catch { }

		siteSkin.SelectedColor = _theme;
	}

</script>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
	<meta name="robots" content="noindex,follow,noarchive" />
	<meta http-equiv="Pragma" content="no-cache" />
	<title>CarrotCake CMS: Under Construction</title>
	<carrot:CmsSkin runat="server" ID="siteSkin" WindowMode="Logon" SelectedColor="Classic" />
</head>
<body>
	<form id="form1" runat="server">
		<div id="banner">
			<h2></h2>
			<h2>&nbsp;</h2>
			<h2 class="banner">UNDER CONSTRUCTION</h2>
			<h2>&nbsp;</h2>
			<h2>To manage your site, visit the <a href="<%= SiteData.AdminFolderPath %>">site administration</a>.</h2>
		</div>
	</form>
</body>
</html>
