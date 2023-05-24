CREATE PROCEDURE [dbo].[SP_Genre_Update]
	@id int,
	@name NVARCHAR(50),
	@description NVARCHAR(500) = NULL
AS
	UPDATE [Genre]
	SET
		[Name] = @name,
		[Description] = @description
	WHERE [Id_Genre] = @id
