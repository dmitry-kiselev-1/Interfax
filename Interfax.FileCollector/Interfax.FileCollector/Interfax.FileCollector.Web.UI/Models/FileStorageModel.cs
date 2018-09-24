using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interfax.FileCollector.Web.UI.Models
{
    public class FileStorageModel : FileCollectorServiceReference.FileStorage
    {
        public new string CreateDate { get; set; }
        public new string LoadDate { get; set; }
    }
}