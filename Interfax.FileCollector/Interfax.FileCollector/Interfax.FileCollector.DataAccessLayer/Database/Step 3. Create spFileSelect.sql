-- =============================================
-- Author:		Киселёв Дмитрий
-- Create date: 21.09.2013
-- Description:	Возвращает список содержимого FileTable-директории FileStorage
-- позволяет выполнить фильтрацию по идентификатору и имени файла, опционально
-- =============================================
ALTER PROCEDURE [dbo].[spFileSelect]
	@Id uniqueidentifier = NULL,
	@Name nvarchar(255) = NULL
AS
BEGIN
	SELECT [stream_id]								AS Id
		  ,[name]									AS Name
		  ,FileTableRootPath() +
			[file_stream].GetFileNamespacePath()	AS Path
		  --,[path_locator]
		  --,[parent_path_locator]
		  ,[file_type]								AS [Type]
		  ,[cached_file_size]						AS Size
		  ,[last_write_time]						AS CreateDate	-- Дата создания
		  ,[creation_time]							AS LoadDate		-- Дата загрузки
		  --,[last_access_time]
		  --,[is_directory]
		  --,[is_offline]
		  --,[is_hidden]
		  --,[is_readonly]
		  --,[is_archive]
		  --,[is_system]
		  --,[is_temporary]
		  --,[file_stream]							AS Data
	FROM [dbo].[FileStorage]
		WHERE 
			[stream_id]			= ISNULL(@Id,			[stream_id])
		AND [name]				= ISNULL(@Name,			[name])
END

--spFileSelect

