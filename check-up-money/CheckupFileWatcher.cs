using check_up_money.Extensions;
using check_up_money.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NLog;
using static check_up_money.CheckUpFile.CheckUpBlob;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace check_up_money
{
    class CheckupFileWatcher : ICheckupFileWatcher
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        readonly IFileHandler fh;
        private readonly INotificationManager nm;
        private readonly IMain main;
        public FileSystemWatcher watcher;

        int currentFilesInFolder = 0;
        string currentFolder;

        int watcherErrorCounter = 0;
        bool isWatcherRunning = false;

        public int CurrentFilesInFolder { get => currentFilesInFolder; set => currentFilesInFolder = value; }
        public string CurrentFolder { get => currentFolder; set => currentFolder = value; }

        public CheckupFileWatcher(IFileHandler fh, INotificationManager nm, IMain main)
        {
            loggerDebug.Debug("Init.");

            this.fh = fh;
            this.nm = nm;
            this.main = main;
        }
        /// <summary>
        /// Этот метод создает <see cref="FileSystemWatcher"></see> для мониторинга директорий.
        /// </summary>
        /// <param name="labelNameAndTip">Имя инфопанели с подсказкой.</param>
        /// <param name="dirToWatch">Директория для наблюдения.</param>
        /// <param name="budgetType">Тип бюджета.</param> 
        /// <param name="tableLayoutControl">Ссылка на панель.</param> 
        /// <param name="bufferSize">Размер буфера в байтах.</param> 
        /// <param name="fileMask">Фильтр по маске файла.</param> 
        public async Task<bool> RunWatcher((string pathName, string labelTip) labelNameAndTip, 
            string dirToWatch, BudgetType budgetType, Control tableLayoutControl, int bufferSize, string fileMask)
        {
            logger.Info($"Starting watcher for {labelNameAndTip.pathName} in {dirToWatch} with mask {fileMask}");
            
            watcher = new FileSystemWatcher(dirToWatch);

            await Task.Delay(100);

            //UpdateTableFileCounter(pathName, dirToWatch, tableLayoutControl);

            CurrentFolder = dirToWatch;
            //watcher.Changed += new FileSystemEventHandler((sender, e) => Watcher_Changed(sender, e));

            watcher.Error += new ErrorEventHandler((sender, e) => Watcher_Error(sender, e, labelNameAndTip,
            dirToWatch, budgetType, tableLayoutControl, bufferSize, fileMask));

            watcher.Disposed += Watcher_Disposed;

            watcher.Created += new FileSystemEventHandler((sender, e) => Watcher_Created(sender, e, budgetType, tableLayoutControl));
            //watcher.Created += new FileSystemEventHandler((sender, e) => Watcher_Changed(sender, e, labelNameAndTip.pathName, tableLayoutControl));
            watcher.Deleted += new FileSystemEventHandler((sender, e) => Watcher_Deleted(sender, e, tableLayoutControl));
            //watcher.Deleted += new FileSystemEventHandler((sender, e) => Watcher_Changed(sender, e, labelNameAndTip.pathName, tableLayoutControl));
            watcher.Changed += new FileSystemEventHandler((sender, e) => Watcher_Updated(sender, e, budgetType, tableLayoutControl));
            watcher.Renamed += new RenamedEventHandler((sender, e) => Watcher_Renamed(sender, e, tableLayoutControl));

            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = false;
            watcher.InternalBufferSize = bufferSize;

            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.Attributes;

            if (!string.IsNullOrEmpty(fileMask))
            {
                watcher.Filters.Add(fileMask);
            }

            isWatcherRunning = true;

            return true;
        }
        private void Watcher_Disposed(object sender, EventArgs e)
        {
            var watcher = (FileSystemWatcher)sender;

            loggerDebug.Debug($"Watcher disposed: {watcher.Path}.");
        }
        private async void Watcher_Error(object sender, ErrorEventArgs e, (string pathName, string labelTip) labelNameAndTip,
            string dirToWatch, BudgetType budgetType, Control tableLayoutControl, int bufferSize, string fileMask)
        {
            watcherErrorCounter++;
            isWatcherRunning = false;

            var exception = e.GetException();
            var watcher = (FileSystemWatcher)sender;

            //loggerError.Error("Watcher error. " + exception.GetType() + " " + exception.Message);

            tableLayoutControl.ThreadSafeInvokeForTextAssignment("ERR");
            tableLayoutControl.ThreadSafeInvokeForColor(Color.Red);

            if (exception.GetType() == typeof(InternalBufferOverflowException))
            {
                //  This can happen if Windows is reporting many file system events quickly 
                //  and internal buffer of the  FileSystemWatcher is not large enough to handle this
                //  rate of events. The InternalBufferOverflowException error informs the application
                //  that some of the file system events are being lost.
                loggerError.Error($"The file system watcher experienced an internal buffer overflow: {exception.Message} for {watcher.Path}");
            }
            else if(exception.GetType() == typeof(Win32Exception))
            {
                loggerError.Error($"Watcher error. {exception.GetType()} {exception.Message}: {watcher.Path}");
            }

            // Restore watcher.
            while (!isWatcherRunning)
            {
                if (watcherErrorCounter >= 20)
                {
                    loggerError.Error($"The maximum number of restore attempts for the file system watcher has been reached.");
                    throw new Exception($"Превышено максимальное количество попыток восстановления связи с директорией: {watcher.Path}");
                }

                loggerError.Error($"Restoring watcher for: {watcher.Path}. Attempt № {watcherErrorCounter}");

                if (watcher != null)
                    watcher.Dispose();

                await Task.Delay(10000);

                if(Directory.Exists(watcher.Path))
                {
                    await RunWatcher(labelNameAndTip, dirToWatch, budgetType, tableLayoutControl, bufferSize, fileMask);

                    loggerDebug.Debug($"Watcher {watcher.Path} was restored after {watcherErrorCounter} attempts.");
                }
                else
                {
                    logger.Error($"Directory access error: {watcher.Path}");
                }

                watcherErrorCounter++;
            }

            loggerDebug.Debug($"Watcher {watcher.Path} status: {isWatcherRunning}.");
        }
        public virtual void Watcher_Created(object sender, FileSystemEventArgs e, BudgetType budgetType, Control control)
        {
            logger.Info("Watcher_Created");

            // Skip directory checking files.
            if (!string.IsNullOrEmpty(e.FullPath))
            {
                if(Path.GetExtension(e.FullPath) != ".test")
                {
                    logger.Info($"New file -> {e.FullPath}.");

                    AddFileToObservableCollection(e.FullPath, budgetType);
                    currentFilesInFolder++;

                    logger.Info($"Current files: {CurrentFilesInFolder}");

                    if(nm != null && main != null)
                        nm.NotificationData = main.CheckUpBlobsBindingList;
                }
            }
            else
            {
                loggerDebug.Error($"File {e.FullPath} was null.");

                if(control != null)
                {
                    control.ThreadSafeInvokeForTextAssignment("ERR");
                    control.ThreadSafeInvokeForColor(Color.Red);
                }
            }
        }
        private void Watcher_Deleted(object sender, FileSystemEventArgs e, Control control)
        {
            if (Path.GetExtension(e.FullPath) == ".test")
            {
                logger.Debug($"File {e.FullPath} has not supported extension.");
            }
            else if (!string.IsNullOrEmpty(e.FullPath))
            {
                if (nm != null && main != null)
                {
                    main.Remove(e.FullPath);
                    currentFilesInFolder--;
                    nm.NotificationData = main.CheckUpBlobsBindingList;
                }
            }
            else
            {
                loggerError.Error($"File {e.FullPath} has not supported extension.");

                if (control != null)
                {
                    control.ThreadSafeInvokeForTextAssignment("ERR");
                    control.ThreadSafeInvokeForColor(Color.Red);
                }
            }
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e, string pathName, Control tableLayoutControl)
        {
            if (File.Exists(e.FullPath))
            {
                if (Path.GetExtension(e.FullPath) == ".test")
                {
                    logger.Debug($"File {e.FullPath} has not supported extension.");
                }
                else
                {
                    //CurrentFilesInFolder = Directory.GetFiles(Path.GetDirectoryName(e.FullPath)).Count();

                    //UpdateTableFileCounter(pathName, Path.GetDirectoryName(e.FullPath), tableLayoutControl);

                    // Update oc.
                    UpdateFileObservableCollection(Path.GetFileName(e.Name));

                    if (nm != null && main != null)
                    {
                        nm.NotificationData = main.CheckUpBlobsBindingList;
                    }
                }
            }
            else
            {
                //tableLayoutControl.ThreadSafeInvokeForTextAssignment("ERR");
                //tableLayoutControl.ThreadSafeInvokeForColor(Color.Red);
            }
        }
        private void Watcher_Updated(object sender, FileSystemEventArgs e, BudgetType budgetType, Control control)
        {
            if (File.Exists(e.FullPath) && main.CheckUpBlobsBindingList.Any(cb => cb.FilePath.Equals(e.FullPath)))
            {
                if (Path.GetExtension(e.FullPath) == ".test")
                {
                    logger.Debug($"File {e.FullPath} has not supported extension.");
                }
                else
                {
                    logger.Info($"Update file: {e.FullPath}");

                    // Re-parse file and update in oc
                    fh.UpdateFile(e.FullPath, budgetType);

                    if (nm != null && main != null)
                    {
                        nm.NotificationData = main.CheckUpBlobsBindingList;
                    }
                }
            }
            else
            {
                if (control != null)
                {
                    control.ThreadSafeInvokeForTextAssignment("ERR");
                    control.ThreadSafeInvokeForColor(Color.Red);
                }
            }
        }
        private void Watcher_Renamed(object sender, RenamedEventArgs e, Control control)
        {
            //loggerDebug.Debug($"Watcher_Renamed -> {e.OldName}");

            if (File.Exists(e.FullPath))
            {
                if (Path.GetExtension(e.FullPath) == ".test")
                {
                    logger.Debug($"File {e.FullPath} has not supported extension.");
                }
                else
                {
                    // Update file name in oc
                    string oldFullPath = e.OldFullPath;
                    string newFullPath = e.FullPath;
                    string newFileName = e.Name;

                    logger.Info($"File renamed {e.FullPath}. Attempting rename {oldFullPath} to {newFullPath} ({newFileName})");

                    if (!string.IsNullOrWhiteSpace(oldFullPath) && !string.IsNullOrWhiteSpace(newFullPath) && !string.IsNullOrWhiteSpace(newFileName))
                    {
                        fh.UpdateFileName(oldFullPath, newFullPath, newFileName);
                    }
                    else
                    {
                        loggerError.Error($"Unable to rename file {oldFullPath} to {newFullPath}");
                    }

                    if (nm != null && main != null)
                    {
                        nm.NotificationData = main.CheckUpBlobsBindingList;
                    }
                }
            }
            else
            {
                if (control != null)
                {
                    control.ThreadSafeInvokeForTextAssignment("ERR");
                    control.ThreadSafeInvokeForColor(Color.Red);
                }
            }
        }
        // Add file to oc.
        public virtual void AddFileToObservableCollection(string fullFilePath, BudgetType budgetType)
        {
            //logger.Info($"Trying add {fullFilePath} to observable collection!");

            if (main != null)
            {
                main.Add(fullFilePath, budgetType);
            }
        }
        // Update oc.
        private void UpdateFileObservableCollection(string fullFilePath)
        {
            //fh.Remove(fullFilePath);
        }
    }
}