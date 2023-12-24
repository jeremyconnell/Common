using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CFormatList : List<CFormat>
    {
        #region Constructors
        //Basic constructor
        public CFormatList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CFormatList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CFormatList(CFormatList list) : base(list.Count)
        {
            foreach (CFormat i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CFormatList(IList list) : base(list.Count)
        {
            foreach (CFormat i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CFormatList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CFormatList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CFormatList(this.GetRange(this.Count - count - 1, count));
        }
        public CFormatList Page(int pageSize, int pageIndex)
        {
            return new CFormatList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CFormatList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CFormatList SortBy(string propertyName, bool descending)
        {
            CFormatList copy = new CFormatList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CFormatList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CFormatList_SortBy : CReflection.GenericSortBy, IComparer<CFormat>
        {
            public CFormatList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CFormat x, CFormat y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CFormat i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CFormat i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CFormat i in this)
                        ids.Add(i.FormatId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CFormatList GetByIds(List<int> ids)
        {
            CFormatList list = new CFormatList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public new void Add(CFormat item)
        {
            if (null != _index && ! _index.ContainsKey(item.FormatId))
                _index[item.FormatId] = item;
            base.Add(item);
        }
        public new void Remove(CFormat item)
        {
            if (null != _index && _index.ContainsKey(item.FormatId))
                _index.Remove(item.FormatId);
            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CFormat> itemsToAdd)    {   foreach (CFormat i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CFormat> itemsToRemove) {   foreach (CFormat i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on FormatId)
        public CFormat GetById(int formatId)
        {
            CFormat c = null;
            Index.TryGetValue(formatId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CFormat> _index;
        private Dictionary<int,CFormat> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CFormat>(this.Count);
                        foreach (CFormat i in this) 
                            _index[i.FormatId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        #endregion

    }
}
