using NLog;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using check_up_money.Extensions;

namespace check_up_money_test
{
    [TestFixture]
    public class FileProcesingTest
    {
        readonly Logger logger = LogManager.GetLogger("CheckUpTest-logger-info");

        TestInitializer testInitializer;

        [SetUp]
        [Obsolete]
        public void Init()
        {
            testInitializer = new();
            testInitializer.Init();
            logger.Info("File procesing test init.");
        }
        [Test]
        [TestCaseSource(typeof(TestCases), nameof(TestCases.FileProcessingTestCases))]
        public async Task CopyFiles((string pathType, string fileName, string expectedFileType, string expectedDir, int foreignCurrencyOverride) testCase)
        {
            var pathToInit = testInitializer.pathsToInit.Single(pti => pti.pathType.Equals(testCase.pathType));
            var sourceDir = pathToInit.path + "\\";
            var sourceFilePath = Path.Combine(sourceDir, testCase.fileName);
            var sourceFilesSize = TestHelpers.FileSize(sourceFilePath);
            var budget = testInitializer.pathManager.GetBudgetTypeFromSetting(testCase.pathType);
            var expectedDir = Path.Combine(testInitializer.destDirPath, testCase.expectedDir);
            var expectedFilePath = Path.Combine(expectedDir, testCase.fileName);
            var archiveFilePath = Path.Combine(testInitializer.archivePath, testCase.fileName);

            logger.Info($"Source: {sourceDir} expectedDir: {expectedDir}");
            logger.Info($"File ({testCase.fileName}) budget: {budget} isForeignCurrencyMode: {testCase.foreignCurrencyOverride} size: {sourceFilesSize}");

            var cb = await testInitializer.fileParser.ParseFile(sourceDir, testCase.fileName, budget);

            testInitializer.checkUpBlobs.Add(cb);

            var res = await testInitializer.fileHandler.ProccessTheFileNew(0, testCase.foreignCurrencyOverride);

            var archiveFileSize = TestHelpers.FileSize(archiveFilePath);
            var isArchiveFileExist = File.Exists(archiveFilePath);
            var expectedFileSize = TestHelpers.FileSize(expectedFilePath);
            var isExpectedFileExist = File.Exists(expectedFilePath);

            Assert.AreEqual(true, res);

            // Test archive copy.
            Assert.AreEqual(true, isArchiveFileExist);
            Assert.AreEqual(sourceFilesSize, archiveFileSize);

            // Test file type.
            Assert.AreEqual(testCase.expectedFileType, cb.CheckUpFileType.GetEnumDescription());

            Assert.AreEqual(true, isExpectedFileExist);
            Assert.AreEqual(sourceFilesSize, expectedFileSize);
        }
        [TearDown]
        public void Cleanup()
        {
            testInitializer.ClearDirs();
        } 
    }
}