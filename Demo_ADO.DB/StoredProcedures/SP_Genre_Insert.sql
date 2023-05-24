CREATE PROCEDURE [dbo].[SP_Genre_Insert]
	@id INTEGER OUTPUT,
	@name NVARCHAR(50),
	@description NVARCHAR(500) = NULL
AS
	INSERT INTO [Genre]([Name], [Description])
	VALUES (@name, @description);
	SELECT @id = @@IDENTITY FROM [Genre];
