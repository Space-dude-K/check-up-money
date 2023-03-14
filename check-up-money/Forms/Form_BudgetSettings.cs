using check_up_money.Interfaces;
using check_up_money.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace check_up_money.Forms
{
    public partial class Form_BudgetSettings : Form
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        readonly IConfigurator conf;
        readonly IMain main;

        public Form_BudgetSettings(IConfigurator conf, IMain main)
        {
            InitializeComponent();
            this.conf = conf;
            this.main = main;
            InitBudgetSettings();
        }
        // Load checked status from config.
        private void InitBudgetSettings()
        {
            LoadBudgetStatuses();
            LoadCurrencyStatuses();
            LoadCbTicketStatuses();
            LoadUnsentFilesStatuses();
            /*
            if (isBudgetLinkEnabled)
            {
                InitTextBoxes();
                SubscribeToPathDisableEvent();
            }
            else
            {
                DisableTextBoxes();
            }
            */
        }
        private void LoadBudgetStatuses()
        {
            List<(string budgetType, bool isEnabled)> budgetSettings = conf.LoadBudgetSettings();

            this.checkBox_FBS_RepBudget.Checked = budgetSettings[0].isEnabled;
            this.checkBox_FBS_OblBudget.Checked = budgetSettings[1].isEnabled;
            this.checkBox_FBS_CityBudget.Checked = budgetSettings[2].isEnabled;
            this.checkBox_FBS_RegBudget.Checked = budgetSettings[3].isEnabled;
            this.checkBox_FBS_UniBudget.Checked = budgetSettings[4].isEnabled;
            this.checkBox_FBS_Extrabudget.Checked = budgetSettings[5].isEnabled;
        }
        private void LoadCurrencyStatuses()
        {
            List<(string budgetType, bool isEnabled)> currencySettings = conf.LoadCurrencySettings();

            this.checkBox_FBS_CurrencyRep.Checked = currencySettings[0].isEnabled;
            this.checkBox_FBS_CurrencyObl.Checked = currencySettings[1].isEnabled;
            this.checkBox_FBS_CurrencyCity.Checked = currencySettings[2].isEnabled;
            this.checkBox_FBS_CurrencyReg.Checked = currencySettings[3].isEnabled;
            this.checkBox_FBS_CurrencyUni.Checked = currencySettings[4].isEnabled;
            this.checkBox_FBS_CurrencyExt.Checked = currencySettings[5].isEnabled;
        }
        private void LoadCbTicketStatuses()
        {
            List<(string ticketType, bool isEnabled)> cbTicketSettings = conf.LoadTicketSettings();

            this.checkBox_FBS_TicketRepBudget.Checked = cbTicketSettings[0].isEnabled;
            this.checkBox_FBS_TicketOblBudget.Checked = cbTicketSettings[1].isEnabled;
            this.checkBox_FBS_TicketCityBudget.Checked = cbTicketSettings[2].isEnabled;
            this.checkBox_FBS_TicketRegBudget.Checked = cbTicketSettings[3].isEnabled;
            this.checkBox_FBS_TicketUniBudget.Checked = cbTicketSettings[4].isEnabled;
            this.checkBox_FBS_TicketExtBudget.Checked = cbTicketSettings[5].isEnabled;
        }
        private void LoadUnsentFilesStatuses()
        {
            List<(string budgetType, bool isEnabled)> unsentFilesSettings = conf.LoadUnsentFilesSettings();

            this.checkBox_FBS_UnsentRepBudget.Checked = unsentFilesSettings[0].isEnabled;
            this.checkBox_FBS_UnsentOblBudget.Checked = unsentFilesSettings[1].isEnabled;
            this.checkBox_FBS_UnsentCityBudget.Checked = unsentFilesSettings[2].isEnabled;
            this.checkBox_FBS_UnsentRegBudget.Checked = unsentFilesSettings[3].isEnabled;
            this.checkBox_FBS_UnsentUniBudget.Checked = unsentFilesSettings[4].isEnabled;
            this.checkBox_FBS_UnsentExtBudget.Checked = unsentFilesSettings[5].isEnabled;
        }
        private void InitTextBoxes()
        {
            foreach (Control x in this.Controls)
            {
                if (x is CheckBox && x.Name.Contains("checkBox_FBS"))
                {
                    ChangeTbStatus(x as CheckBox);
                }
            }
        }
        private void DisableTextBoxes()
        {
            foreach (Control x in this.Controls)
            {
                if (x is TextBox && x.Name.Contains("textBox_FBS"))
                {
                    x.Enabled = false;
                }
            }
        }
        private void SubscribeToPathDisableEvent()
        {
            foreach (Control x in this.Controls)
            {
                if (x is CheckBox)
                {
                    CheckBox cb = x as CheckBox;

                    cb.CheckedChanged += CheckBox_CheckedChanged;
                }
            }
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            ChangeTbStatus(cb);
        }
        private void ChangeTbStatus(CheckBox cb)
        {
            if (cb != null)
            {
                // Find corresponding text box.
                Control[] tb = this.Controls.Find("textBox_FBS_" + cb.Name.Substring("checkBox_FBS".Length + 1), true);

                if (tb != null && tb.Length > 0)
                {
                    if (!cb.Checked)
                    {
                        tb[0].Enabled = false;
                    }
                    else
                    {
                        tb[0].Enabled = true;
                    }
                }
            }
        }
        [Obsolete]
        private void Button_FBS_Save_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug($"Button_FBS_Save_Click");
            loggerDebug.Debug($"Saving budget settings.");

            WriteBudgetSettings();
            WriteCurrencySettings();
            WriteCbTicketSettings();
            WriteUnsentFilesSettings();

            // Update paths.
            //UpdateCheckedBudgetsStatuses(main.CheckedBudgets);

            // Re-init watchers.
            main.Init();
            //this.Close();
        }
        /*
        private void UpdateCheckedBudgetsStatuses(List<Tuple<string, bool>> checkedBudgets)
        {
            main.CheckedBudgets[0] = new Tuple<string, bool>(checkedBudgets[0].Item1, this.checkBox_FBS_RepBudget.Checked);
            main.CheckedBudgets[1] = new Tuple<string, bool>(checkedBudgets[0].Item1, this.checkBox_FBS_OblBudget.Checked);
            main.CheckedBudgets[2] = new Tuple<string, bool>(checkedBudgets[0].Item1, this.checkBox_FBS_CityBudget.Checked);
            main.CheckedBudgets[3] = new Tuple<string, bool>(checkedBudgets[0].Item1, this.checkBox_FBS_RegBudget.Checked);
            main.CheckedBudgets[4] = new Tuple<string, bool>(checkedBudgets[0].Item1, this.checkBox_FBS_UniBudget.Checked);
            main.CheckedBudgets[5] = new Tuple<string, bool>(checkedBudgets[0].Item1, this.checkBox_FBS_Extrabudget.Checked);
        }
        */
        // TODO. Is there a more generic way?
        private void WriteBudgetSettings()
        {
            conf.WriteBudgetSettings(new List<(string budgetType, bool isEnabled)>()
            {
                ("rep", this.checkBox_FBS_RepBudget.Checked),
                ("obl", this.checkBox_FBS_OblBudget.Checked),
                ("city", this.checkBox_FBS_CityBudget.Checked),
                ("reg", this.checkBox_FBS_RegBudget.Checked),
                ("uni", this.checkBox_FBS_UniBudget.Checked),
                ("ext", this.checkBox_FBS_Extrabudget.Checked)
            }
            );
        }
        private void WriteCurrencySettings()
        {
            conf.WriteCurrencySettings(new List<(string budgetType, bool isEnabled)>()
            {
                ("rep", this.checkBox_FBS_CurrencyRep.Checked),
                ("obl", this.checkBox_FBS_CurrencyObl.Checked),
                ("city", this.checkBox_FBS_CurrencyCity.Checked),
                ("reg", this.checkBox_FBS_CurrencyReg.Checked),
                ("uni", this.checkBox_FBS_CurrencyUni.Checked),
                ("ext", this.checkBox_FBS_CurrencyExt.Checked)
            }
            );
        }
        private void WriteCbTicketSettings()
        {
            conf.WriteTicketSettings(new List<(string ticketType, bool isEnabled)>()
            {
                ("repBankTicket", this.checkBox_FBS_TicketRepBudget.Checked),
                ("oblBankTicket", this.checkBox_FBS_TicketOblBudget.Checked),
                ("cityBankTicket", this.checkBox_FBS_TicketCityBudget.Checked),
                ("regBankTicket", this.checkBox_FBS_TicketRegBudget.Checked),
                ("uniBankTicket", this.checkBox_FBS_TicketUniBudget.Checked),
                ("extBankTicket", this.checkBox_FBS_TicketExtBudget.Checked)
            }
            );
        }
        private void WriteUnsentFilesSettings()
        {
            conf.WriteUnsentFilesSettings(new List<(string budgetType, bool isEnabled)>()
            {
                ("rep", this.checkBox_FBS_UnsentRepBudget.Checked),
                ("obl", this.checkBox_FBS_UnsentOblBudget.Checked),
                ("city", this.checkBox_FBS_UnsentCityBudget.Checked),
                ("reg", this.checkBox_FBS_UnsentRegBudget.Checked),
                ("uni", this.checkBox_FBS_UnsentUniBudget.Checked),
                ("ext", this.checkBox_FBS_UnsentExtBudget.Checked)
            }
            );
        }
        private void Button_FBS_Cancel_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug($"Button_FBS_Cancel_Click");
            this.Close();
        }
    }
}