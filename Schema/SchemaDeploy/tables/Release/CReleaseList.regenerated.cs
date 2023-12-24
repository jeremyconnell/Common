using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CReleaseList : List<CRelease>
    {
        #region Constructors
        //Basic constructor
        public CReleaseList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CReleaseList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CReleaseList(CReleaseList list) : base(list.Count)
        {
            foreach (CRelease i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CReleaseList(IList list) : base(list.Count)
        {
            foreach (CRelease i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CReleaseList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CReleaseList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CReleaseList(this.GetRange(this.Count - count - 1, count));
        }
        public CReleaseList Page(int pageSize, int pageIndex)
        {
            return new CReleaseList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CReleaseList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CReleaseList SortBy(string propertyName, bool descending)
        {
            CReleaseList copy = new CReleaseList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CReleaseList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CReleaseList_SortBy : CReflection.GenericSortBy, IComparer<CRelease>
        {
            public CReleaseList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CRelease x, CRelease y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CRelease i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CRelease i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CRelease i in this)
                        ids.Add(i.ReleaseId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CReleaseList GetByIds(List<int> ids)
        {
            CReleaseList list = new CReleaseList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CRelease item)
        {
            if (null != _index && ! _index.ContainsKey(item.ReleaseId))
                _index[item.ReleaseId] = item;
            base.Add(item);
        }
        public new void Remove(CRelease item)
        {
            if (null != _index && _index.ContainsKey(item.ReleaseId))
                _index.Remove(item.ReleaseId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CRelease> itemsToAdd)    {   foreach (CRelease i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CRelease> itemsToRemove) {   foreach (CRelease i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on ReleaseId)
        public CRelease GetById(int releaseId)
        {
            CRelease c = null;
            Index.TryGetValue(releaseId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CRelease> _index;
        private Dictionary<int,CRelease> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CRelease>(this.Count);
                        foreach (CRelease i in this) 
                            _index[i.ReleaseId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by ReleaseAppId
        public CReleaseList GetByAppId(int appId)
        {
            CReleaseList temp = null;
            if (! IndexByAppId.TryGetValue(appId, out temp))
            {
                temp = new CReleaseList();
                IndexByAppId[appId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CReleaseList> _indexByAppId;
        private Dictionary<int, CReleaseList> IndexByAppId
        {
            get
            {
                if (null == _indexByAppId)
                {
                    Dictionary<int, CReleaseList> index = new Dictionary<int, CReleaseList>();
                    CReleaseList temp = null;
                    foreach (CRelease i in this)
                    {
                        if (! index.TryGetValue(i.ReleaseAppId, out temp))
                        {
                            temp = new CReleaseList();
                            index[i.ReleaseAppId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByAppId = index;
                }
                return _indexByAppId;
            }
        }
        //Index by ReleaseInstanceId
        public CReleaseList GetByInstanceId(int instanceId)
        {
            CReleaseList temp = null;
            if (! IndexByInstanceId.TryGetValue(instanceId, out temp))
            {
                temp = new CReleaseList();
                IndexByInstanceId[instanceId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CReleaseList> _indexByInstanceId;
        private Dictionary<int, CReleaseList> IndexByInstanceId
        {
            get
            {
                if (null == _indexByInstanceId)
                {
                    Dictionary<int, CReleaseList> index = new Dictionary<int, CReleaseList>();
                    CReleaseList temp = null;
                    foreach (CRelease i in this)
                    {
                        if (! index.TryGetValue(i.ReleaseInstanceId, out temp))
                        {
                            temp = new CReleaseList();
                            index[i.ReleaseInstanceId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByInstanceId = index;
                }
                return _indexByInstanceId;
            }
        }
        //Index by ReleaseVersionId
        public CReleaseList GetByVersionIds(params int[] versionIds)
        {
            var temp = new CReleaseList();
            foreach (var i in versionIds)
                temp.AddRange(GetByVersionId(i));
            return temp;
        }
        public CReleaseList GetByVersionId(int versionId)
        {
            CReleaseList temp = null;
            if (! IndexByVersionId.TryGetValue(versionId, out temp))
            {
                temp = new CReleaseList();
                IndexByVersionId[versionId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CReleaseList> _indexByVersionId;
        private Dictionary<int, CReleaseList> IndexByVersionId
        {
            get
            {
                if (null == _indexByVersionId)
                {
                    Dictionary<int, CReleaseList> index = new Dictionary<int, CReleaseList>();
                    CReleaseList temp = null;
                    foreach (CRelease i in this)
                    {
                        if (! index.TryGetValue(i.ReleaseVersionId, out temp))
                        {
                            temp = new CReleaseList();
                            index[i.ReleaseVersionId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByVersionId = index;
                }
                return _indexByVersionId;
            }
        }
        #endregion

    }
}
