CREATE PROCEDURE [dbo].[SP_Genre_Select]
	@id int
AS
	SELECT 
		[Id_Genre],
		[Name],
		[Description]
	FROM [Genre]
	WHERE [Id_Genre] = @id
