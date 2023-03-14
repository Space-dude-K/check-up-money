using System.Configuration;

namespace check_up_money.Settings.PathMisc
{
    class PathMiscObjectElement : ConfigurationElement
    {
        #region Configuration Properties
        [ConfigurationProperty("pathType", IsRequired = true)]
        public string PathTypeElement
        {
            get { return (string)this["pathType"]; }
            set { this["pathType"] = value; }
        }
        [ConfigurationProperty("enabled", IsRequired = true)]
        public string PathStatusElement
        {
            get { return (string)this["enabled"]; }
            set { this["enabled"] = value; }
        }
        [ConfigurationProperty("in", IsRequired = false)]
        public string PathElementInValue
        {
            get { return (string)this["in"]; }
            set { this["in"] = value; }
        }
        [ConfigurationProperty("out", IsRequired = false)]
        public string PathElementOutValue
        {
            get { return (string)this["out"]; }
            set { this["out"] = value; }
        }
        private string CheckPath(string rawStr)
        {
            if (rawStr.Contains("="))
            {
                if (rawStr.Remove(0, 2).StartsWith(@"\"))
                {
                    return rawStr;
                }
                else
                {
                    return rawStr.Insert(2, @"\");
                }
            }
            else
            {
                return rawStr;
            }
        }
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        #region Constructors
        public PathMiscObjectElement()
        {
        }
        #endregion
    }
}