using System;
using System.Configuration;

namespace check_up_money.Settings.UnsentFiles
{
    [ConfigurationCollection(typeof(UnsentFilesObjectElement), AddItemName = "us", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    class UnsentFilesObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UnsentFilesObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("NullUnsentFilesSetting");

            return ((UnsentFilesObjectElement)element).BudgetType;
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
        public UnsentFilesObjectElement this[int index]
        {
            get { return base.BaseGet(index) as UnsentFilesObjectElement; }
        }
        /*
        public new UnsentFilesObjectElement this[string key]
        {
            get { return base.BaseGet(key) as UnsentFilesObjectElement; }
        }
        */
        public new string this[string key]
        {
            get { return base[key] as string; }
            set { base[key] = value; }
        }
        public int GetIndex(string rawBudget)
        {
            int index = 0;
            
            for(int i = 0; i < this.Count; i++)
            {
                if (this[i].BudgetType.Equals(rawBudget))
                    index = i;
            }

            return index;
        }
        
        public void Change(string budget, string changetStatusTo)
        {
            var _enumerator = base.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                UnsentFilesObjectElement _budget = (UnsentFilesObjectElement)_enumerator.Current;

                if (budget.Equals(_budget.BudgetType))
                    _budget.IsEnabled = changetStatusTo;
            }
        }
        [ConfigurationProperty("checkDelayInSeconds", IsDefaultCollection = false)]
        public string CheckDelayInSeconds
        {
            get { return (string)this["checkDelayInSeconds"]; }
            set { this["checkDelayInSeconds"] = value; }
        }
        public void Clear()
        {
            BaseClear();
        }
        #endregion
    }
}