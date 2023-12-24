using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;
using System.IO;

namespace SchemaDeploy
{
	//Table-Mapping Class (Customisable half)
	public partial class CBinaryFile
	{
		#region Constants
		private static string JOIN_VERSIONFILE_OUTER = String.Concat(TABLE_NAME, " LEFT OUTER JOIN ", CVersionFile.TABLE_NAME, " ON VFBinaryMD5=MD5");
		private static string JOIN_VERSIONFILE_NOFILE = String.Concat(TABLE_NAME, " INNER JOIN ", CVersionFile.TABLE_NAME, " ON VFBinaryMD5=MD5");
		private static string JOIN_VERSIONFILE_WITH_FILE = String.Concat(VIEW_NAME, " INNER JOIN ", CVersionFile.TABLE_NAME, " ON VFBinaryMD5=MD5");
		#endregion

		#region Constructors (Public)
		//Default Connection String
		public CBinaryFile() : base() { }

		//Alternative Connection String
		public CBinaryFile(CDataSrc dataSrc) : base(dataSrc) { }

		//Hidden  (UI code should use cache instead)
		protected internal CBinaryFile(Guid mD5) : base(mD5) { }
		protected internal CBinaryFile(CDataSrc dataSrc, Guid mD5) : base(dataSrc, mD5) { }
		protected internal CBinaryFile(CDataSrc dataSrc, Guid mD5, IDbTransaction txOrNull) : base(dataSrc, mD5, txOrNull) { }
		#endregion

		#region Default Values
		protected override void InitValues_Custom()
		{
			_created = DateTime.Now;
		}
		#endregion

		#region Default Connection String
		protected override CDataSrc DefaultDataSrc() { return CDataSrc.Default; }
		#endregion

		#region Properties - Relationships
		//Relationships - Foriegn Keys (e.g parent)

		//Relationships - Collections (e.g. children)
		public CVersionFileList VersionFiles { get { return CVersionFile.Cache.GetByBinaryMD5(this.MD5); } }
		protected CVersionFileList VersionFiles_(IDbTransaction tx) { return new CVersionFile(DataSrc).SelectByBinaryMD5(this.MD5, tx); } //Only used for cascade deletes


		//Relationships - 2-Step Walk
		public CVersionList Versions { get { return VersionFiles.Versions; } }
		#endregion

		#region Properties - Customisation
		public string N { get { return VersionFiles.Count.ToString("n0"); } }
		public string Usage { get { return CUtilities.CountSummary(VersionFiles, "version", ""); } }

		protected byte[] Bin { get { return CBinary.Unzip(_binGz); } set { _binGz = (null == value) ? null : CBinary.Zip(value); } }
		public int Size { get { return _size; } }
		public string Size_ { get { return CUtilities.FileSize(Size); } }

		public byte[] FileAsBytes { get { return GetFile(); } set { SetFile(value); } }

		public void SetFile(string rootFolder)
		{
			SetFile(rootFolder, this.Path);
		}
		public void SetFile(string rootFolder, string path)
		{
			this.Path = path.Replace("\\", "/");
			SetFile(File.ReadAllBytes(string.Concat(rootFolder, "/", path)));
		}
		public void SetFile(byte[] b, string path)
		{
			this.Path = path.Replace("\\", "/");
			SetFile(b);
		}
		public void SetFile(byte[] b)
		{
			Bin = b;
			_size = b.Length;
			_mD5 = CBinary.MD5_(b);
		}
		public void SetFile(CFileNameAndContent fc)
		{
			Bin = fc.Content;
			_size = fc.Content.Length;
			_mD5 = fc.Md5;
			Path = fc.Name;
		}
		public void SetFile(CFileHash h)
		{
			_size = 0;
			_mD5 = h.MD5;
			Path = h.Name;
		}
		public void SetFile(CSchemaInfo s)
		{
			SetFile(CProto.Serialise(s));
			Path = s.ToString();
		}


		public byte[] GetFile() { return CBinary.Unzip(GetFileGz()); }
		public byte[] GetFileGz() { return SelectById_WithFile(this.MD5)._binGz; }

		//Derived/ReadOnly (e.g. xml classes, presentation logic)
		#endregion


