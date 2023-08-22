using check_up_money.CheckUpFile;
using check_up_money.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using check_up_money.Extensions;
using System.Collections.Specialized;
using static check_up_money.CheckUpFile.CheckUpBlob;
using System.Diagnostics;
using NLog;
using System.Security.Cryptography;
using System.Text;
using System.Security.Principal;
using check_up_money.Db;
using System.ComponentModel;
using System.Data.Odbc;

namespace check_up_money
{
    class FileHandler : IFileHandler
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");
        Logger loggerCopy = LogManager.GetLogger("CheckUpCopy-logger-info");

        bool isTestModeActive = false;
        BindingList<CheckUpBlob> bindingList;
        private readonly string localBackupFolder;
        private readonly IMain main;
        readonly IFileParser fp;
        readonly DataGridView dgv;
        IRemoteHostAvailabilityChecker rhac;
        string mainDb;

        readonly List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit;
        private readonly IStreamManager sm;
        private readonly ISqlCmdExecutor sce;
        private readonly DbConnector dbConnector;
        private readonly IPathManager pathManager;
        private readonly string archiveBackupFolder;
        private readonly bool isFileBackupActive;

        public FileHandler(IMain main, IFileParser fp,
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit,
            IStreamManager sm, ISqlCmdExecutor sce, DbConnector dbConnector, IPathManager pathManager, 
            IRemoteHostAvailabilityChecker rhac,
            string archiveBackupFolder, bool isFileBackupActive, string mainDb)
        {
            loggerDebug.Debug("Init.");
            this.main = main;
            this.fp = fp;
            this.pathsToInit = pathsToInit;
            this.sm = sm;
            this.sce = sce;
            this.dbConnector = dbConnector;
            this.pathManager = pathManager;
            this.rhac = rhac;
            this.archiveBackupFolder = archiveBackupFolder;
            this.isFileBackupActive = isFileBackupActive;
            this.mainDb = mainDb;
        }
        public FileHandler(IMain main, IFileParser fp,
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit,
            IStreamManager sm,
            string archiveBackupFolder, bool isFileBackupActive)
        {
            loggerDebug.Debug("Test init.");
            this.main = main;
            this.fp = fp;
            this.pathsToInit = pathsToInit;
            this.sm = sm;
            this.archiveBackupFolder = archiveBackupFolder;
            this.isFileBackupActive = isFileBackupActive;
        }
        public FileHandler(IFileParser fp,
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit,
            IStreamManager sm, IPathManager pathManager,
            string archiveBackupFolder, 
            bool isFileBackupActive, BindingList<CheckUpBlob> bindingList, string localBackupFolder)
        {
            loggerDebug.Debug("Test init.");
            this.fp = fp;
            this.pathsToInit = pathsToInit;
            this.sm = sm;
            this.pathManager = pathManager;
            this.archiveBackupFolder = archiveBackupFolder;

            this.isFileBackupActive = isFileBackupActive;
            isTestModeActive = true;
            this.bindingList = bindingList;
            this.localBackupFolder = localBackupFolder;
        }
        public void Init(IEnumerable<FileInfo> di, BudgetType budgetType)
        {
        }
        public void CheckUpBlob_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (CheckUpBlob newCheckUpBlob in e.NewItems)
                {
                    logger.Info($"New file in oc: {newCheckUpBlob.FileName}");
                    AddColumnToDataGridView_Main(newCheckUpBlob);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (CheckUpBlob removingItem in e.OldItems)
                {
                    logger.Info($"Removing {removingItem.FilePath} from oc.");
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (CheckUpBlob removingItem in e.OldItems)
                {
                    logger.Info($"Replacing {removingItem.FilePath} in oc.");
                }
            }
        }
        #region Table UI methods
        private void AddColumnToDataGridView_Main(CheckUpBlob cb)
        {
            /*
            // Thread safe invoke.
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new MethodInvoker(delegate () 
                {
                    dgv.Rows.Add(cb.FileName, cb.FileDate, cb.FileSizeFormatted, cb.CheckUpFileType.GetEnumDescription(), cb.CheckUpBudgetType.GetEnumDescription(),
                        cb.Data.Count, cb.TotalSumm);
                }));
            }
            else
            {
                dgv.Rows.Add(cb.FileName, cb.FileDate, cb.FileSizeFormatted, cb.CheckUpFileType.GetEnumDescription(), cb.CheckUpBudgetType.GetEnumDescription(), 
                    cb.Data.Count, cb.TotalSumm);
            }
            */
        }
        public void RemoveColumnFromDataGridView_Main(int cbItemIndex)
        {
            /*
            // Thread safe invoke
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new MethodInvoker(delegate () {
                    dgv.Rows.RemoveAt(cbItemIndex);
                }));
            }
            else
            {
                dgv.Rows.RemoveAt(cbItemIndex);
            }
            */
        }
        #endregion
        #region Collection methods
        
