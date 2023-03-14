using check_up_money.Settings.Budget;
using check_up_money.Settings.Currency;
using check_up_money.Settings.DataBase;
using check_up_money.Settings.Path;
using check_up_money.Settings.PathMisc;
using check_up_money.Settings.Ticket;
using check_up_money.Settings.UnsentFiles;
using System.Configuration;

namespace check_up_money.Settings
{
    class SettingsConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("budgetSettings", IsDefaultCollection = false)]
        public BudgetObjectElementCollection BudgetSettings
        {
            get { return ((BudgetObjectElementCollection)(base["budgetSettings"])); }
        }
        [ConfigurationProperty("currencySettings", IsDefaultCollection = false)]
        public CurrencyObjectElementCollection CurrencySettings
        {
            get { return ((CurrencyObjectElementCollection)(base["currencySettings"])); }
        }
        [ConfigurationProperty("ticketSettings", IsDefaultCollection = false)]
        public TicketObjectElementCollection TicketSettings
        {
            get { return ((TicketObjectElementCollection)(base["ticketSettings"])); }
        }
        [ConfigurationProperty("unsentFilesSettings", IsDefaultCollection = false)]
        public UnsentFilesObjectElementCollection UnsentFilesSettings
        {
            get { return ((UnsentFilesObjectElementCollection)(base["unsentFilesSettings"])); }
        }
        [ConfigurationProperty("dataBaseSettings", IsDefaultCollection = false)]
        public DataBaseObjectElementCollection DbSettings
        {
            get { return ((DataBaseObjectElementCollection)(base["dataBaseSettings"])); }
        }
        [ConfigurationProperty("pathSettings", IsDefaultCollection = false)]
        public PathObjectElementCollection PathSettings
        {
            get { return ((PathObjectElementCollection)(base["pathSettings"])); }
        }
        [ConfigurationProperty("pathSettingsMisc", IsDefaultCollection = false)]
        public PathMiscObjectElementCollection PathSettingsMisc
        {
            get { return ((PathMiscObjectElementCollection)(base["pathSettingsMisc"])); }
        }
        [ConfigurationProperty("loggerDir", IsDefaultCollection = false)]
        public string LoggerDir
        {
            get { return (string)this["loggerDir"]; }
            set { this["loggerDir"] = value; }
        }
        [ConfigurationProperty("loggerBackupDir", IsDefaultCollection = false)]
        public string LoggerBackupDir
        {
            get { return (string)this["loggerBackupDir"]; }
            set { this["loggerBackupDir"] = value; }
        }
        [ConfigurationProperty("mainDb", IsDefaultCollection = false)]
        public string MainDb
        {
            get { return (string)this["mainDb"]; }
            set { this["mainDb"] = value; }
        }
        [ConfigurationProperty("archiveBackupDir", IsDefaultCollection = false)]
        public string ArchiveBackupDir
        {
            get { return (string)this["archiveBackupDir"]; }
            set { this["archiveBackupDir"] = value; }
        }
        [ConfigurationProperty("fileBackup", IsDefaultCollection = false)]
        public string FileBackup
        {
            get { return (string)this["fileBackup"]; }
            set { this["fileBackup"] = value; }
        }
        [ConfigurationProperty("isBudgetLinkEnabled", IsDefaultCollection = false)]
        public string IsBudgetLinkEnabled
        {
            get { return (string)this["isBudgetLinkEnabled"]; }
            set { this["isBudgetLinkEnabled"] = value; }
        }
        [ConfigurationProperty("periodicNotificationDelayInMinutes", IsDefaultCollection = false)]
        public string PeriodicNotificationDelayInMinutes
        {
            get { return (string)this["periodicNotificationDelayInMinutes"]; }
            set { this["periodicNotificationDelayInMinutes"] = value; }
        }
        [ConfigurationProperty("balloonTipTimePeriodInSeconds", IsDefaultCollection = false)]
        public string BalloonTipTimePeriodInSeconds
        {
            get { return (string)this["balloonTipTimePeriodInSeconds"]; }
            set { this["balloonTipTimePeriodInSeconds"] = value; }
        }
        [ConfigurationProperty("maxiumCopyRetries", IsDefaultCollection = false)]
        public string MaxiumCopyRetries
        {
            get { return (string)this["maxiumCopyRetries"]; }
            set { this["maxiumCopyRetries"] = value; }
        }
        [ConfigurationProperty("copyTimeoutInSec", IsDefaultCollection = false)]
        public string CopyTimeoutInSec
        {
            get { return (string)this["copyTimeoutInSec"]; }
            set { this["copyTimeoutInSec"] = value; }
        }
        [ConfigurationProperty("watcherBufferSizeInKb", IsDefaultCollection = false)]
        public string WatcherBufferSizeInKb
        {
            get { return (string)this["watcherBufferSizeInKb"]; }
            set { this["watcherBufferSizeInKb"] = value; }
        }
        [ConfigurationProperty("startInTray", IsDefaultCollection = false)]
        public string StartInTray
        {
            get { return (string)this["startInTray"]; }
            set { this["startInTray"] = value; }
        }
        [ConfigurationProperty("isFileHandlerEnabled", IsDefaultCollection = false)]
        public string IsFileHandlerEnabled
        {
            get { return (string)this["isFileHandlerEnabled"]; }
            set { this["isFileHandlerEnabled"] = value; }
        }
        [ConfigurationProperty("directoryStatusCheckerTimeout", IsDefaultCollection = false)]
        public string DirectoryStatusCheckerTimeout
        {
            get { return (string)this["directoryStatusCheckerTimeout"]; }
            set { this["directoryStatusCheckerTimeout"] = value; }
        }
        [ConfigurationProperty("windowLocation", IsDefaultCollection = false)]
        public string WindowLocation
        {
            get { return (string)this["windowLocation"]; }
            set { this["windowLocation"] = value; }
        }
        [ConfigurationProperty("windowSize", IsDefaultCollection = false)]
        public string WindowSize
        {
            get { return (string)this["windowSize"]; }
            set { this["windowSize"] = value; }
        }
    }
}