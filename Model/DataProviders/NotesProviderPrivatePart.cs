﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Model.DataProviders
{
	public partial class NotesProvider
	{
		private static readonly NotesProvider provider = new NotesProvider();

		private readonly DataContractJsonSerializer jsonSerializer;
		private readonly object locker = new object();

		private const string FileName = "LocalDb";
		private const string FileExtention = "json";
		private const string FullName = FileName + "." + FileExtention;

		private List<CustomerNote> notes;

		private List<CustomerNote> ReadJson()
		{
			using var sf = new FileStream(FullName, FileMode.OpenOrCreate, FileAccess.Read);
			var notes = new List<CustomerNote>();

			lock (locker)
			{
				try
				{
					notes = jsonSerializer.ReadObject(sf) as List<CustomerNote>;
				}
				catch { }
			}

			return notes;
		}

		private void WriteJson(List<CustomerNote> notes)
		{
			using var sf = new FileStream(FullName, FileMode.OpenOrCreate, FileAccess.Write);

			lock (locker)
			{
				try
				{
					jsonSerializer.WriteObject(sf, notes);
				}
				catch
				{ }
			}
		}
	}
}
