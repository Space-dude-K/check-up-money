using check_up_money.Interfaces;
using check_up_money.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static check_up_money.CheckUpFile.CheckUpBlob;

namespace check_up_money
{
    class PathManager : IPathManager
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");


        private readonly TableLayoutPanel tlp;
        private List<(string pathType, string path)> pathSettings;
        private List<(string budgetType, bool isEnabled)> budgetSettings;
        private List<(string budgetType, bool isEnabled)> currencySettings;
        private List<(string ticketType, bool isEnabled)> ticketSettings;

        public PathManager(TableLayoutPanel tlp, 
            List<(string pathType, string path)> pathSettings, List<(string budgetType, bool isEnabled)> budgetSettings,
            List<(string budgetType, bool isEnabled)> currencySettings,
            List<(string ticketType, bool isEnabled)> ticketSettings)
        {
            this.tlp = tlp;
            this.pathSettings = pathSettings;
            this.budgetSettings = budgetSettings;
            this.currencySettings = currencySettings;
            this.ticketSettings = ticketSettings;
        }
        // Merge path based on status.
        public List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> GetPathsToInit()
        {
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> checkedPaths = new();

            // Main settings.
            foreach (var path in pathSettings)
            {
                string budgetType = path.pathType.Substring(0, 3);
                bool isEnabled = false;
                bool isCurrencyEnabled = false;
                string fileMask = "*.*";

                if (!path.pathType.Contains("BankTicket"))
                {
                    isEnabled = GetEnabledStatusForPath(budgetType, path.pathType, checkedPaths);
                }
                else
                {
                    isEnabled = GetEnabledStatusForPath(budgetType, path.pathType, checkedPaths);
                    fileMask = "CW*.txt";
                }

                if (path.pathType.Contains("Out") && !path.pathType.Contains("Currency"))
                    isCurrencyEnabled = currencySettings.Single(cs => budgetType.Equals(cs.budgetType.Substring(0, 3))).isEnabled;

                loggerDebug.Debug($"Path: {path.pathType} - file mask: {fileMask} - status: {isEnabled} - currency status: {isCurrencyEnabled}");
                checkedPaths.Add((path.pathType, path.path, fileMask, isEnabled, isCurrencyEnabled));
            }

            loggerDebug.Debug($"Paths to init: {checkedPaths.Count}");

            return checkedPaths;
        }
        private bool GetEnabledStatusForPath(string budgetType, string pathType, 
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> checkedPaths)
        {
            bool isEnabled = false;

            if(pathType.Contains("Currency"))
            {
                var parentPath = checkedPaths.Single(cp => cp.pathType.Equals(pathType.Substring(0, pathType.Length - "Currency".Length)));

                //loggerDebug.Debug($"Parent path for: {pathType} - {budgetType}: {parentPath.pathType}");

                if(parentPath.isEnabled && parentPath.isCurrencyEnabled)
                {
                    isEnabled = true;
                }
            }
            else
            {
                isEnabled = budgetSettings.Single(ps =>
                        budgetType.Equals(ps.budgetType.Substring(0, 3))).isEnabled;
            }

            return isEnabled;
        }
        public List<(string pathType, Control unsentFilesLabel)>
            GetLabelsFromBudgetForDirectoriesStatusChecker(
            List<(string pathType, string path, string fileMask, bool isEnabled, bool isCurrencyEnabled)> pathsToInit)
        {
            List<(string pathType, Control label)> directoryCheckerLabelList = new();

            int columnIndex = 0;
            int rowIndex = 0;

            for (int i = 0; i < pathsToInit.Count - 18; i++)
            {
                directoryCheckerLabelList.Add((pathsToInit[i].pathType, tlp.GetControlFromPosition(columnIndex, rowIndex)));

                columnIndex++;

                if ((i + 1) % 6 == 0)
                {
                    columnIndex = 0;
                    rowIndex++;
                }
            }

            return directoryCheckerLabelList;
        }
        // TODO: Рефакторинг.
        public (string labelName, string labelTip) GetCounterNameAndTipFromPathSetting(string pathName)
        {
            string counterName = string.Empty;
            string labelTip = string.Empty;

            //loggerDebug.Debug($"Path name - {pathName}");

            switch (pathName)
            {
                // MAIN
                case "repIn":
                    counterName = "REP  -   ";
                    labelTip = "Выгрузка на банк." + GetBudgetSuffixForLabelTip("repIn");
                    break;
                case "oblIn":
                    counterName = "OBL  -   ";
                    labelTip = "Выгрузка на банк." + GetBudgetSuffixForLabelTip("oblIn");
                    break;
                case "cityIn":
                    counterName = "CITY  -   ";
                    labelTip = "Выгрузка на банк." + GetBudgetSuffixForLabelTip("cityIn");
                    break;
                case "regIn":
                    counterName = "REG  -   ";
                    labelTip = "Выгрузка на банк." + GetBudgetSuffixForLabelTip("regIn");
                    break;
                case "uniIn":
                    counterName = "UNI  -   ";
                    labelTip = "Выгрузка на банк." + GetBudgetSuffixForLabelTip("uniIn");
                    break;
                case "extIn":
                    counterName = "EXT  -   ";
                    labelTip = "Выгрузка на банк." + GetBudgetSuffixForLabelTip("extIn");
                    break;
                // BANK MAIN
                case "repBankMainOut":
                    counterName = "Bank main REP  -   ";
                    labelTip = "Платёжки в банк." + GetBudgetSuffixForLabelTip("repBankMainOut");
                    break;
                case "oblBankMainOut":
                    counterName = "Bank main OBL  -   ";
                    labelTip = "Платёжки в банк." + GetBudgetSuffixForLabelTip("oblBankMainOut");
                    break;
                case "cityBankMainOut":
                    counterName = "Bank main CITY -   ";
                    labelTip = "Платёжки в банк." + GetBudgetSuffixForLabelTip("cityBankMainOut");
                    break;
                case "regBankMainOut":
                    counterName = "Bank main REG  -   ";
                    labelTip = "Платёжки в банк." + GetBudgetSuffixForLabelTip("regBankMainOut");
                    break;
                case "uniBankMainOut":
                    counterName = "Bank main UNI  -   ";
                    labelTip = "Платёжки в банк." + GetBudgetSuffixForLabelTip("uniBankMainOut");
                    break;
                case "extBankMainOut":
                    counterName = "Bank main EXT  -   ";
                    labelTip = "Платёжки в банк." + GetBudgetSuffixForLabelTip("extBankMainOut");
                    break;
                // BANK MISC
                case "repBankMiscOut":
                    counterName = "Bank misc REP  -   ";
                    labelTip = "Документы свободного формата в банк." + GetBudgetSuffixForLabelTip("repBankMiscOut");
                    break;
                case "oblBankMiscOut":
                    counterName = "Bank misc OBL  -   ";
                    labelTip = "Документы свободного формата в банк." + GetBudgetSuffixForLabelTip("oblBankMiscOut");
                    break;
                case "cityBankMiscOut":
                    counterName = "Bank misc CITY -   ";
                    labelTip = "Документы свободного формата в банк." + GetBudgetSuffixForLabelTip("cityBankMiscOut");
                    break;
                case "regBankMiscOut":
                    counterName = "Bank misc REG  -   ";
                    labelTip = "Документы свободного формата в банк." + GetBudgetSuffixForLabelTip("regBankMiscOut");
                    break;
                case "uniBankMiscOut":
                    counterName = "Bank misc UNI  -   ";
                    labelTip = "Документы свободного формата в банк." + GetBudgetSuffixForLabelTip("uniBankMiscOut");
                    break;
                case "extBankMiscOut":
                    counterName = "Bank misc EXT  -   ";
                    labelTip = "Документы свободного формата в банк." + GetBudgetSuffixForLabelTip("extBankMiscOut");
                    break;
                // BANK TICKET IN
                case "repBankTicketIn":
                    counterName = "Квитанции in REP  -   ";
                    labelTip = "Квитанции из банка." + GetBudgetSuffixForLabelTip("repBankTicketIn");
                    break;
                case "oblBankTicketIn":
                    counterName = "Квитанции in OBL  -   ";
                    labelTip = "Квитанции из банка." + GetBudgetSuffixForLabelTip("oblBankTicketIn");
                    break;
                case "cityBankTicketIn":
                    counterName = "Квитанции in CITY -   ";
                    labelTip = "Квитанции из банка." + GetBudgetSuffixForLabelTip("cityBankTicketIn");
                    break;
                case "regBankTicketIn":
                    counterName = "Квитанции in REG  -   ";
                    labelTip = "Квитанции из банка." + GetBudgetSuffixForLabelTip("regBankTicketIn");
                    break;
                case "uniBankTicketIn":
                    counterName = "Квитанции in UNI  -   ";
                    labelTip = "Квитанции из банка." + GetBudgetSuffixForLabelTip("uniBankTicketIn");
                    break;
                case "extBankTicketIn":
                    counterName = "Квитанции in EXT  -   ";
                    labelTip = "Квитанции из банка." + GetBudgetSuffixForLabelTip("extBankTicketIn");
                    break;
                // BANK TICKET OUT
                case "repBankTicketOut":
                    counterName = "Квитанции out REP  -   ";
                    labelTip = "Квитанции в банк." + GetBudgetSuffixForLabelTip("repBankTicketOut");
                    break;
                case "oblBankTicketOut":
                    counterName = "Квитанции out OBL  -   ";
                    labelTip = "Квитанции из банка." + GetBudgetSuffixForLabelTip("oblBankTicketOut");
                    break;
                case "cityBankTicketOut":
                    counterName = "Квитанции out CITY -   ";
                    labelTip = "Квитанции в банк." + GetBudgetSuffixForLabelTip("cityBankTicketOut");
                    break;
                case "regBankTicketOut":
                    counterName = "Квитанции out REG  -   ";
                    labelTip = "Квитанции в банк." + GetBudgetSuffixForLabelTip("regBankTicketOut");
                    break;
                case "uniBankTicketOut":
                    counterName = "Квитанции out UNI  -   ";
                    labelTip = "Квитанции в банк." + GetBudgetSuffixForLabelTip("uniBankTicketOut");
                    break;
                case "extBankTicketOut":
                    counterName = "Квитанции out EXT  -   ";
                    labelTip = "Квитанции в банк." + GetBudgetSuffixForLabelTip("extBankTicketOut");
                    break;

                default:
                    counterName = " - ";
                    labelTip = " - ";
                    break;
            }

            return (counterName, labelTip);
        }
        private string GetBudgetSuffixForLabelTip(string rawBudget)
        {
            string budget = string.Empty;
            rawBudget = rawBudget.ToLower().Substring(0, 3);

            switch (rawBudget)
            {
                case "rep":
                    budget = " Республиканский бюджет.";
                    break;
                case "obl":
                    budget = " Областной бюджет.";
                    break;
                case "cit":
                    budget = " Городской бюджет.";
                    break;
                case "reg":
                    budget = " Районный бюджет.";
                    break;
                case "uni":
                    budget = " Союзный бюджет.";
                    break;
                case "ext":
                    budget = " Внебюджет.";
                    break;
            }

            return budget;
        }
        public string GetCounterSuffixForLabelFileType(bool isParentEnabled, bool isChildEnabled)
        {
            string suffix = string.Empty;

            if(isParentEnabled && isChildEnabled)
            {
                suffix = " Файлы в рублях и валюте.";
            }
            else if(isParentEnabled && !isChildEnabled)
            {
                suffix = " Файлы в рублях.";
            }
            else if(!isParentEnabled && isChildEnabled)
            {
                suffix = " Файлы в валюте.";
            }

            return suffix;
        }
        public BudgetType GetBudgetTypeFromSetting(string setting)
        {
            switch (setting)
            {
                case "repIn":
                    return BudgetType.Rep;
                case "oblIn":
                    return BudgetType.Obl;
                case "cityIn":
                    return BudgetType.City;
                case "regIn":
                    return BudgetType.Reg;
                case "uniIn":
                    return BudgetType.Union;
                case "extIn":
                    return BudgetType.Extra;
                default:
                    return BudgetType.Undetermined;
            }
        }
        public string GetPathTypeFromCheckUpFileTypeAndBudgetType(FileType fileType, BudgetType budgetType)
        {
            string pathType = string.Empty;

            var mainBankTypes = new List<FileType>() { FileType.Inc, FileType.Out, FileType.IncOut, FileType.OutForeign, FileType.IncForeign };
            var secondaryBankTypes = new List<FileType>() { FileType.Penalty, FileType.Star, FileType.Stop, FileType.Withdrawal, FileType.CurrencyExchange };
            var currencyTypes = new List<FileType>() { FileType.OutForeign, FileType.CurrencyExchange, FileType.IncForeign };

            if (mainBankTypes.Any(t => t == fileType))
            {
                switch (budgetType)
                {
                    case BudgetType.Rep:
                        pathType = "repBankMainOut";
                        break;
                    case BudgetType.Obl:
                        pathType = "oblBankMainOut";
                        break;
                    case BudgetType.City:
                        pathType = "cityBankMainOut";
                        break;
                    case BudgetType.Reg:
                        pathType = "regBankMainOut";
                        break;
                    case BudgetType.Union:
                        pathType = "uniBankMainOut";
                        break;
                    case BudgetType.Extra:
                        pathType = "extBankMainOut";
                        break;
                    case BudgetType.Undetermined:
                        loggerError.Error($"Undefined file type");
                        throw new Exception("Undefined file type");
                    default:
                        loggerError.Error($"Undefined file type");
                        throw new Exception("Undefined file type");
                }
            }
            else if (secondaryBankTypes.Any(t => t == fileType))
            {
                switch (budgetType)
                {
                    case BudgetType.Rep:
                        pathType = "repBankMiscOut";
                        break;
                    case BudgetType.Obl:
                        pathType = "oblBankMiscOut";
                        break;
                    case BudgetType.City:
                        pathType = "cityBankMiscOut";
                        break;
                    case BudgetType.Reg:
                        pathType = "regBankMiscOut";
                        break;
                    case BudgetType.Union:
                        pathType = "uniBankMiscOut";
                        break;
                    case BudgetType.Extra:
                        pathType = "extBankMiscOut";
                        break;
                    case BudgetType.Undetermined:
                        loggerError.Error($"Undefined file type");
                        throw new Exception("Undefined file type");
                    default:
                        loggerError.Error($"Undefined file type");
                        throw new Exception("Undefined file type");
                }
            }
            else if (fileType == FileType.CbTicket)
            {
                switch (budgetType)
                {
                    case BudgetType.Rep:
                        pathType = "repBankTicketOut";
                        break;
                    case BudgetType.Obl:
                        pathType = "oblBankTicketOut";
                        break;
                    case BudgetType.City:
                        pathType = "cityBankTicketOut";
                        break;
                    case BudgetType.Reg:
                        pathType = "regBankTicketOut";
                        break;
                    case BudgetType.Union:
                        pathType = "uniBankTicketOut";
                        break;
                    case BudgetType.Extra:
                        pathType = "extBankTicketOut";
                        break;
                    case BudgetType.Undetermined:
                        loggerError.Error($"Undefined file type");
                        throw new Exception("Undefined file type");
                    default:
                        loggerError.Error($"Undefined file type");
                        throw new Exception("Undefined file type");
                }
            }
            else
            {
                loggerError.Error("Processed file is not exist in defined group.");
                throw new Exception("Невозможно соотнести выбранный тип файла с путём.");
            }

            return pathType;
        }
    }
}