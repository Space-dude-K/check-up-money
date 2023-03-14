using System.Configuration;

namespace check_up_money.Settings.Ticket
{
    class TicketObjectElement : ConfigurationElement
    {
        #region Configuration Properties
        [ConfigurationProperty("type", IsRequired = true)]
        public string TicketType
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }
        [ConfigurationProperty("enabled", IsRequired = false)]
        public string IsTicketEnabled
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
        public TicketObjectElement()
        {
        }
        #endregion
    }
}