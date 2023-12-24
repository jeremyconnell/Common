using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CPushedUpgradeList : List<CPushedUpgrade>
    {
        #region Constructors
        //Basic constructor
        public CPushedUpgradeList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CPushedUpgradeList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CPushedUpgradeList(CPushedUpgradeList list) : base(list.Count)
        {
            foreach (CPushedUpgrade i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CPushedUpgradeList(IList list) : base(list.Count)
        {
            foreach (CPushedUpgrade i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CPushedUpgradeList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CPushedUpgradeList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CPushedUpgradeList(this.GetRange(this.Count - count - 1, count));
        }
        public CPushedUpgradeList Page(int pageSize, int pageIndex)
        {
            return new CPushedUpgradeList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CPushedUpgradeList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CPushedUpgradeList SortBy(string propertyName, bool descending)
        {
            CPushedUpgradeList copy = new CPushedUpgradeList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CPushedUpgradeList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CPushedUpgradeList_SortBy : CReflection.GenericSortBy, IComparer<CPushedUpgrade>
        {
            public CPushedUpgradeList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CPushedUpgrade x, CPushedUpgrade y) { return base.Compare(x, y); }
        }
        #endregion

        #region SaveAll/DeleteAll
        //Use default connection (may be overridden in base class)
        public void SaveAll()   {   if (this.Count > 0) { SaveAll(  this[0].DataSrc);    }    }
        public void DeleteAll() {   if (this.Count > 0) { DeleteAll(this[0].DataSrc);    }    }

        //Use connection supplied
        public void SaveAll(  CDataSrc dataSrc) {   dataSrc.BulkSave(this);    }
        public void DeleteAll(CDataSrc dataSrc) {   dataSrc.BulkDelete(this);  }

        //Use transaction supplied
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CPushedUpgrade i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CPushedUpgrade i in this) {   i.Delete(txOrNull);   }   }

        //Use a specified isolation level
        public void SaveAll(  IsolationLevel txIsolationLevel)   {   if (this.Count > 0) { SaveAll(  this[0].DataSrc, txIsolationLevel);  }    }
        public void DeleteAll(IsolationLevel txIsolationLevel)   {   if (this.Count > 0) { DeleteAll(this[0].DataSrc, txIsolationLevel);  }    }

        //Use a specified connection and isolation level
        public void SaveAll(  CDataSrc dataSrc, IsolationLevel txIsolationLevel)   {   dataSrc.BulkSave(  this, txIsolationLevel);  }
        public void DeleteAll(CDataSrc dataSrc, IsolationLevel txIsolationLevel)   {   dataSrc.BulkDelete(this, txIsolationLevel);  }
        #endregion

        #region List of Ids
        List<int> _ids;
        public List<int> Ids
        {
            get
            {
                if (null == _ids)
                {
                    List<int> ids = new List<int>(this.Count);
                    foreach (CPushedUpgrade i in this)
                        ids.Add(i.PushId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CPushedUpgradeList GetByIds(List<int> ids)
        {
            CPushedUpgradeList list = new CPushedUpgradeList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CPushedUpgrade item)
        {
            if (null != _index && ! _index.ContainsKey(item.PushId))
                _index[item.PushId] = item;
            base.Add(item);
        }
        public new void Remove(CPushedUpgrade item)
        {
            if (null != _index && _index.ContainsKey(item.PushId))
                _index.Remove(item.PushId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CPushedUpgrade> itemsToAdd)    {   foreach (CPushedUpgrade i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CPushedUpgrade> itemsToRemove) {   foreach (CPushedUpgrade i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on PushId)
        public CPushedUpgrade GetById(int pushId)
        {
            CPushedUpgrade c = null;
            Index.TryGetValue(pushId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CPushedUpgrade> _index;
        private Dictionary<int,CPushedUpgrade> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CPushedUpgrade>(this.Count);
                        foreach (CPushedUpgrade i in this) 
                            _index[i.PushId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by PushInstanceId
        public CPushedUpgradeList GetByInstanceId(int instanceId)
        {
            CPushedUpgradeList temp = null;
            if (! IndexByInstanceId.TryGetValue(instanceId, out temp))
            {
                temp = new CPushedUpgradeList();
                IndexByInstanceId[instanceId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CPushedUpgradeList> _indexByInstanceId;
        private Dictionary<int, CPushedUpgradeList> IndexByInstanceId
        {
            get
            {
                if (null == _indexByInstanceId)
                {
                    Dictionary<int, CPushedUpgradeList> index = new Dictionary<int, CPushedUpgradeList>();
                    CPushedUpgradeList temp = null;
                    foreach (CPushedUpgrade i in this)
                    {
                        if (! index.TryGetValue(i.PushInstanceId, out temp))
                        {
                            temp = new CPushedUpgradeList();
                            index[i.PushInstanceId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByInstanceId = index;
                }
                return _indexByInstanceId;
            }
        }
        //Index by PushOldVersionId
        public CPushedUpgradeList GetByOldVersionId(int oldVersionId)
        {
            CPushedUpgradeList temp = null;
            if (! IndexByOldVersionId.TryGetValue(oldVersionId, out temp))
            {
                temp = new CPushedUpgradeList();
                IndexByOldVersionId[oldVersionId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CPushedUpgradeList> _indexByOldVersionId;
        private Dictionary<int, CPushedUpgradeList> IndexByOldVersionId
        {
            get
            {
                if (null == _indexByOldVersionId)
                {
                    Dictionary<int, CPushedUpgradeList> index = new Dictionary<int, CPushedUpgradeList>();
                    CPushedUpgradeList temp = null;
                    foreach (CPushedUpgrade i in this)
                    {
                        if (! index.TryGetValue(i.PushOldVersionId, out temp))
                        {
                            temp = new CPushedUpgradeList();
                            index[i.PushOldVersionId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByOldVersionId = index;
                }
                return _indexByOldVersionId;
            }
        }
        //Index by PushNewVersionId
        public CPushedUpgradeList GetByNewVersionId(int newVersionId)
        {
            CPushedUpgradeList temp = null;
            if (! IndexByNewVersionId.TryGetValue(newVersionId, out temp))
            {
                temp = new CPushedUpgradeList();
                IndexByNewVersionId[newVersionId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CPushedUpgradeList> _indexByNewVersionId;
        private Dictionary<int, CPushedUpgradeList> IndexByNewVersionId
        {
            get
            {
                if (null == _indexByNewVersionId)
                {
                    Dictionary<int, CPushedUpgradeList> index = new Dictionary<int, CPushedUpgradeList>();
                    CPushedUpgradeList temp = null;
                    foreach (CPushedUpgrade i in this)
                    {
                        if (! index.TryGetValue(i.PushNewVersionId, out temp))
                        {
                            temp = new CPushedUpgradeList();
                            index[i.PushNewVersionId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByNewVersionId = index;
                }
                return _indexByNewVersionId;
            }
        }
        #endregion

    }
}
