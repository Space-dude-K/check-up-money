using System;
using System.Configuration;

namespace check_up_money.Settings.Path
{
    [ConfigurationCollection(typeof(PathObjectElement), AddItemName = "ps", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    class PathObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PathObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("PathObjectSetting");

            return ((PathObjectElement)element).PathTypeElement;
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
        public PathObjectElement this[int index]
        {
            get { return base.BaseGet(index) as PathObjectElement; }
        }
        public new PathObjectElement this[string key]
        {
            get { return base.BaseGet(key) as PathObjectElement; }
        }
        public void Change(string pathType, bool changetStatusTo)
        {
            var _enumerator = base.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                PathObjectElement _poe = (PathObjectElement)_enumerator.Current;

                if (pathType.Equals(_poe.PathTypeElement))
                    _poe.PathElementValue = changetStatusTo.ToString();
            }
        }
        public void Clear()
        {
            BaseClear();
        }
        #endregion
    }
}