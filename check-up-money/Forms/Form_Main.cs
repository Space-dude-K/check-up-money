using check_up_money.CheckUpFile;
using check_up_money.Cypher;
using check_up_money.Db;
using check_up_money.Extensions;
using check_up_money.FileHelpers;
using check_up_money.Forms;
using check_up_money.Interfaces;
using check_up_money.Settings;
using check_up_money.ValidatorsAndCheckers;
using Microsoft.Win32;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static check_up_money.CheckUpFile.CheckUpBlob;

[assembly: InternalsVisibleTo("check-up-money-test")]
namespace check_up_money
{
    public partial class Form_Main : Form, IMain
    {
        bool isTestingModeActive = false;

        private List<string> allowedFileExtensions = new List<string>() { ".P02", ".P01", ".38", ".36" };
        private readonly string aboutString = "CheckUpMoney " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Skoibeda K.S. 2022";

        public string loggerCfgPath;
        public string mainCfgPath;
        public string testBaseDir;

        public XmlLoggingConfiguration xmlLoggingConfiguration;

        string assemblyFolder;

        Logger sessionLogger = LogManager.GetLogger("CheckUpSession-logger-info");
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private string mainDbAddress;
        private RequisiteInformation mainDbRi;

        IFormHelper formHelper;
        ICypher cypher;
        IConfigurator conf;
        IPathsValidator pv;
        IRemoteHostAvailabilityChecker rhac;
        IFileParser fp;
        IFileHandler fh;
        INotificationManager nm;
        ISqlCmdExecutor sce;
        IStreamManager sm;
        IFileLockHelper flh;
        IUnsentFileChecker unsentFileChecker;
        IDirectoryStatusChecker directoryStatusChecker;
        internal TableLayoutPanel tlp;
        IPathManager pathManager;

        CancellationTokenSource ctsForNotificationManager;
        CancellationTokenSource ctsForUnsentFileChecker;
        CancellationTokenSource ctsForDirectoryStatusChecker;

        bool isFileBackupActive;
        string backupLoggerDir;
        string archiveBackupDir;

        List<(string budgetType, bool isEnabled)> budgetSettings;
        List<(string budgetType, bool isEnabled)> currencySettings;
        List<(string ticketType, bool isEnabled)> ticketSettings;
        List<(string budgetType, bool isEnabled)> unsentFileSettings;

        int unsentFileCheckerDelayInSeconds;
        List<(string pathType, string path)> pathSettings;
        List<RequisiteInformation> dbSettings;

        List<Tuple<string, bool>> checkedBudgets;
        Tuple<int, int> copySettings;
        int bufferSizeForFileWatcher;

        int periodicNotificationDelayInMinutes;
        int balloonTipTimePeriodInSeconds;

        bool startInTray;
        bool isFileHandlerEnabled;
        int directoryStatusCheckerTimeoutInSeconds;

        List<Task> initDirTasks = new List<Task>();

        // To prevent from gc cleanup.
        private List<CheckupFileWatcher> watchers;
        NotifyIcon notifyIconForMainStatus;
        ToolTip toolTipForLabels;

        public List<Tuple<string, bool>> CheckedBudgets { get => checkedBudgets; }
        public List<string> AllowedFileExtensions { get => allowedFileExtensions; set => allowedFileExtensions = value; }
        public string AboutString => aboutString;
        public List<(string pathType, string path)> PathSettings
        {
            get
            {
                return pathSettings;
            }

            set
            {
                pathSettings = value;
            }
        }
        private string selectedValue;
        BindingList<CheckUpBlob> checkUpBlobsBindingList;
        public BindingList<CheckUpBlob> CheckUpBlobsBindingList
        {
            get
            {
                return checkUpBlobsBindingList;
            }

            set
            {
                checkUpBlobsBindingList = value;
            }
        }
        public List<(string budgetType, bool isEnabled)> BudgetSettings
        {
            get
            {
                return budgetSettings;
            }

            set
            {
                budgetSettings = value;
            }
        }
        public List<(string budgetType, bool isEnabled)> CurrencySettings
        {
            get
            {
                return currencySettings;
            }

            set
            {
                currencySettings = value;
            }
        }
        public List<(string ticketType, bool isEnabled)> TicketSettings
        {
            get
            {
                return ticketSettings;
            }

            set
            {
                ticketSettings = value;
            }
        }
        internal IFileHandler Fh
        {
            get
            {
                return fh;
            }
        }
        List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit;
        public Form_Main()
        {
            InitLogger();
            CInit();
        }
        public Form_Main(bool isTestCase)
        {
            if(isTestCase)
            {
                InitLogger(loggerCfgPath);
                CInit();
            }
        }
        
