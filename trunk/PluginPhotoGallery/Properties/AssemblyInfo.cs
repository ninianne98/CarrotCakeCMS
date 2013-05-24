using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.UI;


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PluginPhotoGallery")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Carrotware")]
[assembly: AssemblyProduct("PluginPhotoGallery")]
[assembly: AssemblyCopyright("Copyright © Carrotware 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("C0649337-A7C7-45c1-929E-909CF4D06392")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.*")]
//  [assembly: AssemblyFileVersion("1.0.0.0")]


[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.scripts.CreateGallery.sql", "text/plain")]


[assembly: TagPrefix("FancyBox", "carrot")]
[assembly: TagPrefix("PrettyPhoto", "carrot")]


[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.blank.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancybox-x.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancybox-y.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancybox.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_close.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_loading.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_nav_left.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_nav_right.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_e.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_n.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_ne.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_nw.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_s.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_se.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_sw.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_shadow_w.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_title_left.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_title_main.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_title_over.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.images.fancy_title_right.png", "image/png")]

[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.easing-p.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.fancybox.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.fancybox-p.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.fancybox.mousewheel-p.js", "text/javascript", PerformSubstitution = true)]



[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_rounded.loader.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_rounded.btnNext.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_rounded.btnPrevious.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_rounded.contentPattern.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_rounded.default_thumbnail.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_rounded.sprite.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_square.loader.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_square.btnNext.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_square.btnPrevious.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_square.contentPattern.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_square.default_thumbnail.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.dark_square.sprite.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.default.sprite.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.default.sprite_x.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.default.default_thumb.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.default.loader.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.default.sprite_y.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.default.sprite_next.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.default.sprite_prev.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.contentPatternLeft.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.loader.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.btnNext.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.btnPrevious.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.contentPatternBottom.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.sprite.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.contentPatternRight.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.contentPatternTop.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.facebook.default_thumbnail.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_rounded.loader.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_rounded.btnNext.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_rounded.btnPrevious.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_rounded.default_thumbnail.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_rounded.sprite.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_square.loader.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_square.btnNext.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_square.btnPrevious.png", "image/png")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_square.default_thumbnail.gif", "image/gif")]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.images.light_square.sprite.png", "image/png")]

[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.prettyPhoto.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("Carrotware.CMS.UI.Plugins.PhotoGallery.prettyphoto.prettyPhoto.css", "text/css", PerformSubstitution = true)]
