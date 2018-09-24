using System;
using System.Collections.Generic;
using System.ServiceModel;
using Interfax.FileCollector.Web.Service.DataContracts;

namespace Interfax.FileCollector.Web.Service
{
    [ServiceContract]
    public interface IFileCollectorService
    {
        /// <summary>
        /// Возвращает список всех файлов, загруженных в хранилище (без их содержимого - только список)
        /// </summary>
        [OperationContract]
        List<FileStorage> GetFiles();

        /// <summary>
        /// Добавляет файл в хранилище, загружая его на SQL-сервер и возвращет его идентификатор
        /// </summary>
        [OperationContract]
        Guid? UploadFile(string name, DateTimeOffset? createDate, byte[] data);

        /// <summary>
        /// Скачивает файл из хранилища, с SQL-сервера
        /// </summary>
        [OperationContract]
        List<FileStorage> DownloadFile(Guid Id);
    }
}
