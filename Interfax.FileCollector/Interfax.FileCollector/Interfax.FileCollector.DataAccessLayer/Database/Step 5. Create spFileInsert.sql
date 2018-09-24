-- =============================================
-- Author:		Киселёв Дмитрий
-- Create date: 21.09.2013
-- Description:	Добавляет файл в FileTable-директорию FileStorage
-- и возвращает его идентификатор
-- =============================================
CREATE PROCEDURE [dbo].[spFileInsert]
	 @Name			nvarchar(255)
	,@CreateDate	datetimeoffset(7)	-- Дата создания
	--,@LoadDate	datetimeoffset(7)	-- Дата загрузки (формируется автоматически)
	,@Data			varbinary(max)
AS
BEGIN

	INSERT FileStorage
	(
		 [name]
		,[last_write_time]
		--,[creation_time]
		,[file_stream]
	) 
	OUTPUT INSERTED.[stream_id] AS Id	-- возврат идентификатора добавленной записи в результирующем наборе
	VALUES 
	(
		 @Name 
		,@CreateDate 
		--,@LoadDate
		,@Data
	)

END

/*
DECLARE @Data varbinary(max) 
SET @Data = CAST('Тест' AS varbinary(max))
EXEC spFileInsert 'Файл 2', '20010101', @Data
*/

GO