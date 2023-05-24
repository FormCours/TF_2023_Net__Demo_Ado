CREATE TYPE T_Genre AS TABLE(
	[Name] NVARCHAR(50) NOT NULL UNIQUE,
	[Description] NVARCHAR(500)
)
GO

CREATE PROCEDURE [dbo].[SP_Genre_InsertTable]
	@genres T_Genre READONLY
AS
	INSERT INTO [Genre] ([Name], [Description])
	SELECT [Name], [Description] FROM @genres
