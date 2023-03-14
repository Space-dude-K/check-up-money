using check_up_money.Cypher;
using check_up_money.Extensions;
using check_up_money.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money.ValidatorsAndCheckers
{
    // TODO: Рефакторинг.
    class RemoteHostAvailabilityChecker : IRemoteHostAvailabilityChecker
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        int currentExecuteAttempts = 0;

        public RemoteHostAvailabilityChecker()
        {
            loggerDebug.Debug("Init.");
        }
        public bool CheckDbFromRi(List<(string budgetType, RequisiteInformation ri, Control control, bool isEnabled)> list)
        {
            List<bool> dbStatuses = new();

            foreach(var dbSet in list)
            {
                bool s = CheckRealServer(dbSet.ri.Host);
                loggerDebug.Debug($"Check {dbSet.ri.Host} -> {s}");
                dbStatuses.Add(s);
            }

            return dbStatuses.Any(db => !db);
        }
        public async Task<bool> WaitForConnection(string connectionStr, Control control = null)
        {
            while (!CheckRealServer(connectionStr.GetHostFromOdbcConnectionString()))
            {
                loggerError.Error($"Waiting connection for {connectionStr.GetHostFromOdbcConnectionString()} ...");

                if (control != null)
                {
                    control.ThreadSafeInvokeForTextAssignment("ERR");
                    control.ThreadSafeInvokeForColor(Color.Red);
                }

                await Task.Delay(10000);

                currentExecuteAttempts++;
            }

            loggerDebug.Debug($"Connection is ok for {connectionStr.GetHostFromOdbcConnectionString()}");

            return true;
        }
        public bool CheckRealServer(string serverIpOrDnsName)
        {
            bool result = false;
            PingReply pingReply;

            loggerDebug.Debug($"Check {serverIpOrDnsName}");

            try
            {
                using (var ping = new Ping())
                {
                    pingReply = ping.Send(serverIpOrDnsName);
                    result = pingReply.Status == IPStatus.Success;
                }
            }
            catch (PingException ex)
            {
                loggerError.Error($"Remote host {serverIpOrDnsName} is unavailable. {ex.Message}");
            }

            return result;
        }
        public async Task<bool> CheckHostAndBdSync(string host, string connectionStr)
        {
            return CheckRealServer(host) && await CheckRealBd(connectionStr);
        }
        public async Task<bool> CheckHostAndBd(string connectionStr)
        {
            string host = connectionStr.GetHostFromOdbcConnectionString();
            return CheckRealServer(host) && await IsConnectionViable(connectionStr);
        }
        public async Task<bool> IsConnectionViable(string connectionStr)
        {
            await using var sqlConn = new OdbcConnection(connectionStr);
            
            return await IsConnectionViable(sqlConn);
        }
        public async Task<bool> IsConnectionViable(OdbcConnection connection)
        {
            var isConnected = false;

            try
            {
                await connection.OpenAsync();
                isConnected = (connection.State == ConnectionState.Open);
            }
            catch (Exception)
            {
                // ignored
            }

            //loggerError.Error($"Is {connection.ConnectionString} is connected? - {isConnected}");

            return isConnected;
        }
        public async Task<bool> CheckRealBd(string connStr)
        {
            using (var connection = new OdbcConnection(connStr))
            {
                try
                {
                    await connection.OpenAsync();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}