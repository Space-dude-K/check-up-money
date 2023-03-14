using check_up_money;
using check_up_money.CheckUpFile;
using check_up_money.Cypher;
using check_up_money.Extensions;
using check_up_money.FileHelpers;
using check_up_money.Interfaces;
using check_up_money.Settings;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace check_up_money_test
{
    internal class TestFormMain : Form_Main
    {
        public TestFormMain(bool isTestCase) : base(isTestCase)
        {

        }
    }
    class TestFileHandler : FileHandler
    {
        public TestFileHandler(IFileParser fp,
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit,
            IStreamManager sm, IPathManager pathManager,
            string archiveBackupFolder, bool isFileBackupActive, BindingList<CheckUpBlob> checkUpBlobs, string localBackupFolder) :
            base(fp, pathsToInit, sm, pathManager, archiveBackupFolder, isFileBackupActive, checkUpBlobs, localBackupFolder)
        {

        }
    }
    internal class TestPathManager : PathManager
    {
        public TestPathManager(
            TableLayoutPanel tlp,
            List<(string pathType, string path)> pathSettings,
            List<(string budgetType, bool isEnabled)> budgetSettings,
            List<(string budgetType, bool isEnabled)> currencySettings,
            List<(string ticketType, bool isEnabled)> ticketSettings) :
            base(tlp, pathSettings, budgetSettings, currencySettings, ticketSettings)
        {
        }
    }
    internal class TestFileParser : FileParser
    {
        public TestFileParser(IFileLockHelper flh, List<string> allowedFileExtensions, IStreamManager streamManager) :
            base(flh, allowedFileExtensions, streamManager)
        {
        }
    }
    class TestInitializer
    {
        internal readonly Logger logger = LogManager.GetLogger("CheckUpTest-logger-info");

        internal string solutionFolder;
        internal string settingsFolder;

        internal string sourceDirPath;
        internal string destDirPath;
        internal string archivePath;

        internal List<(string budgetType, bool isEnabled)> budgetSettings;
        internal List<(string budgetType, bool isEnabled)> currencySettings;
        internal List<(string ticketType, bool isEnabled)> ticketSettings;
        internal List<(string pathType, string path)> pathSettings;

        internal IFormHelper formHelper;
        internal IPathManager pathManager;
        internal IFileLockHelper fileLockHelper;
        internal IStreamManager streamManager;
        internal TestFileParser fileParser;
        internal TestFileHandler fileHandler;
        internal INotificationManager notificationManager;
        internal IMain main;

        private List<string> allowedFileExtensions = new List<string>() { ".P02", ".P01", ".38", ".36" };

        internal BindingList<CheckUpBlob> checkUpBlobs;

        internal List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit;

        [Obsolete]
        public void Init()
        {
            solutionFolder = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            sourceDirPath = Path.Combine(solutionFolder, @"check-up-money-test\TestFiles\TestPackage");
            destDirPath = Path.Combine(solutionFolder, @"check-up-money-test\TestFiles\TestDir");
            archivePath = Path.Combine(solutionFolder, @"check-up-money-test\TestFiles\TestArchive");
            settingsFolder = Path.Combine(solutionFolder, @"check-up-money-test\Settings");

            InitSettings();
            InitLogger(new XmlLoggingConfiguration(Path.Combine(settingsFolder, "NLog.config"), true));
            CreateTestFoldersAndFiles();
            InitBase();

            Debug.WriteLine("Ln: " + logger.Name);
        }
        /// <summary>
        /// Инициализируем NLog.
        /// </summary>
        /// <param name="cfg"></param>
        [Obsolete]
        private void InitLogger(XmlLoggingConfiguration cfg)
        {
            LogManager.Configuration = cfg;
        }
        /// <summary>
        /// Инициализируем настройки путей, изменяем пути под тестовый набор директорий и файлов.
        /// </summary>
        private void InitSettings()
        {
            ICypher cypher = new Encryptor();
            Configurator configurator = new Configurator(cypher, Path.Combine(settingsFolder, "TestSettings.config"));

            budgetSettings = configurator.LoadBudgetSettings();
            currencySettings = configurator.LoadCurrencySettings();
            ticketSettings = configurator.LoadTicketSettings();
            pathSettings = configurator.LoadPathSettings().ChangeBaseDirForPathSettings(destDirPath);

            checkUpBlobs = new();
        }
        /// <summary>
        /// Создаём тестовые директории и копируем тестовый набор файлов.
        /// </summary>
        public void CreateTestFoldersAndFiles()
        {
            bool isDestDirExist = Directory.Exists(destDirPath);

            if (isDestDirExist)
                Directory.Delete(destDirPath, true);

            TestHelpers.CopyDirectory(sourceDirPath, destDirPath, true);

            isDestDirExist = Directory.Exists(destDirPath);
            var sourceDirSize = TestHelpers.DirSize(new DirectoryInfo(sourceDirPath));
            var destDirSize = TestHelpers.DirSize(new DirectoryInfo(sourceDirPath));

            logger.Info($"Test folder creation test. Dir exist: {isDestDirExist} - sDirSize: {sourceDirSize} - dDirSize: {destDirSize}");

            //Assert.AreEqual(true, isDestDirExist);
            //Assert.AreEqual(sourceDirSize, destDirSize);
        }
        private void InitBase()
        {
            logger.Info($"" +
                $"Ps: {pathSettings.Count} - " +
                $"bs: {budgetSettings.Count} - " +
                $"cs: {currencySettings.Count} - " +
                $"ts: {ticketSettings.Count}");

            formHelper = new FormHelper();
            pathManager = new PathManager
                (
                null,
                pathSettings,
                budgetSettings,
                currencySettings,
                ticketSettings
                );
            pathsToInit = pathManager.GetPathsToInit();

            fileLockHelper = new FileLockHelper();
            streamManager = new StreamManager(fileLockHelper, 20, 30);
            fileParser = new TestFileParser(fileLockHelper, allowedFileExtensions, streamManager);
            fileHandler = new TestFileHandler(fileParser, pathsToInit, streamManager, pathManager, archivePath, false, checkUpBlobs, archivePath);
            
        }
        public void InitMainTestMainForm()
        {
            main = new TestFormMain(true);

            //notificationManager = new NotificationManager(main, 1, 20, null, null);
        }
        public void ClearDirs()
        {
            logger.Info($"Cleanup {destDirPath}, {archivePath}.");

            if(Directory.Exists(destDirPath))
                Directory.Delete(destDirPath, true);

            if(Directory.Exists(archivePath))
                Directory.Delete(archivePath, true);
        }
    }
}