using check_up_money.CheckUpFile;
using System;
using System.ComponentModel;
using System.Data.Odbc;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace check_up_money.Interfaces
{
    public interface ISqlCmdExecutor
    {
        Task<int> CreateNewCurrencyType(string currencyName, string connStr);
        Task<int> CreateNewLogEntry(string connStr, 
            DateTime dateTime, string fileName, decimal summ, int totalDocs, int budgetId, int fileTypeId, int userId, int currencyTypeId);
        Task<int> CreateNewUser(string userName, DateTime dateTime, string connStr);
        Task<BindingList<CheckUpBlobHistory>> ExecuteOdbcCmdForDocsByDateTimeAndUserFromDb(string connStr, OdbcCommand command, Control control = null);
        Task<object> ExecuteOdbcCmdForUserId(string connStr, OdbcCommand command, Control control = null);
        Task<object> ExecuteOdbcCmdScalar(string connStr, OdbcCommand command, Control control = null);
        Task<int> GetCurrencyTypeId(string currencyName, string connStr);
        Task<BindingList<CheckUpBlobHistory>> GetDocsByDateTimeAndUserFromDb(string database, string connStr, 
            DateTime dateTimeFrom, DateTime dateTimeTo, string user);
        Task<int> GetUnsentFilesFromDb(string database, string connStr, Control control);
        Task<int> GetUserId(string userName, string connStr);
        Task<bool> IsCurrencyTypeExistInDb(string currencyName, string connStr);
        Task<bool> IsUserExistInDb(string userName, string connStr);
    }
}