Notes on what/when to install which SQL scripts


If installing/running CMS for first time run

		** CREATE 01 - whole database.sql
			-- creates the core tables for the CMS, inital seed of membership groups
		** CREATE 02 - default user.sql  
			-- you can update the username & email listed in script, note that the password is still going to be carrot123 unless you change it (PLEASE CHANGE ASAP!!!!)
			*OR* 
			-- skip this script and use visual studio to create additional users, groups are auto created for you

		** create tblCalendar.sql
			-- to use the calendar sample
		** create tblFAQ.sql
			-- to use the FAQ sample

If coming from an earlier version, run these scripts

		** ALTER 01 - add meta fields to content.sql     (from before 2012-01-25)

		** ALTER 02 - new widget structure.sql     (from before 2012-07-16)

		** ALTER 03 - add create date to root content.sql     (from before 2012-07-22)

		** ALTER 04 - move data to carrot prefix tables.sql     (from before 2012-08-12)		