        public void UpdateFileName(string oldFileFullPath, string newFileFullPath, string newName)
        {
            try
            {
                logger.Info($"Update file name ({oldFileFullPath}) to ({newFileFullPath}). New name -> {newName}");

                int updateItemIndex = -1;

                for (int i = 0; i < main.CheckUpBlobsBindingList.Count; i++)
                {
                    if (main.CheckUpBlobsBindingList[i].FilePath.Equals(oldFileFullPath))
                    {
                        updateItemIndex = i;
                        break;
                    }
                }

                if (updateItemIndex < 0)
                {
                    loggerError.Error("Cannot find oldFileFullPath in collection.");
                    throw new InvalidOperationException("Cannot find oldFileFullPath in collection.");
                }
                    
                logger.Info($"Rename item at {updateItemIndex}");

                main.CheckUpBlobsBindingList.ElementAt(updateItemIndex).FilePath = newFileFullPath;
                main.CheckUpBlobsBindingList.ElementAt(updateItemIndex).FileName = newName;
            }

            catch (InvalidOperationException e)
            {
                logger.Info($"Unable update {oldFileFullPath}. Item didn't exist. {e.Message}");
            }
        }
        public void UpdateFile(string fileFullPath, BudgetType budgetType)
        {
            try
            {
                //logger.Info($"Update file ({fileFullPath}). Budget type -> {budgetType.GetEnumDescription()}");

                int updateItemIndex = -1;

                for (int i = 0; i < main.CheckUpBlobsBindingList.Count; i++)
                {
                    if (main.CheckUpBlobsBindingList[i].FilePath.Equals(fileFullPath))
                    {
                        updateItemIndex = i;
                        break;
                    }
                }

                if (updateItemIndex < 0)
                {
                    loggerError.Error("Cannot find oldFileFullPath in collection.");
                    throw new InvalidOperationException("Cannot find oldFileFullPath in collection.");
                }
                    
                CheckUpBlob cb = fp.ParseFile(Path.GetDirectoryName(fileFullPath), Path.GetFileName(fileFullPath), budgetType).Result;
                main.CheckUpBlobsBindingList[updateItemIndex] = cb;
            }
            catch (InvalidOperationException e)
            {
                loggerError.Error($"Unable update {fileFullPath}. Item didn't exist. {e.Message}");
            }
        }
        #endregion
        #region File processing
        public async Task<bool> ProccessTheFileNew(int fileIndex, int foreignCurrencyOverride = 0)
        {
            bool isSuccessful = false;

            if(!isTestModeActive && !rhac.CheckRealServer(mainDb))
            {
                logger.Info($"Db {main} is offline.");
                return isSuccessful;
            }

            logger.Info($"File backup mode -> {isFileBackupActive}");

            var checkUpBlob = isTestModeActive ? bindingList[fileIndex] : main.CheckUpBlobsBindingList[fileIndex];
            string pathSettingType = pathManager.
                GetPathTypeFromCheckUpFileTypeAndBudgetType(checkUpBlob.CheckUpFileType, checkUpBlob.CheckUpBudgetType);

            string localBackupDir = isTestModeActive ? localBackupFolder :
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archive",
                    Path.GetDirectoryName(checkUpBlob.FileName).GetUniqueFilePathFromFolder());
            string remoteBackupDir = archiveBackupFolder.GetUniqueFilePathFromFolder();

            var pathToInit = pathsToInit.
                Single(p => p.pathType.Equals(pathSettingType));

            if(isTestModeActive && foreignCurrencyOverride != -1)
            {
                pathToInit.isCurrencyEnabled = foreignCurrencyOverride != 0;
            }

