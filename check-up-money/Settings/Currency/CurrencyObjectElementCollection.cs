using System;
using System.Configuration;

namespace check_up_money.Settings.Currency
{
    [ConfigurationCollection(typeof(CurrencyObjectElement), AddItemName = "cs", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    class CurrencyObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CurrencyObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("NullCurrencySetting");

            return ((CurrencyObjectElement)element).BudgetType;
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
        public CurrencyObjectElement this[int index]
        {
            get { return base.BaseGet(index) as CurrencyObjectElement; }
        }
        public new CurrencyObjectElement this[string key]
        {
            get { return base.BaseGet(key) as CurrencyObjectElement; }
        }
        public void Change(string budget, bool changetStatusTo)
        {
            var _enumerator = base.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                CurrencyObjectElement _budget = (CurrencyObjectElement)_enumerator.Current;

                if (budget.Equals(_budget.BudgetType))
                    _budget.IsEnabled = changetStatusTo.ToString();
            }
        }
        public void Clear()
        {
            BaseClear();
        }
        #endregion
    }
}