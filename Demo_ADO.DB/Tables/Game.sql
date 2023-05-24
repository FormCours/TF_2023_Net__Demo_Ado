CREATE TABLE [dbo].[Game]
(
	[Id_Game] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[Resume] NVARCHAR(4000) NULL,
	[Release_Date] DATE NULL,
	[Is_Active] BIT DEFAULT 1,
	[Price] DECIMAL(15,2) NULL,
	[Id_Publisher] INT NOT NULL, 

    CONSTRAINT PK_Game PRIMARY KEY([Id_Game]),
	CONSTRAINT FK_Game__Publisher FOREIGN KEY ([Id_Publisher]) REFERENCES [Publisher]([Id_Publisher]) 
)

GO

CREATE TRIGGER [dbo].[Trigger_Game_Delete]
    ON [dbo].[Game]
    INSTEAD OF DELETE
    AS
    BEGIN
        SET NoCount ON
		DECLARE @id INTEGER;
		DECLARE CR_deleted CURSOR FOR SELECT [Id_Game] FROM [deleted];

		OPEN CR_deleted;
		FETCH CR_deleted INTO @id;
		WHILE(@@FETCH_STATUS = 0)
		BEGIN
			UPDATE [Game]
			SET
				[Is_Active] = 0
			WHERE [Id_Game] = @id
			FETCH CR_deleted INTO @id;
		END
		CLOSE CR_deleted;
		DEALLOCATE CR_deleted;
    END