<%@ Page Title="" Language="C#" MasterPageFile="PlainTemplate.master" AutoEventWireup="true" Inherits="Carrotware.CMS.UI.Base.GenericPageFromMaster" %>

<%@ MasterType VirtualPath="PlainTemplate.master" %>

<script runat="server">
	protected override void OnInit(EventArgs e) {
		base.OnInit(e);

		Master.IsBW = false;
	}
</script>
