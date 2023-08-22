using check_up_money.CheckUpFile;
using check_up_money.Extensions;
using check_up_money.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money
{
    internal class NotificationManager : INotificationManager
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private BindingList<CheckUpBlob> notificationData;
        public BindingList<CheckUpBlob> NotificationData
        {
            get
            {
                return notificationData;
            }
            set
            {
                notificationData = value;
            }
        }

        private NotifyIcon notifyIconForMainStatus;

        private readonly IMain main;
        private readonly int periodicNotificationDelayInMinutes;
        private readonly IUnsentFileChecker unsentFileChecker;
        private readonly IDirectoryStatusChecker directoryStatusChecker;
        private readonly int balloonTipTimePeriodInMilliseconds;

        public NotificationManager(IMain main, int periodicNotificationDelayInMinutes, int balloonTipTimePeriodInSeconds, 
            IUnsentFileChecker unsentFileChecker, IDirectoryStatusChecker directoryStatusChecker, 
            NotifyIcon notifyIconForMainStatus)
        {
            loggerDebug.Debug("Init.");
            this.main = main;
            this.periodicNotificationDelayInMinutes = periodicNotificationDelayInMinutes;
            this.unsentFileChecker = unsentFileChecker;
            this.directoryStatusChecker = directoryStatusChecker;
            this.notifyIconForMainStatus = notifyIconForMainStatus;
            this.balloonTipTimePeriodInMilliseconds = (int)TimeSpan.FromSeconds(balloonTipTimePeriodInSeconds).TotalMilliseconds;
            InitNotifyIconForNormalStatus();
        }
        private void InitNotifyIconForNormalStatus()
        {
            notifyIconForMainStatus.MouseDoubleClick += main.NotifyIcon_MouseDoubleClick;
            notifyIconForMainStatus.BalloonTipClicked += main.NotifyIconForMainStatus_BalloonTipClicked;
            notifyIconForMainStatus.BalloonTipIcon = ToolTipIcon.Info;
            notifyIconForMainStatus.BalloonTipTitle = "CheckUpMoney";
            notifyIconForMainStatus.Icon = check_up_money.Properties.Resources.Money;
            notifyIconForMainStatus.Text = main.AboutString;
            notifyIconForMainStatus.Visible = true;
        }
        public void ShowTrayNotificationForMinimizedState()
        {
            string notificationTitle = "Внимание!";
            string notificationText = "CheckUpMoney свёрнут.";

            notifyIconForMainStatus.ShowBalloonTip(balloonTipTimePeriodInMilliseconds, 
                notificationTitle, notificationText, ToolTipIcon.Info);
        }
        public void DisableTrayNotificationForMainState()
        {
            if (notifyIconForMainStatus != null)
                notifyIconForMainStatus.Dispose();
        }
        private void ShowTrayNotification(int filesCount, int outFilesCount, 
            BindingList<CheckUpBlob> notificationData = null, string fileName = null)
        {

            if (filesCount > 1)
            {
                ShowTrayNotificationForProcessingFiles(notificationData);
            }
            else if (filesCount == 1)
            {
                ShowTrayNotificationForProcessingFile(notificationData.First().FileName);
            }
            else if(unsentFileChecker.UnsentFilesCounters.Any(ufc => ufc > 0))
            {
                ShowTrayNotificationForUnsentFiles();
            }
        }
        public void ShowTrayNotificationForProcessingFiles(BindingList<CheckUpBlob> notificationData)
        {
            int notificationFilesCount = notificationData.Count;
            string notificationTitle = "Внимание!";
            string notificationText = 
                notificationFilesCount.GetDeclension("Доступен", "Доступно", "Доступно") 
                + " " + notificationFilesCount 
                + " " + notificationFilesCount.GetDeclension("файл", "файла", "файлов") + " на обработку.";

            StringBuilder sb = new StringBuilder();
            sb.Append(notificationText);

            if(notificationData.Count > 0)
                notifyIconForMainStatus.ShowBalloonTip(balloonTipTimePeriodInMilliseconds, notificationTitle, 
                    sb.ToString() + System.Environment.NewLine + 
                    GetTextForUnsentFilesNotification(unsentFileChecker.UnsentFilesCounters), ToolTipIcon.Info);
        }
        public void ShowTrayNotificationForProcessingFile(string fileName)
        {
            string notificationTitle = "Внимание!";
            string notificationText = "Доступен следующий файл на обработку: ";

            StringBuilder sb = new StringBuilder();
            sb.Append(notificationText);
            sb.Append(fileName);

            notifyIconForMainStatus.ShowBalloonTip(balloonTipTimePeriodInMilliseconds, notificationTitle, 
                sb.ToString() + System.Environment.NewLine 
                + GetTextForUnsentFilesNotification(unsentFileChecker.UnsentFilesCounters), ToolTipIcon.Info);
        }
        private void ShowTrayNotificationForUnsentFiles()
        {
            string notificationTitle = "Внимание!";

            StringBuilder sb = new StringBuilder();
            sb.Append(GetTextForUnsentFilesNotification(unsentFileChecker.UnsentFilesCounters));

            notifyIconForMainStatus.ShowBalloonTip(balloonTipTimePeriodInMilliseconds, notificationTitle,
                sb.ToString() + System.Environment.NewLine 
                + GetTextForUnsentFilesNotification(unsentFileChecker.UnsentFilesCounters), ToolTipIcon.Info);
        }
        private string GetNotificationTextPartForInFiles(int inFilesCount)
        {
            return inFilesCount.GetDeclension("Доступен", "Доступно", "Доступно")
                + " " + inFilesCount
                + " " + inFilesCount.GetDeclension("файл", "файла", "файлов") + " на обработку.";
        }
        private string GetNotificationTextForOutLtAndCbFiles(
            int inFilesCount, int outCbFiles, int inTicketFiles, int outTicketFiles)
        {
            string mixedText = string.Empty;
            int outFilesForCb = outCbFiles + outTicketFiles;

            if(inFilesCount > 0 && inTicketFiles == 0 && outCbFiles == 0)
            {
                mixedText = "Обработка в CM: " + inFilesCount;
            }
            else if (inFilesCount > 0 && inTicketFiles > 0 && outCbFiles > 0)
            {
                mixedText = "Обработка в CM: " + inFilesCount + ", в LT: " + inTicketFiles + ", в CB: " + outFilesForCb;
            }
            else if (inFilesCount > 0 && inTicketFiles > 0 && outCbFiles == 0)
            {
                mixedText = "Обработка в CM: " + inFilesCount + ", в LT: " + inTicketFiles;
            }
            else if (inFilesCount > 0 && inTicketFiles == 0 && outCbFiles > 0)
            {
                mixedText = "Обработка в CM: " + inFilesCount + ", в CB: " + outFilesForCb;
            }
            else if (inFilesCount == 0 && inTicketFiles > 0 && outCbFiles > 0)
            {
                mixedText = "Обработка в LT: " + inTicketFiles + ", в CB: " + outFilesForCb;
            }
            else if (inFilesCount == 0 && inTicketFiles > 0 && outFilesForCb == 0)
            {
                mixedText = "Обработка в LT: " + inTicketFiles;
            }
            else if(inFilesCount == 0 && inTicketFiles == 0 && outFilesForCb > 0)
            {
                mixedText = "Обработка в CB: " + outFilesForCb;
            }

            return mixedText;
        }
        private string GetTextForUnsentFilesNotification(AsyncObservableCollection<int> unsentFilesCounters)
        {
            string unsentFilesNotificationString = string.Empty;
            StringBuilder sb = new StringBuilder();
            List<string> budgets = new List<string>() { "r", "o", "c", "rg", "u", "e" };

            if (unsentFilesCounters.Any(ufc => ufc > 0))
            {
                sb.Append("На отправку (LT) rp-ob-ct-rg-un-ex: " + Environment.NewLine);

                for (int i = 0; i < unsentFilesCounters.Count; i++)
                {
                    //sb.Append(budgets[i] + ": " + unsentFilesCounters[i]);
                    sb.Append(unsentFilesCounters[i]);

                    if (i != unsentFilesCounters.Count - 1)
                        sb.Append("-");
                }

                unsentFilesNotificationString = sb.ToString();
            }

            return unsentFilesNotificationString;
        }
        private string GenerateNotificationString(int inFilesCount, int outCbFiles, 
            int inTicketFiles, int outTicketFiles, AsyncObservableCollection<int> unsentFilesCounters)
        {
            logger.Info($"Notification data: {inFilesCount} " +
                $"- {outCbFiles} - {inTicketFiles} - {outTicketFiles} - {unsentFilesCounters.Where(c => c > 0).Count()}");

            StringBuilder sb = new StringBuilder();

            int totalFiles = inFilesCount + outCbFiles + inTicketFiles + outTicketFiles;

            sb.Append(totalFiles > 0 ? GetNotificationTextPartForInFiles(totalFiles) + System.Environment.NewLine : null);
            sb.Append(totalFiles > 0 ? 
                GetNotificationTextForOutLtAndCbFiles(inFilesCount, outCbFiles, 
                inTicketFiles, outTicketFiles) + System.Environment.NewLine : null);

            sb.Append(GetTextForUnsentFilesNotification(unsentFilesCounters));

            return sb.ToString();
        }
        private void ShowNotificationAll(int inFilesCount, AsyncObservableCollection<int> unsentFilesCounters)
        {
            string notificationTitle = "Внимание!";
            var fc = FilterCounters(directoryStatusChecker.GetFileCounterForOutFolders());

            if (inFilesCount > 0 || fc.outCbFiles > 0 
                || fc.inTicketFiles > 0 || fc.outTicketFiles > 0 || unsentFilesCounters.Any(c => c > 0))
            {
                notifyIconForMainStatus.ShowBalloonTip(balloonTipTimePeriodInMilliseconds, notificationTitle,
                GenerateNotificationString(fc.mainInFiles, fc.outCbFiles, 
                fc.inTicketFiles, fc.outTicketFiles, unsentFilesCounters), ToolTipIcon.Info);
            }
        }
        private (int mainInFiles, int outCbFiles, 
            int inTicketFiles, int outTicketFiles) FilterCounters(List<(string pathType, int filesInDir)> dirCounters)
        {
            int mainInFiles = 0;
            int outCbFiles = 0;
            int inTicketFiles = 0;
            int outTicketFiles = 0;

            var processedFilesPathTypes = new List<string> 
            { 
                "repIn", "oblIn", "cityIn", "regIn", "uniIn", "extIn" 
            };
            mainInFiles = dirCounters.Where(dc => processedFilesPathTypes.Any(pt => pt.Equals(dc.pathType))).Sum(fc => fc.filesInDir);

            var outCbPathTypes = new List<string>()
            {
                "repBankMainOut", "oblBankMainOut", "cityBankMainOut", "regBankMainOut", "uniBankMainOut", "extBankMainOut",
                "repBankMiscOut", "oblBankMiscOut", "cityBankMiscOut", "regBankMiscOut", "uniBankMiscOut", "extBankMiscOut",
                "repBankMainOutCurrency", "oblBankMainOutCurrency", 
                "cityBankMainOutCurrency", "regBankMainOutCurrency", "uniBankMainOutCurrency", "extBankMainOutCurrency",
                "repBankMiscOutCurrency", "oblBankMiscOutCurrency", 
                "cityBankMiscOutCurrency", "regBankMiscOutCurrency", "uniBankMiscOutCurrency", "extBankMiscOutCurrency"
            };
            outCbFiles = dirCounters.Where(dc => outCbPathTypes.Any(pt => pt.Equals(dc.pathType))).Sum(fc => fc.filesInDir);

            var inTicketPathTypes = new List<string>
            {
                "repBankTicketIn", "oblBankTicketIn", "cityBankTicketIn", "regBankTicketIn","uniBankTicketIn", "extBankTicketIn"
            };
            inTicketFiles = dirCounters.Where(dc => inTicketPathTypes.Any(pt => pt.Equals(dc.pathType))).Sum(fc => fc.filesInDir);

            var outTicketPathTypes = new List<string>
            {
                "repBankTicketOut", "oblBankTicketOut", "cityBankTicketOut", "regBankTicketOut",
                "uniBankTicketOut", "extBankTicketOut",
                "repBankTicketOutCurrency", "oblBankTicketOutCurrency", "cityBankTicketOutCurrency", 
                "regBankTicketOutCurrency","uniBankTicketOutCurrency", "extBankTicketOutCurrency"
            };
            outTicketFiles = dirCounters.Where(dc => outTicketPathTypes.Any(pt => pt.Equals(dc.pathType))).Sum(fc => fc.filesInDir);


            loggerDebug.Debug($"Filtered file counters. main in: {mainInFiles} " +
                $"- outCb: {outCbFiles} - inTicket: {inTicketFiles} - outTicket: {outTicketFiles} ");

            return (mainInFiles, outCbFiles, inTicketFiles, outTicketFiles);
        }
        public async Task RunNotificationReminderForFiles(CancellationTokenSource source, 
            BindingList<CheckUpBlob> notificationData = null)
        {
            // Define the cancellation token.
            CancellationToken token = source.Token;
            this.notificationData = notificationData;

            logger.Info($"Starting notification reminder for files. Repeat time: {periodicNotificationDelayInMinutes}");

            try
            {
                while(!token.IsCancellationRequested)
                {
                    // Wait for timer.
                    await Task.Delay(new TimeSpan(0, periodicNotificationDelayInMinutes, 0));

                    // Run notification.
                    ShowNotificationAll(notificationData.Count, unsentFileChecker.UnsentFilesCounters);
                }
            }
            catch (OperationCanceledException)
            {
                logger.Info($"Notification reminder was stopped by cancelation request.");
            }
            catch (Exception ex)
            {
                loggerError.Error($"{ex.Message}");
            }
        }
    }
}