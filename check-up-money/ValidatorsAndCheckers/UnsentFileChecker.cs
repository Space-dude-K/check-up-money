using check_up_money.Cypher;
using check_up_money.Db;
using check_up_money.Extensions;
using check_up_money.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money
{
    class UnsentFileChecker : IUnsentFileChecker
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private readonly ToolTip toolTipForLabels;
        private readonly ISqlCmdExecutor sce;
        private readonly ICypher cypher;
        private readonly IRemoteHostAvailabilityChecker rhac;

        private AsyncObservableCollection<int> unsentFilesCounters;
        public AsyncObservableCollection<int> UnsentFilesCounters
        {
            get
            {
                return unsentFilesCounters;
            }
        }
        public UnsentFileChecker(ToolTip toolTipForLabels, ISqlCmdExecutor sqlCmdExecutor, ICypher cypher, IRemoteHostAvailabilityChecker rhac)
        {
            unsentFilesCounters = new AsyncObservableCollection<int>() { 0,0,0,0,0,0 };
            this.toolTipForLabels = toolTipForLabels;
            this.sce = sqlCmdExecutor;
            this.cypher = cypher;
            this.rhac = rhac;
        }
        public void InitUnsentFileChecker(List<(string budgetType, bool isEnabled)> unsentFileSettings, List<(string budgetType, 
            Control unsentFilesLabel)> unsentFilesControls, 
            List<RequisiteInformation> databaseInfos, int delayInSeconds, CancellationTokenSource ctSource)
        {
            logger.Info($"Init unsent file checker with delay {delayInSeconds}");

            var unsentFilesDataSet = GetDataSetForUnsentfileChecker(unsentFileSettings, databaseInfos, unsentFilesControls);

            foreach(var ds in unsentFilesDataSet)
            {
                if(ds.isEnabled)
                {
                    try
                    {
                        Task.Run(() => RunUnsentFileChecker(ctSource, delayInSeconds, ds.budgetType, ds.ri.Db,
                        new DbConnector(ds.ri, cypher), ds.counterLabel), ctSource.Token);
                    }
                    catch (Exception ex)
                    {
                        loggerError.Error(ex);
                        throw;
                    }
                }
                else
                {
                    logger.Info($"{ds.budgetType} is disabled.");
                    ds.counterLabel.ThreadSafeInvokeForTextAssignment("-");
                }
            }
        }
        // TODO. Рефакторинг.
        /// <summary>
        /// Этот метод соотносит тип бюджета с реквизитами и информационными панелями по настройкам неотправленных файлов, 
        /// реквизитам баз данных и панелей для неотправленных файлов.
        /// </summary>
        /// <param name="unsentFileSettings">Настройки неотправленных файлов.</param>
        /// <param name="databaseInfos">Реквизиты баз данных.</param>
        /// <param name="unsentFilesControls">Панели для неотправленных файлов.</param> 
        /// <returns>
        /// <see cref="List{T}"></see> с <see cref="Tuple{string, RequisiteInformation, Control, bool}"></see>  типом бюджета, 
        /// реквизитами БД, инфо панелью и состоянием.
        /// </returns>
        public List<(string budgetType, RequisiteInformation ri, Control counterLabel, bool isEnabled)> GetDataSetForUnsentfileChecker(
            List<(string budgetType, bool isEnabled)> unsentFileSettings, 
            List<RequisiteInformation> databaseInfos, List<(string budgetType, Control unsentFilesLabel)> unsentFilesControls)
        {
            List<(string budgetType, RequisiteInformation ri, Control counterLabel, bool isEnabled)> dataSet = new();

            foreach(var eb in unsentFileSettings)
            {
                switch (eb.budgetType)
                {
                    case "rep":
                        var repRi = databaseInfos.FirstOrDefault(di => di.Host.Equals("G600-SRREP") && di.Db.Equals("Rep"));

                        if(repRi != null)
                            dataSet.Add((
                            "rep",
                            repRi,
                            unsentFilesControls.FirstOrDefault(ufc => ufc.budgetType.Equals("rep")).unsentFilesLabel, eb.isEnabled));
                        break;
                    case "obl":
                        var oblRi = databaseInfos.FirstOrDefault(di => di.Host.Equals("G600-SROBL") && di.Db.Equals("Obl"));

                        if(oblRi != null)
                            dataSet.Add((
                            "obl",
                            oblRi,
                            unsentFilesControls.FirstOrDefault(ufc => ufc.budgetType.Equals("obl")).unsentFilesLabel, eb.isEnabled));
                        break;
                    case "city":
                        var cityRi = databaseInfos.FirstOrDefault(di => di.Host.Equals("G600-SRREG") && di.Db.Equals("City"));

                        if(cityRi != null)
                            dataSet.Add((
                            "city",
                            cityRi,
                            unsentFilesControls.FirstOrDefault(ufc => ufc.budgetType.Equals("city")).unsentFilesLabel, eb.isEnabled));
                        break;
                    case "reg":
                        var regRi = databaseInfos.FirstOrDefault(di => di.Host.Equals("G600-SRREG") && di.Db.Equals("Reg"));

                        if(regRi != null)
                            dataSet.Add((
                            "reg",
                            regRi,
                            unsentFilesControls.FirstOrDefault(ufc => ufc.budgetType.Equals("reg")).unsentFilesLabel, eb.isEnabled));
                        break;
                    case "uni":
                        var uniRi = databaseInfos.FirstOrDefault(di => di.Host.Equals("G600-SRREP") && di.Db.Equals("Uni"));

                        if(uniRi != null)
                            dataSet.Add((
                            "uni",
                            uniRi,
                            unsentFilesControls.FirstOrDefault(ufc => ufc.budgetType.Equals("uni")).unsentFilesLabel, eb.isEnabled));
                        break;
                    case "ext":
                        var extRi = databaseInfos.FirstOrDefault(di => di.Host.Equals("G600-SRREP") && di.Db.Equals("Vn"));

                        if(extRi != null)
                            dataSet.Add((
                            "ext",
                            extRi,
                            unsentFilesControls.FirstOrDefault(ufc => ufc.budgetType.Equals("ext")).unsentFilesLabel, eb.isEnabled));
                        break;
                    default:
                        break;
                }
            }

            return dataSet;
        }
        private int GetTupleUnsentFilesIndexFromBudgetName(string budgetType)
        {
            int tupleIndex = 0;

            switch (budgetType)
            {
                case "rep":
                    tupleIndex = 0;
                    break;
                case "obl":
                    tupleIndex = 1;
                    break;
                case "city":
                    tupleIndex = 2;
                    break;
                case "reg":
                    tupleIndex = 3;
                    break;
                case "uni":
                    tupleIndex = 4;
                    break;
                case "ext":
                    tupleIndex = 5;
                    break;
                default:
                    break;
            }

            return tupleIndex;
        }
        public async Task RunUnsentFileChecker(CancellationTokenSource source, int delayInSeconds, string budget, string databaseName, 
            DbConnector dbConnector, Control control)
        {
            // Define the cancellation token.
            CancellationToken token = source.Token;

            logger.Info($"Starting unsent file checker with delay {delayInSeconds} {budget} {databaseName}");

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var dbResult = Convert.ToInt32(await sce.ExecuteOdbcCmdScalar(dbConnector.ConnectionStringOdbc,
                        SqlCommands.GetDocsStateCodes(null, databaseName), control));

                    // TODO: Replace two boxing operations (maybe array?).
                    UnsentFilesCounters[GetTupleUnsentFilesIndexFromBudgetName(budget)] = dbResult;

                    var textForLabel = "На отправку " + budget.ToUpper() + " - [" + dbResult.ToString() + "]";
                    var textForToolTip = "На отправку в банк (из программы LT)." + budget.GetBudgetSuffixForLabelTip();

                    // Thread safe invoke.
                    control.ThreadSafeInvokeForTextAssignment(textForLabel);
                    control.ThreadSafeInvokeForLabelToolTip(toolTipForLabels, textForToolTip);
                    control.ThreadSafeInvokeForColor(Color.Black);

                    loggerDebug.Debug($"Unsent files for {budget} with db {databaseName}: {dbResult}");

                    // Wait for timer.
                    await Task.Delay(new TimeSpan(0, 0, delayInSeconds));
                }
            }
            catch (OperationCanceledException)
            {
                loggerError.Error($"Unsent file checker was stopped by cancelation request.");
            }
            /*
            catch (Exception ex)
            {
                control.ThreadSafeInvokeForTextAssignment("ERR");
                control.ThreadSafeInvokeForColor(Color.Red);

                loggerError.Error($"{ex.GetType()} - {ex.Message}");
            }
            */
        }
    }
}