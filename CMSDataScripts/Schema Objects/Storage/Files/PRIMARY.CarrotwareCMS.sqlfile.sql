ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [CarrotwareCMS], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

