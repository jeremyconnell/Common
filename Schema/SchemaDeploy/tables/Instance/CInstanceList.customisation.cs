using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;

using Framework;

namespace SchemaDeploy
{
    //Collection Class (Customisable half)
    public partial class CInstanceList
    {
        #region Filters
        public CInstanceList MainBranch {  get { return GetByIsBranch(false); } }
        public CInstanceList SideBranch { get { return GetByIsBranch(true); } }
        #endregion

        #region Aggregation
        public string Names_ {  get { return CUtilities.ListToString(Names); } }
        public List<string> Names
        {
            get
            {
                var list = new List<string>(this.Count);
                foreach (var i in this)
                    list.Add(CUtilities.Truncate(i.InstanceName));
                return list;
            }
        }
        #endregion

        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CInstanceList Search(string nameOrId, int appId, int clientId, int branchVersionId) { ...
        public CInstanceList Search(string nameOrId, int appId, int clientId)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CInstanceList results = this;
            
            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            if (int.MinValue != appId) results = results.GetByAppId(appId);
            if (int.MinValue != clientId) results = results.GetByClientId(clientId);
            //if (int.MinValue != branchVersionId) results = results.GetBySpecialVersionId(branchVersionId);

            //Special case - unique index (e.g. primary key)
            if (!string.IsNullOrEmpty(nameOrId)) 
            {
                int id;
                if (int.TryParse(nameOrId, out id))
                {
                    CInstance obj = this.GetById(id);
                    if (null != obj)
                    {
                        results = new CInstanceList(1);
                        results.Add(obj);
                        return results;
                    }
                }
            }
            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CInstanceList shortList = new CInstanceList();
            foreach (CInstance i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CInstance obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                if (null != obj.InstanceName && obj.InstanceName.ToLower().Contains(name))   return true;
                if (null != obj.InstanceSpecialVersionName && obj.InstanceSpecialVersionName.ToLower().Contains(name))   return true;
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CInstanceList Clone(CDataSrc target) //, int parentId)
        {
            //No Transaction
            if (target is CDataSrcRemote)
                return Clone(target, null); //, parentId);

            //Transaction
            using (IDbConnection cn = target.Local.Connection())
            {
                IDbTransaction tx = cn.BeginTransaction();
                try
                {
                    CInstanceList clone = Clone(target, tx); //, parentId);
                    tx.Commit();
                    return clone;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
        public CInstanceList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CInstanceList list = new CInstanceList(this.Count);
            foreach (CInstance i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "Instances.csv"); }
        public void ExportToCsv(HttpResponse response, string fileName)
        {
            CDataSrc.ExportToCsv(response, fileName); //Standard response headers
            StreamWriter sw = new StreamWriter(response.OutputStream);
            ExportToCsv(sw);
            sw.Flush();
            response.End();
        }

        //Non-web
        public void ExportToCsv(string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath);
            ExportToCsv(sw);
            sw.Close();
        }

        //Logic
        protected void ExportToCsv(StreamWriter sw)
        {
            string[] headings = new string[] {"InstanceId", "InstanceAppId", "InstanceClientId", "InstanceName", "InstanceSpecialVersionId", "InstanceSpecialVersionName", "InstanceCreated"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CInstance i in this)
            {
                object[] data = new object[] {i.InstanceId, i.InstanceAppId, i.InstanceClientId, i.InstanceName, i.InstanceSpecialVersionId, i.InstanceSpecialVersionName, i.InstanceCreated};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion


        //Main Logic
        public new void Add(CInstance item)
        {
            if (null != _index && !_index.ContainsKey(item.InstanceId))
                _index[item.InstanceId] = item;

            _indexByName = null;
            _indexByIsBranch = null;
            _indexByClientId = null;
            _indexBySpecialVersionId = null;
            _indexByAppId = null;

            base.Add(item);
        }
        public new void Remove(CInstance item)
        {
            if (null != _index && _index.ContainsKey(item.InstanceId))
                _index.Remove(item.InstanceId);

            _indexByName = null;
            _indexByIsBranch = null;
            _indexByClientId = null;
            _indexBySpecialVersionId = null;
            _indexByAppId = null;

            base.Remove(item);
        }


        #region Unique Index (on InstanceName)
        public CInstance GetByName(string instanceName)
        {
            instanceName = instanceName.ToLower().Trim();
            CInstance c = null;
            IndexByName.TryGetValue(instanceName, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<string, CInstance> _indexByName;
        private Dictionary<string, CInstance> IndexByName
        {
            get
            {
                if (null != _indexByName)
                    if (_indexByName.Count == this.Count)
                        return _indexByName;

                _indexByName = new Dictionary<string, CInstance>(this.Count);
                foreach (CInstance i in this)
                    try
                    {
                        var name = i.InstanceName.ToLower().Trim();
                        _indexByName[name] = i;
                    }
                    catch { };
                return _indexByName;
            }
        }
        #endregion

        #region Unique Index (on InstanceHost)
        public CInstance GetByHost(string host)
        {
            host = host.ToLower().Trim();
            CInstance c = null;
            IndexByHost.TryGetValue(host, out c);
            return c;
        }
        [NonSerialized]
        private Dictionary<string, CInstance> _indexByHost;
        private Dictionary<string, CInstance> IndexByHost
        {
            get
            {
                if (null != _indexByHost)
                    if (_indexByHost.Count == this.Count)
                        return _indexByHost;

                _indexByHost = new Dictionary<string, CInstance>(this.Count);
                foreach (CInstance i in this)
                    try
                    {
                        var name = i.InstanceWebHostName.ToLower().Trim();
                        _indexByHost[name] = i;
                    }
                    catch { };
                return _indexByHost;
            }
        }
        #endregion

        //Index by Use-Custom-Branch
        public CInstanceList GetByIsBranch(bool isBranch)
        {
            CInstanceList temp = null;
            if (!IndexByIsBranch.TryGetValue(isBranch, out temp))
            {
                temp = new CInstanceList(0);
                IndexByIsBranch[isBranch] = temp;
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<bool, CInstanceList> _indexByIsBranch;
        private Dictionary<bool, CInstanceList> IndexByIsBranch
        {
            get
            {
                if (null == _indexByIsBranch)
                {
                    Dictionary<bool, CInstanceList> index = new Dictionary<bool, CInstanceList>();
                    CInstanceList temp = null;
                    foreach (CInstance i in this)
                    {
                        if (!index.TryGetValue(i.IsBranch, out temp))
                        {
                            temp = new CInstanceList();
                            index[i.IsBranch] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByIsBranch = index;
                }
                return _indexByIsBranch;
            }
        }
    }
}
