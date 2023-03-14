using System;
using System.Configuration;

namespace check_up_money.Settings.DataBase
{
    [ConfigurationCollection(typeof(DataBaseObjectElement), AddItemName = "ds", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    class DataBaseObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DataBaseObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("NullDataBaseSetting");

            return ((DataBaseObjectElement)element).DataBaseElementPasswordSalt;
            //return element;
        }
        #region Add/Remove/Clear/Indexer Methods
        public void Add(DataBaseObjectElement dboe)
        {
            BaseAdd(dboe);
        }
        public void Remove(string name)
        {
            BaseRemove(name);
        }
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        public DataBaseObjectElement this[int index]
        {
            get { return base.BaseGet(index) as DataBaseObjectElement; }
        }

        public DataBaseObjectElement this[string host, string bd]
        {
            get 
            {
                DataBaseObjectElement _resultDboe = null;

                var _enumerator = base.GetEnumerator();

                while (_enumerator.MoveNext())
                {
                    DataBaseObjectElement _dboe = (DataBaseObjectElement)_enumerator.Current;

                    if (host.Equals(_dboe.HostElement) && bd.Equals(_dboe.DataBaseElement))
                        _resultDboe = base.BaseGet(host) as DataBaseObjectElement;


                }

                //if (_resultDboe == null)
                    //_resultDboe = new DataBaseObjectElement(host, bd);

                    //throw new IndexOutOfRangeException("DataBaseObjectElement index is not exist.");

                return _resultDboe;
            }
        }
        public DataBaseObjectElement this[string host]
        {
            get
            {
                DataBaseObjectElement _resultDboe = null;

                var _enumerator = base.GetEnumerator();

                while (_enumerator.MoveNext())
                {
                    DataBaseObjectElement _dboe = (DataBaseObjectElement)_enumerator.Current;

                    if (host.Equals(_dboe.HostElement))
                        _resultDboe = base.BaseGet(host) as DataBaseObjectElement;


                }

                return _resultDboe;
            }
        }
        public void Clear()
        {
            BaseClear();
        }
        #endregion
    }
}
