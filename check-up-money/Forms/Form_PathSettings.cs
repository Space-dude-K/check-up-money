using check_up_money.Interfaces;
using check_up_money.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace check_up_money.Forms
{
    public partial class Form_PathSettings : Form
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        readonly IConfigurator conf;
        readonly IPathsValidator pValidator;
        private readonly List<(string budgetType, bool isEnabled)> checkedBudgets;
        private readonly List<(string ticketType, bool isEnabled)> checkedCbTickets;
        private readonly List<(string pathType, string path)> pathSettings;
        private readonly IMain main;

        public Form_PathSettings(IConfigurator conf, IPathsValidator pValidator, IMain main,
            List<(string pathType, string path)> pathSettings,
            List<(string budgetType, bool isEnabled)> checkedBudgets, List<(string ticketType, bool isEnabled)> checkedCbTickets)
        {
            InitializeComponent();

            this.conf = conf;
            this.checkedBudgets = checkedBudgets;
            this.checkedCbTickets = checkedCbTickets;
            this.main = main;
            this.pathSettings = pathSettings;
            this.pValidator = pValidator;

            InitPathSettings();
        }
        private void InitPathSettings()
        {
            DisableTextBoxsAndLabels();
            LoadBudgetPathSettings();
            LoadCbMainOutPathSettings();
            LoadCbMiscOutPathSettings();
            LoadCbInTicketPathSettings();
            LoadCbOutTicketPathSettings();
        }
        private void LoadBudgetPathSettings()
        {
            this.textBox_PS_RepBudget.Text = pathSettings[0].path;
            this.textBox_PS_OblBudget.Text = pathSettings[1].path;
            this.textBox_PS_CityBudget.Text = pathSettings[2].path;
            this.textBox_PS_RegBudget.Text = pathSettings[3].path;
            this.textBox_PS_UniBudget.Text = pathSettings[4].path;
            this.textBox_PS_ExtBudget.Text = pathSettings[5].path;
        }
        private void LoadCbMainOutPathSettings()
        {
            this.textBox_PS_BankOutMainRep.Text = pathSettings[6].path;
            this.textBox_PS_BankOutMainObl.Text = pathSettings[7].path;
            this.textBox_PS_BankOutMainCity.Text = pathSettings[8].path;
            this.textBox_PS_BankOutMainReg.Text = pathSettings[9].path;
            this.textBox_PS_BankOutMainUni.Text = pathSettings[10].path;
            this.textBox_PS_BankOutMainExt.Text = pathSettings[11].path;
        }
        private void LoadCbMiscOutPathSettings()
        {
            this.textBox_PS_BankOutMiscRep.Text = pathSettings[12].path;
            this.textBox_PS_BankOutMiscObl.Text = pathSettings[13].path;
            this.textBox_PS_BankOutMiscCity.Text = pathSettings[14].path;
            this.textBox_PS_BankOutMiscReg.Text = pathSettings[15].path;
            this.textBox_PS_BankOutMiscUni.Text = pathSettings[16].path;
            this.textBox_PS_BankOutMiscExt.Text = pathSettings[17].path;
        }
        private void LoadCbInTicketPathSettings()
        {
            this.textBox_PS_BankInTicketRep.Text = pathSettings[18].path;
            this.textBox_PS_BankInTicketObl.Text = pathSettings[19].path;
            this.textBox_PS_BankInTicketCity.Text = pathSettings[20].path;
            this.textBox_PS_BankInTicketReg.Text = pathSettings[21].path;
            this.textBox_PS_BankInTicketUni.Text = pathSettings[22].path;
            this.textBox_PS_BankInTicketExt.Text = pathSettings[23].path;
        }
        private void LoadCbOutTicketPathSettings()
        {
            this.textBox_PS_BankOutTicketRep.Text = pathSettings[24].path;
            this.textBox_PS_BankOutTicketObl.Text = pathSettings[25].path;
            this.textBox_PS_BankOutTicketCity.Text = pathSettings[26].path;
            this.textBox_PS_BankOutTicketReg.Text = pathSettings[27].path;
            this.textBox_PS_BankOutTicketUni.Text = pathSettings[28].path;
            this.textBox_PS_BankOutTicketExt.Text = pathSettings[29].path;
        }
        private void DisableTextBoxsAndLabels()
        {
            logger.Info($"Checked budgets count: {checkedBudgets.Count} Check tickets count: {checkedCbTickets.Count}");

            bool isRepEnabled = checkedBudgets[0].isEnabled;
            bool isOblEnabled = checkedBudgets[1].isEnabled;
            bool isCityEnabled = checkedBudgets[2].isEnabled;
            bool isRegEnabled = checkedBudgets[3].isEnabled;
            bool isUniEnabled = checkedBudgets[4].isEnabled;
            bool isExtEnabled = checkedBudgets[5].isEnabled;

            bool isRepTicketsEnabled = checkedCbTickets[0].isEnabled;
            bool isOblTicketsEnabled = checkedCbTickets[1].isEnabled;
            bool isCityTicketsEnabled = checkedCbTickets[2].isEnabled;
            bool isRegTicketsEnabled = checkedCbTickets[3].isEnabled;
            bool isUniTicketsEnabled = checkedCbTickets[4].isEnabled;
            bool isExtTicketsEnabled = checkedCbTickets[5].isEnabled;

            // Rep
            this.label_PS_RepBudget.Enabled = isRepEnabled;
            this.textBox_PS_RepBudget.Enabled = isRepEnabled;
            this.label_PS_BankOutMainRep.Enabled = isRepEnabled;
            this.textBox_PS_BankOutMainRep.Enabled = isRepEnabled;
            this.label_PS_BankOutMiscRep.Enabled = isRepEnabled;
            this.textBox_PS_BankOutMiscRep.Enabled = isRepEnabled;
            this.label_PS_BankInTicketRep.Enabled = isRepTicketsEnabled;
            this.textBox_PS_BankInTicketRep.Enabled = isRepTicketsEnabled;
            this.label_PS_BankOutTicketRep.Enabled = isRepTicketsEnabled;
            this.textBox_PS_BankOutTicketRep.Enabled = isRepTicketsEnabled;

            // Obl
            this.label_PS_OblBudget.Enabled = isOblEnabled;
            this.textBox_PS_OblBudget.Enabled = isOblEnabled;
            this.label_PS_BankOutMainObl.Enabled = isOblEnabled;
            this.textBox_PS_BankOutMainObl.Enabled = isOblEnabled;
            this.label_PS_BankOutMiscObl.Enabled = isOblEnabled;
            this.textBox_PS_BankOutMiscObl.Enabled = isOblEnabled;
            this.label_PS_BankInTicketObl.Enabled = isOblTicketsEnabled;
            this.textBox_PS_BankInTicketObl.Enabled = isOblTicketsEnabled;
            this.label_PS_BankOutTicketObl.Enabled = isOblTicketsEnabled;
            this.textBox_PS_BankOutTicketObl.Enabled = isOblTicketsEnabled;

            // City
            this.label_PS_CityBudget.Enabled = isCityEnabled;
            this.textBox_PS_CityBudget.Enabled = isCityEnabled;
            this.label_PS_BankOutMainCity.Enabled = isCityEnabled;
            this.textBox_PS_BankOutMainCity.Enabled = isCityEnabled;
            this.label_PS_BankOutMiscCity.Enabled = isCityEnabled;
            this.textBox_PS_BankOutMiscCity.Enabled = isCityEnabled;
            this.label_PS_BankInTicketCity.Enabled = isCityTicketsEnabled;
            this.textBox_PS_BankInTicketCity.Enabled = isCityTicketsEnabled;
            this.label_PS_BankOutTicketCity.Enabled = isCityTicketsEnabled;
            this.textBox_PS_BankOutTicketCity.Enabled = isCityTicketsEnabled;

            // Reg
            this.label_PS_RegBudget.Enabled = isRegEnabled;
            this.textBox_PS_RegBudget.Enabled = isRegEnabled;
            this.label_PS_BankOutMainReg.Enabled = isRegEnabled;
            this.textBox_PS_BankOutMainReg.Enabled = isRegEnabled;
            this.label_PS_BankOutMiscReg.Enabled = isRegEnabled;
            this.textBox_PS_BankOutMiscReg.Enabled = isRegEnabled;
            this.label_PS_BankInTicketReg.Enabled = isRegTicketsEnabled;
            this.textBox_PS_BankInTicketReg.Enabled = isRegTicketsEnabled;
            this.label_PS_BankOutTicketReg.Enabled = isRegTicketsEnabled;
            this.textBox_PS_BankOutTicketReg.Enabled = isRegTicketsEnabled;

            // Uni
            this.label_PS_UniBudget.Enabled = isUniEnabled;
            this.textBox_PS_UniBudget.Enabled = isUniEnabled;
            this.label_PS_BankOutMainUni.Enabled = isUniEnabled;
            this.textBox_PS_BankOutMainUni.Enabled = isUniEnabled;
            this.label_PS_BankOutMiscUni.Enabled = isUniEnabled;
            this.textBox_PS_BankOutMiscUni.Enabled = isUniEnabled;
            this.label_PS_BankInTicketUni.Enabled = isUniTicketsEnabled;
            this.textBox_PS_BankInTicketUni.Enabled = isUniTicketsEnabled;
            this.label_PS_BankOutTicketUni.Enabled = isUniTicketsEnabled;
            this.textBox_PS_BankOutTicketUni.Enabled = isUniTicketsEnabled;

            // Ext
            this.label_PS_ExtBudget.Enabled = isExtEnabled;
            this.textBox_PS_ExtBudget.Enabled = isExtEnabled;
            this.label_PS_BankOutMainExt.Enabled = isExtEnabled;
            this.textBox_PS_BankOutMainExt.Enabled = isExtEnabled;
            this.label_PS_BankOutMiscExt.Enabled = isExtEnabled;
            this.textBox_PS_BankOutMiscExt.Enabled = isExtEnabled;
            this.label_PS_BankInTicketExt.Enabled = isExtTicketsEnabled;
            this.textBox_PS_BankInTicketExt.Enabled = isExtTicketsEnabled;
            this.label_PS_BankOutTicketExt.Enabled = isExtTicketsEnabled;
            this.textBox_PS_BankOutTicketExt.Enabled = isExtTicketsEnabled;
        }
        private void Button_PS_Save_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("Button_PS_Save_Click");

            if (pValidator.ValidatePathSettingsFromControls(pathSettings, this.tabControl_PS_Main.SelectedTab.Controls))
            {
                var budgetSettings = new List<(string pathType, string path)>()
                { 
                    // Main in
                    ("repIn",               this.textBox_PS_RepBudget.Text),
                    ("oblIn",               this.textBox_PS_OblBudget.Text),
                    ("cityIn",              this.textBox_PS_CityBudget.Text),
                    ("regIn",               this.textBox_PS_RegBudget.Text),
                    ("uniIn",               this.textBox_PS_UniBudget.Text),
                    ("extIn",               this.textBox_PS_ExtBudget.Text),
                    // Bank main out
                    ("repBankMainOut",   this.textBox_PS_BankOutMainRep.Text),
                    ("oblBankMainOut",   this.textBox_PS_BankOutMainObl.Text),
                    ("cityBankMainOut",   this.textBox_PS_BankOutMainCity.Text),
                    ("regBankMainOut",   this.textBox_PS_BankOutMainReg.Text),
                    ("uniBankMainOut",   this.textBox_PS_BankOutMainUni.Text),
                    ("extBankMainOut",   this.textBox_PS_BankOutMainExt.Text),
                    // Bank misc out
                    ("repBankMiscOut",   this.textBox_PS_BankOutMiscRep.Text),
                    ("oblBankMiscOut",   this.textBox_PS_BankOutMiscObl.Text),
                    ("cityBankMiscOut",   this.textBox_PS_BankOutMiscCity.Text),
                    ("regBankMiscOut",   this.textBox_PS_BankOutMiscReg.Text),
                    ("uniBankMiscOut",   this.textBox_PS_BankOutMiscUni.Text),
                    ("extBankMiscOut",   this.textBox_PS_BankOutMiscExt.Text),
                    // Ticket in
                    ("repBankTicketIn",   this.textBox_PS_BankInTicketRep.Text),
                    ("oblBankTicketIn",   this.textBox_PS_BankInTicketObl.Text),
                    ("cityBankTicketIn",   this.textBox_PS_BankInTicketCity.Text),
                    ("regBankTicketIn",   this.textBox_PS_BankInTicketReg.Text),
                    ("uniBankTicketIn",   this.textBox_PS_BankInTicketUni.Text),
                    ("extBankTicketIn",   this.textBox_PS_BankInTicketExt.Text),
                    // Ticket out
                    ("repBankTicketOut",   this.textBox_PS_BankOutTicketRep.Text),
                    ("oblBankTicketOut",   this.textBox_PS_BankOutTicketObl.Text),
                    ("cityBankTicketOut",   this.textBox_PS_BankOutTicketCity.Text),
                    ("regBankTicketOut",   this.textBox_PS_BankOutTicketReg.Text),
                    ("uniBankTicketOut",   this.textBox_PS_BankOutTicketUni.Text),
                    ("extBankTicketOut",   this.textBox_PS_BankOutTicketExt.Text)
                };

                conf.WritePathSettings(budgetSettings);

                // Update pathes, blobl, re-init watchers.
                main.PathSettings = budgetSettings;
                main.InitWatchers();
            }
            else
            {
                loggerError.Error("Incorrect file paths.");
                MessageBox.Show("Некорректные пути!");
            }
        }
        private void Button_PS_Cancel_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("Button_PS_Cancel_Click");
            this.Close();
        }
    }
}