using check_up_money.Cypher;
using check_up_money.Interfaces;
using NLog;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace check_up_money.Db
{
    public class DbConnector
    {
        Logger logger = LogManager.GetLogger("CheckUpMain-logger-info");
        Logger loggerError = LogManager.GetLogger("CheckUpError-logger-error");
        Logger loggerDebug = LogManager.GetLogger("CheckUpMain-logger-debug");

        private SqlConnectionStringBuilder sConnBSqlClient;
        private OdbcConnectionStringBuilder sConnBOdbc;

        // Odbc connector.
        public DbConnector(List<RequisiteInformation> ri, ICypher cypher, bool forcedOdbc)
        {
            sConnBOdbc = new OdbcConnectionStringBuilder();
            sConnBOdbc["Driver"] = ri[0].Driver;
            sConnBOdbc["Server"] = ri[0].Host + @"\" + ri[0].Instance;
            sConnBOdbc["Database"] = ri[0].Db;
            sConnBOdbc["uid"] = cypher.ToInsecureString(cypher.DecryptString(ri[0].User, ri[0].USalt));
            sConnBOdbc["pwd"] = cypher.ToInsecureString(cypher.DecryptString(ri[0].Password, ri[0].PSalt));
        }
        // Sql client connector.
        public DbConnector(List<RequisiteInformation> ri, ICypher cypher)
        {
            sConnBSqlClient = new SqlConnectionStringBuilder()
            {
                DataSource = ri[0].Host + @"\" + ri[0].Instance,
                InitialCatalog = ri[0].Db,
                UserID = cypher.ToInsecureString(cypher.DecryptString(ri[0].User, ri[0].USalt)),
                Password = cypher.ToInsecureString(cypher.DecryptString(ri[0].Password, ri[0].PSalt))
            };
        }
        public DbConnector(RequisiteInformation ri, ICypher cypher)
        {
            if(ri != null)
            {
                sConnBOdbc = new OdbcConnectionStringBuilder();
                sConnBOdbc["Driver"] = ri.Driver;
                sConnBOdbc["Server"] = string.IsNullOrWhiteSpace(ri.Instance) || ri.Instance.Equals("-") ? ri.Host : ri.Host + @"\" 
+ ri.Instance;
                sConnBOdbc["Database"] = ri.Db;
                sConnBOdbc["uid"] = cypher.ToInsecureString(cypher.DecryptString(ri.User, ri.USalt));
                sConnBOdbc["pwd"] = cypher.ToInsecureString(cypher.DecryptString(ri.Password, ri.PSalt));
            }
            else
            {
                loggerError.Error("Requisite section is null.");
            }
        }
        public string ConnectionStringSqlClient
        {
            get { return sConnBSqlClient.ConnectionString; }
        }
        public string ConnectionStringOdbc
        {
            get { return sConnBOdbc.ConnectionString; }
        }
    }
}