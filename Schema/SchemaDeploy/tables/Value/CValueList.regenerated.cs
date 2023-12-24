using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CValueList : List<CValue>
    {
        #region Constructors
        //Basic constructor
        public CValueList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CValueList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CValueList(CValueList list) : base(list.Count)
        {
            foreach (CValue i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CValueList(IList list) : base(list.Count)
        {
            foreach (CValue i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CValueList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CValueList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CValueList(this.GetRange(this.Count - count - 1, count));
        }
        public CValueList Page(int pageSize, int pageIndex)
        {
            return new CValueList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CValueList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CValueList SortBy(string propertyName, bool descending)
        {
            CValueList copy = new CValueList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CValueList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CValueList_SortBy : CReflection.GenericSortBy, IComparer<CValue>
        {
            public CValueList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CValue x, CValue y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CValue i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CValue i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CValue i in this)
                        ids.Add(i.ValueId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CValueList GetByIds(List<int> ids)
        {
            CValueList list = new CValueList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Supplementary List Overloads
        public void Add(   IList<CValue> itemsToAdd)    {   foreach (CValue i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CValue> itemsToRemove) {   foreach (CValue i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on ValueId)
        public CValue GetById(int valueId)
        {
            CValue c = null;
            Index.TryGetValue(valueId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CValue> _index;
        private Dictionary<int,CValue> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CValue>(this.Count);
                        foreach (CValue i in this) 
                            _index[i.ValueId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)

        //Index by ValueInstanceId
        public CValueList GetByInstanceId(int instanceId)
        {
            CValueList temp = null;
            if (! IndexByInstanceId.TryGetValue(instanceId, out temp))
            {
                temp = new CValueList();
                IndexByInstanceId[instanceId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CValueList> _indexByInstanceId;
        private Dictionary<int, CValueList> IndexByInstanceId
        {
            get
            {
                if (null == _indexByInstanceId)
                {
                    Dictionary<int, CValueList> index = new Dictionary<int, CValueList>();
                    CValueList temp = null;
                    foreach (CValue i in this)
                    {
                        if (! index.TryGetValue(i.ValueInstanceId, out temp))
                        {
                            temp = new CValueList();
                            index[i.ValueInstanceId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByInstanceId = index;
                }
                return _indexByInstanceId;
            }
        }
        //Index by ValueKeyName
        public CValueList GetByKeyName(string keyName)
        {
            CValueList temp = null;
            if (! IndexByKeyName.TryGetValue(keyName, out temp))
            {
                temp = new CValueList();
                IndexByKeyName[keyName] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<string, CValueList> _indexByKeyName;
        private Dictionary<string, CValueList> IndexByKeyName
        {
            get
            {
                if (null == _indexByKeyName)
                {
                    Dictionary<string, CValueList> index = new Dictionary<string, CValueList>();
                    CValueList temp = null;
                    foreach (CValue i in this)
                    {
                        if (! index.TryGetValue(i.ValueKeyName, out temp))
                        {
                            temp = new CValueList();
                            index[i.ValueKeyName] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByKeyName = index;
                }
                return _indexByKeyName;
            }
        }
        #endregion

    }
}
