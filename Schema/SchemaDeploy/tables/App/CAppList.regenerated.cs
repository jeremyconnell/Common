using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CAppList : List<CApp>
    {
        #region Constructors
        //Basic constructor
        public CAppList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CAppList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CAppList(CAppList list) : base(list.Count)
        {
            foreach (CApp i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CAppList(IList list) : base(list.Count)
        {
            foreach (CApp i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CAppList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CAppList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CAppList(this.GetRange(this.Count - count - 1, count));
        }
        public CAppList Page(int pageSize, int pageIndex)
        {
            return new CAppList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CAppList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CAppList SortBy(string propertyName, bool descending)
        {
            CAppList copy = new CAppList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CAppList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CAppList_SortBy : CReflection.GenericSortBy, IComparer<CApp>
        {
            public CAppList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CApp x, CApp y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CApp i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CApp i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CApp i in this)
                        ids.Add(i.AppId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CAppList GetByIds(List<int> ids)
        {
            CAppList list = new CAppList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CApp item)
        {
            if (null != _index && ! _index.ContainsKey(item.AppId))
                _index[item.AppId] = item;
            base.Add(item);
        }
        public new void Remove(CApp item)
        {
            if (null != _index && _index.ContainsKey(item.AppId))
                _index.Remove(item.AppId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CApp> itemsToAdd)    {   foreach (CApp i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CApp> itemsToRemove) {   foreach (CApp i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on AppId)
        public CApp GetById(int appId)
        {
            CApp c = null;
            Index.TryGetValue(appId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CApp> _index;
        private Dictionary<int,CApp> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CApp>(this.Count);
                        foreach (CApp i in this) 
                            _index[i.AppId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by AppMainVersionId
        public CAppList GetByMainVersionId(int mainVersionId)
        {
            CAppList temp = null;
            if (! IndexByMainVersionId.TryGetValue(mainVersionId, out temp))
            {
                temp = new CAppList();
                IndexByMainVersionId[mainVersionId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CAppList> _indexByMainVersionId;
        private Dictionary<int, CAppList> IndexByMainVersionId
        {
            get
            {
                if (null == _indexByMainVersionId)
                {
                    Dictionary<int, CAppList> index = new Dictionary<int, CAppList>();
                    CAppList temp = null;
                    foreach (CApp i in this)
                    {
                        if (! index.TryGetValue(i.AppMainVersionId, out temp))
                        {
                            temp = new CAppList();
                            index[i.AppMainVersionId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByMainVersionId = index;
                }
                return _indexByMainVersionId;
            }
        }
        #endregion

    }
}
