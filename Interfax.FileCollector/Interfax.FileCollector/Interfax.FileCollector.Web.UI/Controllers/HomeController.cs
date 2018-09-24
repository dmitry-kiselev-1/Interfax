using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Interfax.FileCollector.Web.UI.FileCollectorServiceReference;
using Interfax.FileCollector.Web.UI.Models;

namespace Interfax.FileCollector.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "";

            IEnumerable<FileStorageModel> model;

            using (var service = new FileCollectorServiceClient())
            {
                model = service.GetFiles().Select(r => new FileStorageModel()
                                                       {
                                                           Id = r.Id,
                                                           Name = r.Name,
                                                           Path = r.Path,
                                                           Type = r.Type,
                                                           Size = r.Size,
                                                           CreateDate = r.CreateDate.DateTime.ToShortDateString()
                                                                        + " : " +
                                                                        r.CreateDate.DateTime.ToShortTimeString(),
                                                           LoadDate = r.LoadDate.DateTime.ToShortDateString()
                                                                        + " : " +
                                                                        r.LoadDate.DateTime.ToShortTimeString(),
                                                           Data = null
                                                       }).ToList();
            }
            return View(model);
        }

        public ActionResult DownLoad(Guid id)
        {
            FileStorageModel model;

            using (var service = new FileCollectorServiceClient())
            {
                model = service.DownloadFile(id).Select(r => new FileStorageModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Path = r.Path,
                    Type = r.Type,
                    Size = r.Size,
                    CreateDate = r.CreateDate.DateTime.ToShortDateString()
                                 + " : " +
                                 r.CreateDate.DateTime.ToShortTimeString(),
                    LoadDate = r.LoadDate.DateTime.ToShortDateString()
                                 + " : " +
                                 r.LoadDate.DateTime.ToShortTimeString(),
                    Data = r.Data
                }).Single();
            }

            return new FileContentResult(model.Data, "application/octet-stream")
                   {
                       FileDownloadName = model.Name
                   };
        }
    }
}
