CREATE PROCEDURE [dbo].[SP_Genre_SelectAll]
AS
	SELECT 
		[Id_Genre],
		[Name],
		[Description]
	FROM [Genre]
