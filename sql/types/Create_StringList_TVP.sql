
IF NOT EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'StringList')
BEGIN
    CREATE TYPE [dbo].[StringList] AS TABLE(
        [item] [nvarchar](250) NOT NULL
    );
    PRINT 'Table-Valued Parameter type [dbo].[StringList] created.';
END
ELSE
BEGIN
    PRINT 'Table-Valued Parameter type [dbo].[StringList] already exists.';
END