            logger.Info($"File currency mode -> {pathToInit.isCurrencyEnabled}");

            string destDir = pathToInit.path;
            string localBackupDirFilePath = Path.Combine(localBackupDir, checkUpBlob.FileName);
            string remoteBackupDirDestFilePath = Path.Combine(remoteBackupDir, checkUpBlob.FileName);
            string sourceFilePath = checkUpBlob.FilePath;
            string destFilePath = Path.Combine(destDir, checkUpBlob.FileName);

            int filesInBankDir = CountFilesInBankDirBasedOnPathType(pathSettingType, pathToInit.isCurrencyEnabled);
            bool isClientBankDirHasFiles = filesInBankDir > 0;

            if (checkUpBlob.CheckUpFileType == FileType.Unknown)
            {
                logger.Info($"Unable to process uknown type file: {checkUpBlob.FilePath}");
                MessageBox.Show("Обработка файла с неизвестным типом запрещена. Обратитесь к администратору. Подробности в лог-файле.");
            }
            else if(isClientBankDirHasFiles)
            {
                logger.Info($"Processing is blocked. Bank dir's has {filesInBankDir} files.");
                MessageBox.Show("Имеются необработанные файлы для клиент-банка. Обработайте файлы для продолжения работы. ");
            }
            else if (checkUpBlob.CheckUpFileType == FileType.IncForeign || checkUpBlob.CheckUpFileType == FileType.OutForeign 
                || checkUpBlob.CheckUpFileType == FileType.CurrencyExchange)
            {
                destDir = pathToInit.isCurrencyEnabled ? pathsToInit.Single(pi => pi.pathType.Equals(pathToInit.pathType + "Currency")).path : 
                    pathToInit.path;
                destFilePath = Path.Combine(destDir, checkUpBlob.FileName);

                isSuccessful = await CopyFileNew(sourceFilePath,
                    destFilePath, destDir,
                    localBackupDirFilePath, localBackupDir,
                    remoteBackupDirDestFilePath, remoteBackupDir,
                    false, fileIndex, checkUpBlob.CheckUpFileType);
            }
            else
            {
                isSuccessful = await CopyFileNew(sourceFilePath, 
                    destFilePath, destDir, 
                    localBackupDirFilePath, localBackupDir,
                    remoteBackupDirDestFilePath, remoteBackupDir,
                    false, fileIndex, checkUpBlob.CheckUpFileType);
            }

