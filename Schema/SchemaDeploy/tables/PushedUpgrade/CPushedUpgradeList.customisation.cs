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
	public partial class CPushedUpgradeList
	{
		#region Filters
		#endregion

		#region Aggregation
		#endregion

		#region Searching (Optional)
		//Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
		//e.g. public CPushedUpgradeList Search(string nameOrId, int instanceId, int oldVersionId, int newVersionId) { ...
		public CPushedUpgradeList Search(string nameOrId, int appId, int instanceId)
		{
			//1. Normalisation
			nameOrId = (nameOrId ?? string.Empty).Trim().ToLower();

			//2. Start with a complete list
			CPushedUpgradeList results = this;

			//3. Use any available index, such as those generated for fk/bool columns
			//Normal Case - non-unique index (e.g. foreign key)

			if (int.MinValue != instanceId)
				results = results.GetByInstanceId(instanceId);
			else if (int.MinValue != appId)
				results = results.GetByAppId(appId);

			//Special case - unique index (e.g. primary key)
			/*
            if (!string.IsNullOrEmpty(nameOrId)) 
            {
                int id;
                if (int.TryParse(nameOrId, out id))
                {
                    CPushedUpgrade obj = this.GetById(id);
                    if (null != obj)
                    {
                        results = new CPushedUpgradeList(1);
                        results.Add(obj);
                        return results;
                    }
                }
            }
            */

			//4. Exit early if remaining (non-index) filters are blank
			if (string.IsNullOrEmpty(nameOrId)) return results;

			//5. Manually search each record using custom match logic, building a shortlist
			CPushedUpgradeList shortList = new CPushedUpgradeList();
			foreach (CPushedUpgrade i in results)
				if (Match(nameOrId, i))
					shortList.Add(i);
			return shortList;
		}
		//Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
		private bool Match(string name, CPushedUpgrade obj)
		{
			if (!string.IsNullOrEmpty(name)) //Match any string column
			{
				if (null != obj.PushUserName && obj.PushUserName.ToLower().Contains(name)) return true;
				return false;   //If filter is active, reject any items that dont match
			}
			return true;    //No active filters (should catch this in step #4)
		}
		#endregion

		#region Cloning
		public CPushedUpgradeList Clone(CDataSrc target) //, int parentId)
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
					CPushedUpgradeList clone = Clone(target, tx); //, parentId);
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
		public CPushedUpgradeList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
		{
			CPushedUpgradeList list = new CPushedUpgradeList(this.Count);
			foreach (CPushedUpgrade i in this)
				list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
			return list;
		}
		#endregion

		#region Export to Csv
		//Web - Need to add a project reference to System.Web, or comment out these two methods
		public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "PushedUpgrades.csv"); }
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
			string[] headings = new string[] { "PushId", "PushInstanceId", "PushUserName", "PushOldVersionId", "PushOldSchemaMD5", "PushNewVersionId", "PushNewSchemaMD5", "PushStarted", "PushCompleted" };
			CDataSrc.ExportToCsv(headings, sw);
			foreach (CPushedUpgrade i in this)
			{
				object[] data = new object[] { i.PushId, i.PushInstanceId, i.PushUserName, i.PushOldVersionId, i.PushOldSchemaMD5, i.PushNewVersionId, i.PushNewSchemaMD5, i.PushStarted, i.PushCompleted };
				CDataSrc.ExportToCsv(data, sw);
			}
		}
		#endregion


		//Index by PushInstanceId
		public CPushedUpgradeList GetByAppId(int appId)
		{
			CPushedUpgradeList temp = null;
			if (!IndexByAppId.TryGetValue(appId, out temp))
			{
				temp = new CPushedUpgradeList();
				IndexByAppId[appId] = temp;
			}
			return temp;
		}

		[NonSerialized]
		private Dictionary<int, CPushedUpgradeList> _indexByAppId;
		private Dictionary<int, CPushedUpgradeList> IndexByAppId
		{
			get
			{
				if (null == _indexByAppId)
				{
					Dictionary<int, CPushedUpgradeList> index = new Dictionary<int, CPushedUpgradeList>();
					CPushedUpgradeList temp = null;
					foreach (CPushedUpgrade i in this)
					{
						if (!index.TryGetValue(i.Instance.InstanceAppId, out temp))
						{
							temp = new CPushedUpgradeList();
							index[i.Instance.InstanceAppId] = temp;
						}
						temp.Add(i);
					}
					_indexByAppId = index;
				}
				return _indexByAppId;
			}
		}
	}
}
