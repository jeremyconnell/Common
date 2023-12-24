using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CUpgradeHistoryList : List<CUpgradeHistory>
    {
        #region Constructors
        //Basic constructor
        public CUpgradeHistoryList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CUpgradeHistoryList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CUpgradeHistoryList(CUpgradeHistoryList list) : base(list.Count)
        {
            foreach (CUpgradeHistory i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CUpgradeHistoryList(IList list) : base(list.Count)
        {
            foreach (CUpgradeHistory i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CUpgradeHistoryList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CUpgradeHistoryList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CUpgradeHistoryList(this.GetRange(this.Count - count - 1, count));
        }
        public CUpgradeHistoryList Page(int pageSize, int pageIndex)
        {
            return new CUpgradeHistoryList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CUpgradeHistoryList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CUpgradeHistoryList SortBy(string propertyName, bool descending)
        {
            CUpgradeHistoryList copy = new CUpgradeHistoryList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CUpgradeHistoryList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CUpgradeHistoryList_SortBy : CReflection.GenericSortBy, IComparer<CUpgradeHistory>
        {
            public CUpgradeHistoryList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CUpgradeHistory x, CUpgradeHistory y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CUpgradeHistory i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CUpgradeHistory i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CUpgradeHistory i in this)
                        ids.Add(i.ChangeId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CUpgradeHistoryList GetByIds(List<int> ids)
        {
            CUpgradeHistoryList list = new CUpgradeHistoryList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CUpgradeHistory item)
        {
            if (null != _index && ! _index.ContainsKey(item.ChangeId))
                _index[item.ChangeId] = item;
            base.Add(item);
        }
        public new void Remove(CUpgradeHistory item)
        {
            if (null != _index && _index.ContainsKey(item.ChangeId))
                _index.Remove(item.ChangeId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CUpgradeHistory> itemsToAdd)    {   foreach (CUpgradeHistory i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CUpgradeHistory> itemsToRemove) {   foreach (CUpgradeHistory i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on ChangeId)
        public CUpgradeHistory GetById(int changeId)
        {
            CUpgradeHistory c = null;
            Index.TryGetValue(changeId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CUpgradeHistory> _index;
        private Dictionary<int,CUpgradeHistory> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CUpgradeHistory>(this.Count);
                        foreach (CUpgradeHistory i in this) 
                            _index[i.ChangeId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by ChangeReportId
        public CUpgradeHistoryList GetByReportId(int reportId)
        {
            CUpgradeHistoryList temp = null;
            if (! IndexByReportId.TryGetValue(reportId, out temp))
            {
                temp = new CUpgradeHistoryList();
                IndexByReportId[reportId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CUpgradeHistoryList> _indexByReportId;
        private Dictionary<int, CUpgradeHistoryList> IndexByReportId
        {
            get
            {
                if (null == _indexByReportId)
                {
                    Dictionary<int, CUpgradeHistoryList> index = new Dictionary<int, CUpgradeHistoryList>();
                    CUpgradeHistoryList temp = null;
                    foreach (CUpgradeHistory i in this)
                    {
                        if (! index.TryGetValue(i.ChangeReportId, out temp))
                        {
                            temp = new CUpgradeHistoryList();
                            index[i.ChangeReportId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByReportId = index;
                }
                return _indexByReportId;
            }
        }
        //Index by ChangeNewVersionId
        public CUpgradeHistoryList GetByNewVersionId(int newVersionId)
        {
            CUpgradeHistoryList temp = null;
            if (! IndexByNewVersionId.TryGetValue(newVersionId, out temp))
            {
                temp = new CUpgradeHistoryList();
                IndexByNewVersionId[newVersionId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CUpgradeHistoryList> _indexByNewVersionId;
        private Dictionary<int, CUpgradeHistoryList> IndexByNewVersionId
        {
            get
            {
                if (null == _indexByNewVersionId)
                {
                    Dictionary<int, CUpgradeHistoryList> index = new Dictionary<int, CUpgradeHistoryList>();
                    CUpgradeHistoryList temp = null;
                    foreach (CUpgradeHistory i in this)
                    {
                        if (! index.TryGetValue(i.ChangeNewVersionId, out temp))
                        {
                            temp = new CUpgradeHistoryList();
                            index[i.ChangeNewVersionId] = temp;
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
