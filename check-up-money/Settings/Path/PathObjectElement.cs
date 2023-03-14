using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace check_up_money.Settings.Path
{
    class PathObjectElement : ConfigurationElement
    {
        #region Configuration Properties
        [ConfigurationProperty("pathType", IsRequired = true)]
        public string PathTypeElement
        {
            get { return (string)this["pathType"]; }
            set { this["pathType"] = value; }
        }
        [ConfigurationProperty("value", IsRequired = false)]
        public string PathElementValue
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
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
        public PathObjectElement()
        {
        }
        #endregion
    }
}