using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.UI;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WebControls")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Carrotware")]
[assembly: AssemblyProduct("WebControls")]
[assembly: AssemblyCopyright("Copyright © Carrotware 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a7dbb655-e885-4a5e-b319-7910bce3d13c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("4.4.*")]
//  [assembly: AssemblyFileVersion("4.3.0.0")]

[assembly: TagPrefix("Calendar", "carrot")]
[assembly: TagPrefix("Captcha", "carrot")]
[assembly: TagPrefix("CarrotGridView", "carrot")]
[assembly: TagPrefix("CarrotHeaderSortTemplateField", "carrot")]
[assembly: TagPrefix("ImageSizer", "carrot")]
[assembly: TagPrefix("ListItemPlaceHolder", "carrot")]
[assembly: TagPrefix("ListItemRepeater", "carrot")]
[assembly: TagPrefix("ListItemWrapper", "carrot")]
[assembly: TagPrefix("ListItemWrapperForPager", "carrot")]
[assembly: TagPrefix("NavLinkForPagerTemplate", "carrot")]
[assembly: TagPrefix("NavLinkForTemplate", "carrot")]
[assembly: TagPrefix("NavPageNumberDisplay", "carrot")]
[assembly: TagPrefix("jquery", "carrot")]
[assembly: TagPrefix("jquerybasic", "carrot")]
[assembly: TagPrefix("jqueryui", "carrot")]
[assembly: TagPrefix("jsHelperLib", "carrot")]

[assembly: WebResource("Carrotware.Web.UI.Controls.ucFancyPager.ascx", "text/plain")]
[assembly: WebResource("Carrotware.Web.UI.Controls.ucSimplePager1.ascx", "text/plain")]
[assembly: WebResource("Carrotware.Web.UI.Controls.ucSimplePager2.ascx", "text/plain")]
[assembly: WebResource("Carrotware.Web.UI.Controls.CarrotGridView.accept.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.CarrotGridView.attach.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.CarrotGridView.cancel.png", "image/png")]

[assembly: WebResource("Carrotware.Web.UI.Controls.jsHelperLibrary.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.Web.UI.Components.carrotHelp.js", "text/javascript")]

[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-1-10-2.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-1-11-3.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-1-12-4.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-1-6-4.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-1-7-2.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-1-8-3.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-1-9-1.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-2-2-4.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-3-6-4.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-mig-1-2-1.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-mig-1-3-0.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-mig-1-4-1.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-unobtrusive-ajax.min.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jquery-validate.unobtrusive.min.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-10-2.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-11-4.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-12-1.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-13-2.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-7-3.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-8-24.js", "text/javascript")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquery.jqueryui-1-9-2.js", "text/javascript")]

[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-black.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-blue.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-green.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-lightgreen.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-purple.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.jquery-ui-silver.css", "text/css", PerformSubstitution = true)]

[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-bg_glass_100_333333_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-bg_glass_100_aaaaaa_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-bg_glass_100_efefef_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-bg_glass_50_666666_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-bg_glass_80_111111_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-bg_hexagon_90_eeeeee_12x10.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-icons_111111_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-icons_2e83ff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-icons_cd0a0a_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-icons_eeeeee_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.black.ui-icons_ffffff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_diagonals-thick_90_eeeeee_40x40.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_glass_100_e4f1fb_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_glass_50_3baae3_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_glass_80_d7ebf9_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_highlight-hard_100_f2f5f7_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_highlight-hard_70_000000_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_highlight-soft_100_deedf7_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-bg_highlight-soft_25_ffef8f_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-icons_2694e8_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-icons_2e83ff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-icons_3d80b3_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-icons_72a7cf_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.blue.ui-icons_ffffff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_glass_55_fcf0ba_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_gloss-wave_100_ece8da_500x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_highlight-hard_100_f5f3e5_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_highlight-hard_100_fafaf4_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_highlight-hard_15_459e00_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_highlight-hard_95_cccccc_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_highlight-soft_25_67b021_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_highlight-soft_95_ffedad_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-bg_inset-soft_15_2b2922_1x100.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-icons_808080_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-icons_847e71_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-icons_8dc262_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-icons_cd0a0a_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-icons_eeeeee_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.green.ui-icons_ffffff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-bg_glass_100_b7d7c4_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-bg_glass_100_dff6e8_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-bg_glass_100_f2f5f7_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-bg_glass_50_468b5e_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-bg_glass_80_b7d7c4_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-bg_hexagon_90_eeeeee_12x10.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-icons_2e83ff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-icons_468b5e_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-icons_cd0a0a_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.lightgreen.ui-icons_ffffff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-bg_glass_100_d3c4ec_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-bg_glass_100_e3daf2_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-bg_glass_50_774499_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-bg_glass_80_d1c2ea_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-bg_hexagon_90_eeeeee_12x10.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-icons_2e83ff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-icons_774499_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-icons_cd0a0a_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.purple.ui-icons_ffffff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-bg_glass_100_686868_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-bg_glass_100_d0d0d0_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-bg_glass_100_d6d6d6_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-bg_glass_100_d9d9d9_1x400.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-bg_hexagon_80_dddddd_12x10.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-icons_111111_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-icons_222222_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-icons_2e83ff_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-icons_cd0a0a_256x240.png", "image/png")]
[assembly: WebResource("Carrotware.Web.UI.Controls.jquerybasic.silver.ui-icons_ffffff_256x240.png", "image/png")]