        public Form_Main(string loggerCfgPath, string mainCfgPath, string testBaseDir, bool isTestingModeActive = false)
        {
            this.isTestingModeActive = isTestingModeActive;
            InitLogger(loggerCfgPath);
            this.mainCfgPath = mainCfgPath;
            this.testBaseDir = testBaseDir;
            CInit();
        }
        #region Init
        private async void CInit()
        {
            InitializeComponent();
            
            InitBase();
            InitMainForm();

            if(!isTestingModeActive)
                await InitTableLayoutPanel();

            Start();
        }
        private void InitLogger(string loggerCfgPath = null)
        {
            if(loggerCfgPath != null)
            {
                // Load nlog config
                xmlLoggingConfiguration = new XmlLoggingConfiguration(loggerCfgPath, true);
            }
            else
            {
                // Load nlog config
                assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                xmlLoggingConfiguration = new XmlLoggingConfiguration(assemblyFolder + "\\Settings\\NLog.config", true);
            }

            NLog.LogManager.Configuration = xmlLoggingConfiguration;

            this.loggerCfgPath = loggerCfgPath;
            logger.Info($"Logger init: {loggerCfgPath}");

            sessionLogger.
                Info($"<=======================================================================================================================================================================================>");
        }
        private void InitBase()
        {
            // Handle the ApplicationExit event to know when the application is exiting.
            SystemEvents.SessionEnding += new SessionEndingEventHandler(this.OnApplicationExit);
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            formHelper = new FormHelper();
            cypher = new Encryptor();
            conf = string.IsNullOrEmpty(mainCfgPath) ? new Configurator(cypher) : new Configurator(cypher, mainCfgPath);

            pv = new PathsValidator();

            LoadBaseConfigs();

            rhac = new RemoteHostAvailabilityChecker();
            flh = new FileLockHelper();
            sm = new StreamManager(flh, copySettings.Item1, copySettings.Item2);
            fp = new FileParser(flh, allowedFileExtensions, sm);
        }
        public void Start()
        {
            loggerDebug.Debug($"Startup check.");

            bool isAllSettingsValid = true;

            logger.Info($"Settings status: {isAllSettingsValid}");

            if (isAllSettingsValid)
            {
                Init();
            }
            else
            {
                loggerError.Error($"Ошибка в заданных настройках.");
                ShowErrorBox();
            }
        }
        [Obsolete]
        public void Init()
        {
            LoadPathConfigs();

            sce = new SqlCmdExecutor(rhac, mainDbAddress, new DbConnector(mainDbRi, cypher));

            InitToolTipForLabels();
            unsentFileChecker = new UnsentFileChecker(toolTipForLabels, sce, cypher, rhac);
            directoryStatusChecker = new DirectoryStatusChecker(this, toolTipForLabels, pathManager, pathsToInit);

            InitNotifyIcon();

            nm = new NotificationManager(this, 
                periodicNotificationDelayInMinutes, balloonTipTimePeriodInSeconds, unsentFileChecker, directoryStatusChecker, notifyIconForMainStatus);
            //GetFileCounterForOutFolders();

            fh = new FileHandler(this, fp, pathsToInit,
                sm, sce, 
                new DbConnector(new List<RequisiteInformation>() { mainDbRi }, cypher, true), pathManager, rhac, archiveBackupDir, isFileBackupActive, mainDbAddress);
            checkedBudgets = new List<Tuple<string, bool>>();

            if (watchers != null)
                ClearWatchers();

            InitCheckupBlobs();

            // Run file tasks.
            Task.Run(() => InitWatchers());
            
            // Cancel reminder.
            if (ctsForNotificationManager != null)
                ctsForNotificationManager.Cancel();
            ctsForNotificationManager = new CancellationTokenSource();

            // Run notification reminder.
            Task.Run(() => nm.RunNotificationReminderForFiles(ctsForNotificationManager, CheckUpBlobsBindingList), 
                ctsForNotificationManager.Token);
            
            // Cancel checker.
            if (ctsForDirectoryStatusChecker != null)
                ctsForDirectoryStatusChecker.Cancel();
            ctsForDirectoryStatusChecker = new CancellationTokenSource();
            
            // Run directory status checker.
            Task.Run(() => directoryStatusChecker.RunDirectoryStatusChecker(ctsForDirectoryStatusChecker, 
                pathManager.GetLabelsFromBudgetForDirectoriesStatusChecker(pathsToInit),
                directoryStatusCheckerTimeoutInSeconds), ctsForDirectoryStatusChecker.Token);
            
            // Init fifth row for unsent files.
            InitUnsentFileChecker();
        }
        private void LoadBaseConfigs()
        {
            loggerDebug.Debug($"Load base cfgs.");

            isFileBackupActive = conf.LoadFileBackupSetting();
            backupLoggerDir = conf.LoadLoggerBackupDir();
            archiveBackupDir = conf.LoadArchiveBackupSetting();
            unsentFileCheckerDelayInSeconds = conf.LoadUnsentFilesCheckerDelay();
            periodicNotificationDelayInMinutes = conf.LoadNotificationReminderSetting();
            balloonTipTimePeriodInSeconds = conf.LoadBalloonTipTimePeriodSetting();
            copySettings = conf.LoadCopySettings();
            bufferSizeForFileWatcher = conf.LoadBufferSizeForFileWatcher();
            startInTray = conf.LoadStartInTraySetting();
            isFileHandlerEnabled = conf.LoadFileHandlerSetting();
            directoryStatusCheckerTimeoutInSeconds = conf.LoadDirectoryStatusCheckerTimeout();
        }
        private void LoadPathConfigs()
        {
            loggerDebug.Debug($"Load path cfgs.");

            PathSettings = isTestingModeActive ? conf.LoadPathSettings().ChangeBaseDirForPathSettings(testBaseDir) : conf.LoadPathSettings();
            budgetSettings = conf.LoadBudgetSettings();
            currencySettings = conf.LoadCurrencySettings();
            ticketSettings = conf.LoadTicketSettings();
            unsentFileSettings = conf.LoadUnsentFilesSettings();
            dbSettings = conf.LoadDbSettings();
            mainDbAddress = conf.LoadMainDbAddress();
            
            mainDbRi = dbSettings.Find(ri => ri.Host.Equals(mainDbAddress));
 
            pathManager = new PathManager(tlp, pathSettings, budgetSettings, currencySettings, ticketSettings);
            pathsToInit = pathManager.GetPathsToInit();

            bool isAllPathesOk = pv.ValidatePathSettingsFromCfgs(pathsToInit);

            if (isAllPathesOk == false)
            {
                loggerError.Error($"Path setting validation has failed.");
                MessageBox.Show("Ошибка доступа к заданным каталогам. Проверьте настройки путей. Подробности в лог-файлах. Приложение будет закрыто.");
                System.Environment.Exit(1);
            }
        }
        private void InitToolTipForLabels()
        {
            if(toolTipForLabels != null)
            {
                toolTipForLabels.Dispose();
            }

            toolTipForLabels = new ToolTip();
        }
        private void InitNotifyIcon()
        {
            if (notifyIconForMainStatus != null)
            {
                notifyIconForMainStatus.Dispose();
                notifyIconForMainStatus = null;
            }

            notifyIconForMainStatus = new NotifyIcon();
        }
        private void InitMainForm()
        {
            if (startInTray)
                this.WindowState = FormWindowState.Minimized;

            this.Icon = check_up_money.Properties.Resources.Money;
            this.Resize += MainForm_Resize;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = "CheckupMoney v. " + version;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            this.Load += Form_Main_Load;
            this.FormClosing += Form_Main_FormClosing;

            this.dataGridView_Main.CellMouseDoubleClick += DataGridView_Main_CellMouseDoubleClick;
            //this.dataGridView_Main.CellMouseDown += DataGridView_Main_CellMouseDown;
            //this.dataGridView_Main.Sorted += DataGridView_Main_Sorted;

            this.dataGridView_Main.AutoGenerateColumns = false;

            this.button_Main_Process.Enabled = isFileHandlerEnabled;

            this.MinimumSize = new Size(960, 360);
        }
        private void ClearWatchers()
        {
            loggerDebug.Debug("Clear watchers.");

            foreach(var w in watchers)
            {
                if(w.watcher != null)
                {
                    w.watcher.EnableRaisingEvents = false;
                    w.watcher.Dispose();
                    w.watcher = null;
                }
            }
        }
        public void InitWatchers()
        {
            int watcherIndex = 0;
            int rowIndex = 0;

            watchers = new List<CheckupFileWatcher>();

            //var pathSettingToInit = GetPathsToInit(false);

            loggerDebug.Debug($"Path to init: {pathsToInit.Count}");

            // Init first row for budgets.
            for (int i = 0; i < 6; i++)
            {
                if (pathsToInit[i].isEnabled)
                {
                    InitWatcher(watcherIndex, i, i, rowIndex, pathsToInit, fh, bufferSizeForFileWatcher);
                    watcherIndex++;
                }
                else
                {
                    // Update label text for disabled budgets.
                    //tlp.GetControlFromPosition(i, rowIndex).ThreadSafeInvokeForTextAssignment("-");
                }
            }

            rowIndex = 3;

            // Init fourth row for tickets.
            for (int i = 6; i < 12; i++)
            {
                if (pathsToInit[i].isEnabled)
                {
                    InitWatcher(watcherIndex, i + 12, i, rowIndex, pathsToInit, fh, bufferSizeForFileWatcher);
                    watcherIndex++;
                }
                else
                {
                    // Update label text for disabled budgets.
                    //tlp.GetControlFromPosition(i, rowIndex).ThreadSafeInvokeForTextAssignment("-");
                }
            }

            // Init third row for bank thicket in.
            //InitWatcher(watcherIndex, 14, 0, 3, pathSetting, fh, bufferSizeForFileWatcher, "CW*.txt");

            //loggerDebug.Debug("Wait for init dir completion.");
            Task.WaitAll(initDirTasks.ToArray());

            loggerDebug.Debug("Init binding.");

            if(this.dataGridView_Main.InvokeRequired)
            {
                this.dataGridView_Main.Invoke(new Action(() =>
                {
                    this.dataGridView_Main.DataSource = CheckUpBlobsBindingList;
                }));
            }
            else
            {
                this.dataGridView_Main.DataSource = CheckUpBlobsBindingList;
            } 
        }
        public void InitWatcher(int watcherIndex, int settingIndex, int col, int row,
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> settings,
            IFileHandler fh, int bufferSize)
        {
            if (!string.IsNullOrWhiteSpace(settings[settingIndex].pathType) && !string.IsNullOrWhiteSpace(settings[settingIndex].path))
            {
                logger.Info($"INIT WATCHER {watcherIndex} {settings[settingIndex].pathType}");

                // Init watcher.
                watchers.Add(new CheckupFileWatcher(fh, nm, this));

                initDirTasks.Add(InitDir(settings[settingIndex].path,
                    pathManager.GetBudgetTypeFromSetting(settings[settingIndex].pathType), settings[settingIndex].fileMask));

                // Run watcher.
                Task.Run(() => watchers[watcherIndex].
                    RunWatcher(
                    pathManager.GetCounterNameAndTipFromPathSetting(settings[settingIndex].pathType),
                    settings[settingIndex].path,
                    pathManager.GetBudgetTypeFromSetting(settings[settingIndex].pathType),
                    tlp.GetControlFromPosition(col, row), bufferSize, settings[settingIndex].fileMask));
            }
        }
        public void InitUnsentFileChecker()
        {
            dbSettings = conf.LoadDbSettings();

            if (ctsForUnsentFileChecker != null)
                ctsForUnsentFileChecker.Cancel();
            ctsForUnsentFileChecker = new CancellationTokenSource();

            Task.Run(() =>
            unsentFileChecker.InitUnsentFileChecker(unsentFileSettings,
            GetLabelsFromBudgetForUnsentChecker(), dbSettings, unsentFileCheckerDelayInSeconds, ctsForUnsentFileChecker), ctsForUnsentFileChecker.Token);
        }
        private List<(string budgetType, Control unsentFilesLabel)> GetLabelsFromBudgetForUnsentChecker()
        {
            List<(string budgetType, Control unsentFilesLabel)> unsentFilesLabelList = new List<(string budgetType, Control unsentFilesLabel)>();

            int columnIndex = 0;
            int rowIndex = 5;

            foreach (var ufs in unsentFileSettings)
            {
                unsentFilesLabelList.Add((ufs.budgetType, tlp.GetControlFromPosition(columnIndex, rowIndex)));
                columnIndex++;
            }

            return unsentFilesLabelList;
        }
        private void InitCheckupBlobs()
        {
            if (CheckUpBlobsBindingList != null)
            {
                CheckUpBlobsBindingList.Clear();
                //this.dataGridView_Main.DataSource = null;
            }
            else
            {
                CheckUpBlobsBindingList = new BindingList<CheckUpBlob>();
            }
        }
        public async Task<bool> InitDir(string dir, BudgetType budgetType, string fileMask)
        {
            List<bool> isDirParsed = new List<bool>();

            var dirFiles = string.IsNullOrEmpty(fileMask) ? Directory.EnumerateFiles(dir)
                : Directory.EnumerateFiles(dir, fileMask, SearchOption.TopDirectoryOnly);

            loggerDebug.Debug($"Init dir {dir} for watcher. Type - {budgetType}. Mask - {fileMask}. Files: {dirFiles.Count()}");

            foreach (var file in dirFiles)
            {
                loggerDebug.Debug($"Init file -> {file}");
                isDirParsed.Add(await Add(file, budgetType, true).ConfigureAwait(false));
            }

            return isDirParsed.All(pr => pr);
        }
        #endregion
        #region Icon
        public void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            loggerDebug.Debug("NotifyIcon_MouseDoubleClick");
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        public void NotifyIconForMainStatus_BalloonTipClicked(object sender, EventArgs e)
        {
            loggerDebug.Debug("NotifyIconForMainStatus_BalloonTipClicked");
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        #endregion
        #region Main form
        public async Task<bool> Add(string fullFilePath, BudgetType budgetType, bool isInitAdd = false)
        {
            bool parseResult = false;

            // TODO. Error handling.
            try
            {
                var cbT = await fp.ParseFile(Path.GetDirectoryName(fullFilePath), Path.GetFileName(fullFilePath), budgetType);

                loggerDebug.Debug($"Add CheckUpBlob -> {cbT.FileName}");

                if (this.dataGridView_Main.InvokeRequired)
                {
                    this.dataGridView_Main.Invoke(new Action(() =>
                    {
                        CheckUpBlobsBindingList.Add(cbT);
                    }));
                }
                else
                {
                    CheckUpBlobsBindingList.Add(cbT);
                }

                if (cbT.CheckUpFileType == CheckUpBlob.FileType.Unknown)
                    logger.Info($"Unknown file type. {fullFilePath}");

                parseResult = true;
            }
            catch (Exception ex)
            {
                loggerError.Error(ex);
                throw;
            }

            return parseResult;
        }
        public void Remove(string fullFilePath)
        {
            try
            {
                logger.Info($"Removing item -> {fullFilePath}");

                CheckUpBlob cb = CheckUpBlobsBindingList.Single(e => e.FilePath.Equals(fullFilePath));

                int removeItemIndex = CheckUpBlobsBindingList.IndexOf(cb);
                //RemoveColumnFromDataGridView_Main(removeItemIndex);

                if (this.dataGridView_Main.InvokeRequired)
                {
                    this.dataGridView_Main.Invoke(new Action(() => { CheckUpBlobsBindingList.Remove(cb); }));

                }
                else
                {
                    CheckUpBlobsBindingList.Remove(cb);
                }

            }
            catch (InvalidOperationException)
            {
                loggerError.Error($"Unable to delete {fullFilePath} from collection. Item didn't exist.");
                throw new Exception("Ошибка при удалении файла с коллекции. Обратитесь к администратору.");
            }
        }
        private void Form_Main_Load(object sender, EventArgs e)
        {
            var sizeAndLocation = conf.LoadWindowSizeAndLocation();

            // Set window location.
            this.Location = sizeAndLocation.windowLocation;
            //this.Location = this.PointToClient(sizeAndLocation.windowLocation);

            //logger.Info($"Loaded location -> {this.PointToClient(sizeAndLocation.windowLocation)} - {this.Location}");

            // Set window size.
            this.Size = sizeAndLocation.windowSize;
        }
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveWindowsLocation();
        }
        private void SaveWindowsLocation()
        {
            // Copy window location to app settings.
            conf.SaveWindowLocationToConfig(this.Location);

            // Copy window size to app settings.
            if (this.WindowState == FormWindowState.Normal)
            {
                conf.SaveWindowSize(this.Size);
            }
            else
            {
                conf.SaveWindowSize(this.RestoreBounds.Size);
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                nm.ShowTrayNotificationForMinimizedState();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                //
            }
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                var logDir = new DirectoryInfo(Path.Combine(assemblyFolder, "Logs"));

                if(logDir.Exists && logDir.EnumerateFiles().Any())
                {
                    var uniqueFilePath = backupLoggerDir.GetUniqueFilePathFromFolder();

                    logger.Info($"Log dir exist: {logDir}");

                    // Copy backup logs.
                    if (Directory.Exists(backupLoggerDir))
                    {
                        string backupLoggerDirForUser = uniqueFilePath;

                        if (!Directory.Exists(backupLoggerDirForUser))
                            Directory.CreateDirectory(backupLoggerDirForUser);

                        foreach (var file in logDir.GetFilesByExtensions(new string[] { ".log" }))
                        {
                            File.Copy(file.FullName, Path.Combine(backupLoggerDirForUser, file.Name), true);
                        }
                    }
                    else
                    {
                        loggerError.Error($"Log backup directory {backupLoggerDir} didnt exist");
                    }
                }
                else
                {
                    loggerError.Error($"No logs to copy in dir: {logDir}");
                }

                SaveWindowsLocation();
            }
            catch(Exception ex)
            {
                loggerError.Error($"Log backup error: {ex.Message}");
            }
            finally
            {
                if(nm != null)
                    nm.DisableTrayNotificationForMainState();
            }
        }
        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("ToolStripMenuItem_Exit_Click");
            Application.Exit();
        }
        private void ToolStripMenuItem_BudgetSettings_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("ToolStripMenuItem_BudgetSettings_Click");

            Form_BudgetSettings fBs = new Form_BudgetSettings(conf, this);

            fBs.FormBorderStyle = FormBorderStyle.FixedSingle;
            fBs.StartPosition = FormStartPosition.CenterScreen;

            fBs.Show();
        }
        private void ToolStripMenuItem_FileHistory_Click(object sender, EventArgs e)
        {
            // new DbConnector(ds.ri, cypher)

            var dbConnector = new DbConnector(mainDbRi, cypher);

            //MessageBox.Show(sce ? null);
            //MessageBox.Show(dbConnector.ConnectionStringOdbc);

            if (sce == null)
                MessageBox.Show("Sce is null.");

            Form_FileHistory form_FileHistory = new(sce, dbConnector);
            form_FileHistory.Show();
        }
        private void ToolStripMenuItem_BdSettings_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("ToolStripMenuItem_BdSettings_Click");

            Form_DataBaseSettings fDbs = new Form_DataBaseSettings(cypher, conf, dbSettings, pv, this);

            fDbs.FormBorderStyle = FormBorderStyle.FixedSingle;
            fDbs.StartPosition = FormStartPosition.CenterScreen;

            fDbs.Show();
        }
        private void ToolStripMenuItem_PathSettings_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("ToolStripMenuItem_PathSettings_Click");

            //LoadConfigs();

            Form_PathSettings fPs = new Form_PathSettings(conf, pv, this, pathSettings, budgetSettings, ticketSettings);

            fPs.FormBorderStyle = FormBorderStyle.FixedSingle;
            fPs.StartPosition = FormStartPosition.CenterScreen;

            fPs.Show();
        }
        private void ToolStripMenuItem_About_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("ToolStripMenuItem_About_Click");

            MessageBox.Show(AboutString);
        }
        private async void Button_Main_Process_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug($"button_Main_Process_Click");

            bool isCopySuccessful = false;

            try
            {
                if(this.dataGridView_Main.CurrentCell != null)
                    {
                    //int selectedRowIndex = this.dataGridView_Main.CurrentCell.RowIndex;
                    int selectedRowIndex = this.dataGridView_Main.SelectedCells[0].RowIndex;

                    if (selectedRowIndex == -1)
                    {
                        MessageBox.Show("Необходимо выбрать обрабатываемый файл.");
                    }
                    else
                    {
                        logger.Info($"Processing file at {selectedRowIndex}");
                        isCopySuccessful = await fh.ProccessTheFileNew(selectedRowIndex);
                        logger.Info($"Processing status: {isCopySuccessful}");
                    }
                }
                    else
                {
                    logger.Info($"Nothing to process. Current cell is null.");
                    MessageBox.Show("Нет файлов для обработки.");
                }
            }
            catch (Exception ex)
            {
                logger.Info($"{ex.Message}");
                MessageBox.Show("Возникла ошибка в приложении. Подробности в лог-файле. Обратитесь к администратору.");
                //throw;
            }

            if(isCopySuccessful)
            {
                loggerDebug.Debug("Copy successful. Updating counters.");
                directoryStatusChecker.CheckDirectories(pathManager.GetLabelsFromBudgetForDirectoriesStatusChecker(pathsToInit));
            }
        }
        #endregion
        #region Main - Table layot
        private async Task<bool> InitTableLayoutPanel()
        {
            logger.Info("InitTableLayoutPanel.");

            if (tlp != null)
            {
                this.Controls.Remove(tlp);
                tlp.Dispose();
                tlp = null;
            }

            tlp = formHelper.CreateTableLayoutForFileCounters();

            this.Controls.Add(tlp);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(10000);

            var tlpInitResult = await WaitForTableLayoutInit(cancellationTokenSource.Token);

            logger.Info($"InitTableLayoutPanel init result -> {tlpInitResult}");

            loggerDebug.Debug($"Total labels in grid: {this.tlp.GetAll(typeof(Label)).Count()}");

            logger.Info("InitTableLayoutPanel -> done.");

            return tlpInitResult;
        }
        private async Task<bool> WaitForTableLayoutInit(CancellationToken ct)
        {
            bool isTlpReady = false;

            try
            {
                while (this.tlp.GetAll(typeof(Label)).All(l => l.IsHandleCreated) == false)
                {
                    ct.ThrowIfCancellationRequested();

                    // Wait for timer.
                    await Task.Delay(1200, ct);
                }

                isTlpReady = true;
            }
            catch(TaskCanceledException)
            {
                logger.Info($"Table layout panel was not created on time.");
            }
            catch (OperationCanceledException)
            {
                logger.Info($"Table layout panel creation was canceled.");
            }
            catch (Exception ex)
            {
                loggerError.Error($"{ex.Message}");
            }

            return isTlpReady;
        }
        private void DataGridView_Main_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            loggerDebug.Debug("DataGridView_Main_CellContentClick");

            //int selectedRowIndex = this.dataGridView_Main.CurrentCell.RowIndex;
            var sc = this.dataGridView_Main.SelectedCells;

            if (sc != null && sc.Count > 0)
            {
                int selectedRowIndex = sc[0].RowIndex;
                loggerDebug.Debug($"Open folder with selected row index {selectedRowIndex}.");
                fh.OpenFileFolder(selectedRowIndex);
            }
            else
            {
                logger.Info($"Selected index in out of range.");
            }
        }
        void DataGridView_Main_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                selectedValue = this.dataGridView_Main.SelectedRows[0].Cells[0].Value.ToString();
            }
        }
        void DataGridView_Main_Sorted(object sender, EventArgs e)
        {
        }
        #endregion
        private void ShowErrorBox()
        {
            // Initializes the variables to pass to the MessageBox. Show method.
            string message = "Возникли ошибки при запуске. Приложение будет закрыто. Обратитесь к администратору. Подробности в лог-файле.";
            string caption = "Error";

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Exit.
                System.Environment.Exit(1);
            }
        }
        public void UpdateBudgetSettingsStatus()
        {
            foreach(var budSett in budgetSettings)
            {
                var tup = new Tuple<string, bool>(budSett.Item1, budSett.isEnabled);
                checkedBudgets.Add(tup);
                logger.Info($"Budget settings add: {tup.Item1} {tup.Item2}");
            }
        }
    }
}