using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
	[Serializable()]
	public partial class CInstanceList : List<CInstance>
	{
		#region Constructors
		//Basic constructor
		public CInstanceList() : base() { }

		//More efficient memory-allocation if size is known
		public CInstanceList(int capacity) : base(capacity) { }

		//Shares the index (if its already been computed)
		public CInstanceList(CInstanceList list) : base(list.Count)
		{
			foreach (CInstance i in list)
				base.Add(i);
		}

		//Generic list (eg. from paging control), have to assume type
		public CInstanceList(IList list) : base(list.Count)
		{
			foreach (CInstance i in list)
				base.Add(i);
		}
		#endregion

		#region Top/Bottom/Page
		public CInstanceList Top(int count)
		{
			if (count >= this.Count)
				return this;
			return Page(count, 0);
		}
		public CInstanceList Bottom(int count)
		{
			if (count > this.Count)
				count = this.Count;
			return new CInstanceList(this.GetRange(this.Count - count - 1, count));
		}
		public CInstanceList Page(int pageSize, int pageIndex)
		{
			return new CInstanceList(CUtilities.Page(this, pageSize, pageIndex));
		}
		#endregion

		#region BulkEditLogic
		public bool HaveSameValue(string propertyName) { return CReflection.HaveSameValue(this, propertyName); }
		public void SetSameValue(string propertyName, object value) { CReflection.SetSameValue(this, propertyName, value); }
		#endregion

		#region SortBy
		//Public
		public CInstanceList SortBy(string propertyName) { return SortBy(propertyName, false); }
		public CInstanceList SortBy(string propertyName, bool descending)
		{
			CInstanceList copy = new CInstanceList(this);
			if (this.Count == 0) return copy;
			copy.Sort(new CInstanceList_SortBy(propertyName, descending, this));
			return copy;
		}
		//Private 
		private class CInstanceList_SortBy : CReflection.GenericSortBy, IComparer<CInstance>
		{
			public CInstanceList_SortBy(string propertyName, bool descending, IList list) : base(propertyName, descending, list) { }
			public int Compare(CInstance x, CInstance y) { return base.Compare(x, y); }
		}
		#endregion

		#region SaveAll/DeleteAll
		//Use default connection (may be overridden in base class)
		public void SaveAll() { if (this.Count > 0) { SaveAll(this[0].DataSrc); } }
		public void DeleteAll() { if (this.Count > 0) { DeleteAll(this[0].DataSrc); } }

		//Use connection supplied
		public void SaveAll(CDataSrc dataSrc) { dataSrc.BulkSave(this); }
		public void DeleteAll(CDataSrc dataSrc) { dataSrc.BulkDelete(this); }

		//Use transaction supplied
		public void SaveAll(IDbTransaction txOrNull) { foreach (CInstance i in this) { i.Save(txOrNull); } }
		public void DeleteAll(IDbTransaction txOrNull) { foreach (CInstance i in this) { i.Delete(txOrNull); } }

		//Use a specified isolation level
		public void SaveAll(IsolationLevel txIsolationLevel) { if (this.Count > 0) { SaveAll(this[0].DataSrc, txIsolationLevel); } }
		public void DeleteAll(IsolationLevel txIsolationLevel) { if (this.Count > 0) { DeleteAll(this[0].DataSrc, txIsolationLevel); } }

		//Use a specified connection and isolation level
		public void SaveAll(CDataSrc dataSrc, IsolationLevel txIsolationLevel) { dataSrc.BulkSave(this, txIsolationLevel); }
		public void DeleteAll(CDataSrc dataSrc, IsolationLevel txIsolationLevel) { dataSrc.BulkDelete(this, txIsolationLevel); }
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
					foreach (CInstance i in this)
						ids.Add(i.InstanceId);
					_ids = ids;
				}
				return _ids;
			}
		}
		public CInstanceList GetByIds(List<int> ids)
		{
			CInstanceList list = new CInstanceList(ids.Count);
			foreach (int id in ids)
				if (null != GetById(id))
					list.Add(GetById(id));
			return list;
		}
		#endregion

		#region Cache-Control
		//Main Logic

		//Supplementary List Overloads
		public void Add(IList<CInstance> itemsToAdd) { foreach (CInstance i in itemsToAdd) { Add(i); } }
		public void Remove(IList<CInstance> itemsToRemove) { foreach (CInstance i in itemsToRemove) { Remove(i); } }
		#endregion

		#region Main Index (on InstanceId)
		public CInstance GetById(int instanceId)
		{
			CInstance c = null;
			Index.TryGetValue(instanceId, out c);
			return c;
		}
		[NonSerialized]
		private Dictionary<int, CInstance> _index;
		private Dictionary<int, CInstance> Index
		{
			get
			{
				if (null != _index)
					if (_index.Count == this.Count)
						return _index;

				_index = new Dictionary<int, CInstance>(this.Count);
				foreach (CInstance i in this)
					_index[i.InstanceId] = i;
				return _index;
			}
		}
		#endregion

		#region Foreign-Key Indices (Subsets)
		//Index by InstanceClientId
		public CInstanceList GetByClientId(int clientId)
		{
			CInstanceList temp = null;
			if (!IndexByClientId.TryGetValue(clientId, out temp))
			{
				temp = new CInstanceList();
				IndexByClientId[clientId] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<int, CInstanceList> _indexByClientId;
		private Dictionary<int, CInstanceList> IndexByClientId
		{
			get
			{
				if (null == _indexByClientId)
				{
					Dictionary<int, CInstanceList> index = new Dictionary<int, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceClientId, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceClientId] = temp;
						}
						temp.Add(i);
					}
					_indexByClientId = index;
				}
				return _indexByClientId;
			}
		}
		//Index by InstanceClientName
		public CInstanceList GetByClientName(string clientName)
		{
			CInstanceList temp = null;
			if (!IndexByClientName.TryGetValue(clientName, out temp))
			{
				temp = new CInstanceList();
				IndexByClientName[clientName] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<string, CInstanceList> _indexByClientName;
		private Dictionary<string, CInstanceList> IndexByClientName
		{
			get
			{
				if (null == _indexByClientName)
				{
					Dictionary<string, CInstanceList> index = new Dictionary<string, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceClientName, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceClientName] = temp;
						}
						temp.Add(i);
					}
					_indexByClientName = index;
				}
				return _indexByClientName;
			}
		}
		//Index by InstanceClientCode
		public CInstanceList GetByClientCode(string clientCode)
		{
			CInstanceList temp = null;
			if (!IndexByClientCode.TryGetValue(clientCode, out temp))
			{
				temp = new CInstanceList();
				IndexByClientCode[clientCode] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<string, CInstanceList> _indexByClientCode;
		private Dictionary<string, CInstanceList> IndexByClientCode
		{
			get
			{
				if (null == _indexByClientCode)
				{
					Dictionary<string, CInstanceList> index = new Dictionary<string, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceClientCode, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceClientCode] = temp;
						}
						temp.Add(i);
					}
					_indexByClientCode = index;
				}
				return _indexByClientCode;
			}
		}
		//Index by InstanceSuffix
		public CInstanceList GetBySuffix(string suffix)
		{
			CInstanceList temp = null;
			if (!IndexBySuffix.TryGetValue(suffix, out temp))
			{
				temp = new CInstanceList();
				IndexBySuffix[suffix] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<string, CInstanceList> _indexBySuffix;
		private Dictionary<string, CInstanceList> IndexBySuffix
		{
			get
			{
				if (null == _indexBySuffix)
				{
					Dictionary<string, CInstanceList> index = new Dictionary<string, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceSuffix, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceSuffix] = temp;
						}
						temp.Add(i);
					}
					_indexBySuffix = index;
				}
				return _indexBySuffix;
			}
		}
		//Index by InstanceAppId
		public CInstanceList GetByAppId(int appId)
		{
			CInstanceList temp = null;
			if (!IndexByAppId.TryGetValue(appId, out temp))
			{
				temp = new CInstanceList();
				IndexByAppId[appId] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<int, CInstanceList> _indexByAppId;
		private Dictionary<int, CInstanceList> IndexByAppId
		{
			get
			{
				if (null == _indexByAppId)
				{
					Dictionary<int, CInstanceList> index = new Dictionary<int, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceAppId, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceAppId] = temp;
						}
						temp.Add(i);
					}
					_indexByAppId = index;
				}
				return _indexByAppId;
			}
		}
		//Index by InstanceSpecialVersionId
		public CInstanceList GetBySpecialVersionId(int specialVersionId)
		{
			CInstanceList temp = null;
			if (!IndexBySpecialVersionId.TryGetValue(specialVersionId, out temp))
			{
				temp = new CInstanceList();
				IndexBySpecialVersionId[specialVersionId] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<int, CInstanceList> _indexBySpecialVersionId;
		private Dictionary<int, CInstanceList> IndexBySpecialVersionId
		{
			get
			{
				if (null == _indexBySpecialVersionId)
				{
					Dictionary<int, CInstanceList> index = new Dictionary<int, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceSpecialVersionId, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceSpecialVersionId] = temp;
						}
						temp.Add(i);
					}
					_indexBySpecialVersionId = index;
				}
				return _indexBySpecialVersionId;
			}
		}
		//Index by InstanceWebNameAzure
		public CInstanceList GetByWebNameAzure(string webNameAzure)
		{
			CInstanceList temp = null;
			if (!IndexByWebNameAzure.TryGetValue(webNameAzure, out temp))
			{
				temp = new CInstanceList();
				IndexByWebNameAzure[webNameAzure] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<string, CInstanceList> _indexByWebNameAzure;
		private Dictionary<string, CInstanceList> IndexByWebNameAzure
		{
			get
			{
				if (null == _indexByWebNameAzure)
				{
					Dictionary<string, CInstanceList> index = new Dictionary<string, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceWebNameAzure, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceWebNameAzure] = temp;
						}
						temp.Add(i);
					}
					_indexByWebNameAzure = index;
				}
				return _indexByWebNameAzure;
			}
		}
		//Index by InstanceWebHostName
		public CInstanceList GetByWebHostName(string webHostName)
		{
			CInstanceList temp = null;
			if (!IndexByWebHostName.TryGetValue(webHostName, out temp))
			{
				temp = new CInstanceList();
				IndexByWebHostName[webHostName] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<string, CInstanceList> _indexByWebHostName;
		private Dictionary<string, CInstanceList> IndexByWebHostName
		{
			get
			{
				if (null == _indexByWebHostName)
				{
					Dictionary<string, CInstanceList> index = new Dictionary<string, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceWebHostName, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceWebHostName] = temp;
						}
						temp.Add(i);
					}
					_indexByWebHostName = index;
				}
				return _indexByWebHostName;
			}
		}
		//Index by InstanceWebUseSsl
		public CInstanceList GetByWebUseSsl(bool webUseSsl)
		{
			CInstanceList temp = null;
			if (!IndexByWebUseSsl.TryGetValue(webUseSsl, out temp))
			{
				temp = new CInstanceList();
				IndexByWebUseSsl[webUseSsl] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<bool, CInstanceList> _indexByWebUseSsl;
		private Dictionary<bool, CInstanceList> IndexByWebUseSsl
		{
			get
			{
				if (null == _indexByWebUseSsl)
				{
					Dictionary<bool, CInstanceList> index = new Dictionary<bool, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceWebUseSsl, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceWebUseSsl] = temp;
						}
						temp.Add(i);
					}
					_indexByWebUseSsl = index;
				}
				return _indexByWebUseSsl;
			}
		}
		//Index by InstanceDbNameAzure
		public CInstanceList GetByDbNameAzure(string dbNameAzure)
		{
			CInstanceList temp = null;
			if (!IndexByDbNameAzure.TryGetValue(dbNameAzure, out temp))
			{
				temp = new CInstanceList();
				IndexByDbNameAzure[dbNameAzure] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<string, CInstanceList> _indexByDbNameAzure;
		private Dictionary<string, CInstanceList> IndexByDbNameAzure
		{
			get
			{
				if (null == _indexByDbNameAzure)
				{
					Dictionary<string, CInstanceList> index = new Dictionary<string, CInstanceList>();
					CInstanceList temp = null;
					foreach (CInstance i in this)
					{
						if (!index.TryGetValue(i.InstanceDbNameAzure, out temp))
						{
							temp = new CInstanceList();
							index[i.InstanceDbNameAzure] = temp;
						}
						temp.Add(i);
					}
					_indexByDbNameAzure = index;
				}
				return _indexByDbNameAzure;
			}
		}
		#endregion

	}
}
