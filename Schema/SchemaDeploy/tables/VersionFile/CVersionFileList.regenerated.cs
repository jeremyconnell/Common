using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CVersionFileList : List<CVersionFile>
    {
        #region Constructors
        //Basic constructor
        public CVersionFileList() : base() {}
       
        //More efficient memory-allocation if size is known
        public CVersionFileList(int capacity) : base(capacity) {}
        
        //Shares the index (if its already been computed)
        public CVersionFileList(CVersionFileList list) : base(list.Count)
        {
            foreach (CVersionFile i in list)
                base.Add(i);
        }

        //Generic list (eg. from paging control), have to assume type
        public CVersionFileList(IList list) : base(list.Count)
        {
            foreach (CVersionFile i in list)
                base.Add(i);
        }
        #endregion

        public int VersionId {  set { foreach (var i in this) i.VFVersionId = value; } }
        
        #region Top/Bottom/Page
        public CVersionFileList Top(int count)
        {
            if (count >= this.Count)
                return this;
            return Page(count, 0); 
        }
        public CVersionFileList Bottom(int count)   
        {
            if (count > this.Count)
                count = this.Count;
            return new CVersionFileList(this.GetRange(this.Count - count - 1, count));
        }
        public CVersionFileList Page(int pageSize, int pageIndex)
        {
            return new CVersionFileList( CUtilities.Page(this, pageSize, pageIndex) );
        }
        #endregion

        #region BulkEditLogic
        public bool HaveSameValue(string propertyName)               { return CReflection.HaveSameValue(this, propertyName); }
        public void SetSameValue( string propertyName, object value) {        CReflection.SetSameValue( this, propertyName, value); }
        #endregion

        #region SortBy
        //Public
        public CVersionFileList SortBy(string propertyName) { return SortBy(propertyName, false); }
        public CVersionFileList SortBy(string propertyName, bool descending)
        {
            CVersionFileList copy = new CVersionFileList(this);
            if (this.Count == 0)    return copy;
            copy.Sort(new CVersionFileList_SortBy(propertyName, descending, this));
            return copy;
        }
        //Private 
        private class CVersionFileList_SortBy : CReflection.GenericSortBy, IComparer<CVersionFile>
        {
            public CVersionFileList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
            public int Compare(CVersionFile x, CVersionFile y) { return base.Compare(x, y); }
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
        public void SaveAll(  IDbTransaction txOrNull)    {   foreach (CVersionFile i in this) {   i.Save(  txOrNull);   }   }
        public void DeleteAll(IDbTransaction txOrNull)    {   foreach (CVersionFile i in this) {   i.Delete(txOrNull);   }   }

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
                    foreach (CVersionFile i in this)
                        ids.Add(i.VFId);
                    _ids = ids;
                }
                return _ids;
            }
        }
        public CVersionFileList GetByIds(List<int> ids)
        {
            CVersionFileList list = new CVersionFileList(ids.Count);
            foreach (int id in ids)
                if (null != GetById(id))
                    list.Add(GetById(id));
            return list;
        }
        #endregion
        
        #region Cache-Control
    
        //Supplementary List Overloads
        public void Add(   IList<CVersionFile> itemsToAdd)    {   foreach (CVersionFile i in itemsToAdd)   { Add(   i); }   }
        public void Remove(IList<CVersionFile> itemsToRemove) {   foreach (CVersionFile i in itemsToRemove){ Remove(i); }   }
        #endregion
        
        #region Main Index (on VFId)
        public CVersionFile GetById(int vFId)
        {
            CVersionFile c = null;
            Index.TryGetValue(vFId, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<int,CVersionFile> _index;
        private Dictionary<int,CVersionFile> Index
        {
            get
            {
                if (null != _index)
                    if (_index.Count == this.Count)
                        return _index;

                    _index = new Dictionary<int,CVersionFile>(this.Count);
                        foreach (CVersionFile i in this) 
                            _index[i.VFId] = i;
                return _index;
            }
        }
        #endregion
            
        #region Foreign-Key Indices (Subsets)
        //Index by VFVersionId
        public CVersionFileList GetByVersionId(int versionId)
        {
            CVersionFileList temp = null;
            if (! IndexByVersionId.TryGetValue(versionId, out temp))
            {
                temp = new CVersionFileList();
                IndexByVersionId[versionId] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<int, CVersionFileList> _indexByVersionId;
        private Dictionary<int, CVersionFileList> IndexByVersionId
        {
            get
            {
                if (null == _indexByVersionId)
                {
                    Dictionary<int, CVersionFileList> index = new Dictionary<int, CVersionFileList>();
                    CVersionFileList temp = null;
                    foreach (CVersionFile i in this)
                    {
                        if (! index.TryGetValue(i.VFVersionId, out temp))
                        {
                            temp = new CVersionFileList();
                            index[i.VFVersionId] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByVersionId = index;
                }
                return _indexByVersionId;
            }
        }
        //Index by VFBinaryMD5
        public CVersionFileList GetByBinaryMD5(Guid binaryMD5)
        {
            CVersionFileList temp = null;
            if (! IndexByBinaryMD5.TryGetValue(binaryMD5, out temp))
            {
                temp = new CVersionFileList();
                IndexByBinaryMD5[binaryMD5] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<Guid, CVersionFileList> _indexByBinaryMD5;
        private Dictionary<Guid, CVersionFileList> IndexByBinaryMD5
        {
            get
            {
                if (null == _indexByBinaryMD5)
                {
                    Dictionary<Guid, CVersionFileList> index = new Dictionary<Guid, CVersionFileList>();
                    CVersionFileList temp = null;
                    foreach (CVersionFile i in this)
                    {
                        if (! index.TryGetValue(i.VFBinaryMD5, out temp))
                        {
                            temp = new CVersionFileList();
                            index[i.VFBinaryMD5] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByBinaryMD5 = index;
                }
                return _indexByBinaryMD5;
            }
        }
        #endregion

    }
}
