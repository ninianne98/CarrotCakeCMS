EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'CarrotUser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'CarrotUser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'CarrotUser';