		#region Save/Delete Overrides
		public override void Save(IDbTransaction txOrNull)
		{
			Path = Path.Replace("\\", "/");

			base.Save(txOrNull);

			_binGz = null;
		}
		public override void Delete(IDbTransaction txOrNull)
		{
			this.Bin = new byte[] { };
			this.Deleted = DateTime.Now;

			this.Save(txOrNull);
		}
		public void DeletePermanently()
		{
			base.Delete(null);
		}
		#endregion

		#region Custom Database Queries
		//(Not normally required for cached classes, use list class for searching etc)
		//For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
		//For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
		public CBinaryFile SelectById_WithFile(Guid md5)
		{
			var list = SelectWhere(new CCriteriaList("MD5", md5), TABLE_NAME);
			if (list.Count == 1)
				return list[0];
			return null;
		}
		public CBinaryFileList SelectByVersionId_WithFile(int versionId)
		{
			return SelectWhere(new CCriteriaList("VFVersionId", versionId), JOIN_VERSIONFILE_NOFILE);
		}
		public CBinaryFileList SelectByVersionId_WithoutFile(int versionId)
		{
			return SelectWhere(new CCriteriaList("VFVersionId", versionId), JOIN_VERSIONFILE_WITH_FILE);
		}
		public CBinaryFileList SelectByVersionId_WithoutFile(int versionId, IDbTransaction tx)
		{
			return SelectWhere(new CCriteriaList("VFVersionId", versionId), JOIN_VERSIONFILE_NOFILE, tx);
		}
		public CBinaryFileList SelectByAppId_WithoutFile(int appId, IDbTransaction tx)
		{
			return SelectWhere(new CCriteriaList("VersionAppId", appId), JOIN_VERSIONFILE_NOFILE, tx);
		}
		protected List<Guid> OrphanFileIds(IDbTransaction tx)
		{
			return DataSrc.MakeListGuid(new CSelectWhere("MD5", JOIN_VERSIONFILE_OUTER, new CCriteria("VFVersionId", int.MinValue), null, tx));
		}
		public int DeleteOrphans(IDbTransaction tx)
		{
			var ids = OrphanFileIds(tx);
			return DeleteWhere("MD5", ESign.IN, ids, tx);
		}
		#endregion

		#region Searching (Optional)
		//For cached classes, custom seach logic resides in static methods on the list class
		// e.g. CBinaryFile.Cache.Search("...")

		//See also the auto-generated methods based on indexes
		//' e.g. CBinaryFile.Cache.GetBy...
		#endregion

		#region Caching Details
		//Cache Key
		internal static string CACHE_KEY = typeof(CBinaryFile).ToString();    //TABLE_NAME

		//Cache data
		private static CBinaryFileList LoadCache() { return new CBinaryFile().SelectAll(); }
		//Cache Timeout
		private static void SetCache(CBinaryFileList value)
		{
			if (null != value)
				value.Sort();
			CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
		}
		//Helper Method
		private CBinaryFile CacheGetById(CBinaryFileList list) { return list.GetById(this.MD5); }
		#endregion

		#region Cloning
		public CBinaryFile Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
		{
			//Shallow copy: Copies the immediate record, excluding autogenerated Pks
			CBinaryFile copy = new CBinaryFile(this, target);

			//Deep Copy - Child Entities: Cloned children must reference their cloned parent
			//copy.SampleParentId = parentId;

			copy.Save(txOrNull);

			//Deep Copy - Parent Entities: Cloned parents also clone their child collections
			//this.Children.Clone(target, txOrNull, copy.MD5);

			return copy;
		}
		#endregion

		#region ToXml
		protected override void ToXml_Custom(System.Xml.XmlWriter w)
		{
			//Store(w, "Example", this.Example)
		}
		#endregion



		protected override CNameValueList ColumnNameValues()
		{
			CNameValueList data = new CNameValueList();
			data.Add("MD5", CAdoData.NullVal(_mD5));
			data.Add("Path", CAdoData.NullVal(_path));
			if (null != _binGz)
				data.Add("BinGz", CAdoData.NullVal(_binGz));
			data.Add("Size", CAdoData.NullVal(_size));
			data.Add("IsSchema", CAdoData.NullVal(_isSchema));
			data.Add("Created", CAdoData.NullVal(_created));
			data.Add("Deleted", CAdoData.NullVal(_deleted));
			return data;
		}
	}
}