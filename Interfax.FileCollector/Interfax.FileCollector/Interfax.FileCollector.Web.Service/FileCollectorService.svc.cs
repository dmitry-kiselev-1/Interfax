using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Configuration;
using Interfax.FileCollector.DataAccessLayer.ORM;
using Interfax.FileCollector.Web.Service.DataContracts;

namespace Interfax.FileCollector.Web.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class FileStorageService : IFileCollectorService
    {
        public Guid? UploadFile(string name, DateTimeOffset? createDate, byte[] data)
        {
            int maxUploadFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings.Get("MaxUploadFileSize"));

            if (data.Length <= maxUploadFileSize)
            {
                using (var db = new FileCollectorEntities())
                {
                    return db.spFileInsert(name, createDate, data).FirstOrDefault();
                }
            }
            else
            {
                throw new FaultException("Размер загружаемого файла превышает указанный в конфигурационном файле лимит");
            }
        }

        public List<FileStorage> GetFiles()
        {
            using (var db = new FileCollectorEntities())
            {
                var result = db.spFileSelect(null, null).Select(r => new FileStorage()
                                                                     {
                                                                         Id = r.Id,
                                                                         Name = r.Name,
                                                                         Path = r.Path,
                                                                         Type = r.Type,
                                                                         Size = r.Size,
                                                                         CreateDate = r.CreateDate,
                                                                         LoadDate = r.LoadDate,
                                                                         Data = null
                                                                     }).ToList();
                return result;
            }
        }

        public List<FileStorage> DownloadFile(Guid Id)
        {
            using (var db = new FileCollectorEntities())
            {
                var result = db.spFileDownload(Id).Select(r => new FileStorage()
                                                               {
                                                                   Id = r.Id,
                                                                   Name = r.Name,
                                                                   Path = r.Path,
                                                                   Type = r.Type,
                                                                   Size = r.Size,
                                                                   CreateDate = r.CreateDate,
                                                                   LoadDate = r.LoadDate,
                                                                   Data = r.Data
                                                               }).ToList();
                return result;
            }
        }
    }
}
