using System.Security;
namespace check_up_money.Cypher
{
    public class RequisiteInformation
    {
        private string driver;
        public string Driver
        {
            get { return driver; }
            set { driver = value; }
        }
        private string host;

        public string Host
        {
            get { return host; }
            set { host = value; }
        }
        private string instance;
        public string Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        private string bd;

        public string Db
        {
            get { return bd; }
            set { bd = value; }
        }
        private SecureString user;

        public SecureString User
        {
            get { return user; }
            set { user = value; }
        }
        private SecureString password;

        public SecureString Password
        {
            get { return password; }
            set { password = value; }
        }
        private string uSalt;

        public string USalt
        {
            get { return uSalt; }
            set { uSalt = value; }
        }
        private string pSalt;

        public string PSalt
        {
            get { return pSalt; }
            set { pSalt = value; }
        }
        public RequisiteInformation(
            string driver, string host, string instance, string bd, SecureString user, string uSalt, SecureString password, string pSalt)
        {
            Driver = driver;
            Host = host;
            Instance = instance;
            Db = bd;
            User = user;
            USalt = uSalt;
            Password = password;
            PSalt = pSalt;
        }

        public RequisiteInformation()
        {
            // TODO: Complete member initialization
        }
    }
}