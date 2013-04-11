<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoGalleryAdminImportWP.ascx.cs" Inherits="Carrotware.CMS.UI.Plugins.PhotoGallery.PhotoGalleryAdminImportWP" %>
<h2>
	Photo Gallery : Wordpress Import</h2>
<br />
<asp:Panel runat="server" ID="pnlUpload">
	<p>
		<asp:Label ID="lblWarning" runat="server" />
	</p>
	<p>
		Select a previously exported content to import:<br />
		<asp:FileUpload ID="upFile" runat="server" />
	</p>
	<p>
		You will be able to review the imported data before finalizing changes.
	</p>
	<p>
		<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
	</p>
</asp:Panel>
<asp:Panel runat="server" ID="pnlReview">
	<div>
		<table width="750">
			<tr>
				<td valign="top" width="250" align="right">
					Widget Zone Target:
				</td>
				<td valign="top" align="left">
					<asp:TextBox ID="txtPlaceholderName" runat="server" Text="phCenterBottom" />
				</td>
				<td>
					&nbsp;&nbsp;&nbsp;
				</td>
				<td valign="top" width="250" align="right">
					Attachment Folder (if downloading):
				</td>
				<td valign="top" align="left">
					<asp:DropDownList ID="ddlFolders" runat="server" DataTextField="FileName" DataValueField="FolderPath">
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td valign="top" align="right">
					Selected skin:
				</td>
				<td valign="top" align="left">
					<asp:DropDownList ID="ddlSkin" runat="server" DataTextField="Value" DataValueField="Key">
					</asp:DropDownList>
				</td>
				<td>
					&nbsp;&nbsp;&nbsp;
				</td>
				<td valign="top" align="right">
					Selected thumbnail size:
				</td>
				<td valign="top" align="left">
					<asp:DropDownList ID="ddlSize" runat="server" DataTextField="Value" DataValueField="Key">
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td valign="top" align="left" colspan="5">
					<div class="table-subblock">
						<b class="caption">Do Not Download Images:</b>&nbsp;&nbsp;
						<asp:CheckBox ID="chkFileGrab" runat="server" />
					</div>
					<div class="table-subblock">
						<b class="caption">Show Heading:</b>&nbsp;&nbsp;
						<asp:CheckBox ID="chkShowHeading" runat="server" />
					</div>
					<div class="table-subblock">
						<b class="caption">Scale Image:</b>&nbsp;&nbsp;
						<asp:CheckBox ID="chkScaleImage" runat="server" />
					</div>
				</td>
			</tr>
		</table>
		<br />
		<p>
			<asp:Literal ID="litTrust" runat="server"><b>Downloading images requires a full trust website, and this installation has not been detected as such, leaving the download option disabled. </b></asp:Literal>
			The selected directory must already exist and be write enabled (to import images, if not already present). Images may be downloaded as part of this import
			provided the file does not already exist locally, the directory can be written to, and the remote file is reachable. Please download the images manually
			if you are unable to write files or simply do not want to download the images, simply check the box below to block the downloading of the images.
		</p>
		<p>
			<asp:Button ID="btnCreate" runat="server" Text="Import Selected" OnClick="btnCreate_Click" /><br />
		</p>
		<p>
			<asp:Label ID="lblPages" runat="server" Text="Label" />
			records
		</p>
		<div id="SortableGrid">
			<carrot:CarrotGridView CssClass="datatable" DefaultSort="PostTitle ASC" ID="gvPages" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="tablehead"
				AlternatingRowStyle-CssClass="rowalt" RowStyle-CssClass="rowregular">
				<Columns>
					<asp:TemplateField ItemStyle-HorizontalAlign="Center">
						<HeaderTemplate>
							Select
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="chkSelect" runat="server" />
							<asp:HiddenField ID="hdnPostID" runat="server" Value='<%# Eval("PostID") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="PostTitle" HeaderText="Post Title" />
					<asp:BoundField DataField="PostDateUTC" HeaderText="Created On" DataFormatString="{0:d}" />
					<carrot:CarrotHeaderSortTemplateField ItemStyle-HorizontalAlign="Center" DataField="IsPublished" HeaderText="Published" AlternateTextFalse="Inactive" AlternateTextTrue="Active"
						ShowBooleanImage="true" />
				</Columns>
			</carrot:CarrotGridView>
		</div>
	</div>
</asp:Panel>
