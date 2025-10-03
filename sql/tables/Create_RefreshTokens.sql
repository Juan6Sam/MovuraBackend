
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RefreshTokens](
        [id_refresh_token] [int] IDENTITY(1,1) NOT NULL,
        [id_usuario] [int] NOT NULL,
        [token] [nvarchar](512) NOT NULL,
        [expires_at] [datetime2](7) NOT NULL,
        [created_at] [datetime2](7) NOT NULL,
        [created_by_ip] [nvarchar](50) NULL,
        [revoked_at] [datetime2](7) NULL,
        [revoked_by_ip] [nvarchar](50) NULL,
        [replaced_by_token] [nvarchar](512) NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([id_refresh_token] ASC),
        CONSTRAINT [FK_RefreshTokens_Usuarios] FOREIGN KEY([id_usuario]) REFERENCES [dbo].[Usuarios] ([id_usuario])
    );
    PRINT 'Table [dbo].[RefreshTokens] created.';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[RefreshTokens] already exists.';
END
