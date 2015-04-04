Notes on what/when to install which SQL scripts

If installing/running CMS for first time run

		** CREATE 01 - whole database.sql
			-- creates the core tables for the CMS, inital seed of membership groups
		** CREATE 02 - default user.sql  
			-- you can update the username & email listed in script, note that the password is still going to be carrot123 unless you change it (PLEASE CHANGE ASAP!!!!)
			*OR* 
			-- skip this script and use visual studio to create additional users, groups are auto created for you

		** carrot_CalendarEvent.sql
			-- to use the event calendar sample
		** carrot_FaqItem.sql
			-- to use the FAQ sample
		** tblGallery.sql
			-- to use the Gallery sample

If coming from an earlier version, run these scripts

		** ALTER 01 - add meta fields to content.sql     (from before 2012-01-25)

		** ALTER 02 - new widget structure.sql     (from before 2012-07-16)

		** ALTER 03 - add create date to root content.sql     (from before 2012-07-22)

		** ALTER 04 - move data to carrot prefix tables.sql     (from before 2012-08-12)

		** ALTER 05 - create combined content view.sql     (from before 2012-09-08)

		** ALTER 06 - create blog tables and views.sql     (from before 2012-11-19)

		** ALTER 07 - create date based publishing.sql     (from before 2012-12-15)

		** ALTER 08 - create trackback queue and comment view.sql     (from before 2012-12-28)

		** ALTER 09 - parent child view and misc setting properties.sql     (from before 2013-01-12)

		** ALTER 10 - create version count and text widget tables.sql     (from before 2013-05-30)

		** ALTER 11 - update tally sproc.sql     (from before 2013-10-01)

		** ALTER 12 - update time zone and credits.sql     (from before 2014-06-01)


