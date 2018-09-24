using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfax.FileCollector.Windows.Service.FileCollectorServiceReference;

namespace Interfax.FileCollector.Windows.Service
{
    internal class FileProcessor
    {
        /// <summary>
        /// Обработчик файлов, отправляет их в хранилище асинхронно 
        /// и регистрирует все процессы, включая ошибки, в журнале приложений windows.
        /// </summary>
        /// <param name="fileInfo">Файл</param>
        /// <param name="eventLog">Журнал</param>
        async internal void ProcessAsync(FileInfo fileInfo, EventLog eventLog)
        {
            var monitoringPath = new DirectoryInfo(Settings.Default.MonitoringPath);
            var sizeErrorPath = new DirectoryInfo(Settings.Default.SizeErrorPath);
            var uploadErrorPath = new DirectoryInfo(Settings.Default.UploadErrorPath);
            var maxUploadFileSize = Settings.Default.MaxUploadFileSize;

            fileInfo.Refresh();

            if (fileInfo.Length <= maxUploadFileSize)
            {
                try
                {
                    eventLog.WriteEntry("Старт загрузки файла " + fileInfo.Name, EventLogEntryType.Information);
                    using (var service = new FileCollectorServiceClient())
                    {
                        byte[] data = File.ReadAllBytes(fileInfo.FullName);
                        
                        // Асинхронная загрузка файла в новом потоке:
                        await service.UploadFileAsync(fileInfo.Name, fileInfo.LastWriteTime, data);

                        eventLog.WriteEntry("Успешное завершение загрузки файла " + fileInfo.Name, EventLogEntryType.Information);
                        try
                        {
                            File.Delete(fileInfo.FullName);
                            eventLog.WriteEntry("Удаление после успешной загрузки файла " + fileInfo.Name, EventLogEntryType.Information);
                        }
                        catch (Exception e0)
                        {
                            eventLog.WriteEntry("Ошибка после успешной загрузки при удалении файла " + fileInfo.Name + ". Описание ошибки: " + e0, EventLogEntryType.Error);
                        }
                    }
                }
                catch (Exception e1)
                {
                    eventLog.WriteEntry("Старт перемещения файла по причине сбоя загрузки: " + fileInfo.Name + ". Описание ошибки: " + e1, EventLogEntryType.Error);
                    try
                    {
                        File.Move(fileInfo.FullName, uploadErrorPath.FullName + fileInfo.Name);
                        eventLog.WriteEntry("Завершение перемещения файла по причине сбоя загрузки: " + fileInfo.Name, EventLogEntryType.Information);
                    }
                    catch (Exception e2)
                    {
                        eventLog.WriteEntry("Ошибка при перемещении файла по причине сбоя загрузки: " + fileInfo.Name + fileInfo.Name + ". Описание ошибки: " + e2, EventLogEntryType.Error);
                    }
                }
            }
            else
            {
                eventLog.WriteEntry("Старт перемещения файла по причине превышения размера: " + fileInfo.Name, EventLogEntryType.Error);
                try
                {
                    File.Move(fileInfo.FullName, sizeErrorPath.FullName + fileInfo.Name);
                    eventLog.WriteEntry("Завершение перемещения файла по причине превышения размера: " + fileInfo.Name, EventLogEntryType.Information);
                }
                catch (Exception e3)
                {
                    eventLog.WriteEntry("Ошибка при перемещении файла по причине превышения размера: " + fileInfo.Name + fileInfo.Name + ". Описание ошибки: " + e3, EventLogEntryType.Error);
                }
            }
        }

        /// <summary>
        /// Асинхронный паттерн TAP.
        /// Обработчик файлов для вызова в асинхронной операции, 
        /// отправляет их в хранилище и регистрирует все процессы, включая ошибки, в журнале приложений windows.
        /// </summary>
        /// <param name="fileInfo">Файл</param>
        /// <param name="eventLog">Журнал</param>
        internal static Task<bool> ProcessTaskAsync(FileInfo fileInfo, EventLog eventLog)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            var monitoringPath = new DirectoryInfo(Settings.Default.MonitoringPath);
            var sizeErrorPath = new DirectoryInfo(Settings.Default.SizeErrorPath);
            var uploadErrorPath = new DirectoryInfo(Settings.Default.UploadErrorPath);
            var maxUploadFileSize = Settings.Default.MaxUploadFileSize;

            fileInfo.Refresh();

            if (fileInfo.Length <= maxUploadFileSize)
            {
                try
                {
                    eventLog.WriteEntry("Старт загрузки файла " + fileInfo.Name, EventLogEntryType.Information);
                    using (var service = new FileCollectorServiceClient())
                    {
                        byte[] data = File.ReadAllBytes(fileInfo.FullName);

                        service.UploadFile(fileInfo.Name, fileInfo.LastWriteTime, data);

                        eventLog.WriteEntry("Успешное завершение загрузки файла " + fileInfo.Name, EventLogEntryType.Information);
                        try
                        {
                            File.Delete(fileInfo.FullName);
                            eventLog.WriteEntry("Удаление после успешной загрузки файла " + fileInfo.Name, EventLogEntryType.Information);
                        }
                        catch (Exception e0)
                        {
                            eventLog.WriteEntry("Ошибка после успешной загрузки при удалении файла " + fileInfo.Name + ". Описание ошибки: " + e0, EventLogEntryType.Error);
                        }
                    }
                }
                catch (Exception e1)
                {
                    eventLog.WriteEntry("Старт перемещения файла по причине сбоя загрузки: " + fileInfo.Name + ". Описание ошибки: " + e1, EventLogEntryType.Error);
                    try
                    {
                        File.Move(fileInfo.FullName, uploadErrorPath.FullName + fileInfo.Name);
                        eventLog.WriteEntry("Завершение перемещения файла по причине сбоя загрузки: " + fileInfo.Name, EventLogEntryType.Information);
                    }
                    catch (Exception e2)
                    {
                        eventLog.WriteEntry("Ошибка при перемещении файла по причине сбоя загрузки: " + fileInfo.Name + fileInfo.Name + ". Описание ошибки: " + e2, EventLogEntryType.Error);
                    }
                }
            }
            else
            {
                eventLog.WriteEntry("Старт перемещения файла по причине превышения размера: " + fileInfo.Name, EventLogEntryType.Error);
                try
                {
                    File.Move(fileInfo.FullName, sizeErrorPath.FullName + fileInfo.Name);
                    eventLog.WriteEntry("Завершение перемещения файла по причине превышения размера: " + fileInfo.Name, EventLogEntryType.Information);
                }
                catch (Exception e3)
                {
                    eventLog.WriteEntry("Ошибка при перемещении файла по причине превышения размера: " + fileInfo.Name + fileInfo.Name + ". Описание ошибки: " + e3, EventLogEntryType.Error);
                }
            }

            taskCompletionSource.SetResult(true);

            return taskCompletionSource.Task;
        }
    }
}
