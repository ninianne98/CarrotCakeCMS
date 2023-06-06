# CarrotCakeCMS (WebForms)
Source code for CarrotCakeCMS (WebForms), ASP.Net 3.5 SP1

[SITE_CT]: http://www.carrotware.com/contact?from=github
[REPO_SF]: http://sourceforge.net/projects/carrotcakecms/
[REPO_GH]: https://github.com/ninianne98/CarrotCakeCMS/

[DOC_PDF]: http://www.carrotware.com/fileassets/CarrotCakeCMSDevNotes.pdf?from=github
[DOC]: http://www.carrotware.com/carrotcake-download?from=github "CarrotCakeCMS User Documentation"
[TMPLT]: http://www.carrotware.com/carrotcake-templates?from=github
[IDE]: https://visualstudio.microsoft.com/
[VWDISO2013]: https://go.microsoft.com/fwlink/?LinkId=532501&type=ISO&clcid=0x409
[CEISO2013]: https://go.microsoft.com/fwlink/?LinkId=532496&type=ISO&clcid=0x409
[CEISO2015]: http://download.microsoft.com/download/b/e/d/bedddfc4-55f4-4748-90a8-ffe38a40e89f/vs2015.3.com_enu.iso
[SQL]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
[SSMS]: https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms

Welcome to the GitHub project for CarrotCake CMS, an open source c# project. CarrotCake is a [template-based][TMPLT] ASP.Net CMS (content management system) built with C#, SQL server, jQueryUI, and TinyMCE. This content management system supports multi-tenant webroots with shared databases and works well in medium trust. 

## If you have found this tool useful please [contact us][SITE_CT].

Source code and [documentation][DOC_PDF] is available on [GitHub][REPO_GH] and [SourceForge][REPO_SF]. Documentation and assemblies can be found [here][DOC].

Some features include: blogging engine, configurable date based blog post URLs, blog post content association with categories and tags, assignment/customization of category and tag URL patterns, simple content feedback collection and review, blog post pagination/indexes (with templating support), designation of default listing blog page (required to make search, category links, or tag links function), URL date formatting patterns, RSS feed support for posts and pages, import and export of site content, and import of content from WordPress XML export files.

Other features also include date based release and retirement of content - allowing you to queue up content to appear or disappear from your site on a pre-arranged schedule, site time-zone designation, ability to rename the administration folder, and site search. Supports the use of master pages to provide re-use when designing site templates.

---

## CarrotCakeCMS Developer Quick Start Guide

Copyright (c) 2011, 2023 Samantha Copeland
Licensed under the MIT or GPL v3 License

CarrotCakeCMS is maintained by Samantha Copeland

### Install Development Tools

1. **[Visual Studio Community/Express/Pro/Enterprise][IDE]** ([ISO VWD 2013][VWDISO2013], [ISO CE 2013][CEISO2013], or [ISO CE 2015][CEISO2015]) Professional (or higher) editions OK (make sure to target the 3.5 framework).  Typically being developed on VS 2015 Enterprise or VS 2019 Express.  Even VS 2012 is OK, the database project won't load, but that's OK because it's just there to maintain a schema history, it is not a required of part of the build
1. **[SQL Server Express 2008 (or higher/later)][SQL]** - currently vetted on 2008, 2012R2, and 2016 Express.
1. **[SQL Server Management Studio (SSMS)][SSMS]** - required for managing the database

### Get the Source Code

1. Go to the repository ([GitHub][REPO_GH] or [SourceForge][REPO_SF]) in a browser

1. Download either a GIT or ZIP archive or connect using either a GIT or SVN client

### Open the Project

1. Start **Visual Studio**

1. Open **CarrotwareCMS.sln** solution in the root of the repository

	Note: If your file extensions are hidden, you will not see the ".sln"
	Other SLN files are demo widgets for how to wire in custom code/extensions

1. Edit **Web.config** under **CMSAdmin** root directory (this corresponds to the **CMSAdmin** project)

	- In the connectionStrings section, configure the CarrotwareCMSConnectionString value to point to your server and the name of your database.
		Note: the credentials require database owner/dbo level as it will create the database artifacts for you.
	- In the mailSettings, configure the pickupDirectoryLocation to a directory on your development machine (for testing purposes).

1. Right-click on **CMSAdmin** and select **Set as StartUp Project**

1. Right-click on **CMSAdmin** and select **Rebuild**. The project should compile successfully

	There may be some warnings, you can ignore them

1. The SQL Server database should be running and an empty database matching the one specified in the connection string. If you are running the code a second or later time, it will auto update if there are schema changes (see dbo note above).  Do not share a database between the MVC and WebForms editions.

1. if the database is empty or has pending database changes, you will be greeted with a maintenance screen, follow the link provided.

1. The first time you start up the website, it will create the required artifacts in the database (tables/views/sprocs etc.)

1. Click the **Play** button (or hit F5) in the main toolbar to launch CarrotCakeCMS

1. When you run the website with an empty user database, you will be prompted to create the first user

1. Once you have created a user, you can go to the login screen, enter the credentials

1. After successfully logging in, you can create and manage your new website

### Using CarrotCakeCMS

For additional information on how to use CarrotCakeCMS, please see the **[CarrotCakeCMS Documentation][DOC]**.
