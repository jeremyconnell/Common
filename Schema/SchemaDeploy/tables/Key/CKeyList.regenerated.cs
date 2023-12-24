using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CKeyList : List<CKey>
    {
        #region Constructors
        //Basic constructor
        public CKeyList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CKeyList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CKeyList(CKeyList list) : base(list.Count)
        {
            foreach (CKey i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CKeyList(IList list) : base(list.Count)
        {
            foreach (CKey i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CKeyList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CKeyList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CKeyList(this.GetRange(this.Count - count - 1, count));
        }
        public CKeyList Page(int pageSize, int pageIndex)
        {
            return new CKeyList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CKeyList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CKeyList SortBy(string propertyName, bool descending)
        {
            CKeyList copy = new CKeyList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CKeyList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CKeyList_SortBy : CReflection.GenericSortBy, IComparer<CKey>
        {
            public CKeyList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CKey x, CKey y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CKey i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CKey i in this) {   i.Delete(txOrNull);   }   }

        //Use a specified isolation level
        public void SaveAll(  IsolationLevel txIsolationLevel)   {   if (this.Count > 0) { SaveAll(  this[0].DataSrc, txIsolationLevel);  }    }
        public void DeleteAll(IsolationLevel txIsolationLevel)   {   if (this.Count > 0) { DeleteAll(this[0].DataSrc, txIsolationLevel);  }    }

        //Use a specified connection and isolation level
        public void SaveAll(  CDataSrc dataSrc, IsolationLevel txIsolationLevel)   {   dataSrc.BulkSave(  this, txIsolationLevel);  }
        public void DeleteAll(CDataSrc dataSrc, IsolationLevel txIsolationLevel)   {   dataSrc.BulkDelete(this, txIsolationLevel);  }
        #endregion

        #region List of Ids
        List<string> _ids;
        public List<string> Ids
        {
            get
            {
                if (null == _ids)
                {
                    List<string> ids = new List<string>(this.Count);
                    foreach (CKey i in this)
                        ids.Add(i.KeyName);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CKeyList GetByIds(List<string> ids)
        {
            CKeyList list = new CKeyList(ids.Count);
            foreach (string id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        //Supplementary List Overloads
        public void Add(   IList<CKey> itemsToAdd)    {   foreach (CKey i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CKey> itemsToRemove) {   foreach (CKey i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on KeyName)
        public CKey GetById(string keyName)
        {
            CKey c = null;
            Index.TryGetValue(keyName, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<string,CKey> _index;
        private Dictionary<string,CKey> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<string,CKey>(this.Count);
                        foreach (CKey i in this) 
                            _index[i.KeyName] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by KeyGroupId
        public CKeyList GetByGroupId(int groupId)
        {
            CKeyList temp = null;
            if (! IndexByGroupId.TryGetValue(groupId, out temp))
            {
                temp = new CKeyList();
                IndexByGroupId[groupId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CKeyList> _indexByGroupId;
        private Dictionary<int, CKeyList> IndexByGroupId
        {
            get
            {
                if (null == _indexByGroupId)
                {
                    Dictionary<int, CKeyList> index = new Dictionary<int, CKeyList>();
                    CKeyList temp = null;
                    foreach (CKey i in this)
                    {
                        if (! index.TryGetValue(i.KeyGroupId, out temp))
                        {
                            temp = new CKeyList();
                            index[i.KeyGroupId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByGroupId = index;
                }
                return _indexByGroupId;
            }
        }
        //Index by KeyFormatId
        public CKeyList GetByFormatId(int formatId)
        {
            CKeyList temp = null;
            if (! IndexByFormatId.TryGetValue(formatId, out temp))
            {
                temp = new CKeyList();
                IndexByFormatId[formatId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CKeyList> _indexByFormatId;
        private Dictionary<int, CKeyList> IndexByFormatId
        {
            get
            {
                if (null == _indexByFormatId)
                {
                    Dictionary<int, CKeyList> index = new Dictionary<int, CKeyList>();
                    CKeyList temp = null;
                    foreach (CKey i in this)
                    {
                        if (! index.TryGetValue(i.KeyFormatId, out temp))
                        {
                            temp = new CKeyList();
                            index[i.KeyFormatId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByFormatId = index;
                }
                return _indexByFormatId;
            }
        }
        //Index by KeyDefaultBoolean
        public CKeyList GetByDefaultBoolean(bool? defaultBoolean)
        {
            CKeyList temp = null;
            if (! IndexByDefaultBoolean.TryGetValue(defaultBoolean, out temp))
            {
                temp = new CKeyList();
                IndexByDefaultBoolean[defaultBoolean] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<bool?, CKeyList> _indexByDefaultBoolean;
        private Dictionary<bool?, CKeyList> IndexByDefaultBoolean
        {
            get
            {
                if (null == _indexByDefaultBoolean)
                {
                    var index = new Dictionary<bool?, CKeyList>();
                    CKeyList temp = null;
                    foreach (CKey i in this)
                    {
                        if (! index.TryGetValue(i.KeyDefaultBoolean, out temp))
                        {
                            temp = new CKeyList();
                            index[i.KeyDefaultBoolean] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByDefaultBoolean = index;
                }
                return _indexByDefaultBoolean;
            }
        }
        //Index by KeyDefaultInteger
        public CKeyList GetByDefaultInteger(int? defaultInteger)
        {
            CKeyList temp = null;
            if (! IndexByDefaultInteger.TryGetValue(defaultInteger, out temp))
            {
                temp = new CKeyList();
                IndexByDefaultInteger[defaultInteger] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int?, CKeyList> _indexByDefaultInteger;
        private Dictionary<int?, CKeyList> IndexByDefaultInteger
        {
            get
            {
                if (null == _indexByDefaultInteger)
                {
                    var index = new Dictionary<int?, CKeyList>();
                    CKeyList temp = null;
                    foreach (CKey i in this)
                    {
                        if (! index.TryGetValue(i.KeyDefaultInteger, out temp))
                        {
                            temp = new CKeyList();
                            index[i.KeyDefaultInteger] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByDefaultInteger = index;
                }
                return _indexByDefaultInteger;
            }
        }
        //Index by KeyIsEncrypted
        public CKeyList GetByIsEncrypted(bool isEncrypted)
        {
            CKeyList temp = null;
            if (! IndexByIsEncrypted.TryGetValue(isEncrypted, out temp))
            {
                temp = new CKeyList();
                IndexByIsEncrypted[isEncrypted] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<bool, CKeyList> _indexByIsEncrypted;
        private Dictionary<bool, CKeyList> IndexByIsEncrypted
        {
            get
            {
                if (null == _indexByIsEncrypted)
                {
                    Dictionary<bool, CKeyList> index = new Dictionary<bool, CKeyList>();
                    CKeyList temp = null;
                    foreach (CKey i in this)
                    {
                        if (! index.TryGetValue(i.KeyIsEncrypted, out temp))
                        {
                            temp = new CKeyList();
                            index[i.KeyIsEncrypted] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByIsEncrypted = index;
                }
                return _indexByIsEncrypted;
            }
        }
        #endregion

    }
}
