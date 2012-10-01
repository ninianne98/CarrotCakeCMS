ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [CarrotwareCMS], FILENAME = '$(DefaultDataPath)$(DatabaseName).MDF', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

