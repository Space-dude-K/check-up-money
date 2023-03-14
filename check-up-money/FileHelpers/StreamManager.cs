using check_up_money.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace check_up_money.FileHelpers
{
    class StreamManager : IStreamManager
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private readonly IFileLockHelper flh;
        private readonly int copyMaxiumRetries;
        private readonly int copyTimeOutDelay;

        public StreamManager(IFileLockHelper flh, int copyMaxiumRetries, int copyTimeOutDelay)
        {
            loggerDebug.Debug("Init.");

            this.flh = flh;
            this.copyMaxiumRetries = copyMaxiumRetries;
            this.copyTimeOutDelay = copyTimeOutDelay;
        }

        public async Task<FileStream> WaitForFileStream(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            int currentTries = 0;
            FileStream fs = null;

            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(copyTimeOutDelay)).Token;

            loggerDebug.Debug($"Timeout -> {TimeSpan.FromSeconds(copyTimeOutDelay)} - max retr: {copyMaxiumRetries} - time out delay: {copyTimeOutDelay} ");

            while (true)
            {
                loggerDebug.Debug($"Stream reader current attempts: {currentTries} for {fullPath}");

                if (cancellationToken.IsCancellationRequested)
                {
                    logger.Info($"Timeout {copyTimeOutDelay} for {fullPath}");
                    loggerError.Error($"Timeout {copyTimeOutDelay} for {fullPath}");
                    break;
                }

                if (currentTries == copyMaxiumRetries)
                {
                    logger.Info($"Maxium tries for {fullPath} was reached!");
                    loggerError.Error($"Maxium tries for {fullPath} was reached!");
                    break;
                }

                try
                {
                    // File is ready to use.
                    fs = new FileStream(fullPath, mode, access, share);

                    return fs;
                }
                catch (IOException)
                {
                    logger.Info($"File {fullPath} is locked. Attempts remaining: {copyMaxiumRetries - currentTries}");

                    if (fs != null)
                    {
                        fs.Dispose();
                    }

                    // Trying to get the processes causing the blocking.
                    foreach (var proccess in flh.FindLockerProcesses(fullPath))
                    {
                        logger.Info($"File was blocked by: {proccess.strAppName} {proccess.strServiceShortName}");
                        loggerError.Error($"File was blocked by: {proccess.strAppName} {proccess.strServiceShortName}");
                    }

                    await Task.Delay(500);

                    // throw new Exception("Ошибка копирования. Файл заблокирован.");
                }
                catch (Exception ex)
                {
                    loggerError.Error($"Copy stream error: {ex.Message}");
                }

                await Task.Delay(500);

                currentTries++;
            }

            // The file is accessible now.
            return fs;
        }
    }
}
