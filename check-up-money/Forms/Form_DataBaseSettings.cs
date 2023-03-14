using check_up_money.Cypher;
using check_up_money.Db;
using check_up_money.Interfaces;
using check_up_money.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace check_up_money.Forms
{
    public partial class Form_DataBaseSettings : Form
    {
        int selectedListBoxItemIndex = 0;

        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        readonly ICypher cypher;
        readonly IConfigurator conf;
        readonly List<RequisiteInformation> riList;
        readonly IPathsValidator pv;
        private readonly IMain main;

        public Form_DataBaseSettings(ICypher cypher, IConfigurator conf, List<RequisiteInformation> riList, IPathsValidator pv, IMain main)
        {
            InitializeComponent();

            this.cypher = cypher;
            this.conf = conf;
            this.pv = pv;
            this.main = main;
            this.riList = riList;

            InitListBox();
        }
        private void InitListBox()
        {
            this.listBox_DBS_LoadedDbSettings.SelectedIndexChanged += Lb_ItemSelectionChanged;

            if(riList.Count > 0)
            {
                foreach (RequisiteInformation ri in riList)
                {
                    logger.Info($"InitListBox. Host: {ri.Host} Bd: {ri.Db}");

                    this.listBox_DBS_LoadedDbSettings.Items.Add(
                        "Driver: " + ri.Driver +
                        " Host: " + ri.Host + 
                        " Instance: " + ri.Instance +
                        " Bd: " + ri.Db + 
                        " User: " + cypher.ToInsecureString(cypher.DecryptString(ri.User, ri.USalt)) +
                        " Pass: " + "*******"
                        );
                }

                this.listBox_DBS_LoadedDbSettings.SelectedIndex = 0;
            }
        }
        private void InitSecurityTextBox(RequisiteInformation ri)
        {
            this.securityTextBox_DBS_Password.Pb.MouseUp += Pb_MouseUp;
            this.securityTextBox_DBS_Password.Pb.MouseDown += Pb_MouseDown;
            this.securityTextBox_DBS_Password.SetPasswordDummy(0, true);

            this.textBox_DBS_Driver.Text = ri.Driver;
            this.textBox_DBS_ServName.Text = ri.Host;
            this.textBox_DBS_Instance.Text = ri.Instance;
            this.textBox_DBS_Name.Text = ri.Db;
            this.textBox_DBS_User.Text = cypher.ToInsecureString(cypher.DecryptString(ri.User, ri.USalt));
        }
        private void Lb_ItemSelectionChanged(object sender, EventArgs e)
        {
            var listBox = (ListBox)sender;
            selectedListBoxItemIndex = listBox.SelectedIndex;

            // Load into tb
            if(selectedListBoxItemIndex >= 0)
            {
                logger.Info("Load db settings at " + selectedListBoxItemIndex);
                InitSecurityTextBox(riList[selectedListBoxItemIndex]);
            }
        }
        private void Pb_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.securityTextBox_DBS_Password.SecureString.Length != 0)
            {
                this.securityTextBox_DBS_Password.SetPasswordDummy(this.securityTextBox_DBS_Password.SecureString.Length);
            }
        }
        private void Pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.securityTextBox_DBS_Password.SecureString.Length != 0)
            {
                this.securityTextBox_DBS_Password.Tb.Text = cypher.ToInsecureString(this.securityTextBox_DBS_Password.SecureString);
            }  
        }
        private void InitDbSettings(RequisiteInformation ri)
        {
            this.textBox_DBS_ServName.Text = ri.Host;
            this.textBox_DBS_Name.Text = ri.Db;
            this.textBox_DBS_User.Text = cypher.ToInsecureString(cypher.DecryptString(ri.User, ri.USalt));
        }
        private void Button_DBS_Add_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("Button_DBS_Add_Click");
            RequisiteInformation ri = null;

            ri = cypher.Encrypt(
                        this.textBox_DBS_Driver.Text,
                        this.textBox_DBS_ServName.Text,
                        this.textBox_DBS_Instance.Text,
                        this.textBox_DBS_Name.Text,
                        cypher.ToSecureString(this.textBox_DBS_User.Text),
                        this.securityTextBox_DBS_Password.SecureString
                        );

            riList.Add(ri);
            //conf.WriteDbSettings(riList);

            this.listBox_DBS_LoadedDbSettings.Items.Add(
                "Host: " + ri.Host
                + " Bd: " + ri.Db
                + " User: " + cypher.ToInsecureString(cypher.DecryptString(ri.User, ri.USalt)));

            this.listBox_DBS_LoadedDbSettings.SelectedIndex = riList.Count - 1;
        }
        private void Button_DBS_Save_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("Button_DBS_Save_Click");
            //RequisiteInformation ri = null;

            if (pv.IsDbSettingsInputsFromControlsValid(this.Controls, riList, true))
            {
                conf.WriteDbSettings(riList);
                /*
                foreach(var setting in riList)
                {
                    ri = cypher.Encrypt(
                        this.textBox_DBS_Driver.Text,
                        this.textBox_DBS_ServName.Text,
                        this.textBox_DBS_Instance.Text,
                        this.textBox_DBS_Name.Text,
                        cypher.ToSecureString(this.textBox_DBS_User.Text),
                        this.securityTextBox_DBS_Password.SecureString
                        );

                    //riList[selectedListBoxItemIndex] = ri;
                    //riList.Add(ri);

                    this.listBox_DBS_LoadedDbSettings.Items[selectedListBoxItemIndex]
                        = "Host: " + ri.Host
                        + " Bd: " + ri.Db
                        + " User: " + cypher.ToInsecureString(cypher.DecryptString(ri.User, ri.USalt));
                }
                */

            }
        }
        private void Button_DBS_Cancel_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("Button_DBS_Cancel_Click");

            // Re-init unsent file checker.
            main.InitUnsentFileChecker();

            this.Close();
        }
        private void Button_DBS_Delete_Click(object sender, EventArgs e)
        {
            loggerDebug.Debug("Button_DBS_Delete_Click");
            conf.DeleteDbSettings(riList[selectedListBoxItemIndex].Host);
            this.listBox_DBS_LoadedDbSettings.Items.RemoveAt(selectedListBoxItemIndex);
        }
    }
}