using check_up_money.Cypher;
using System;
using System.Collections.Generic;
using System.Drawing;
using static check_up_money.CheckUpFile.CheckUpBlob;

namespace check_up_money.Settings
{
    public interface IConfigurator
    {
        List<(string budgetType, bool isEnabled)> LoadBudgetSettings();
        void WriteBudgetSettings(List<(string budgetType, bool isEnabled)> budgetSettings);
        List<RequisiteInformation> LoadDbSettings();
        List<(string pathType, string path)> LoadPathSettings();
        void WriteDbSettings(List<RequisiteInformation> reqInfoList);
        void DeleteDbSettings(string hostToDelete);
        void WritePathSettings(List<(string pathType, string path)> pathSettings);
        int LoadNotificationReminderSetting();
        int LoadBalloonTipTimePeriodSetting();
        string LoadLoggerBackupDir();
        Tuple<int, int> LoadCopySettings();
        bool LoadStartInTraySetting();
        (Size windowSize, Point windowLocation) LoadWindowSizeAndLocation();
        void SaveWindowSize(Size windowSize);
        void SaveWindowLocationToConfig(Point windowPoint);
        string LoadArchiveBackupSetting();
        int LoadBufferSizeForFileWatcher();
        List<(string ticketType, bool isEnabled)> LoadTicketSettings();
        void WriteTicketSettings(List<(string ticketType, bool isEnabled)> ticketSettings);
        List<(string budgetType, bool isEnabled)> LoadUnsentFilesSettings();
        void WriteUnsentFilesSettings(List<(string budgetType, bool isEnabled)> unsentFileSettings);
        int LoadUnsentFilesCheckerDelay();
        int LoadDirectoryStatusCheckerTimeout();
        bool LoadFileBackupSetting();
        bool LoadFileHandlerSetting();
        string LoadMainDbAddress();
        List<(string budgetType, bool isEnabled)> LoadCurrencySettings();
        void WriteCurrencySettings(List<(string budgetType, bool isEnabled)> budgetSettings);
    }
}