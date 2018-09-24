USE [FileCollector]
	GO
	CREATE TABLE FileStorage AS FILETABLE
	  WITH
	  (
		FILETABLE_DIRECTORY = 'FileStorage',
		FILETABLE_COLLATE_FILENAME = database_default
	  )
	GO