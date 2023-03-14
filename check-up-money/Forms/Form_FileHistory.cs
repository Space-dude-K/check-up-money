using check_up_money.CheckUpFile;
using check_up_money.Db;
using check_up_money.Extensions;
using check_up_money.Interfaces;
using NLog;
using System;
using System.ComponentModel;
using System.Security.Principal;
using System.Windows.Forms;

namespace check_up_money.Forms
{
    public partial class Form_FileHistory : Form
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private readonly ISqlCmdExecutor sqlExecutor;
        private readonly DbConnector dbConnector;
        private string userName;

        BindingList<CheckUpBlobHistory> docs;

        public Form_FileHistory(ISqlCmdExecutor sqlExecutor, DbConnector dbConnector)
        {
            loggerDebug.Debug($"Init file history form.");

            this.sqlExecutor = sqlExecutor;
            this.dbConnector = dbConnector;

            userName = WindowsIdentity.GetCurrent().GetUserNameFromIdentity();

            loggerDebug.Debug($"User -> {userName}");

            docs = new BindingList<CheckUpBlobHistory>();

            IntiForm();
            InitializeComponent();
            InitiDatePickers();
            InitDataGrid();
        }
        private void IntiForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }
        private void InitDataGrid()
        {
            this.dataGridView_FileHistory.AutoGenerateColumns = false;
            this.dataGridView_FileHistory.DataSource = docs;

            FillDataGrid();
        }
        private void FillDataGrid()
        {
            try
            {
                //var res = sqlExecutor.ExecuteCmdAsync(sqlExecutor.GetDocsByDateTimeAndUserFromDb("CheckUpMoney", dbConnector.ConnectionStringOdbc,
                //dateTimePicker_FileHistory_From.Value, dateTimePicker_FileHistory_To.Value, userName)).Result;

                BindingList<CheckUpBlobHistory> res = sqlExecutor.ExecuteOdbcCmdForDocsByDateTimeAndUserFromDb(dbConnector.ConnectionStringOdbc, 
                    SqlCommands.GetDocsByDateTimeAndUser(null, "CheckUpMoney",
                dateTimePicker_FileHistory_From.Value, dateTimePicker_FileHistory_To.Value, userName)).Result;

                loggerDebug.Debug($"Total docs from db: {res.Count}");

                foreach (CheckUpBlobHistory cb in res)
                {
                    loggerDebug.Debug($"{cb.FileDate} {cb.FileType}");
                    docs.Add(cb);
                }
            }
            catch (Exception ex)
            {
                loggerError.Error(ex);
            }
        }
        private void InitiDatePickers()
        {
            this.dateTimePicker_FileHistory_From.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker_FileHistory_From.CustomFormat = "dd-MM-yyyy";
            this.dateTimePicker_FileHistory_To.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker_FileHistory_To.CustomFormat = "dd-MM-yyyy";
            this.dateTimePicker_FileHistory_From.Value = DateTime.Now;
            this.dateTimePicker_FileHistory_To.Value = DateTime.Now.AddDays(3);
        }
        private void Button_FileHistory_Proceed_Click(object sender, EventArgs e)
        {
            docs.Clear();
            FillDataGrid();
        }
        private void dateTimePicker_FileHistory_From_ValueChanged(object sender, EventArgs e)
        {

        }
        private void dateTimePicker_FileHistory_To_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}