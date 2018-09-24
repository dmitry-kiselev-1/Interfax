using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Interfax.FileCollector.Windows.Service.FileCollectorServiceReference;

namespace Interfax.FileCollector.Windows.Service
{
    public partial class FileCollectorService : ServiceBase
    {
        public FileCollectorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.WriteEntry("Выполнена команда запуска сервиса", EventLogEntryType.Information);
            
            var monitoringPath  = new DirectoryInfo( Settings.Default.MonitoringPath );

            var watcher = new FileSystemWatcher()
                          {
                             Path = monitoringPath.FullName, 
                             IncludeSubdirectories = false,
                             EnableRaisingEvents = true
                          };

            // Вариант 1: обработчик файлов является асинхронным (вызывается в отдельном потоке для каждого файла):
            watcher.Created += async (sender, eventArgs) =>
            {
                if (eventArgs.ChangeType == WatcherChangeTypes.Created)
                {
                    await FileProcessor.ProcessTaskAsync(new FileInfo(eventArgs.FullPath), this.EventLog);
                }
            };

            /*
            // Вариант 2: в обработчике файлов только вызов веб-сервиса является асинхронным (вызывается в отдельном потоке для каждого файла):
            watcher.Created += (sender, eventArgs) =>
            {
                if (eventArgs.ChangeType == WatcherChangeTypes.Created)
                {
                    var fileProcessor = new FileProcessor();
                    fileProcessor.ProcessAsync(new FileInfo(eventArgs.FullPath), this.EventLog);
                }
            };
            */
        }

        protected override void OnStop()
        {
            this.EventLog.WriteEntry("Выполнена команда остановки сервиса", EventLogEntryType.Information);
        }
    }
}
