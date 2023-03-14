using check_up_money;
using check_up_money.CheckUpFile;
using check_up_money.Interfaces;
using check_up_money_test;
using NLog;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace check_up_money_test
{
    class CheckupFileWatcherWrapper : CheckupFileWatcher
    {
        public int counter;

        public CheckupFileWatcherWrapper(IFileHandler fh, INotificationManager nm, IMain main) : base(fh, nm, main)
        {
        }

        public override void AddFileToObservableCollection(string fullFilePath, CheckUpBlob.BudgetType budgetType)
        {
            Interlocked.Increment(ref counter);
        }
    }
    /// <summary>
    /// TODO. Необходимо переписать тест, т.к. FileSystemWatcher полагается на файловую систему. К примеру, можно абстрагироваться через интерфейс и испольвать мок.
    /// </summary>
    [TestFixture]
    public class CheckupFileWatcherTest
    {
        readonly Logger logger = LogManager.GetLogger("CheckUpTest-logger-info");

        TestInitializer testInitializer;
        CheckupFileWatcherWrapper checkupFileWatcher;

        [SetUp]
        [Obsolete]
        public async Task Init()
        {
            testInitializer = new();
            testInitializer.Init();

            var settings = testInitializer.pathsToInit;

            checkupFileWatcher = new CheckupFileWatcherWrapper(testInitializer.fileHandler, null, null);
            checkupFileWatcher.RunWatcher(
                    testInitializer.pathManager.GetCounterNameAndTipFromPathSetting(settings[0].pathType),
                    settings[0].path,
                    testInitializer.pathManager.GetBudgetTypeFromSetting(settings[0].pathType),
                    null, 65536, settings[0].fileMask);

            //await Task.Delay(5000);

            logger.Info("File watcher test init.");
        }
        private void SetupWatcher(CheckupFileWatcher checkupFileWatcher, 
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> settings, int settingIndex, int bufferSize)
        {
            checkupFileWatcher.RunWatcher(
                ("", ""),
                //testInitializer.pathManager.GetCounterNameAndTipFromPathSetting(settings[settingIndex].pathType),
                    settings[settingIndex].path,
                    //testInitializer.pathManager.GetBudgetTypeFromSetting(settings[settingIndex].pathType),
                    CheckUpBlob.BudgetType.Rep,
                    null, bufferSize, settings[settingIndex].fileMask);
        }
        private async Task CopyFiles(int numOfFiles, 
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> settings, int settingIndex, string[] testFiles)
        {
            // Copy files into watcher folder.
            for (int i = 0; i < numOfFiles; i++)
            {
                var destFile = Path.Combine(settings[settingIndex].path, Path.GetFileName(testFiles[i]));

                logger.Info($"Copy {testFiles[i]} to {destFile}");

                File.Copy(testFiles[i], destFile, true);
            }

            await Task.Delay(1000);
        }
        [Test]
        [TestCaseSource(typeof(TestCases), nameof(TestCases.FileWatcherTestCases))]
        public async Task TestWatchers((int watcherIndex, int settingIndex, int col, int row, int bufferSize) testCase)
        {
            int settingIndex = testCase.settingIndex;
            var settings = testInitializer.pathsToInit;
            int bufferSize = testCase.bufferSize;
            int col = testCase.col;
            int row = testCase.row;
            var tlp = testInitializer.formHelper.CreateTableLayoutForFileCounters();
            string dir = settings[settingIndex].path;

            /*
            checkupFileWatcher.RunWatcher(
                    testInitializer.pathManager.GetCounterNameAndTipFromPathSetting(settings[settingIndex].pathType),
                    settings[settingIndex].path,
                    testInitializer.pathManager.GetBudgetTypeFromSetting(settings[settingIndex].pathType),
                    null, bufferSize, settings[settingIndex].fileMask);
            */
            
            var testFiles = Directory.GetFiles(testInitializer.sourceDirPath, "*.*", SearchOption.AllDirectories);

            int numOfFiles = 1;

            await Task.FromResult(CopyFiles(numOfFiles, settings, settingIndex, testFiles));
            //await CopyFiles(numOfFiles, settings, settingIndex, testFiles);

            //logger.Info($"Total test files: {numOfFiles} - watcher count: {checkupFileWatcher.CurrentFilesInFolder}");

            Assert.AreEqual(numOfFiles, checkupFileWatcher.counter);

            //Assert.That(() => numOfFiles == checkupFileWatcher.CurrentFilesInFolder, Is.True.After(5).Seconds.PollEvery(500).MilliSeconds);

            logger.Info($"Total test files: {numOfFiles} - watcher count: {checkupFileWatcher.counter}");
        }
        [TearDown]
        public void Cleanup()
        {
            testInitializer.ClearDirs();
        }
    }
}