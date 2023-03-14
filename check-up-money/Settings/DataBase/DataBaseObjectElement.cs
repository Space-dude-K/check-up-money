using System.Configuration;

namespace check_up_money.Settings.DataBase
{
    class DataBaseObjectElement : ConfigurationElement
    {
        #region Configuration Properties
        [ConfigurationProperty("driver", IsRequired = false)]
        public string DriverElement
        {
            get { return (string)this["driver"]; }
            set { this["driver"] = value; }
        }
        [ConfigurationProperty("host", IsRequired = false)]
        public string HostElement
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }
        [ConfigurationProperty("instance", IsRequired = false)]
        public string InstanceElement
        {
            get { return (string)this["instance"]; }
            set { this["instance"] = value; }
        }
        [ConfigurationProperty("db", IsRequired = false)]
        public string DataBaseElement
        {
            get { return (string)this["db"]; }
            set { this["db"] = value; }
        }
        [ConfigurationProperty("user", IsRequired = false)]
        public string DataBaseElementUser
        {
            get { return (string)this["user"]; }
            set { this["user"] = value; }
        }
        [ConfigurationProperty("password", IsRequired = false)]
        public string DataBaseElementPassword
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }
        [ConfigurationProperty("uSalt", IsRequired = false)]
        public string DataBaseElementUserSalt
        {
            get { return (string)this["uSalt"]; }
            set { this["uSalt"] = value; }
        }
        [ConfigurationProperty("pSalt", IsRequired = false)]
        public string DataBaseElementPasswordSalt
        {
            get { return (string)this["pSalt"]; }
            set { this["pSalt"] = value; }
        }
        public DataBaseObjectElement(string driver, string host, string instance, string db, string user, string uSalt, string password, string pSalt)
        {
            DriverElement = driver;
            HostElement = host;
            InstanceElement = instance;
            DataBaseElement = db;
            DataBaseElementUser = user;
            DataBaseElementUserSalt = uSalt;
            DataBaseElementPassword = password;
            DataBaseElementPasswordSalt = pSalt;
        }
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        #region Constructors
        public DataBaseObjectElement()
        {
        }
        #endregion
    }
}