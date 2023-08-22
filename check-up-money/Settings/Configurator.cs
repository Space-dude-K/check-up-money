using check_up_money.Cypher;
using check_up_money.Interfaces;
using check_up_money.Settings.Budget;
using check_up_money.Settings.Currency;
using check_up_money.Settings.DataBase;
using check_up_money.Settings.Path;
using check_up_money.Settings.Ticket;
using check_up_money.Settings.UnsentFiles;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace check_up_money.Settings
{
    public class Configurator : IConfigurator
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        string mainSettingsConfigPath;
        ICypher cypher;
        public Configurator(ICypher cypher, string mainSettingsConfigPath)
        {
            this.mainSettingsConfigPath = mainSettingsConfigPath;
            this.cypher = cypher;
        }
        public Configurator(ICypher cypher)
        {
            mainSettingsConfigPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Settings", "MainSettings.config");
            this.cypher = cypher;
        }
        #region Budget, Bd, Path, Logger settings
        private ConfigurationSection LoadConfigByPath(string path, string section)
        {
            System.Configuration.Configuration configuration;

            try
            {
                // Path to your config file
                System.Configuration.ConfigurationFileMap fileMap = new ConfigurationFileMap(path);
                configuration = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(fileMap);

                if (configuration.GetSection(section) == null)
                {
                    loggerError.Error($"Configuration section {section} is null.");
                    throw new NullReferenceException($"Configuration section {section} is null.");
                }
                    
            }
            catch (Exception ex)
            {
                loggerError.Error(ex);
                throw;
            }

            return configuration.GetSection(section);
        }
        private void UpdateConfig(string section)
        {
            try
            {
                ConfigurationManager.RefreshSection(section); 
            }
            catch (Exception ex)
            {
                loggerError.Error(ex);
                throw;
            }
        }
        #region Budget settigns
        public List<(string budgetType, bool isEnabled)> LoadBudgetSettings()
        {
            List<(string budgetType, bool isEnabled)> _budgetSettings = new List<(string budgetType, bool isEnabled)>();

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (BudgetObjectElement boe in myConfig.BudgetSettings)
            {
                bool isBudgetEnabled;
                bool.TryParse(CheckSetting(boe.IsEnabled), out isBudgetEnabled);

                _budgetSettings.Add((CheckSetting(boe.BudgetType), isBudgetEnabled));
            }

            return _budgetSettings;
        }
        public void WriteBudgetSettings(List<(string budgetType, bool isEnabled)> budgetSettings)
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (var budgetSetting in budgetSettings)
            {
                myConfig.BudgetSettings[budgetSetting.budgetType].IsEnabled = budgetSetting.isEnabled.ToString();
            }

            myConfig.CurrentConfiguration.Save();
        }
        #endregion
        #region Currency settigns
        public List<(string budgetType, bool isEnabled)> LoadCurrencySettings()
        {
            List<(string budgetType, bool isEnabled)> _currencySettings = new List<(string budgetType, bool isEnabled)>();

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (CurrencyObjectElement boe in myConfig.CurrencySettings)
            {
                bool isCurrencyEnabled;
                bool.TryParse(CheckSetting(boe.IsEnabled), out isCurrencyEnabled);

                _currencySettings.Add((CheckSetting(boe.BudgetType), isCurrencyEnabled));
            }

            return _currencySettings;
        }
        public void WriteCurrencySettings(List<(string budgetType, bool isEnabled)> currencySettings)
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (var currencySetting in currencySettings)
            {
                myConfig.CurrencySettings[currencySetting.budgetType].IsEnabled = currencySetting.isEnabled.ToString();
            }

            myConfig.CurrentConfiguration.Save();
        }
        #endregion
        #region Ticket settings
        public List<(string ticketType, bool isEnabled)> LoadTicketSettings()
        {
            List<(string ticketType, bool isEnabled)> _ticketSettings = new List<(string ticketType, bool isEnabled)>();

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (TicketObjectElement toe in myConfig.TicketSettings)
            {
                bool isTicketEnabled;
                bool.TryParse(CheckSetting(toe.IsTicketEnabled), out isTicketEnabled);

                _ticketSettings.Add((CheckSetting(toe.TicketType), isTicketEnabled));
            }

            return _ticketSettings;
        }
        public void WriteTicketSettings(List<(string ticketType, bool isEnabled)> ticketSettings)
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (var ticketSetting in ticketSettings)
            {
                myConfig.TicketSettings[ticketSetting.ticketType].IsTicketEnabled = ticketSetting.isEnabled.ToString();
            }

            myConfig.CurrentConfiguration.Save();
        }
        #endregion
        #region Unsent files settings
        public List<(string budgetType, bool isEnabled)> LoadUnsentFilesSettings()
        {
            List<(string budgetType, bool isEnabled)> _unsentFilesSettings = new List<(string budgetType, bool isEnabled)>();

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (UnsentFilesObjectElement ufoe in myConfig.UnsentFilesSettings)
            {
                bool isEnabled;
                bool.TryParse(CheckSetting(ufoe.IsEnabled), out isEnabled);

                _unsentFilesSettings.Add((CheckSetting(ufoe.BudgetType), isEnabled));
            }

            return _unsentFilesSettings;
        }
        public void WriteUnsentFilesSettings(List<(string budgetType, bool isEnabled)> unsentFileSettings)
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (var unsentFileSetting in unsentFileSettings)
            {
               myConfig.UnsentFilesSettings[myConfig.UnsentFilesSettings.GetIndex(unsentFileSetting.budgetType)].IsEnabled = unsentFileSetting.isEnabled.ToString();
            }

            myConfig.CurrentConfiguration.Save();
        }
        public int LoadUnsentFilesCheckerDelay()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            int delayInSeconds = 60;
            int.TryParse(CheckSetting(myConfig.UnsentFilesSettings.CheckDelayInSeconds), out delayInSeconds);

            return delayInSeconds;
        }
        #endregion
        #region Db settings
        public List<RequisiteInformation> LoadDbSettings()
        {
            List<RequisiteInformation> _dbSettings = new List<RequisiteInformation>();
            RequisiteInformation _reqInfo = new RequisiteInformation();

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            try
            {
                foreach (DataBaseObjectElement dboe in myConfig.DbSettings)
                {
                    _dbSettings.Add(new RequisiteInformation(
                        CheckSetting(dboe.DriverElement),
                        CheckSetting(dboe.HostElement),
                        CheckSetting(dboe.InstanceElement),
                        CheckSetting(dboe.DataBaseElement),
                        cypher.ToSecureString(CheckSetting(dboe.DataBaseElementUser)),
                        CheckSetting(dboe.DataBaseElementUserSalt),
                        cypher.ToSecureString(CheckSetting(dboe.DataBaseElementPassword)),
                        CheckSetting(dboe.DataBaseElementPasswordSalt)
                        ));
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Ошибка при загрузке настроек для баз данных. Обратитесь к администратору.");
            }

            return _dbSettings;
        }
        public void WriteDbSettings(List<RequisiteInformation> reqInfoList)
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach(RequisiteInformation ri in reqInfoList)
            {
                if (myConfig.DbSettings.Count > 0)
                {
                    AddDbSettings(ri, myConfig);
                }
            }
        }
        public void DeleteDbSettings(string hostToDelete)
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            Debug.WriteLine("Deleting host -> " + hostToDelete);
            myConfig.DbSettings.Remove(hostToDelete);

            myConfig.CurrentConfiguration.Save();
            UpdateConfig("settings");
        }
        private void AddDbSettings(RequisiteInformation reqInfo, SettingsConfiguration config)
        {
            if (config.DbSettings[reqInfo.Host] != null)
            {
                config.DbSettings[reqInfo.Host].DriverElement = reqInfo.Driver;
                config.DbSettings[reqInfo.Host].DataBaseElement = reqInfo.Db;
                config.DbSettings[reqInfo.Host].InstanceElement = reqInfo.Instance;
                config.DbSettings[reqInfo.Host].DataBaseElementUser = cypher.ToInsecureString(reqInfo.User);
                config.DbSettings[reqInfo.Host].DataBaseElementUserSalt = reqInfo.USalt;
                config.DbSettings[reqInfo.Host].DataBaseElementPassword = cypher.ToInsecureString(reqInfo.Password);
                config.DbSettings[reqInfo.Host].DataBaseElementPasswordSalt = reqInfo.PSalt;
            }
            else
            {
                config.DbSettings.Add(new DataBaseObjectElement(
                    reqInfo.Driver,
                    reqInfo.Host,
                    reqInfo.Instance,
                    reqInfo.Db,
                    cypher.ToInsecureString(reqInfo.User),
                    reqInfo.USalt,
                    cypher.ToInsecureString(reqInfo.Password),
                    reqInfo.PSalt
                    ));
            }

            config.CurrentConfiguration.Save();
            UpdateConfig("settings");
        }
        #endregion
        #region Path settings
        public List<(string pathType, string path)> LoadPathSettings()
        {
            List<(string pathType, string path)> pathSettings = new List<(string pathType, string path)>();

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (PathObjectElement poe in myConfig.PathSettings)
            {
                pathSettings.Add((CheckSetting(poe.PathTypeElement),CheckSetting(poe.PathElementValue)));
            }

            return pathSettings;
        }
        public void WritePathSettings(List<(string pathType, string path)> pathSettings)
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            foreach (var pathSetting in pathSettings)
            {
                myConfig.PathSettings[pathSetting.pathType].PathElementValue = pathSetting.path;
            }

            myConfig.CurrentConfiguration.Save();
        }
        #endregion
        #region Logger settings
        public string LoadLoggerDir()
        {
            string _loggerPathSetting;

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            _loggerPathSetting = CheckSetting(myConfig.LoggerDir);

            return _loggerPathSetting;
        }
        public string LoadLoggerBackupDir()
        {
            string _loggerPathSetting;

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            _loggerPathSetting = CheckSetting(myConfig.LoggerBackupDir);

            return _loggerPathSetting;
        }
        public string LoadMainDbAddress()
        {
            string _mainDbAddress;

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            _mainDbAddress = CheckSetting(myConfig.MainDb);

            return _mainDbAddress;
        }
        #endregion
        #region Notification settings
        public int LoadNotificationReminderSetting()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            return int.Parse(CheckSetting(myConfig.PeriodicNotificationDelayInMinutes));
        }
        public int LoadBalloonTipTimePeriodSetting()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            return int.Parse(CheckSetting(myConfig.BalloonTipTimePeriodInSeconds));
        }
        #endregion
        #region Misc settings
        public string LoadArchiveBackupSetting()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            return CheckSetting(myConfig.ArchiveBackupDir);
        }
        public bool LoadFileBackupSetting()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            bool isActive = false;
            bool.TryParse(CheckSetting(myConfig.FileBackup), out isActive);

            return isActive;
        }
        public bool LoadBudgetLinkSetting()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            return bool.Parse(CheckSetting(myConfig.IsBudgetLinkEnabled));
        }
        public Tuple<int, int> LoadCopySettings()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            int copyRetries = 8;
            int copyTimeout = 8;

            int.TryParse(CheckSetting(myConfig.MaxiumCopyRetries), out copyRetries);
            int.TryParse(CheckSetting(myConfig.CopyTimeoutInSec), out copyTimeout);

            return Tuple.Create(copyRetries, copyTimeout);
        }
        public int LoadBufferSizeForFileWatcher()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            int bufferSize = 8192;

            int.TryParse(CheckSetting(myConfig.WatcherBufferSizeInKb), out bufferSize);

            return bufferSize;
        }
        public bool LoadStartInTraySetting()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            bool startInTray = false;

            bool.TryParse(CheckSetting(myConfig.StartInTray), out startInTray);

            return startInTray;
        }
        public bool LoadFileHandlerSetting()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            bool isFileHandlerEnabled = false;

            bool.TryParse(CheckSetting(myConfig.IsFileHandlerEnabled), out isFileHandlerEnabled);

            return isFileHandlerEnabled;
        }
        public int LoadDirectoryStatusCheckerTimeout()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            int directoryStatusCheckerTimeout = 10;

            int.TryParse(CheckSetting(myConfig.DirectoryStatusCheckerTimeout), out directoryStatusCheckerTimeout);

            return directoryStatusCheckerTimeout;
        }
        public (Size windowSize, Point windowLocation) LoadWindowSizeAndLocation()
        {
            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            var rawPoint = Regex.Replace(CheckSetting(myConfig.WindowLocation), @"[{}XY=]|[\n]{2}", string.Empty).Split(",");

            loggerDebug.Debug(rawPoint[0] + " " + rawPoint[1]);

            int xLoc;
            int yLoc;

            if (string.IsNullOrWhiteSpace(rawPoint[0]) || int.TryParse(rawPoint[0], out xLoc) && xLoc <= 0)
            {
                xLoc = 572;
            }
            if (string.IsNullOrWhiteSpace(rawPoint[1]) || int.TryParse(rawPoint[1], out yLoc) && yLoc <= 0)
            {
                yLoc = 1;
            }

            loggerDebug.Debug($"Point: x - {xLoc}, y - {yLoc}");

            var rawSize = Regex.Replace(CheckSetting(myConfig.WindowSize), @"[{}XY=WidthHeight]|[\n]{2}", string.Empty).Split(",");

            loggerDebug.Debug(rawSize[0] + " " + rawSize[1]);

            int xSize;
            int ySize;

            if (string.IsNullOrWhiteSpace(rawSize[0]) || int.TryParse(rawSize[0], out xSize) && xSize <= 0)
            {
                xSize = 960;
            }
            if (string.IsNullOrWhiteSpace(rawSize[1]) || int.TryParse(rawSize[1], out ySize) && ySize <= 0)
            {
                ySize = 360;
            }

            loggerDebug.Debug($"Size: x - {xSize}, y - {ySize}");

            return (new Size(xSize, ySize), new Point(xLoc, yLoc));
        }
        public void SaveWindowSize(Size windowSize)
        {
            loggerDebug.Debug($"SaveWindowSize -> {windowSize.Height} - {windowSize.Width}");

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            myConfig.WindowSize = windowSize.ToString();

            myConfig.CurrentConfiguration.Save();
        }
        public void SaveWindowLocationToConfig(Point windowPoint)
        {
            loggerDebug.Debug($"SaveWindowLocation -> {windowPoint.X} - {windowPoint.Y}");

            SettingsConfiguration myConfig = (SettingsConfiguration)LoadConfigByPath(mainSettingsConfigPath, "settings");

            myConfig.WindowLocation = windowPoint.ToString();

            myConfig.CurrentConfiguration.Save();
        }
        #endregion
        private string CheckSetting(string setting)
        {
            if (string.IsNullOrEmpty(setting))
            {
                loggerError.Error($"Null setting exception for {setting}.");
                throw new Exception($"Null setting exception for {setting}.");
            }
            else
            {
                //Debug.WriteLine(setting);
                return setting;
            }
        }
        #endregion
    }
}