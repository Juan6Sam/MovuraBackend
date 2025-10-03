
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'is_temp_password' AND Object_ID = Object_ID(N'dbo.Usuarios'))
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [is_temp_password] BIT NOT NULL CONSTRAINT [DF_Usuarios_is_temp_password] DEFAULT 0;
    PRINT 'Column [is_temp_password] added to table [dbo].[Usuarios].';
END
ELSE
BEGIN
    PRINT 'Column [is_temp_password] already exists in table [dbo].[Usuarios].';
END