            return isSuccessful;
        }
        private int CountFilesInBankDirBasedOnPathType(string pathSettingType, bool isCurrencyEnabled)
        {
            int totalFiles = 0;
            
            if(pathSettingType.Contains("BankMain") || pathSettingType.Contains("BankMisc"))
            {
                totalFiles = new DirectoryInfo(pathsToInit.Single(p => p.pathType.Equals(pathSettingType)).path).GetFiles().ToList().Count();

                if (isCurrencyEnabled)
                    totalFiles += new DirectoryInfo(pathsToInit.Single(p => p.pathType.Equals(pathSettingType + "Currency")).path).GetFiles().ToList().Count();

                logger.Info($"Total files in {pathSettingType}: {totalFiles}");
            }

            return totalFiles;
        }
        private async Task<bool> CopyFileNew(string sourceFilePath, 
            string destFilePath, string destDir, 
            string localBackupFilePath, string localBackupDir,
            string remoteBackupFilePath, string remoteBackupDir,
            bool overwriteCopyCheck, int fileIndex, FileType fileType)
        {
            bool isSuccessful = false;
            bool isFilesChecked = false;

            try
            {
                TimeSpan[] totalElapsedTimeForBackups = null;

                logger.Info($"Processing: {sourceFilePath}");

                if (overwriteCopyCheck)
                {
                    if (File.Exists(sourceFilePath))
                    {
                        loggerError.Error($"Overwrite copy check was failed. File in destination folder exist. {sourceFilePath} == {destFilePath}");
                        throw new Exception("Возникла ошибка при копировании файла. Обнаружен файл с одинаковым именем. Обратитесь к администратору.");
                    }
                }

                logger.Info($"Backup file: {sourceFilePath}");

                if (isFileBackupActive)
                {
                    CheckAndCreateDir(localBackupDir);
                    CheckAndCreateDir(remoteBackupDir);

                    var copyToLocalBackup = CopyFile(sourceFilePath, localBackupFilePath, fileType);
                    var copyToRemoteBackup = CopyFile(sourceFilePath, remoteBackupFilePath, fileType);
                    totalElapsedTimeForBackups = await Task.WhenAll(copyToLocalBackup, copyToRemoteBackup).ConfigureAwait(false);
                }
                else
                {
                    CheckAndCreateDir(localBackupDir);

                    var copyToLocalBackup = CopyFile(sourceFilePath, localBackupFilePath, fileType);
                    totalElapsedTimeForBackups = await Task.WhenAll(copyToLocalBackup).ConfigureAwait(false);
                }
                
                logger.Info($"Backup complete: {new TimeSpan(totalElapsedTimeForBackups.Sum(r => r.Ticks)).ToString("mm':'ss':'fff")}");

                logger.Info($"Copy file to destination: {sourceFilePath} -> {destFilePath}");

                CheckAndCreateDir(destDir);

                var copyToDest = CopyFile(sourceFilePath, destFilePath, fileType);

                var totalElapsedTimeForDestCopy = await Task.WhenAll(copyToDest).ConfigureAwait(false);

                logger.Info($"Copy completed: {new TimeSpan(totalElapsedTimeForDestCopy.Sum(r => r.Ticks)).ToString("mm':'ss':'fff")}");

                // Check files.
                isFilesChecked = CopyCheckNew(sourceFilePath, destFilePath, localBackupFilePath, remoteBackupFilePath);

                if(isFilesChecked)
                {
                    logger.Info($"Delete -> {sourceFilePath}");

                    File.Delete(sourceFilePath);

                    // Write to db.
                    if (!isTestModeActive)
                        await Task.Run(() => LogToDb(main.CheckUpBlobsBindingList[fileIndex]));

                    isSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                loggerError.Error($"Copy failed. {sourceFilePath} {ex.Message}");
                throw new Exception("Возникла ошибка при копировании файла. Обратитесь к администратору.");
            }

            return isSuccessful;
        }
        private void CheckAndCreateDir(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        private bool CopyCheckNew(string sourceFilePath, string destFilePath, string localBackupFilePath, string remoteBackupFilePath)
        {
            bool isSuccessful = false;

            logger.Info($"Copy check for {sourceFilePath}");

            try
            {
                bool isFilesExist = false;
                bool isMD5HashesIsEqual = false;

                if (isFileBackupActive)
                {
                    isFilesExist = File.Exists(destFilePath) && File.Exists(localBackupFilePath) && File.Exists(remoteBackupFilePath);
                    isMD5HashesIsEqual =
                        CompareMD5(sourceFilePath, destFilePath) &&
                        CompareMD5(sourceFilePath, localBackupFilePath) &&
                        CompareMD5(sourceFilePath, remoteBackupFilePath);
                }
                else
                {
                    isFilesExist = File.Exists(destFilePath) && File.Exists(localBackupFilePath);
                    isMD5HashesIsEqual =
                        CompareMD5(sourceFilePath, destFilePath) &&
                        CompareMD5(sourceFilePath, localBackupFilePath);
                }

                if (isFilesExist && isMD5HashesIsEqual)
                {
                    isSuccessful = true; 
                }
            }
            catch (Exception ex)
            {
                loggerError.Error(ex, $"Copy check was failed for: {sourceFilePath}");
                throw new Exception("Возникла ошибка при проверке файлов. Обратитесь к администратору.");
            }

            return isSuccessful;
        }
        private bool CompareMD5(string sourceFilePath, string destFilePath)
        {
            bool isHashesAreEqual = false;

            var sourceMD5 = GetMD5(sourceFilePath);
            var destMD5 = GetMD5(destFilePath);

            if (sourceMD5 == destMD5)
            {
                isHashesAreEqual = true;
                logger.Info($"File hashes are equal: source -> [{sourceMD5}], dest -> [{destMD5}]");
            }
            else
            {
                loggerError.Error($"File hashes are not equal: source -> [{sourceMD5}], dest -> [{destMD5}]");
            }

            return isHashesAreEqual;
        }
        private string GetMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }
        private void LogToDb(CheckUpBlob checkUpBlob)
        {
            bool isUserExist = false;
            bool isCurrencyTypeExist = false;

            string userName = WindowsIdentity.GetCurrent().GetUserNameFromIdentity();
            string currencyType = checkUpBlob.CurrencyType;

            isUserExist = Convert.ToBoolean(sce.ExecuteOdbcCmdScalar(dbConnector.ConnectionStringOdbc, SqlCommands.IsUserExist(null, userName)).Result);
            isCurrencyTypeExist = Convert.ToBoolean(sce.ExecuteOdbcCmdScalar(dbConnector.ConnectionStringOdbc, SqlCommands.IsCurrencyTypeExist(null, currencyType)).Result);

            if (!isUserExist)
            {
                int userResult = (int)(sce.ExecuteOdbcCmdScalar(dbConnector.ConnectionStringOdbc, SqlCommands.CreateNewUser(null, userName, DateTime.Now)).Result);

                logger.Info($"New user [{userName}] creation status: {userResult}");
            }

            if (!isCurrencyTypeExist)
            {
                int currencyTypeResult = (int)(sce.ExecuteOdbcCmdScalar(dbConnector.ConnectionStringOdbc, SqlCommands.CreateNewCurrencyType(null, currencyType)).Result);

                logger.Info($"New currency type [{currencyType}] creation status: {currencyTypeResult}");
            }

            int userId = (int)(sce.ExecuteOdbcCmdForUserId(dbConnector.ConnectionStringOdbc, SqlCommands.GetUserId(null, userName)).Result);
            int currencyId = (int)(sce.ExecuteOdbcCmdScalar(dbConnector.ConnectionStringOdbc, SqlCommands.GetCurrencyId(null, currencyType)).Result);

            OdbcCommand newLogCmd;

#if DEBUG
            newLogCmd = SqlCommands.NewLogEntryForTest(null, DateTime.Now, checkUpBlob.FileName, checkUpBlob.TotalSumm, checkUpBlob.Data.Count,
                (int)checkUpBlob.CheckUpBudgetType + 1, (int)checkUpBlob.CheckUpFileType + 1, userId, currencyId);
#else
            newLogCmd = SqlCommands.NewLogEntry(null, DateTime.Now, checkUpBlob.FileName, checkUpBlob.TotalSumm, checkUpBlob.Data.Count,
                (int)checkUpBlob.CheckUpBudgetType + 1, (int)checkUpBlob.CheckUpFileType + 1, userId, currencyId);
#endif

            var logResult = sce.ExecuteOdbcCmdScalar(dbConnector.ConnectionStringOdbc, newLogCmd).Result;
        }
        #endregion
        private async Task<TimeSpan> CopyFile(string sourceFile, string destinationFile, FileType fileType)
        {
            TimeSpan elapsedTime = TimeSpan.Zero;

            try
            {
                Stopwatch sw = new Stopwatch();

                loggerCopy.Info($"Copy file [{fileType.GetEnumDescription()}] -> [{sourceFile}] to [{destinationFile}]");

                using(FileStream sourceStream = await sm.WaitForFileStream(sourceFile, FileMode.Open,
                    FileAccess.Read, FileShare.Read))
                using (FileStream destinationStream = File.Create(destinationFile))
                {
                    sw.Start();

                    await sourceStream.CopyToAsync(destinationStream);

                    sourceStream.Close();

                    sw.Stop();
                    elapsedTime = sw.Elapsed;
                }
            }
            catch (IOException ioex)
            {
                loggerError.Error($"An IOException occured during move {ioex.Message}");
            }
            catch (Exception ex)
            {
                loggerError.Error($"An Exception occured during move {ex.Message}");
            }

            return elapsedTime;
        }
        public void OpenFileFolder(int rowIndex)
        {
            string filePath = main.CheckUpBlobsBindingList[rowIndex].FilePath;
            string fileDir = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(filePath) && Directory.Exists(fileDir))
            {
                logger.Info($"Open folder -> {fileDir} at {rowIndex}");
                Process.Start("explorer.exe", fileDir);
            }
            else
            {
                loggerError.Error($"Cannot open selected folder for selected index -> {rowIndex}");
            }
        }
    }
}