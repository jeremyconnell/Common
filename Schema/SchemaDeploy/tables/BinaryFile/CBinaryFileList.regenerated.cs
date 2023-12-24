using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CBinaryFileList : List<CBinaryFile>
    {
        #region Constructors
        //Basic constructor
        public CBinaryFileList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CBinaryFileList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CBinaryFileList(CBinaryFileList list) : base(list.Count)
        {
            foreach (CBinaryFile i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CBinaryFileList(IList list) : base(list.Count)
        {
            foreach (CBinaryFile i in list)
                base.Add(i);
        }
        #endregion
        
        #region Top/Bottom/Page
        public CBinaryFileList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CBinaryFileList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CBinaryFileList(this.GetRange(this.Count - count - 1, count));
        }
        public CBinaryFileList Page(int pageSize, int pageIndex)
        {
            return new CBinaryFileList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CBinaryFileList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CBinaryFileList SortBy(string propertyName, bool descending)
        {
            CBinaryFileList copy = new CBinaryFileList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CBinaryFileList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CBinaryFileList_SortBy : CReflection.GenericSortBy, IComparer<CBinaryFile>
        {
            public CBinaryFileList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CBinaryFile x, CBinaryFile y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CBinaryFile i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CBinaryFile i in this) {   i.Delete(txOrNull);   }   }

        //Use a specified isolation level
        public void SaveAll(  IsolationLevel txIsolationLevel)   {   if (this.Count > 0) { SaveAll(  this[0].DataSrc, txIsolationLevel);  }    }
        public void DeleteAll(IsolationLevel txIsolationLevel)   {   if (this.Count > 0) { DeleteAll(this[0].DataSrc, txIsolationLevel);  }    }

        //Use a specified connection and isolation level
        public void SaveAll(  CDataSrc dataSrc, IsolationLevel txIsolationLevel)   {   dataSrc.BulkSave(  this, txIsolationLevel);  }
        public void DeleteAll(CDataSrc dataSrc, IsolationLevel txIsolationLevel)   {   dataSrc.BulkDelete(this, txIsolationLevel);  }
        #endregion

        #region List of Ids
        List<Guid> _ids;
        public List<Guid> Ids
        {
            get
            {
                if (null == _ids)
                {
                    List<Guid> ids = new List<Guid>(this.Count);
                    foreach (CBinaryFile i in this)
                        ids.Add(i.MD5);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CBinaryFileList GetByIds(List<Guid> ids)
        {
            CBinaryFileList list = new CBinaryFileList(ids.Count);
            foreach (Guid id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
        //Main Logic
        public  void Add_(CBinaryFile item)
        {
            if (null != _index && ! _index.ContainsKey(item.MD5))
                _index[item.MD5] = item;

            _indexByIsSchema = null;

            base.Add(item);
        }
        public  void Remove_(CBinaryFile item)
        {
            if (null != _index && _index.ContainsKey(item.MD5))
                _index.Remove(item.MD5);

            _indexByIsSchema = null;

            base.Remove(item);
        }
    
        //Supplementary List Overloads
        public void Add(   IList<CBinaryFile> itemsToAdd)    {   foreach (CBinaryFile i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CBinaryFile> itemsToRemove) {   foreach (CBinaryFile i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on MD5)
        public CBinaryFile GetById(Guid mD5)
        {
            CBinaryFile c = null;
            Index.TryGetValue(mD5, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<Guid,CBinaryFile> _index;
        private Dictionary<Guid,CBinaryFile> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<Guid,CBinaryFile>(this.Count);
                        foreach (CBinaryFile i in this) 
                            _index[i.MD5] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by IsSchema
        public CBinaryFileList GetByIsSchema(bool isSchema)
        {
            CBinaryFileList temp = null;
            if (! IndexByIsSchema.TryGetValue(isSchema, out temp))
            {
                temp = new CBinaryFileList();
                IndexByIsSchema[isSchema] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<bool, CBinaryFileList> _indexByIsSchema;
        private Dictionary<bool, CBinaryFileList> IndexByIsSchema
        {
            get
            {
                if (null == _indexByIsSchema)
                {
                    Dictionary<bool, CBinaryFileList> index = new Dictionary<bool, CBinaryFileList>();
                    CBinaryFileList temp = null;
                    foreach (CBinaryFile i in this)
                    {
                        if (! index.TryGetValue(i.IsSchema, out temp))
                        {
                            temp = new CBinaryFileList();
                            index[i.IsSchema] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByIsSchema = index;
                }
                return _indexByIsSchema;
            }
        }
        #endregion

    }
}
