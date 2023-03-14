using System;
using System.Configuration;

namespace check_up_money.Settings.Ticket
{
    [ConfigurationCollection(typeof(TicketObjectElement), AddItemName = "ts", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    class TicketObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TicketObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("NullTickettSetting");

            return ((TicketObjectElement)element).TicketType;
            //return element;
        }
        #region Add/Remove/Clear/Indexer Methods
        public void Remove(string name)
        {
            BaseRemove(name);
        }
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        public TicketObjectElement this[int index]
        {
            get { return base.BaseGet(index) as TicketObjectElement; }
        }
        public new TicketObjectElement this[string key]
        {
            get { return base.BaseGet(key) as TicketObjectElement; }
        }
        public void Change(string type, bool changetStatusTo)
        {
            var _enumerator = base.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                TicketObjectElement _budget = (TicketObjectElement)_enumerator.Current;

                if (type.Equals(_budget.TicketType))
                    _budget.IsTicketEnabled = changetStatusTo.ToString();
            }
        }
        public void Clear()
        {
            BaseClear();
        }
        #endregion
    }
}