CREATE PROCEDURE [dbo].[SP_Genre_Delete]
	@id int
AS
	DELETE FROM [Genre]
	WHERE [Id_Genre] = @id
