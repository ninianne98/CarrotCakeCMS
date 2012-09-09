var tinyBrowseHeight = 500;
var tinyBrowseWidth = 700;


function TinyMCEParamInit(winWidth, winHeight) {

	tinyBrowseHeight = parseInt(winHeight);
	tinyBrowseWidth = parseInt(winWidth);

	tinyMCE.init({

		//		mode : "textareas",
		//		theme : "advanced",
		//		theme_advanced_toolbar_location : "top",
		//		theme_advanced_toolbar_align : "left",
		//		document_base_url : "http://www.site.com/path1/"

		mode: "textareas",
		theme: "advanced",
		editor_selector: "mceEditor",
		theme_advanced_source_editor_width: winWidth,
		theme_advanced_source_editor_height: winHeight,
		skin: "o2k7",
		skin_variant: "silver",
		plugins: "advimage,advlink,advlist,media,inlinepopups",
		file_browser_callback: "cmsFileBrowserCallback",
		theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,forecolor,|,outdent,indent,blockquote,|,bullist,numlist,|,link,unlink,anchor,image,media,|,fileupbtn,cleanup,code,help",
		theme_advanced_buttons2: "",
		theme_advanced_buttons3: "",
		theme_advanced_toolbar_location: "top",
		theme_advanced_toolbar_align: "left",
		theme_advanced_statusbar_location: "bottom",
		relative_urls: false,
		remove_script_host: true,
		content_css: "/Manage/Includes/richedit.css",

		// Add a custom button
		setup: function (ed) {
			ed.addButton('fileupbtn', {
				title: 'FileUpload',
				image: '/Manage/tiny_mce/insertfile.gif',
				onclick: function () {
					ed.focus();
					var x = cmsFileBrowserCallback(ed, '', '', this);
				}
			});
		}

	});
}


// http://wiki.moxiecode.com/index.php/TinyMCE:Custom_filebrowser
function cmsFileBrowserCallback(field_name, url, type, win) {
	var sURL = "/Manage/FileBrowser.aspx?useTiny=1&fldrpath=/";
	setTimeout("tinyResetFileBrowserOpenStatus();", 500);

	// block multiple file browser windows
	if (!tinyMCE.selectedInstance.fileBrowserAlreadyOpen) {
		tinyMCE.selectedInstance.fileBrowserAlreadyOpen = true; // but now it will be

		tinyMCE.activeEditor.windowManager.open({
			file: sURL,
			title: 'File Browser',
			width: tinyBrowseWidth,
			height: tinyBrowseHeight,
			resizable: "no",
			scrollbars: "yes",
			status: "yes",
			inline: "yes",
			close_previous: "yes"
		}, {
			window: win,
			input: field_name
		});
	}

	setTimeout("tinyResetFileBrowserOpenStatus();", 1000);

	return false;
}


function tinyResetFileBrowserOpenStatus() {
	tinyMCE.selectedInstance.fileBrowserAlreadyOpen = false;
}


function cmsToggleTinyMCE(id) {
	if (!tinyMCE.get(id))
		tinyMCE.execCommand('mceAddControl', false, id);
	else
		tinyMCE.execCommand('mceRemoveControl', false, id);
}


var fldName = '';
var winBrowse = null;
function cmsFileBrowserOpen(fldN) {
	fldN = '#' + fldN;
	var fld = $(fldN);
	fldName = fld.attr('id');

	if (winBrowse != null) {
		winBrowse.close();
	}
	//winBrowse = window.open('/Manage/FileBrowser.aspx?useTiny=0&fldrpath=/', '_winBrowse', 'resizable=yes,location=no,menubar=no,scrollbars=yes,status=yes,toolbar=no,fullscreen=no,dependent=yes,width=650,height=500,left=50,top=50');

	ShowWindowNoRefresh('/Manage/FileBrowser.aspx?useTiny=0&fldrpath=/');

	return false;
}


function cmsSetFileName(v) {
	var fldN = '#' + fldName;
	var fld = $(fldN);
	fld.val(v);

	winBrowse.close();
	winBrowse = null;
}