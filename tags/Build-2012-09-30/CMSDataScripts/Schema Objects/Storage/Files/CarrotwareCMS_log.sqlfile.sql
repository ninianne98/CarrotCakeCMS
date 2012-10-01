ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [CarrotwareCMS_log], FILENAME = '$(DefaultLogPath)$(DatabaseName)_1.LDF', MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

