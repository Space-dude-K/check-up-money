using System;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace check_up_money.Db
{
    public static class SqlCommands
    {
        internal static SqlCommand WriteToTblProcessedFiles(SqlConnection connection, string fileName, DateTime dateTime, decimal summ, int quantityDoc, int budget)
        {
            SqlCommand cmd =
                new SqlCommand(@"
                USE [CheckUpMoney]
                INSERT INTO [dbo].[tblProcessedFiles]
                ([Id], [Date], [Amount], [QuantityDoc], [Budget])
                VALUES
                (<Date, datetime,>, <Amount, float,>, <QuantityDoc, int,>, <Budget, int,>)",
                connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@fileName", fileName);
            cmd.Parameters.AddWithValue("@dateTime", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@summ", summ);
            cmd.Parameters.AddWithValue("@quantityDoc", quantityDoc);

            return cmd;
        }
        internal static OdbcCommand IsUserExist(OdbcConnection connection, string userName)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                SELECT COUNT(*)
                FROM [dbo].[tblUserRm]
                WHERE [Name]= ?", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Name", OdbcType.NVarChar).Value = userName;

            return cmd;
        }
        internal static OdbcCommand GetUserId(OdbcConnection connection, string userName)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                SELECT [UserId]
                FROM [dbo].[tblUserRm]
                WHERE [Name]= ?", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Name", OdbcType.NVarChar).Value = userName;

            return cmd;
        }
        internal static OdbcCommand CreateNewUser(OdbcConnection connection, string userName, DateTime dateTime)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                INSERT INTO [dbo].[tblUserRm]
                    ([Name], [CreatedAt])
                VALUES
                    (?, ?)", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Name", OdbcType.NVarChar).Value = userName;
            cmd.Parameters.Add("@CreatedAt", OdbcType.DateTime).Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            return cmd;
        }
        internal static OdbcCommand IsCurrencyTypeExist(OdbcConnection connection, string currencyName)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                SELECT COUNT(*)
                FROM [dbo].[tblCurrencyTypeRm]
                WHERE [Name]= ?", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Name", OdbcType.NVarChar).Value = currencyName;

            return cmd;
        }
        internal static OdbcCommand GetCurrencyId(OdbcConnection connection, string currencyName)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                SELECT [CurrencyTypeId]
                FROM [dbo].[tblCurrencyTypeRm]
                WHERE [Name]= ?", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Name", OdbcType.NVarChar).Value = currencyName;

            return cmd;
        }
        internal static OdbcCommand CreateNewCurrencyType(OdbcConnection connection, string currencyName)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                INSERT INTO [dbo].[tblCurrencyTypeRm]
                    ([Name])
                VALUES
                    (?)", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Name", OdbcType.NVarChar).Value = currencyName;

            return cmd;
        }
        internal static OdbcCommand NewLogEntry(OdbcConnection connection,
            DateTime dateTime, string fileName, decimal summ, int totalDocs, int budgetId, int fileTypeId, int userId, int currencyTypeId)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                INSERT INTO [dbo].[tblProcessedFileRm]
                    ([Date],[FileName],[Summ],[TotalDocs],[BudgetId],[FileTypeId],[UserId],[CurrencyTypeId])
                VALUES
                    (?, ?, ?, ?, ?, ?, ?, ?)", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Date", OdbcType.DateTime).Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@FileName", OdbcType.NVarChar).Value = fileName;
            cmd.Parameters.Add(@"Summ", OdbcType.Decimal).Value = summ;
            cmd.Parameters.Add("@TotalDocs", OdbcType.Int).Value = totalDocs;
            cmd.Parameters.Add(@"BudgetId", OdbcType.Int).Value = budgetId;
            cmd.Parameters.Add(@"FileTypeId", OdbcType.Int).Value = fileTypeId;
            cmd.Parameters.Add("@UserId", OdbcType.Int).Value = userId;
            cmd.Parameters.Add(@"CurrencyTypeId", OdbcType.Int).Value = currencyTypeId;

            return cmd;
        }
        internal static OdbcCommand NewLogEntryForTest(OdbcConnection connection,
            DateTime dateTime, string fileName, decimal summ, int totalDocs, int budgetId, int fileTypeId, int userId, int currencyTypeId)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                INSERT INTO [dbo].[tblProcessedFileTestRm]
                    ([Date],[FileName],[Summ],[TotalDocs],[BudgetId],[FileTypeId],[UserId],[CurrencyTypeId])
                VALUES
                    (?, ?, ?, ?, ?, ?, ?, ?)", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@Date", OdbcType.DateTime).Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@FileName", OdbcType.NVarChar).Value = fileName;
            cmd.Parameters.Add(@"Summ", OdbcType.Decimal).Value = summ;
            cmd.Parameters.Add("@TotalDocs", OdbcType.Int).Value = totalDocs;
            cmd.Parameters.Add(@"BudgetId", OdbcType.Int).Value = budgetId;
            cmd.Parameters.Add(@"FileTypeId", OdbcType.Int).Value = fileTypeId;
            cmd.Parameters.Add("@UserId", OdbcType.Int).Value = userId;
            cmd.Parameters.Add(@"CurrencyTypeId", OdbcType.Int).Value = currencyTypeId;

            return cmd;
        }
        internal static OdbcCommand GetDocsStateCodes(OdbcConnection connection, string database)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [" + database + "] SELECT " +
                "(SELECT COUNT([Id]) FROM [" + database +"].[dbo].[PaymentDoc] " +
                "WHERE [StateDocCode] = 5 OR [StateDocCode] = 10) + " +
                "(SELECT COUNT([Id]) FROM [" + database + "].[dbo].[IncomePaymentDocs]" +
                " WHERE [StateDocCode] = 5 OR [StateDocCode] = 10)", connection);

            cmd.CommandType = System.Data.CommandType.Text;

            return cmd;
        }
        internal static OdbcCommand GetDocsByDateTimeAndUser(OdbcConnection connection, string database, 
            DateTime dateTimeFrom, DateTime dateTimeTo, string user)
        {
            OdbcCommand cmd =
                new OdbcCommand(@"
                USE [CheckUpMoney]
                SELECT
                dbo.tblProcessedFileRm.Date,
                dbo.tblProcessedFileRm.FileName,
                dbo.tblProcessedFileRm.Summ,
                dbo.tblCurrencyTypeRm.Name AS CurrencyType,
                dbo.tblProcessedFileRm.TotalDocs,
                dbo.tblBudgetRm.Name AS BudgetType,
                dbo.tblFileTypeRm.Name AS FileType,
                dbo.tblUserRm.Name
                FROM [CheckUpMoney].[dbo].[tblProcessedFileRm]
                INNER JOIN dbo.tblBudgetRm ON dbo.tblProcessedFileRm.BudgetId = dbo.tblBudgetRm.BudgetId
                INNER JOIN dbo.tblFileTypeRm ON dbo.tblProcessedFileRm.FileTypeId = dbo.tblFileTypeRm.FileTypeId
                INNER JOIN dbo.tblUserRm ON dbo.tblProcessedFileRm.UserId = dbo.tblUserRm.UserId
                INNER JOIN dbo.tblCurrencyTypeRm ON dbo.tblProcessedFileRm.CurrencyTypeId = dbo.tblCurrencyTypeRm.CurrencyTypeId
                WHERE (CONVERT(varchar(10), dbo.tblProcessedFileRm.Date, 120) BETWEEN ? AND ?) AND dbo.tblUserRm.Name = ?
                ORDER BY [Date] Asc", connection);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add("@dateTimeFrom", OdbcType.VarChar).Value = dateTimeFrom.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@dateTimeTo", OdbcType.VarChar).Value = dateTimeTo.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@dbo.tblUserRm.Name", OdbcType.VarChar).Value = user;

            System.Diagnostics.Debug.WriteLine(cmd.CommandText);

            return cmd;
        }
    }
}