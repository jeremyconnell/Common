using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CVersionList : List<CVersion>
    {
        #region Constructors
        //Basic constructor
        public CVersionList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CVersionList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CVersionList(CVersionList list) : base(list.Count)
        {
            foreach (CVersion i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CVersionList(IList list) : base(list.Count)
        {
            foreach (CVersion i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CVersionList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CVersionList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CVersionList(this.GetRange(this.Count - count - 1, count));
        }
        public CVersionList Page(int pageSize, int pageIndex)
        {
            return new CVersionList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CVersionList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CVersionList SortBy(string propertyName, bool descending)
        {
            CVersionList copy = new CVersionList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CVersionList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CVersionList_SortBy : CReflection.GenericSortBy, IComparer<CVersion>
        {
            public CVersionList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CVersion x, CVersion y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CVersion i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CVersion i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CVersion i in this)
                        ids.Add(i.VersionId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CVersionList GetByIds(List<int> ids)
        {
            CVersionList list = new CVersionList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CVersion item)
        {
            if (null != _index && ! _index.ContainsKey(item.VersionId))
                _index[item.VersionId] = item;
            base.Add(item);
        }
        public new void Remove(CVersion item)
        {
            if (null != _index && _index.ContainsKey(item.VersionId))
                _index.Remove(item.VersionId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CVersion> itemsToAdd)    {   foreach (CVersion i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CVersion> itemsToRemove) {   foreach (CVersion i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on VersionId)
        public CVersion GetById(int versionId)
        {
            CVersion c = null;
            Index.TryGetValue(versionId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CVersion> _index;
        private Dictionary<int,CVersion> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CVersion>(this.Count);
                        foreach (CVersion i in this) 
                            _index[i.VersionId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by VersionAppId
        public CVersionList GetByAppId(int appId)
        {
            CVersionList temp = null;
            if (! IndexByAppId.TryGetValue(appId, out temp))
            {
                temp = new CVersionList();
                IndexByAppId[appId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CVersionList> _indexByAppId;
        private Dictionary<int, CVersionList> IndexByAppId
        {
            get
            {
                if (null == _indexByAppId)
                {
                    Dictionary<int, CVersionList> index = new Dictionary<int, CVersionList>();
                    CVersionList temp = null;
                    foreach (CVersion i in this)
                    {
                        if (! index.TryGetValue(i.VersionAppId, out temp))
                        {
                            temp = new CVersionList();
                            index[i.VersionAppId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByAppId = index;
                }
                return _indexByAppId;
            }
        }
        //Index by VersionFilesMD5
        public CVersionList GetByFilesMD5(Guid filesMD5)
        {
            CVersionList temp = null;
            if (! IndexByFilesMD5.TryGetValue(filesMD5, out temp))
            {
                temp = new CVersionList();
                IndexByFilesMD5[filesMD5] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<Guid, CVersionList> _indexByFilesMD5;
        private Dictionary<Guid, CVersionList> IndexByFilesMD5
        {
            get
            {
                if (null == _indexByFilesMD5)
                {
                    Dictionary<Guid, CVersionList> index = new Dictionary<Guid, CVersionList>();
                    CVersionList temp = null;
                    foreach (CVersion i in this)
                    {
                        if (! index.TryGetValue(i.VersionFilesMD5, out temp))
                        {
                            temp = new CVersionList();
                            index[i.VersionFilesMD5] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByFilesMD5 = index;
                }
                return _indexByFilesMD5;
            }
        }
        //Index by VersionSchemaMD5
        public CVersionList GetBySchemaMD5(Guid schemaMD5)
        {
            CVersionList temp = null;
            if (! IndexBySchemaMD5.TryGetValue(schemaMD5, out temp))
            {
                temp = new CVersionList();
                IndexBySchemaMD5[schemaMD5] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<Guid, CVersionList> _indexBySchemaMD5;
        private Dictionary<Guid, CVersionList> IndexBySchemaMD5
        {
            get
            {
                if (null == _indexBySchemaMD5)
                {
                    Dictionary<Guid, CVersionList> index = new Dictionary<Guid, CVersionList>();
                    CVersionList temp = null;
                    foreach (CVersion i in this)
                    {
                        if (! index.TryGetValue(i.VersionSchemaMD5, out temp))
                        {
                            temp = new CVersionList();
                            index[i.VersionSchemaMD5] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexBySchemaMD5 = index;
                }
                return _indexBySchemaMD5;
            }
        }
        #endregion

    }
}
