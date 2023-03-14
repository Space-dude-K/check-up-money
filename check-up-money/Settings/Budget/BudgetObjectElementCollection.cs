using System;
using System.Configuration;

namespace check_up_money.Settings.Budget
{
    [ConfigurationCollection(typeof(BudgetObjectElement), AddItemName = "bs", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    class BudgetObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BudgetObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("NullBudgetSetting");

            return ((BudgetObjectElement)element).BudgetType;
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
        public BudgetObjectElement this[int index]
        {
            get { return base.BaseGet(index) as BudgetObjectElement; }
        }
        public new BudgetObjectElement this[string key]
        {
            get { return base.BaseGet(key) as BudgetObjectElement; }
        }
        public void Change(string budget, bool changetStatusTo)
        {
            var _enumerator = base.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                BudgetObjectElement _budget = (BudgetObjectElement)_enumerator.Current;

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