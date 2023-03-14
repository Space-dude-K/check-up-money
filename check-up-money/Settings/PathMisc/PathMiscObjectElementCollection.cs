using System;
using System.Configuration;

namespace check_up_money.Settings.PathMisc
{
    [ConfigurationCollection(typeof(PathMiscObjectElement), AddItemName = "ps", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    class PathMiscObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PathMiscObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("PathObjectSetting");

            return ((PathMiscObjectElement)element).PathTypeElement;
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
        public PathMiscObjectElement this[int index]
        {
            get { return base.BaseGet(index) as PathMiscObjectElement; }
        }
        public new PathMiscObjectElement this[string key]
        {
            get { return base.BaseGet(key) as PathMiscObjectElement; }
        }
        public void Change(string pathType, bool changetStatusTo)
        {
            var _enumerator = base.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                PathMiscObjectElement _poe = (PathMiscObjectElement)_enumerator.Current;

                if (pathType.Equals(_poe.PathTypeElement))
                    _poe.PathElementInValue = changetStatusTo.ToString();
            }
        }
        public void Clear()
        {
            BaseClear();
        }
        #endregion
    }
}