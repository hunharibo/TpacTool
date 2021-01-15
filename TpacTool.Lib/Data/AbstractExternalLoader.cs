﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TpacTool.Lib
{
	public abstract class AbstractExternalLoader
	{
		protected static readonly IDictionary<object, object> EMPTY_USERDATA = new Dictionary<object, object>();

		internal protected FileInfo _file;

		internal protected ulong _offset;

		internal protected ulong _actualSize;

		internal protected ulong _storageSize;

		internal protected StorageFormat _storageFormat;

		internal protected ulong _unknownUlong;

		internal protected uint _unknownUint;

		public Guid TypeGuid { internal set; get; }

		public Guid OwnerGuid { set; get; }

		protected Lazy<Dictionary<object, object>> _userdata = new Lazy<Dictionary<object, object>>();

		public IDictionary<object, object> UserData
		{
			get { return _userdata.Value; }
		}

		public abstract bool IsDataLoaded();

		internal protected abstract void ForceLoad();

		internal protected abstract void ForceLoad(BinaryReader fullStream);

		internal protected byte[] SaveTo(out ulong actualSize, out ulong storageSize, out StorageFormat storageType)
		{
			var memStream = new MemoryStream();
			using (var stream = new BinaryWriter(memStream, Encoding.UTF8))
			{
				SaveTo(stream, out actualSize, out storageSize, out storageType);
				stream.Flush();
				return memStream.ToArray();
			}
		}

		internal protected abstract void SaveTo(BinaryWriter stream, 
			out ulong actualSize, out ulong storageSize, out StorageFormat storageType);

		public abstract void MarkLongLive();

		public virtual bool IsLargeData
		{
			get => _actualSize > 100 * 1024 * 1024;
		}

		public enum StorageFormat : byte
		{
			Uncompressed = 0,
			LZ4HC = 1
		}
	}
}