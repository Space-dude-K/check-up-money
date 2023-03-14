using System.Configuration;

namespace check_up_money.Settings.Budget
{
    class BudgetObjectElement : ConfigurationElement
    {
        #region Configuration Properties
        [ConfigurationProperty("budget", IsRequired = true)]
        public string BudgetType
        {
            get { return (string)this["budget"]; }
            set { this["budget"] = value; }
        }
        [ConfigurationProperty("enabled", IsRequired = false)]
        public string IsEnabled
        {
            get { return (string)this["enabled"]; }
            set { this["enabled"] = value; }
        }
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        #region Constructors
        public BudgetObjectElement()
        {
        }
        #endregion
    }
}