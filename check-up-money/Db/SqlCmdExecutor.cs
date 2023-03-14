using System;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using NLog;
using check_up_money.Interfaces;
using check_up_money.CheckUpFile;
using System.ComponentModel;
using System.Windows.Forms;
using check_up_money.Extensions;
using System.Drawing;

namespace check_up_money.Db
{
    // TODO: Рефакторинг.
    class SqlCmdExecutor : ISqlCmdExecutor
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private readonly IRemoteHostAvailabilityChecker rhac;
        private readonly string dbAddress;
        private readonly DbConnector dbConnector;

        public SqlCmdExecutor(IRemoteHostAvailabilityChecker rhac, string dbAddress, DbConnector dbConnector)
        {
            this.rhac = rhac;
            this.dbAddress = dbAddress;
            this.dbConnector = dbConnector;
        }
        // TODO: Переписать через generic.
        /// <summary>
        /// Этот метод запускает <see cref="OdbcCommand"/> команды для БД.
        /// </summary>
        /// <param name="connStr">Строка подключения.</param>
        /// <param name="command"><see cref="OdbcCommand"/> команда.</param>
        /// <param name="control">Лабель для вывода статуса.</param>
        /// <returns>
        /// <see cref="Task"/> с результатом выполнения.
        /// </returns>
        public async Task<object> ExecuteOdbcCmdScalar(string connStr, OdbcCommand command, Control control = null)
        {
            object result = null;

            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(1);
            var timeOut = false;

            var db = connStr.GetDatabaseFromOdbcConnectionString();
            var host = connStr.GetHostFromOdbcConnectionString();

            await rhac.WaitForConnection(connStr, control);

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(100000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug($"Connection state for {db} - {connection.State}");

                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    while (connection.State == ConnectionState.Connecting)
                    {
                        if (startTime.CompareTo(endTime) >= 0)
                        {
                            timeOut = true;
                            break;
                        }

                        await Task.Delay(500);
                    }

                    if(timeOut)
                        loggerError.Error($"Database timeout for {db}");

                    result = await command.ExecuteScalarAsync(cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    if(control != null)
                    {
                        control.ThreadSafeInvokeForTextAssignment("ERR");
                        control.ThreadSafeInvokeForColor(Color.Red);
                    }
                    
                    loggerError.Error($"{ex.GetType()} - {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
        public async Task<BindingList<CheckUpBlobHistory>> ExecuteOdbcCmdForDocsByDateTimeAndUserFromDb(string connStr, OdbcCommand command, Control control = null)
        {
            BindingList<CheckUpBlobHistory> docs = new BindingList<CheckUpBlobHistory>();

            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(1);
            var timeOut = false;

            var db = connStr.GetDatabaseFromOdbcConnectionString();
            var host = connStr.GetHostFromOdbcConnectionString();

            await rhac.WaitForConnection(connStr, control);

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(100000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug($"Connection state for {db} - {connection.State}");

                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    while (connection.State == ConnectionState.Connecting)
                    {
                        if (startTime.CompareTo(endTime) >= 0)
                        {
                            timeOut = true;
                            break;
                        }

                        await Task.Delay(500);
                    }

                    if (timeOut)
                        loggerError.Error($"Database timeout for {db}");

                    var r = await command.ExecuteReaderAsync(cancellationTokenSource.Token);

                    while (await r.ReadAsync())
                    {
                        CheckUpBlobHistory checkUpBlob = new CheckUpBlobHistory();

                        checkUpBlob.FileName = r["FileName"].ToString();
                        checkUpBlob.FileDate = r["Date"].ToString();
                        checkUpBlob.FileType = r["FileType"].ToString();
                        checkUpBlob.BudgetType = r["BudgetType"].ToString();
                        checkUpBlob.TotalDocs = r["TotalDocs"].ToString();
                        checkUpBlob.TotalSumm = r["Summ"].ToString();
                        checkUpBlob.CurrencyType = r["CurrencyType"].ToString();

                        docs.Add(checkUpBlob);
                    }
                }
                catch (Exception ex)
                {
                    if (control != null)
                    {
                        control.ThreadSafeInvokeForTextAssignment("ERR");
                        control.ThreadSafeInvokeForColor(Color.Red);
                    }

                    loggerError.Error($"{ex.GetType()} - {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return docs;
        }
        public async Task<object> ExecuteOdbcCmdForUserId(string connStr, OdbcCommand command, Control control = null)
        {
            int result = 0;

            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(1);
            var timeOut = false;

            var db = connStr.GetDatabaseFromOdbcConnectionString();
            var host = connStr.GetHostFromOdbcConnectionString();

            await rhac.WaitForConnection(connStr, control);

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(100000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug($"Connection state for {db} - {connection.State}");

                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    while (connection.State == ConnectionState.Connecting)
                    {
                        if (startTime.CompareTo(endTime) >= 0)
                        {
                            timeOut = true;
                            break;
                        }

                        await Task.Delay(500);
                    }

                    if (timeOut)
                        loggerError.Error($"Database timeout for {db}");

                    var reader = await command.ExecuteReaderAsync(cancellationTokenSource.Token);

                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader["UserId"]);
                    }
                }
                catch (Exception ex)
                {
                    if (control != null)
                    {
                        control.ThreadSafeInvokeForTextAssignment("ERR");
                        control.ThreadSafeInvokeForColor(Color.Red);
                    }

                    loggerError.Error($"{ex.GetType()} - {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
        public async Task<bool> IsUserExistInDb(string userName, string connStr)
        {
            bool isUserExist = false;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.IsUserExist(connection, userName);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug(connection.State);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    var r = await cmd.ExecuteScalarAsync(cancellationTokenSource.Token);

                    isUserExist = Convert.ToInt32(r) != 0;
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return isUserExist;
        }
        public async Task<int> GetUserId(string userName, string connStr)
        {
            int userId = 0;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.GetUserId(connection, userName);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug(connection.State);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    //var r = await cmd.ExecuteScalarAsync(cancellationTokenSource.Token);
                    var reader = await cmd.ExecuteReaderAsync(cancellationTokenSource.Token);

                    while (reader.Read())
                    {
                        userId = Convert.ToInt32(reader["UserId"]);
                    }
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return userId;
        }
        public async Task<int> CreateNewUser(string userName, DateTime dateTime, string connStr)
        {
            int result = 0;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.CreateNewUser(connection, userName, dateTime);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    var r = await cmd.ExecuteScalarAsync(cancellationTokenSource.Token);

                    result = Convert.ToInt32(r);
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
        public async Task<bool> IsCurrencyTypeExistInDb(string currencyName, string connStr)
        {
            bool isUserExist = false;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.IsCurrencyTypeExist(connection, currencyName);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug(connection.State);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    var r = await cmd.ExecuteScalarAsync(cancellationTokenSource.Token);

                    isUserExist = Convert.ToInt32(r) != 0;
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return isUserExist;
        }
        public async Task<int> GetCurrencyTypeId(string currencyName, string connStr)
        {
            int userId = 0;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.GetCurrencyId(connection, currencyName);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug(connection.State);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    var reader = await cmd.ExecuteReaderAsync(cancellationTokenSource.Token);

                    while (reader.Read())
                    {
                        userId = Convert.ToInt32(reader["CurrencyTypeId"]);
                    }
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return userId;
        }
        public async Task<int> CreateNewCurrencyType(string currencyName, string connStr)
        {
            int result = 0;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.CreateNewCurrencyType(connection, currencyName);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    var r = await cmd.ExecuteScalarAsync(cancellationTokenSource.Token);

                    result = Convert.ToInt32(r);
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
        public async Task<int> CreateNewLogEntry(string connStr, 
            DateTime dateTime, string fileName, decimal summ, int totalDocs, int budgetId, int fileTypeId, int userId, int currencyTypeId)
        {
            int result = 0;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd;

#if DEBUG
                    cmd = SqlCommands.NewLogEntryForTest(connection,
                            dateTime, fileName, summ, totalDocs, budgetId, fileTypeId, userId, currencyTypeId);
#else
                    cmd = SqlCommands.NewLogEntry(connection,
                            dateTime, fileName, summ, totalDocs, budgetId, fileTypeId, userId, currencyTypeId);
#endif

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    var r = await cmd.ExecuteScalarAsync(cancellationTokenSource.Token);

                    result = Convert.ToInt32(r);
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
        public async Task<int> GetUnsentFilesFromDb(string database, string connStr, Control control)
        {
            int unsentDocs = 0;

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.GetDocsStateCodes(connection, database);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(100000);

                    await rhac.WaitForConnection(connStr, control);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug($"Connection state for {database} - {connection.State}");

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    while(connection.State == ConnectionState.Connecting)
                    {
                        // do nothing.
                    }

                    var r = await cmd.ExecuteScalarAsync(cancellationTokenSource.Token);

                    unsentDocs = Convert.ToInt32(r);
                }
                catch (Exception ex)
                {
                    control.ThreadSafeInvokeForTextAssignment("ERR");
                    control.ThreadSafeInvokeForColor(Color.Red);

                    loggerError.Error($"{ex.GetType()} - {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return unsentDocs;
        }
        public async Task<BindingList<CheckUpBlobHistory>> GetDocsByDateTimeAndUserFromDb(string database, string connStr, 
            DateTime dateTimeFrom, DateTime dateTimeTo, string user)
        {
            BindingList<CheckUpBlobHistory> docs = new BindingList<CheckUpBlobHistory>();

            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    OdbcCommand cmd = SqlCommands.GetDocsByDateTimeAndUser(connection, database, dateTimeFrom, dateTimeTo, user);

                    foreach(OdbcParameter param in cmd.Parameters)
                    {
                        loggerDebug.Debug($"Cmd -> {param.Value}");
                    }
                    
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(10000);

                    await connection.OpenAsync(cancellationTokenSource.Token);

                    loggerDebug.Debug(connection.State);

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    var r = await cmd.ExecuteReaderAsync(cancellationTokenSource.Token);

                    while(await r.ReadAsync())
                    {
                        CheckUpBlobHistory checkUpBlob = new CheckUpBlobHistory();

                        checkUpBlob.FileName = r["FileName"].ToString();
                        checkUpBlob.FileDate = r["Date"].ToString();
                        checkUpBlob.FileType = r["FileType"].ToString();
                        checkUpBlob.BudgetType = r["BudgetType"].ToString();
                        checkUpBlob.TotalDocs = r["TotalDocs"].ToString();
                        checkUpBlob.TotalSumm = r["Summ"].ToString();
                        checkUpBlob.CurrencyType = r["CurrencyType"].ToString();

                        docs.Add(checkUpBlob);
                    }
                }
                catch (SqlException ex)
                {
                    loggerError.Error(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return docs;
        }
    }
}