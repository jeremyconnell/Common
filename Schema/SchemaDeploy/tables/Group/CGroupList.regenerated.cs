using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CGroupList : List<CGroup>
    {
        #region Constructors
        //Basic constructor
        public CGroupList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CGroupList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CGroupList(CGroupList list) : base(list.Count)
        {
            foreach (CGroup i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CGroupList(IList list) : base(list.Count)
        {
            foreach (CGroup i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CGroupList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CGroupList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CGroupList(this.GetRange(this.Count - count - 1, count));
        }
        public CGroupList Page(int pageSize, int pageIndex)
        {
            return new CGroupList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CGroupList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CGroupList SortBy(string propertyName, bool descending)
        {
            CGroupList copy = new CGroupList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CGroupList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CGroupList_SortBy : CReflection.GenericSortBy, IComparer<CGroup>
        {
            public CGroupList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CGroup x, CGroup y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CGroup i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CGroup i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CGroup i in this)
                        ids.Add(i.GroupId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CGroupList GetByIds(List<int> ids)
        {
            CGroupList list = new CGroupList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CGroup item)
        {
            if (null != _index && ! _index.ContainsKey(item.GroupId))
                _index[item.GroupId] = item;
            base.Add(item);
        }
        public new void Remove(CGroup item)
        {
            if (null != _index && _index.ContainsKey(item.GroupId))
                _index.Remove(item.GroupId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CGroup> itemsToAdd)    {   foreach (CGroup i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CGroup> itemsToRemove) {   foreach (CGroup i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on GroupId)
        public CGroup GetById(int groupId)
        {
            CGroup c = null;
            Index.TryGetValue(groupId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CGroup> _index;
        private Dictionary<int,CGroup> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CGroup>(this.Count);
                        foreach (CGroup i in this) 
                            _index[i.GroupId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by GroupAppId
        public CGroupList GetByAppId(int appId)
        {
            CGroupList temp = null;
            if (! IndexByAppId.TryGetValue(appId, out temp))
            {
                temp = new CGroupList();
                IndexByAppId[appId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CGroupList> _indexByAppId;
        private Dictionary<int, CGroupList> IndexByAppId
        {
            get
            {
                if (null == _indexByAppId)
                {
                    Dictionary<int, CGroupList> index = new Dictionary<int, CGroupList>();
                    CGroupList temp = null;
                    foreach (CGroup i in this)
                    {
                        if (! index.TryGetValue(i.GroupAppId, out temp))
                        {
                            temp = new CGroupList();
                            index[i.GroupAppId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByAppId = index;
                }
                return _indexByAppId;
            }
        }
        #endregion

    }
}
