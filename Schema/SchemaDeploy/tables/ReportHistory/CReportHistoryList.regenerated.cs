using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CReportHistoryList : List<CReportHistory>
    {
        #region Constructors
        //Basic constructor
        public CReportHistoryList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CReportHistoryList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CReportHistoryList(CReportHistoryList list) : base(list.Count)
        {
            foreach (CReportHistory i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CReportHistoryList(IList list) : base(list.Count)
        {
            foreach (CReportHistory i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CReportHistoryList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CReportHistoryList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CReportHistoryList(this.GetRange(this.Count - count - 1, count));
        }
        public CReportHistoryList Page(int pageSize, int pageIndex)
        {
            return new CReportHistoryList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CReportHistoryList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CReportHistoryList SortBy(string propertyName, bool descending)
        {
            CReportHistoryList copy = new CReportHistoryList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CReportHistoryList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CReportHistoryList_SortBy : CReflection.GenericSortBy, IComparer<CReportHistory>
        {
            public CReportHistoryList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CReportHistory x, CReportHistory y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CReportHistory i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CReportHistory i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CReportHistory i in this)
                        ids.Add(i.ReportId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CReportHistoryList GetByIds(List<int> ids)
        {
            CReportHistoryList list = new CReportHistoryList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CReportHistory item)
        {
            if (null != _index && ! _index.ContainsKey(item.ReportId))
                _index[item.ReportId] = item;
            base.Add(item);
        }
        public new void Remove(CReportHistory item)
        {
            if (null != _index && _index.ContainsKey(item.ReportId))
                _index.Remove(item.ReportId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CReportHistory> itemsToAdd)    {   foreach (CReportHistory i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CReportHistory> itemsToRemove) {   foreach (CReportHistory i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on ReportId)
        public CReportHistory GetById(int reportId)
        {
            CReportHistory c = null;
            Index.TryGetValue(reportId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CReportHistory> _index;
        private Dictionary<int,CReportHistory> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CReportHistory>(this.Count);
                        foreach (CReportHistory i in this) 
                            _index[i.ReportId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by ReportInstanceId
        public CReportHistoryList GetByInstanceId(int instanceId)
        {
            CReportHistoryList temp = null;
            if (! IndexByInstanceId.TryGetValue(instanceId, out temp))
            {
                temp = new CReportHistoryList();
                IndexByInstanceId[instanceId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CReportHistoryList> _indexByInstanceId;
        private Dictionary<int, CReportHistoryList> IndexByInstanceId
        {
            get
            {
                if (null == _indexByInstanceId)
                {
                    Dictionary<int, CReportHistoryList> index = new Dictionary<int, CReportHistoryList>();
                    CReportHistoryList temp = null;
                    foreach (CReportHistory i in this)
                    {
                        if (! index.TryGetValue(i.ReportInstanceId, out temp))
                        {
                            temp = new CReportHistoryList();
                            index[i.ReportInstanceId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByInstanceId = index;
                }
                return _indexByInstanceId;
            }
        }
        //Index by ReportInitialVersionId
        public CReportHistoryList GetByInitialVersionId(int initialVersionId)
        {
            CReportHistoryList temp = null;
            if (! IndexByInitialVersionId.TryGetValue(initialVersionId, out temp))
            {
                temp = new CReportHistoryList();
                IndexByInitialVersionId[initialVersionId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CReportHistoryList> _indexByInitialVersionId;
        private Dictionary<int, CReportHistoryList> IndexByInitialVersionId
        {
            get
            {
                if (null == _indexByInitialVersionId)
                {
                    Dictionary<int, CReportHistoryList> index = new Dictionary<int, CReportHistoryList>();
                    CReportHistoryList temp = null;
                    foreach (CReportHistory i in this)
                    {
                        if (! index.TryGetValue(i.ReportInitialVersionId, out temp))
                        {
                            temp = new CReportHistoryList();
                            index[i.ReportInitialVersionId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByInitialVersionId = index;
                }
                return _indexByInitialVersionId;
            }
        }
        #endregion

    }
}
