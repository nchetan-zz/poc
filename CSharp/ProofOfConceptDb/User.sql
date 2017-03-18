CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL, 
    [UserName] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY ([Id]), 
    CONSTRAINT [UQ_User_Column] UNIQUE ([UserName]),
)
