CREATE TABLE [dbo].ShortUrl (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ShortnedUrl] NVARCHAR(50) NOT NULL,
    [OriginalUrl] NVARCHAR(2000) NOT NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_UrlShortner] PRIMARY KEY ([Id])
);
